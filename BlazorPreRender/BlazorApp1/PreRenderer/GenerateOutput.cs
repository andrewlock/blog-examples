using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PreRenderer
{
    public class GenerateOutput : IClassFixture<AppTestFixture>, IDisposable
    {
        private readonly AppTestFixture _fixture;
        private readonly HttpClient _client;
        private readonly string _outputPath;
        private readonly ITestOutputHelper _output;

        public GenerateOutput(AppTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _fixture.Output = output;
            _client = fixture.CreateDefaultClient();

            var config = _fixture.Services.GetRequiredService<IConfiguration>();
            _outputPath = config["RenderOutputDirectory"];

            if (string.IsNullOrEmpty(_outputPath))
            {
                throw new ArgumentException("RenderOutputDirectory config value was null or empty", nameof(_outputPath));
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
            _fixture.Output = null;
        }

        /// <summary>
        /// Massage the values into something that works for xunit theory
        /// </summary>
        public static IEnumerable<object[]> GetPagesToPreRender()
            => PrerenderRouteHelper
                .GetRoutes(typeof(BlazorApp1.App).Assembly)
                .Select(config => new object[] { config });

        [Theory, Trait("Category", "PreRender")]
        [MemberData(nameof(GetPagesToPreRender))]
        public async Task Render(string route)
        {
            // strip the initial / off
            var renderPath = route.Substring(1);

            var relativePath = Path.Combine(_outputPath, renderPath);
            var outputDirectory = Path.GetFullPath(relativePath);

            _output.WriteLine($"Creating directory '{outputDirectory}'");
            Directory.CreateDirectory(outputDirectory);

            var fileName = Path.Combine(outputDirectory, "index.html");

            _output.WriteLine($"Fetching prerendered content for '{route}'");
            var result = await _client.GetStreamAsync(route);

            _output.WriteLine($"Writing content to '{fileName}'");
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await result.CopyToAsync(file);
            }

            _output.WriteLine($"Pre rendering complete");

        }
    }
}
