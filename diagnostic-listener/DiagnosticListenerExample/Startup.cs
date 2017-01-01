using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;

namespace DiagnosticListenerExample
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, DiagnosticListener diagnosticListener)
        {
            // Listen for middleware events and log them to the console.
            var listener = new DemoDiagnosticListener();
            diagnosticListener.SubscribeWithAdapter(listener);

            app.UseMiddleware<DemoMiddleware>();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
