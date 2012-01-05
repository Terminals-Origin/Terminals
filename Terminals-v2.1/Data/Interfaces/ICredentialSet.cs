using System;
using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Container of stored user authentication.
    /// </summary>
    internal interface ICredentialSet
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
        string Username { get; set; }
        string Domain { get; set; }

        /// <summary>
        /// Gets the encrypted password hash.
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Gets or sets the password in not encrypted form.
        /// </summary>
        [XmlIgnore]
        string SecretKey { get; set; }

        /// <summary>
        /// Replaces stored password hash by new one using newKeymaterial
        /// </summary>
        /// <param name="newKeymaterial">key created from master password hash</param>
        void UpdatePasswordByNewKeyMaterial(string newKeymaterial);
    }
}