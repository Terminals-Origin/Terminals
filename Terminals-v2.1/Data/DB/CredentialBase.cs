using System;
using Terminals.Security;

namespace Terminals.Data.DB
{
  internal partial class CredentialBase : ICredentialBase
  {
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

    public void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
    {
      string secret = ((ICredentialBase)this).Password;
      if (!string.IsNullOrEmpty(secret))
        this.EncryptedPassword = PasswordFunctions.EncryptPassword(secret, newKeymaterial);
    }
  }
}
