using System.Windows.Forms;

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
    }
}
