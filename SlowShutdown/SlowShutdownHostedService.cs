using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SlowShutdown
{
    public class SlowShutdownHostedService : IHostedService
    {
        readonly ILogger<SlowShutdownHostedService> _logger;

        public SlowShutdownHostedService(ILogger<SlowShutdownHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SlowShutdownHostedService started");
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SlowShutdownHostedService stopping...");
            await Task.Delay(10_000);
            _logger.LogInformation("SlowShutdownHostedService stopped");
        }
    }
}
