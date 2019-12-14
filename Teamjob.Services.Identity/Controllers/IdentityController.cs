using System.Threading.Tasks;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teamjob.Services.Identity.Commands;
using Teamjob.Services.Identity.DTO;
using Teamjob.Services.Identity.Requests;

namespace Teamjob.Services.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : BaseController
    {
        private readonly IRequestDispatcher _requestDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public IdentityController(IRequestDispatcher InRequestDispatcher,
                                  ICommandDispatcher InCommandDispatcher)
        {
            _requestDispatcher = InRequestDispatcher;
            _commandDispatcher = InCommandDispatcher;
        }

        [HttpGet("me")]
        [JwtAuth]
        public IActionResult Get() => Content($"Your id: '{UserId:N}'.");

        // POST api/identity/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Login([FromBody]Login InRequest)
        {
            return Ok(await _requestDispatcher.DispatchAsync<Login, LoginInfo>(InRequest));
        }

        // POST api/identity/register
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody]Register InRequest)
        {
            return Ok(await _requestDispatcher.DispatchAsync<Register, LoginInfo>(InRequest));
        }

        // POST api/identity/forgot-password
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPassword InCommand)
        {
            await _commandDispatcher.SendAsync(InCommand);

            return CreatedAtAction(nameof(ResetPassword), InCommand.Email, new { email = InCommand.Email });
        }

        // PUT api/identity/me/password
        [HttpPut("me/password")]
        [JwtAuth]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> ChangePassword([FromBody]ChangePassword InCommand)
        {
            await _commandDispatcher.SendAsync(InCommand);

            return CreatedAtAction(nameof(ChangePassword), InCommand.Id, new { id = InCommand.Id });
        }
    }
}
