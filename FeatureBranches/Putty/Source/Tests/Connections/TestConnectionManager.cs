using System.Collections.Generic;
using Moq;
using Terminals.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Rdp;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Connections.Web;
using Terminals.Data;

namespace Tests.Connections
{
    internal class TestConnectionManager
    {
        private static readonly List<IConnectionPlugin> connectionPlugins = new List<IConnectionPlugin>()
        {
            new RdpConnectionPlugin(),
            new HttpConnectionPlugin(),
            new HttpsConnectionPlugin(),
            new VncConnectionPlugin(),
            new VmrcConnectionPlugin(),
            new TelnetConnectionPlugin(),
            new SshConnectionPlugin(),
            new ICAConnectionPlugin()
        };

        private static readonly Mock<IPluginsLoader> mockLoader = CreateMockLoader();

        private static readonly ConnectionManager instance = new ConnectionManager(mockLoader.Object);

        /// <summary>
        /// Gets instance of manager configured by staticaly loaded plugins.
        /// Otherwise we would need to deploy the plugins into test directory.
        /// </summary>
        public static ConnectionManager Instance { get { return instance; } }

        private static Mock<IPluginsLoader> CreateMockLoader()
        {
            var mockLoader = new Mock<IPluginsLoader>();
            mockLoader.Setup(l => l.Load())
                .Returns(() => connectionPlugins);
            return mockLoader;
        }

        internal static FavoriteIcons CreateTestFavoriteIcons()
        {
            return new FavoriteIcons(Instance);
        }
    }
}