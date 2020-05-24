using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class InvalidRoleException : DomainException
    {
        public override string Code { get; } = "invalid_role";

        public InvalidRoleException(string role)
            : base($"Invalid role: {role}.")
        {
        }
    }
}
