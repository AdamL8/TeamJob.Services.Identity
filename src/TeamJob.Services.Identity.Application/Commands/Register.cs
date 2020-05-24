using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace TeamJob.Services.Identity.Application.Commands
{
    public class Register : ICommand
    {
        public Guid UserId                     { get; }
        public string Email                    { get; }
        public string Password                 { get; }
        public string Role                     { get; }
        public IEnumerable<string> Permissions { get; }

        public Register(string email, string password, string role, IEnumerable<string> permissions)
        {
            UserId      =  Guid.NewGuid();
            Email       = email;
            Password    = password;
            Role        = role;
            Permissions = permissions;
        }
    }
}
