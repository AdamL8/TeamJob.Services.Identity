using Convey.CQRS.Events;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class RegisterRejected : IEvent
    {
        public Guid UserId   { get; }
        public string Email  { get; }
        public string Reason { get; }

        public RegisterRejected(Guid userId, string email, string reason)
        {
            UserId = userId;
            Email  = email;
            Reason = reason;
        }
    }
}
