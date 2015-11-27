using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Connections
{
    /// <summary>
    /// Implementation of default values in case required plugin is not available.
    /// </summary>
    internal class DummyPlugin : IConnectionPlugin
    {
        public int Port { get { return 0; } }

        public string PortName { get { return KnownConnectionConstants.RDP; } }

        public Image GetIcon()
        {
            return ConnectionManager.Terminalsicon;
        }

        public Connection CreateConnection()
        {
            return new Connection();
        }

        public Control[] CreateOptionsControls()
        {
            return new Control[0];
        }

        public Type GetOptionsType()
        {
            return typeof (EmptyOptions);
        }

        public ProtocolOptions CreateOptions()
        {
            return new EmptyOptions();
        }
    }
}