using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Plugins.Vmrc;

namespace Terminals.Connections.VMRC
{
    internal class VmrcConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory,
        IToolbarExtenderFactory, IOptionsConverterFactory
    {
        internal const string VMRC = "VMRC";

        internal const int VMRCPort = 5900;

        public int Port { get { return VMRCPort; } }

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
            return new VmrcToolbarVisitor(provider);
        }

        public Image GetIcon()
        {
            return Connection.Terminalsicon;
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new VmrcOptionsConverter();
        }
    }
}
