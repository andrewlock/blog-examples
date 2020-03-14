using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace RouteValuesInEndpointRouting
{
    public static class RandomNumberRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapRandomNumberEndpoint(
            this IEndpointRouteBuilder endpoints, string pattern)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<RandomNumberMiddleware>()
                .Build();

            return endpoints.Map(pattern, pipeline).WithDisplayName("Random Number");
        }
    }

}
