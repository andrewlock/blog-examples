using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RouteValuesInEndpointRouting
{

    public class RandomNumberMiddleware
    {
        private static readonly Random _random = new Random();

        public RandomNumberMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var maybeValues = ParseMaxMin(context);

            if (!maybeValues.HasValue)
            {
                context.Response.StatusCode = 400; //couldn't parse
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
            var routeValues = context.GetRouteData().Values;
            
            var minValue = routeValues["min"] as string;
            var maxValue = routeValues["max"] as string;

            if (!int.TryParse(minValue, out var min)) return null; // e.g. /random/blah/123
            if (!int.TryParse(maxValue, out var max)) return null; // e.g. /random/123/blah

            return (min, max);
        }
    }

}
