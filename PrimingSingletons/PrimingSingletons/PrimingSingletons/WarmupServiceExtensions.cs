using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace PrimingSingletons
{
    public static class WarmupServiceExtensions
    {
        public static IServiceCollection AddWarmupService(this IServiceCollection services)
        {
            services
                .AddHostedService<WarmupService>()
                .TryAddSingleton(services);

            return services;
        }
    }
}