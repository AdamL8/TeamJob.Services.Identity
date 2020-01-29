using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Events
{
    public class RefreshAccessTokenRejected : IEvent
    {
        public Guid Id       { get; }
        public string Reason { get; }

        public RefreshAccessTokenRejected(Guid id, string reason)
        {
            Id     = id;
            Reason = reason;
        }
    }
}
