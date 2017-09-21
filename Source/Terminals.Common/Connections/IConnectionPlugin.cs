using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    public interface IConnectionPlugin
    {
        /// <summary>
        /// Gets valid network port number (in range 0-65535). To be able propoerly identify protocol by its port, it needs to be unique.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Gets not null or empty shortcut name of the protocol. This identifier needs to be unique across plugins. Only one plugin is allowed to be loaded per protocol.
        /// </summary>
        string PortName { get; }

		/// <summary>
        /// Returns not null image to be used in favorites tree, menus etc. to distinguis each protocol by color or its symbol.
        /// </summary>
        Image GetIcon();

        /// <summary>
        /// Returns not null, always new instance of not configured, disconnected connection.
        /// </summary>
        Connection CreateConnection();

        /// <summary>
        /// Returns not null collection of always new instances of user controls used to configure
        /// protocol options created by <see cref="CreateOptions"/>. If you dont need any options, return empty collection.
		/// Each Control also needs to implement <see cref="IProtocolOptionsControl"/> to be able load and save edited values.
        /// </summary>
        Control[] CreateOptionsControls();
        
        /// <summary>
        /// Returns not null type of connections created by <see cref="CreateOptions"/>
        /// </summary>
        Type GetOptionsType();

        /// <summary>
        /// Creates always new instance of options used to configure connection favorite. 
		/// Inherit your own ProtocolOptions to provide custom settings, the class needs to be serializable.
        /// The options are stored as part of each favorite. User may edit them using user controls created by CreateOptionsControls method.
        /// </summary>
        ProtocolOptions CreateOptions();
    }
}
