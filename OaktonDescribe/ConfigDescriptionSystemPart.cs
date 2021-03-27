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
                    (string Value, IConfigurationProvider Provider) valueAndProvider = GetValueAndProvider(_configRoot, child.Path);
        
                    IHasTreeNodes parent = node;
                    if (valueAndProvider.Provider != null)
                    {
                            node.AddNode(new Table()
                                .Border(TableBorder.None)
                                .HideHeaders()
                                .AddColumn("Key")
                                .AddColumn("Value")
                                .AddColumn("Provider")
                                .HideHeaders()
                                .AddRow($"[yellow]{child.Key}[/]", valueAndProvider.Value, $@"([grey]{valueAndProvider.Provider}[/])")
                            );
                    }
                    else
                    {
                        parent = node.AddNode($"[yellow]{child.Key}[/]");
                    }
        
                    RecurseChildren(parent, child.GetChildren());
                }
            }

            var tree = new Tree(string.Empty);
        
            RecurseChildren(tree, _configRoot.GetChildren());

            AnsiConsole.Render(tree);
        
            return Task.CompletedTask;
        }
        
        private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(
            IConfigurationRoot root,
            string key)
        {
            foreach (IConfigurationProvider provider in root.Providers.Reverse())
            {
                if (provider.TryGet(key, out string value))
                {
                    return (value, provider);
                }
            }

            return (null, null);
        }
    }
}