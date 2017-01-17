using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    internal class SshConnectionPlugin : IConnectionPlugin, IOptionsConverterFactory
    {
        internal const int SSHPort = 22;
        internal const string SSH = "PuttySSH";

        public int Port { get { return SSHPort; } }

        public string PortName { get {return SSH; } }

        public static Image TreeIconSsh
        {
            get {
                return Connection.Terminalsicon;
            }
        }

        public Connection CreateConnection()
        {
            return new PuttyConnection();
        }

        public ProtocolOptions CreateOptions()
        {
            return new SshOptions();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new SshOptionsControl() { Name = "Putty SSH" } };
        }

        public IOptionsConverter CreatOptionsConverter()
        {
            return new SshOptionsConverter();
        }

        //TODO(JRG): Get putty icon
        public System.Drawing.Image GetIcon()
        {
            return Connection.Terminalsicon; 
        }

        public Type GetOptionsType()
        {
            return typeof(SshOptions);
        }
    }
}
