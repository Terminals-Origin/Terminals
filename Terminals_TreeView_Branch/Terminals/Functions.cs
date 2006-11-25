using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography; 

namespace Terminals
{
    internal static class Functions
    {
        internal static string DecryptPassword(string encryptedPassword)
        {
            if (String.IsNullOrEmpty(encryptedPassword))
                return encryptedPassword;

            try
            {
                return DPAPI.Decrypt(encryptedPassword);
            }
            catch
            {
                return "";
            }
        }

        internal static string EncryptPassword(string decryptedPassword)
        {
            return DPAPI.Encrypt(DPAPI.KeyType.UserKey, decryptedPassword);
        }
    }
}
