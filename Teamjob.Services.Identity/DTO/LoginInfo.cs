using Newtonsoft.Json;
using System;

namespace Teamjob.Services.Identity.DTO
{
    public class LoginInfo
    {
        public Guid Id            { get; private set; }
        public string AccessToken { get; private set; }
        public string Role        { get; private set; }

        [JsonConstructor]
        public LoginInfo(Guid id, string accessToken, string role)
        {
            Id          = id;
            AccessToken = accessToken;
            Role        = role;
        }
    }
}
