using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSC = MSTSCLib;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TabControl;
using System.IO;

namespace Terminals.Connections {
    public class ConnectionManager {

        public const int RDPPort = 3389;
        public const int VNCVMRCPort = 5900;
        public const int TelnetPort = 23;
        public const int SSHPort = 22;
        public const int ICAPort = 1494;

        public static void GetSize(ref int Height, ref int Width, Connections.Connection Connection, DesktopSize Size) {


            switch(Size) {
                case DesktopSize.x640:
                    Width = 640;
                    Height = 480;
                    break;
                case DesktopSize.x800:
                    Width = 800;
                    Height = 600;
                    break;
                case DesktopSize.x1024:
                    Width = 1024;
                    Height = 768;
                    break;
                case DesktopSize.x1280:
                    Width = 1280;
                    Height = 1024;
                    break;
                case DesktopSize.FitToWindow:
                    Width = Connection.TerminalTabPage.Width;
                    Height = Connection.TerminalTabPage.Height;
                    break;
                case DesktopSize.FullScreen:
                    Width = Screen.FromControl(Connection).Bounds.Width;
                    Height = Screen.FromControl(Connection).Bounds.Height - 1;
                    break;
                case DesktopSize.AutoScale:
                    Width = Screen.FromControl(Connection).Bounds.Width;
                    Height = Screen.FromControl(Connection).Bounds.Height - 1;
                    break;
                case DesktopSize.Custom:
                    break;
            }
            int maxWidth = Settings.SupportsRDP6 ? 4096 : 1600;
            int maxHeight = Settings.SupportsRDP6 ? 2048 : 1200;

            Width = Math.Min(maxWidth, Width);
            Height = Math.Min(maxHeight, Height); ;

        }
        public static IConnection CreateConnection(FavoriteConfigurationElement Favorite, TerminalTabControlItem TerminalTabPage, MainForm parentForm) {
            IConnection conn = null; ;
            switch (Favorite.Protocol) {
                case "VNC":
                    conn = new VNCConnection();
                    break;
                case "VMRC":
                    conn = new VMRCConnection();
                    break;
                case "RAS":
                    conn = new RASConnection();
                    break;
                case "Telnet":
                    conn = new TerminalConnection();
                    break;
                case "ICA Citrix":
                    conn = new ICAConnection();
                    break;
                default:
                    conn = new RDPConnection();
                    break;
            }
            conn.Favorite = Favorite;
            TerminalTabPage.Connection = conn;
            conn.TerminalTabPage = TerminalTabPage;
            conn.ParentForm = parentForm;
            return conn;
        }
        public static int GetPort(string Name) {
            int port = 0;
            switch (Name) {
                case "VNC":
                    port = VNCVMRCPort;
                    break;
                case "VMRC":
                    port = VNCVMRCPort;
                    break;
                case "Telnet":
                    port = TelnetPort;
                    break;
                case "RDP":
                    port = RDPPort;
                    break;
                case "ICA Citrix":
                    port = ICAPort;
                    break;
                default:
                    port = 0;
                    break;
            }
            return port;
        }
        public static string GetPortName(int Port, bool isVMRC) {
            string port = "RDP";
            switch (Port) {
                case VNCVMRCPort:
                    if (isVMRC) {
                        port = "VMRC";
                    } else {
                        port = "VNC";
                    }
                    break;
                case TelnetPort:
                    port = "Telnet";
                    break;
                case SSHPort:
                    port = "SSH";
                    break;
                case ICAPort:
                    port = "ICA Citrix";
                    break;
                default:
                    port = "RDP";
                    break;
            }
            return port;
        }

    }
}
