using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace InspectMiddleware
{
    public class ConditionalMiddlewareStartupFilter : IStartupFilter
    {
        private readonly string _runBefore;
        public ConditionalMiddlewareStartupFilter(string runBefore)
        {
            _runBefore = runBefore;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                var wrappedBuilder = new ConditionalMiddlewareBuilder(builder, _runBefore);
                next(wrappedBuilder);
            };
        }
    }
}