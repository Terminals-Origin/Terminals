using System.Windows.Forms;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections.VNC
{
    internal class VncConnectionPlugin : IConnectionPlugin
    {
        public int Port { get { return ConnectionManager.VNCVMRCPort; } }

        public string PortName { get { return ConnectionManager.VNC; } }

        public Connection CreateConnection()
        {
            //return new FakeVNCConnection();
            return new VNCConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new VncControl() { Name = "VNC" } };
        }
    }
}
