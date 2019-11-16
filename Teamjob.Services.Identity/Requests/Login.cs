using Convey.WebApi.Requests;
using Newtonsoft.Json;

namespace Teamjob.Services.Identity.Requests
{
    public class Login : IRequest
    {
        public string Email    { get; }
        public string Password { get; }

        [JsonConstructor]
        public Login(string email, string password)
        {
            Password = password;
            Email    = email;
        }
    }
}
