using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal interface IConnectionPlugin
    {
        /// <summary>
        /// Gets valid network port number.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Gets shortcut name of the protocol.
        /// </summary>
        string PortName { get; }

        /// <summary>
        /// Returns not null, always new instance of not configured, disconnected connection.
        /// </summary>
        Connection CreateConnection();

        /// <summary>
        /// Returns not null collection of always new instances of user controls used to configure
        /// protocol options created by <see cref="CreateOptions"/>
        /// </summary>
        Control[] CreateOptionsControls();
        
        /// <summary>
        /// Returns not null type of connections created by <see cref="CreateOptions"/>
        /// </summary>
        Type GetOptionsType();

        /// <summary>
        /// Creates always new instance of options used to configure connection.
        /// The options are stored as part of each favorite.
        /// </summary>
        ProtocolOptions CreateOptions();
    }
}
