using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExampleApp
{
    public class LoggingHostedService : IHostedService
    {
        readonly ILogger<LoggingHostedService> _logger;

        public LoggingHostedService(ILogger<LoggingHostedService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _logger.LogWarning("Starting service");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _logger.LogWarning("Stopping service");
        }
    }
}