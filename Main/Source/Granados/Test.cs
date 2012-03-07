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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Granados;

namespace Granados.SSHCTest
{
	class Reader : ISSHConnectionEventReceiver, ISSHChannelEventReceiver {
		public SSHConnection _conn;
		public bool _ready;

		public void OnData(byte[] data, int offset, int length) {
			System.Console.Write(Encoding.ASCII.GetString(data, offset, length));
		}
		public void OnDebugMessage(bool always_display, byte[] data) {
			Debug.WriteLine("DEBUG: "+ Encoding.ASCII.GetString(data));
		}
		public void OnIgnoreMessage(byte[] data) {
			Debug.WriteLine("Ignore: "+ Encoding.ASCII.GetString(data));
		}
		public void OnAuthenticationPrompt(string[] msg) {
			Debug.WriteLine("Auth Prompt "+msg[0]);
		}

		public void OnError(Exception error) {
			Debug.WriteLine("ERROR: "+ error.Message);
		}
		public void OnChannelClosed() {
			Debug.WriteLine("Channel closed");
			_conn.Disconnect("");
			//_conn.AsyncReceive(this);
		}
		public void OnChannelEOF() {
			_pf.Close();
			Debug.WriteLine("Channel EOF");
		}
		public void OnExtendedData(int type, byte[] data) {
			Debug.WriteLine("EXTENDED DATA");
		}
		public void OnConnectionClosed() {
			Debug.WriteLine("Connection closed");
		}
		public void OnUnknownMessage(byte type, byte[] data) {
			Debug.WriteLine("Unknown Message " + type);
		}
		public void OnChannelReady() {
			_ready = true;
		}
		public void OnChannelError(Exception error) {
			Debug.WriteLine("Channel ERROR: "+ error.Message);
		}
		public void OnMiscPacket(byte type, byte[] data, int offset, int length) {
		}

		public PortForwardingCheckResult CheckPortForwardingRequest(string host, int port, string originator_host, int originator_port) {
			PortForwardingCheckResult r = new PortForwardingCheckResult();
			r.allowed = true;
			r.channel = this;
			return r;
		}
		public void EstablishPortforwarding(ISSHChannelEventReceiver rec, SSHChannel channel) {
			_pf = channel;
		}

		public SSHChannel _pf;
	}

	class Test
	{
		private static SSHConnection _conn;

		[STAThread]
		static void Main(string[] args)
		{
			/*
			string cn = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
			string t1 = Routrek.SSHC.Strings.GetString("NotSSHServer");
			System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja");
			Routrek.SSHC.Strings.Reload();
			string t2 = Routrek.SSHC.Strings.GetString("NotSSHServer");
			*/

#if false //RSA keygen
			//RSA KEY GENERATION TEST
			byte[] testdata = Encoding.ASCII.GetBytes("CHRISTIAN VIERI");
			RSAKeyPair kp = RSAKeyPair.GenerateNew(2048, new Random());
			byte[] sig = kp.Sign(testdata);
			kp.Verify(sig, testdata);

			new SSH2UserAuthKey(kp).WritePublicPartInOpenSSHStyle(new FileStream("C:\\IOPort\\newrsakey", FileMode.Create));
			//SSH2UserAuthKey newpk = SSH2PrivateKey.FromSECSHStyleFile("C:\\IOPort\\newrsakey", "nedved");
#endif
			
#if false //DSA keygen
			//DSA KEY GENERATION TEST
			byte[] testdata = Encoding.ASCII.GetBytes("CHRISTIAN VIERI 0000");
			DSAKeyPair kp = DSAKeyPair.GenerateNew(2048, new Random());
			byte[] sig = kp.Sign(testdata);
			kp.Verify(sig, testdata);
			new SSH2UserAuthKey(kp).WritePublicPartInOpenSSHStyle(new FileStream("C:\\IOPort\\newdsakey", FileMode.Create));
			//SSH2PrivateKey newpk = SSH2PrivateKey.FromSECSHStyleFile("C:\\IOPort\\newdsakey", "nedved");
#endif

			SSHConnectionParameter f = new SSHConnectionParameter();
			f.UserName = "root";
#if false //SSH1
			//SSH1
			f.Password = "";
			f.Protocol = SSHProtocol.SSH2;
			f.AuthenticationType = AuthenticationType.Password;
			f.PreferableCipherAlgorithms = new CipherAlgorithm[] { CipherAlgorithm.Blowfish, CipherAlgorithm.TripleDES };
#else //SSH2
			f.Password = "";
			f.Protocol = SSHProtocol.SSH2;
			f.AuthenticationType = AuthenticationType.Password;
			f.WindowSize = 0x1000;
#endif
			Reader reader = new Reader();
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//s.Blocking = false;
			s.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.1"), 22));
			_conn = SSHConnection.Connect(f, reader, s);
			reader._conn = _conn;
#if false //Remote->Local
			_conn.ListenForwardedPort("0.0.0.0", 29472);
#elif false //Local->Remote
			SSHChannel ch = _conn.ForwardPort(reader, "www.yahoo.co.jp", 80, "localhost", 0);
			reader._pf = ch;
			while(!reader._ready) System.Threading.Thread.Sleep(100);
			reader._pf.Transmit(Encoding.ASCII.GetBytes("GET / HTTP/1.0\r\n\r\n"));
#elif false //SSH over SSH
			f.Password = "okajima";
			SSHConnection con2 = _conn.OpenPortForwardedAnotherConnection(f, reader, "kuromatsu", 22);
			reader._conn = con2;
			SSHChannel ch = con2.OpenShell(reader);
			reader._pf = ch;
#else //normal shell
			SSHChannel ch = _conn.OpenShell(reader);
			reader._pf = ch;
#endif			

			//Debug.WriteLine(_conn.ConnectionInfo.DumpHostKeyInKnownHostsStyle());
			SSHConnectionInfo ci = _conn.ConnectionInfo;

			Thread.Sleep(1000);
			//((SSH2Connection)_conn).ReexchangeKeys();

			byte[] b = new byte[1];
			while(true) {
				int input = System.Console.Read();
				
				b[0] = (byte)input;
				//Debug.WriteLine(input);
				reader._pf.Transmit(b);
			}

		}

	}
}
