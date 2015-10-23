using System.Windows.Forms;

namespace Terminals.Connections.Web
{
    internal class HttpsConnectionPlugin: IConnectionPlugin
    {
        public int Port { get { return ConnectionManager.HTTPSPort; } }

        public string PortName { get { return ConnectionManager.HTTPS; } }

        public Connection CreateConnection()
        {
            return new HTTPConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[0];
        }
    }
}
