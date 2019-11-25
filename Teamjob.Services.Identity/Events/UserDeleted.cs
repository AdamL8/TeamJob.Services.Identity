using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class UserDeleted : IEvent
    {
        public Guid   UserId { get; }
        public string Email  { get; }
        public string Role   { get; }

        [JsonConstructor]
        public UserDeleted(Guid userId, string email, string role)
        {
            UserId = userId;
            Email  = email;
            Role   = role;
        }
    }
}
