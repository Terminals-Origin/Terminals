using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections.Terminal
{
    internal class SshConnectionPlugin: IConnectionPlugin
    {
        public int Port { get { return ConnectionManager.SSHPort; } }

        public string PortName { get { return ConnectionManager.SSH; } }

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
    }
}
