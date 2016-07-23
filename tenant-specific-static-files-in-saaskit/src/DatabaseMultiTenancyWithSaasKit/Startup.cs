using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseMultiTenancyWithSaasKit.Models;
using DatabaseMultiTenancyWithSaasKit.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatabaseMultiTenancyWithSaasKit
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
            // Add framework services.
            services.AddMvc();

            services.AddMultitenancy<AppTenant, AppTenantResolver>();

            services.Configure<MultitenancyOptions>(Configuration.GetSection("Multitenancy"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMultitenancy<AppTenant>();

            app.UsePerTenant<AppTenant>((ctx, builder) =>
            {
                bool isTenantSpecificPath = false;
                const string prefix = "/tenant";
                builder.Use(next =>
                {
                    return async httpContext =>
                    {
                        var originalPath = httpContext.Request.Path;
                        isTenantSpecificPath = httpContext.Request.Path.StartsWithSegments(new PathString(prefix));
                        if (isTenantSpecificPath)
                        {
                            var previousPath = httpContext.Request.Path.ToUriComponent()
                                .Substring(prefix.Length);

                            var newPath = new PathString($"/tenant/{ctx.Tenant.Tag}{previousPath}");
                            httpContext.Request.Path = newPath;
                        }
                        await next(httpContext);

                        //replace the original url after the remaing middleware has finished processing
                        httpContext.Request.Path = originalPath;
                    };
                });
                //render static files
                builder.UseStaticFiles();
                builder.Use(next =>
                {
                    return async httpContext =>
                    {
                        if (isTenantSpecificPath)
                        {
                            httpContext.Response.StatusCode = 404;
                        }
                        else
                        {
                            await next(httpContext);
                        }
                    };
                });
            });



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
