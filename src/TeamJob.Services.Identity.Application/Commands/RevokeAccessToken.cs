using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace TeamJob.Services.Identity.Application.Commands
{
    public class RevokeAccessToken : ICommand
    {
        public string AccessToken { get; }

        public RevokeAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
