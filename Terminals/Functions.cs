using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Terminals {
    internal static class Functions {
        public static Unified.Encryption.EncryptionAlgorithm EncryptionAlgorithm = Unified.Encryption.EncryptionAlgorithm.Rijndael;
        internal static string DecryptPassword(string encryptedPassword) {
            if (String.IsNullOrEmpty(encryptedPassword))
                return encryptedPassword;
            try {
                if(Settings.TerminalsPassword==string.Empty) {
            		byte[] cyphertext = Convert.FromBase64String(encryptedPassword);
            		byte[] b_entropy = Encoding.UTF8.GetBytes(String.Empty);
		            byte[] plaintext = ProtectedData.Unprotect(cyphertext, b_entropy, DataProtectionScope.CurrentUser);
		            return Encoding.UTF8.GetString(plaintext);
                } else {
                    string hashedPass = Settings.TerminalsPassword.Substring(0, keyLength);
                    byte[] IV = System.Text.Encoding.Default.GetBytes(Settings.TerminalsPassword.Substring(Settings.TerminalsPassword.Length - ivLength));
                    //string hashedPass = Settings.TerminalsPassword.Substring(0, keyLength);
                    string password = "";
                    //System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(encryptedPassword))
                    Unified.Encryption.Decryptor dec = new Unified.Encryption.Decryptor(EncryptionAlgorithm);
                    dec.IV = IV;
                    byte[] data = dec.Decrypt(System.Convert.FromBase64String(encryptedPassword), System.Text.Encoding.Default.GetBytes(hashedPass));
                    if (data != null && data.Length > 0) {
                        password = System.Text.Encoding.Default.GetString(data);
                    }
                    return password;
                }
            } catch (Exception e) {
                Terminals.Logging.Log.Info("", e);
                return "";
            }
        }
        internal static int keyLength = 24;
        internal static int ivLength = 16;
        internal static string EncryptPassword(string decryptedPassword) {
            if (Settings.TerminalsPassword == string.Empty) {
        		byte[] plaintext = Encoding.UTF8.GetBytes(decryptedPassword);
           		byte[] b_entropy = Encoding.UTF8.GetBytes(String.Empty);
           		byte[] cyphertext = ProtectedData.Protect(plaintext, b_entropy, DataProtectionScope.CurrentUser);
        		return Convert.ToBase64String(cyphertext);
            } else {
                string password = "";
                try {
                    string hashedPass = Settings.TerminalsPassword.Substring(0, keyLength);
                    byte[] IV = System.Text.Encoding.Default.GetBytes(Settings.TerminalsPassword.Substring(Settings.TerminalsPassword.Length - ivLength));
                    Unified.Encryption.Encryptor enc = new Unified.Encryption.Encryptor(EncryptionAlgorithm);
                    enc.IV = IV;
                    byte[] data = enc.Encrypt(System.Text.Encoding.Default.GetBytes(decryptedPassword), System.Text.Encoding.Default.GetBytes(hashedPass));
                    if (data != null && data.Length > 0) {
                        password = Convert.ToBase64String(data);
                        //password = System.Text.Encoding.Default.GetString(data);
                    }
                } catch (Exception ec) {
                    Terminals.Logging.Log.Info("", ec);
                }
                return password;
                
            }
        }

        internal static string UserDisplayName(string domain, string user) {
            return String.IsNullOrEmpty(domain) ? (user) : (domain + "\\" + user);
        }
    }
}