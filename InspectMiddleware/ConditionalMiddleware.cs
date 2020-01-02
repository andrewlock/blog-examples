using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InspectMiddleware
{
    internal class ConditionalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ConditionalMiddleware> _logger;
        private readonly string _runBefore;
        private readonly bool _runMiddleware;

        public ConditionalMiddleware(RequestDelegate next, ILogger<ConditionalMiddleware> logger, string runBefore)
        {
            _runMiddleware = next.Target.GetType().FullName == runBefore;

            _next = next;
            _logger = logger;
            _runBefore = runBefore;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (_runMiddleware)
            {
                _logger.LogInformation("Running conditional middleware before {NextMiddleware}", _runBefore);
            }

            await _next(httpContext);
        }
    }
}