using System;
using System.Security;
using System.Security.Cryptography;

namespace Terminals.Security
{
    // Strategy links
    // http://msdn.microsoft.com/en-us/magazine/cc163913.aspx
    // http://blogs.msdn.com/b/shawnfa/archive/2004/04/14/generating-a-key-from-a-password.aspx?Redirected=true
    // http://msdn.microsoft.com/en-us/library/system.security.cryptography.rfc2898derivebytes.aspx

    /// <summary>
    /// Security encryption/decryption logic for stored passwords after version 2.0.
    /// This version resolves previous version problems.
    /// </summary>
    internal class PasswordFunctions2
    {
        internal static bool MasterPasswordIsValid(string password, string storedPassword)
        {
            throw new NotImplementedException();
        }

        public static string CalculateMasterPasswordKey(string password)
        {
            throw new NotImplementedException();
            var rfbk2 = new Rfc2898DeriveBytes("", new byte[] { }, 1000);
            var salt = rfbk2.GetBytes(16);
            SecureString key = new SecureString();
            PasswordFunctions.CalculateMasterPasswordKey(password);
        }

        public static string ComputeMasterPasswordHash(string password)
        {
            throw new NotImplementedException();
        }

        internal static string EncryptPassword(string password, string keyMaterial)
        {
            throw new NotImplementedException();
        }

        internal static string DecryptPassword(string encryptedPassword, string keyMaterial)
        {
            throw new NotImplementedException();
        }
    }
}