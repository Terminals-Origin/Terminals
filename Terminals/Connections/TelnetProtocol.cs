/*
 * Created by SharpDevelop.
 * User: CableJ01
 * Date: 19/11/2008
 * Time: 07:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Terminals
{
	/// <summary>
	/// Description of TelnetProtocol.
	/// </summary>
	public class TelnetProtocol
	{
		#region Public Properties
		#endregion
		#region Public Enums
		#endregion
		#region Public Fields
		public string TerminalType;
		public string Username;
		public string Password;
		#endregion
		#region Public Delegates
		public delegate void DataIndicate(byte[] data);
		public delegate void DataRequest(byte[] data);
		#endregion
		#region Public Events
	    public event DataIndicate OnDataIndicated;
	    public event DataRequest OnDataRequested;
		#endregion
		#region Public Constructors
		public TelnetProtocol()
		{
			local = new Options();
			remote = new Options();
			local.supported[(int)OPT.TTYPE] = true; // terminal type
			remote.supported[(int)OPT.ECHO] = true; // echo
		}
		#endregion
		#region Public Methods
		public void RequestData (byte[] data)
		{
			if(first_tx)
			{
				first_tx=false;
				TelnetCmd(CMD.DO, (byte)OPT.ECHO);
			}
			int n=0;
			// count the chars that look like IACs
			foreach(byte b in data)
			{
				if(b==255)
				{
					n++;
				}
			}
			if(n==0)
			{
				// no 0xff chars just send on the buffer
				_RequestData(data);
			}
			else
			{
				// some 0xff chars. copy array, doubling 0xff chars
				byte[] bytes = new byte[data.Length+n];
				int i=0;
				foreach(byte b in data)
				{
					if(b==255)
					{
						bytes[i++]=b;
					}
					bytes[i++]=b;
				}
				_RequestData(data);
			}
		}
		public void IndicateData (byte[] data)
		{
			MemoryStream input = new MemoryStream(data);
			MemoryStream output = new MemoryStream();
			while(input.Position<input.Length)
			{
				int b = input.ReadByte();
				if((byte)b == (byte)CMD.IAC)
				{
					int cmd = input.ReadByte();
					if(cmd<0)
						break;
					if((byte)cmd == (byte)CMD.IAC)
					{						
						output.WriteByte((byte)cmd);
					}
					else
					{
						process_telnet_command((byte)cmd, input);
					}
				}
				else
				{
					output.WriteByte((byte)b);
				}
			}
			byte[] obuf = new byte[output.Length];
			System.Array.Copy(output.GetBuffer(), obuf, obuf.Length);
			_IndicateData(obuf);
		}
		#endregion
		#region Public Overrides
		#endregion
		#region Private Enums
		private enum tristate
		{
			not_set=0, on=1, off=2
		}
		private enum CMD 
		{
			SE    = 240, 
			NOP   = 241, 
			DM    = 242, 
			BRK   = 243, 
			IP    = 244, 
			AO    = 245, 
			AYT   = 246, 
			EC    = 247, 
			EL    = 248, 
			GA    = 249, 
			SB    = 250, 
			WILL  = 251, 
			WONT  = 252, 
			DO    = 253, 
			DONT  = 254, 
			IAC   = 255, 
		}
		private enum OPT
		{
			ECHO     = 1,   // echo
			SGA      = 3,   // suppress go ahead
			STATUS   = 5,   // status
			TM       = 6,   // timing mark
			TTYPE    = 24,  // terminal type
			NAWS     = 31,  // window size
			TSPEED   = 32,  // terminal speed
			LFLOW    = 33,  // remote flow control
			LINEMODE = 34,  // linemode
			ENVIRON  = 36,  // environment variables
		}	
		#endregion
		#region Private Fields
		private Options local;
		private Options remote;
		private bool first_tx = true;
		#endregion
		#region Private Classes
		private class Options
		{
			public tristate[] in_effect;
			public bool[] supported;
			public Options()
			{
				in_effect = new tristate[256];
				supported = new bool[256];
			}
		};
		#endregion
		#region Private Structs
		#endregion
		#region Private Methods
		private void _RequestData (byte[] data)
		{
			if(OnDataRequested!=null) OnDataRequested(data);
		}
		private void _IndicateData(byte[] data)
		{
			if(OnDataIndicated!=null) OnDataIndicated(data);
		}
		protected void process_telnet_command(byte b, Stream s)
		{
			// TODO what if end of command sequence not in this buffer?
			if(b<240)
				return; // error
			int option = s.ReadByte();
			switch((CMD)b)
			{
				case CMD.SE:
				case CMD.NOP:
				case CMD.DM:
				case CMD.BRK:
				case CMD.IP:
				case CMD.AO:
				case CMD.AYT:
				case CMD.EC:
				case CMD.EL:
				case CMD.GA:
					break;
				case CMD.SB:
					subnegotiate(s, option);
					break;
				case CMD.WILL:
					if(remote.supported[option])
					{
						if(remote.in_effect[option]!=tristate.on)
						{
							remote.in_effect[option]=tristate.on;
							TelnetCmd(CMD.DO, (byte)option);
						}
					}
					else
					{
						TelnetCmd(CMD.DONT, (byte)option);
					}
					break;
				case CMD.WONT:
					if(remote.supported[option])
					{
						if(remote.in_effect[option]!=tristate.off)
						{
							remote.in_effect[option]=tristate.off;
							TelnetCmd(CMD.DONT, (byte)option);
						}
					}
					break;
				case CMD.DO:
					if(local.supported[option])
					{
						if(local.in_effect[option]!=tristate.on)
						{
							local.in_effect[option]=tristate.on;
							TelnetCmd(CMD.WILL, (byte)option);
						}
					}
					else
					{
						local.in_effect[option]=tristate.off;
						TelnetCmd(CMD.WONT, (byte)option);
					}
					break;
				case CMD.DONT:
					if(local.supported[option])
					{
						if(local.in_effect[option]!= tristate.off)
						{
							local.in_effect[option]=tristate.off;
							TelnetCmd(CMD.WONT, (byte)option);
						}
					}
					break;
				case CMD.IAC:
					byte[] ff = {(byte)CMD.IAC};
					_IndicateData(ff);
					break;
				default: // can't happen
					break;
			}
		}
		private void TelnetCmd(CMD cmd, byte option)
		{
			byte[] data = {(byte)CMD.IAC, (byte)cmd, option};
			_RequestData(data);
		}
		private void TelnetSendSubopt(OPT option, string val)
		{
			byte[] bval = (new System.Text.ASCIIEncoding()).GetBytes(val);
			byte[] data = new byte[6+val.Length];
			int i=0;
			data[i++] = (byte)CMD.IAC;
			data[i++] = (byte)CMD.SB;
			data[i++] = (byte)option;
			data[i++] = 0;
			bval.CopyTo(data, i); i += bval.Length;
			data[i++] = (byte)CMD.IAC;
			data[i++] = (byte)CMD.SE;
			_RequestData(data);
		}
		protected void subnegotiate(Stream s, int option)
		{
			int send = s.ReadByte();
			if(send == 0)
			{
				while(s.ReadByte()!=255);// not interested in values at the moment
				s.ReadByte();
				return; 
			}
			switch((OPT)option) // what happens if its undefined ?
			{
				case OPT.TTYPE:
					TelnetSendSubopt(OPT.TTYPE, TerminalType);
					break;
				default:
					break;
			}
			return;
		}
		#endregion
		#region Private Overrides
		#endregion
	}
}
