using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class InvalidPasswordException : DomainException
    {
        public override string Code { get; } = "invalid_password";

        public InvalidPasswordException()
            : base($"Invalid password.")
        {
        }
    }
}
