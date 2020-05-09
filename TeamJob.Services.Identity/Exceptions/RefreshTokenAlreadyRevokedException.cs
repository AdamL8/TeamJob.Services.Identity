using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Exceptions;

namespace TeamJob.Services.Identity.Exceptions
{
    public class RefreshTokenAlreadyRevokedException : TeamJobException
    {
        public override string Code { get; } = "jwt_refresh_token_already_revoked";

        public RefreshTokenAlreadyRevokedException(Guid InTokenId)
            : base($"Refresh token [{InTokenId}] is already revoked")
        {
        }
    }
}
