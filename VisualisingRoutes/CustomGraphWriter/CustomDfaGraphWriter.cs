using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Routing;
using ImpromptuInterface;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.Options;

namespace CustomGraphWriter
{
    public partial class CustomDfaGraphWriter
    {
        private readonly IServiceProvider _services;
        private readonly GraphDisplayOptions _options;

        public CustomDfaGraphWriter(IServiceProvider services, GraphDisplayOptions options)
        {
            _services = services;
            _options = options;
        }

        public void Write(EndpointDataSource dataSource, TextWriter writer)
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

            var rawTree = builder.BuildDfaTree(includeLabel: true);
            IDfaNode tree = rawTree.ActLike<IDfaNode>();

            writer.WriteLine("digraph DFA {");
            Visit(tree, WriteNode);
            writer.WriteLine("}");

            void WriteNode(IDfaNode node)
            {
                if (!visited.TryGetValue(node, out var label))
                {
                    label = visited.Count;
                    visited.Add(node, label);
                }

                // We can safely index into visited because this is a post-order traversal,
                // all of the children of this node are already in the dictionary.

                if (node.Literals != null)
                {
                    foreach (DictionaryEntry dictEntry in node.Literals)
                    {
                        var key = (string)dictEntry.Key;
                        IDfaNode value = dictEntry.Value.ActLike<IDfaNode>();
                        writer.WriteLine($"{label} -> {visited[value]} [label=\"/{key}\" {_options.LiteralEdge}]");
                    }
                }

                if (node.Parameters != null)
                {
                    IDfaNode parameters = node.Parameters.ActLike<IDfaNode>();
                    writer.WriteLine($"{label} -> {visited[parameters]} [label=\"/*\" {_options.ParametersEdge}]");
                }

                if (node.CatchAll != null && node.Parameters != node.CatchAll)
                {
                    IDfaNode catchAll = node.CatchAll.ActLike<IDfaNode>();
                    writer.WriteLine($"{label} -> {visited[catchAll]} [label=\"/**\" {_options.CatchAllEdge}]");
                }

                if (node.PolicyEdges != null)
                {
                    foreach (DictionaryEntry dictEntry in node.PolicyEdges)
                    {
                        var key = (object)dictEntry.Key;
                        IDfaNode value = dictEntry.Value.ActLike<IDfaNode>();
                        writer.WriteLine($"{label} -> {visited[value]} [label=\"{key}\" {_options.PolicyEdge}]");
                    }
                }

                var matchCount = node?.Matches?.Count ?? 0;
                var extras = matchCount > 0 ? _options.MatchingNode : _options.DefaultNode;
                writer.WriteLine($"{label} [label=\"{node.Label}\" {extras}]");
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
