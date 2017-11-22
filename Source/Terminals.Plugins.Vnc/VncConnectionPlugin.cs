using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Plugins.Vnc;
using Terminals.Plugins.Vnc.Properties;
using VncSharp;

namespace Terminals.Connections.VNC
{
    internal class VncConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory,
        IToolbarExtenderFactory, IExtraDetection, IOptionsConverterFactory
    {
        internal const string VNC = "VNC";

        internal const int VncPort = 5900;

        internal static readonly Image TreeIconVnc = Resources.treeIcon_vnc;

        public int Port { get { return VncPort; } }

        public string PortName { get { return VNC; } }

        // because there is no interface on Vnc RemoteDesktop to wrapp.
        private readonly Action<string, int> serviceCheck;

        public VncConnectionPlugin() : this(CheckRealService)
        {
        }

        internal VncConnectionPlugin(Action<string, int> serviceCheck)
        {
            this.serviceCheck = serviceCheck;
        }

        public Connection CreateConnection()
        {
            //return new FakeVNCConnection();
            return new VNCConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new VncControl() { Name = "VNC" } };
        }

        public Type GetOptionsType()
        {
            return typeof (VncOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new VncOptions();
        }

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsVncExport();
        }

        public IToolbarExtender CreateToolbarExtender(ICurrenctConnectionProvider provider)
        {
            return new VncToolbarVisitor(provider);
        }

        public Image GetIcon()
        {
            return TreeIconVnc;
        }

        public bool IsValid(string ipAddress, int port)
        {
            try
            {
                this.serviceCheck(ipAddress, port);
                return true;
            }
            catch (CryptographicException ce)
            {
                // ignore this kind of exception, because of detecting with empty password may fail.
                Logging.Info(string.Empty, ce);
                return true;
            }
            catch (Exception exc)
            {
                Logging.Error("VNC Port Scan Failed", exc);
                return false;
            }
        }

        private static void CheckRealService(string ipAddress, int port)
        {
            using (var rd = new RemoteDesktop())
            {
                rd.VncPort = port;
                rd.GetPassword = new AuthenticateDelegate(() => string.Empty);
                rd.Connect(ipAddress);
                rd.Disconnect();
            }
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new VncOptionsConverter();
        }
    }
}
