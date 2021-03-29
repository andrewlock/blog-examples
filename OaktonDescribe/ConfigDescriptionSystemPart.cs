using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oakton.Descriptions;
using Spectre.Console;

namespace OaktonDescribe
{
    public class ConfigDescriptionSystemPart: IDescribedSystemPart, IWriteToConsole
    {
        readonly IConfigurationRoot _configRoot;

        public ConfigDescriptionSystemPart(IConfiguration config)
        {
            _configRoot = config as IConfigurationRoot;
        }

        public string Title => "Configuration values and sources";

        public Task Write(TextWriter writer)
        {
            return writer.WriteAsync(_configRoot.GetDebugView());
        }

        public Task WriteToConsole()
        {
            void RecurseChildren(IHasTreeNodes node, IEnumerable<IConfigurationSection> children)
            {
                foreach (IConfigurationSection child in children)
                {
                    var valuesAndProviders = GetValueAndProviders(_configRoot, child.Path);

                    IHasTreeNodes parent = node;
                    if (valuesAndProviders.Count == 0)
                    {
                        parent = node.AddNode($"[blue]{child.Key}[/]");
                    }
                    else
                    {
                        var current = valuesAndProviders.Pop();
                        var currentNode = node.AddNode(new Table()
                            .Border(TableBorder.None)
                            .HideHeaders()
                            .AddColumn("Key")
                            .AddColumn("Value")
                            .AddColumn("Provider")
                            .HideHeaders()
                            .AddRow($"[yellow]{child.Key}[/]", current.Value, $@"([grey]{current.Provider}[/])")
                        );
                    
                        // Add the overriden values
                        foreach (var valueAndProvider in valuesAndProviders)
                        {
                            currentNode.AddNode(new Table()
                                .Border(TableBorder.None)
                                .HideHeaders()
                                .AddColumn("Value")
                                .AddColumn("Provider")
                                .HideHeaders()
                                .AddRow($"[strikethrough]{valueAndProvider.Value}[/]", $@"([grey]{valueAndProvider.Provider}[/])")
                            );
                        }
                    }

                    RecurseChildren(parent, child.GetChildren());
                }
            }

            var tree = new Tree(string.Empty);
        
            RecurseChildren(tree, _configRoot.GetChildren());

            AnsiConsole.Render(tree);
        
            return Task.CompletedTask;
        }
        
        private static Stack<(string Value, IConfigurationProvider Provider)> GetValueAndProviders(
            IConfigurationRoot root,
            string key)
        {
            var stack = new Stack<(string, IConfigurationProvider)>();
            foreach (IConfigurationProvider provider in root.Providers)
            {
                if (provider.TryGet(key, out string value))
                {
                    stack.Push((value, provider));
                }
            }

            return stack;
        }
    }
}