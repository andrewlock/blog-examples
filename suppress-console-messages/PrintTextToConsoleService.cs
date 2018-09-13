using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace suppress_console_messages
{
    public class PrintTextToConsoleService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public PrintTextToConsoleService(ILogger<PrintTextToConsoleService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");

            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5),
              TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation($"Background work with text: {DateTimeOffset.UtcNow}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
