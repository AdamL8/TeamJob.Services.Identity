using System;
using System.Collections.Generic;
using System.Text;
using Convey.MessageBrokers.RabbitMQ;
using TeamJob.Services.Identity.Application.Commands;
using TeamJob.Services.Identity.Application.Events.Rejected;
using TeamJob.Services.Identity.Core.Exceptions;

namespace TeamJob.Services.Identity.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch

            {
                EmailInUseException ex => new RegisterRejected(ex.Email, ex.Message, ex.Code),
                InvalidCredentialsException ex => new LoginRejected(ex.Email, ex.Message, ex.Code),
                InvalidEmailException ex => message switch
                {
                    Login command => new LoginRejected(command.Email, ex.Message, ex.Code),
                    Register command => new RegisterRejected(command.Email, ex.Message, ex.Code),
                    _ => null
                },
                _ => null
            };
    }
}
