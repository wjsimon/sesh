using Microsoft.AspNetCore.Mvc;

namespace SSHC.Generator
{
    [ApiController, AutoGenerateApiClient]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() { return Ok(); }

        [HttpPost]
        public IActionResult Post() { return Ok(); }

        public IActionResult NotAnnotated() { return BadRequest(); }
        private void PrivateMethod() { }
    }
}
