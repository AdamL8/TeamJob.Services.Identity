using System.Threading.Tasks;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.WebApi;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teamjob.Services.Identity.Commands;
using Teamjob.Services.Identity.Requests;

namespace Teamjob.Services.Identity.Controllers
{
    [ApiController]
    [JwtAuth]
    [Route("api/[controller]")]
    public class TokensController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IRequestDispatcher _requestDispatcher;

        public TokensController(ICommandDispatcher InCommandDispatcher,
                                  IRequestDispatcher InRequestDispatcher)
        {
            _commandDispatcher = InCommandDispatcher;
            _requestDispatcher = InRequestDispatcher;
        }

        [HttpPost("access-tokens/{InRefreshToken}/refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken(string InRefreshToken, RefreshAccessToken InCommand)
            => Ok(await _requestDispatcher.DispatchAsync<RefreshAccessToken, string>(InCommand.Bind(c => c.Token, InRefreshToken)));

        [HttpPost("access-tokens/revoke")]
        public async Task<IActionResult> RevokeAccessToken(RevokeAccessToken InCommand)
        {
            await _commandDispatcher.SendAsync(InCommand);

            return CreatedAtAction(nameof(RevokeAccessToken), InCommand.UserId, new { userId = InCommand.UserId, token = InCommand.Token });
        }

        [HttpPost("refresh-tokens/{InRefreshToken}/revoke")]
        public async Task<IActionResult> RevokeRefreshToken(string InRefreshToken, RevokeRefreshToken InCommand)
        {
            await _commandDispatcher.SendAsync(InCommand.Bind(c => c.Token, InRefreshToken));

            return CreatedAtAction(nameof(RevokeRefreshToken), InCommand.UserId, new { userId = InCommand.UserId, token = InCommand.Token });
        }
    }
}
