using System;
using System.Xml.Serialization;
using Terminals.Security;

namespace Terminals.Data
{
    /// <summary>
    /// Container of stored user authentication.
    /// </summary>
    [Serializable]
    public class CredentialSet : ICredentialSet
    {
        private Guid id = Guid.NewGuid();
        [XmlAttribute("id")]
        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

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
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password in not encrypted form.
        /// </summary>
        [XmlIgnore]
        string ICredentialSet.SecretKey
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

        void ICredentialSet.UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            string secret = ((ICredentialSet)this).SecretKey;
            if (!string.IsNullOrEmpty(secret))
                this.Password = PasswordFunctions.EncryptPassword(secret, newKeymaterial); 
        }

        public override string ToString()
        {
            return String.Format(@"{0}:{1}\{2}", Name, Domain, Username);
        }
    }
}
