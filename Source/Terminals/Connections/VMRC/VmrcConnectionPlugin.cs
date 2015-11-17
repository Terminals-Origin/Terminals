using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections.VMRC
{
    internal class VmrcConnectionPlugin : IConnectionPlugin
    {
        internal const string VMRC = "VMRC";

        public int Port { get { return ConnectionManager.VNCVMRCPort; } }

        public string PortName { get { return VMRC; }}

        public Connection CreateConnection()
        {
            //return new FakeVmrcConnection();
            return new VMRCConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new VmrcControl() { Name = "VMRC" } };
        }

        public Type GetOptionsType()
        {
            return typeof (VMRCOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new VMRCOptions();
        }
    }
}
