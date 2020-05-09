using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events;
using Teamjob.Services.Identity.Exceptions;

namespace TeamJob.Services.Identity.Commands.Handlers
{
    public class DeleteUserHandler : ICommandHandler<DeleteUser>
    {
        private readonly IMongoRepository<User, Guid>  _userRepository;
        private readonly IBusPublisher                 _busPublisher;
        private readonly ILogger<DeleteUserHandler>    _logger;

        public DeleteUserHandler(IMongoRepository<User, Guid> InUserRepository,
                                 IBusPublisher                InBusPublisher,
                                 ILogger<DeleteUserHandler>   InLogger)
        {
            _userRepository = InUserRepository;
            _busPublisher   = InBusPublisher;
            _logger         = InLogger;
        }

        public async Task HandleAsync(DeleteUser InCommand)
        {
            var userId = InCommand.Id;

            var user = await _userRepository.GetAsync(userId);
            if (user is null)
            {
                _logger.LogError($"Could not delete user with ID [{userId}] because it does not exist");
                throw new UserNotFoundException(userId);
            }

            await _userRepository.DeleteAsync(userId);
            _logger.LogInformation($"User with ID [{userId}] was DELETED");

            await _busPublisher.PublishAsync(new UserDeleted(user.Id, user.Email, user.Role.ToString()));
        }
    }
}
