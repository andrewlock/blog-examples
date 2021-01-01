using System.Collections.Generic;

namespace PreRenderer
{
    public static class PrerenderRoutes
    {
        public static List<string> Values { get; } = new()
        {
            "/",
            "/counter",
            "/fetchdata",
        };
    }
}
