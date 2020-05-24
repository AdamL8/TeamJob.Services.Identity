using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public override string Code { get; } = "invalid_email";

        public InvalidEmailException(string email)
            : base($"Invalid email: {email}.")
        {
        }
    }
}
