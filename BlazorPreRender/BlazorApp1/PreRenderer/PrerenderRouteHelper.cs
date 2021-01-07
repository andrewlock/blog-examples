using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PreRenderer
{
    public static class PrerenderRouteHelper
    {

        /// <summary>
        /// Creates a <see cref="PrerenderRouteHelper"/> from the <see cref="RouteAttribute"/>s
        /// decorating components in the provided assembly
        /// </summary>
        public static List<string> GetRoutes(Assembly assembly)
        {
            // Get all the components whose base class is ComponentBase
            var components = assembly
                .ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(ComponentBase)));

            return components
                .Select(component => GetRouteFromComponent(component))
                .Where(config => config is not null)
                .ToList();
        }

        private static string GetRouteFromComponent(Type component)
        {
            var attributes = component.GetCustomAttributes(inherit: true);

            var routeAttribute = attributes.OfType<RouteAttribute>().FirstOrDefault();

            if (routeAttribute is null)
            {
                // Only map routable components
                return null;
            }

            var route = routeAttribute.Template;

            if (string.IsNullOrEmpty(route))
            {
                throw new Exception($"RouteAttribute in component '{component}' has empty route template");
            }

            // Don't support tokens at this point
            if (route.Contains('{'))
            {
                throw new Exception($"RouteAttribute for component '{component}' contains route values. Route values are invalid for prerendering");
            }

            return route;
        }
    }
}
