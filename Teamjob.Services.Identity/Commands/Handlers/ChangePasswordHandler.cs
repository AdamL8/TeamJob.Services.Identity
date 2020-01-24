using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Commands.Handlers
{
    public class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly IMongoRepository<User, Guid>   _userRepository;
        private readonly IPasswordHasher<User>          _passwordHasher;
        private readonly IBusPublisher                  _busPublisher;
        private readonly ILogger<ChangePasswordHandler> _logger;

        public ChangePasswordHandler(IMongoRepository<User,Guid>    InUserRepository,
                                     IPasswordHasher<User>          InPasswordHasher,
                                     IBusPublisher                  InBusPublisher,
                                     ILogger<ChangePasswordHandler> InLogger)
        {
            _userRepository = InUserRepository;
            _passwordHasher = InPasswordHasher;
            _busPublisher   = InBusPublisher;
            _logger         = InLogger;
        }

        public async Task HandleAsync(ChangePassword InCommand)
        {
            var user = await _userRepository.GetAsync(InCommand.Id);
            if (user is null)
            {
                _logger.LogError($"Cannot change password of User with ID : [{InCommand.Id}] because it doesn't exist");
                throw new TeamJobException("Codes.UserNotFound",
                    $"User with id: '{InCommand.Id}' was not found.");
            }

            if (user.ValidatePassword(InCommand.CurrentPassword, _passwordHasher) == false)
            {
                _logger.LogError($"Invalid password supplied for User with ID : [{InCommand.Id}].");
                throw new TeamJobException("Codes.InvalidCurrentPassword",
                    "Invalid current password.");
            }

            user = new User(user.Id, user.Email, user.Role, user.CreatedAt);
            user.SetPassword(InCommand.NewPassword, _passwordHasher);

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Changed password of User with ID [{InCommand.Id}].");
            await _busPublisher.PublishAsync(new PasswordChanged(InCommand.Id));
        }
    }
}
