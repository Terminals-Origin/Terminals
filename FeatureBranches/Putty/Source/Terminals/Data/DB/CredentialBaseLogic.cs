namespace Terminals.Data.DB
{
    internal partial class DbCredentialBase : ICredentialBase
    {
        protected void CopyTo(DbCredentialBase copy)
        {
            copy.EncryptedUserName = this.EncryptedUserName;
            copy.EncryptedDomain = this.EncryptedDomain;
            copy.EncryptedPassword = this.EncryptedPassword;
        }
    }
}
