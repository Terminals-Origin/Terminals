using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Properties;

namespace Terminals.Connections.Web
{
    internal class HttpConnectionPlugin: IConnectionPlugin
    {
        internal static readonly Image TreeIconHttp = Resources.treeIcon_http;

        public int Port { get { return KnownConnectionConstants.HTTPPort; } }

        public string PortName { get { return KnownConnectionConstants.HTTP; } }

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

        public Image GetIcon()
        {
            return TreeIconHttp;
        }
    }
}
