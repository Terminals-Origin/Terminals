using System.Collections.Generic;
using System.Linq;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.Forms.OptionPanels
{
    internal class PluginsSelection
    {
        private readonly IPluginSettings settings;

        private readonly IPluginsLoader loader;

        internal PluginsSelection(IPluginSettings settings, IPluginsLoader loader)
        {
            this.settings = settings;
            this.loader = loader;
        }

        internal IEnumerable<SelectedPlugin> LoadPlugins()
        {
            IEnumerable<PluginDefinition> allAvailable = this.loader.FindAvailablePlugins();
            return allAvailable.Select(this.ToSelectedPlugin)
                .ToList();
        }

        private SelectedPlugin ToSelectedPlugin(PluginDefinition plugin)
        {
            string[] disabledPlugins = this.settings.DisabledPlugins;
            bool isEnabled = !disabledPlugins.Contains(plugin.FullPath);
            return new SelectedPlugin(plugin.Description, plugin.FullPath, isEnabled);
        }

        internal void SaveSelected(List<SelectedPlugin> allPlugins)
        {
            string[] disabledPlugins = SelectDisabledPluginPaths(allPlugins);
            this.settings.DisabledPlugins = disabledPlugins;
        }

        private static string[] SelectDisabledPluginPaths(List<SelectedPlugin> allPlugins)
        {
            return allPlugins.Where(p => !p.Enabled)
                .Select(p => p.FullPath)
                .ToArray();
        }
    }
}