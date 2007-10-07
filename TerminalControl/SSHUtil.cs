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
using System.Threading;
using System.Diagnostics;
using System.Text;
//using SHA1CryptoServiceProvider = System.Security.Cryptography.SHA1CryptoServiceProvider;
using HMACSHA1 = System.Security.Cryptography.HMACSHA1;

using Routrek.PKI;
using Routrek.Crypto;
using Routrek.Toolkit;

namespace Routrek.SSHC
{
	/*
	internal class RWBuffer {
		private byte[] _data;
		private int _readOffset;
		private int _writeOffset;

		public RWBuffer() {
			_data = new byte[0x1000];
		}

		public void Write(byte[] src, int offset, int length) {
			lock(this) {
				while(_data.Length-_writeOffset < length)
					Expand();
				Array.Copy(src, offset, _data, _writeOffset, length);
				_writeOffset += length;
				Monitor.Pulse(this);
			}
		}
		public int Read(byte[] dest, int offset, int length) {
			while(_writeOffset - _readOffset < length) {
				Debug.WriteLine("waiting");
				Monitor.Wait(this);
			}

			lock(this) {
				Array.Copy(_data, _readOffset, dest, offset, length);
				_readOffset += length;
				return length;
			}
		}

		private void Expand() {
			lock(this) {
				byte[] t = new byte[_data.Length*2];
				Array.Copy(_data, 0, t, 0, _data.Length);
				_data = t;
			}
		}
	}
	*/		




	public class SSHException : Exception {
		private byte[] _data;

		public SSHException(string msg, byte[] data) : base(msg) {
			_data = data;
		}
	
		public SSHException(string msg) : base(msg) {
		}
	}

	public enum SSHProtocol {
		SSH1,
		SSH2
	}
	
	public enum CipherAlgorithm {
		TripleDES = 3,
		Blowfish = 6,
		AES128 = 10 //SSH2 ONLY
	}
	
	public enum AuthenticationType {
		PublicKey = 2, //uses identity file
		Password = 3,
		KeyboardInteractive = 4
	}
	public enum AuthenticationResult {
		Success,
		Failure,
		Prompt
	}


	public enum MACAlgorithm {
		HMACSHA1
	}


	internal class SSHUtil {

		public static string ClientVersionString(SSHProtocol p) {
			return p==SSHProtocol.SSH1? "SSH-1.5-Granados-1.0" : "SSH-2.0-Granados-1.0";
		}

		public static int ReadInt32(Stream input)	{
			byte[] t = new byte[4];
			ReadAll(input, t, 0, t.Length);
			return ReadInt32(t, 0);
		}
		public static int ReadInt32(byte[] data, int offset) {
			int ret =0;
			ret |= (int)(data[offset]);
			ret <<= 8;
			ret |= (int)(data[offset + 1]);
			ret <<= 8;
			ret |= (int)(data[offset + 2]);
			ret <<= 8;
			ret |= (int)(data[offset + 3]);
			return ret;
		}
		/**
		* Network-byte-orderで32ビット値を書き込む。
		*/
		public static void WriteIntToByteArray(byte[] dst, int pos, int data) {
			uint udata = (uint)data;
			uint a = udata & 0xFF000000;
			a >>= 24;
			dst[pos] = (byte)a;
			
			a = udata & 0x00FF0000;
			a >>= 16;
			dst[pos+1] = (byte)a;
			
			a = udata & 0x0000FF00;
			a >>= 8;
			dst[pos+2] = (byte)a;

			a = udata & 0x000000FF;
			dst[pos+3] = (byte)a;
			
		}
		public static void WriteIntToStream(Stream input, int data)	{
			byte[] t = new byte[4];
			WriteIntToByteArray(t, 0, data);
			input.Write(t, 0, t.Length);
		}

		public static void ReadAll(Stream input, byte[] buf, int offset, int len) {
			do {
				int fetched = input.Read(buf, offset, len);
				len -= fetched;
				offset += fetched;
			} while(len > 0);
		}

		public static bool ContainsString(string[] s, string v) {
			foreach(string x in s)
				if(x==v) return true;

			return false;
		}

		public static int memcmp(byte[] d1, byte[] d2) {
			for(int i = 0; i<d1.Length; i++) {
				if(d1[i]!=d2[i]) return (int)(d2[i]-d1[i]);
			}
			return 0;
		}
		public static int memcmp(byte[] d1, int o1, byte[] d2, int o2, int len) {
			for(int i = 0; i<len; i++) {
				if(d1[o1+i]!=d2[o2+i]) return (int)(d2[o2+i]-d1[o1+i]);
			}
			return 0;
		}
	}

	public class Strings {
		private static StringResources _strings;
		public static string GetString(string id) {
			if(_strings==null) Reload();
			return _strings.GetString(id);
		}

		//load resource corresponding to current culture
		public static void Reload() {
			_strings = new StringResources("TerminalControl.strings", typeof(Strings).Assembly);
		}
	}

	internal class DebugUtil {
		public static string DumpByteArray(byte[] data) {
			return DumpByteArray(data, 0, data.Length);
		}
		public static string DumpByteArray(byte[] data, int offset, int length) {
			StringBuilder bld = new StringBuilder();
			for(int i=0; i<length; i++) {
				bld.Append(data[offset+i].ToString("X2"));
				if((i % 4)==3) bld.Append(' ');
			}
			return bld.ToString();
		}
	}
}
