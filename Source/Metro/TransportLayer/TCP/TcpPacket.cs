/******************************************************************************\
* Metro                                                                        *
*                                                                              *
* Copyright (C) 2004 Chris Waddell <IRBMe@icechat.net>		                   *
*                                                                              *
* This program is free software; you can redistribute it and/or modify         *
* it under the terms of the GNU General Public License as published by         *
* the Free Software Foundation; either version 2, or (at your option)          *
* any later version.                                                           *
*                                                                              *
* This program is distributed in the hope that it will be useful,              *
* but WITHOUT ANY WARRANTY; without even the implied warranty of               *
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
* GNU General Public License for more details.                                 *
*                                                                              *
* You should have received a copy of the GNU General Public License            *
* along with this program; if not, write to the Free Software                  *
* Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.                    *
*                                                                              *
* Please consult the LICENSE.txt file included with this project for           *
* more details                                                                 *
*                                                                              *
\******************************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace Metro.TransportLayer.Tcp
{
	#region Delegates

	/// <summary>
	///		The handler for when a new packet has arrived.
	/// </summary>
	public delegate void TcpPacketArrivedHandler (TcpPacket packet);

	#endregion

	#region Enumerations

	/// <summary>
	///		Control bits.
	/// </summary>
	public enum TcpFlags : byte
	{
		/// <summary>
		///		FIN: No more data from sender.
		/// </summary>
		Finish = 1,
		
		/// <summary>
		///		SYN: Synchronize sequence numbers.
		/// </summary>
		Synchronize = 2,
		
		/// <summary>
		///		RST: Reset the connection.
		/// </summary>
		Reset = 4,
	
		/// <summary>
		///		PSH: Push Function.
		/// </summary>
		Push = 8,

		/// <summary>
		///		ACK: Acknowledgment field significant.
		/// </summary>
		Acknowledgment = 16,
		
		/// <summary>
		///		URG: Urgent Pointer field significant.
		/// </summary>
		Urgent = 32,
	}
	
	
	/// <summary>
	///		The type of option.
	/// </summary>
	public enum TcpOptionNumber : byte
	{
		/// <summary>
		///		End of option list.
		/// </summary>
		EndOfOptions,
		
		/// <summary>
		///		No operation.
		/// </summary>
		NoOperation,
		
		/// <summary>
		///		Maximum segment size.
		/// </summary>
		MaximumSegmentSize
	}
	
	
	#endregion
	
	#region Classes

	/// <summary>
	///		The options may appear or not in datagrams.
	/// </summary>
	public class TcpOption
	{
		#region Private Fields

		/// <summary>
		///		The option type.
		/// </summary>
		private TcpOptionNumber m_number = TcpOptionNumber.NoOperation;
	
		/// <summary>
		///		The length of the option including the option type field and the
		///		length field.
		/// </summary>
		private int m_length = 1;
		
		/// <summary>
		///		The data stored in the option. This is parsed by specific option
		///		classes
		/// </summary>
		private byte[] m_data;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The option type.
		/// </summary>
		public TcpOptionNumber OptionType
		{
			get
			{
				return m_number;
			}
			set
			{
				m_number = value;
			}
		}
		
		
		/// <summary>
		///		The length of the option including the option type field and the
		///		length field.
		/// </summary>
		public int Length
		{
			get
			{
				return m_length;
			}
			set
			{
				m_length = value;
			}
		}
		
		
		/// <summary>
		///		The data stored in the option. This is parsed by specific option
		///		classes.
		/// </summary>
		public byte[] Data
		{
			get
			{
				return m_data;
			}
			set
			{
				m_data = value;
			}
		}
		
		
		#endregion
	}


	/// <summary>
	///		TCP is a connection-oriented, end-to-end reliable protocol designed to
	///		fit into a layered hierarchy of protocols which support multi-network
	///		applications. The TCP provides for reliable inter-process
	///		communication between pairs of processes in host computers attached to
	///		distinct but interconnected computer communication networks. Very few
	///		assumptions are made as to the reliability of the communication
	///		protocols below the TCP layer. TCP assumes it can obtain a simple,
	///		potentially unreliable datagram service from the lower level
	///		protocols. In principle, the TCP should be able to operate above a
	///		wide spectrum of communication systems ranging from hard-wired
	///		connections to packet-switched or circuit-switched networks.
	/// </summary>
	public class TcpPacket
	{
		/***********************************************************************\
		*                                                                       *
		*	       0               1               2               3            *
		*    0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7    *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |          Source Port          |       Destination Port        |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                        Sequence Number                        |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                    Acknowledgment Number                      |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |  Data |           |U|A|P|R|S|F|                               |   *
		*   | Offset| Reserved  |R|C|S|S|Y|I|            Window             |   *
		*   |       |           |G|K|H|T|N|N|                               |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |           Checksum            |         Urgent Pointer        |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                    Options                    |    Padding    |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                             data                              |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*                                                                       *
		\***********************************************************************/ 
	
		#region Private Fields
		
		/// <summary>
		///		Used for generating random numbers.
		/// </summary>
		private Random m_random = new Random ();
		
		/// <summary>
		///		The source port number.
		/// </summary>
		private ushort m_sourcePort = 1000;
		
		/// <summary>
		///		The destination port number.
		/// </summary>
		private ushort m_destPort = 1000;
		
		/// <summary>
		///		The sequence number of the first data octet in this segment (except
		///		when SYN is present). If SYN is present the sequence number is the
		///		initial sequence number (ISN) and the first data octet is ISN+1.
		/// </summary>
		private uint m_sequenceNumber;
		
		/// <summary>
		///		If the ACK control bit is set this field contains the value of the
		///		next sequence number the sender of the segment is expecting to
		///		receive. Once a connection is established this is always sent.
		/// </summary>
		private uint m_acknowledgmentNumber;
		
		/// <summary>
		///		This indicates where the data begins.
		/// </summary>
		private ushort m_offset = 0x14;
		
		/// <summary>
		///		Control bits.
		/// </summary>
		private byte m_flags = 0;
		
		/// <summary>
		///		The number of data octets beginning with the one indicated in the
		///		acknowledgment field which the sender of this segment is willing to
		///		accept.
		/// </summary>
		private ushort m_window = ushort.MaxValue;
		
		/// <summary>
		///		The checksum field is the 16 bit one's complement of the one's
		///		complement sum of all 16 bit words in the header and text. If a
		///		segment contains an odd number of header and text octets to be
		///		checksummed, the last octet is padded on the right with zeros to
		///		form a 16 bit word for checksum purposes. The pad is not
		///		transmitted as part of the segment.  While computing the checksum,
		///		the checksum field itself is replaced with zeros.
		/// </summary>
		private ushort m_checksum = 0;
		
		/// <summary>
		///		This field communicates the current value of the urgent pointer as a
		///		positive offset from the sequence number in this segment.  The
		///		urgent pointer points to the sequence number of the octet following
		///		the urgent data.  This field is only be interpreted in segments with
		///		the URG control bit set.
		/// </summary>
		private ushort m_urgentPointer = 0;
		
		/// <summary>
		///		Options may occupy space at the end of the TCP header and are a
		///		multiple of 8 bits in length. All options are included in the
		///		checksum. An option may begin on any octet boundary.
		/// </summary>
		private TcpOption[] m_options;
		
		/// <summary>
		///		The TCP header padding is used to ensure that the TCP header ends
		///		and data begins on a 32 bit boundary. The padding is composed of
		///		zeros.
		/// </summary>
		private byte[] m_padding;
		
		/// <summary>
		///		The data.
		/// </summary>
		private byte[] m_data;

		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The source port number.
		/// </summary>
		public ushort SourcePort
		{
			get
			{
				return m_sourcePort;
			}
			set
			{
				m_sourcePort = value;
			}
		}
		
		
		/// <summary>
		///		The destination port number.
		/// </summary>
		public ushort DestinationPort
		{
			get
			{
				return m_destPort;
			}
			set
			{
				m_destPort = value;
			}
		}
		
		
		/// <summary>
		///		The sequence number of the first data octet in this segment (except
		///		when SYN is present). If SYN is present the sequence number is the
		///		initial sequence number (ISN) and the first data octet is ISN+1.
		/// </summary>
		public uint SequenceNumber
		{
			get
			{
				return m_sequenceNumber;
			}
			set
			{
				m_sequenceNumber = value;
			}
		}
		
		
		/// <summary>
		///		If the ACK control bit is set this field contains the value of the
		///		next sequence number the sender of the segment is expecting to
		///		receive. Once a connection is established this is always sent.
		/// </summary>
		public uint AcknowledgmentNumber
		{
			get
			{
				return m_acknowledgmentNumber;
			}
			set
			{
				m_acknowledgmentNumber = value;
			}
		}
		
		
		/// <summary>
		///		This indicates where the data begins.
		/// </summary>
		public ushort Offset
		{
			get
			{
				return m_offset;
			}
			set
			{
				m_offset = value;
			}
		}
		
		
		/// <summary>
		///		Control bits.
		/// </summary>
		public byte Flags
		{
			get
			{
				return m_flags;
			}
			set
			{
				m_flags = value;
			}
		}
		
		
		/// <summary>
		///		The number of data octets beginning with the one indicated in the
		///		acknowledgment field which the sender of this segment is willing to
		///		accept.
		/// </summary>
		public ushort Window
		{
			get
			{
				return m_window;
			}
			set
			{
				m_window = value;
			}
		}
		
		
		/// <summary>
		///		The checksum field is the 16 bit one's complement of the one's
		///		complement sum of all 16 bit words in the header and text. If a
		///		segment contains an odd number of header and text octets to be
		///		checksummed, the last octet is padded on the right with zeros to
		///		form a 16 bit word for checksum purposes. The pad is not
		///		transmitted as part of the segment.  While computing the checksum,
		///		the checksum field itself is replaced with zeros.
		/// </summary>
		public ushort Checksum
		{
			get
			{
				return m_checksum;
			}
			set
			{
				m_checksum = value;
			}
		}
		
		
		/// <summary>
		///		This field communicates the current value of the urgent pointer as a
		///		positive offset from the sequence number in this segment.  The
		///		urgent pointer points to the sequence number of the octet following
		///		the urgent data.  This field is only be interpreted in segments with
		///		the URG control bit set.
		/// </summary>
		public ushort UrgentPointer
		{
			get
			{
				return m_urgentPointer;
			}
			set
			{
				m_urgentPointer = value;
			}
		}
		
		
		/// <summary>
		///		Options may occupy space at the end of the TCP header and are a
		///		multiple of 8 bits in length. All options are included in the
		///		checksum. An option may begin on any octet boundary.
		/// </summary>
		public TcpOption[] Options
		{
			get
			{
				return m_options;
			}
			set
			{
				m_options = value;
			}
		}
		
		
		/// <summary>
		///		The TCP header padding is used to ensure that the TCP header ends
		///		and data begins on a 32 bit boundary. The padding is composed of
		///		zeros.
		/// </summary>
		public byte[] Padding
		{
			get
			{
				return m_padding;
			}
			set
			{
				m_padding = value;
			}
		}
		
		
		/// <summary>
		///		The data.
		/// </summary>
		public byte[] Data
		{
			get
			{
				return m_data;
			}
			set
			{
				m_data = value;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new Tcp packet.
		/// </summary>
		public TcpPacket ()
		{
			m_acknowledgmentNumber = (uint)m_random.Next (int.MaxValue);
			m_sequenceNumber = (uint)m_random.Next (int.MaxValue);
		}
		
		
		/// <summary>
		///		Create a new Tcp packet.
		/// </summary>
		/// <param name="data">
		///		The byte array representing the Tcp packet.
		///	</param>
		public TcpPacket (byte[] data)
		{
			int position = 0;
		
			#region Basic Fields
		
			// first extract the source port, remembering to convert it to host order
			m_sourcePort			= (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			// same with the destination port
			m_destPort				= (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			// now extract the sequence number, again converting to host order
			m_sequenceNumber		= (uint)IPAddress.NetworkToHostOrder (BitConverter.ToInt32 (data, position));
			position += 4;
			
			// same witht he acknowledgment number
			m_acknowledgmentNumber	= (uint)IPAddress.NetworkToHostOrder (BitConverter.ToInt32 (data, position));
			position += 4;
			
			// copy out the data offset. This is the upper 3 bits of the next byte
			// we also multiply it by 4 since this the value returned is the number of 
			// 32 bit words.
			m_offset				= (ushort)((data[position] >> 4) * 4);
			position++;
			
			// now extract the flags field. These are stored in the lower 6 bits of the next byte
			m_flags					= (byte)(data[position] & 0x3f);
			position++;
			
			// copy out the window field and convert it to host order
			m_window				= (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			// same again for the checksum
			m_checksum				= (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			// and the same for the urgent pointer
			m_urgentPointer			= (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			#endregion
			
			#region Options and Padding
			
			// a header length of more than 20 bytes indicates that there are some options
			if (m_offset > 0x14)
			{
				// create a new buffer of the correct size and copy the options into it
				byte[] optionsAndPadding = new byte[m_offset - 0x14];
				Array.Copy (data, position, optionsAndPadding, 0, optionsAndPadding.Length);
				
				// go through each byte of the options
				for (int i = 0; i < optionsAndPadding.Length;)
				{
					TcpOption option = new TcpOption ();
					
					// fill in basic fields
					option.OptionType	= (TcpOptionNumber)optionsAndPadding[i];
					i++;
					
					#region Fill in Basic Option Fields
					
					if (option.OptionType == TcpOptionNumber.EndOfOptions)
					{
						// copy the padding field out of the header
						if (optionsAndPadding.Length - i > 0)
						{
							m_padding = new byte[optionsAndPadding.Length - i];
							Array.Copy (optionsAndPadding, i, m_padding, 0, m_padding.Length);
						}
						
						// add this option to the array
						option.Length = 1;
					}
					else if (option.OptionType != TcpOptionNumber.NoOperation)
					{
						// copy out the length field
						option.Length = (int)(optionsAndPadding[i]);
						option.Data = new byte[option.Length - 2];

						// copy the actual data out of the packet
						i++;	
						Array.Copy (optionsAndPadding, i, option.Data, 0, option.Data.Length);
						
						// add this new option to the array
						i += option.Data.Length;
					}
					else
					{
						option.Length = 1;
					}
					
					#endregion
					
					#region Add Option
					
					if (m_options == null)
					{
						m_options = new TcpOption[1];
					}
					else
					{
						TcpOption[] tempOptions = new TcpOption[m_options.Length];
						Array.Copy (m_options, 0, tempOptions, 0, m_options.Length);
						
						m_options = new TcpOption[m_options.Length + 1];
						Array.Copy (tempOptions, 0,  m_options, 0, tempOptions.Length);
						
						m_options[m_options.Length - 1] = option;
					}
			
					m_options[m_options.Length - 1] = option;
					
					#endregion
							
					if (option.OptionType == TcpOptionNumber.EndOfOptions)
					{
						break;
					}
				}
		
				position += optionsAndPadding.Length;
			}
			
			#endregion
			
			// now check if there is any data
			if (data.Length > m_offset)
			{
				// allocate a new buffer for it and copy the data in to the buffer
				m_data = new byte[data.Length - m_offset];
				Array.Copy (data, position, m_data, 0, m_data.Length);
			}
		}
		
		
		/// <summary>
		///		This method will calculate several fields such as the checksum then pack the fields 
		///		into a byte array in the format of a tcp header. This can then be attached to a 
		///		network layer protocol and then transmitted over the internet.
		/// </summary>
		/// <returns>
		///		The return value is a byte array with the tcp header at the beginning which can be
		///		appended to a network layer protocol such as IP. 
		/// </returns>
		public byte[] Serialize(IPAddress sourceAddress, IPAddress destAddress)
		{
			#region Initial Calculations
			
			m_offset = 0x14;
			
			if (m_options != null)
			{
				int optionsLength = 0;
				
				for (int i = 0; i < m_options.Length; i++)
				{
					optionsLength += m_options[i].Length;
				}
			
				// if the options field isn't a multiple of 4 it needs padding
				if (optionsLength % 4 != 0)
				{
					m_padding = new byte[4 - (optionsLength % 4)];
				}
				
				int paddingLength = (m_padding != null ? m_padding.Length : 0);
				
				// add on the length of the options field + the new padding onto the header length
				m_offset += (ushort) (optionsLength + paddingLength);
			}

			#endregion
			
			// allocate a large enough buffer to hold the header + the data
			byte[] buffer = new byte[m_offset + (m_data != null ? m_data.Length : 0)];
			int position = 0;
		
			#region Copy TCP Header Fields
		
			// copy in the source port
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_sourcePort)), 0, buffer, position, 2);
			position += 2;
		
			// copy in the destination port
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_destPort)), 0, buffer, position, 2);
			position += 2;
			
			// copy in the sequence number
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((int)m_sequenceNumber)), 0, buffer, position, 4);
			position += 4;
		
			// copy in the acknowledgment number
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((int)m_acknowledgmentNumber)), 0, buffer, position, 4);
			position += 4;
		
			// copy in the offset field
			buffer[position] = (byte)((m_offset / 4) << 4);
			position++;
			
			// copy in the flags field
			buffer[position] = m_flags;
			position++;
			
			// copy in the window field
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_window)), 0, buffer, position, 2);
			position += 2;
		
			// skip over the checksum field right now. We leave it 0 until we have finished
			// filling in everything else, then we calculate the checksum
			position += 2;
			
			// copy in the urgent pointer
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_urgentPointer)), 0, buffer, position, 2);
			position += 2;
		
			#endregion
			
			#region Copy In Options
		
			// now copy in any options
			if (m_options != null)
			{
				
				// go through each options
				for (int i = 0; i < m_options.Length; i++)
				{
					buffer[position] =  (byte)m_options[i].OptionType;
					position++;
				
					if (m_options[i].OptionType == TcpOptionNumber.EndOfOptions)
					{
						break;
					}
					else if (m_options[i].OptionType != TcpOptionNumber.NoOperation)
					{
						buffer[position] = (byte)m_options[i].Length;
						position++;
						Array.Copy (m_options[i].Data, 0, buffer, position, m_options[i].Length - 2);
						position += m_options[i].Length - 2;
					}
				}
			}
			
			// now copy in any padding
			if (m_padding != null)
			{
				Array.Copy (m_padding, 0, buffer, position, m_padding.Length);
				position += m_padding.Length;
			}
			
			#endregion
		
			// copy in any data after that
			if (m_data != null)
			{
				Array.Copy (m_data, 0, buffer, position, m_data.Length);
			}
		
			// the checksum is calculated by constructing a pseudo header consisting of the
			// source/destination addresses, the protocol and the length of the tcp packet
			// (length of header + data). This pseudo header is then prefixed on to the 
			// udp header and the data and the checksum calculation performed on this.
			#region Calculate Checksum

			// now build the pseudo header used for calculating the checksum. The pseudo header
			// looks like this:
			//
			//  +--------+--------+--------+--------+
			//  |          source address           |
			//  +--------+--------+--------+--------+
			//  |        destination address        |
			//  +--------+--------+--------+--------+
			//  |  zero  |protocol|   TCP length    |
			//  +--------+--------+--------+--------+

			byte[] pseudoHeader = new byte[12];

			// first copy in the source and dest addresses
			Array.Copy (sourceAddress.GetAddressBytes (), 0, pseudoHeader, 0, 4); 
			Array.Copy (destAddress.GetAddressBytes (), 0, pseudoHeader, 4, 4); 

			// then a 0 byte followed by the protocol id (tcp)
			pseudoHeader[8] = 0;
			pseudoHeader[9] = (byte)ProtocolType.Tcp;

			// now convert the length to network byte order and copy that in
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)(m_offset + (m_data != null ? m_data.Length : 0)))), 0, pseudoHeader, 10,  2);

			// at this point we have build the pseudo header. Now we prefix it onto the tcp header
			// and the data (which we already have stored in the variable "packet" as a byte array)
			//
			//  +-----------------------------------+
			//  |          Pseudo Header            |
			//  +-----------------------------------+
			//  |            Tcp Header             |
			//  +-----------------------------------+
			//  |                                   |
			//  |          data octets ...   
			//  +---------------- ...  

			byte[] tempBuffer = new byte[pseudoHeader.Length + buffer.Length];
			Array.Copy (pseudoHeader, 0, tempBuffer, 0, pseudoHeader.Length);
			Array.Copy (buffer, 0, tempBuffer, pseudoHeader.Length, buffer.Length);

			// now finally calculate the checksum from the temp buffer and copy it into
			// the packet
			m_checksum = PacketUtils.CalculateChecksum (tempBuffer);
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_checksum)), 0, buffer, 16, 2);

			#endregion
			
			return buffer;
		}
		
		
		/// <summary>
		///		Check whether or not a certain TCP flag is set.
		/// </summary>
		/// <param name="flag">
		///		The flag to check.
		///	</param>
		/// <returns>
		///		Returns true if the flag is set, false otherwise.
		///	</returns>
		public bool IsFlagSet (TcpFlags flag)
		{
			return ((m_flags & (byte)flag) == (byte)flag);
		}
		
		
		/// <summary>
		///		Set or unset a particular flag.
		/// </summary>
		/// <param name="flag">
		///		The flag to set or unset.
		///	</param>
		/// <param name="flagValue">
		///		true to set, false to unset.
		///	</param>
		public void SetFlag (TcpFlags flag, bool flagValue)
		{
			if (flagValue)
			{
				m_flags |= (byte)flag;
			}
			else
			{
				m_flags &= (byte)(~((byte)flag));
			}
		}

		
		#endregion
	}


	#endregion
}