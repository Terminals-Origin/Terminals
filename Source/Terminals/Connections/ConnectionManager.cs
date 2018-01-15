using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Integration.Export;

namespace Terminals.Connections
{
    internal class ConnectionManager : IConnectionManager
    {
        private readonly IConnectionPlugin dummyPlugin = new DummyPlugin();

        private readonly Dictionary<string, IConnectionPlugin> plugins;

        internal ConnectionManager(IPluginsLoader loader)
        {
            // RAS, // this protocol doesnt fit to the concept and seems to be broken 
            IEnumerable<IConnectionPlugin> loaded = loader.Load();
            this.plugins = SortExternalPlugins(loaded);
        }

        private static Dictionary<string, IConnectionPlugin> SortExternalPlugins(IEnumerable<IConnectionPlugin> plugins)
        {
           var sortedPlugins = new Dictionary<string, IConnectionPlugin>();

            foreach (IConnectionPlugin loaded in plugins)
            {
                SortPlugin(sortedPlugins, loaded);
            }

            return sortedPlugins;
        }

        private static void SortPlugin(Dictionary<string, IConnectionPlugin> sortedPlugins, IConnectionPlugin loaded)
        {
            if (sortedPlugins.ContainsKey(loaded.PortName))
                LogDuplicitPlugin(loaded);
            else
                sortedPlugins.Add(loaded.PortName, loaded);
        }

        private static void LogDuplicitPlugin(IConnectionPlugin loaded)
        {
            const string MESSAGE_FORMAT = "Plugin for protocol {0} ({1}:{2}) already present.";
            Type pluginType = loaded.GetType();
            string assemblyPath = pluginType.Assembly.CodeBase;
            string message = string.Format(MESSAGE_FORMAT, loaded.PortName, pluginType, assemblyPath);
            Logging.Warn(message);
        }

        internal Dictionary<string, Image> GetPluginIcons()
        {
            return this.plugins.Values.ToDictionary(p => p.PortName, p => p.GetIcon());
        }

        /// <summary>
        /// Explicit call of update properties container depending on selected protocol.
        /// Don't call this in property setter, because of serializer.
        /// Returns never null instance of the options, in case the protocol is identical, returns currentOptions.
        /// </summary>
        internal ProtocolOptions UpdateProtocolPropertiesByProtocol(string newProtocol, ProtocolOptions currentOptions)
        {
            IConnectionPlugin plugin = FindPlugin(newProtocol);
            return SwitchPropertiesIfNotTheSameType(currentOptions, plugin);
        }

        private static ProtocolOptions SwitchPropertiesIfNotTheSameType(ProtocolOptions currentOptions, IConnectionPlugin plugin)
        {
            // prevent to reset properties
            if (currentOptions == null || currentOptions.GetType() != plugin.GetOptionsType()) 
                return plugin.CreateOptions();

            return currentOptions;
        }

        internal ushort[] SupportedPorts()
        {
            return this.plugins.Values.Where(p => !this.IsProtocolWebBased(p.PortName))
                .Select(p => Convert.ToUInt16(p.Port))
                .Distinct()
                .ToArray();
        }

        internal Connection CreateConnection(IFavorite favorite)
        {
            IConnectionPlugin plugin = FindPlugin(favorite.Protocol);
            return plugin.CreateConnection();
        }

        internal int GetPort(string name)
        {
            IConnectionPlugin plugin = FindPlugin(name);
            return plugin.Port;
        }

        /// <summary>
        /// Returns at least one pluging representing port.
        /// </summary>
        public IEnumerable<IConnectionPlugin> GetPluginsByPort(int port)
        {
            var resolvedPlugins = this.plugins.Values.Where(p => PluginIsOnPort(port, p))
                .ToList();

            if (resolvedPlugins.Count > 0)
                return resolvedPlugins;

            return new List<IConnectionPlugin>() { this.dummyPlugin};
        }

        /// <summary>
        /// Resolves first service from known plugins assigned to requested port.
        /// Returns RDP as default service.
        /// </summary>
        internal string GetPortName(int port)
        {
            // hack to let the VNC take precedence over the VMRC
            var plugin = this.plugins.Values.OrderBy(p => p.PortName.Length)
                .FirstOrDefault(p => PluginIsOnPort(port, p));

            if (plugin != null)
                return plugin.PortName;

            return this.dummyPlugin.PortName;
        }

        private static bool PluginIsOnPort(int port, IConnectionPlugin plugin)
        {
            return plugin.Port == port;
        }

        /// <summary>
        /// Ensures web based protocol shortcut. Returns true in case of HTTP or HTTPS.
        /// </summary>
        /// <param name="protocol">One of connection short cuts.</param>
        internal bool IsProtocolWebBased(string protocol)
        {
            return protocol == KnownConnectionConstants.HTTP || protocol == KnownConnectionConstants.HTTPS;
        }

        internal bool IsKnownProtocol(string protocol)
        {
            return this.plugins.Any(p => p.Key == protocol);
        }

        internal Control[] CreateControls(string newProtocol)
        {
            IConnectionPlugin plugin = FindPlugin(newProtocol);
            return plugin.CreateOptionsControls();
        }

        private IConnectionPlugin FindPlugin(string protocolName)
        {
            IConnectionPlugin plugin;
            if (this.plugins.TryGetValue(protocolName, out plugin))
                return plugin;

            return this.dummyPlugin;
        }

        public string[] GetAvailableProtocols()
        {
            return this.plugins.Values.Select(p => p.PortName)
                .ToArray();
        }

        internal ITerminalsOptionsExport[] GetTerminalsOptionsExporters()
        {
            return this.plugins.Values.OfType<IOptionsExporterFactory>()
                .Select(p => p.CreateOptionsExporter())
                .ToArray();
        }

        public Type[] GetAllKnownProtocolOptionTypes()
        {
            List<Type> knownTypes = this.plugins.Values
                .Select(p => p.GetOptionsType())
                .ToList();

            knownTypes.Add(typeof(EmptyOptions));
            return knownTypes.Distinct()
                .ToArray();
        }

        public IOptionsConverterFactory GetOptionsConverterFactory(string protocolName)
        {
            var protocolPlugin = this.FindPlugin(protocolName) as IOptionsConverterFactory;
            if (protocolPlugin == null)
                protocolPlugin = this.dummyPlugin as IOptionsConverterFactory;

            return protocolPlugin;
        }

        internal void SetDefaultProtocol(IFavorite favorite)
        {
            string defaultProtocol = KnownConnectionConstants.RDP;
            var available = this.GetAvailableProtocols();

            if (!available.Contains(defaultProtocol))
                defaultProtocol = available.First();

            this.ChangeProtocol(favorite, defaultProtocol);
        }

        public void ChangeProtocol(IFavorite favorite, string protocol)
        {
            ProtocolOptions options = this.UpdateProtocolPropertiesByProtocol(protocol, favorite.ProtocolProperties);
            favorite.ChangeProtocol(protocol, options);
        }

        public IToolbarExtender[] CreateToolbarExtensions(ICurrenctConnectionProvider provider)
        {
            return this.plugins.Values.OfType<IToolbarExtenderFactory>()
                .Select(p => p.CreateToolbarExtender(provider))
                .ToArray();
        }

        public IMenuExtender[] CreateMenuExtenders()
        {
            return this.plugins.Values.OfType<IMenuExtenderFactory>()
                .Select(p => p.CreateMenuExtender())
                .ToArray();
        }

        public override string ToString()
        {
            string[] loadedProtocols = this.GetAvailableProtocols();
            string pluginsLabel = string.Join(",", loadedProtocols);
            return string.Format("ConnectionManager:{0}", pluginsLabel);
        }
    }
}
