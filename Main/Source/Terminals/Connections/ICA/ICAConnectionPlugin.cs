using System.Windows.Forms;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections.ICA
{
    internal class ICAConnectionPlugin :IConnectionPlugin
    {
        public int Port { get { return ConnectionManager.ICAPort; } }

        public string PortName { get { return ConnectionManager.ICA_CITRIX; } }

        public Connection CreateConnection()
        {
            return new ICAConnection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[] { new CitrixControl() { Name = "ICA Citrix" } };
        }
    }
}
