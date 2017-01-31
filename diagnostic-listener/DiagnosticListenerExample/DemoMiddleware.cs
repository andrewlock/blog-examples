using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiagnosticListenerExample
{
    public class DemoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DiagnosticSource _diagnostics;
        private readonly ILogger _logger;

        public DemoMiddleware(RequestDelegate next, DiagnosticSource diagnosticSource, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DemoMiddleware>();
            _next = next;
            _diagnostics = diagnosticSource;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Starting middleware");
            if (_diagnostics.IsEnabled("DiagnosticListenerExample.MiddlewareStarting"))
            {
                _diagnostics.Write("DiagnosticListenerExample.MiddlewareStarting",
                    new
                    {
                        httpContext = context
                    });
            }

            await _next.Invoke(context);
        }
    }
}