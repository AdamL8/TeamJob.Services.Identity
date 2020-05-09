using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Exceptions
{
    public class UserAlreadyExistsException : TeamJobException
    {
        public override string Code { get; } = "user_already_exists";

        public UserAlreadyExistsException(string InEmail)
            : base($"User with an email [{InEmail}] already exists.")
        {
        }
    }
}
