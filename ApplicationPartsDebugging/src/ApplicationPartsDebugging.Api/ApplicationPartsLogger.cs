using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApplicationPartsDebugging.Api
{
    public class ApplicationPartsLogger : IHostedService
    {
        private readonly ILogger<ApplicationPartsLogger> _logger;
        private readonly ApplicationPartManager _partManager;

        public ApplicationPartsLogger(ILogger<ApplicationPartsLogger> logger, ApplicationPartManager partManager)
        {
            _logger = logger;
            _partManager = partManager;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var applicationParts = _partManager.ApplicationParts.Select(x => x.Name);

            var controllerFeature = new ControllerFeature();
            _partManager.PopulateFeature(controllerFeature);
            var controllers = controllerFeature.Controllers.Select(x => x.Name);

            _logger.LogInformation("Found the following application parts: '{ApplicationParts}', with the following controllers: '{Controllers}'",
                string.Join(", ", applicationParts), string.Join(", ", controllers));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
