using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HostFilteringExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string GetPasswordReset()
        {
            return Url.Action("Get", "WeatherForecast", values: null, protocol: "https");
        }
    }
}
