using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Plugins.Putty.Properties;

namespace Terminals.Plugins.Putty
{
    internal class TelnetConnectionPlugin : IConnectionPlugin, IOptionsConverterFactory
    {
        internal const int TelnetPort = 23;
        internal const string TELNET = "Telnet";

        public int Port { get { return TelnetPort; } }

        public string PortName { get {return TELNET; } }

        internal static readonly Image TreeIconTelnet = Resources.treeIcon_telnet;

        public Connection CreateConnection()
        {
            return new PuttyConnection();
        }

        public ProtocolOptions CreateOptions()
        {
            return new TelnetOptions();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new TelnetOptionsControl() { Name = "Telnet" } };
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new TelnetOptionsConverter();
        }

        public Image GetIcon()
        {
            return TreeIconTelnet; 
        }

        public Type GetOptionsType()
        {
            return typeof(TelnetOptions);
        }
    }
}
