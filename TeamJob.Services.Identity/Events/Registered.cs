using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class Registered : IEvent
    {
        public Guid   Id    { get; }
        public string Email { get; }
        public string Role  { get; }

        [JsonConstructor]
        public Registered(Guid id, string email, string role)
        {
            Id    = id;
            Email = email;
            Role  = role;
        }
    }
}
