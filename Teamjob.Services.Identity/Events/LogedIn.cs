using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class LogedIn : IEvent 
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public LogedIn(Guid userId)
        {
            UserId = userId;
        }
    }
}
