using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RouteValuesInEndpointRouting
{
    public class LegacyRandomMiddleware
    {
        private static readonly Random _random = new Random();

        public LegacyRandomMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var maybeValues = ParseMaxMin(context);

            if (!maybeValues.HasValue)
            {
                context.Response.StatusCode = 400; //couldn't parse.
                return;
            }

            var (min, max) = maybeValues.Value;
            var value = GetRandomValue(min, max);

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(value.ToString());
        }

        private static int GetRandomValue(int min, int max)
        {
            return min < max
                ? _random.Next(min, max)
                : _random.Next(max, min);
        }

        private static (int min, int max)? ParseMaxMin(HttpContext context)
        {
            var path = context.Request.Path;
            if (!path.HasValue) return null; // e.g. /random, /random/

            var segments = path.Value.Split('/');

            if (segments.Length != 3) return null; // e.g. /random/12, /random/blah, /random/123/12/tada
            System.Diagnostics.Debug.Assert(string.IsNullOrEmpty(segments[0])); // first segment is always empty
            if (!int.TryParse(segments[1], out var min)) return null; // e.g. /random/blah/123
            if (!int.TryParse(segments[2], out var max)) return null; // e.g. /random/123/blah

            return (min, max);
        }
    }

}
