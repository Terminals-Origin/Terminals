using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections.Web
{
    internal class HttpConnectionPlugin: IConnectionPlugin
    {
        public int Port { get { return ConnectionManager.HTTPPort; } }

        public string PortName { get { return ConnectionManager.HTTP; } }

        public Connection CreateConnection()
        {
            return new HTTPConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[0];
        }

        public Type GetOptionsType()
        {
            return typeof (WebOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new WebOptions();
        }
    }
}
