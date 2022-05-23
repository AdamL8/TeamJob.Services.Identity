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
using TeamJob.Services.Identity.Application.Exceptions;

namespace TeamJob.Services.Identity.Application.Events.External.Handlers
{
    public class ResetPasswordHandler : IEventHandler<ResetPassword>
    {
        private readonly IUserRepository                _userRepository;
        private readonly IMessageBroker                 _messageBroker;
        private readonly ILogger<ResetPasswordHandler>  _logger;
        private readonly IPasswordService               _passwordService;

        public ResetPasswordHandler( IUserRepository                userRepository,
                                     IMessageBroker                 messageBroker,
                                     ILogger<ResetPasswordHandler>  logger,
                                     IPasswordService               passwordService)
        {
            _userRepository  = userRepository;
            _messageBroker   = messageBroker;
            _logger          = logger;
            _passwordService = passwordService;
        }

        public async Task HandleAsync(ResetPassword @event)
        {
            var userId = @event.UserId;

            var user = await _userRepository.GetAsync(userId);

            if (user is null)
            {
                _logger.LogError($"Cannot find User with ID : [{userId}] because it doesn't exist");
                throw new UserNotFoundException(userId);
            }

            var password = _passwordService.Hash(@event.NewPassword);
            user.SetNewPassword(password);
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation($"User with ID [{userId}] had his password updated");
        }
    }
}
