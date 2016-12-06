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
            string[] enabledPlugins = this.settings.EnabledPlugins;
            bool isEnabled = enabledPlugins.Contains(plugin.FullPath);
            return new SelectedPlugin(plugin.Description, plugin.FullPath, isEnabled);
        }

        internal void SaveSelected(List<SelectedPlugin> allPlugins)
        {
            string[] enabledPlugins = SelectEnabledPluginPaths(allPlugins);
            this.settings.EnabledPlugins = enabledPlugins;
        }

        private static string[] SelectEnabledPluginPaths(List<SelectedPlugin> allPlugins)
        {
            return allPlugins.Where(p => p.Enabled)
                .Select(p => p.FullPath)
                .ToArray();
        }
    }
}