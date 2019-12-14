﻿using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events;
using Teamjob.Services.Identity.Exceptions;

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
            var refreshToken = await _tokenRepository.GetAsync(x => x.Token == InCommand.Token);

            if (refreshToken is null || refreshToken.UserId != InCommand.Id)
            {
                await _busPublisher.PublishAsync(new RevokeRefreshTokenRejected(refreshToken.UserId,
                    $"Refresh token [{InCommand.Token}] to revoke with User ID [{InCommand.Id}] was not found."));
                _logger.LogError($"Refresh token [{InCommand.Token}] to revoke with User ID [{InCommand.Id}] was not found.");

                throw new TeamJobException("Codes.RefreshTokenNotFound",
                    $"Refresh token [{InCommand.Token}] to revoke with User ID [{InCommand.Id}] was not found.");
            }

            refreshToken.Revoke();
            await _tokenRepository.UpdateAsync(refreshToken);

            _logger.LogInformation($"Refresh token fo User with ID [{InCommand.Id}] has been revoked.");
            await _busPublisher.PublishAsync(new RefreshTokenRevoked(refreshToken.UserId));
        }
    }
}
