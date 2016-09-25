using System;
using System.Collections.Generic;
using System.Drawing;

namespace Terminals.Data
{
    /// <summary>
    /// Connection properties persisted for future reuse
    /// </summary>
    public interface IFavorite : IStoreIdEquals<IFavorite>, INamedItem
    {
        /// <summary>
        /// Gets the unique identifier of this instance in associated store
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets or sets not null name of an item. This is usually validated against persistence to case sensitive unique.
        /// Hides InamedItem because ob findings and sorting, we need it to be explicitly defined here
        /// </summary>
        new string Name { get; set; }

        /// <summary>
        /// Gets the protocol type. Accepted value is one of ConnectionManager values.
        /// Set this value using <see cref="ChangeProtocol"/>.
        /// </summary>
        String Protocol { get; }

        /// <summary>
        /// Gets or sets positive number in range 0 - 65535
        /// of port on which server service defined by protocol is listening.
        /// </summary>
        Int32 Port { get; set; }

        /// <summary>
        /// Gets or sets the hostname, IP address or URL of the server.
        /// </summary>
        String ServerName { get; set; }

        /// <summary>
        /// Gets the image loaded from assigned icon file.
        /// </summary>
        Image ToolBarIconImage { get; }

        /// <summary>
        /// Gets or sets the flag identifying, if the connection should be opened in new window or in TabControl.
        /// False by default.
        /// </summary>
        Boolean NewWindow { get; set; }

        String DesktopShare { get; set; }

        String Notes { get; set; }

        /// <summary>
        /// Gets collection of groups, where this favorite appears. 
        /// This is navigation property only and shouldn't be used for changes.
        /// </summary>
        List<IGroup> Groups { get; }

        /// <summary>
        /// Gets not null label, which lists all comma separated group names listed in Groups property.
        /// </summary>
        string GroupNames { get; }

        IDisplayOptions Display { get; }
        ISecurityOptions Security { get; }
        IBeforeConnectExecuteOptions ExecuteBeforeConnect { get; }

        /// <summary>
        /// Depending on selected protocol, this should contain the protocol detailed options.
        /// Because default protocol is RDP, also this properties are RdpOptions by default.
        /// This property should be always updated by changing Protocol property value.
        /// Set this value using <see cref="ChangeProtocol"/>.
        /// </summary>
        ProtocolOptions ProtocolProperties { get; }

        /// <summary>
        /// Creates new deep copy of this instance. The only property which isn't copied is Id.
        /// </summary>
        /// <returns>Not null newly created copy of this instance</returns>
        IFavorite Copy();

        /// <summary>
        /// Updates this instance from source instance using deep copy. The only property which isn't updated is Id.
        /// </summary>
        /// <param name="source">Not null item, which properties should be use to update this instance</param>
        void UpdateFrom(IFavorite source);

        /// <summary>
        /// Use this method to update the protocol.
        /// </summary>
        /// <param name="protocol">Not null, New protocol name to assing.</param>
        /// <param name="options">Not null, structure for protocol specific options, has to fit to protocol.</param>
        void ChangeProtocol(string protocol, ProtocolOptions options);
    }
}
