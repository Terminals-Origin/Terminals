using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Connections.ICA;
using Terminals.Connections.Rdp;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Connections.Web;
using Terminals.Data;
using Terminals.Integration.Export;
using Terminals.Properties;

namespace Terminals.Connections
{
    internal class ConnectionManager
    {
        private readonly IConnectionPlugin vmrcPlugin = new VmrcConnectionPlugin();
        private readonly IConnectionPlugin rdpPlugin = new RdpConnectionPlugin();

        private readonly Dictionary<string, IConnectionPlugin> plugins;

        // cached images, bad performace, but simplifies check, if the image is known
        internal static readonly Image TreeIconRdp = Resources.treeIcon_rdp;
        internal static readonly Image TreeIconHttp = Resources.treeIcon_http;
        internal static readonly Image TreeIconVnc = Resources.treeIcon_vnc;
        internal static readonly Image TreeIconTelnet = Resources.treeIcon_telnet;
        internal static readonly Image TreeIconSsh = Resources.treeIcon_ssh;
        internal static readonly Image Terminalsicon = Resources.terminalsicon;

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

        public ConnectionManager()
        {
            // RAS, // this protocol doesnt fit to the concept and seems to be broken 
            this.plugins = new Dictionary<string, IConnectionPlugin>()
            {
                { KnownConnectionConstants.HTTP, new HttpConnectionPlugin() },
                { KnownConnectionConstants.HTTPS, new HttpsConnectionPlugin() },
                { VncConnectionPlugin.VNC, new VncConnectionPlugin() },
                { VmrcConnectionPlugin.VMRC, new VmrcConnectionPlugin() },
                { TelnetConnectionPlugin.TELNET, new TelnetConnectionPlugin() },
                { SshConnectionPlugin.SSH, new SshConnectionPlugin() },
                { ICAConnectionPlugin.ICA_CITRIX, new ICAConnectionPlugin() },
                { KnownConnectionConstants.RDP, this.rdpPlugin }
            };
        }

        internal Dictionary<string, Image> GetPluginIcons()
        {
            return new Dictionary<string, Image>()
            {
                {KnownConnectionConstants.RDP, TreeIconRdp },
                {VncConnectionPlugin.VNC, TreeIconVnc },
                {SshConnectionPlugin.SSH, TreeIconSsh },
                {TelnetConnectionPlugin.TELNET, TreeIconTelnet },
                {KnownConnectionConstants.HTTP, TreeIconHttp },
                {KnownConnectionConstants.HTTPS, TreeIconHttp }
            };
        }

        /// <summary>
        /// Explicit call of update properties container depending on selected protocol.
        /// Don't call this in property setter, because of serializer.
        /// Returns never null instance of the options, in case the protocol is identical, returns currentOptions.
        /// </summary>
        internal ProtocolOptions UpdateProtocolPropertiesByProtocol(string newProtocol, ProtocolOptions currentOptions)
        {
            IConnectionPlugin plugin = null;

            if (this.plugins.TryGetValue(newProtocol, out plugin))
                return SwitchPropertiesIfNotTheSameType(currentOptions, plugin);
            
            return new EmptyOptions();
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
            IConnectionPlugin plugin = null;
            if (this.plugins.TryGetValue(favorite.Protocol, out plugin))
                return plugin.CreateConnection();

            return this.rdpPlugin.CreateConnection();
        }

        internal int GetPort(string name)
        {
            IConnectionPlugin plugin;
            if (this.plugins.TryGetValue(name, out plugin))
                return plugin.Port;

            return 0;
        }

        internal string GetPortName(int port, bool isVMRC)
        {
            if (isVMRC && this.vmrcPlugin.Port == port)
                return vmrcPlugin.PortName;

            var plugin = this.plugins.Values.FirstOrDefault(p => p.Port == port);
            if (plugin != null)
                return plugin.PortName;

            return rdpPlugin.PortName;
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
            IConnectionPlugin plugin;
            if(this.plugins.TryGetValue(newProtocol, out plugin))
                return plugin.CreateOptionsControls();
            
            return new Control[0];
        }

        public string[] GetAvailableProtocols()
        {
            return this.plugins.Values.Select(p => p.PortName)
                .ToArray();
        }

        internal ITerminalsOptionsExport[] GetTerminalsOptionsExporters()
        {
            return this.plugins.OfType<IOptionsExporterFactory>()
                .Select(p => p.CreateOptionsExporter())
                .ToArray();
        }

        public static IToolbarExtender[] CreateToolbarExtensions(ICurrenctConnectionProvider provider)
        {
            return new IToolbarExtender[]
            {
                new RdpMenuVisitor(provider), 
                new VncMenuVisitor(provider), 
                new VmrcMenuVisitor(provider)
            };
        }
    }
}
