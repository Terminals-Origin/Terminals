using System;

namespace Terminals.Data
{
    /// <summary>
    /// General properties, which represent user authentication values
    /// </summary>
    [Serializable]
    public class CredentialBase : ICredentialBase
    {
        public string EncryptedUserName { get; set; }

        public string EncryptedDomain { get; set; }
                
        public string EncryptedPassword { get; set; }

        protected void UpdateFrom(CredentialBase source)
        {
            this.EncryptedUserName = source.EncryptedUserName;
            this.EncryptedDomain = source.EncryptedDomain;
            this.EncryptedPassword = source.EncryptedPassword;
        }
    }
}