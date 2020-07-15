using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.Options;

namespace CustomGraphWriter
{
    public class GraphEndpointMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CustomDfaGraphWriter _graphWriter;
        private readonly EndpointDataSource _endpointData;

        public GraphEndpointMiddleware(
            RequestDelegate next, 
            IServiceProvider provider, 
            EndpointDataSource endpointData,
            IOptions<GraphDisplayOptions> options)
        {
            _next = next;
            _graphWriter = new CustomDfaGraphWriter(provider, options.Value);
            _endpointData = endpointData;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/plain";
            await using (var sw = new StringWriter())
            {
                _graphWriter.Write(_endpointData, sw);
                await context.Response.WriteAsync(sw.ToString());
            }
        }
    }
}
