using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace TeamJob.Services.Identity.Application.Events
{
    public class UserDeleted : IEvent
    {
        public string Id      { get; }
        public string Email { get; }
        public string Role  { get; }

        public UserDeleted(string id, string email, string role)
        {
            Id    = id;
            Email = email;
            Role  = role;
        }
    }
}
