using Convey.Types;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Exceptions;

namespace Teamjob.Services.Identity.Domain
{
    public class RefreshToken : IIdentifiable<Guid>
    {
        public Guid      Id        { get; private set; }
        public Guid      UserId    { get; private set; }
        public string    Token     { get; private set; }
        public DateTime  CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool      IsRevoked   => RevokedAt.HasValue;

        protected RefreshToken()
        {
        }

        public RefreshToken(User InUser, IPasswordHasher<User> InPasswordHasher)
        {
            Id        = Guid.NewGuid();
            UserId    = InUser.Id;
            CreatedAt = DateTime.UtcNow;
            Token     = CreateToken(InUser, InPasswordHasher);
        }

        public void Revoke()
        {
            if (IsRevoked)
            {
                throw new TeamJobException("Codes.RefreshTokenAlreadyRevoked",
                    $"Refresh token: '{Id}' was already revoked at '{RevokedAt}'.");
            }

            RevokedAt = DateTime.UtcNow;
        }

        private static string CreateToken(User InUser, IPasswordHasher<User> InPasswordHasher)
            => InPasswordHasher.HashPassword(InUser, Guid.NewGuid().ToString("N"))
                .Replace("=", string.Empty)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty);
    }
}
