using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _configuration.GetSection("MySection")
                .AsEnumerable().Select(kvp => $"{kvp.Key}:{kvp.Value}");
        }
    }
}
