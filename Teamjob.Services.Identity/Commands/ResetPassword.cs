using Convey.CQRS.Commands;
using Newtonsoft.Json;

namespace Teamjob.Services.Identity.Commands
{
    public class ResetPassword : ICommand
    {
        public string Email { get; }
        public string Lang  { get; }

        [JsonConstructor]
        public ResetPassword(string email, string lang)
        {
            Email = email;
            Lang  = lang;
        }
    }
}
