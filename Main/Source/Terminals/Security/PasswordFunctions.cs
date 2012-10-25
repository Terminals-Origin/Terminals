using System;
using System.Text;
using System.Security.Cryptography;
using Unified.Encryption;
using Unified.Encryption.Hash;
using Terminals.Data;

namespace Terminals.Security
{
    internal static class PasswordFunctions
    {
        private const int KEY_LENGTH = 24;
        private const int IV_LENGTH = 16;
        private static EncryptionAlgorithm EncryptionAlgorithm = EncryptionAlgorithm.Rijndael;

        internal static string CalculateMasterPasswordKey(string password)
        {
            if (String.IsNullOrEmpty(password))
                return String.Empty;
            String hashToCheck = ComputeMasterPasswordHash(password);
            return ComputeMasterPasswordHash(password + hashToCheck);
        }

        internal static string ComputeMasterPasswordHash(string password)
        {
          return Hash.GetHash(password, Hash.HashType.SHA512);
        }

        internal static string DecryptPassword(string encryptedPassword, string keyMaterial)
        {
            try
            {
                if (String.IsNullOrEmpty(encryptedPassword))
                    return encryptedPassword;

                if (keyMaterial == String.Empty)
                    return DecryptByEmptyKey(encryptedPassword);

                return DecryptByKey(encryptedPassword, keyMaterial);
            }
            catch (Exception e)
            {
                Logging.Log.Error("Error Decrypting Password", e);
                return String.Empty;
            }
        }

        private static string DecryptByKey(string encryptedPassword, string keyMaterial)
        {
            string hashedPass = keyMaterial.Substring(0, KEY_LENGTH);
            byte[] IV = Encoding.Default.GetBytes(keyMaterial.Substring(keyMaterial.Length - IV_LENGTH));
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

        internal static string EncryptPassword(string decryptedPassword, string keyMaterial)
        {
            try
            {
                if (String.IsNullOrEmpty(decryptedPassword))
                    return decryptedPassword;

                if (keyMaterial == String.Empty)
                    return EncryptByEmptyKey(decryptedPassword);

                return EncryptByKey(decryptedPassword, keyMaterial);
            }
            catch (Exception ec)
            {
                Logging.Log.Error("Error Encrypting Password", ec);
                return String.Empty;
            }
        }

        private static string EncryptByKey(string decryptedPassword, string keyMaterial)
        {
            string hashedPass = keyMaterial.Substring(0, KEY_LENGTH);
            byte[] IV = Encoding.Default.GetBytes(keyMaterial.Substring(keyMaterial.Length - IV_LENGTH));
            Encryptor enc = new Encryptor(EncryptionAlgorithm);
            enc.IV = IV;
            byte[] data = enc.Encrypt(Encoding.Default.GetBytes(decryptedPassword), Encoding.Default.GetBytes(hashedPass));
            if (data != null && data.Length > 0)
            {
                return Convert.ToBase64String(data);
            }

            return String.Empty;
        }

        private static string EncryptByEmptyKey(string decryptedPassword)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(decryptedPassword);
            byte[] b_entropy = Encoding.UTF8.GetBytes(String.Empty);
            byte[] cyphertext = ProtectedData.Protect(plaintext, b_entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(cyphertext);
        }
    }
}