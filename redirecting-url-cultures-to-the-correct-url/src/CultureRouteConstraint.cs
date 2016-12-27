using Microsoft.AspNetCore.Routing.Constraints;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    /// <summary>
    /// Represents a route constraint for service <see cref="CultureInfor">cultures</see>.
    /// </summary>
    public sealed class CultureRouteConstraint : RegexRouteConstraint
    {
        public CultureRouteConstraint() : base(@"^[a-zA-Z]{2}(\-[a-zA-Z]{2})?$") { }
    }
}