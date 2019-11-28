using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.WebApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teamjob.Services.Identity.DTO;
using Teamjob.Services.Identity.Requests;

namespace Teamjob.Services.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : BaseController
    {
        private readonly IRequestDispatcher _requestDispatcher;

        public IdentityController(IRequestDispatcher InRequestDispatcher)
        {
            _requestDispatcher = InRequestDispatcher;
        }

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
    }
}
