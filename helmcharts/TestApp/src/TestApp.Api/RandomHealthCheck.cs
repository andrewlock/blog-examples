using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TestApp.Api
{
    public class RandomHealthCheck : IHealthCheck
    {
        private static readonly Random _rnd = new Random();

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var result = _rnd.Next(5) == 0
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy("Failed random");

            return Task.FromResult(result);
        }
    }
}
