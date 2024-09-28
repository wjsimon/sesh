using Microsoft.AspNetCore.Mvc;

namespace SSHC.Generator
{
    [ApiController, AutoGenerateApiClient]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet, Returns(typeof(string))]
        public IActionResult Get() { return Ok(); }

        [HttpGet("RenamedGet"), Returns(typeof(int))]
        public IActionResult GetRenamed() { return Ok(); }

        [HttpGet, Returns(typeof(bool))]
        public IActionResult ParameterizedGet([FromBody] int index, [FromBody] string name, [FromBody] object value) 
        { 
            return Ok(); 
        }

        [HttpPost, Returns(typeof(void))]
        public IActionResult Post() { return Ok(); }

        public IActionResult NotAnnotated() { return BadRequest(); }
        private void PrivateMethod() { }
    }
}
