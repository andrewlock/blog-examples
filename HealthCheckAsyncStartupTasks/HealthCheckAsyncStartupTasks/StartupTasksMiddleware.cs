using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HealthCheckAsyncStartupTasks
{
    public class StartupTasksMiddleware
    {
        private readonly StartupTaskContext _context;
        private readonly RequestDelegate _next;
        private readonly StartupTasksCompleteOptions _options;

        public StartupTasksMiddleware(StartupTaskContext context, RequestDelegate next, IOptions<StartupTasksCompleteOptions> options)
        {
            _context = context;
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (_context.IsComplete)
            {
                await _next(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = _options.FailureResponseCode;
                httpContext.Response.Headers["Retry-After"] = _options.RetryAfterSeconds.ToString();
                await httpContext.Response.WriteAsync("Service Unavailable");
            }
        }
    }
}