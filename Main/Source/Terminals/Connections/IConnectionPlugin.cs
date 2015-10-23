using System.Windows.Forms;

namespace Terminals.Connections
{
    internal interface IConnectionPlugin
    {
        int Port { get; }

        string PortName { get; }

        Connection CreateConnection();

        Control[] CreateOptionsControls();
    }
}
