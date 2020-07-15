using ApiRoutes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ApiRoutesTest
{
    public class GenerateGraphTest
        : IClassFixture<WebApplicationFactory<ApiRoutes.Startup>>
    {

        private readonly WebApplicationFactory<ApiRoutes.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public GenerateGraphTest(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public void GenerateGraph()
        {
            var graphWriter = _factory.Services.GetRequiredService<DfaGraphWriter>();
            var endpointData = _factory.Services.GetRequiredService<EndpointDataSource>();


            using (var sw = new StringWriter())
            {
                graphWriter.Write(endpointData, sw);
                var graph = sw.ToString();
                _output.WriteLine(graph);
            }
        }
    }
}
