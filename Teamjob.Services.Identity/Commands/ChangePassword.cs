using Convey.CQRS.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Commands
{
    public class ChangePassword : ICommand
    {
        public Guid Id                { get; }
        public string CurrentPassword { get; }
        public string NewPassword     { get; }

        [JsonConstructor]
        public ChangePassword(Guid   id, 
                              string currentPassword,
                              string newPassword)
        {
            Id              = id;
            CurrentPassword = currentPassword;
            NewPassword     = newPassword;
        }
    }
}
