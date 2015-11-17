using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Connections.ICA
{
    internal class ICAConnectionPlugin :IConnectionPlugin
    {
        internal const int ICAPort = 1494;

        internal const string ICA_CITRIX = "ICA Citrix";

        public int Port { get { return ICAPort; } }

        public string PortName { get { return ICA_CITRIX; } }

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
