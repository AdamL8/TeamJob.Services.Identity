using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class InvalidRefreshTokenException : DomainException
    {
        public override string Code { get; } = "service.identity.exception.invalid_refresh_token";

        public InvalidRefreshTokenException()
            : base("Invalid refresh token.")
        {
        }
    }
}
