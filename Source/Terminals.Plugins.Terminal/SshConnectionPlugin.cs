using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;
using Terminals.Plugins.Terminal.Properties;

namespace Terminals.Connections.Terminal
{
    internal class SshConnectionPlugin: IConnectionPlugin, IOptionsExporterFactory
    {
        internal const int SSHPort = 22;

        internal const string SSH = "SSH";

        internal static readonly Image TreeIconSsh = Resources.treeIcon_ssh;

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

        public Image GetIcon()
        {
            return TreeIconSsh;
        }
    }
}
