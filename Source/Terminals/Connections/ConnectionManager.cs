using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Rdp;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.Web;
using Terminals.Data;
using Terminals.Integration.Export;

namespace Terminals.Connections
{
    internal class ConnectionManager : IConnectionManager
    {
        private readonly IConnectionPlugin dummyPlugin = new DummyPlugin();

        private readonly Dictionary<string, IConnectionPlugin> plugins;

        #region Thread safe singleton with lazy loading

        /// <summary>
        /// Gets the thread safe singleton instance. Use only for startup procedure, will removed in the future.
        /// </summary>
        public static ConnectionManager Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private static class Nested
        {
            internal static readonly ConnectionManager instance = new ConnectionManager();
        }

        #endregion

        public ConnectionManager(): this(LoadPlugins)
        {
        }

        private static Dictionary<string, IConnectionPlugin> LoadPlugins()
        {
            // RAS, // this protocol doesnt fit to the concept and seems to be broken 
            // todo how to sort the plugins, so the VNC preceeds the VMRC to let the GetPortName works.
            var plugins = new Dictionary<string, IConnectionPlugin>()
            {
                {KnownConnectionConstants.RDP, new RdpConnectionPlugin()},
                {KnownConnectionConstants.HTTP, new HttpConnectionPlugin()},
                {KnownConnectionConstants.HTTPS, new HttpsConnectionPlugin()},
                //{ VncConnectionPlugin.VNC, new VncConnectionPlugin() },
                //{ VmrcConnectionPlugin.VMRC, new VmrcConnectionPlugin() },
                {TelnetConnectionPlugin.TELNET, new TelnetConnectionPlugin()},
                {SshConnectionPlugin.SSH, new SshConnectionPlugin()},
                {ICAConnectionPlugin.ICA_CITRIX, new ICAConnectionPlugin()}
            };

            LoadExternalPlugins(plugins);

            plugins.Add(VmrcConnectionPlugin.VMRC, new VmrcConnectionPlugin());
            return plugins;
        }

        internal ConnectionManager(Func<Dictionary<string, IConnectionPlugin>> loadPlugins)
        {
            this.plugins = loadPlugins();
        }

        private static void LoadExternalPlugins(Dictionary<string, IConnectionPlugin> plugins)
        {
            var pluginLoader = new PluginsLoader();
            foreach (IConnectionPlugin loaded in pluginLoader.Load())
            {
                plugins.Add(loaded.PortName, loaded);
            }
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
            var plugin = this.plugins.Values.FirstOrDefault(p => PluginIsOnPort(port, p));
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

        public IToolbarExtender[] CreateToolbarExtensions(ICurrenctConnectionProvider provider)
        {
            return this.plugins.Values.OfType<IToolbarExtenderFactory>()
                .Select(p => p.CreateToolbarExtender(provider))
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

        public void ChangeProtocol(IFavorite favorite, string protocol)
        {
            ProtocolOptions options = this.UpdateProtocolPropertiesByProtocol(protocol, favorite.ProtocolProperties);
            favorite.ChangeProtocol(protocol, options);
        }
    }
}
