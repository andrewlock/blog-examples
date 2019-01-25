using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HealthCheckAsyncStartupTasks
{
    public class DelayStartupTask : BackgroundService, IStartupTask
    {
        private readonly StartupTaskContext _startupTaskContext;

        public DelayStartupTask(StartupTaskContext startupTaskContext)
        {
            _startupTaskContext = startupTaskContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(10_000, stoppingToken);
            _startupTaskContext.MarkTaskAsComplete();
        }
    }
}