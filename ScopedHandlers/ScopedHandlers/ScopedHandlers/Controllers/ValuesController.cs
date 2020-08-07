using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ScopedHandlers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;
        private readonly ScopedService _service;
        private readonly ILogger<ValuesController> _logger;
        public ValuesController(IHttpClientFactory factory, ScopedService service, ILogger<ValuesController> logger)
        {
            _factory = factory;
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> GetAsync()
        {
            var client = _factory.CreateClient("test");
            var result = await client.GetAsync("posts");
            _logger.LogInformation("Service ID in controller {InstanceId}", _service.InstanceId);

            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }
    }
}
