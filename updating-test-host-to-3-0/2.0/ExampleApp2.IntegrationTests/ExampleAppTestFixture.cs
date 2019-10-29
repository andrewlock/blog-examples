using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ExampleApp2.IntegrationTests
{
    public class ExampleAppTestFixture : WebApplicationFactory<ExampleApp.Program>
    {
        public ITestOutputHelper Output { get; set; }
        
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = base.CreateWebHostBuilder();
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddXUnit(Output);
            });

            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices((services) =>
            {
                services.RemoveAll(typeof(IHostedService));
            });
        }

        protected override void Dispose(bool disposing)
        {
            // Have to disable logging here as occurs
            // outside a test context
            Output = null;
            base.Dispose(disposing);
        }
        
    }
}