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
using System.Diagnostics;
using System.Security.Cryptography;

namespace Routrek.PKI
{

	public interface ISigner {
		byte[] Sign(byte[] data);
	}
	public interface IVerifier {
		void Verify(byte[] data, byte[] expected);
	}

	public interface IKeyWriter {
		void Write(BigInteger bi);
	}

	
	public enum PublicKeyAlgorithm {
		DSA,
		RSA
	}

	public abstract class PublicKey {
		public abstract void WriteTo(IKeyWriter writer);
		public abstract PublicKeyAlgorithm Algorithm { get; }
	}

	public abstract class KeyPair {
		
		public abstract PublicKey PublicKey { get; }
		public abstract PublicKeyAlgorithm Algorithm { get; }
	}

	public class PKIUtil {
		// OID { 1.3.14.3.2.26 }
		// iso(1) identified-org(3) OIW(14) secsig(3) alg(2) sha1(26)
		public static readonly byte[] SHA1_ASN_ID = new byte[] { 0x30, 0x21, 0x30, 0x09, 0x06, 0x05, 0x2b, 0x0e, 0x03, 0x02, 0x1a, 0x05, 0x00, 0x04, 0x14 };
	}

	public class VerifyException : Exception {
		public VerifyException(string msg) : base(msg) {}
	}


}
