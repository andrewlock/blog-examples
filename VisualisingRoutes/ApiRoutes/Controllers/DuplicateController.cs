using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiRoutes.Controllers
{
    [ApiController]
    public class DuplicateController
    {
        [HttpGet("api/values")]
        public string Get()
        {
            return "oops";
        }
    }
}
