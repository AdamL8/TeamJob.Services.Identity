using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teamjob.Services.Identity.Commands;
using Teamjob.Services.Identity.Requests;

namespace Teamjob.Services.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IRequestDispatcher _requestDispatcher;

        public IdentityController(ICommandDispatcher InCommandDispatcher,
                                  IRequestDispatcher InRequestDispatcher)
        {
            _commandDispatcher = InCommandDispatcher;
            _requestDispatcher = InRequestDispatcher;
        }

        // POST api/identity/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Login([FromBody]Login InCommand)
        {
            return Ok(await _requestDispatcher.DispatchAsync<Login, string>(InCommand));
        }

        // POST api/identity/register
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody]Register InCommand)
        {
            await _commandDispatcher.SendAsync(InCommand);

            return CreatedAtAction(nameof(Register), string.Empty, new { email = InCommand.Email });
        }
    }
}
