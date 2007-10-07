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

namespace Metro.TransportLayer.Udp
{
	#region Delegates

	/// <summary>
	///		The handler for when a new packet has arrived.
	/// </summary>
	public delegate void UdpPacketArrivedHandler (UdpPacket packet);

	#endregion

	#region Classes

	/// <summary>
	///		The User Datagram Protocol (UDP) is defined to make available a
	///		datagram mode of packet-switched computer communication in the
	///		environment of an interconnected set of computer networks. This
	///		protocol assumes that the Internet Protocol (IP) is used as the
	///		underlying protocol.
	///
	///		This protocol provides a procedure  for application programs to send
	///		messages to other programs with a minimum of protocol mechanism. The
	///		protocol is transaction oriented, and delivery and duplicate protection
	///		are not guaranteed. Applications requiring ordered reliable delivery of
	///		streams of data should use the Transmission Control Protocol (TCP).
	/// </summary>
	public class UdpPacket
	{
		/************************************************\
		*                                                *
		*	      1         2       3      4             *
        *    +--------+--------+--------+--------+       *
        *    |     Source      |   Destination   |       *
        *    |      Port       |      Port       |       *
        *    +--------+--------+--------+--------+       *
        *    |                 |                 |       *
        *    |     Length      |    Checksum     |       *
        *    +--------+--------+--------+--------+       *
        *    |                                           *
        *    |          data octets ...                  *
        *    +---------------- ...                       *
		*                                                *
		\************************************************/ 

		#region Private Fields
		
		/// <summary>
		///		Source Port is an optional field, when meaningful, it indicates the port
		///		of the sending process, and may be assumed to be the port to which a
		///		reply should be addressed in the absence of any other information. If
		///		not used, a value of zero is inserted.
		/// </summary>
		private ushort m_sourcePort = 1000;
		
		/// <summary>
		///		Destination Port has a meaning within the context of a particular
		///		internet destination address.
		/// </summary>
		private ushort m_destPort = 1000;
		
		/// <summary>
		///		Length is the length in octets of this user datagram including this
		///		header and the data. (This  means the minimum value of the length is
		///		eight.)
		/// </summary>
		private ushort m_length = 0x8;
		
		/// <summary>
		///		Checksum is the 16-bit one's complement of the one's complement sum of a
		///		pseudo header of information from the IP header, the UDP header, and the
		///		data, padded with zero octets at the end (if necessary) to make a
		///		multiple of two octets.
		/// </summary>
		private ushort m_checksum = 0;
		
		/// <summary>
		///		All data above the UDP header is placed in this buffer.
		/// </summary>
		private byte[] m_data;

		#endregion

		#region Public Fields
		
		/// <summary>
		///		Source Port is an optional field, when meaningful, it indicates the port
		///		of the sending process, and may be assumed to be the port to which a
		///		reply should be addressed in the absence of any other information. If
		///		not used, a value of zero is inserted.
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
		///		Destination Port has a meaning within the context of a particular
		///		internet destination address.
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
		///		Length is the length in octets of this user datagram including this
		///		header and the data. (This  means the minimum value of the length is
		///		eight.)
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The minimum length of the udp header is 8 bytes.
		///	</exception>
		public ushort Length
		{
			get
			{
				return m_length;
			}
			set
			{
				if (value < 8)
				{
					throw new ArgumentOutOfRangeException ("value", "The minimum length is 8 bytes");
				}
				else
				{
					m_length = value;
				}
			}
		}
		
		
		/// <summary>
		///		Checksum is the 16-bit one's complement of the one's complement sum of a
		///		pseudo header of information from the IP header, the UDP header, and the
		///		data, padded with zero octets at the end (if necessary) to make a
		///		multiple of two octets.
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
		///		All hte data above the UDP header is placed in this buffer.
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
		///		Create a new Udp packet.
		/// </summary>
		public UdpPacket ()
		{
		}
		
		
		/// <summary>
		///		Create a new Udp packet.
		/// </summary>
		/// <param name="data">
		///		The byte array representing the Udp packet.
		///	</param>
		public UdpPacket (byte[] data)
		{
			// copy out the source port and dest port
			m_sourcePort = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, 0));
			m_destPort   = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, 2));
			
			// copy out the length
			m_length	 = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, 4));
			
			// copy out the checksum
			m_checksum	 = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, 6));
			
			// and the rest, if any
			if (data.Length > 8)
			{
				m_data = new byte[data.Length - 8];
				Array.Copy (data, 8, m_data, 0, data.Length - 8);
			}
		}
		
		
		/// <summary>
		///		This method will calculate the length field and the checksum then pack the fields 
		///		into a byte array in the format of a udp header. This can then be attached to a 
		///		network layer protocol and then transmitted over the internet.
		/// </summary>
		/// <returns>
		///		The return value is a byte array with the udp header at the beginning which can be
		///		appended to a network layer protocol such as IP. 
		/// </returns>
		/// <remarks>
		///		Note that NO CHECKSUM is calculated with this method.
		///	</remarks>
		public byte[] Serialize()
		{
			// allocate enough space for the packet
			byte[] packet = new byte[8 + (m_data != null ? m_data.Length : 0)];

			// copy in the source port and dest port
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_sourcePort)), 0, packet, 0, 2);
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_destPort)), 0, packet, 2, 2);
			
			// copy in the length
			m_length = (ushort)packet.Length;
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_length)), 0, packet, 4, 2);
			
			// copy in the rest, if any
			if (m_data != null)
			{
				Array.Copy (m_data, 0, packet, 8, m_data.Length);
			}

			return packet;
		}
		
		
		/// <summary>
		///		This method will calculate the length field and the checksum then pack the fields 
		///		into a byte array in the format of a udp header. This can then be given data, 
		///		attached to a network layer protocol and then transmitted over the internet.
		/// </summary>
		/// <param name="sourceAddress">
		///		Destination address required for the checksum calculation.	
		///	</param>
		/// <param name="destAddress">
		///		Destination address required for the checksum calculation.
		///	</param>
		/// <returns>
		///		The return value is a byte array with the udp header at the beginning which can be
		///		appended to a network layer protocol such as IP.
		/// </returns>
		/// <remarks>
		///		Note that this method WILL CALCULATE A CHECKSUM.
		///	</remarks>
		public byte[] Serialize(IPAddress sourceAddress, IPAddress destAddress)
		{
			// allocate enough space for the packet
			byte[] packet = Serialize ();
			
			// the checksum is calculated by constructing a pseudo header consisting of the
			// source/destination addresses, the protocol and the length of the udp packet
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
			//  |  zero  |protocol|   UDP length    |
			//  +--------+--------+--------+--------+

			byte[] pseudoHeader = new byte[12];

			// first copy in the source and dest addresses
			Array.Copy (sourceAddress.GetAddressBytes (), 0, pseudoHeader, 0, 4); 
			Array.Copy (destAddress.GetAddressBytes (), 0, pseudoHeader, 4, 4); 

			// then a 0 byte followed by the protocol id (udp)
			pseudoHeader[8] = 0;
			pseudoHeader[9] = (byte)ProtocolType.Udp;

			// now convert the length to network byte order and copy that in
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_length)), 0, pseudoHeader, 10,  2);

			// at this point we have build the pseudo header. Now we prefix it onto the udp header
			// and the data (which we already have stored in the variable "packet" as a byte array)
			//
			//  +-----------------------------------+
			//  |          Pseudo Header            |
			//  +-----------------------------------+
			//  |            Udp Header             |
			//  +-----------------------------------+
			//  |                                   |
			//  |          data octets ...   
			//  +---------------- ...  

			byte[] tempBuffer = new byte[pseudoHeader.Length + packet.Length];
			Array.Copy (pseudoHeader, 0, tempBuffer, 0, pseudoHeader.Length);
			Array.Copy (packet, 0, tempBuffer, pseudoHeader.Length, packet.Length);

			// now finally calculate the checksum from the temp buffer and copy it into
			// the packet
			m_checksum = PacketUtils.CalculateChecksum (tempBuffer);
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_checksum)), 0, packet, 6, 2);

			#endregion
			
			return packet;
		}
		
		
		#endregion
	}
	
	
	#endregion
}
