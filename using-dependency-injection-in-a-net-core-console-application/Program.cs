using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IFooService, FooService>()
                .AddSingleton<BarService>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogDebug("Starting application");

            //do the hard work here
            var bar = serviceProvider.GetService<BarService>();

            bar.DoSomeRealWork();

            logger.LogDebug("All done!");

        }
    }
}
