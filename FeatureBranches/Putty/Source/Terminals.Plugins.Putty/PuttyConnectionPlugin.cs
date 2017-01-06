using System;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    internal class PuttyConnectionPlugin : IConnectionPlugin, IOptionsConverterFactory
    {
        internal const int SSHPort = 22;
        internal const string SSH = "Putty SSH";

        public int Port { get { return SSHPort; } }

        public string PortName { get {return SSH; } }

        public Connection CreateConnection()
        {
            return new PuttyConnection();
        }

        public ProtocolOptions CreateOptions()
        {
            return new PuttyOptions();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new PuttyOptionsControl() { Name = "Putty SSH" } };
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new PuttyOptionsConverter();
        }

        //TODO(JRG): Get putty icon
        public System.Drawing.Image GetIcon()
        {
            return Connection.Terminalsicon; 
        }

        public Type GetOptionsType()
        {
            return typeof(PuttyOptions);
        }
    }
}
