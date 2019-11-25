using Microsoft.AspNetCore.Mvc;

namespace Teamjob.Services.Identity.Controllers
{
    [Route("")]
    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Get() => Ok("TeamJob Identity Service");
    }
}
