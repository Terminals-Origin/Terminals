/*
 * Created by SharpDevelop.
 * User: cablej01
 * Date: 17/11/2008
 * Time: 18:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms; // for messagebox

namespace Terminals
{
	/// <summary>
	/// Description of TelnetStream.
	/// </summary>
	public class TelnetStream : Stream
	{
		public TelnetStream(NetworkStream s) : base()
		{
			first_tx = true;
			stream = s;
			local = new Options();
			remote = new Options();
			local.supported[(int)OPT.TTYPE] = true; // terminal type
			remote.supported[(int)OPT.ECHO] = true; // echo
		}

		private enum tristate
		{
			not_set=0, on=1, off=2
		}
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
		
		public string TerminalType;
		public string Username;
		public string Password;
		
		private NetworkStream stream;
		private Options local;
		private Options remote;
		private bool first_tx;
		private State	receiveState;
		
		private struct State
		{
			public Stream stream;
			public byte[] buffer;
		}

		public override void Write(byte[] data, int offset, int length)
		{
			if(first_tx)
			{
				first_tx=false;
				if(remote.in_effect[(byte)OPT.ECHO]!=tristate.on)
					TelnetCmd(CMD.DO, (byte)OPT.ECHO);
			}
			int n=0;
			// count the chars that look like IACs
			for(int i=0; i<length; i++)
			{
				byte b = data[offset+i];
				if(b==255)
				{
					n++;
				}
			}
			if(n==0)
			{
				// no 0xff chars just send on the buffer
				stream.Write(data, offset, length);
			}
			else
			{
				// some 0xff chars. write array, doubling 0xff chars
				for(int i=0; i<length; i++)
				{
					byte b = data[offset+i];
					if(b==(byte)CMD.IAC)
					{
						stream.WriteByte(b);
					}
					stream.WriteByte(b);
				}
			}			
		}
		
		public override int Read(byte[] buffer, int offset, int length)
		{
			MemoryStream m = new MemoryStream(buffer, offset, length);
			while(m.Position==0)
			{
				while(stream.DataAvailable==false)
					System.Threading.Thread.Sleep(500);
				while(stream.DataAvailable && m.Position<length)
				{
					int b = stream.ReadByte();
					if(b<0)
					{
						break;
					}
					if((byte)b == (byte)CMD.IAC)
					{
						int cmd = stream.ReadByte();
						if(cmd<0)
							break;
						if((byte)cmd == (byte)CMD.IAC)
						{						
							m.WriteByte((byte)cmd);
						}
						else
						{
							process_telnet_command((byte)cmd, stream);
						}
					}
					else
					{
						m.WriteByte((byte)b);
					}
				}
			}
			return (int)m.Position;
		}
		
		public override void SetLength(long l)
		{
			stream.SetLength(l);
		}
		public override long Seek(long amount, SeekOrigin from)
		{
			return stream.Seek(amount, from);
		}
		public override void Flush()
		{
			stream.Flush();
		}
		
		public override long Position
		{
			get
			{
				return stream.Position;
			}
			set
			{
				stream.Position = value;
			}
		}
		public override long Length
		{
			get
			{
				return stream.Length;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return stream.CanWrite;
			}
		}
		public override bool CanRead
		{
			get
			{
				return stream.CanRead;
			}
		}
		public override bool CanSeek
		{
			get
			{
				return stream.CanSeek;
			}
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
				case CMD.IAC: // can't happen
					break;
				default: // can't happen
					break;
			}
		}

		private void TelnetCmd(CMD cmd, byte option)
		{
			byte[] data = {(byte)CMD.IAC, (byte)cmd, option};
			stream.Write(data, 0, data.Length);
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
			stream.Write(data, 0, data.Length);
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
	}
}
