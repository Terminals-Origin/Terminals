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

namespace Metro.TransportLayer.Icmp
{
	#region Delegates

	/// <summary>
	///		The handler for when a new packet has arrived.
	/// </summary>
	public delegate void IcmpPacketArrivedHandler (IcmpPacket packet);

	#endregion

	#region Enumerations
	
	/// <summary>
	///		The type of message.
	/// </summary>
	public enum IcmpMessageType : byte
	{
		/// <summary>
		///		The data received in the echo message must be returned in the echo
		///		reply message.
		/// </summary>
		EchoReply = 0,
		
		/// <summary>
		///		If, according to the information in the gateway's routing tables,
		///		the network specified in the internet destination field of a
		///		datagram is unreachable, e.g., the distance to the network is
		///		infinity, the gateway may send a destination unreachable message
		///		to the internet source host of the datagram. In addition, in some
		///		networks, the gateway may be able to determine if the internet
		///		destination host is unreachable. Gateways in these networks may
		///		send destination unreachable messages to the source host when the
		///		destination host is unreachable.
		///
		///		If, in the destination host, the IP module cannot deliver the
		///		datagram  because the indicated protocol module or process port is
		///		not active, the destination host may send a destination
		///		unreachable message to the source host.
		///
		///		Another case is when a datagram must be fragmented to be forwarded
		///		by a gateway yet the Don't Fragment flag is on. In this case the
		///		gateway must discard the datagram and may return a destination
		///		unreachable message.
		/// </summary>
		DestinationUnreachable = 3,
		
		/// <summary>
		///		A gateway may discard internet datagrams if it does not have the
		///		buffer space needed to queue the datagrams for output to the next
		///		network on the route to the destination network. If a gateway
		///		discards a datagram, it may send a source quench message to the
		///		internet source host of the datagram. A destination host may also
		///		send a source quench message if datagrams arrive too fast to be
		///		processed. The source quench message is a request to the host to
		///		cut back the rate at which it is sending traffic to the internet
		///		destination. The gateway may send a source quench message for
		///		every message that it discards. On receipt of a source quench
		///		message, the source host should cut back the rate at which it is
		///		sending traffic to the specified destination until it no longer
		///		receives source quench messages from the gateway. The source host
		///		can then gradually increase the rate at which it sends traffic to
		///		the destination until it again receives source quench messages.
		///
		///		The gateway or host may send the source quench message when it
		///		approaches its capacity limit rather than waiting until the
		///		capacity is exceeded. This means that the data datagram which
		///		triggered the source quench message may be delivered.
		/// </summary>
		SourceQuench = 4,
		
		/// <summary>
		///		The gateway sends a redirect message to a host in the following
		///		situation. A gateway, G1, receives an internet datagram from a
		///		host on a network to which the gateway is attached. The gateway,
		///		G1, checks its routing table and obtains the address of the next
		///		gateway, G2, on the route to the datagram's internet destination
		///		network, X.  If G2 and the host identified by the internet source
		///		address of the datagram are on the same network, a redirect
		///		message is sent to the host. The redirect message advises the
		///		host to send its traffic for network X directly to gateway G2 as
		///		this is a shorter path to the destination. The gateway forwards
		///		the original datagram's data to its internet destination.
		///
		///		For datagrams with the IP source route options and the gateway
		///		address in the destination address field, a redirect message is
		///		not sent even if there is a better route to the ultimate
		///		destination than the next address in the source route.
		/// </summary>
		Redirect = 5,
		
		/// <summary>
		///		The data received in the echo message must be returned in the echo
		///		reply message.
		/// </summary>
		Echo = 8,
		
		/// <summary>
		///		If the gateway processing a datagram finds the time to live field
		///		is zero it must discard the datagram. The gateway may also notify
		///		the source host via the time exceeded message.
		///
		///		If a host reassembling a fragmented datagram cannot complete the
		///		reassembly due to missing fragments within its time limit it
		///		discards the datagram, and it may send a time exceeded message.
		///
		///		If fragment zero is not available then no time exceeded need be
		///		sent at all.
		/// </summary>
		TimeExceeded = 11,
		
		/// <summary>
		///		If the gateway or host processing a datagram finds a problem with
		///		the header parameters such that it cannot complete processing the
		///		datagram it must discard the datagram. One potential source of
		///		such a problem is with incorrect arguments in an option. The
		///		gateway or host may also notify the source host via the parameter
		///		problem message. This message is only sent if the error caused
		///		the datagram to be discarded.
		/// </summary>
		ParameterProblem = 12,
		
		/// <summary>
		///		The data received (a timestamp) in the message is returned in the
		///		reply together with an additional timestamp. The timestamp is 32
		///		bits of milliseconds since midnight UT.
		///
		///		The Originate Timestamp is the time the sender last touched the
		///		message before sending it, the Receive Timestamp is the time the
		///		echoer first touched it on receipt, and the Transmit Timestamp is
		///		the time the echoer last touched the message on sending it.
		///
		///		If the time is not available in miliseconds or cannot be provided
		///		with respect to midnight UT then any time can be inserted in a
		///		timestamp provided the high order bit of the timestamp is also set
		///		to indicate this non-standard value.
		/// </summary>
		TimeStamp = 13,
		
		/// <summary>
		///		The data received (a timestamp) in the message is returned in the
		///		reply together with an additional timestamp. The timestamp is 32
		///		bits of milliseconds since midnight UT.
		///
		///		The Originate Timestamp is the time the sender last touched the
		///		message before sending it, the Receive Timestamp is the time the
		///		echoer first touched it on receipt, and the Transmit Timestamp is
		///		the time the echoer last touched the message on sending it.
		///
		///		If the time is not available in miliseconds or cannot be provided
		///		with respect to midnight UT then any time can be inserted in a
		///		timestamp provided the high order bit of the timestamp is also set
		///		to indicate this non-standard value.
		/// </summary>
		TimestampReply = 14,

	}


	#endregion
	
	#region Classes
	
	/// <summary>
	///		Occasionally a gateway or destination host will communicate with a source host, 
	///		for example, to report an error in datagram processing. For such purposes this 
	///		protocol, the Internet Control Message Protocol (ICMP), is used. ICMP, uses 
	///		the basic support of IP as if it were a higher level protocol, however, 
	///		ICMP is actually an integral part of IP, and must be implemented by every IP 
	///		module.
	///
	///		ICMP messages are sent in several situations: for example, when a datagram 
	///		cannot reach its destination, when the gateway does not have the buffering 
	///		capacity to forward a datagram, and when the gateway can direct the host to send 
	///		traffic on a shorter route.
	///
	///		The ICMP messages typically report errors in the processing of datagrams.  
	///		To avoid the infinite regress of messages about messages etc., no ICMP messages 
	///		are sent about ICMP messages. Also ICMP messages are only sent about errors in 
	///		handling fragment zero of fragemented datagrams. (Fragment zero has the fragment 
	///		offeset equal zero).
	/// </summary>
	public class IcmpPacket
	{
		#region Private Fields

		/// <summary>
		///		The type of message being sent. This should be enumerated and the data passed
		///		to the correct class to be parsed.
		/// </summary>
		private IcmpMessageType m_type;
		
		/// <summary>
		///		The meaning of the code field is dependant on the type of message. Codes are
		///		given a meaning once passed to the correct icmp class.
		/// </summary>
		private byte m_code;
		
		/// <summary>
		///		The checksum is the 16-bit ones's complement of the one's
		///		complement sum of the ICMP message starting with the ICMP Type.
		///		For computing the checksum, the checksum field should be zero.
		/// </summary>
		private ushort m_checksum;

		/// <summary>
		///		The rest of the ICMP packet. This data is parsed further onces
		///		passed to the specific ICMP class.
		/// </summary>
		private byte[] m_data;

		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The type of message being sent. This should be enumerated and the data passed
		///		to the correct class to be parsed.
		/// </summary>
		public IcmpMessageType MessageType
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}
		
		
		/// <summary>
		///		The meaning of the code field is dependant on the type of message. Codes are
		///		given a meaning once passed to the correct icmp class.
		/// </summary>
		public byte Code
		{
			get
			{
				return m_code;
			}
			set
			{
				m_code = value;
			}
		}
		
		
		/// <summary>
		///		The checksum is the 16-bit ones's complement of the one's
		///		complement sum of the ICMP message starting with the ICMP Type.
		///		For computing the checksum, the checksum field should be zero.
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
		///		The rest of the ICMP packet. This data is parsed further onces
		///		passed to the specific ICMP class.
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
		///		Create a new Icmp packet.
		/// </summary>
		public IcmpPacket ()
		{
		}
		
		
		/// <summary>
		///		Create a new Icmp packet.
		/// </summary>
		/// <param name="data">
		///		The byte array representing the Icmp packet.
		///	</param>
		public IcmpPacket (byte[] data)
		{
			// copy out the message type
			m_type = (IcmpMessageType)data[0];
		
			// copy out the code
			m_code = data[1];
		
			// copy out the checksum
			m_checksum = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, 2));
		
			// and the rest of the data
			m_data = new byte[data.Length - 4];
			Array.Copy (data, 4, m_data, 0, m_data.Length);
		}
		
		
		/// <summary>
		///		This method will calculate the checksum then pack the fields into a byte array 
		///		in the format of an icmp header. This can then be attached to a network 
		///		layer protocol and then transmitted over the internet.
		/// </summary>
		/// <returns>
		///		The return value is a byte array with the Icmp header at the beginning which can be
		///		appended to a network layer protocol such as IP. 
		/// </returns>
		public byte[] Serialize()
		{
			// allocate enough space
			byte[] packet = new byte[4 + (m_data != null ? m_data.Length : 0)];
		
			// copy in the message type
			packet[0] = (byte)m_type;
		
			// copy in the code
			packet[1] = m_code;

			// and the rest of the data
			if (m_data != null)
			{
				Array.Copy (m_data, 0, packet, 4, m_data.Length);
			}
			
			// calculate in the checksum
			m_checksum = PacketUtils.CalculateChecksum (packet);
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder((short)m_checksum)), 0, packet, 2, 2);
		
			
			return packet;
		}
		
		
		#endregion
	}
	
	
	#endregion
}
