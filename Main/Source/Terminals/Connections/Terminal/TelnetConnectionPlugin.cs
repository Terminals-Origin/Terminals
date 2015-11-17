using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections.Terminal
{
    internal class TelnetConnectionPlugin : IConnectionPlugin
    {
        internal const string CONSOLE = "Console";

        public int Port { get { return ConnectionManager.TelnetPort; }}

        public string PortName { get { return ConnectionManager.TELNET; } }

        public Connection CreateConnection()
        {
            return new TerminalConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new ConsolePreferences() { Name = CONSOLE } };
        }

        public Type GetOptionsType()
        {
            return typeof (ConsoleOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new ConsoleOptions();
        }
    }
}
