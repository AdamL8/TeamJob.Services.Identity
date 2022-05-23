using System;
using System.Collections.Generic;
using System.Text;
using Convey.Auth;
using TeamJob.Services.Identity.Application.DTO;
using TeamJob.Services.Identity.Application.Services;

namespace TeamJob.Services.Identity.Infrastructure.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IJwtHandler _jwtHandler;

        public JwtProvider(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public AuthDto Create(string userId, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null)
        {
            var jwt = _jwtHandler.CreateToken(userId, role, audience, claims);

            return new AuthDto
            {
                // ID Should be removed later
                Id          = userId,
                AccessToken = jwt.AccessToken,
                Role        = jwt.Role,
                Expires     = jwt.Expires
            };
        }
    }
}
