using System.Collections.Generic;
using DatabaseMultiTenancyWithSaasKit.Models;

namespace DatabaseMultiTenancyWithSaasKit
{
    public class MultitenancyOptions
    {
        public ICollection<AppTenant> AppTenants { get; set; } = new List<AppTenant>();
    }
}