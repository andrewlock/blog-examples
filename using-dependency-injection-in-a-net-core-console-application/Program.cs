using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using StructureMap;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //    Uncomment to use the built in container
            //    var serviceProvider = new ServiceCollection()
            //        .AddLogging()
            //        .AddSingleton<IFooService, FooService>()
            //        .AddSingleton<BarService>()
            //        .BuildServiceProvider();

            //add the framework services
            var services = new ServiceCollection()
                .AddLogging();

            //add structuremp
            var container = new Container();
            container.Configure(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.Scan(_ =>
                            {
                                _.AssemblyContainingType(typeof(Program));
                                _.WithDefaultConventions();
                            });
                //Populate the container using the service collection
                config.Populate(services);
            });

            var serviceProvider = container.GetInstance<IServiceProvider>();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            //do the hard work here
            var bar = serviceProvider.GetService<IBarService>();

            bar.DoSomeRealWork();

            logger.LogDebug("All done!");

        }
    }
}
