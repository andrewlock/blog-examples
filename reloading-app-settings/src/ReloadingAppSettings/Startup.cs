using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReloadingAppSettings.Options;

namespace ReloadingAppSettings
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddSingleton<IConfigurationRoot>(Configuration);

            services.Configure<MyValues>(Configuration.GetSection("MyValues"), true);

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptionsMonitor<MyValues> monitor)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            monitor.OnChange(vals =>
                                {
                                    var logger = loggerFactory.CreateLogger<IOptionsMonitor<MyValues>>();
                                    logger.LogDebug($"Config changed: {string.Join(", ", vals)}");
                                });

            app.UseMvc();

            //app.UseMiddleware<TestMiddleware>();
        }

        public class TestMiddleware
        {
            private readonly RequestDelegate _next;
            public TestMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context, IOptionsMonitor<MyValues> values)
            {
                await context.Response.WriteAsync(values.CurrentValue.DefaultValue);
            }
        }
    }
}
