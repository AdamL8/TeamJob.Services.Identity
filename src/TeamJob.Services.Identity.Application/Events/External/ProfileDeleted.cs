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
        public string Id            { get; }
        public List<string> TeamIds { get; }
        public string Role        { get; }


        [JsonConstructor]
        public ProfileDeleted(string id, List<string> teamIds, string role)
        {
            Id      = id;
            TeamIds = teamIds;
            Role    = role;
        }
    }
}
