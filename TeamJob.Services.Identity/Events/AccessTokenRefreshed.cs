using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class AccessTokenRefreshed : IEvent
    {
        public Guid Id { get; }

        [JsonConstructor]
        public AccessTokenRefreshed(Guid id)
        {
            Id = id;
        }
    }
}
