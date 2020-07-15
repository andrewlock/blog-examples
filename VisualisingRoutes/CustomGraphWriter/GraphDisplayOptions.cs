using System;
using System.Collections.Generic;
using System.Text;

namespace CustomGraphWriter
{
    public class GraphDisplayOptions
    {
        /// <summary>
        /// Additional display options for literal edges
        /// </summary>
        public string LiteralEdge { get; set; } = string.Empty;

        /// <summary>
        /// Additional display options for parameter edges
        /// </summary>
        public string ParametersEdge { get; set; } = "arrowhead=diamond color=\"blue\"";

        /// <summary>
        /// Additional display options for catchall parameter edges
        /// </summary>
        public string CatchAllEdge { get; set; } = "arrowhead=odot color=\"green\"";

        /// <summary>
        /// Additional display options for policy edges
        /// </summary>
        public string PolicyEdge { get; set; } = "color=\"red\" style=dashed arrowhead=open";

        /// <summary>
        /// Additional display options for node which contains matches
        /// </summary>
        public string MatchingNode { get; set; } = "shape=box style=filled color=\"brown\" fontcolor=\"white\"";

        /// <summary>
        /// Additional display options for node without matches
        /// </summary>
        public string DefaultNode { get; set; } = string.Empty;


    }
}
