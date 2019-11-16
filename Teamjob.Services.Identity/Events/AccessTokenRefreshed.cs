using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class AccessTokenRefreshed : IEvent
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public AccessTokenRefreshed(Guid userId)
        {
            UserId = userId;
        }
    }
}
