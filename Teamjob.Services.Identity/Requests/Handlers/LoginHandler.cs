using Convey.Auth;
using Convey.CQRS.Commands;
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

namespace Teamjob.Services.Identity.Requests
{
    public class LoginHandler : IRequestHandler<Login, LoginInfo>
    {
        private readonly IMongoRepository<User, Guid>         _userRepository;
        private readonly IMongoRepository<RefreshToken, Guid> _refreshTokenRepository;
        private readonly IPasswordHasher<User>                _passwordHasher;
        private readonly IJwtHandler                          _jwtHandler;
        private readonly IBusPublisher                        _busPublisher;
        private readonly ILogger<LoginHandler>                _logger;

        public LoginHandler(IMongoRepository<User, Guid>         InUserRepository,
                            IMongoRepository<RefreshToken, Guid> InRefreshTokenRepository,
                            IPasswordHasher<User>                InPasswordHasher,
                            IJwtHandler                          InJwtHandler,
                            IBusPublisher                        InBusPublisher,
                            ILogger<LoginHandler>                InLogger)
        {
            _userRepository         = InUserRepository;
            _refreshTokenRepository = InRefreshTokenRepository;
            _passwordHasher         = InPasswordHasher;
            _jwtHandler             = InJwtHandler;
            _busPublisher           = InBusPublisher;
            _logger                 = InLogger;
        }

        public async Task<LoginInfo> HandleAsync(Login InRequest)
        {
            var user = await _userRepository.GetAsync(x => x.Email == InRequest.Email);
            if (user is null || user.ValidatePassword(InRequest.Password, _passwordHasher) == false)
            {
                await _busPublisher.PublishAsync(new LoginRejected(InRequest.Email, "Invalid credentials"));
                _logger.LogInformation($"Login for email : [{InRequest.Email}] rejected with reason : Invalid credentials");

                throw new TeamJobException("Codes.InvalidCredentials",
                                           "Invalid credentials.");
            }

            var refreshToken = new RefreshToken(user, _passwordHasher);
            var jwt          = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role.ToString());
            jwt.RefreshToken = refreshToken.Token;

            await _refreshTokenRepository.AddAsync(refreshToken);

            _logger.LogInformation($"Logged in User with ID : {user.Id} and Email : {InRequest.Email}. JWT : {jwt.AccessToken}");
            await _busPublisher.PublishAsync(new LogedIn(user.Id));

            return new LoginInfo { Id = user.Id, AccessToken = jwt.AccessToken, Role = user.Role.ToString() };
        }
    }
}
