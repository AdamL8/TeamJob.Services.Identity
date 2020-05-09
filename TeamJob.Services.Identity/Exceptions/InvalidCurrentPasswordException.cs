using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Exceptions;

namespace TeamJob.Services.Identity.Exceptions
{
    public class InvalidCurrentPasswordException : TeamJobException
    {
        public override string Code { get; } = "invalid_current_password";

        public InvalidCurrentPasswordException()
            : base($"Invalid current password")
        {
        }
    }
}
