using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class RevokedRefreshTokenException : DomainException
    {
        public override string Code { get; } = "service.identity.exception.revoked_refresh_token";

        public RevokedRefreshTokenException()
            : base("Revoked refresh token.")
        {
        }
    }
}
