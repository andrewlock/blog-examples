using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace CustomGraphWriter
{
    // https://github.com/dotnet/aspnetcore/blob/cc3d47f5501cdfae3e5b5be509ef2c0fb8cca069/src/Http/Routing/src/Matching/DfaNode.cs
    public interface IDfaNode
    {
        public string Label { get; set; }
        public List<Endpoint> Matches { get; }
        // Dictionary<string, IDfaNode>
        public IDictionary Literals { get; }
        public object Parameters { get; }
        public object CatchAll { get; }
        // Dictionary<object, IDfaNode>
        public IDictionary PolicyEdges { get; }
    }
}
