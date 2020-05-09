using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events;
using TeamJob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Commands.Handlers
{
    public class RevokeRefreshTokenHandler : ICommandHandler<RevokeRefreshToken>
    {
        private readonly IMongoRepository<RefreshToken, Guid> _tokenRepository;
        private readonly IBusPublisher                        _busPublisher;
        private readonly ILogger<RevokeRefreshTokenHandler>   _logger;

        public RevokeRefreshTokenHandler(IMongoRepository<RefreshToken,Guid> InTokenRepository,
                                         IBusPublisher                       InBusPublisher,
                                         ILogger<RevokeRefreshTokenHandler>  InLogger)
        {
            _tokenRepository = InTokenRepository;
            _busPublisher    = InBusPublisher;
            _logger          = InLogger;
        }

        public async Task HandleAsync(RevokeRefreshToken InCommand)
        {
            var token  = InCommand.Token;
            var userId = InCommand.Id;

            var refreshToken = await _tokenRepository.GetAsync(x => x.Token == token);

            if (refreshToken is null || refreshToken.UserId != userId)
            {
                await _busPublisher.PublishAsync(new RevokeRefreshTokenRejected(userId,
                    $"Refresh token [{token}] to revoke with User ID [{userId}] was not found."));
                _logger.LogError($"Refresh token [{token}] to revoke with User ID [{userId}] was not found.");

                throw new RefreshTokenNotFoundException(token, userId);
            }

            refreshToken.Revoke();
            await _tokenRepository.UpdateAsync(refreshToken);

            _logger.LogInformation($"Refresh token [{token}] for User with ID [{userId}] has been revoked.");
            await _busPublisher.PublishAsync(new RefreshTokenRevoked(userId));
        }
    }
}
