using Microsoft.AspNetCore.Mvc;

namespace ApplicationPartsDebugging.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get() => "The test works!";
    }
}
