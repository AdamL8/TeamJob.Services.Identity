using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class InvalidCredentialsException : DomainException
    {
        public override string Code { get; } = "service.identity.exception.invalid_credentials";
        public string Email { get; }

        public InvalidCredentialsException(string email)
            : base("Invalid credentials.")
        {
            Email = email;
        }
    }
}
