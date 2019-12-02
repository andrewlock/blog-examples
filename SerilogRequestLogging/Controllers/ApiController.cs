using Microsoft.AspNetCore.Mvc;

namespace SerilogRequestLogging.Controllers
{
    [ApiController]
    public class ApiController
    {
        [HttpGet("api")]
        public string SomeApiMethod()
        {
            return "Api called ok";
        }

    }
}