using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _myValues.DefaultValues;
        }
    }
}
