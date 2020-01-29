using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Events
{
    public class RefreshTokenRevoked : IEvent
    {
        public Guid Id { get; }

        [JsonConstructor]
        public RefreshTokenRevoked(Guid id)
        {
            Id = id;
        }
    }
}
