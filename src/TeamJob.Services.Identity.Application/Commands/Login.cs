using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace TeamJob.Services.Identity.Application.Commands
{
    public class Login : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Login(string email, string password)
        {
            Email    = email;
            Password = password;
        }
    }
}
