using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseMultiTenancyWithSaasKit.Models;
using Microsoft.AspNetCore.Http;
using SaasKit.Multitenancy;

namespace DatabaseMultiTenancyWithSaasKit.Services
{
    public class AppTenantResolver : ITenantResolver<AppTenant>
    {
        private readonly ApplicationDbContext _dbContext;

        public AppTenantResolver(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TenantContext<AppTenant>> ResolveAsync(HttpContext context)
        {
            TenantContext<AppTenant> tenantContext = null;

            var tenant = _dbContext.AppTenants.FirstOrDefault(
                t => t.Hostname.Equals(context.Request.Host.Value.ToLower()));

            if (tenant != null)
            {
                tenantContext = new TenantContext<AppTenant>(tenant);
            }

            return Task.FromResult(tenantContext);
        }
    }
}
