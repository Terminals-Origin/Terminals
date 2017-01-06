using System;
using System.Security.Principal;

namespace Terminals.Network
{
  /// <summary>
  /// Windows user name and security ID resolution. Used to identify the users in history
  /// and other user specific place like single instance application per user
  /// </summary>
  internal class WindowsUserIdentifiers
  {
    internal static string GetCurrentUserSid()
    {
      try
      {
        string fullLogin = Environment.UserDomainName + "\\" + Environment.UserName;
        var account = new NTAccount(fullLogin);
        IdentityReference sidReference = account.Translate(typeof(SecurityIdentifier));
        return sidReference.ToString();
      }
      catch
      {
        return null;
      }
    }

    internal static string ResolveUserNameFromSid(string userSid)
    {
        try
        {
            var userIdentifier = new SecurityIdentifier(userSid);
            IdentityReference userLoginReference = userIdentifier.Translate(typeof(NTAccount));
            return userLoginReference.ToString();
        }
        catch
        {
            return null;
        }
    }
  }
}
