using System.Windows.Forms;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections.VMRC
{
    internal class VmrcConnectionPlugin : IConnectionPlugin
    {
        public int Port { get { return ConnectionManager.VNCVMRCPort; } }

        public string PortName { get { return ConnectionManager.VMRC; }}

        public Connection CreateConnection()
        {
            //return new FakeVmrcConnection();
            return new VMRCConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new VmrcControl() { Name = "VMRC" } };
        }
    }
}
