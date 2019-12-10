using Convey.CQRS.Events;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Events
{
    public class PasswordChanged : IEvent 
    {
        public Guid Id { get; }

        [JsonConstructor]
        public PasswordChanged(Guid id)
        {
            Id = id;
        }
    }
}
