using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;

namespace DiagnosticListenerExample
{
    public class DemoDiagnosticListener
    {
        [DiagnosticName("DiagnosticListenerExample.MiddlewareStarting")]
        public virtual void OnMiddlewareStarting(HttpContext httpContext, string name)
        {
            Console.WriteLine($"Demo Middleware Starting, path: {httpContext.Request.Path}");
        }
    }
}