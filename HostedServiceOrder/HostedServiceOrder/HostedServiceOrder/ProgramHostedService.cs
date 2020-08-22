using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedServiceOrder
{
    public class ProgramHostedService : IHostedService
    {
        private readonly ILogger _logger;

        public ProgramHostedService(ILogger<ProgramHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting IHostedService registered in Program.cs");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StoppingIHostedService registered in Program.cs");
            return Task.CompletedTask;
        }
    }
}
