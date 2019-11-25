using Convey.CQRS.Commands;
using Newtonsoft.Json;

namespace Teamjob.Services.Identity.Commands
{
    public class Register : ICommand
    {
        public string Email    { get; }
        public string Password { get; }
        public string Role     { get; }

        [JsonConstructor]
        public Register(string email, string password, string role)
        {
            Email    = email;
            Password = password;
            Role     = role;
        }
    }
}
