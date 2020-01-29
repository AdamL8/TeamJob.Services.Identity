using Convey.CQRS.Events;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class RegisterRejected : IEvent
    {
        public Guid Id       { get; }
        public string Email  { get; }
        public string Reason { get; }

        public RegisterRejected(Guid id, string email, string reason)
        {
            Id     = id;
            Email  = email;
            Reason = reason;
        }
    }
}
