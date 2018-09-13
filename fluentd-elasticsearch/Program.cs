using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

namespace fluentd_elasticsearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog((ctx, config) =>
            {
                config
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext();

                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    config.WriteTo.Console();
                }
                else
                {
                    config.WriteTo.Console(new ElasticsearchJsonFormatter());
                }

            })
                .UseStartup<Startup>();
    }
}
