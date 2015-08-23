using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections
{
    internal static class ConnectionManager
    {
        internal const int RDPPort = 3389;
        internal const int VNCVMRCPort = 5900;
        internal const int TelnetPort = 23;
        internal const int SSHPort = 22;
        internal const int ICAPort = 1494;
        internal const int HTTPPort = 80;
        internal const int HTTPSPort = 443;

        internal const string VNC = "VNC";
        internal const string VMRC = "VMRC";
        internal const string RAS = "RAS";
        internal const string TELNET = "Telnet";
        internal const string SSH = "SSH";
        internal const string RDP = "RDP";
        internal const string ICA_CITRIX = "ICA Citrix";
        internal const string HTTP = "HTTP";
        internal const string HTTPS = "HTTPS";

        private const int MAX_WIDTH = 4096;
        private const int MAX_HEIGHT = 2048;

        private const string CONSOLE = "Console";

        public static Size GetSize(Connection connection, IFavorite favorite)
        {
            int height = favorite.Display.Height;
            int width = favorite.Display.Width;

            switch (favorite.Display.DesktopSize)
            {
                case DesktopSize.x640:
                    return new Size(640, 480);
                case DesktopSize.x800:
                    return new Size(800, 600);
                case DesktopSize.x1024:
                    return new Size(1024, 768);
                case DesktopSize.x1152:
                    return new Size(1152, 864);
                case DesktopSize.x1280:
                    return new Size(1280, 1024);
                case DesktopSize.FullScreen:
                    width = Screen.FromControl(connection).Bounds.Width - 13;
                    height = Screen.FromControl(connection).Bounds.Height - 1;
                    return GetMaxAvailableSize(width, height);
                case DesktopSize.FitToWindow:
                case DesktopSize.AutoScale:
                    width = connection.Parent.Width;
                    height = connection.Parent.Height;
                    return GetMaxAvailableSize(width, height);
                default:
                    return new Size(width, height);
            }
        }

        private static Size GetMaxAvailableSize(int width, int height)
        {
            width = Math.Min(MAX_WIDTH, width);
            height = Math.Min(MAX_HEIGHT, height);
            return new Size(width, height);
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
