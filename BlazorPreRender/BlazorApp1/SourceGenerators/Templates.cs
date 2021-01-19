using System.Collections.Generic;
using System.Text;

namespace SourceGenerators
{
    internal static class Templates
    {
        public static string AppRoutes(IEnumerable<string> allRoutes)
        {
            // hard code the namespace for now
            var sb = new StringBuilder(@"
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlazorApp1
{
    public static class AppRoutes
    {
        public static ReadOnlyCollection<string> Routes { get; } = 
            new ReadOnlyCollection<string>(
                new List<string>
                {
");
            foreach (var route in allRoutes)
            {
                sb.AppendLine($"\"{route}\",");
            }

            sb.Append(@"
                }
            );
    }
}");
            return sb.ToString();
        }

        public static string PageDetail()
        {
            return @"
namespace BlazorApp1
{
    public record PageDetail(string Route, string Title);
}
";
        }

        public static string MenuPages(IEnumerable<RouteableComponent> pages)
        {
            // hard code the namespace for now
            var sb = new StringBuilder(@"
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlazorApp1
{
    public static class PageDetails
    {
        public static ReadOnlyCollection<PageDetail> MenuPages { get; } = 
            new ReadOnlyCollection<PageDetail>(
                new List<PageDetail>
                {
");
            foreach (var page in pages)
            {
                sb.AppendLine($"new PageDetail(\"{page.Route}\", \"{page.Title}\"),");
            }

            sb.Append(@"
                }
            );
    }
}");
            return sb.ToString();
        }
    }
}