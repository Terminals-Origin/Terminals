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
using System.Text;

using Routrek.PKI;
using Routrek.Toolkit;

namespace Routrek.SSHC {
	/// <summary>
	/// ConnectionInfo describes the attributes of the host or the connection.
	/// It is available after the connection is established without any errors.
	/// </summary>
	public abstract class SSHConnectionInfo {
		internal string _serverVersionString;
		internal string _clientVersionString;
		internal string _supportedCipherAlgorithms;
		internal PublicKey _hostkey;

		internal CipherAlgorithm _algorithmForTransmittion;
		internal CipherAlgorithm _algorithmForReception;

		public string ServerVersionString {
			get {
				return _serverVersionString;
			}
		}
		public string ClientVerisonString {
			get {
				return _clientVersionString;
			}
		}
		public string SupportedCipherAlgorithms {
			get {
				return _supportedCipherAlgorithms;
			}
		}
		public CipherAlgorithm AlgorithmForTransmittion {
			get {
				return _algorithmForTransmittion;
			}
		}
		public CipherAlgorithm AlgorithmForReception {
			get {
				return _algorithmForReception;
			}
		}
		public PublicKey HostKey {
			get {
				return _hostkey;
			}
		}

		public abstract string DumpHostKeyInKnownHostsStyle();

	
	}
}

namespace Routrek.SSHCV1 {
	using Routrek.SSHC;

	public class SSH1ConnectionInfo : SSHConnectionInfo {
		internal SSHServerInfo _serverinfo;

		public override string DumpHostKeyInKnownHostsStyle() {
			StringBuilder bld = new StringBuilder();
			bld.Append("ssh1 ");
			SSH1DataWriter wr = new SSH1DataWriter();
			//RSA only for SSH1
			RSAPublicKey rsa = (RSAPublicKey)_hostkey;
			wr.Write(rsa.Exponent);
			wr.Write(rsa.Modulus);
			bld.Append(Encoding.ASCII.GetString(Base64.Encode(wr.ToByteArray())));
			return bld.ToString();
		}

		public void SetSupportedCipherAlgorithms(int mask) {
			StringBuilder bld = new StringBuilder();
			if((mask &  2)!=0) AppendSupportedCipher(bld, "Idea");
			if((mask &  4)!=0) AppendSupportedCipher(bld, "DES");
			if((mask &  8)!=0) AppendSupportedCipher(bld, "TripleDES");
			if((mask & 16)!=0) AppendSupportedCipher(bld, "TSS");
			if((mask & 32)!=0) AppendSupportedCipher(bld, "RC4");
			if((mask & 64)!=0) AppendSupportedCipher(bld, "Blowfish");

			_supportedCipherAlgorithms = bld.ToString();
		}

		private static void AppendSupportedCipher(StringBuilder bld, string text) {
			if(bld.Length>0) bld.Append(',');
			bld.Append(text);
		}

	}
}

namespace Routrek.SSHCV2 {
	using Routrek.SSHC;
	using Routrek.PKI;

	public class SSH2ConnectionInfo : SSHConnectionInfo {
		internal string _supportedHostKeyAlgorithms;
		internal PublicKeyAlgorithm _algorithmForHostKeyVerification;
		internal string _supportedKEXAlgorithms;

		public string SupportedHostKeyAlgorithms {
			get {
				return _supportedHostKeyAlgorithms;
			}
		}

		public PublicKeyAlgorithm AlgorithmForHostKeyVerification {
			get {
				return _algorithmForHostKeyVerification;
			}
		}
		public string SupportedKEXAlgorithms {
			get {
				return _supportedKEXAlgorithms;
			}
		}
		public override string DumpHostKeyInKnownHostsStyle() {
			StringBuilder bld = new StringBuilder();
			bld.Append(SSH2Util.PublicKeyAlgorithmName(_hostkey.Algorithm));
			bld.Append(' ');
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.Write(SSH2Util.PublicKeyAlgorithmName(_hostkey.Algorithm));
			if(_hostkey.Algorithm==PublicKeyAlgorithm.RSA) {
				RSAPublicKey rsa = (RSAPublicKey)_hostkey;
				wr.Write(rsa.Exponent);
				wr.Write(rsa.Modulus);
			}
			else if(_hostkey.Algorithm==PublicKeyAlgorithm.DSA) {
				DSAPublicKey dsa = (DSAPublicKey)_hostkey;
				wr.Write(dsa.P);
				wr.Write(dsa.Q);
				wr.Write(dsa.G);
				wr.Write(dsa.Y);
			}
			else
				throw new SSHException("Host key algorithm is unsupported");

			bld.Append(Encoding.ASCII.GetString(Base64.Encode(wr.ToByteArray())));
			return bld.ToString();
		}
	}
}

