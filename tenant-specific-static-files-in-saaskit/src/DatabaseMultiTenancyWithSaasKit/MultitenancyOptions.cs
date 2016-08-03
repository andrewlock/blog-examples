using System.Collections.Generic;
using DatabaseMultiTenancyWithSaasKit.Models;

namespace DatabaseMultiTenancyWithSaasKit.Services
{
    public class MultitenancyOptions
    {
        public List<AppTenant> Tenants { get; set; }
    }
}