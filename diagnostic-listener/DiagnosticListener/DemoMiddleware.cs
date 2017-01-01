using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiagnosticListener
{
    public class DemoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DiagnosticSource _diagnostics;

        public DemoMiddleware(RequestDelegate next, DiagnosticSource diagnosticSource)
        {
            _next = next;
            _diagnostics = diagnosticSource;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_diagnostics.IsEnabled("DiagnosticListener.MiddlewareStarting"))
            {
                _diagnostics.Write("DiagnosticListener.MiddlewareStarting",
                    new
                    {
                        httpContext = context
                    });
            }
            
            await _next.Invoke(context);
        }
    }
}