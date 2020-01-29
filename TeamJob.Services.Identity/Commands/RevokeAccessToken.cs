using Convey.CQRS.Commands;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Commands
{
    public class RevokeAccessToken : ICommand
    {
        public Guid Id      { get; }
        public string Token { get; }

        [JsonConstructor]
        public RevokeAccessToken(Guid id, string token)
        {
            Id     = id;
            Token  = token;
        }
    }
}
