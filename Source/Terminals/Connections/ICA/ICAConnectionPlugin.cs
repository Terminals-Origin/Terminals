using System;
using System.Windows.Forms;
using Terminals.Data;
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

        public Type GetOptionsType()
        {
            return typeof (ICAOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new ICAOptions();
        }
    }
}
