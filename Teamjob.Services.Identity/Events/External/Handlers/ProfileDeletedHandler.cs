﻿using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Events.External.Handlers
{
    public class ProfileDeletedHandler : IEventHandler<ProfileDeleted>
    {
        private readonly IMongoRepository<User, Guid>         _userRepository;
        private readonly IMongoRepository<RefreshToken, Guid> _tokenRepository;
        private readonly IBusPublisher                        _busPublisher;
        private readonly ILogger<ProfileDeletedHandler>       _logger;

        public ProfileDeletedHandler(IMongoRepository<User, Guid>         InUserRepository,
                                     IMongoRepository<RefreshToken, Guid> InTokenRepository,
                                     IBusPublisher                        InBusPublisher,
                                     ILogger<ProfileDeletedHandler>       InLogger)
        {
            _userRepository  = InUserRepository;
            _tokenRepository = InTokenRepository;
            _busPublisher    = InBusPublisher;
            _logger          = InLogger;
        }

        public async Task HandleAsync(ProfileDeleted InEvent)
        {
            var user = await _userRepository.GetAsync(InEvent.Id);
            if (user is null)
            {
                _logger.LogInformation($"Cannot delete User with ID : [{InEvent.Id}] because it doesn't exist");
                return;
            }

            var token = await _tokenRepository.GetAsync(x => x.UserId == user.Id);

            if (token != null)
            {
                await _tokenRepository.DeleteAsync(token.Id);
                _logger.LogInformation($"Refresh Token with ID [{token.Id}] associated with User with ID [{user.Id}] DELETED");
            }

            await _userRepository.DeleteAsync(InEvent.Id);
            _logger.LogInformation($"User with ID [{user.Id}] DELETED");

            await _busPublisher.PublishAsync(new UserDeleted(user.Id, user.Email, user.Role.ToString()));
        }
    }
}
