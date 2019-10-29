using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ExampleApp2.IntegrationTests
{
    public class HttpTests: IClassFixture<ExampleAppTestFixture>, IDisposable
    {
        readonly ExampleAppTestFixture _fixture;
        readonly HttpClient _client;

        public HttpTests(ExampleAppTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            fixture.Output = output;
            _client = fixture.CreateClient();
        }
        
        public void Dispose() => _fixture.Output = null;

        [Fact]
        public async Task CanCallApi()
        {
            var result = await _client.GetAsync("/");

            result.EnsureSuccessStatusCode();
            
            var content = await result.Content.ReadAsStringAsync();

            Assert.Contains("Welcome", content);
        }
    }
}
