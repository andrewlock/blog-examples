using System;
using System.Collections.Generic;

namespace AsciiTreeDiagram
{
    class Program
    {
        private const string _cross = "├─";
        private const string _corner = "└─";
        private const string _vertical = "│ ";
        private const string _space = "  ";

        static void Main(string[] args)
        {
            List<Node> topLevelNodes = CreateNodeList();

            foreach (var node in topLevelNodes)
            {
                PrintNode(node, "", isLast: false, isTopLevel: true);
            }
        }

        static void PrintNode(Node node, string indent, bool isLast, bool isTopLevel)
        {
            Console.Write(indent);
            if (isLast)
            {
                Console.Write(_corner);
                indent += _space;
            }
            else if (!isTopLevel)
            {
                Console.Write(_cross);
                indent += _vertical;
            }

            Console.WriteLine(node.Name);

            for (var i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                PrintNode(child, indent, isLast: i == (node.Children.Count - 1), isTopLevel: false);
            }

        }

        static void PrintNodeIncorrectly(Node node, int depth, bool isFirstChild, bool isLastChild, bool parentIsLastChild)
        {
            // something
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
