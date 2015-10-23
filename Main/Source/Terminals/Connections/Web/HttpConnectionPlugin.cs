using System.Windows.Forms;

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
    }
}
