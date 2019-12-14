using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Events
{
    public class RevokeRefreshTokenRejected
    {
        public Guid Id       { get; }
        public string Reason { get; }

        public RevokeRefreshTokenRejected(Guid id, string reason)
        {
            Id     = id;
            Reason = reason;
        }
    }
}
