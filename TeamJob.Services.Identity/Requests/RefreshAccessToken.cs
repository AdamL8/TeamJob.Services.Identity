using Convey.WebApi.Requests;
using Newtonsoft.Json;

namespace Teamjob.Services.Identity.Requests
{
    public class RefreshAccessToken : IRequest
    {
        public string Token { get; }

        [JsonConstructor]
        public RefreshAccessToken(string token)
        {
            Token = token;
        }
    }
}
