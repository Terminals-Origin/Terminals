using System;
using System.Text;
using System.Security.Cryptography;
using Terminals.Configuration;
using Unified.Encryption;

namespace Terminals
{
    internal static class Functions
    {
        private static int keyLength = 24;
        private static int ivLength = 16;
        private static EncryptionAlgorithm EncryptionAlgorithm = EncryptionAlgorithm.Rijndael;

        internal static string DecryptPassword(string encryptedPassword)
        {
           return DecryptPassword(encryptedPassword, Settings.KeyMaterial);
        }

        internal static string DecryptPassword(string encryptedPassword, string keyMaterial)
        {
            try
            {
                if (String.IsNullOrEmpty(encryptedPassword))
                    return encryptedPassword;

                if (keyMaterial == string.Empty)
                    return DecryptByEmptyKey(encryptedPassword);

                return DecryptByKey(encryptedPassword, keyMaterial);
            }
            catch (Exception e)
            {
                Logging.Log.Error("Error Decrypting Password", e);
                return string.Empty;
            }
        }

        private static string DecryptByKey(string encryptedPassword, string keyMaterial)
        {
            string hashedPass = keyMaterial.Substring(0, keyLength);
            byte[] IV = Encoding.Default.GetBytes(keyMaterial.Substring(keyMaterial.Length - ivLength));
            string password = "";
            Decryptor dec = new Decryptor(EncryptionAlgorithm);
            dec.IV = IV;
            byte[] data = dec.Decrypt(Convert.FromBase64String(encryptedPassword), Encoding.Default.GetBytes(hashedPass));
            if (data != null && data.Length > 0)
            {
                password = Encoding.Default.GetString(data);
            }
            return password;
        }

        private static string DecryptByEmptyKey(string encryptedPassword)
        {
            byte[] cyphertext = Convert.FromBase64String(encryptedPassword);
            byte[] b_entropy = Encoding.UTF8.GetBytes(String.Empty);
            byte[] plaintext = ProtectedData.Unprotect(cyphertext, b_entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plaintext);
        }

        internal static string EncryptPassword(string decryptedPassword)
        {
            return EncryptPassword(decryptedPassword, Settings.KeyMaterial);
        }

        internal static string EncryptPassword(string decryptedPassword, string keyMaterial)
        {
            try
            {
                if (keyMaterial == string.Empty)
                    return EncryptByEmptyKey(decryptedPassword);

                return EncryptByKey(decryptedPassword, keyMaterial);
            }
            catch (Exception ec)
            {
                Logging.Log.Error("Error Encrypting Password", ec);
                return string.Empty;
            }
        }

        private static string EncryptByKey(string decryptedPassword, string keyMaterial)
        {
            string hashedPass = keyMaterial.Substring(0, keyLength);
            byte[] IV = Encoding.Default.GetBytes(keyMaterial.Substring(keyMaterial.Length - ivLength));
            Encryptor enc = new Encryptor(EncryptionAlgorithm);
            enc.IV = IV;
            byte[] data = enc.Encrypt(Encoding.Default.GetBytes(decryptedPassword), Encoding.Default.GetBytes(hashedPass));
            if (data != null && data.Length > 0)
            {
                return Convert.ToBase64String(data);
            }

            return string.Empty;
        }

        private static string EncryptByEmptyKey(string decryptedPassword)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(decryptedPassword);
            byte[] b_entropy = Encoding.UTF8.GetBytes(String.Empty);
            byte[] cyphertext = ProtectedData.Protect(plaintext, b_entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(cyphertext);
        }

        internal static string UserDisplayName(string domain, string user)
        {
            return String.IsNullOrEmpty(domain) ? (user) : (domain + "\\" + user);
        }

        internal static string GetErrorMessage(int code)
        {
            //error messages from: http://msdn2.microsoft.com/en-us/aa382170.aspx
            switch (code)
            {
                case 260: return "DNS name lookup failure";
                case 262: return "Out of memory";
                case 264: return "Connection timed out";
                case 516: return "WinSock socket connect failure";
                case 518: return "Out of memory";
                case 520: return "Host not found error";
                case 772: return "WinSock send call failure";
                case 774: return "Out of memory";
                case 776: return "Invalid IP address specified";
                case 1028: return "WinSock recv call failure";
                case 1030: return "Invalid security data";
                case 1032: return "Internal error";
                case 1286: return "Invalid encryption method specified";
                case 1288: return "DNS lookup failed";
                case 1540: return "GetHostByName call failed";
                case 1542: return "Invalid server security data";
                case 1544: return "Internal timer error";
                case 1796: return "Time-out occurred";
                case 1798: return "Failed to unpack server certificate";
                case 2052: return "Bad IP address specified";
                case 2056: return "Internal security error";
                case 2308: return "Socket closed";
                case 2310: return "Internal security error";
                case 2312: return "Licensing time-out";
                case 2566: return "Internal security error";
                case 2822: return "Encryption error";
                case 3078: return "Decryption error";
            }
            return null;
        }
    }
}