using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections.Terminal
{
    internal class TelnetConnectionPlugin : IConnectionPlugin
    {
        internal const string CONSOLE = "Console";

        internal const int TelnetPort = 23;

        internal const string TELNET = "Telnet";

        public int Port { get { return TelnetPort; }}

        public string PortName { get { return TELNET; } }

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
