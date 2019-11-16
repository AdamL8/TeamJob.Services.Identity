using Microsoft.AspNetCore.Mvc;

namespace Teamjob.Services.Identity.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("TeamJob Identity Service");
    }
}
