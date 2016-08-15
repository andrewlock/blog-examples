using System.Linq;
using DatabaseMultiTenancyWithSaasKit.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DatabaseMultiTenancyWithSaasKit
{
    public class ConfigureMultitenancyOptions : IConfigureOptions<MultitenancyOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ConfigureMultitenancyOptions(IServiceScopeFactory serivceScopeFactory)
        {
            _serviceScopeFactory = serivceScopeFactory;
        }

        public void Configure(MultitenancyOptions options)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var dbContext = provider.GetRequiredService<ApplicationDbContext>())
                {
                    options.AppTenants = dbContext.AppTenants.ToList();
                }
            }
        }
    }
}