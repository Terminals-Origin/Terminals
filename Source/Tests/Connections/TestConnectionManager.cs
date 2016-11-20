using System.Collections.Generic;
using Terminals.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Rdp;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Connections.Web;

namespace Tests.Connections
{
    internal class TestConnectionManager
    {
        private static readonly ConnectionManager instance = new ConnectionManager(() =>
            new List<IConnectionPlugin>()
            {
                new RdpConnectionPlugin(),
                new HttpConnectionPlugin(),
                new HttpsConnectionPlugin(),
                new VncConnectionPlugin(),
                new VmrcConnectionPlugin(),
                new TelnetConnectionPlugin(),
                new SshConnectionPlugin(),
                new ICAConnectionPlugin()
            });

        /// <summary>
        /// Gets instance of manager configured by staticaly loaded plugins.
        /// Otherwise we would need to deploy the plugins into test directory.
        /// </summary>
        public static ConnectionManager Instance { get { return instance; } }
    }
}