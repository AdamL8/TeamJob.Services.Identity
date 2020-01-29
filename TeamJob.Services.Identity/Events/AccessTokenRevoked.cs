using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class AccessTokenRevoked : IEvent
    {
        public Guid Id { get; }

        [JsonConstructor]
        public AccessTokenRevoked(Guid id)
        {
            Id = id;
        }
    }
}
