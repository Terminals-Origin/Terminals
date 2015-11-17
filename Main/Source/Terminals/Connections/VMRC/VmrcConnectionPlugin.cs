using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;

namespace Terminals.Connections.VMRC
{
    internal class VmrcConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory, IToolbarExtenderFactory
    {
        internal const string VMRC = "VMRC";

        public int Port { get { return KnownConnectionConstants.VNCVMRCPort; } }

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

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsVmrcExport();
        }

        public IToolbarExtender CreateToolbarExtender(ICurrenctConnectionProvider provider)
        {
            return new VmrcMenuVisitor(provider);
        }
    }
}
