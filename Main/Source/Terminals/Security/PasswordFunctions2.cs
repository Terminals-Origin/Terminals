using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Terminals.Security
{
    // Strategy links
    // http://msdn.microsoft.com/en-us/magazine/cc163913.aspx
    // http://blogs.msdn.com/b/shawnfa/archive/2004/04/14/generating-a-key-from-a-password.aspx?Redirected=true
    // http://msdn.microsoft.com/en-us/library/system.security.cryptography.rfc2898derivebytes.aspx

    /// <summary>
    /// Security encryption/decryption logic for stored passwords after version 2.0.
    /// This version resolves previous version problems.
    /// Used Algorithm and length of key and salt are identical with previous version.
    /// Passwords are stored as text combined of initialization vector + encrypted password.
    /// Master password key and stored password keys are based on random initialization vector.
    /// There is no stored master password key hash.
    /// </summary>
    internal class PasswordFunctions2
    {
        /// <summary>
        /// Some random passwords used to be stored in an encrypted form,
        /// as replacement of stored hash from previous version
        /// </summary>
        private const string VALIDATION_KEY = "validation@Key.2645WR:GN?#897";
        private const int ITERATION_COUNT = 1212;
        private static readonly RandomNumberGenerator saltGenerator = RandomNumberGenerator.Create();

        internal static bool MasterPasswordIsValid(string password, string storedPassword)
        {
            byte[] keySalt = SplitEncryptedPassword(storedPassword).Item1;
            byte[] key = CalculateMasterPasswordKey(password, keySalt);
            string decryptedValidationKey = DecryptPassword(storedPassword, key);
            return VALIDATION_KEY == decryptedValidationKey;
        }

        internal static string CalculateMasterPasswordKey(string password)
        {
            byte[] keySalt = CreateRandomKeySalt();
            return CalculateMasterPasswordKeyText(password, keySalt);
        }

        private static string CalculateMasterPasswordKeyText(string password, byte[] keySalt)
        {
            byte[] key = CalculateMasterPasswordKey(password, keySalt);
            return Convert.ToBase64String(key);
        }

        private static byte[] CalculateMasterPasswordKey(string password, byte[] keySalt)
        {
            var rfbk2 = new Rfc2898DeriveBytes(password, keySalt, ITERATION_COUNT);
            return rfbk2.GetBytes(PasswordFunctions.KEY_LENGTH);            
        }

        /// <summary>
        /// Replacement of v1 passwords <see cref="PasswordFunctions.ComputeMasterPasswordHash"/>
        /// </summary>
        internal static string ComputeMasterPasswordStoredKey(string password)
        {
            byte[] keySalt = CreateRandomKeySalt();
            byte[] key = CalculateMasterPasswordKey(password, keySalt);
            return EncryptPassword(VALIDATION_KEY, key, keySalt);
        }

        internal static string EncryptPassword(string password, string keyMaterial)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            byte[] initializationVector = CreateRandomKeySalt();
            byte[] passwordKey = Convert.FromBase64String(keyMaterial);
            return EncryptPassword(password, passwordKey, initializationVector);
        }

        private static string EncryptPassword(string password, byte[] passwordKey, byte[] initializationVector)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] encryptedPassword = PasswordFunctions.EncryptByKey(passwordBytes, initializationVector, passwordKey);
            return ConcatenatePasswordParts(initializationVector, encryptedPassword);
        }

        internal static string DecryptPassword(string encryptedPassword, string keyMaterial)
        {
            if (string.IsNullOrEmpty(encryptedPassword) || encryptedPassword.Length < PasswordFunctions.IV_LENGTH)
                return string.Empty;
            
            byte[] passwordKey = Convert.FromBase64String(keyMaterial);
            return DecryptPassword(encryptedPassword, passwordKey);
        }

        private static string DecryptPassword(string encryptedPassword, byte[] passwordKey)
        {
            Tuple<byte[], byte[]> passwordParts = SplitEncryptedPassword(encryptedPassword);
            byte[] decrypted = PasswordFunctions.DecryptByKey(passwordParts.Item2, passwordParts.Item1, passwordKey);
            return Encoding.Unicode.GetString(decrypted);
        }

        private static string ConcatenatePasswordParts(byte[] initializationVector, byte[] encryptedPassword)
        {
            byte[] encryptedResult = new byte[initializationVector.Length + encryptedPassword.Length];
            initializationVector.CopyTo(encryptedResult, 0);
            encryptedPassword.CopyTo(encryptedResult, initializationVector.Length);
            return Convert.ToBase64String(encryptedResult);
        }

        /// <summary>
        /// Extracts initialization vector as Item1 and password bytes as Item2 from encryptedPassword.
        /// </summary>
        private static Tuple<byte[], byte[]> SplitEncryptedPassword(string encryptedPassword)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            byte[] initializationVector = encryptedBytes.Take(PasswordFunctions.IV_LENGTH).ToArray();
            byte[] passwordPart = encryptedBytes.Skip(PasswordFunctions.IV_LENGTH).ToArray();
            return new Tuple<byte[], byte[]>(initializationVector, passwordPart);
        }

        private static byte[] CreateRandomKeySalt()
        {
            var initializationVector = new byte[PasswordFunctions.IV_LENGTH];
            saltGenerator.GetBytes(initializationVector);
            return initializationVector;
        }
    }
}