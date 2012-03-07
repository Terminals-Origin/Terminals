using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Base user authentication values. These are used when ever stored password si required.
    /// </summary>
    internal interface ICredentialBase
    {
        string UserName { get; set; }
        string Domain { get; set; }

        /// <summary>
        /// Gets the encrypted password hash.
        /// </summary>
        string EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the password in not encrypted form.
        /// </summary>
        [XmlIgnore]
        string Password { get; set; }

        /// <summary>
        /// Replaces stored password hash by new one using newKeymaterial
        /// </summary>
        /// <param name="newKeymaterial">key created from master password hash</param>
        void UpdatePasswordByNewKeyMaterial(string newKeymaterial);
    }
}