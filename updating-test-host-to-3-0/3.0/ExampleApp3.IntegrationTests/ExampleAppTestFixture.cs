using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ExampleApp3.IntegrationTests
{
    public class ExampleAppTestFixture : WebApplicationFactory<ExampleApp.Program>
    {
        public ITestOutputHelper Output { get; set; }
        
        // This won't be called because we're using the generic host
//        protected override IWebHostBuilder CreateWebHostBuilder()
//        {
//            var builder = base.CreateWebHostBuilder();
//            builder.ConfigureLogging(logging =>
//            {
//                logging.ClearProviders();
//                logging.AddXUnit(Output);
//            });
//
//            return builder;
//        }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder =  base.CreateHostBuilder();
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
    }
}