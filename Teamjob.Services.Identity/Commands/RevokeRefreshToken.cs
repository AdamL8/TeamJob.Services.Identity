using Convey.CQRS.Commands;
using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.Commands
{
    public class RevokeRefreshToken : ICommand
    {
        public Guid UserId  { get; }
        public string Token { get; }

        [JsonConstructor]
        public RevokeRefreshToken(Guid userId, string token)
        {
            UserId = userId;
            Token  = token;
        }
    }
}
