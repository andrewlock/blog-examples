using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ExampleApp3.IntegrationTests
{
    public class GenericHostExampleTests
    {
        [Fact]
        public async Task ShouldReturnHelloWorld()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                // Add TestServer
                webHost.UseTestServer();
                    webHost.Configure(app => app.Run(async ctx =>
                        await ctx.Response.WriteAsync("Hello World!")));
                });

            // Build and start the IHost
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient to send requests to the TestServer
            var client = host.GetTestClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello World!", responseString);
        }
    }
}
