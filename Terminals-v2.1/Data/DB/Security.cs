using System;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class Security : ISecurityOptions
    {
        public string UserName
        {
            get { return this.CredentialBase.UserName; }
            set { this.CredentialBase.UserName = value; }
        }

        public string Domain
        {
            get { return this.CredentialBase.Domain; }
            set { this.CredentialBase.Domain = value; }
        }

        public string EncryptedPassword
        {
            get { return this.CredentialBase.EncryptedPassword; }
            set { this.CredentialBase.EncryptedPassword = value; }
        }

        string ICredentialBase.Password
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EncryptedPassword))
                    return PasswordFunctions.DecryptPassword(this.EncryptedPassword);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                else
                    this.EncryptedPassword = PasswordFunctions.EncryptPassword(value);
            }
        }

        public Guid Credential
        {
            get
            {
                if (this.CredentialSet != null)
                {
                    return ((ICredentialSet) this.CredentialSet).Id;
                }
                return Guid.Empty;
            }
            set
            {
                if (value != Guid.Empty)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    this.CredentialSet = null;
                }
            }
        }

        internal Security Copy()
        {
            return new Security
            {
                CredentialSet = this.CredentialSet,
                Domain = this.Domain,
                EncryptedPassword = this.EncryptedPassword,
                UserName = this.UserName
            };
        }

        public ISecurityOptions GetResolvedCredentials()
        {
            throw new NotImplementedException();
        }

        public void UpdateFromCredential(ICredentialSet source)
        {
            throw new NotImplementedException();
        } 
        
        public void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            throw new NotImplementedException();
        }
    }
}
