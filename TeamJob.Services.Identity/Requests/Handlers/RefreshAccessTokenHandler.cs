using Convey.Auth;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Convey.WebApi.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events;
using Teamjob.Services.Identity.Exceptions;
using TeamJob.Services.Identity.Exceptions;

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

            var userId         = refreshToken.UserId;
            var refreshTokenId = refreshToken.Id;

            if (refreshToken.IsRevoked)
            {
                //await _busPublisher.PublishAsync(new RefreshAccessTokenRejected(user.Id, $"Refresh token: '{refreshToken.Id}' was revoked."));

                throw new RefreshTokenAlreadyRevokedException(refreshTokenId);
            }

            var user = await _userRepository.GetAsync(userId);

            if (user is null)
            {
                await _busPublisher.PublishAsync(new RefreshAccessTokenRejected(userId,
                    $"User: '{userId}' referenced by the refresh token was not found."));
                _logger.LogError($"User: '{userId}' referenced by the refresh token was not found.");

                throw new UserNotFoundException(userId);
            }

            var jwt          = _jwtHandler.CreateToken(userId.ToString("N"), user.Role.ToString());
            jwt.RefreshToken = refreshToken.Token;

            await _busPublisher.PublishAsync(new AccessTokenRefreshed(userId));

            return jwt.AccessToken;
        }
    }
}
