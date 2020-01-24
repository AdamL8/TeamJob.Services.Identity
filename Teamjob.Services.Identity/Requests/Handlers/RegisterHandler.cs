using Convey.Auth;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.DTO;
using Teamjob.Services.Identity.Events;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Requests.Handlers
{
    public class RegisterHandler : IRequestHandler<Register, LoginInfo>
    {
        private readonly IMongoRepository<User, Guid>  _userRepository;
        private readonly IPasswordHasher<User>         _passwordHasher;
        private readonly IJwtHandler                   _jwtHandler;
        private readonly IBusPublisher                 _busPublisher;
        private readonly ILogger<RegisterHandler>      _logger;

        public RegisterHandler(IMongoRepository<User, Guid> InUserRepository,
                               IPasswordHasher<User>        InPasswordHasher,
                               IJwtHandler                  InJwtHandler,
                               IBusPublisher                InBusPublisher,
                               ILogger<RegisterHandler>     InLogger)
        {
            _userRepository = InUserRepository;
            _passwordHasher = InPasswordHasher;
            _busPublisher   = InBusPublisher;
            _logger         = InLogger;
            _jwtHandler     = InJwtHandler;
        }

        public async Task<LoginInfo> HandleAsync(Register InCommand)
        {
            var user = await _userRepository.GetAsync(x => x.Email == InCommand.Email);
            if (user != null)
            {
                await _busPublisher.PublishAsync(new RegisterRejected(user.Id, InCommand.Email, "Email already in use"));
                _logger.LogError($"Registration rejected. User with ID : [{user.Id}] already uses this email address");

                throw new TeamJobException("Codes.EmailInUse",
                    $"Email: '{InCommand.Email}' is already in use.");
            }

            var role = Role.User;

            if (Enum.TryParse(InCommand.Role, out Role result))
            {
                role = result;
            }

            user = new User(Guid.NewGuid(), InCommand.Email, role);
            user.SetPassword(InCommand.Password, _passwordHasher);

            var refreshToken = new RefreshToken(user, _passwordHasher);
            var jwt          = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role.ToString());
            jwt.RefreshToken = refreshToken.Token;

            await _userRepository.AddAsync(user);
            _logger.LogInformation($"New User registered. ID : [{user.Id}] | Email : [{InCommand.Email}]");

            await _busPublisher.PublishAsync(new Registered(user.Id, InCommand.Email, user.Role.ToString()));

            return new LoginInfo { Id = user.Id, AccessToken = jwt.AccessToken, Role = user.Role.ToString() };
        }
    }
}
