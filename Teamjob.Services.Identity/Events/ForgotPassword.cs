using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Events
{
    public class ForgotPassword : IEvent
    {
        public Guid Id       { get; }
        public string Email  { get; }
        public string Lang   { get; }
        public long ExpireAt { get; }

        public ForgotPassword(Guid id, string email, string lang, long expireAt)
        {
            Id       = id;
            Email    = email;
            Lang     = lang;
            ExpireAt = expireAt;
        }
    }
}
