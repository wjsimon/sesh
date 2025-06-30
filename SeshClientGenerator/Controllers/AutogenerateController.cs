using Microsoft.AspNetCore.Mvc;

namespace SeshLib.Generators.HttpClient.Controllers
{
    [ApiController, AutoGenerateSeshClient]
    [Route("[controller]")]
    public class AutogenerateController : ControllerBase
    {
        [HttpGet, Returns(typeof(string))]
        public IActionResult Get() { return Ok(); }
    }
}
