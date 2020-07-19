using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Routing;
using ImpromptuInterface;
using System.Linq;

namespace CustomGraphWriter
{
    public class DuplicateEndpointDetector
    {
        private readonly IServiceProvider _services;

        public DuplicateEndpointDetector(IServiceProvider services)
        {
            _services = services;
        }

        public Dictionary<string, List<string>> GetDuplicateEndpoints(EndpointDataSource dataSource)
        {
            // get the DfaMatcherBuilder - internal, so needs reflection :(
            Type matcherBuilder = typeof(IEndpointSelectorPolicy).Assembly
                .GetType("Microsoft.AspNetCore.Routing.Matching.DfaMatcherBuilder");

            var rawBuilder = _services.GetRequiredService(matcherBuilder);
            IDfaMatcherBuilder builder = rawBuilder.ActLike<IDfaMatcherBuilder>();

            var endpoints = dataSource.Endpoints;
            for (var i = 0; i < endpoints.Count; i++)
            {
                if (endpoints[i] is RouteEndpoint endpoint && (endpoint.Metadata.GetMetadata<ISuppressMatchingMetadata>()?.SuppressMatching ?? false) == false)
                {
                    builder.AddEndpoint(endpoint);
                }
            }

            // Assign each node a sequential index.
            var visited = new Dictionary<IDfaNode, int>();
            var duplicates = new Dictionary<string, List<string>>();

            var rawTree = builder.BuildDfaTree(includeLabel: true);
            IDfaNode tree = rawTree.ActLike<IDfaNode>();

            Visit(tree, LogDuplicates);

            return duplicates;

            void LogDuplicates(IDfaNode node)
            {
                if (!visited.TryGetValue(node, out var label))
                {
                    label = visited.Count;
                    visited.Add(node, label);
                }

                // We can safely index into visited because this is a post-order traversal,
                // all of the children of this node are already in the dictionary.

                var matchCount = node?.Matches?.Count ?? 0;
                if(matchCount > 1)
                {
                    var duplicateEndpoints = node.Matches.Select(x => x.DisplayName).ToList();
                    duplicates[node.Label] = duplicateEndpoints;
                }
            }
        }

        static void Visit(IDfaNode node, Action<IDfaNode> visitor)
        {
            if (node.Literals?.Values != null)
            {
                foreach (var dictValue in node.Literals.Values)
                {
                    IDfaNode value = dictValue.ActLike<IDfaNode>();
                    Visit(value, visitor);
                }
            }

            // Break cycles
            if (node.Parameters != null && !ReferenceEquals(node, node.Parameters))
            {
                IDfaNode parameters = node.Parameters.ActLike<IDfaNode>();
                Visit(parameters, visitor);
            }

            // Break cycles
            if (node.CatchAll != null && !ReferenceEquals(node, node.CatchAll))
            {
                IDfaNode catchAll = node.CatchAll.ActLike<IDfaNode>();
                Visit(catchAll, visitor);
            }

            if (node.PolicyEdges?.Values != null)
            {
                foreach (var dictValue in node.PolicyEdges.Values)
                {
                    IDfaNode value = dictValue.ActLike<IDfaNode>();
                    Visit(value, visitor);
                }
            }

            visitor(node);
        }
    }
}
