using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AsciiTreeDiagram
{
    class Program
    {
        // Constants for drawing lines and spaces
        private const string _cross = " ├─";
        private const string _corner = " └─";
        private const string _vertical = " │ ";
        private const string _space = "   ";

        static void Main(string[] args)
        {
            // Get the list of nodes
            List<Node> topLevelNodes = CreateNodeList();

            foreach (var node in topLevelNodes)
            { 
                PrintNode(node, indent: "");
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit...");
                Console.Read();
            }
        }

        static void PrintNode(Node node, string indent)
        {
            Console.WriteLine(node.Name);

            // Loop through the children recursively, passing in the
            // indent, and the isLast parameter
            var numberOfChildren = node.Children.Count;
            for (var i = 0; i < numberOfChildren; i++)
            {
                var child = node.Children[i];
                var isLast = (i == (numberOfChildren - 1));
                PrintChildNode(child, indent, isLast);
            }
        }

        static void PrintChildNode(Node node, string indent, bool isLast)
        {
            // Print the provided pipes/spaces indent
            Console.Write(indent);

            // Depending if this node is a last child, print the
            // corner or cross, and calculate the indent that will
            // be passed to its children
            if (isLast)
            {
                Console.Write(_corner);
                indent += _space;
            }
            else
            {
                Console.Write(_cross);
                indent += _vertical;
            }

            PrintNode(node, indent);
        }

        static void PrintNodeIncorrectly(Node node, int depth, bool isFirstChild, bool isLastChild, bool parentIsLastChild)
        {
            // something
        }

        static void PrintNodeButMakeHardWorkOfIt(Node node, bool isFirstChild, bool isLastChild, Node[] parentNodes)
        {
            // implementation
        }

        static List<Node> CreateNodeList()
        {
            return new List<Node>
            {
                new Node
                {
                    Name = "Default",
                    Children =
                    {
                        new Node
                        {
                            Name = "Package",
                            Children = {
                                new Node
                                {
                                    Name = "Zip-Files",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Copy-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },

                                new Node
                                {
                                    Name = "Create-Nuget-Packages",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Copy-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },

                new Node
                {
                    Name = "AppVeyor",
                    Children =
                    {
                        new Node
                        {
                            Name = "Upload-AppVeyor-Artifacts",
                            Children = {
                                new Node
                                {
                                    Name = "Sign-Binaries",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Zip-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },

                                new Node
                                {
                                    Name = "Create-Nuget-Packages",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Copy-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
