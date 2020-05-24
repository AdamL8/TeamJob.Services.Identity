using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Newtonsoft.Json;

namespace TeamJob.Services.Identity.Application.Events.External
{
    [Message(exchange: "profile", external: true)]
    public class ProfileDeleted : IEvent
    {
        public Guid Id            { get; }
        public List<Guid> TeamIds { get; }
        public string Role        { get; }


        [JsonConstructor]
        public ProfileDeleted(Guid id, List<Guid> teamIds, string role)
        {
            Id      = id;
            TeamIds = teamIds;
            Role    = role;
        }
    }
}
