using Microsoft.AspNetCore.Mvc;

namespace Simons.Generators.ApiClient
{
    [ApiController, AutoGenerateApiClient]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        //[HttpGet, Returns(typeof(string))]
        //public IActionResult Get() { return Ok(); }

        //[HttpGet("RenamedGet"), Returns(typeof(int))]
        //public IActionResult GetRenamed() { return Ok(); }

        //[HttpGet, Returns(typeof(bool))]
        //public IActionResult OneParameterGet([FromQuery] int index)
        //{
        //    return Ok();
        //}

        //[HttpGet, Returns(typeof(bool))]
        //public IActionResult TwoParameterGet([FromQuery] int index, [FromQuery] string name)
        //{
        //    return Ok();
        //}

        //[HttpGet, Returns(typeof(bool))]
        //public IActionResult ThreeParameterGet([FromQuery] int index, [FromQuery] string name, [FromQuery] object value)
        //{
        //    return Ok();
        //}

        //[HttpPost, Returns(typeof(void))]
        //public IActionResult PostEmpty() { return Ok(); }

        //[HttpPost, Returns(typeof(void))]
        //public IActionResult OneParameterPostEmpty([FromQuery] int index) { return Ok(); }

        //[HttpPost, Returns(typeof(void))]
        //public IActionResult TwoParametersPostEmpty([FromQuery] int index, [FromQuery] string name) { return Ok(); }

        //[HttpPost, Returns(typeof(bool))]
        //public IActionResult ThreeParametersPostEmpty([FromQuery] int index, [FromQuery] string name, [FromQuery] object value) { return Ok(); }

        //[HttpPost, Returns(typeof(void))]
        //public IActionResult Post([FromBody] object payload) { return Ok(); }

        //[HttpPost, Returns(typeof(void))]
        //public IActionResult OneParameterPost([FromQuery] int index, [FromBody] object payload) { return Ok(); }

        [HttpPost, Returns(typeof(Dictionary<string, string>))]
        public IActionResult TwoParametersPost([FromQuery] int index, [FromQuery] string name, [FromBody] object payload) { return Ok(); }

        //[HttpPost, Returns(typeof(List<object>))]
        //public IActionResult ThreeParametersPost([FromQuery] int index, [FromQuery] string name, [FromQuery] object value, [FromBody] object payload) { return Ok(); }

        //[HttpPost, Returns(typeof(bool))]
        //public IActionResult ThreeParametersPostUnordered([FromQuery] int index, [FromBody] object payload, [FromQuery] string name, [FromQuery] object value) { return Ok(); }

        //public IActionResult NotAnnotated() { return BadRequest(); }
        //private void PrivateMethod() { }
    }
}
