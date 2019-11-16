using Convey.CQRS.Commands;
using Newtonsoft.Json;

namespace Teamjob.Services.Identity.Commands
{
    public class Register : ICommand
    {
        public string Email    { get; set;  }
        public string Password { get; set; }
        public string Role     { get; set;  }

        [JsonConstructor]
        public Register(string email, string password, string role)
        {
            Email    = email;
            Password = password;
            Role     = role;
        }
    }
}
