using System;
using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Container of stored user authentication.
    /// </summary>
    public interface ICredentialSet : ICredentialBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of this instance. This value should be unique.
        /// </summary>
        [XmlAttribute("id")]
        Guid Id { get; }

        /// <summary>
        /// Gets or sets the unique not empty name of the set.
        /// </summary>
        string Name { get; set; }

        // todo test, if the UserName and Domain properties are still shown in grid
        // are required to be shown in GridControls, see implementation
    }
}