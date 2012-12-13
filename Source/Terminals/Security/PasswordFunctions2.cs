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

        /// <summary>
        /// Ensures if, password entered by user is valid against storedPassword.
        /// </summary>
        /// <param name="password">Password entered by user</param>
        /// <param name="storedMasterPassword">Validation key, encrypted by masterPassword key,
        /// which should be identical to key, which we are able to generate from entered password</param>
        internal static bool MasterPasswordIsValid(string password, string storedMasterPassword)
        {
            byte[] keySalt = SplitEncryptedPassword(storedMasterPassword).Item1;
            byte[] key = CalculateMasterPasswordKey(password, keySalt);
            string decryptedValidationKey = DecryptPassword(storedMasterPassword, key);
            return VALIDATION_KEY == decryptedValidationKey;
        }

        /// <summary>
        /// Extracts key salt used to encrypt master password and generate master password key.
        /// </summary>
        /// <param name="storedMasterPassword">Validation key, encrypted by masterPassword key,
        /// which should be identical to key, which we are able to generate from entered password</param>
        internal static byte[] GetMasterPasswordKeySalt(string storedMasterPassword)
        {
            return SplitEncryptedPassword(storedMasterPassword).Item1;
        }

        /// <summary>
        /// Calculates new master password key using random salt.
        /// Because of random salt, always generates unique result for the same master password.
        /// Use only for test purpose.
        /// </summary>
        /// <param name="password">new master password for which the key has to be generated</param>
        internal static string CalculateMasterPasswordKey(string password)
        {
            byte[] keySalt = CreateRandomKeySalt();
            return CalculateMasterPasswordKeyText(password, keySalt);
        }

        /// <summary>
        /// Calculates new master password key using key salt used for stored master password.
        /// Doesn't validate, if the stored master password is valid. Use only this overload on real data.
        /// </summary>
        /// <param name="password">master password for which the key has to be generated</param>
        /// <param name="storedMasterPassword">Not empty stored <see cref="VALIDATION_KEY"/>
        /// from which master password salt will be extracted</param>
        internal static string CalculateMasterPasswordKey(string password, string storedMasterPassword)
        {
            byte[] keySalt = GetMasterPasswordKeySalt(storedMasterPassword);
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
        internal static string ComputeStoredMasterPasswordKey(string password)
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