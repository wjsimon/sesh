using Microsoft.AspNetCore.Mvc;

namespace Simons.Generators.Http.Controllers
{
    [ApiController, AutoGenerateApiClient]
    [Route("[controller]")]
    public class AutogenerateController : ControllerBase
    {
        [HttpGet, Returns(typeof(string))]
        public IActionResult Get() { return Ok(); }
    }
}
