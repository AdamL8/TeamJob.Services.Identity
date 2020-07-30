using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Newtonsoft.Json;

namespace TeamJob.Services.Identity.Application.Events.External
{
    [Message(exchange: "password_reset", external: true)]
    public class ResetPassword : IEvent
    {
        public Guid UserId          { get; }
        public string NewPassword   { get; }


        [JsonConstructor]
        public ResetPassword(Guid userId, string newPassword)
        {
            UserId = userId;
            NewPassword = newPassword;
        }
    }
}
