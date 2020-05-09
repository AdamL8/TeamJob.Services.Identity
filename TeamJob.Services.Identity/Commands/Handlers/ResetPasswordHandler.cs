using Convey.CQRS.Commands;
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
    public class ResetPasswordHandler : ICommandHandler<ResetPassword>
    {
        private readonly IMongoRepository<User, Guid>   _userRepository;
        private readonly IBusPublisher                  _busPublisher;
        private readonly ILogger<ResetPasswordHandler>  _logger;

        public ResetPasswordHandler(IMongoRepository<User,Guid>   InUserRepository,
                                    IBusPublisher                 InBusPublisher,
                                    ILogger<ResetPasswordHandler> InLogger)
        {
            _userRepository = InUserRepository;
            _busPublisher   = InBusPublisher;
            _logger         = InLogger;
        }

        public async Task HandleAsync(ResetPassword InCommand)
        {
            var userEmail = InCommand.Email;

            var user = await _userRepository.GetAsync(x => x.Email.Equals(InCommand.Email));
            if (user is null)
            {
                _logger.LogError($"Cannot reset password of User with Email : [{userEmail}] because it doesn't exist");
                throw new UserNotFoundException(userEmail);
            }

            var userId     = user.Id;
            var expiryDate = DateTimeOffset.UtcNow.AddDays(1);

            _logger.LogInformation($"Request to create new temporary password for User with ID : [{user.Id}] made. Expires at [{expiryDate.DateTime}]");

            await _busPublisher.PublishAsync(new ForgotPassword(userId, userEmail, InCommand.Lang, expiryDate.ToUnixTimeSeconds()));
        }
    }
}
