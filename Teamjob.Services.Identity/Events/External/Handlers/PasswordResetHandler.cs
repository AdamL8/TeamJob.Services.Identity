using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Events.External.Handlers
{
    public class PasswordResetHandler : IEventHandler<PasswordReset>
    {
        private readonly IMongoRepository<User, Guid>        _userRepository;
        private readonly IBusPublisher                       _busPublisher;
        private readonly IPasswordHasher<User>               _passwordHasher;
        private readonly ILogger<PasswordResetHandler>       _logger;

        public PasswordResetHandler(IMongoRepository<User, Guid>  InUserRepository,
                                    IBusPublisher                 InBusPublisher,
                                    IPasswordHasher<User>         InPasswordHasher,
                                    ILogger<PasswordResetHandler> InLogger)
        {
            _userRepository  = InUserRepository;
            _busPublisher    = InBusPublisher;
            _passwordHasher  = InPasswordHasher;
            _logger          = InLogger;
        }

        public async Task HandleAsync(PasswordReset InEvent)
        {
            var user = await _userRepository.GetAsync(InEvent.Id);
            if (user is null)
            {
                _logger.LogInformation($"Cannot delete User with ID : [{InEvent.Id}] because it doesn't exist");
                return;
            }

            // TODO: Possibly check against the expiry date if this service is the one storing the temporary passwords

            user.SetPassword(InEvent.NewPassword, _passwordHasher);

            _logger.LogInformation($"Successfully reset the password of User with ID : [{user.Id}]");

            await _userRepository.UpdateAsync(user);
            await _busPublisher.PublishAsync(new PasswordChanged(user.Id));
        }
    }
}
