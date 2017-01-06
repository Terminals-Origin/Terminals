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
using Metro.NetworkLayer.IpV4;

namespace Metro.TransportLayer.Icmp
{
	#region Classes

	/// <summary>
	///		Icmp messages which consist of some kind of error where the IP header and
	///		the first 64 bits of data are placed after the unused field can inherit
	///		from this class.
	/// </summary>
	public class IcmpGenericError
	{
		/**********************************************************************\
		*                                                                      *
		*         0               1               2               3            *
		*   0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7    *
		*  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*  |     Type      |     Code      |          Checksum             |   *
		*  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*  |                             unused                            |   *
		*  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*  |      Internet Header + 64 bits of Original Data Datagram      |   *
		*  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*                                                                      *
		\**********************************************************************/

		#region private Fields

		/// <summary>
		///		The IP header and the first 64 bits of data from the originating
		///		packet which caused the ICMP message.
		/// </summary>
		private IpV4Packet m_badPacket;
	
		#endregion
		
		#region Public Fields

		/// <summary>
		///		The IP header and the first 64 bits of data from the originating
		///		packet which caused the ICMP message.
		/// </summary>
		public IpV4Packet BadPacket
		{
			get
			{
				return m_badPacket;
			}
			set
			{
				m_badPacket = value;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new generic error ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		public void Constructor (IcmpPacket packet)
		{
			if (packet == null)
			{
				throw new ArgumentNullException ("packet");
			}

			// discard the unused field
			byte[] tempBuffer = new byte[packet.Data.Length - 4];
			Array.Copy (packet.Data, 4, tempBuffer, 0, tempBuffer.Length);
			
			// parse out the packet data
			m_badPacket = new IpV4Packet (tempBuffer);
		}
		
		
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public virtual IcmpPacket Serialize ()
		{
			IcmpPacket packet = new IcmpPacket ();
			
			// serialize the bad packet and allocate enough space in the buffer
			// for it	
			byte[] badPacket = m_badPacket.Serialize ();
			byte[] buffer = new byte[4 + badPacket.Length];
		
			// copy in the bad packet just after the unused field
			Array.Copy (badPacket, 0, buffer, 4, badPacket.Length);
			
			return packet;
		}
		
		
		#endregion
	}
	

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
	public class IcmpDestinationUnreachable : IcmpGenericError
	{
		#region Enumerations
		
		/// <summary>
		///		The code type.
		/// </summary>
		public enum CodeType : byte
		{
			/// <summary>
			///		Net unreachable.
			/// </summary>
			NetUnreachable,
			
			/// <summary>
			///		Host unreachable.
			/// </summary>
			HostUnreachable,
			
			/// <summary>
			///		Protocol unreachable.
			/// </summary>
			ProtocolUnreachable,
			
			/// <summary>
			///		Port unreachable.
			/// </summary>
			PortUnreachable,
			
			/// <summary>
			///		Fragmentation is required but the Don't Fragment (DF) bit is set.
			/// </summary>
			FragmentationNeeded,
			
			/// <summary>
			///		Source route failed.
			/// </summary>
			SourceRouteFailed
		}
		
		
		#endregion
		
		#region Private Fields
		
		/// <summary>
		///		The code type.
		/// </summary>
		private CodeType m_code = CodeType.HostUnreachable;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The code type.
		/// </summary>
		public CodeType Code
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
		

		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new destination unreachable ICMP packet.
		/// </summary>
		public IcmpDestinationUnreachable ()
		{
		}
		
		
		/// <summary>
		///		Create a new destination unreachable ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The packet was not a Destination Unreachable type packet.
		///	</exception>
		public IcmpDestinationUnreachable (IcmpPacket packet)
		{
			if (packet.MessageType != IcmpMessageType.DestinationUnreachable)
			{
				throw new ArgumentException ("The packet was not a Destination Unreachable type packet", "packet");
			}
			
			base.Constructor (packet);
			m_code = (CodeType)packet.Code;	
		}
		
	
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public override IcmpPacket Serialize ()
		{
			IcmpPacket packet = base.Serialize ();
			packet.MessageType = IcmpMessageType.DestinationUnreachable;
			
			// copy in the code
			packet.Code = (byte)m_code;
			
			return packet;
		}
		
		
		#endregion
	}


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
	public class IcmpTimeExceeded : IcmpGenericError
	{
		#region Enumerations
		
		/// <summary>
		///		The code type.
		/// </summary>
		public enum CodeType : byte
		{
			/// <summary>
			///		Time to live exceeded in transit;
			/// </summary>
			TtlExceeded,
			
			/// <summary>
			///		Fragment reassembly time exceeded.
			/// </summary>
			ReassemblyTimeExceeded
		}
		
		
		#endregion
		
		#region Private Fields
		
		/// <summary>
		///		The code type.
		/// </summary>
		private CodeType m_code = CodeType.TtlExceeded;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The code type.
		/// </summary>
		public CodeType Code
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
		

		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new time exceeded ICMP packet.
		/// </summary>
		public IcmpTimeExceeded ()
		{
		}
		
		
		/// <summary>
		///		Create a new time exceeded ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The packet was not a Time Exceeded type packet.
		///	</exception>
		public IcmpTimeExceeded (IcmpPacket packet)
		{
			if (packet.MessageType != IcmpMessageType.TimeExceeded)
			{
				throw new ArgumentException ("The packet was not a Time Exceeded type packet", "packet");
			}
			
			base.Constructor (packet);	
			m_code = (CodeType)packet.Code;
		}
		
		
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public override IcmpPacket Serialize ()
		{
			IcmpPacket packet = base.Serialize ();
			packet.MessageType = IcmpMessageType.TimeExceeded;
			
			// copy in the code
			packet.Code = (byte)m_code;
			
			return packet;
		}
		
		
		#endregion
	}


	/// <summary>
	///		If the gateway or host processing a datagram finds a problem with
	///		the header parameters such that it cannot complete processing the
	///		datagram it must discard the datagram. One potential source of
	///		such a problem is with incorrect arguments in an option. The
	///		gateway or host may also notify the source host via the parameter
	///		problem message. This message is only sent if the error caused
	///		the datagram to be discarded.
	/// </summary>	
	public class IcmpParameterProblem : IcmpGenericError
	{
		#region Private Fields
		
		/// <summary>
		///		Identifies the octet where an error was detected.
		/// </summary>
		private byte m_pointer = 0;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Identifies the octet where an error was detected.
		/// </summary>
		public byte ErrorPointer
		{
			get
			{
				return m_pointer;
			}
			set
			{
				m_pointer = value;
			}
		}
		
		
		#endregion
	
		#region Public Methods
		
		/// <summary>
		///		Create a new parameter problem ICMP packet.
		/// </summary>
		public IcmpParameterProblem ()
		{
		}
		
		
		/// <summary>
		///		Create a new parameter problem ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The packet was not a Parameter Problem type packet.
		///	</exception>
		public IcmpParameterProblem (IcmpPacket packet)
		{
			if (packet.MessageType != IcmpMessageType.ParameterProblem)
			{
				throw new ArgumentException ("The packet was not a Time Parameter Problem packet", "packet");
			}
			
			base.Constructor (packet);
			
			// extract the pointer
			m_pointer = packet.Data[0];
		}
		
		
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public override IcmpPacket Serialize ()
		{
			IcmpPacket packet = base.Serialize ();
			packet.MessageType = IcmpMessageType.ParameterProblem;
			
			// copy in the pointer
			packet.Data[0] = m_pointer;
			
			return packet;
		}
	
		
		#endregion
	}


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
	public class IcmpSourceQuench : IcmpGenericError
	{
		#region Public Methods
		
		/// <summary>
		///		Create a new source quench ICMP packet.
		/// </summary>
		public IcmpSourceQuench ()
		{
		}
		
		
		/// <summary>
		///		Create a new source quench ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The packet was not a Source Quench type packet.
		///	</exception>
		public IcmpSourceQuench (IcmpPacket packet)
		{
			if (packet.MessageType != IcmpMessageType.SourceQuench)
			{
				throw new ArgumentException ("The packet was not a Source Quench type packet", "packet");
			}
			
			base.Constructor (packet);	
		}
		
		
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public override IcmpPacket Serialize ()
		{
			IcmpPacket packet = base.Serialize ();
			packet.MessageType = IcmpMessageType.SourceQuench;
			
			return packet;
		}
		
		
		#endregion
	}


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
	public class IcmpRedirect : IcmpGenericError
	{
		#region Enumerations
		
		/// <summary>
		///		The code type.
		/// </summary>
		public enum CodeType : byte
		{
			/// <summary>
			///		Redirect datagrams for the Network.
			/// </summary>
			RedirectForNetwork,
			
			/// <summary>
			///		Redirect datagrams for the Host.
			/// </summary>
			RedirectForHost,
			
			/// <summary>
			///		Redirect datagrams for the Type of Service and Network.
			/// </summary>
			RedirectForTOSAndNetwork,
			
			/// <summary>
			///		Redirect datagrams for the Type of Service and Host.
			/// </summary>
			RedirectForTOSAndHost
		}
		
		
		#endregion
	
		#region Private Fields
		
		/// <summary>
		///		The code type.
		/// </summary>
		private CodeType m_code = CodeType.RedirectForHost;
		
		/// <summary>
		///		Address of the gateway to which traffic for the network specified
		///		in the internet destination network field of the original
		///		datagram's data should be sent.
		/// </summary>
		private IPAddress m_gatewayAddress = IPAddress.Any;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The code type.
		/// </summary>
		public CodeType Code
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
		///		Address of the gateway to which traffic for the network specified
		///		in the internet destination network field of the original
		///		datagram's data should be sent.
		/// </summary>
		public IPAddress GatewayAddress
		{
			get
			{
				return m_gatewayAddress;
			}
			set
			{
				m_gatewayAddress = value;
			}
		}


		#endregion
	
		#region Public Methods
		
		/// <summary>
		///		Create a new redirect ICMP packet.
		/// </summary>
		public IcmpRedirect ()
		{
		}
		
		
		/// <summary>
		///		Create a new redirect ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The packet was not a Redirect type packet.
		///	</exception>
		public IcmpRedirect (IcmpPacket packet)
		{
			if (packet.MessageType != IcmpMessageType.Redirect)
			{
				throw new ArgumentException ("The packet was not a Redirect type packet", "packet");
			}
			
			base.Constructor (packet);
			m_code = (CodeType)packet.Code;
			
			// copy out the address
			m_gatewayAddress = new IPAddress ((long)BitConverter.ToInt32 (packet.Data, 4));
		}
		
		
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public override IcmpPacket Serialize ()
		{
			IcmpPacket packet = base.Serialize ();
			packet.MessageType = IcmpMessageType.SourceQuench;
			
			// copy in the code
			packet.Code = (byte)m_code;
			
			// copy in the address
			Array.Copy (m_gatewayAddress.GetAddressBytes (), 0, packet.Data, 4, 4);
			
			return packet;
		}
		
		
		#endregion
	}


	/// <summary>
	///		The data received in the echo message must be returned in the echo
	///		reply message.
	/// </summary>
	public class IcmpEcho
	{
		#region Private Fields
		
		/// <summary>
		///		Used for generating random numbers
		/// </summary>
		private Random m_random = new Random ();
		
		/// <summary>
		///		An identifier to aid in matching echos and replies, may be zero.
		/// </summary>
		private ushort m_identifier;
		
		/// <summary>
		///		A sequence number to aid in matching echos and replies, may be zero.
		/// </summary>
		private ushort m_sequenceNumber;
		
		/// <summary>
		///		The data received in the echo message must be returned in the echo
		///		reply message.
		/// </summary>
		private byte[] m_data;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		An identifier to aid in matching echos and replies, may be zero.
		/// </summary>
		public ushort Identifier
		{
			get
			{
				return m_identifier;
			}
			set
			{
				m_identifier = value;
			}
		}
		
		
		/// <summary>
		///		A sequence number to aid in matching echos and replies, may be zero.
		/// </summary>
		public ushort SequenceNumber
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
		///		The data received in the echo message must be returned in the echo
		///		reply message.
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
		///		Create a new echo ICMP packet.
		/// </summary>
		public IcmpEcho ()
		{
			m_identifier = (ushort)m_random.Next (ushort.MaxValue);
			m_sequenceNumber = (ushort)m_random.Next (ushort.MaxValue);
		}
		
		
		/// <summary>
		///		Create a new echo ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The ICMP packet to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		If the packet is null then an argument null exception occurs.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The packet was not a Echo packet.
		///	</exception>
		public IcmpEcho (IcmpPacket packet)
		{
			if (packet == null)
			{
				throw new ArgumentNullException ("packet");
			}
			
			if (packet.MessageType != IcmpMessageType.Echo && packet.MessageType != IcmpMessageType.EchoReply)
			{
				throw new ArgumentException ("The packet was not an Echo or Echo Reply type packet", "packet");
			}
			
			// extract the identifier
			m_identifier = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (packet.Data, 0));
			
			// extract the sequence number
			m_sequenceNumber = (ushort)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (packet.Data, 2));
			
			// copy out the data
			m_data = new byte[packet.Data.Length - 4];
			Array.Copy (packet.Data, 4, m_data, 0, m_data.Length);
		}
		
		
		/// <summary>
		///		Serlialize this icmp message into an IcmpPacket which can
		///		then be serialized further.
		/// </summary>
		/// <param name="reply">
		///		If true, this packet is an echo reply, otherwise it is an echo request.
		///	</param>
		/// <returns>	
		///		An IcmpPacket class.
		///	</returns>
		public IcmpPacket Serialize (bool reply)
		{

			IcmpPacket packet = new IcmpPacket ();
			
			packet.MessageType = (reply ? IcmpMessageType.EchoReply : IcmpMessageType.Echo);
			
			// allocate enough space for the data
			packet.Data = new byte[4 + (m_data != null ? m_data.Length : 0)];
			
			// copy in the identifier
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_identifier)), 0, packet.Data, 0, 2);
			
			// copy in the sequence number
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_sequenceNumber)), 0, packet.Data, 2, 2);
			
			// copy in the echo data
			Array.Copy (m_data, 0, packet.Data, 4, m_data.Length);
			
			return packet;
		}
		
		
		#endregion
	}


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
	public class IcmpTimeStamp
	{
	
	
	}


	#endregion
}
