using Convey.Types;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Domain
{
    public class User : IIdentifiable<Guid>
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public Guid     Id           { get; private set; }
        public string   Email        { get; private set; }
        public Role     Role         { get; private set; }
        public string   PasswordHash { get; private set; }
        public DateTime CreatedAt    { get; private set; }
        public DateTime UpdatedAt    { get; private set; }

        protected User()
        {
        }

        public User(Guid InId, string InEmail, Role InRole)
        {
            if (EmailRegex.IsMatch(InEmail) == false)
            {
                throw new TeamJobException("Codes.InvalidEmail",
                    $"Invalid email: '{InEmail}'.");
            }

            Id        = InId;
            Email     = InEmail.ToLowerInvariant();
            Role      = InRole;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string InPassword, IPasswordHasher<User> InPasswordHasher)
        {
            if (string.IsNullOrWhiteSpace(InPassword))
            {
                throw new TeamJobException("Codes.InvalidPassword",
                    "Password can not be empty.");
            }

            PasswordHash = InPasswordHasher.HashPassword(this, InPassword);
        }

        public bool ValidatePassword(string InPassword, IPasswordHasher<User> InPasswordHasher)
            => InPasswordHasher.VerifyHashedPassword(this, PasswordHash, InPassword) != PasswordVerificationResult.Failed;

    }
}
