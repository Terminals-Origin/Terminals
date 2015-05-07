using System;
using System.IO;
using System.Security.Cryptography;

namespace Unified.Encryption {
    public class Encryptor {
        private EncryptTransformer transformer;

        private byte[] initVec;

        private byte[] encKey;


        public byte[] IV {
            get {
                return initVec;
            }

            set {
                initVec = value;
            }
        }

        public byte[] Key {
            get {
                return encKey;
            }
        }

        public Encryptor(EncryptionAlgorithm algId) {
            transformer = new EncryptTransformer(algId);
        }

        public byte[] Encrypt(byte[] bytesData, byte[] bytesKey) {
            MemoryStream memoryStream = new MemoryStream();
            transformer.IV = initVec;
            ICryptoTransform iCryptoTransform = transformer.GetCryptoServiceProvider(bytesKey);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write);
            try {
                cryptoStream.Write(bytesData, 0, (int)bytesData.Length);
            }
            catch (Exception e) {
                //Terminals.Logging.Info(e);
                throw new Exception(String.Concat("Error while writing encrypted data to the stream: \n", e.Message));
            }
            encKey = transformer.Key;
            initVec = transformer.IV;
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return memoryStream.ToArray();
        }
    }

}
