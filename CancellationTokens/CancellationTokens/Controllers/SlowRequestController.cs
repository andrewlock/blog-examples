using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CancellationTokens.Controllers
{
    public class SlowRequestController : Controller
    {
        private readonly ILogger _logger;

        public SlowRequestController(ILogger<SlowRequestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/slowtest/{delayTimeInSeconds:int?}")]
        public async Task<string> Get(CancellationToken cancellationToken,
            int delayTimeInSeconds = 10)
        {
            _logger.LogInformation("Starting to do slow work");

            await Task.Delay(delayTimeInSeconds * 1000, cancellationToken);

            var message = $"Finished slow delay of {delayTimeInSeconds} seconds.";

            for (var i = 0; i < delayTimeInSeconds; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }

            _logger.LogInformation(message);

            return $"Finished slow delay of {delayTimeInSeconds * 2} seconds.";
        }
    }
}
