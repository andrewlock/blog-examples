using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace CustomMiddleware.Tests
{
    public class CustomHeaderMiddlewareTests
    {
        [Fact]
        public async Task HttpRequest_WithDefaultSecurityPolicy_SetsSecurityHeaders()
        {
            // Arrange
            var hostBuilder = new WebHostBuilder()
                .Configure(app =>
                           {
                               app.UseCustomHeaderMiddleware("X-Custom-Header", "My custom value");
                               app.Run(async context =>
                                             {
                                                 await context.Response.WriteAsync("Test response");
                                             });
                           });

            using (var server = new TestServer(hostBuilder))
            {
                // Act
                // Actual request.
                var response = await server.CreateRequest("/")
                    .SendAsync("PUT");

                // Assert
                response.EnsureSuccessStatusCode();

                Assert.Equal("Test response", await response.Content.ReadAsStringAsync());

                var header = response.Headers.GetValues("X-Custom-Header").FirstOrDefault();
                Assert.Equal(header, "My custom value");
            }
        }
    }
}
