using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ScopedHandlers
{
    public class ScopedService
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
    }
}
