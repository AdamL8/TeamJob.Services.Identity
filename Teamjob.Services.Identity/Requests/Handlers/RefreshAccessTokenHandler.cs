using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Convey.WebApi.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Requests.Handlers
{
    public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessToken, string>
    {
        private readonly IMongoRepository<RefreshToken, Guid> _tokenRepository;
        private readonly IMongoRepository<User, Guid>         _userRepository;
        private readonly IBusPublisher                        _busPublisher;
        private readonly IJwtHandler                          _jwtHandler;
        private readonly ILogger<RefreshAccessTokenHandler>   _logger;

        public RefreshAccessTokenHandler(IMongoRepository<RefreshToken, Guid> InTokenRepository,
                                         IMongoRepository<User, Guid>         InUserRepository,
                                         IBusPublisher                        InBusPublisher,
                                         IJwtHandler                          InJwtHandler,
                                         ILogger<RefreshAccessTokenHandler>   InLogger)
        {
            _tokenRepository = InTokenRepository;
            _userRepository  = InUserRepository;
            _busPublisher    = InBusPublisher;
            _jwtHandler      = InJwtHandler;
            _logger          = InLogger;
        }


        public async Task<string> HandleAsync(RefreshAccessToken InRequest)
        {
            var refreshToken = await _tokenRepository.GetAsync(x => x.Token == InRequest.Token);

            if (refreshToken is null)
            {
                //await _busPublisher.PublishAsync(new RefreshAccessTokenRejected(user.Id, "Refresh token was not found"));

                throw new TeamJobException("Codes.RefreshTokenNotFound",
                    "Refresh token was not found.");
            }

            if (refreshToken.IsRevoked)
            {
                //await _busPublisher.PublishAsync(new RefreshAccessTokenRejected(user.Id, $"Refresh token: '{refreshToken.Id}' was revoked."));

                throw new TeamJobException("Codes.RefreshTokenAlreadyRevoked",
                    $"Refresh token: '{refreshToken.Id}' was revoked.");
            }

            var user = await _userRepository.GetAsync(refreshToken.UserId);

            if (user is null)
            {
                await _busPublisher.PublishAsync(new RefreshAccessTokenRejected(user.Id,
                    $"User: '{refreshToken.UserId}' referenced by the refresh token was not found."));
                _logger.LogInformation($"User: '{refreshToken.UserId}' referenced by the refresh token was not found.");

                throw new TeamJobException("Codes.UserNotFound",
                    $"User: '{refreshToken.UserId}' was not found.");
            }

            var jwt          = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role.ToString());
            jwt.RefreshToken = refreshToken.Token;

            await _busPublisher.PublishAsync(new AccessTokenRefreshed(user.Id));

            return jwt.AccessToken;
        }
    }
}
