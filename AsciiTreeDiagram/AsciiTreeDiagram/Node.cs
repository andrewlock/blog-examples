using System.Collections.Generic;

namespace AsciiTreeDiagram
{
    class Node
    {
        public string Name { get; set; }

        public List<Node> Children { get; } = new List<Node>();
    }
}
