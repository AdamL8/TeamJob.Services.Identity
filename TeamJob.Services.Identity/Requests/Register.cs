using Convey.CQRS.Commands;
using Convey.WebApi.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Requests
{
    public class Register : IRequest
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
