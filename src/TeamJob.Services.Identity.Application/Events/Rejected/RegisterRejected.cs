using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace TeamJob.Services.Identity.Application.Events.Rejected
{
    public class RegisterRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public RegisterRejected(string email, string reason, string code)
        {
            Email  = email;
            Reason = reason;
            Code   = code;
        }
    }
}
