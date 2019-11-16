using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Events
{
    public class RefreshAccessTokenRejected : IEvent
    {
        public Guid UserId   { get; }
        public string Reason { get; }

        public RefreshAccessTokenRejected(Guid userId, string reason)
        {
            UserId = userId;
            Reason = reason;
        }
    }
}
