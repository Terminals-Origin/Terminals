using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;

namespace Terminals.Connections.Rdp
{
    internal class RdpConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory
    {
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

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsRdpExport();
        }
    }
}
