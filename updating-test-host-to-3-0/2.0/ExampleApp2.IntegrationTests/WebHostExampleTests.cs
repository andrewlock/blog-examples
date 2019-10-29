using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace ExampleApp2.IntegrationTests
{
    public class WebHostExampleTests
    {
        [Fact]
        public async Task ShouldReturnHelloWorld()
        {
            // Arrange
            var webHostBuilder = new WebHostBuilder()
                .Configure(app => app.Run(async ctx =>
                    await ctx.Response.WriteAsync("Hello World!")
                ));

            var server = new TestServer(webHostBuilder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert        
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello World!", responseString);
        }
    }
}