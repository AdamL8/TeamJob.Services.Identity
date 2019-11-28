using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Teamjob.Services.Identity.Events.External
{
    [Message(exchange: "profile", external: true)]
    public class ProfileDeleted : IEvent
    {
        public Guid Id { get; }
        public List<Guid> Teams { get; }

        [JsonConstructor]
        public ProfileDeleted(Guid id, List<Guid> teams)
        {
            Id    = id;
            Teams = teams;
        }
    }
}
