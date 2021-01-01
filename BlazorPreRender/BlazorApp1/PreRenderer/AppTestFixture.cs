using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Xunit.Abstractions;

namespace PreRenderer
{
    public class AppTestFixture : WebApplicationFactory<BlazorApp1.Server.Program>
    {
        public ITestOutputHelper Output { get; set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();
            builder.UseEnvironment(Environments.Production);
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddXUnit(Output);
            });

            builder.ConfigureWebHost(
                webHostBuilder =>
                {
                    webHostBuilder.UseStaticWebAssets();
                    webHostBuilder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton(_ => CreateDefaultClient());
                    });
                });
            return builder;
        }
    }
}
