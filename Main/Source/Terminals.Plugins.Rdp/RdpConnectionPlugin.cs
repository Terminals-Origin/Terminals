using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Plugins.Rdp;
using Terminals.Plugins.Rdp.Properties;

namespace Terminals.Connections.Rdp
{
    internal class RdpConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory, IToolbarExtenderFactory, 
        IOptionsConverterFactory
    {
        internal static readonly Image TreeIconRdp = Resources.treeIcon_rdp;

        public int Port { get { return KnownConnectionConstants.RDPPort; } }

        public string PortName { get { return KnownConnectionConstants.RDP; } }

        public Connection CreateConnection()
        {
            // return new FakeRdpConnection();
            return new RDPConnection();
        }

        public Control[] CreateOptionsControls()
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

        public Type GetOptionsType()
        {
            return typeof (RdpOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new RdpOptions();
        }

        public Image GetIcon()
        {
            return TreeIconRdp;
        }

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsRdpExport();
        }

        public IToolbarExtender CreateToolbarExtender(ICurrenctConnectionProvider provider)
        {
            return new RdpMenuVisitor(provider);
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new RdpOptionsConverter();
        }
    }
}
