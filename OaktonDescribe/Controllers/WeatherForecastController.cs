using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OaktonDescribe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get([FromQuery] TestClass model)
        {
            _logger.LogWarning("Value: {id}", model.Id);
            return model.Id.ToString();
        }
    }

    public class TestClass
    {
        public OrderId Id {get;set;}
    }

    [StronglyTypedId(backingType: StronglyTypedIdBackingType.Guid, jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson)]
    public partial struct OrderId {}
}
