using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    internal class TelnetConnectionPlugin : IConnectionPlugin, IOptionsConverterFactory
    {
        internal const int TelnetPort = 23;
        internal const string TELNET = "PuttyTelnet";

        public int Port { get { return TelnetPort; } }

        public string PortName { get {return TELNET; } }

        public static Image TreeIconTelnet
        {
            get
            {
                return Connection.Terminalsicon;
            }
        }
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
            return new Control[] { new TelnetOptionsControl() { Name = "Putty Telnet" } };
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new TelnetOptionsConverter();
        }

        //TODO(JRG): Get putty icon
        public System.Drawing.Image GetIcon()
        {
            return Connection.Terminalsicon; 
        }

        public Type GetOptionsType()
        {
            return typeof(TelnetOptions);
        }
    }
}
