using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace TeamJob.Services.Identity.Application.Commands
{
    public class RevokeRefreshToken : ICommand
    {
        public string RefreshToken { get; }

        public RevokeRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
