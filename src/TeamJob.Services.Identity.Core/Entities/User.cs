using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamJob.Services.Identity.Core.Exceptions;

namespace TeamJob.Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Email                    { get; private set; }
        public string Role                     { get; private set; }
        public string Password                 { get; private set; }
        public long CreatedAt                  { get; private set; }
        public IEnumerable<string> Permissions { get; private set; }

        public User(Guid                id,
                    string              email,
                    string              password,
                    string              role,
                    long                createdAt,
                    IEnumerable<string> permissions = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException(email);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            if (Entities.Role.IsValid(role) == false)
            {
                throw new InvalidRoleException(role);
            }

            Id          = id;
            Email       = email.ToLowerInvariant();
            Password    = password;
            Role        = role.ToLowerInvariant();
            CreatedAt   = createdAt;
            Permissions = permissions ?? Enumerable.Empty<string>();
        }
    }
}
