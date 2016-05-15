using System.Collections;
using System.Collections.Generic;

namespace AddingDefaultSecurityHeaders.Middleware
{
public class SecurityHeadersPolicy
{
    /// <summary>
    /// A dictionary of Header, Value pairs that should be added to all requests
    /// </summary>
    public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();

    /// <summary>
    /// A hashset of Headers that should be removed from all requests
    /// </summary>
    public ISet<string> RemoveHeaders { get; } = new HashSet<string>();
}
}