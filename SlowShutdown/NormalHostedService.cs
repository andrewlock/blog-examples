using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SlowShutdown
{
    public class NormalHostedService : IHostedService
    {
        readonly ILogger<NormalHostedService> _logger;

        public NormalHostedService(ILogger<NormalHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NormalHostedService started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NormalHostedService stopped");
            return Task.CompletedTask;
        }
    }
}