using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Application.Exceptions
{
    public class UserNotFoundException : AppException
    {
        public override string Code { get; } = "user_not_found";
        public string UserId { get; }

        public UserNotFoundException(string userId)
            : base($"User with ID: '{userId}' was not found.")
        {
            UserId = userId;
        }
    }
}
