using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events.External
{
    [Message(exchange: "profile", external: true)]
    public class ProfileDeleted : IEvent
    {
        public Guid ProfileId { get; }

        [JsonConstructor]
        public ProfileDeleted(Guid profileId)
        {
            ProfileId = profileId;
        }
    }
}
