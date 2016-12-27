using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ReloadingAppSettings.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly MyValues _myValues;
        private readonly MyValues _snapshot;
        private readonly MyValues _directValue;
        public ValuesController(IOptions<MyValues> optionsValue, IOptionsSnapshot<MyValues> snapshotValue, MyValues directValue)
        {
            _myValues = optionsValue.Value;
            _snapshot = snapshotValue.Value;
            _directValue = directValue;
        }

        // Uncomment to inject IConfigurationRoot instead
        // private readonly IConfigurationRoot _config;
        // public ValuesController(IConfigurationRoot config)
        // {
        //     _config = config;
        // }

        [HttpGet]
        public string Get()
        {
            return $@"
IOptions<>:         {_myValues.DefaultValue} 
IOptionsSnapshot<>: {_snapshot.DefaultValue},
Direct values:      {_directValue.DefaultValue},
Are options same:   {_myValues == _snapshot}";  
        }
    }
}
