using System.Collections.Generic;

namespace Terminals.Connections
{
    internal interface IPluginsLoader
    {
        IEnumerable<PluginDefinition> FindAvailablePlugins();

        IEnumerable<IConnectionPlugin> Load();
    }
}