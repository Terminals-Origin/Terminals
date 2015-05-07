using System;
using System.IO;
using System.Security.Cryptography;

namespace Unified.Encryption
{
  public class Decryptor
  {
    private DecryptTransformer transformer;

    private byte[] initVec;


    public byte[] IV
    {

      set
      {
        initVec = value;
      }
    }

    public Decryptor(EncryptionAlgorithm algId)
    {
      transformer = new DecryptTransformer(algId);
    }

    public byte[] Decrypt(byte[] bytesData, byte[] bytesKey)
    {
      MemoryStream memoryStream = new MemoryStream();
      transformer.IV = initVec;
      ICryptoTransform iCryptoTransform = transformer.GetCryptoServiceProvider(bytesKey);
      CryptoStream cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write);
      try
      {
        cryptoStream.Write(bytesData, 0, (int)bytesData.Length);
        cryptoStream.FlushFinalBlock();
        cryptoStream.Close();
        byte[] bs = memoryStream.ToArray();
        return bs;
      }
      catch (Exception e)
      {
          //Terminals.Logging.Error("Decryptor Failed", e);
        throw new Exception(String.Concat("Error while writing encrypted data to the stream: \n", e.Message));
      }
    }
  }

}
