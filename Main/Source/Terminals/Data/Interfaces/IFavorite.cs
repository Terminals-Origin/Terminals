using System;
using System.Collections.Generic;
using System.Drawing;

namespace Terminals.Data
{
    /// <summary>
    /// Connection properties persisted for future reuse
    /// </summary>
    internal interface IFavorite
    {
        /// <summary>
        /// Gets or sets the unique identifier of this instance
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the connection, this will appear as default label in GUI.
        /// The name should be always unique in application version 2.0.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the protocol type. Accepted value is one of ConnectionManager values.
        /// Set this value also updates the ProtocolProperties property to provide extra options.
        /// </summary>
        String Protocol { get; set; }

        /// <summary>
        /// Gets or sets positive number in range 0 - 65535
        /// of port on which server service defined by protocol is lissening.
        /// </summary>
        Int32 Port { get; set; }

        /// <summary>
        /// Gets or sets the hostname, IP address or URL of the server.
        /// </summary>
        String ServerName { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to the tool bar icon file, if custom icon was assigned.
        /// To directly access the icon image use <see cref="ToolBarIconImage"/>
        /// </summary>
        String ToolBarIconFile { get; set; }

        /// <summary>
        /// Gets the image loaded from assigned icon file.
        /// </summary>
        Image ToolBarIconImage { get; }

        /// <summary>
        /// Gets or sets the flag identifiyng, if the connection should be opened in new window or in TabControl.
        /// False by default.
        /// </summary>
        Boolean NewWindow { get; set; }

        String DesktopShare { get; set; }

        String Notes { get; set; }

        /// <summary>
        /// Gets collection of groups, where this favorite appears. 
        /// This is navigation property only and shouldnt be used for changes.
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
        /// Depending on selected protocol, this should contian the protocol detailed options.
        /// Because default protocol is RDP, also this properties are RdpOptions by default.
        /// This property should be always updated by changing Protocol property value
        /// </summary>
        ProtocolOptions ProtocolProperties { get; set; }

        /// <summary>
        /// Creates new deep copy of this instance. The only property which isnt copied is Id.
        /// </summary>
        /// <returns>Not null newly created copy of this instance</returns>
        IFavorite Copy();

        /// <summary>
        /// Gets label, which represents this instance detail informations.
        /// </summary>
        string GetToolTipText();

        /// <summary>
        /// Returns text compareto method values selecting property to compare
        /// depending on Settings default sort property value
        /// </summary>
        /// <param name="target">not null favorite to compare with</param>
        /// <returns>result of String CompareTo method</returns>
        int CompareByDefaultSorting(IFavorite target);

        void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial);
    }
}
