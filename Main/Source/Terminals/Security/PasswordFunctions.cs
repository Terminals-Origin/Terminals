using System;
using System.Text;
using System.Security.Cryptography;
using Unified.Encryption;
using Unified.Encryption.Hash;

namespace Terminals.Security
{
    /// <summary>
    /// Security encryption/decryption logic for stored passwords till version 2.0.
    /// Use only for upgrades from previous version.
    /// Implementation problems:
    /// - master password hash is stored in config file
    /// - rfbk2 wasn't used to make the key stronger
    /// - key generated from identical master password is always the same
    /// </summary>
    internal static class PasswordFunctions
    {
        internal const int KEY_LENGTH = 24;
        internal const int IV_LENGTH = 16;
        private const EncryptionAlgorithm ENCRYPTION_ALGORITHM = EncryptionAlgorithm.Rijndael;

        internal static bool MasterPasswordIsValid(string password, string storedPassword)
        {
            string hashToCheck = string.Empty;
            if (!string.IsNullOrEmpty(password))
                hashToCheck = ComputeMasterPasswordHash(password);
            return hashToCheck == storedPassword;
        }

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
            byte[] initializationVector = GetInitializationVector(keyMaterial);
            byte[] passwordKey = GetPasswordKey(keyMaterial);
            byte[] passwordBytes = Convert.FromBase64String(encryptedPassword);
            byte[] data = DecryptByKey(passwordBytes, initializationVector, passwordKey);

            if (data != null && data.Length > 0)
                return Encoding.Default.GetString(data);

            return string.Empty;
        }

        internal static byte[] DecryptByKey(byte[] encryptedPassword, byte[] initializationVector, byte[] passwordKey)
        {
            var dec = new Decryptor(ENCRYPTION_ALGORITHM);
            dec.IV = initializationVector;
            return dec.Decrypt(encryptedPassword, passwordKey);
        }

        private static string DecryptByEmptyKey(string encryptedPassword)
        {
            byte[] cyphertext = Convert.FromBase64String(encryptedPassword);
            byte[] entropy = Encoding.UTF8.GetBytes(String.Empty);
            byte[] plaintext = ProtectedData.Unprotect(cyphertext, entropy, DataProtectionScope.CurrentUser);
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
            var initializationVector = GetInitializationVector(keyMaterial);
            byte[] passwordKey = GetPasswordKey(keyMaterial);
            byte[] passwordBytes = Encoding.Default.GetBytes(decryptedPassword);
            byte[] data = EncryptByKey(passwordBytes, initializationVector, passwordKey);

            if (data != null && data.Length > 0)
                return Convert.ToBase64String(data);

            return String.Empty;
        }

        internal static byte[] EncryptByKey(byte[] decryptedPassword, byte[] initializationVector, byte[] passwordKey)
        {
            Encryptor enc = new Encryptor(ENCRYPTION_ALGORITHM);
            enc.IV = initializationVector;
            return enc.Encrypt(decryptedPassword, passwordKey);
        }

        private static string EncryptByEmptyKey(string decryptedPassword)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(decryptedPassword);
            byte[] entropy = Encoding.UTF8.GetBytes(String.Empty);
            byte[] cyphertext = ProtectedData.Protect(plaintext, entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(cyphertext);
        }

        private static byte[] GetInitializationVector(string keyMaterial)
        {
            var keyPart = keyMaterial.Substring(keyMaterial.Length - IV_LENGTH);
            return Encoding.Default.GetBytes(keyPart);
        }

        private static byte[] GetPasswordKey(string keyMaterial)
        {
            string keyChars = keyMaterial.Substring(0, KEY_LENGTH);
            return Encoding.Default.GetBytes(keyChars);
        }
    }
}