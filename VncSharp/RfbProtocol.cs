// VncSharp - .NET VNC Client Library
// Copyright (C) 2004  David Humphrey, Chuck Borgh, Matt Cyr
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace VncSharp
{
	/// <summary>
	/// Contains methods and properties to handle all aspects of the RFB Protocol versions 3.3 - 3.8.
	/// </summary>
	public class RfbProtocol
	{
		// Encoding Constants
		public const int RAW_ENCODING 					= 0;
		public const int COPYRECT_ENCODING 				= 1;
		public const int RRE_ENCODING 					= 2;
		public const int CORRE_ENCODING					= 4;
		public const int HEXTILE_ENCODING 				= 5;
		public const int ZRLE_ENCODING 					= 16;

		// Server to Client Message-Type constants
		public const int FRAMEBUFFER_UPDATE 			= 0;
		public const int SET_COLOUR_MAP_ENTRIES			= 1;
		public const int BELL 							= 2;
		public const int SERVER_CUT_TEXT 				= 3;

		// Client to Server Message-Type constants
		protected const byte SET_PIXEL_FORMAT 			= 0;
		protected const byte SET_ENCODINGS 				= 2;
		protected const byte FRAMEBUFFER_UPDATE_REQUEST = 3;
		protected const byte KEY_EVENT 					= 4;
		protected const byte POINTER_EVENT 				= 5;
		protected const byte CLIENT_CUT_TEXT 			= 6;

		protected int verMajor;	// Major version of Protocol--probably 3
		protected int verMinor; // Minor version of Protocol--probably 3, 7, or 8

		protected TcpClient tcp;		// Network object used to communicate with host
		protected NetworkStream stream;	// Stream object used to send/receive data
		protected BinaryReader reader;	// Integral rather than Byte values are typically
		protected BinaryWriter writer;	// sent and received, so these handle this.

//		NOTE: I've re-written things so the mutex is no longer necessary.  However, I've
//		left all the synch code in place below (see various write calls) just in case
//		(how's that for confidence!)
//		protected Mutex writeLock;		// Writing to the underlying stream must be done 
										// in a thread-safe way.  This mutex "tries" to 
										// deal with this.  I'm still not sure that this is
										// the best way--it is the best way I've been able
										// to come-up with so far.

		public RfbProtocol()
		{
		}
		
		/// <summary>
		/// Gets the Protocol Version of the remote VNC Host--probably 3.3, 3.7, or 3.8.
		/// </summary>
		public float ServerVersion {
			get {
				return (float) verMajor + (verMinor * 0.1f);
			}
		}

		/// <summary>
		/// Attempt to connect to a remote VNC Host.
		/// </summary>
		/// <param name="host">The IP Address or Host Name of the VNC Host.</param>
		/// <param name="port">The Port number on which to connect.  Usually this will be 5900, except in the case that the VNC Host is running on a different Display, in which case the Display number should be added to 5900 to determine the correct port.</param>
		public void Connect(string host, int port)
		{
			if (host == null) throw new ArgumentNullException("host");

			// Try to connect, passing any exceptions up to the caller, and if successful, 
			// wrap a big endian Binary Reader and Binary Writer around the resulting stream.
			tcp = new TcpClient();
			tcp.NoDelay = true;  // turn-off Nagle's Algorithm for better interactive performance with host.
						
			tcp.Connect(host, port);
			stream = tcp.GetStream();

			// Most of the RFB protocol uses Big-Endian byte order, while
			// .NET uses Little-Endian. These wrappers convert between the
			// two.  See BigEndianReader and BigEndianWriter below for more details.
			reader = new BigEndianBinaryReader(stream);
			writer = new BigEndianBinaryWriter(stream);
			
//			NOTE: not needed any more, but still testing
//			writeLock = new Mutex();
		}

		/// <summary>
		/// Closes the connection to the remote host.
		/// </summary>
		public void Close()
		{
			try {
				writer.Close();
				reader.Close();
				stream.Close();
				tcp.Close();

//				NOTE: not needed any more, but still testing
//				writeLock.Close(); // like Dispose() -- can't use this anymore!!!
			} catch (Exception ex) {
				Debug.Fail(ex.Message);
			}
		}
		
		/// <summary>
		/// Reads VNC Host Protocol Version message (see RFB Doc v. 3.8 section 6.1.1)
		/// </summary>
		/// <exception cref="NotSupportedException">Thrown if the version of the host is not known or supported.</exception>
		public void ReadProtocolVersion()
		{
			byte[] b = reader.ReadBytes(12);

			// As of the time of writing, the only supported versions are 3.3, 3.7, and 3.8.
			if (   (b[0]  == 0x52 &&					// R
					b[1]  == 0x46 &&					// F
					b[2]  == 0x42 &&					// B
					b[3]  == 0x20 &&					// (space)
					b[4]  == 0x30 &&					// 0
					b[5]  == 0x30 &&					// 0
					b[6]  == 0x33 &&					// 3
					b[7]  == 0x2e)                      // .
                  &&
				  ((b[8]  == 0x30 &&					// 0
					b[9]  == 0x30 &&					// 0
				   (b[10] == 0x33 ||					// 3, 7, OR 8 are all valid and possible
				    b[10] == 0x36 ||					// BUG FIX: UltraVNC reports protocol version 3.6!
				    b[10] == 0x37 ||
				    b[10] == 0x38)))
                    ||                                  // Support Apple Remote Desktop
                   (b[8]  == 0x38 &&					// 8
					b[9]  == 0x38 &&					// 8
			        b[10] == 0x39)                      // 9
                  &&					
				    b[11] == 0x0a)						// \n
			{
				// Since we only currently support the 3.x protocols, this can be assumed here.
				// If and when 4.x comes out, this will need to be fixed--however, the entire 
				// protocol will need to be updated then anyway :)
				verMajor = 3;

				// Figure out which version of the protocol this is:
				switch (b[10]) {
					case 0x33: 
					case 0x36:	// BUG FIX: pass 3.3 for 3.6 to allow UltraVNC to work, thanks to Steve Bostedor.
                    case 0x39:	// FIX: pass 3.889 to allow Apple Remote Desktop to work, thanks to Julian Cable.
                        verMinor = 3;
						break;
					case 0x37:
						verMinor = 7;
						break;
					case 0x38:
						verMinor = 8;
						break;
				}
			} else {
				throw new NotSupportedException("Only versions 3.3, 3.7, and 3.8 of the RFB Protocol are supported.");
			}
		}

		/// <summary>
		/// Send the Protocol Version supported by the client.  Will be highest supported by server (see RFB Doc v. 3.8 section 6.1.1).
		/// </summary>
		public void WriteProtocolVersion()
		{
			// We will use which ever version the server understands, be it 3.3, 3.7, or 3.8.
			Debug.Assert(verMinor == 3 || verMinor == 7 || verMinor == 8, "Wrong Protocol Version!",
						 string.Format("Protocol Version should be 3.3, 3.7, or 3.8 but is {0}.{1}", verMajor.ToString(), verMinor.ToString()));

			writer.Write(GetBytes(string.Format("RFB 003.00{0}\n", verMinor.ToString())));
			writer.Flush();
		}

		/// <summary>
		/// Determine the type(s) of authentication that the server supports. See RFB Doc v. 3.8 section 6.1.2.
		/// </summary>
		/// <returns>An array of bytes containing the supported Security Type(s) of the server.</returns>
		public byte[] ReadSecurityTypes()
		{
			// Read and return the types of security supported by the server (see protocol doc 6.1.2)
			byte[] types = null;
			
			// Protocol Version 3.7 onward supports multiple security types, while 3.3 only 1
			if (verMinor == 3) {
				types = new byte[] { (byte) reader.ReadUInt32() };
			} else {
				byte num = reader.ReadByte();
				types = new byte[num];
				
				for (int i = 0; i < num; ++i) {
					types[i] = reader.ReadByte();
				}
			}
			return types;
		}

		/// <summary>
		/// If the server has rejected the connection during Authentication, a reason is given. See RFB Doc v. 3.8 section 6.1.2.
		/// </summary>
		/// <returns>Returns a string containing the reason for the server rejecting the connection.</returns>
		public string ReadSecurityFailureReason()
		{
			int length = (int) reader.ReadUInt32();
			return GetString(reader.ReadBytes(length));
		}

		/// <summary>
		/// Indicate to the server which type of authentication will be used. See RFB Doc v. 3.8 section 6.1.2.
		/// </summary>
		/// <param name="type">The type of Authentication to be used, 1 (None) or 2 (VNC Authentication).</param>
		public void WriteSecurityType(byte type)
		{
			Debug.Assert(type >= 1, "Wrong Security Type", "The Security Type must be one that requires authentication.");
			
			// Only bother writing this byte if the version of the server is 3.7
			if (verMinor >= 7) {
				writer.Write(type);
				writer.Flush();			
			}
		}

		/// <summary>
		/// When the server uses Security Type 2 (i.e., VNC Authentication), a Challenge/Response 
		/// mechanism is used to authenticate the user. See RFB Doc v. 3.8 section 6.1.2 and 6.2.2.
		/// </summary>
		/// <returns>Returns the 16 byte Challenge sent by the server.</returns>
		public byte[] ReadSecurityChallenge()
		{
			return reader.ReadBytes(16);
		}

		/// <summary>
		/// Sends the encrypted Response back to the server. See RFB Doc v. 3.8 section 6.1.2.
		/// </summary>
		/// <param name="response">The DES password encrypted challege sent by the server.</param>
		public void WriteSecurityResponse(byte[] response)
		{
			writer.Write(response, 0, response.Length);
			writer.Flush();
		}

		/// <summary>
		/// When the server uses VNC Authentication, after the Challege/Response, the server
		/// sends a status code to indicate whether authentication worked. See RFB Doc v. 3.8 section 6.1.3.
		/// </summary>
		/// <returns>An integer indicating the status of authentication: 0 = OK; 1 = Failed; 2 = Too Many (deprecated).</returns>
		public uint ReadSecurityResult()
		{
			return reader.ReadUInt32();
		}

		/// <summary>
		/// Sends an Initialisation message to the server. See RFB Doc v. 3.8 section 6.1.4.
		/// </summary>
		/// <param name="shared">True if the server should allow other clients to connect, otherwise False.</param>
		public void WriteClientInitialisation(bool shared)
		{
			// Non-zero if TRUE, zero if FALSE
			writer.Write((byte)(shared ? 1 : 0));
			writer.Flush();
		}
		
		/// <summary>
		/// Reads the server's Initialization message, specifically the remote Framebuffer's properties. See RFB Doc v. 3.8 section 6.1.5.
		/// </summary>
		/// <returns>Returns a Framebuffer object representing the geometry and properties of the remote host.</returns>
		public Framebuffer ReadServerInit()
		{
			int w = (int) reader.ReadUInt16();
			int h = (int) reader.ReadUInt16();
			Framebuffer buffer = Framebuffer.FromPixelFormat(reader.ReadBytes(16), w, h);
			int length = (int) reader.ReadUInt32();

			buffer.DesktopName = GetString(reader.ReadBytes(length));
			
			return buffer;
		}
		
		/// <summary>
		/// Sends the format to be used by the server when sending Framebuffer Updates. See RFB Doc v. 3.8 section 6.3.1.
		/// </summary>
		/// <param name="buffer">A Framebuffer telling the server how to encode pixel data. Typically this will be the same one sent by the server during initialization.</param>
		public void WriteSetPixelFormat(Framebuffer buffer)
		{
			writer.Write(SET_PIXEL_FORMAT);
			WritePadding(3);
			writer.Write(buffer.ToPixelFormat());		// 16-byte Pixel Format
			writer.Flush();
		}

		/// <summary>
		/// Tell the server which encodings are supported by the client. See RFB Doc v. 3.8 section 6.3.3.
		/// </summary>
		/// <param name="encodings">An array of integers indicating the encoding types supported.  The order indicates preference, where the first item is the first preferred.</param>
		public void WriteSetEncodings(uint[] encodings)
		{
			writer.Write(SET_ENCODINGS);
			WritePadding(1);
			writer.Write((ushort)encodings.Length);
			
			for (int i = 0; i < encodings.Length; i++) {
				writer.Write(encodings[i]);
			}

			writer.Flush();
		}

		/// <summary>
		/// Sends a request for an update of the area specified by (x, y, w, h). See RFB Doc v. 3.8 section 6.3.4.
		/// </summary>
		/// <param name="x">The x-position of the area to be updated.</param>
		/// <param name="y">The y-position of the area to be updated.</param>
		/// <param name="width">The width of the area to be updated.</param>
		/// <param name="height">The height of the area to be updated.</param>
		/// <param name="incremental">Indicates whether only changes to the client's data should be sent or the entire desktop.</param>
		public void WriteFramebufferUpdateRequest(ushort x, ushort y, ushort width, ushort height, bool incremental)
		{
//			NOTE: not needed any more, but still testing
//			writeLock.WaitOne();
		
			writer.Write(FRAMEBUFFER_UPDATE_REQUEST);
			writer.Write((byte)(incremental ? 1 : 0));
			writer.Write(x);
			writer.Write(y);
			writer.Write(width);
			writer.Write(height);
			writer.Flush();

//			NOTE: not needed any more, but still testing
//			writeLock.ReleaseMutex();
		}

		/// <summary>
		/// Sends a key press or release to the server. See RFB Doc v. 3.8 section 6.3.5.
		/// </summary>
		/// <param name="keysym">The value of the key pressed, expressed using X Window "keysym" values.</param>
		/// <param name="pressed"></param>
		public void WriteKeyEvent(uint keysym, bool pressed)
		{
//			NOTE: not needed any more, but still testing
//			writeLock.WaitOne();

			writer.Write(KEY_EVENT);
			writer.Write( (byte) (pressed ? 1 : 0));
			WritePadding(2);
			writer.Write(keysym);
			writer.Flush();

//			NOTE: not needed any more, but still testing
//			writeLock.ReleaseMutex();
		}

		/// <summary>
		/// Sends a mouse movement or button press/release to the server. See RFB Doc v. 3.8 section 6.3.6.
		/// </summary>
		/// <param name="buttonMask">A bitmask indicating which button(s) are pressed.</param>
		/// <param name="point">The location of the mouse cursor.</param>
		public void WritePointerEvent(byte buttonMask, Point point)
		{
//			NOTE: not needed any more, but still testing
//			writeLock.WaitOne();
			
			writer.Write(POINTER_EVENT);
			writer.Write(buttonMask);
			writer.Write( (ushort) point.X);
			writer.Write( (ushort) point.Y);
			writer.Flush();

//			NOTE: not needed any more, but still testing
//			writeLock.ReleaseMutex();
		}

		/// <summary>
		/// Sends text in the client's Cut Buffer to the server. See RFB Doc v. 3.8 section 6.3.7.
		/// </summary>
		/// <param name="text">The text to be sent to the server.</param></param>
		public void WriteClientCutText(string text)
		{
//			NOTE: not needed any more, but still testing
//			writeLock.WaitOne();

			writer.Write(CLIENT_CUT_TEXT);
			WritePadding(3);
			writer.Write( (uint) text.Length);
			writer.Write(GetBytes(text));
			writer.Flush();
			
//			NOTE: not needed any more, but still testing
//			writeLock.ReleaseMutex();
		}

		/// <summary>
		/// Reads the type of message being sent by the server--all messages are prefixed with a message type.
		/// </summary>
		/// <returns>Returns the message type as an integer.</returns>
		public int ReadServerMessageType()
		{
			return (int) reader.ReadByte();
		}

		/// <summary>
		/// Reads the number of update rectangles being sent by the server. See RFB Doc v. 3.8 section 6.4.1.
		/// </summary>
		/// <returns>Returns the number of rectangles that follow.</returns>
		public int ReadFramebufferUpdate()
		{
			ReadPadding(1);
			return (int) reader.ReadUInt16();
		}

		/// <summary>
		/// Reads a rectangle's header information, including its encoding. See RFB Doc v. 3.8 section 6.4.1.
		/// </summary>
		/// <param name="rectangle">The geometry of the rectangle that is about to be sent.</param>
		/// <param name="encoding">The encoding used for this rectangle.</param>
		public void ReadFramebufferUpdateRectHeader(out Rectangle rectangle, out int encoding)
		{
			rectangle = new Rectangle();
			rectangle.X = (int) reader.ReadUInt16();
			rectangle.Y = (int) reader.ReadUInt16();
			rectangle.Width = (int) reader.ReadUInt16();
			rectangle.Height = (int) reader.ReadUInt16();
			encoding = (int) reader.ReadUInt32();
		}

		
//		// TODO: This is not yet used anywhere, and needs to be fixed so the 
//		// colour map array is returned, cached, etc.
//		public void ReadColourMapEntries(out int firstColour, out int[] rgbColours)
//		{
//			ReadPadding(1);
//			firstColour = (int) reader.ReadUInt16();
//			int n = (int) reader.ReadUInt16();
//			rgbColours = new int[n];
//			
//			for (int i = 0; i < n * 3; i++) {  // * 3 since R, G, and B come for each colour
//				rgbColours[i] = (int) reader.ReadUInt16();
//			}
//		}

		/// <summary>
		/// Reads the text from the Cut Buffer on the server. See RFB Doc v. 3.8 section 6.4.4.
		/// </summary>
		/// <returns>Returns the text in the server's Cut Buffer.</returns>
		public string ReadServerCutText()
		{
			ReadPadding(3);
			int length = (int) reader.ReadUInt32();
			return GetString(reader.ReadBytes(length));
		}

		// ---------------------------------------------------------------------------------------
		// Here's all the "low-level" protocol stuff so user objects can access the data directly

		/// <summary>
		/// Reads a single UInt32 value from the server, taking care of Big- to Little-Endian conversion.
		/// </summary>
		/// <returns>Returns a UInt32 value.</returns>
	    public uint ReadUint32()
		{
			return reader.ReadUInt32(); 
		}
		
		/// <summary>
		/// Reads a single UInt16 value from the server, taking care of Big- to Little-Endian conversion.
		/// </summary>
		/// <returns>Returns a UInt16 value.</returns>
		public ushort ReadUInt16()
		{
			return reader.ReadUInt16(); 
		}
		
		/// <summary>
		/// Reads a single Byte value from the server.
		/// </summary>
		/// <returns>Returns a Byte value.</returns>
		public byte ReadByte()
		{
			return reader.ReadByte();
		}
		
		/// <summary>
		/// Reads the specified number of bytes from the server, taking care of Big- to Little-Endian conversion.
		/// </summary>
		/// <param name="count">The number of bytes to be read.</param>
		/// <returns>Returns a Byte Array containing the values read.</returns>
		public byte[] ReadBytes(int count)
		{
			return reader.ReadBytes(count);
		}

		/// <summary>
		/// Writes a single UInt32 value to the server, taking care of Little- to Big-Endian conversion.
		/// </summary>
		/// <param name="value">The UInt32 value to be written.</param>
		public void WriteUint32(uint value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a single UInt16 value to the server, taking care of Little- to Big-Endian conversion.
		/// </summary>
		/// <param name="value">The UInt16 value to be written.</param>
		public void WriteUInt16(ushort value)
		{
			writer.Write(value);
		}
		
		/// <summary>
		/// Writes a single Byte value to the server.
		/// </summary>
		/// <param name="value">The UInt32 value to be written.</param>
		public void WriteByte(byte value)
		{
			writer.Write(value);
		}


		/// <summary>
		/// Reads the specified number of bytes of padding (i.e., garbage bytes) from the server.
		/// </summary>
		/// <param name="length">The number of bytes of padding to read.</param>
		protected void ReadPadding(int length)
		{
			ReadBytes(length);
		}
		
		/// <summary>
		/// Writes the specified number of bytes of padding (i.e., garbage bytes) to the server.
		/// </summary>
		/// <param name="length">The number of bytes of padding to write.</param>
		protected void WritePadding(int length)
		{
			byte [] padding = new byte[length];
			writer.Write(padding, 0, padding.Length);
		}

		/// <summary>
		/// Converts a string to bytes for transfer to the server.
		/// </summary>
		/// <param name="text">The text to be converted to bytes.</param>
		/// <returns>Returns a Byte Array containing the text as bytes.</returns>
		protected static byte[] GetBytes(string text)
		{
			return System.Text.Encoding.ASCII.GetBytes(text);
		}
		
		/// <summary>
		/// Converts a series of bytes to a string.
		/// </summary>
		/// <param name="bytes">The Array of Bytes to be converted to a string.</param>
		/// <returns>Returns a String representation of bytes.</returns>
		protected static string GetString(byte[] bytes)
		{
			return System.Text.ASCIIEncoding.UTF8.GetString(bytes, 0, bytes.Length);
		}
		
		
		/// <summary>
		/// BigEndianBinaryReader is a wrapper class used to read .NET integral types from a Big-Endian stream.  It inherits from BinaryReader and adds Big- to Little-Endian conversion.
		/// </summary>
		protected sealed class BigEndianBinaryReader : BinaryReader
		{
			private byte[] buff = new byte[4];

			public BigEndianBinaryReader(System.IO.Stream input) : base(input)
			{
			}
			
			public BigEndianBinaryReader(System.IO.Stream input, System.Text.Encoding encoding) : base(input, encoding)
			{
			}

			// Since this is being used to communicate with an RFB host, only some of the overrides are provided below.
	
			public override ushort ReadUInt16()
			{
				FillBuff(2);
				return (ushort)(buff[1] | buff[0] << 8);
				
			}
			
			public override short ReadInt16()
			{
				FillBuff(2);
				return (short)(buff[1] & 0xFF | buff[0] << 8);
			}

			public override uint ReadUInt32()
			{
				FillBuff(4);
				return (uint)(buff[3] & 0xFF | buff[2] << 8 | buff[1] << 16 | buff[0] << 24);
			}
			
			public override int ReadInt32()
			{
				FillBuff(4);
				return (int)(buff[3] | buff[2] << 8 | buff[1] << 16 | buff[0] << 24);
			}

			private void FillBuff(int totalBytes)
			{
				int bytesRead = 0;
				int n = 0;

				do {
					n = BaseStream.Read(buff, bytesRead, totalBytes - bytesRead);
					
					if (n == 0)
						throw new IOException("Unable to read next byte(s).");

					bytesRead += n;
				} while (bytesRead < totalBytes);
	        }
		}
		

		/// <summary>
		/// BigEndianBinaryWriter is a wrapper class used to write .NET integral types in Big-Endian order to a stream.  It inherits from BinaryWriter and adds Little- to Big-Endian conversion.
		/// </summary>
		protected sealed class BigEndianBinaryWriter : BinaryWriter
		{
			public BigEndianBinaryWriter(System.IO.Stream input) : base(input)
			{
			}

			public BigEndianBinaryWriter(System.IO.Stream input, System.Text.Encoding encoding) : base(input, encoding)
			{
			}
			
			// Flip all little-endian .NET types into big-endian order and send
			public override void Write(ushort value)
			{
				FlipAndWrite(BitConverter.GetBytes(value));
			}
			
			public override void Write(short value)
			{
				FlipAndWrite(BitConverter.GetBytes(value));
			}

			public override void Write(uint value)
			{
				FlipAndWrite(BitConverter.GetBytes(value));
			}
			
			public override void Write(int value)
			{
				FlipAndWrite(BitConverter.GetBytes(value));
			}

			public override void Write(ulong value)
			{
				FlipAndWrite(BitConverter.GetBytes(value));
			}
			
			public override void Write(long value)
			{
				FlipAndWrite(BitConverter.GetBytes(value));
			}

			private void FlipAndWrite(byte[] b)
			{
				// Given an array of bytes, flip and write to underlying stream
				Array.Reverse(b);
				base.Write(b);
			}
		}
	}
}
