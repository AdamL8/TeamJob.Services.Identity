using Convey.CQRS.Commands;
using Newtonsoft.Json;
using System;

namespace TeamJob.Services.Identity.Commands
{
    public class DeleteUser : ICommand
    {
        public Guid Id { get; }

        [JsonConstructor]
        public DeleteUser(Guid id)
        {
            Id = id;
        }
    }
}
