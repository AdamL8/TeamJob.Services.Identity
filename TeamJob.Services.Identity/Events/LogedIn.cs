using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class LogedIn : IEvent 
    {
        public Guid Id { get; }

        [JsonConstructor]
        public LogedIn(Guid id)
        {
            Id = id;
        }
    }
}
