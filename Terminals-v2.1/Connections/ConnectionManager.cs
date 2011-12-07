using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal class ConnectionManager
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

        public static Size GetSize(Connection Connection, FavoriteConfigurationElement favorite)
        {
            int height = favorite.DesktopSizeHeight;
            int width = favorite.DesktopSizeWidth;

            switch (favorite.DesktopSize)
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
                    width = Screen.FromControl(Connection).Bounds.Width - 13;
                    height = Screen.FromControl(Connection).Bounds.Height - 1;
                    return GetMaxAvailableSize(width, height);
                case DesktopSize.FitToWindow:
                case DesktopSize.AutoScale:
                    width = Connection.TerminalTabPage.Width;
                    height = Connection.TerminalTabPage.Height;
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

        internal static IConnection CreateConnection(FavoriteConfigurationElement Favorite,
          TerminalTabControlItem TerminalTabPage, MainForm parentForm)
        {
            IConnection conn = CreateConnection(Favorite);
            conn.Favorite = Favorite;
            TerminalTabPage.Connection = conn;
            conn.TerminalTabPage = TerminalTabPage;
            conn.ParentForm = parentForm;
            return conn;
        }

        private static IConnection CreateConnection(FavoriteConfigurationElement Favorite)
        {
            switch (Favorite.Protocol)
            {
                case VNC:
                    return new VNCConnection();
                case VMRC:
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
                    return new HTTPConnection();
                case HTTPS:
                    return new HTTPConnection();
                default:
                    return new RDPConnection();
            }
        }

        internal static int GetPort(string Name)
        {
            switch (Name)
            {
                case VNC:
                    return VNCVMRCPort;
                case VMRC:
                    return VNCVMRCPort;
                case TELNET:
                    return TelnetPort;
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

        internal static string GetPortName(int Port, bool isVMRC)
        {
            switch (Port)
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
    }
}
