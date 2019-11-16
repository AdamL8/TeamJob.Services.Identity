using Convey.Auth;
using Convey.CQRS.Commands;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Commands.Handlers
{
    public class RevokeAccessTokenHandler : ICommandHandler<RevokeAccessToken>
    {
        private readonly IAccessTokenService _accessTokenService;

        public RevokeAccessTokenHandler(IAccessTokenService InAccessTokenService)
        {
            _accessTokenService = InAccessTokenService;
        }

        public async Task HandleAsync(RevokeAccessToken InCommand)
        {
            await _accessTokenService.DeactivateAsync(InCommand.UserId.ToString("N"), InCommand.Token);
        }
    }
}
