using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class UserDeleted : IEvent
    {
        public Guid Id      { get; }
        public string Email { get; }
        public string Role  { get; }

        [JsonConstructor]
        public UserDeleted(Guid id, string email, string role)
        {
            Id     = id;
            Email  = email;
            Role   = role;
        }
    }
}
