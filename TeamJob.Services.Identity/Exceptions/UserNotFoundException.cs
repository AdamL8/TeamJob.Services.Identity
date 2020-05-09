using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Exceptions
{
    public class UserNotFoundException : TeamJobException
    {
        public override string Code { get; } = "user_not_found";

        public UserNotFoundException(Guid InUserId)
            : base($"User with ID [{InUserId}] was not found.")
        {
        }

        public UserNotFoundException(string InUserEmail)
            : base($"User with Email [{InUserEmail}] was not found.")
        {
        }
    }
}
