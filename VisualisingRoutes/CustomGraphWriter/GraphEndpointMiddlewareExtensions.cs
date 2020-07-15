using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CustomGraphWriter
{
    public static class GraphEndpointMiddlewareExtensions
    {
        /// <summary>
        /// Visualize at https://dreampuf.github.io/GraphvizOnline
        /// </summary>
        public static IEndpointConventionBuilder MapGraphVisualisation(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var pipeline = endpoints
                .CreateApplicationBuilder()
                .UseMiddleware<GraphEndpointMiddleware>()
                .Build();

            return endpoints.Map(pattern, pipeline).WithDisplayName("Endpoint Graph");
        }
    }
}
