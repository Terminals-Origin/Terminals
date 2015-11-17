using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;

namespace Terminals.Connections.Terminal
{
    internal class SshConnectionPlugin: IConnectionPlugin, IOptionsExporterFactory
    {
        internal const int SSHPort = 22;

        internal const string SSH = "SSH";

        public int Port { get { return SSHPort; } }

        public string PortName { get { return SSH; } }

        public Connection CreateConnection()
        {
            return new TerminalConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[]
            {
                new ConsolePreferences() { Name = TelnetConnectionPlugin.CONSOLE },
                new SshControl() { Name = "SSH" }
            };
        }

        public Type GetOptionsType()
        {
            return typeof (SshOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new SshOptions();
        }

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsSshExport();
        }
    }
}
