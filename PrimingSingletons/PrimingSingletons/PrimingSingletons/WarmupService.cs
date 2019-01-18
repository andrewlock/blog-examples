using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PrimingSingletons
{
    public class WarmupService : IHostedService
    {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _provider;

        public WarmupService(
            IServiceCollection services, 
            IServiceProvider provider)
        {
            _services = services;
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            InstantiteSingletons();
            InstantiteServices();
            return Task.CompletedTask;
        }

        private void InstantiteSingletons()
        {
            foreach (var singleton in GetSingletons(_services))
            {
                //_logger.LogDebug("Warming up {TypeName}", singleton.FullName);
                // may be registered more than once, so get all at once
                _provider.GetServices(singleton);
            }
        }
        private void InstantiteServices()
        {
            using (var scope = _provider.CreateScope())
            {
                foreach (var service in GetServices(_services))
                {
                    //_logger.LogDebug("Warming up {TypeName}", singleton.FullName);
                    // may be registered more than once, so get all at once
                    scope.ServiceProvider.GetServices(service);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        static IEnumerable<Type> GetSingletons(IServiceCollection services)
        {
            return services
                .Where(descriptor => descriptor.Lifetime == ServiceLifetime.Singleton
                                     && descriptor.ImplementationType != typeof(WarmupService))
                .Distinct()
                .Where(descriptor => descriptor.ServiceType.ContainsGenericParameters == false)
                .Select(descriptor => descriptor.ServiceType);
        }

        static IEnumerable<Type> GetServices(IServiceCollection services)
        {
            return services
                .Where(descriptor => descriptor.ImplementationType != typeof(WarmupService))
                .Distinct()
                .Where(descriptor => descriptor.ServiceType.ContainsGenericParameters == false)
                .Select(descriptor => descriptor.ServiceType);
        }
    }
}
