using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Microsoft.Extensions.Logging;
using TeamJob.Services.Identity.Application.Services;
using TeamJob.Services.Identity.Core.Entities;
using TeamJob.Services.Identity.Core.Repositories;

namespace TeamJob.Services.Identity.Application.Events.External.Handlers
{
    public class ProfileDeletedHandler : IEventHandler<ProfileDeleted>
    {
        private readonly IRefreshTokenRepository        _tokenRepository;
        private readonly IUserRepository                _userRepository;
        private readonly IMessageBroker                 _messageBroker;
        private readonly ILogger<ProfileDeletedHandler> _logger;

        public ProfileDeletedHandler(IUserRepository                userRepository,
                                     IRefreshTokenRepository        tokenRepository,
                                     IMessageBroker                 messageBroker,
                                     ILogger<ProfileDeletedHandler> logger)
        {
            _userRepository  = userRepository;
            _tokenRepository = tokenRepository;
            _messageBroker   = messageBroker;
            _logger          = logger;
        }

        public async Task HandleAsync(ProfileDeleted @event)
        {
            var userId = @event.Id;

            var user = await _userRepository.GetAsync(userId);
            if (user is null)
            {
                _logger.LogError($"Cannot delete User with ID : [{userId}] because it doesn't exist");
                return;
            }

            var token = await _tokenRepository.GetAsync(userId);

            if (token != null)
            {
                await _tokenRepository.DeleteAsync(token.Id);
                _logger.LogInformation($"Refresh Token with ID [{token.Id}] associated with User with ID [{userId}] was DELETED");
            }

            await _userRepository.DeleteAsync(userId);
            _logger.LogInformation($"User with ID [{user.Id}] was DELETED");

            await _messageBroker.PublishAsync(new UserDeleted(user.Id, user.Email, user.Role.ToString()));
        }
    }
}
