using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Events
{
    public class RevokeRefreshTokenRejected
    {
        public Guid UserId   { get; }
        public string Reason { get; }

        public RevokeRefreshTokenRejected(Guid userId, string reason)
        {
            UserId = userId;
            Reason = reason;
        }
    }
}
