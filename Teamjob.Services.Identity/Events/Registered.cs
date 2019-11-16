using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class Registered : IEvent
    {
        public Guid   UserId { get; }
        public string Email  { get; }
        public string Role   { get; }

        [JsonConstructor]
        public Registered(Guid userId, string email, string role)
        {
            UserId = userId;
            Email  = email;
            Role   = role;
        }
    }
}
