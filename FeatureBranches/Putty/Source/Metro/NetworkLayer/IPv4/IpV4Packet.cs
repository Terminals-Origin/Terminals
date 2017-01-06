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

namespace Metro.NetworkLayer.IpV4
{
	#region Delegates

	/// <summary>
	///		The handler for when a new packet has arrived.
	/// </summary>
	public delegate void IpV4PacketArrivedHandler (IpV4Packet packet);

	#endregion

	#region Enumerations

	/// <summary>
	///		The option class.
	/// </summary>
	public enum IpV4OptionClass : int
	{
		/// <summary>
		///		Control options.
		/// </summary>
		Control,
		
		/// <summary>
		///		Reserved for future use.
		/// </summary>
		Reserved1,
		
		/// <summary>
		///		Degugging and measurement options.
		/// </summary>
		DebuggingAndMeasurement,
		
		/// <summary>
		///		Reserved for future use.
		/// </summary>
		Reserved2
	}
	

	/// <summary>
	///		The type of option.
	/// </summary>
	public enum IpV4OptionNumber : int
	{
	
		/// <summary>
		///		This option occupies only 1 octet; it has no length octet.
		/// </summary>
		EndOfOptions = 0,
	
		/// <summary>
		///		This option occupies only 1 octet; it has no length octet.
		/// </summary>
		NoOperation = 1,

		/// <summary>
		///		Used to carry Security, Compartmentation, User Group (TCC), and
        ///		Handling Restriction Codes compatible with DOD equirements.
		/// </summary>
		Security = 2,

		/// <summary>
		///		Used to route the internet datagram based on information
		///		supplied by the source.
		/// </summary>
		LooseSourceRouting = 3, 
		
		/// <summary>
		///		Internet Timestamp
		/// </summary>
		InternetTimestamp = 4,

		/// <summary>
		///		Used to trace the route an internet datagram takes.
		/// </summary>
		RecordRoute = 7, 
	
		/// <summary>
		///		Used to carry the stream identifier.
		/// </summary>
		StreamId = 8, 
		
		/// <summary>
		///		Strict Source Routing.  Used to route the internet datagram based 
		///		on information supplied by the source.
		/// </summary>
		StrictSourceRouting = 9
	}


	/// <summary>
	///		An independent measure of the importance of this datagram.
	///		Several networks offer service precedence, which somehow treats high
	///		precedence traffic as more important than other traffic (generally
	///		by accepting only traffic above a certain precedence at time of high
	///		load).
	/// </summary>
	public enum IpV4PrecedenceType : byte
	{
		/// <summary>
		///		Routine traffic. This is the most widely used. Use this when in doubt.
		/// </summary>
		Routine,
		
		/// <summary>
		///		Priority.
		/// </summary>
		Priority,
		
		/// <summary>
		///		Immediate.
		/// </summary>
		Immediate,
		
		/// <summary>
		///		Flash.
		/// </summary>
		Flash,
		
		/// <summary>
		///		Flash Override.
		/// </summary>
		FlashOverride,
		
		/// <summary>
		///		Critic/ECP.
		/// </summary>
		CriticECP,
		
		/// <summary>
		///		Internetwork Control.
		/// </summary>
		InternetworkControl,
		
		/// <summary>
		///		Network Control.
		/// </summary>
		NetworkControl
	}
	
	
	/// <summary>
	///		Prompt delivery is important for datagrams with this indication.
	///		The use of the Delay, Throughput, and Reliability indications may
	///		increase the cost (in some sense) of the service. In many networks
	///		better performance for one of these parameters is coupled with worse
	///		performance on another. Except for very unusual cases at most two
	///		of these three indications should be set.
	/// </summary>
	public enum IpV4DelayType : byte
	{
		/// <summary>
		///		Normal.
		/// </summary>
		Normal,
		
		/// <summary>
		///		Low.
		/// </summary>
		Low
	}
	
	
	/// <summary>
	///		High data rate is important for datagrams with this indication.
	///		The use of the Delay, Throughput, and Reliability indications may
	///		increase the cost (in some sense) of the service. In many networks
	///		better performance for one of these parameters is coupled with worse
	///		performance on another. Except for very unusual cases at most two
	///		of these three indications should be set.
	/// </summary>
	public enum IpV4ThroughputType : byte
	{
		/// <summary>
		///		Normal.
		/// </summary>
		Normal,
		
		/// <summary>
		///		High.
		/// </summary>
		High
	}
	
	
	/// <summary>
	///		A higher level of effort to ensure delivery is important for datagrams 
	///		with this indication.
	///		The use of the Delay, Throughput, and Reliability indications may
	///		increase the cost (in some sense) of the service. In many networks
	///		better performance for one of these parameters is coupled with worse
	///		performance on another. Except for very unusual cases at most two
	///		of these three indications should be set.
	/// </summary>
	public enum IpV4ReliabilityType : byte
	{
	
		/// <summary>
		///		Normal.
		/// </summary>
		Normal,
		
		/// <summary>
		///		High.
		/// </summary>
		High
	}
	

	#endregion

	#region Classes

	/// <summary>
	///		This class describes the type of service field in the IP header which
	///		The type of service (TOS) is for internet service quality selection.
	///		The type of service is specified along the abstract parameters
	///		precedence, delay, throughput, and reliability. These abstract
	///		parameters are to be mapped into the actual service parameters of
	///		the particular networks the datagram traverses.
	///	</summary>
	public class IpV4TypeOfServiceType
	{
		#region Private Fields
		
		/// <summary>
		///		An independent measure of the importance of this datagram.
		///		Several networks offer service precedence, which somehow treats high
		///		precedence traffic as more important than other traffic (generally
		///		by accepting only traffic above a certain precedence at time of high
		///		load).
		/// </summary>
		private IpV4PrecedenceType m_precedence = IpV4PrecedenceType.Routine;
		
		/// <summary>
		///		Prompt delivery is important for datagrams with this indication.
		///		The use of the Delay, Throughput, and Reliability indications may
		///		increase the cost (in some sense) of the service. In many networks
		///		better performance for one of these parameters is coupled with worse
		///		performance on another. Except for very unusual cases at most two
		///		of these three indications should be set.
		/// </summary>
		private IpV4DelayType m_delay = IpV4DelayType.Normal;
		
		/// <summary>
		///		High data rate is important for datagrams with this indication.
		///		The use of the Delay, Throughput, and Reliability indications may
		///		increase the cost (in some sense) of the service. In many networks
		///		better performance for one of these parameters is coupled with worse
		///		performance on another. Except for very unusual cases at most two
		///		of these three indications should be set.
		/// </summary>
		private IpV4ThroughputType m_throughput = IpV4ThroughputType.Normal;
		
		/// <summary>
		///		A higher level of effort to ensure delivery is important for datagrams 
		///		with this indication.
		///		The use of the Delay, Throughput, and Reliability indications may
		///		increase the cost (in some sense) of the service. In many networks
		///		better performance for one of these parameters is coupled with worse
		///		performance on another. Except for very unusual cases at most two
		///		of these three indications should be set.
		/// </summary>
		private IpV4ReliabilityType m_reliability = IpV4ReliabilityType.Normal;

		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		An independent measure of the importance of this datagram.
		///		Several networks offer service precedence, which somehow treats high
		///		precedence traffic as more important than other traffic (generally
		///		by accepting only traffic above a certain precedence at time of high
		///		load).
		/// </summary>
		public IpV4PrecedenceType Precedence
		{
			get
			{
				return m_precedence;
			}
			set
			{
				m_precedence = value;
			}
		}
		
		
		/// <summary>
		///		Prompt delivery is important for datagrams with this indication.
		///		The use of the Delay, Throughput, and Reliability indications may
		///		increase the cost (in some sense) of the service. In many networks
		///		better performance for one of these parameters is coupled with worse
		///		performance on another. Except for very unusual cases at most two
		///		of these three indications should be set.
		/// </summary>
		public IpV4DelayType Delay
		{
			get
			{
				return m_delay;
			}
			set
			{
				m_delay = value;
			}
		}
		
		
		/// <summary>
		///		High data rate is important for datagrams with this indication.
		///		The use of the Delay, Throughput, and Reliability indications may
		///		increase the cost (in some sense) of the service. In many networks
		///		better performance for one of these parameters is coupled with worse
		///		performance on another. Except for very unusual cases at most two
		///		of these three indications should be set.
		/// </summary>
		public IpV4ThroughputType Throughput
		{
			get
			{
				return m_throughput;
			}
			set
			{
				m_throughput = value;
			}
		}
		
		
		/// <summary>
		///		A higher level of effort to ensure delivery is important for datagrams 
		///		with this indication.
		///		The use of the Delay, Throughput, and Reliability indications may
		///		increase the cost (in some sense) of the service. In many networks
		///		better performance for one of these parameters is coupled with worse
		///		performance on another. Except for very unusual cases at most two
		///		of these three indications should be set.
		/// </summary>
		public IpV4ReliabilityType Reliability
		{
			get
			{
				return m_reliability;
			}
			set
			{
				m_reliability = value;
			}
		}
		
		
		#endregion
	}

	
	/// <summary>
	///		Various Control Flags.
	/// </summary>
	public class IpV4ControlFlags
	{
		#region Private Fields

		/// <summary>
		///		An identifying value assigned by the sender to aid in assembling the
		///		fragments of a datagram. If this is set then the packet will not be
		///		fragmented.
		/// </summary>
		private bool m_doNotFragment = true;
		
		/// <summary>
		///		An identifying value assigned by the sender to aid in assembling the
		///		fragments of a datagram. If this is set then the packet is fragmented.
		/// </summary>
		private bool m_moreFragments = false;
		
		/// <summary>
		///		This field indicates where in the datagram this fragment belongs.
		/// </summary>
		private ushort m_fragmentationOffset = 0;

		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		An identifying value assigned by the sender to aid in assembling the
		///		fragments of a datagram. If this is set then the packet will not be
		///		fragmented.
		/// </summary>
		public bool DontFragment
		{
			get
			{
				return m_doNotFragment;
			}
			set
			{
				m_doNotFragment = value;
			}
		}
		
		
		/// <summary>
		///		An identifying value assigned by the sender to aid in assembling the
		///		fragments of a datagram. If this is set then the packet is fragmented.
		/// </summary>
		public bool MoreFragments
		{
			get
			{
				return m_moreFragments;
			}
			set
			{
				m_moreFragments = value;
			}
		}
		
		
		/// <summary>
		///		This field indicates where in the datagram this fragment belongs.
		/// </summary>
		public ushort Offset
		{
			get
			{
				return m_fragmentationOffset;
			}
			set
			{
				m_fragmentationOffset= value;
			}
		}
		
		
		#endregion
	}

	
	/// <summary>
	///		The options may appear or not in datagrams.
	/// </summary>
	public class IpV4Option
	{
		#region Private Fields
	
		/// <summary>
		///		If the copied field is set to true then the option is copied to all
		///		fragments of the packet.
		/// </summary>
		private bool m_copied = true;

		/// <summary>
		///		The option class.
		/// </summary>
		private IpV4OptionClass m_class = IpV4OptionClass.Control;
	
		/// <summary>
		///		The option type.
		/// </summary>
		private IpV4OptionNumber m_number = IpV4OptionNumber.NoOperation;
	
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
		///		If the copied field is set to true then the option is copied to all
		///		fragments of the packet.
		/// </summary>
		public bool IsCopied
		{
			get
			{
				return m_copied;
			}
			set
			{
				m_copied = value;
			}
		}
		
		
		/// <summary>
		///		The option class.
		/// </summary>
		public IpV4OptionClass Class
		{
			get
			{
				return m_class;
			}
			set
			{
				m_class = value;
			}
		}
		
		
		/// <summary>
		///		The option type.
		/// </summary>
		public IpV4OptionNumber OptionType
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
		///		classes
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
	///		This class is used for working with version 4 of the internet protocol.
	///		The Internet Protocol is designed for use in interconnected systems of
	///		packet-switched computer communication networks.  Such a system has
	///		been called a "catenet" [1].  The internet protocol provides for
	///		transmitting blocks of data called datagrams from sources to
	///		destinations, where sources and destinations are hosts identified by
	///		fixed length addresses.  The internet protocol also provides for
	///		fragmentation and reassembly of long datagrams, if necessary, for
	///		transmission through "small packet" networks.
	/// </summary>
	/// <remarks>
	///		Note that this class will fully handle fragmentation when used in
	///		parallel with the IpV4Defragmentor class.
	///	</remarks>
	public class IpV4Packet
	{
		/***********************************************************************\
		*                                                                       *
		*	       1               2               3               4            *
		*    0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7    *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |Version|  IHL  |Type of Service|          Total Length         |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |         Identification        |Flags|      Fragment Offset    |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |  Time to Live |    Protocol   |         Header Checksum       |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                       Source Address                          |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                    Destination Address                        |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*   |                    Options                    |    Padding    |   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   *
		*                                                                       *
		\***********************************************************************/ 

		#region Private Fields
		
		/// <summary>
		///		Used for generating random numbers.
		/// </summary>
		private Random m_random = new Random ();
		
		/// <summary>
		///		The Version field indicates the format of the internet header. This
		///		class uses version 4.
		/// </summary>
		private byte m_version = 4;
		
		/// <summary>
		///		Internet Header Length is the length of the internet header in octets,
		///		and thus points to the beginning of the data. Note that the minimum 
		///		value for a correct header is 20 octets.
		/// </summary>
		private ushort m_headerLength = 0x14;
		
		/// <summary>
		///		The type of service (TOS) is for internet service quality selection.
		///		The type of service is specified along the abstract parameters
		///		precedence, delay, throughput, and reliability. These abstract
		///		parameters are to be mapped into the actual service parameters of
		///		the particular networks the datagram traverses.
		///	</summary>
		private IpV4TypeOfServiceType m_tos = new IpV4TypeOfServiceType ();
		
		/// <summary>
		///		Total Length is the length of the datagram, measured in octets,
		///		including internet header and data. This field allows the length of
		///		a datagram to be up to 65,535 octets. Such long datagrams are
		///		impractical for most hosts and networks. All hosts must be prepared
		///		to accept datagrams of up to 576 octets (whether they arrive whole
		///		or in fragments). It is recommended that hosts only send datagrams
		///		larger than 576 octets if they have assurance that the destination
		///		is prepared to accept the larger datagrams.
		/// </summary>
		private ushort m_totalLength = 0x20;
		
		/// <summary>
		///		An identifying value assigned by the sender to aid in assembling the
		///		fragments of a datagram.
		/// </summary>
		private ushort m_id;
		
		/// <summary>
		///		Various Control Flags.
		/// </summary>
		private IpV4ControlFlags m_icf = new IpV4ControlFlags ();
		
		/// <summary>
		///		This field indicates the maximum time the datagram is allowed to
		///		remain in the internet system. If this field contains the value
		///		zero, then the datagram must be destroyed. This field is modified
		///		in internet header processing. The time is measured in units of
		///		seconds, but since every module that processes a datagram must
		///		decrease the TTL by at least one even if it process the datagram in
		///		less than a second, the TTL must be thought of only as an upper
		///		bound on the time a datagram may exist. The intention is to cause
		///		undeliverable datagrams to be discarded, and to bound the maximum
		///		datagram lifetime.
		/// </summary>
		private byte m_ttl = 128;
	
		/// <summary>
		///		This field indicates the next level protocol used in the data
		///		portion of the internet datagram.
		/// </summary>
		private ProtocolType m_protocol = ProtocolType.Tcp;
		
		/// <summary>
		///		A checksum on the header only. Since some header fields change
		///		(e.g., time to live), this is recomputed and verified at each point
		///		that the internet header is processed.
		///
		///		The checksum field is the 16 bit one's complement of the one's
		///     complement sum of all 16 bit words in the header. For purposes of
		///     computing the checksum, the value of the checksum field is zero.
		/// </summary>
		private ushort m_checksum = 0;
		
		/// <summary>
		///		The source address.
		/// </summary>
		private IPAddress m_sourceAddress = IPAddress.Any;
		
		/// <summary>
		///		The destination address.
		/// </summary>
		private IPAddress m_destAddress = IPAddress.Any;
	
		/// <summary>
		///		Optional fields.
		/// </summary>
		private IpV4Option[] m_options;
	
		/// <summary>
		///		The internet header padding is used to ensure that the internet
		///		header ends on a 32 bit boundary.  The padding is zero.
		/// </summary>
		private byte[] m_padding;
	
		/// <summary>
		///		All data above the IP header is placed in this buffer.
		/// </summary>
		private byte[] m_data;
	
		/// <summary>
		///		If this packet was assembled from fragments, this field contains how 
		///		many fragments it contained.
		/// </summary>
		private byte m_fragments = 0;

		#endregion

		#region Public Fields
		
		/// <summary>
		///		The Version field indicates the format of the internet header. This
		///		class uses version 4.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The version must be version 4.
		///	</exception>
		public byte Version
		{
			get
			{
				return m_version;
			}
			set
			{
				if (value != 0x04)
				{
					throw new ArgumentOutOfRangeException ("This class uses IP version 4 only.");
				}
			
				m_version = value;
			}
		}
		
		
		/// <summary>
		///		Internet Header Length is the length of the internet header in octets,
		///		and thus points to the beginning of the data. Note that the minimum 
		///		value for a correct header is 20 octets.
		/// </summary>
		///	<exception cref="ArgumentOutOfRangeException">
		///		The header length was either too small or too large. The header length must
		///		be between 20 and 60 bytes (Within the interval [20, 60]).
		///	</exception>
		public ushort HeaderLength
		{
			get
			{
				return m_headerLength;
			}
			set
			{
				if (value < 0x14)
				{
					throw new ArgumentOutOfRangeException ("The minimum length an internet header can be is 20 bytes");
				}
				if (value > 0x3c)
				{
					throw new ArgumentOutOfRangeException ("The maximum length an internet header can be is 60 bytes");
				}
				
				m_headerLength = value;
			}
		}
		
		
		/// <summary>
		///		The type of service (TOS) is for internet service quality selection.
		///		The type of service is specified along the abstract parameters
		///		precedence, delay, throughput, and reliability. These abstract
		///		parameters are to be mapped into the actual service parameters of
		///		the particular networks the datagram traverses.
		///	</summary>
		public IpV4TypeOfServiceType TypeOfService
		{
			get
			{
				return m_tos;
			}
			set
			{
				m_tos = value;
			}
		}
		
		
		/// <summary>
		///		Total Length is the length of the datagram, measured in octets,
		///		including internet header and data. This field allows the length of
		///		a datagram to be up to 65,535 octets. Such long datagrams are
		///		impractical for most hosts and networks. All hosts must be prepared
		///		to accept datagrams of up to 576 octets (whether they arrive whole
		///		or in fragments). It is recommended that hosts only send datagrams
		///		larger than 576 octets if they have assurance that the destination
		///		is prepared to accept the larger datagrams.
		/// </summary>
		public ushort TotalLength
		{
			get
			{
				return m_totalLength;
			}
			set
			{
				m_totalLength = value;
			}
		}
		
		
		/// <summary>
		///		An identifying value assigned by the sender to aid in assembling the
		///		fragments of a datagram.
		/// </summary>
		public ushort Identification
		{
			get
			{
				return m_id;
			}
			set
			{
				m_id = value;
			}
		}
		
		
		/// <summary>
		///		Various Control Flags.
		/// </summary>
		public IpV4ControlFlags ControlFlags
		{
			get
			{
				return m_icf;
			}
			set
			{
				m_icf = value;
			}
		}
		
		
		/// <summary>
		///		This field indicates the maximum time the datagram is allowed to
		///		remain in the internet system. If this field contains the value
		///		zero, then the datagram must be destroyed. This field is modified
		///		in internet header processing. The time is measured in units of
		///		seconds, but since every module that processes a datagram must
		///		decrease the TTL by at least one even if it process the datagram in
		///		less than a second, the TTL must be thought of only as an upper
		///		bound on the time a datagram may exist. The intention is to cause
		///		undeliverable datagrams to be discarded, and to bound the maximum
		///		datagram lifetime.
		/// </summary>
		public byte TimeToLive
		{
			get
			{
				return m_ttl;
			}
			set
			{
				m_ttl = value;
			}
		}
			
			
		/// <summary>
		///		This field indicates the next level protocol used in the data
		///		portion of the internet datagram.
		/// </summary>
		public ProtocolType TransportProtocol
		{
			get
			{
				return m_protocol;
			}
			set
			{
				m_protocol = value;
			}
		}


		/// <summary>
		///		A checksum on the header only. Since some header fields change
		///		(e.g., time to live), this is recomputed and verified at each point
		///		that the internet header is processed.
		///
		///		The checksum field is the 16 bit one's complement of the one's
		///     complement sum of all 16 bit words in the header. For purposes of
		///     computing the checksum, the value of the checksum field is zero.
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
		///		The source address.
		/// </summary>
		public IPAddress SourceAddress
		{
			get
			{
				return m_sourceAddress;
			}
			set
			{
				m_sourceAddress = value;
			}
		}
		
		
		/// <summary>
		///		The destination address.
		/// </summary>
		public IPAddress DestinationAddress
		{
			get
			{
				return m_destAddress;
			}
			set
			{
				m_destAddress = value;
			}
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		public IpV4Option[] Options
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
		///		The internet header padding is used to ensure that the internet
		///		header ends on a 32 bit boundary.  The padding is zero.
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
		///		All the data above the IP header is placed in this buffer.
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
		
		/// <summary>
		///		If this packet was assembled from fragments then this field contains
		///		how many fragments it was made from.
		/// </summary>
		public byte Fragments
		{
			get
			{
				return m_fragments;
			}
			set
			{
				m_fragments = value;
			}
		}
		
		
		#endregion

		#region Public Methods

		/// <summary>
		///		Create a new IPv4 packet.
		/// </summary>
		public IpV4Packet ()
		{	
			m_id = (ushort)m_random.Next (ushort.MaxValue);
		}

		
		/// <summary>
		///		Create a new IPv4 packet.
		/// </summary>
		/// <param name="packetFragments">
		///		The fragments from a fragmented packet which will be peiced together
		///		into a whole single packet.
		///	</param>
		public IpV4Packet (IpV4Packet[] packetFragments)
		{
			
			int index = 0;

			// get the index of the first packet fragment
			for (int i = 0; i < packetFragments.Length; i++)
			{
				if (packetFragments[i].ControlFlags.Offset == 0)
				{
					index = i;
					break;
				}
			}
			
			// copy fields over from the first packet
			m_version				= 0x04;
			m_destAddress			= packetFragments[index].DestinationAddress;
			m_headerLength			= packetFragments[index].HeaderLength;
			m_id					= packetFragments[index].Identification;
			m_protocol				= packetFragments[index].TransportProtocol;
			m_sourceAddress			= packetFragments[index].SourceAddress;
			m_ttl					= packetFragments[index].TimeToLive;
			m_tos.Precedence		= packetFragments[index].TypeOfService.Precedence;
			m_tos.Delay				= packetFragments[index].TypeOfService.Delay;
			m_tos.Throughput		= packetFragments[index].TypeOfService.Throughput;
			m_tos.Reliability		= packetFragments[index].TypeOfService.Reliability;
			m_icf.DontFragment		= false;
			m_icf.MoreFragments		= false;
			m_icf.Offset			= 0;
			m_fragments				= (byte)packetFragments.Length;
			
			if (packetFragments[index].Options != null)
			{
				m_options = new IpV4Option [packetFragments[index].Options.Length];
				packetFragments[index].Options.CopyTo (m_options, 0);			
			}

			if (packetFragments[index].Padding != null)
			{
				m_padding = new byte [packetFragments[index].Padding.Length];
				packetFragments[index].Padding.CopyTo (m_padding, 0);
			}
			
			
			// calculate the total size of the packet. We need to do this first so we have
			// enough space in the buffer for all the fragments because there is no guarantee
			// of the order, so we can't just resize it dynamically each time we process a 
			// new fragment. It could go anywhere in the buffer.
			m_totalLength = m_headerLength;
						
			for (int i = 0; i < packetFragments.Length; i++)
			{
				m_totalLength += (ushort) packetFragments[i].Data.Length;
			}

			// ok now allocate enough space
			m_data = new byte[m_totalLength - m_headerLength];

			// now loop through each fragment, and copy it's data into the buffer
			for (int i = 0; i < packetFragments.Length; i++)
			{
				Array.Copy (packetFragments[i].Data, 0, m_data, packetFragments[i].ControlFlags.Offset, packetFragments[i].Data.Length);
			}
		}

		
		/// <summary>
		///		Create a new IPv4 packet.
		/// </summary>
		/// <param name="data">
		///		The byte array representing the IP packet.
		///	</param>
		public IpV4Packet (byte[] data)
		{
			int position = 0;
			
			#region Basic Fields
			
			// extract the version field from the first 4 bits of the packet
			m_version			= Convert.ToByte ((data[position] >> 4));
			
			// extract the header length field from the next 4 bits. This is the number
			// of 32 bit words so multiply by 4 to get the number of 8 bit words..i.e..bytes
			m_headerLength		= Convert.ToByte ((data[position] & 0x0f) * 4);
			position++;
			
			// extract the precedence from the next 3 bits
			m_tos.Precedence	= (IpV4PrecedenceType)	(data[position] >> 5);
			
			// then extract the delay, throughput and reliability bits
			m_tos.Delay			= (IpV4DelayType)		((data[position] & 0x10) >> 4);
			m_tos.Throughput	= (IpV4ThroughputType)	((data[position] & 0x8) >> 3); 
			m_tos.Reliability	= (IpV4ReliabilityType)	((data[position] & 0x4) >> 2); 
			position++;
			
			// get the total length (remembering to convert it to host order)
			m_totalLength		= (ushort)(IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position)));
			position += 2;
			
			// same with the identification field
			m_id				= (ushort)(IPAddress.NetworkToHostOrder (BitConverter.ToInt16	(data, position)));
			position += 2;
			
			// extract the internet control flags
			m_icf.DontFragment	= ((data[position] & 0x40) >> 6) == 1;
			m_icf.MoreFragments	= ((data[position] & 0x20) >> 5) == 1;
			
			// and the fragmentation offsent (remember to convert to host order)
			m_icf.Offset	= (ushort)(IPAddress.NetworkToHostOrder ((Convert.ToInt16 (BitConverter.ToInt16 (data, position) & 0x1fff))) * 8);
			position += 2;
			
			// the time to live is just in a single byte
			m_ttl			= data[position];
			position++;
			
			// as is the protocol
			m_protocol		= (ProtocolType)(data[position]);
			position++;
			
			// the checksum
			m_checksum		= (ushort)(IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position)));
			position += 2;
			
			// now copy in the source address
			m_sourceAddress	= new IPAddress (BitConverter.ToUInt32 (data, position));
			position += 4;
			
			// and the destination address
			m_destAddress	= new IPAddress (BitConverter.ToUInt32 (data, position));
			position += 4;
			
			#endregion
			
			#region Options and Padding
			
			// check if there are any options (if the header length is greater than the
			// smallest, 0x14 bytes)
			if (m_headerLength > 0x14)
			{
				// create a new buffer of the correct size and copy the options into it
				byte[] optionsAndPadding = new byte[m_headerLength - 0x14];
				Array.Copy (data, position, optionsAndPadding, 0, optionsAndPadding.Length);
				
				// go through each byte of the options
				for (int i = 0; i < optionsAndPadding.Length;)
				{
					IpV4Option option = new IpV4Option ();
					
					// fill in basic fields
					option.OptionType	= (IpV4OptionNumber)optionsAndPadding[i];
					i++;
					
					#region Fill in Basic Option Fields
					
					if (option.OptionType == IpV4OptionNumber.EndOfOptions)
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
					else if (option.OptionType != IpV4OptionNumber.NoOperation)
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
						m_options = new IpV4Option[1];
					}
					else
					{
						IpV4Option[] tempOptions = new IpV4Option[m_options.Length];
						Array.Copy (m_options, 0, tempOptions, 0, m_options.Length);
						
						m_options = new IpV4Option[m_options.Length + 1];
						Array.Copy (tempOptions, 0,  m_options, 0, tempOptions.Length);
						
						m_options[m_options.Length - 1] = option;
					}
			
					m_options[m_options.Length - 1] = option;
					
					#endregion
							
					if (option.OptionType == IpV4OptionNumber.EndOfOptions)
					{
						break;
					}
				}
	
				position += optionsAndPadding.Length;
			}
			
			#endregion
			
			// now check if there is any data (which there should be)
			if (data.Length > m_headerLength)
			{
				// allocate a new buffer for it and copy the data in to the buffer
				m_data = new byte[data.Length - m_headerLength];
				Array.Copy (data, position, m_data, 0, m_data.Length);
			}
		}


		/// <summary>
		///		This method will calculate fields such as the checksum, the total length
		///		and the header length then pack the fields into a byte array in the format of
		///		an ip header. This can then be given a transport layer and data and then transmitted 
		///		over the internet.
		/// </summary>
		/// <returns>
		///		The return value is a byte array with the ip header at the beginning which can be
		///		written straight to a raw socket for transmission over the internet,
		///	</returns>
		public byte[] Serialize()
		{
			#region Initial Calculations
			
			// used for various things
			byte[] tempBuffer;
			
			// this class always uses version 4
			m_version = 0x04;

			// the header length has at least 0x14 bytes
			m_headerLength = 0x14;
			
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
				m_headerLength += (ushort) (optionsLength + paddingLength);
				
			}

			// the minimum size of the packet is the size of the ip header.
			// although it wouldn't be much of a packet with just an ip header.
			m_totalLength = (ushort) m_headerLength;
			
			// if there is some data then account for that in the total length field
			if (m_data != null)
			{
				m_totalLength += (ushort) m_data.Length;
			}
			
			#endregion
			
			// allocate a large enough buffer to hold the header + the data
			byte[] buffer = new byte[m_totalLength];
			int position = 0;
			
			#region Copy Internet Header Fields
			
			// pack the header length and version fields into the first byte.
			// (note that the header length is the number of 32 bit words, so we divide it by 4.
			// This is why the header size must be a multiple of 4)
			buffer[position] =  (byte) (m_headerLength / 4);
			buffer[position] |= (byte) (m_version << 4);
			position++;
			
			// now pack the type of service fields into the next byte 
			buffer[position] =  (byte) (m_tos.Precedence);
			buffer[position] |= (byte) (m_tos.Delay			== IpV4DelayType.Low		? 0x10 : 0x00);
			buffer[position] |= (byte) (m_tos.Throughput	== IpV4ThroughputType.High	? 0x08 : 0x00);	
			buffer[position] |= (byte) (m_tos.Reliability	== IpV4ReliabilityType.High ? 0x04 : 0x00);	
			position++;
			
			// copy in the total length (remember to convert it to network byte order)
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short) m_totalLength)), 0, buffer, position, 2);
			position += 2;
			
			// and the identification field
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short) m_id)), 0, buffer, position, 2);
			position += 2;
			
			// now copy the internet control flags and fragmentation offset into the next 2 bytes
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short) (m_icf.Offset / 8))), 0, buffer, position, 2);
			buffer[position] |= (byte) (m_icf.DontFragment  ? 0x40 : 0x00);
			buffer[position] |= (byte) (m_icf.MoreFragments ? 0x20 : 0x00);
			position += 2;
			
			// the ttl goes in the next byte
			buffer[position] = m_ttl;
			position++;
			
			// followed by the transport protocol
			buffer[position] = (byte) m_protocol;
			position++;
			
			// the next 2 bytes are the checksum. For now we leave them as 0
			buffer[position] = 0;
			position++;
			buffer[position] = 0;
			position++;
			
			// and now the source address
			Array.Copy (m_sourceAddress.GetAddressBytes (), 0, buffer, position, 4);
			position += 4;
			
			// and destination address
			Array.Copy (m_destAddress.GetAddressBytes (), 0, buffer, position, 4);
			position += 4;
		
			#endregion
			
			#region Copy In Options
		
			// now copy in any options
			if (m_options != null)
			{
				
				// go through each options
				for (int i = 0; i < m_options.Length; i++)
				{
					buffer[position] =  (byte)m_options[i].OptionType;
					buffer[position] |= (byte)((byte)m_options[i].Class << 5);
					buffer[position] |= (byte)(m_options[i].IsCopied ? 0x80 : 0x00);

					position++;
				
					if (m_options[i].OptionType == IpV4OptionNumber.EndOfOptions)
					{
						break;
					}
					else if (m_options[i].OptionType != IpV4OptionNumber.NoOperation)
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
			
			#region Calculate Checksum
			
			// copy the internet header into a temporary buffer
			tempBuffer = new byte[m_headerLength];
			Array.Copy (buffer, 0, tempBuffer, 0, m_headerLength);
		
			// calculate the checksum on the internet header
			m_checksum = PacketUtils.CalculateChecksum (tempBuffer);
			
			// copy the checksum into bytes 10 and 11 (in network byte order)
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short) m_checksum)), 0, buffer, 10, 2);
			
			#endregion
			
			// and finally copy in any data after that (there should be data otherwise it's just
			// an ip header, which is a bit pointless)
			if (m_data != null)
			{
				Array.Copy (m_data, 0, buffer, position, m_data.Length);
			}
		
			return buffer;
		}
		
		
		/// <summary>
		///		The fragment method will turn this IP packet into several using fragmentation.
		/// </summary>
		/// <param name="maximumTransmissionUnit">
		///		The maximum size of the data in each fragment. The maximum transmission unit must
		///		be a multiple of 8. For example 8, 16, 24, 32 etc.
		///	</param>
		/// <returns>
		///		This method returns an array of fragmented IP packets which can be sent over the
		///		network.
		/// </returns>
		///	<exception cref="ArgumentException">
		///		The MTU (maximum Transmission Unit) must be a multiple of 8 bytes
		///	</exception>
		public IpV4Packet[] Fragment (int maximumTransmissionUnit)
		{
		
			if (maximumTransmissionUnit % 8 != 0 && maximumTransmissionUnit > 0)
			{
				throw new ArgumentException ("The MTU (maximum Transmission Unit) must be a multiple of 8 bytes", "maximumTransmissionUnit");
			}
		
			// calculate how many fragments of the maximum transmission unit size we need
			int fragmentCount = m_data.Length / maximumTransmissionUnit;
			
			// and if the data doesn't exactly divide up into the maximum transmission unit
			// size then add a new fragment for the bit on the end
			if (m_data.Length % maximumTransmissionUnit != 0)
			{
				fragmentCount++;
			}
			
			// allocate space for the fragments
			IpV4Packet[] fragments = new IpV4Packet[fragmentCount];
			
			// build each fragment
			for (int i = 0; i < fragments.Length; i++)
			{
				fragments[i] = new IpV4Packet ();

				fragments[i].SourceAddress		= m_sourceAddress;
				fragments[i].DestinationAddress = m_destAddress;
				fragments[i].Identification		= m_id;
				fragments[i].HeaderLength		= 0x14;
				fragments[i].TimeToLive			= m_ttl;
				fragments[i].TransportProtocol	= m_protocol;
				fragments[i].TypeOfService		= m_tos;
				fragments[i].Version			= 0x04;
				
				#region Add Options
				
				// if there are options...
				if (m_options != null)
				{
					
					int optionsLength = 0;
				
					// if this is the first fragment, simply copy in all the options.
					if (i == 0)
					{
						fragments[i].Options = new IpV4Option[m_options.Length];
						m_options.CopyTo (fragments[i].Options, 0);
						
						// calculate the size in bytes of the options field
						for (int j = 0; j < m_options.Length; j++)
						{
							optionsLength += m_options[j].Length;
						}
					}
					
					// if not, copy in the ones which have the copy flag set
					else
					{
						for (int j = 0; j < m_options.Length; j++)
						{
							if (m_options[j].IsCopied)
							{
							
								if (fragments[i].Options == null)
								{
									fragments[i].Options = new IpV4Option[1];
								}
								else
								{
									// add this new option to the array
									IpV4Option[] tempOptions = new IpV4Option[fragments[i].Options.Length];
									Array.Copy (fragments[i].Options, 0, tempOptions, 0, fragments[i].Options.Length);
									
									fragments[i].Options = new IpV4Option[fragments[i].Options.Length + 1];
									Array.Copy (tempOptions, 0,  fragments[i].Options, 0, tempOptions.Length);
								}
								
								fragments[i].Options[fragments[i].Options.Length - 1] = m_options[j];
									
								// calculate the new size of the options
								optionsLength += m_options[j].Length;
							}
						}
					}
					
					fragments[i].HeaderLength += (ushort)optionsLength;
					
					// if we need padding to end the header on a 32 bit boundary
					if (optionsLength % 4 != 0)
					{
						// then add padding!
						fragments[i].Padding = new byte[4 - (optionsLength % 4)];
						
						fragments[i].HeaderLength += (ushort)(4 - (optionsLength % 4));
					}
				}
				
				
				#endregion
				
				// calculate the offset
				fragments[i].ControlFlags.Offset = (ushort)(i * maximumTransmissionUnit);
				
				// set up the control flags
				fragments[i].ControlFlags.MoreFragments = (i < fragments.Length - 1);
				fragments[i].ControlFlags.DontFragment = false;
			
				// copy the data into the buffer at the correct offset
				if ((i < fragments.Length - 1) || m_data.Length % maximumTransmissionUnit == 0)
				{
					fragments[i].Data = new byte[maximumTransmissionUnit];
					Array.Copy (m_data, fragments[i].ControlFlags.Offset, fragments[i].Data, 0, maximumTransmissionUnit);
				}
				else
				{
					fragments[i].Data = new byte[m_data.Length % maximumTransmissionUnit];
					Array.Copy (m_data, fragments[i].ControlFlags.Offset, fragments[i].Data, 0, m_data.Length % maximumTransmissionUnit);
				}
				
				// and finally the total size
				fragments[i].TotalLength = (ushort)(fragments[i].HeaderLength + fragments[i].Data.Length);
			}
			
			return fragments;
		}


		/// <summary>
		///		Return the string representation of this class
		/// </summary>
		public override string ToString ()
		{
			return m_sourceAddress.ToString() + " -> " + m_destAddress.ToString();
		}
		
		
		#endregion	
	}
	
	
	#endregion
}