using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace injecting_services_when_creating_mvcoptions
{
    public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
    {
        private readonly ILogger<MvcOptions> _logger;
        private readonly ObjectPoolProvider _objectPoolProvider;
        public ConfigureMvcOptions(ILogger<MvcOptions> logger, ObjectPoolProvider objectPoolProvider)
        {
            _logger = logger;
            _objectPoolProvider = objectPoolProvider;
        }

        public void Configure(MvcOptions options)
        {
            options.UseHtmlEncodeJsonInputFormatter(_logger, _objectPoolProvider);
        }
    }
}