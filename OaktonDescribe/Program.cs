using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oakton;

namespace OaktonDescribe
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;
            return CreateHostBuilder(args)
                .RunOaktonCommands(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
