using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections.Web
{
    internal class HttpsConnectionPlugin: IConnectionPlugin
    {
        internal const int HTTPSPort = 443;

        public int Port { get { return HTTPSPort; } }

        public string PortName { get { return KnownConnectionConstants.HTTPS; } }

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
            return HttpConnectionPlugin.TreeIconHttp;
        }
    }
}
