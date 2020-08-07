using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ScopedHandlers
{
    public class ScopedMessageHander: DelegatingHandler
    {
        private readonly Guid _instanceId = Guid.NewGuid();
        private readonly ILogger<ScopedMessageHander> _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly IServiceProvider _provider;

        public ScopedMessageHander(ILogger<ScopedMessageHander> logger, IHttpContextAccessor accessor, IServiceProvider provider)
        {
            _logger = logger;
            _accessor = accessor;
            _provider = provider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Using scoped handler {InstanceId}", _instanceId);
            var scopedService = _provider.GetRequiredService<ScopedService>();
            _logger.LogInformation("Service ID in handler: {InstanceId}", scopedService.InstanceId);

            var contextService = _accessor.HttpContext.RequestServices.GetRequiredService<ScopedService>();
            _logger.LogInformation("Service ID in httpContext: {InstanceId}", contextService.InstanceId);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
