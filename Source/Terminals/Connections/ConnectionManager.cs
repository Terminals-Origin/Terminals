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
        internal const int RDPPort = 3389;
        internal const int VNCVMRCPort = 5900;
        internal const int HTTPPort = 80;
        
        internal const int TelnetPort = 23;
        internal const int SSHPort = 22;
        internal const int ICAPort = 1494;
        internal const int HTTPSPort = 443;

        internal const string HTTP = "HTTP";
        internal const string HTTPS = "HTTPS";

        internal const string VNC = "VNC";
        internal const string VMRC = "VMRC";
        internal const string RAS = "RAS";
        internal const string TELNET = "Telnet";
        internal const string SSH = "SSH";
        internal const string RDP = "RDP";
        internal const string ICA_CITRIX = "ICA Citrix";

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

        private readonly IConnectionPlugin httpPlugin = new HttpConnectionPlugin();
        private readonly IConnectionPlugin httpsPlugin = new HttpsConnectionPlugin();
        private readonly IConnectionPlugin vncPlugin = new VncConnectionPlugin();
        private readonly IConnectionPlugin vmrcPlugin = new VmrcConnectionPlugin();
        private readonly IConnectionPlugin telnetPlugin = new TelnetConnectionPlugin();
        private readonly IConnectionPlugin sshPlugin = new SshConnectionPlugin();
        private readonly IConnectionPlugin icaPlugin = new ICAConnectionPlugin();
        private readonly IConnectionPlugin rdpPlugin = new RdpConnectionPlugin();

        private readonly Dictionary<string, IConnectionPlugin> plugins;

        public ConnectionManager()
        {
            this.plugins = new Dictionary<string, IConnectionPlugin>()
            {
                { HTTP, this.httpPlugin },
                { HTTPS, this.httpsPlugin },
                { VNC, this.vncPlugin },
                { VMRC, this.vmrcPlugin },
                { TELNET, this.telnetPlugin },
                { SSH, this.sshPlugin },
                { ICA_CITRIX, this.icaPlugin },
                { RDP, this.rdpPlugin }
            };
        }

        internal Dictionary<string, Image> GetPluginIcons()
        {
            return new Dictionary<string, Image>()
            {
                { RDP, TreeIconRdp },
                { VNC, TreeIconVnc },
                { SSH, TreeIconSsh },
                { TELNET, TreeIconTelnet },
                { HTTP, TreeIconHttp },
                { HTTPS, TreeIconHttp }
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
            switch (name)
            {
                case VNC:
                    return vncPlugin.Port;
                case VMRC:
                    return vmrcPlugin.Port;
                case TELNET:
                    return telnetPlugin.Port;
                case SSH:
                    return sshPlugin.Port;
                case RDP:
                    return rdpPlugin.Port;
                case ICA_CITRIX:
                    return icaPlugin.Port;
                case HTTP:
                    return httpPlugin.Port;
                case HTTPS:
                    return httpsPlugin.Port;
                default:
                    return 0;
            }
        }

        internal string GetPortName(int port, bool isVMRC)
        {
            switch (port)
            {
                case VNCVMRCPort:
                    if (isVMRC)
                    {
                        return vmrcPlugin.PortName;
                    }

                    return vncPlugin.PortName;
                case TelnetPort:
                    return telnetPlugin.PortName;
                case SSHPort:
                    return sshPlugin.PortName;
                case ICAPort:
                    return icaPlugin.PortName;
                case HTTPPort:
                    return httpPlugin.PortName;
                case HTTPSPort:
                    return httpsPlugin.PortName;
                default:
                    return rdpPlugin.PortName;
            }
        }

        /// <summary>
        /// Ensures web based protocol shortcut. Returns true in case of HTTP or HTTPS.
        /// </summary>
        /// <param name="protocol">One of connection short cuts.</param>
        internal bool IsProtocolWebBased(string protocol)
        {
            return protocol == HTTP || protocol == HTTPS;
        }

        internal bool IsKnownProtocol(string protocol)
        {
            switch (protocol)
            {
                case VNC:
                case VMRC:
                case TELNET:
                case SSH:
                case RDP:
                case ICA_CITRIX:
                case HTTP:
                case HTTPS:
                    return true;
                default:
                    return false;
            } 
        }

        internal ITerminalsOptionsExport[] GetTerminalsOptionsExporters()
        {
            return new ITerminalsOptionsExport[]
            {
                new TerminalsRdpExport(),
                new TerminalsVncExport(),
                new TerminalsSshExport(),
                new TerminalsTelnetExport(),
                new TerminalsVmrcExport(),
                new TerminalsIcaExport()
            };
        }

        internal Control[] CreateControls(string newProtocol)
        {
            switch (newProtocol)
            {
                case RDP:
                    return rdpPlugin.CreateOptionsControls();
                case VNC:
                    return vncPlugin.CreateOptionsControls();
                case VMRC:
                    return vmrcPlugin.CreateOptionsControls();
                case TELNET:
                    return telnetPlugin.CreateOptionsControls();
                case SSH:
                    return sshPlugin.CreateOptionsControls();
                case ICA_CITRIX:
                    return icaPlugin.CreateOptionsControls();
                case HTTP:
                    return httpPlugin.CreateOptionsControls();
                case HTTPS:
                    return httpsPlugin.CreateOptionsControls();
                default:
                    return new Control[0];
            }
        }

        public string[] GetAvailableProtocols()
        {
            return new string[]
                {
                    rdpPlugin.PortName,
                    vncPlugin.PortName,
                    vmrcPlugin.PortName,
                    sshPlugin.PortName,
                    telnetPlugin.PortName,
                    // RAS, // this protocol doesnt fit to the concept and seems to be broken 
                    icaPlugin.PortName,
                    httpPlugin.PortName,
                    httpsPlugin.PortName
                };
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
