using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Exceptions;

namespace TeamJob.Services.Identity.Exceptions
{
    public class InvalidCredentialsException : TeamJobException
    {
        public override string Code { get; } = "invalid_credentials";

        public InvalidCredentialsException(string InEmail)
            : base($"Invalid password entered for email [{InEmail}]")
        {
        }
    }
}
