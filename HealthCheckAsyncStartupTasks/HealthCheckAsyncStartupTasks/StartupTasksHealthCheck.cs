using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckAsyncStartupTasks
{
    public class StartupTasksHealthCheck : IHealthCheck
    {
        private readonly StartupTaskContext _context;

        public StartupTasksHealthCheck(StartupTaskContext context)
        {
            _context = context;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_context.IsComplete)
            {
                return Task.FromResult(HealthCheckResult.Healthy("All startup tasks complete"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Startup tasks not complete"));
        }
    }
}