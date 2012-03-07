using System;
using System.Security.Cryptography;

namespace Unified.Encryption {
    class EncryptTransformer {
        private EncryptionAlgorithm algorithmID;

        private byte[] initVec;

        private byte[] encKey;


        internal byte[] IV {
            get {
                return initVec;
            }

            set {
                initVec = value;
            }
        }

        internal byte[] Key {
            get {
                return encKey;
            }
        }

        internal EncryptTransformer(EncryptionAlgorithm algId) {
            algorithmID = algId;
        }

        internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey) {
            ICryptoTransform iCryptoTransform;

            switch (algorithmID) {
                case EncryptionAlgorithm.Des:
                    DES dES = new DESCryptoServiceProvider();
                    dES.Mode = CipherMode.CBC;
                    if (bytesKey == null) {
                        encKey = dES.Key;
                    }
                    else {
                        dES.Key = bytesKey;
                        encKey = dES.Key;
                    }
                    if (initVec == null) {
                        initVec = dES.IV;
                    }
                    else {
                        dES.IV = initVec;
                    }
                    iCryptoTransform = dES.CreateEncryptor();
                    break;

                case EncryptionAlgorithm.TripleDes:
                    TripleDES tripleDES = new TripleDESCryptoServiceProvider();
                    tripleDES.Mode = CipherMode.CBC;
                    if (bytesKey == null) {
                        encKey = tripleDES.Key;
                    }
                    else {
                        tripleDES.Key = bytesKey;
                        encKey = tripleDES.Key;
                    }
                    if (initVec == null) {
                        initVec = tripleDES.IV;
                    }
                    else {
                        tripleDES.IV = initVec;
                    }
                    iCryptoTransform = tripleDES.CreateEncryptor();
                    break;

                case EncryptionAlgorithm.Rc2:
                    RC2 rC2 = new RC2CryptoServiceProvider();
                    rC2.Mode = CipherMode.CBC;
                    if (bytesKey == null) {
                        encKey = rC2.Key;
                    }
                    else {
                        rC2.Key = bytesKey;
                        encKey = rC2.Key;
                    }
                    if (initVec == null) {
                        initVec = rC2.IV;
                    }
                    else {
                        rC2.IV = initVec;
                    }
                    iCryptoTransform = rC2.CreateEncryptor();
                    break;

                case EncryptionAlgorithm.Rijndael:
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;
                    if (bytesKey == null) {
                        encKey = rijndael.Key;
                    }
                    else {
                        rijndael.Key = bytesKey;
                        encKey = rijndael.Key;
                    }
                    if (initVec == null) {
                        initVec = rijndael.IV;
                    }
                    else {
                        rijndael.IV = initVec;
                    }
                    iCryptoTransform = rijndael.CreateEncryptor();
                    break;

                default:
                    throw new CryptographicException(String.Concat("Algorithm ID \'", algorithmID, "\' not supported."));
            }
            return iCryptoTransform;
        }
    }

}
