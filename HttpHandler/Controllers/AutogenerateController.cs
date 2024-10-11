using Microsoft.AspNetCore.Mvc;
using SSHC.Generator;

namespace SSHC.Controllers
{
    [ApiController, AutoGenerateApiClient]
    [Route("[controller]")]
    public class AutogenerateController : ControllerBase
    {
        [HttpGet, Returns(typeof(string))]
        public IActionResult Get() { return Ok(); }
    }
}
