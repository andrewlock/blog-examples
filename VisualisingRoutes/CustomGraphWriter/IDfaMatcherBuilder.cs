
using Microsoft.AspNetCore.Routing;

namespace CustomGraphWriter
{
    public interface IDfaMatcherBuilder
    {
        void AddEndpoint(RouteEndpoint endpoint);
        object BuildDfaTree(bool includeLabel = false);
    }
}
