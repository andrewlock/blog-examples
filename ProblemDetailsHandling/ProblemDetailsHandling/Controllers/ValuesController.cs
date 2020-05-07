using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsHandling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        const string ErrorMessage = "Validation errors";
        const string PropertyName = "The property";
        
        [HttpGet("400")]
        public BadRequestObjectResult Error400()
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]> {{PropertyName, new[] {ErrorMessage}}}));
        }
        
        [HttpGet("400/automatic")]
        public BadRequestObjectResult Error400Auto([Required] int? unset)
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]> {{PropertyName, new[] {"Should not be called"}}}));
        }
        
        [HttpGet("401")]
        public UnauthorizedResult Error401()
        {
            return Unauthorized();
        }
        
        [HttpGet("403")]
        public ForbidResult Error403()
        {
            return Forbid();
        }
        
        [HttpGet("404")]
        public ActionResult Error404()
        {
            return NotFound("The result was not found");
        }

        [HttpGet("500")]
        public StatusCodeResult Error500()
        {
            return StatusCode(500);
        }
        
        [HttpGet("500/throw")]
        public StatusCodeResult Error500Throw()
        {
            throw new Exception();
        }
        
        [HttpPost("405")] // call with a get
        public StatusCodeResult Error405()
        {
            throw new Exception();
        }
    }
}
