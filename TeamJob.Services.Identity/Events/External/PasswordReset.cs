using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events.External
{
    [Message(exchange: "emailpasswordreset", external: true)]
    public class PasswordReset : IEvent
    {
        public Guid Id            { get; }
        public string Email       { get; }
        public string Lang        { get; }
        public string NewPassword { get; }

        [JsonConstructor]
        public PasswordReset(Guid id, string email, string lang, string newPassword)
        {
            Id          = id;
            Email       = email;
            Lang        = lang;
            NewPassword = newPassword;
        }
    }
}
