// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiExplorerWebSite
{
    public class Startup
    {
        // Set up application services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILoggerFactory, LoggerFactory>();
            services.AddMvc();

            services.AddSingleton<ApiExplorerDataFilter>();
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Documentation}/{action=Index}");
            });
        }
    }
}

