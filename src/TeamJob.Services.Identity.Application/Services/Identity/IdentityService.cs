using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamJob.Services.Identity.Application.Commands;
using TeamJob.Services.Identity.Application.DTO;
using TeamJob.Services.Identity.Application.Events;
using TeamJob.Services.Identity.Core.Entities;
using TeamJob.Services.Identity.Core.Exceptions;
using TeamJob.Services.Identity.Core.Repositories;

namespace TeamJob.Services.Identity.Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly IUserRepository          _userRepository;
        private readonly IPasswordService         _passwordService;
        private readonly IJwtProvider             _jwtProvider;
        private readonly IRefreshTokenService     _refreshTokenService;
        private readonly IMessageBroker           _messageBroker;
        private readonly IDateTimeProvider        _dateTimeProvider;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IUserRepository          userRepository,
                               IPasswordService         passwordService,
                               IJwtProvider             jwtProvider,
                               IRefreshTokenService     refreshTokenService,
                               IMessageBroker           messageBroker,
                               IDateTimeProvider        dateTimeProvider,
                               ILogger<IdentityService> logger)
        {
            _userRepository      = userRepository;
            _passwordService     = passwordService;
            _jwtProvider         = jwtProvider;
            _refreshTokenService = refreshTokenService;
            _messageBroker       = messageBroker;
            _logger              = logger;
            _dateTimeProvider    = dateTimeProvider;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return user is null
                ? null
                : new UserDto(user);
        }

        public async Task<AuthDto> LoginAsync(Login command)
        {
            if (EmailRegex.IsMatch(command.Email) == false)
            {
                _logger.LogError($"Invalid email: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is null)
            {
                _logger.LogError($"User with email: {command.Email} was not found.");
                throw new InvalidCredentialsException(command.Email);
            }

            if (_passwordService.IsValid(user.Password, command.Password) == false)
            {
                _logger.LogError($"Invalid password for user with id: {user.Id.Value}");
                throw new InvalidCredentialsException(command.Email);
            }

            var claims = user.Permissions.Any()
                ? new Dictionary<string, IEnumerable<string>>
                {
                    ["permissions"] = user.Permissions
                }
                : null;

            var auth = _jwtProvider.Create(user.Id, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id);

            _logger.LogInformation($"User with id: {user.Id} has been authenticated.");
            await _messageBroker.PublishAsync(new LogedIn(user.Id, user.Role));

            return auth;
        }

        public async Task<UserDto> RegisterAsync(Register command)
        {
            if (EmailRegex.IsMatch(command.Email) == false)
            {
                _logger.LogError($"Invalid email: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is { })
            {
                _logger.LogError($"Email already in use: {command.Email}");
                throw new EmailInUseException(command.Email);
            }

            var role = string.IsNullOrWhiteSpace(command.Role)
                ? "user"
                : command.Role.ToLowerInvariant();

            var password = _passwordService.Hash(command.Password);
            user = new User(command.UserId, command.Email, password, role, _dateTimeProvider.Now, command.Permissions);
            await _userRepository.AddAsync(user);

            _logger.LogInformation($"Created an account for the user with id: {user.Id}.");
            await _messageBroker.PublishAsync(new Registered(user.Id, user.Email, user.Role));

            return new UserDto(user);
        }
    }
}
