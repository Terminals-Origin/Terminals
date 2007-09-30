/* ---------------------------------------------------------------------------
 *
 * Copyright (c) Routrek Networks, Inc.    All Rights Reserved..
 * 
 * This file is a part of the Granados SSH Client Library that is subject to
 * the license included in the distributed package.
 * You may not use this file except in compliance with the license.
 * 
 * ---------------------------------------------------------------------------
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Routrek.PKI;
using Routrek.SSHC;
using Routrek.Toolkit;

namespace Routrek.SSHCV2
{

	public class SSH2UserAuthKey {

		private const int MAGIC_VAL = 0x3f6ff9eb;

		private KeyPair _keypair;

		public SSH2UserAuthKey(KeyPair kp) {
			_keypair = kp;
		}

		public PublicKeyAlgorithm Algorithm {
			get {
				return _keypair.Algorithm;
			}
		}
		public KeyPair KeyPair {
			get {
				return _keypair;
			}
		}

		public byte[] Sign(byte[] data) {
			PublicKeyAlgorithm a = _keypair.Algorithm;
			if(a==PublicKeyAlgorithm.RSA)
				return ((RSAKeyPair)_keypair).SignWithSHA1(data);
			else
				return ((DSAKeyPair)_keypair).Sign(new SHA1CryptoServiceProvider().ComputeHash(data));
		}
		public byte[] GetPublicKeyBlob() {
			SSH2DataWriter w = new SSH2DataWriter();
			w.Write(SSH2Util.PublicKeyAlgorithmName(_keypair.Algorithm));
			_keypair.PublicKey.WriteTo(w);
			return w.ToByteArray();
		}
		

		public static byte[] PassphraseToKey(string passphrase, int length) {
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] pp = Encoding.UTF8.GetBytes(passphrase);
			int hashlen = md5.HashSize/8;
			byte[] buf = new byte[((length + hashlen) / hashlen) * hashlen];
			int offset = 0;
			
			while(offset < length) {
				MemoryStream s = new MemoryStream();
				s.Write(pp, 0, pp.Length);
				if(offset > 0) s.Write(buf, 0, offset);
				Array.Copy(md5.ComputeHash(s.ToArray()), 0, buf, offset, hashlen);
				offset += hashlen;
				md5.Initialize();
			}

			byte[] key = new byte[length];
			Array.Copy(buf, 0, key, 0, length);
			return key;
		}

		/*
		 * Format style note
		 *  ---- BEGIN SSH2 ENCRYPTED PRIVATE KEY ----
		 *  Comment: *******
		 *  <base64-encoded body>
		 *  ---- END SSH2 ENCRYPTED PRIVATE KEY ----
		 * 
		 *  body = MAGIC_VAL || body-length || type(string) || encryption-algorithm-name(string) || encrypted-body(string)
		 *  encrypted-body = array of BigInteger(algorithm-specific)
		 */ 
		public static SSH2UserAuthKey FromSECSHStyleStream(Stream strm, string passphrase) {
			StreamReader r = new StreamReader(strm, Encoding.ASCII);
			string l = r.ReadLine();
			if(l==null || l!="---- BEGIN SSH2 ENCRYPTED PRIVATE KEY ----") throw new SSHException("Wrong key format");

			l = r.ReadLine();
			StringBuilder buf = new StringBuilder();
			while(l!="---- END SSH2 ENCRYPTED PRIVATE KEY ----") {
				if(l.IndexOf(':')==-1)
					buf.Append(l);
				else {
					while(l.EndsWith("\\")) l = r.ReadLine();
				}
				l = r.ReadLine();
				if(l==null) throw new SSHException("Key is broken");
			}
			r.Close();

			byte[] keydata = Base64.Decode(Encoding.ASCII.GetBytes(buf.ToString()));
			//Debug.WriteLine(DebugUtil.DumpByteArray(keydata));

			/*
			FileStream fs = new FileStream("C:\\IOPort\\keydata1.bin", FileMode.Create);
			fs.Write(keydata, 0, keydata.Length);
			fs.Close();
			*/

			SSH2DataReader re = new SSH2DataReader(keydata);
			int    magic         = re.ReadInt32();
			if(magic!=MAGIC_VAL) throw new SSHException("key file is broken");
			int    privateKeyLen = re.ReadInt32();
			string type          = Encoding.ASCII.GetString(re.ReadString());

			string ciphername    = Encoding.ASCII.GetString(re.ReadString());
			int    bufLen        = re.ReadInt32();
			if(ciphername!="none") {
				CipherAlgorithm algo = CipherFactory.SSH2NameToAlgorithm(ciphername);
				byte[] key = PassphraseToKey(passphrase, CipherFactory.GetKeySize(algo));
				Cipher c = CipherFactory.CreateCipher(SSHProtocol.SSH2, algo, key);
				byte[] tmp = new Byte[re.Image.Length-re.Offset];
				c.Decrypt(re.Image, re.Offset, re.Image.Length-re.Offset, tmp, 0);
				re = new SSH2DataReader(tmp);
			}

			int parmLen          = re.ReadInt32();
			if(parmLen<0 || parmLen>re.Rest)
				throw new SSHException(Strings.GetString("WrongPassphrase"));

			if(type.IndexOf("if-modn")!=-1) {
				//mindterm mistaken this order of BigIntegers
				BigInteger e = re.ReadBigIntWithBits();
				BigInteger d = re.ReadBigIntWithBits();
				BigInteger n = re.ReadBigIntWithBits();
				BigInteger u = re.ReadBigIntWithBits();
				BigInteger p = re.ReadBigIntWithBits();
				BigInteger q = re.ReadBigIntWithBits();
				return new SSH2UserAuthKey(new RSAKeyPair(e, d, n, u, p, q));
			}
			else if(type.IndexOf("dl-modp")!=-1) {
				if(re.ReadInt32()!=0) throw new SSHException("DSS Private Key File is broken");
				BigInteger p = re.ReadBigIntWithBits();
				BigInteger g = re.ReadBigIntWithBits();
				BigInteger q = re.ReadBigIntWithBits();
				BigInteger y = re.ReadBigIntWithBits();
				BigInteger x = re.ReadBigIntWithBits();
				return new SSH2UserAuthKey(new DSAKeyPair(p, g, q, y, x));
			}
			else
				throw new SSHException("unknown authentication method "+type);

		}
		public static SSH2UserAuthKey FromSECSHStyleFile(string filename, string passphrase) {
			return FromSECSHStyleStream(new FileStream(filename, FileMode.Open, FileAccess.Read), passphrase);
		}

		public void WritePrivatePartInSECSHStyleFile(Stream dest, string comment, string passphrase) {
			
			//step1 key body
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.Write(0); //this field is filled later
			if(_keypair.Algorithm==PublicKeyAlgorithm.RSA) {
				RSAKeyPair rsa = (RSAKeyPair)_keypair;
				RSAPublicKey pub = (RSAPublicKey)_keypair.PublicKey;
				wr.WriteBigIntWithBits(pub.Exponent);
				wr.WriteBigIntWithBits(rsa.D);
				wr.WriteBigIntWithBits(pub.Modulus);
				wr.WriteBigIntWithBits(rsa.U);
				wr.WriteBigIntWithBits(rsa.P);
				wr.WriteBigIntWithBits(rsa.Q);
			}
			else {
				DSAKeyPair dsa = (DSAKeyPair)_keypair;
				DSAPublicKey pub = (DSAPublicKey)_keypair.PublicKey;
				wr.Write(0);
				wr.WriteBigIntWithBits(pub.P);
				wr.WriteBigIntWithBits(pub.G);
				wr.WriteBigIntWithBits(pub.Q);
				wr.WriteBigIntWithBits(pub.Y);
				wr.WriteBigIntWithBits(dsa.X);
			}

			int padding_len = 0;
			if(passphrase!=null) {
				padding_len = 8 - (int)wr.Length % 8;
				wr.Write(new byte[padding_len]);
			}
			byte[] encrypted_body = wr.ToByteArray();
			SSHUtil.WriteIntToByteArray(encrypted_body, 0, encrypted_body.Length - padding_len - 4);

			//encrypt if necessary
			if(passphrase!=null) {
				Cipher c = CipherFactory.CreateCipher(SSHProtocol.SSH2, CipherAlgorithm.TripleDES, PassphraseToKey(passphrase,24));
				Debug.Assert(encrypted_body.Length % 8 ==0);
				byte[] tmp = new Byte[encrypted_body.Length];
				c.Encrypt(encrypted_body, 0, encrypted_body.Length, tmp, 0);
				encrypted_body = tmp;
			}

			//step2 make binary key data
			wr = new SSH2DataWriter();
			wr.Write(MAGIC_VAL);
			wr.Write(0); //for total size
			wr.Write(_keypair.Algorithm==PublicKeyAlgorithm.RSA?
				"if-modn{sign{rsa-pkcs1-sha1},encrypt{rsa-pkcs1v2-oaep}}" :
				"dl-modp{sign{dsa-nist-sha1},dh{plain}}");

			wr.Write(passphrase==null? "none" : "3des-cbc");
			wr.WriteAsString(encrypted_body);

			byte[] rawdata = wr.ToByteArray();
			SSHUtil.WriteIntToByteArray(rawdata, 4, rawdata.Length); //fix total length

			//step3 write final data
			StreamWriter sw = new StreamWriter(dest, Encoding.ASCII);
			sw.WriteLine("---- BEGIN SSH2 ENCRYPTED PRIVATE KEY ----");
			if(comment!=null)
				WriteKeyFileBlock(sw, "Comment: " + comment, true);
			WriteKeyFileBlock(sw, Encoding.ASCII.GetString(Base64.Encode(rawdata)), false);
			sw.WriteLine("---- END SSH2 ENCRYPTED PRIVATE KEY ----");
			sw.Close();

		}

		public void WritePublicPartInSECSHStyle(Stream dest, string comment) {
			StreamWriter sw = new StreamWriter(dest, Encoding.ASCII);
			sw.WriteLine("---- BEGIN SSH2 PUBLIC KEY ----");
			if(comment!=null)
				WriteKeyFileBlock(sw, "Comment: " + comment, true);
			WriteKeyFileBlock(sw, FormatBase64EncodedPublicKeyBody(), false);
			sw.WriteLine("---- END SSH2 PUBLIC KEY ----");
			sw.Close();

		}
		public void WritePublicPartInOpenSSHStyle(Stream dest) {
			StreamWriter sw = new StreamWriter(dest, Encoding.ASCII);
			sw.Write(SSH2Util.PublicKeyAlgorithmName(_keypair.Algorithm));
			sw.Write(' ');
			sw.WriteLine(FormatBase64EncodedPublicKeyBody());
			sw.Close();
		}
		private string FormatBase64EncodedPublicKeyBody() {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.Write(SSH2Util.PublicKeyAlgorithmName(_keypair.Algorithm));
			_keypair.PublicKey.WriteTo(wr);
			
			return Encoding.ASCII.GetString(Base64.Encode(wr.ToByteArray()));
		}

		private static void WriteKeyFileBlock(StreamWriter sw, string data, bool escape_needed) {
			char[] d = data.ToCharArray();
			int cursor = 0;
			const int maxlen = 70;
			while(cursor < d.Length) {
				if(maxlen >= d.Length-cursor)
					sw.WriteLine(d, cursor, d.Length-cursor);
				else {
					if(escape_needed) {
						sw.Write(d, cursor, maxlen-1);
						sw.WriteLine('\\');
						cursor--;
					}
					else
						sw.WriteLine(d, cursor, maxlen);
				}

				cursor += maxlen;
			}
		}
	}

}
