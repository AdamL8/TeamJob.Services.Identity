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
    public class RegisterHandler : ICommandHandler<Register>
    {
        private readonly IMongoRepository<User, Guid>  _userRepository;
        private readonly IPasswordHasher<User>         _passwordHasher;
        private readonly IBusPublisher                 _busPublisher;
        private readonly ILogger<RegisterHandler>      _logger;

        public RegisterHandler(IMongoRepository<User, Guid> InUserRepository,
                               IPasswordHasher<User>        InPasswordHasher,
                               IBusPublisher                InBusPublisher,
                               ILogger<RegisterHandler>     InLogger)
        {
            _userRepository = InUserRepository;
            _passwordHasher = InPasswordHasher;
            _busPublisher   = InBusPublisher;
            _logger         = InLogger;
        }

        public async Task HandleAsync(Register InCommand)
        {
            var user = await _userRepository.GetAsync(x => x.Email == InCommand.Email);
            if (user != null)
            {
                await _busPublisher.PublishAsync(new RegisterRejected(user.Id, InCommand.Email, "Email already in use"));
                _logger.LogInformation($"Registration rejected. User with ID : [{user.Id}] already uses this email address");

                throw new TeamJobException("Codes.EmailInUse",
                    $"Email: '{InCommand.Email}' is already in use.");
            }

            var role = Role.User;

            if (Enum.TryParse(typeof(Role), InCommand.Role, out object result))
            {
                role = (Role)result;
            }

            user = new User(Guid.NewGuid(), InCommand.Email, role);
            user.SetPassword(InCommand.Password, _passwordHasher);

            await _userRepository.AddAsync(user);
            _logger.LogInformation($"New User registered. ID : [{user.Id}] | Email : [{InCommand.Email}]");

            await _busPublisher.PublishAsync(new Registered(user.Id, InCommand.Email, user.Role.ToString()));
        }
    }
}
