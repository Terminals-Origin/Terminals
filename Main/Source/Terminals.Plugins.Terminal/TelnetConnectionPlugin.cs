using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;
using Terminals.Integration.Export;
using Terminals.Plugins.Terminal;
using Terminals.Plugins.Terminal.Properties;

namespace Terminals.Connections.Terminal
{
    internal class TelnetConnectionPlugin : IConnectionPlugin, IOptionsExporterFactory, IOptionsConverterFactory
    {
        internal const string CONSOLE = "Console";

        internal const int TelnetPort = 23;

        internal const string TELNET = "Telnet";

        internal static readonly Image TreeIconTelnet = Resources.treeIcon_telnet;

        public int Port { get { return TelnetPort; }}

        public string PortName { get { return TELNET; } }

        public Image GetIcon()
        {
            return TreeIconTelnet;
        }

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

        public ITerminalsOptionsExport CreateOptionsExporter()
        {
            return new TerminalsTelnetExport();
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new ConsoleOptionsConverter();
        }
    }
}
