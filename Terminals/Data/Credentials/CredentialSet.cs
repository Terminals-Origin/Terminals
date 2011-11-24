using System;
using System.Xml.Serialization;
using Terminals.Security;

namespace Terminals.Configuration
{
    /// <summary>
    /// Container of stored user authentication.
    /// </summary>
    [Serializable]
    public class CredentialSet
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                this.name = value;
            }
        }

        private string userName;
        public string Username
        {
            get { return userName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                this.userName = value;
            }
        }

        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password hash.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password in not encrypted form.
        /// </summary>
        [XmlIgnore]
        internal string SecretKey
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Password))
                    return PasswordFunctions.DecryptPassword(this.Password);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.Password = String.Empty;
                else
                    this.Password = PasswordFunctions.EncryptPassword(value);
            }
        }

        internal void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            if (!string.IsNullOrEmpty(this.SecretKey))
                this.Password = PasswordFunctions.EncryptPassword(this.SecretKey, newKeymaterial); 
        }

        public override string ToString()
        {
            return String.Format(@"{0}:{1}\{2}", Name, Domain, Username);
        }
    }
}
