using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomMiddleware.Tests
{
    public class HttpsCustomHeaderMiddlewareFunctionalTests : IClassFixture<HttpCustomHeaderFixture<DemoWebsite.Startup>>
    {
        public HttpsCustomHeaderMiddlewareFunctionalTests(HttpCustomHeaderFixture<DemoWebsite.Startup> fixture)
        {
            Client = fixture.Client;
        }

        public HttpClient Client { get; }

        [Theory]
        [InlineData("GET")]
        [InlineData("HEAD")]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task AllMethods_AddCustomHeader(string method)
        {
            // Arrange
            var path = "/CustomHeadersMiddleware/BG36A632-C4D2-4B71-B2BD-18625ADDA87F";
            var request = new HttpRequestMessage(new HttpMethod(method), path);

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(path, content);
            var responseHeaders = response.Headers;

            var header = response.Headers.GetValues("X-Demo-Header").FirstOrDefault();
            Assert.Equal(header, "Header custom value");

        }
    }
}