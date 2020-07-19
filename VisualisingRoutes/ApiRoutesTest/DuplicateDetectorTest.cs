using ApiRoutes;
using CustomGraphWriter;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace ApiRoutesTest
{
    public class DuplicateDetectorTest
        : IClassFixture<WebApplicationFactory<ApiRoutes.Startup>>
    {

        private readonly WebApplicationFactory<ApiRoutes.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public DuplicateDetectorTest(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public void ShouldNotHaveDuplicateEndpoints()
        {
            var detector = new DuplicateEndpointDetector(_factory.Services);
            var endpointData = _factory.Services.GetRequiredService<EndpointDataSource>();

            var duplicates = detector.GetDuplicateEndpoints(endpointData);

            foreach (var keyValuePair in duplicates)
            {
                var allMatches = string.Join(", ", keyValuePair.Value);
                _output.WriteLine($"Duplicate: '{keyValuePair.Key}'. Matches: {allMatches}");
            }

            Assert.Empty(duplicates);
        }
    }
}
