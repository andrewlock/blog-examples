using Microsoft.EntityFrameworkCore;

namespace DatabaseMultiTenancyWithSaasKit.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppTenant> AppTenants { get; set; }
    }
}
