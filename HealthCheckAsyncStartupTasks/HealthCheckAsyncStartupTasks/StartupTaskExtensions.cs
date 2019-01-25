using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCheckAsyncStartupTasks
{
    public static class StartupTaskExtensions
    {
        private static readonly StartupTaskContext _sharedContext = new StartupTaskContext();
        public static IServiceCollection AddStartupTasks(this IServiceCollection services)
        {
            // Add the singleton StartupTaskContext only once
            if (services.Any(x => x.ServiceType != typeof(StartupTaskContext)))
            {
                services.AddSingleton(_sharedContext);
            }

            return services;
        }

        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask
        {
            _sharedContext.RegisterTask();
            return services
                .AddStartupTasks() // in case AddStartupTasks() hasn't been called
                .AddHostedService<T>();
        }
    }
}