using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InspectMiddleware
{
    internal class NameCheckerMiddleware
    {
        public const string WrappedMiddlewareKey = "NextMiddlewareName";
        private readonly RequestDelegate _next;
        private readonly ILogger<NameCheckerMiddleware> _logger;
        private readonly string _wrappedMiddlewareName;

        public NameCheckerMiddleware(RequestDelegate next, ILogger<NameCheckerMiddleware> logger)
        {
            _wrappedMiddlewareName = next.Target.GetType().FullName;
            _next = next;
            _logger = logger;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items[WrappedMiddlewareKey] = _wrappedMiddlewareName;

            await _next(httpContext);
        }
    }

}