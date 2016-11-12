using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ConsoleApplication
{
    public class FooService : IFooService
    {
        private readonly ILogger<FooService> _logger;
        public FooService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FooService>();
        }

        public void DoThing(int number)
        {
            _logger.LogInformation($"Doing the thing {number}");
        }
    }

}
