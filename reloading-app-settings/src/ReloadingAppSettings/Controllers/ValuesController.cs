using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ReloadingAppSettings.Options;

namespace ReloadingAppSettings.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller, IDisposable
    {
        private readonly MyValues _myValues;
        public ValuesController(IOptionsMonitor<MyValues> values)
        {
            _myValues = values.CurrentValue;
        }

        // Uncomment to inject IConfigurationRoot instead
        // private readonly IConfigurationRoot _config;
        // public ValuesController(IConfigurationRoot config)
        // {
        //     _config = config;
        // }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return _myValues.DefaultValue;
            // return _config.GetValue<string>("MyValues:DefaultValue");
        }
    }
}
