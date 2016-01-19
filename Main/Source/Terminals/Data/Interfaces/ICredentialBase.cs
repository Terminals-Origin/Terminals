using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Base user authentication values. These are used when ever stored password is required.
    /// </summary>
    public interface ICredentialBase
    {
        /// <summary>
        /// Gets or sets the user name in not encrypted form. This value isn't stored.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the domain in not encrypted form. This value isn't stored.
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password. Depends on the master password key.
        /// </summary>
        string EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the password in not encrypted form. This value isn't stored.
        /// </summary>
        [XmlIgnore]
        string Password { get; set; }

        /// <summary>
        /// Replaces stored encrypted password by new one using newKeymaterial
        /// </summary>
        /// <param name="newKeymaterial">key created from master password hash</param>
        void UpdatePasswordByNewKeyMaterial(string newKeymaterial);
    }
}