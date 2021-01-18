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
    }
}