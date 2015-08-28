using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Properties;

namespace Terminals.Connections
{
    internal static class ConnectionManager
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

        private const string CONSOLE = "Console";

        // cached images, bad performace, but simplifies check, if the image is known
        internal static readonly Image TreeIconRdp = Resources.treeIcon_rdp;
        internal static readonly Image TreeIconHttp = Resources.treeIcon_http;
        internal static readonly Image TreeIconVnc = Resources.treeIcon_vnc;
        internal static readonly Image TreeIconTelnet = Resources.treeIcon_telnet;
        internal static readonly Image TreeIconSsh = Resources.treeIcon_ssh;
        internal static readonly Image Terminalsicon = Resources.terminalsicon;

        internal static Dictionary<string, Image> GetPluginIcons()
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
        internal static ProtocolOptions UpdateProtocolPropertiesByProtocol(string newProtocol, ProtocolOptions currentOptions)
        {
            switch (newProtocol)
            {
                case VNC:
                    return SwitchPropertiesIfNotTheSameType<VncOptions>(currentOptions);
                case VMRC:
                    return SwitchPropertiesIfNotTheSameType<VMRCOptions>(currentOptions);
                case TELNET:
                    return SwitchPropertiesIfNotTheSameType<ConsoleOptions>(currentOptions);
                case SSH:
                    return SwitchPropertiesIfNotTheSameType<SshOptions>(currentOptions);
                case RDP:
                    return SwitchPropertiesIfNotTheSameType<RdpOptions>(currentOptions);
                case ICA_CITRIX:
                    return SwitchPropertiesIfNotTheSameType<ICAOptions>(currentOptions);
                case HTTP:
                case HTTPS:
                    return SwitchPropertiesIfNotTheSameType<WebOptions>(currentOptions);
                default:
                    return new EmptyOptions();
            }
        }

        private static ProtocolOptions SwitchPropertiesIfNotTheSameType<TOptions>(ProtocolOptions currentOptions)
            where TOptions : ProtocolOptions
        {
            if (!(currentOptions is TOptions)) // prevent to reset properties
                return Activator.CreateInstance<TOptions>();

            return currentOptions;
        }

        internal static ushort[] SupportedPorts()
        {
            return new ushort[] { ICAPort, RDPPort, SSHPort, TelnetPort, VNCVMRCPort };
        }

        internal static Connection CreateConnection(IFavorite favorite)
        {
            switch (favorite.Protocol)
            {
                case VNC:
                    //return new FakeVNCConnection();
                    return new VNCConnection();
                case VMRC:
                    //return new FakeVmrcConnection();
                    return new VMRCConnection();
                case RAS:
                    return new RASConnection();
                case TELNET:
                    return new TerminalConnection();
                case SSH:
                    return new TerminalConnection();
                case ICA_CITRIX:
                    return new ICAConnection();
                case HTTP:
                case HTTPS:
                    return new HTTPConnection();
                default:
                   // return new FakeRdpConnection();
                   return new RDPConnection();
            }
        }

        internal static int GetPort(string name)
        {
            switch (name)
            {
                case VNC:
                    return VNCVMRCPort;
                case VMRC:
                    return VNCVMRCPort;
                case TELNET:
                    return TelnetPort;
                case SSH:
                    return SSHPort;
                case RDP:
                    return RDPPort;
                case ICA_CITRIX:
                    return ICAPort;
                case HTTP:
                    return HTTPPort;
                case HTTPS:
                    return HTTPSPort;
                default:
                    return 0;
            }
        }

        internal static string GetPortName(int port, bool isVMRC)
        {
            switch (port)
            {
                case VNCVMRCPort:
                    if (isVMRC)
                    {
                        return VMRC;
                    }

                    return VNC;
                case TelnetPort:
                    return TELNET;
                case SSHPort:
                    return SSH;
                case ICAPort:
                    return ICA_CITRIX;
                case HTTPPort:
                    return HTTP;
                case HTTPSPort:
                    return HTTPS;
                default:
                    return RDP;
            }
        }

        /// <summary>
        /// Ensures web based protocol shortcut. Returns true in case of HTTP or HTTPS.
        /// </summary>
        /// <param name="protocol">One of connection short cuts.</param>
        internal static bool IsProtocolWebBased(string protocol)
        {
            return protocol == HTTP || protocol == HTTPS;
        }

        internal static bool IsKnownProtocol(string protocol)
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

        internal static ITerminalsOptionsExport[] GetTerminalsOptionsExporters()
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

        internal static Control[] CreateControls(string newProtocol)
        {
            switch (newProtocol)
            {
                case RDP:
                    return CreateRdpControls();
                case VNC:
                    return new Control[] { new VncControl() { Name = "VNC" } };
                case VMRC:
                    return new Control[] { new VmrcControl() { Name = "VMRC" } };
                case TELNET:
                    return new Control[] { new ConsolePreferences() { Name = CONSOLE } };
                case SSH:
                    return CreateSshControls();
                case ICA_CITRIX:
                    return new Control[] { new CitrixControl() { Name = "ICA Citrix" } };
                case HTTP:
                case HTTPS:
                    return new Control[0];
                default:
                    return new Control[0];
            }
        }

        private static Control[] CreateRdpControls()
        {
            return new Control[]
            {
                new RdpDisplayControl() { Name = "Display" },
                new RdpExtendedSettingsControl() { Name = "Extended settings" },
                new RdpLocalResourcesControl() { Name = "Local resources" },
                new RdpSecurityControl() { Name = "Security" },
                new RdpTsGatewayControl() { Name = "TS Gateway" }
            };
        }

        private static Control[] CreateSshControls()
        {
            return new Control[]
            {
                new ConsolePreferences() { Name = CONSOLE },
                new SshControl() { Name = "SSH" }
            };
        }

        public static string[] GetAvailableProtocols()
        {
            return new string[]
                {
                    RDP,
                    VNC,
                    VMRC,
                    SSH,
                    TELNET,
                    // RAS, // this protocol doesnt fit to the concept and seems to be broken 
                    ICA_CITRIX,
                    HTTP,
                    HTTPS
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
