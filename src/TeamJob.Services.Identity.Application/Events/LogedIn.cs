using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace TeamJob.Services.Identity.Application.Events
{
    public class LogedIn : IEvent
    {
        public Guid UserId { get; }
        public string Role { get; }

        public LogedIn(Guid userId, string role)
        {
            UserId = userId;
            Role   = role;
        }
    }
}
