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
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

using Routrek.SSHC;

namespace Routrek.SSHCV1
{
	/// <summary>
	/// private key for user authentication
	/// </summary>
	public class SSH1UserAuthKey
	{
		private BigInteger _modulus;
		private BigInteger _publicExponent;
		private BigInteger _privateExponent;
		private BigInteger _primeP;
		private BigInteger _primeQ;
		private BigInteger _crtCoefficient;

		public BigInteger PublicModulus {
			get {
				return _modulus;
			}
		}
		public BigInteger PublicExponent {
			get {
				return _publicExponent;
			}
		}

		/// <summary>
		/// constructs from file
		/// </summary>
		/// <param name="path">file name</param>
		/// <param name="passphrase">passphrase or empty string if passphrase is not required</param>
		public SSH1UserAuthKey(string path, string passphrase)
		{
			Stream s = File.Open(path, FileMode.Open);
			byte[] header = new byte[32];
			s.Read(header, 0, header.Length);
            if(Encoding.Default.GetString(header) != "SSH PRIVATE KEY FILE FORMAT 1.1\n")
				throw new SSHException(String.Format(Strings.GetString("BrokenKeyFile"), path));

			SSH1DataReader reader = new SSH1DataReader(ReadAll(s));
			s.Close();

			byte[] cipher = reader.Read(2); //first 2 bytes indicates algorithm and next 8 bytes is space
			reader.Read(8);

			_modulus = reader.ReadMPInt();
			_publicExponent = reader.ReadMPInt();
			byte[] comment = reader.ReadString();
			byte[] prvt = reader.ReadAll();
			//ïKóvÇ»ÇÁïúçÜ
			CipherAlgorithm algo = (CipherAlgorithm)cipher[1];
			if(algo!=0) {
				Cipher c = CipherFactory.CreateCipher(SSHProtocol.SSH1, algo, ConvertToKey(passphrase));
				byte[] buf = new byte[prvt.Length];
				c.Decrypt(prvt, 0, prvt.Length, buf, 0);
				prvt = buf;
			}

			SSH1DataReader prvtreader = new SSH1DataReader(prvt);
			byte[] mark = prvtreader.Read(4);
			if(mark[0]!=mark[2] || mark[1]!=mark[3])
				throw new SSHException(Strings.GetString("WrongPassphrase"));

			_privateExponent = prvtreader.ReadMPInt();
			_crtCoefficient = prvtreader.ReadMPInt();
			_primeP = prvtreader.ReadMPInt();
			_primeQ = prvtreader.ReadMPInt();
		}

		public BigInteger decryptChallenge(BigInteger encryptedchallenge) {
			BigInteger primeExponentP = PrimeExponent(_privateExponent, _primeP);
			BigInteger primeExponentQ = PrimeExponent(_privateExponent, _primeQ);

			BigInteger p2;
			BigInteger q2;
			BigInteger k;
			BigInteger result;

			p2 = encryptedchallenge % _primeP;
			p2 = p2.modPow(primeExponentP, _primeP);

			q2 = encryptedchallenge % _primeQ;
			q2 = q2.modPow(primeExponentQ, _primeQ);

			if(p2==q2) return p2;

			k = (q2 - p2) % _primeQ;
			if(k.IsNegative) k += _primeQ;
			k = k * _crtCoefficient;
			k = k % _primeQ;

			result = k * _primeP;
			result = result + p2;
			
			return result;
		}

		private static BigInteger PrimeExponent(BigInteger privateExponent,	BigInteger prime) {
			BigInteger pe = prime - new BigInteger(1);
			return privateExponent % pe;

		}

		private static byte[] ReadAll(Stream s) {
			byte[] t = new byte[0x1000];
			int l = s.Read(t, 0, 0x1000);
			if(l==t.Length) throw new IOException("Key file is too big");
			return t;
		}

		//key from passphrase
		private static byte[] ConvertToKey(string passphrase) {
            byte[] t = Encoding.Default.GetBytes(passphrase);
			byte[] md5 = new MD5CryptoServiceProvider().ComputeHash(t);
			byte[] result = new byte[32];
			Array.Copy(md5, 0, result, 0, 16);
			Array.Copy(md5, 0, result, 16, 16);
			return result;
		}
	}
}
