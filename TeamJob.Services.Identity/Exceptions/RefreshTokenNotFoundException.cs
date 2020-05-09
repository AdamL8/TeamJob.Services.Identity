using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Exceptions;

namespace TeamJob.Services.Identity.Exceptions
{
    public class RefreshTokenNotFoundException : TeamJobException
    {
        public override string Code { get; } = "refresh_token_not_found";

        public RefreshTokenNotFoundException(string InToken, Guid InUserId)
            : base($"Refresh token [{InToken}] to revoke with User ID [{InUserId}] was not found.")
        {
        }
    }
}
