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
	#region Enumerations

	/// <summary>
	///		Specifies one of 16 levels of security (eight of which are
	///		reserved for future use).
	/// </summary>
	public enum IpV4SecurityLevelType : ushort
	{

		/// <summary>
		///		Unclassified - 00000000 00000000
		/// </summary>
		Unclassified = 0,
	
		/// <summary>
		///		Confidential - 11110001 00110101
		/// </summary>
		Confidential = 0xf135, 
	
		/// <summary>
		///		EFTO - 01111000 10011010
		/// </summary>
		EFTO = 0x789a,
	
		/// <summary>
		///		MMMM - 10111100 01001101
		/// </summary>
		MMMM = 0xbc4d, 

		/// <summary>
		///		PROG - 01011110 00100110
		/// </summary>
		PROG = 0x5e26,

		/// <summary>
		///		Restricted - 10101111 00010011
		/// </summary>
		Restricted = 0xaf13,
		
		/// <summary>
		///		Secret - 11010111 10001000
		/// </summary>
		Secret = 0xd788, 

		/// <summary>
		///		Top Secret - 01101011 11000101
		/// </summary>
		TopSecret = 0x6bc5,

		/// <summary>
		///		(Reserved for future use) - 00110101 11100010
		/// </summary>
		Reserved1 = 0x35e2,
		
		/// <summary>
		///		(Reserved for future use) - 10011010 11110001
		/// </summary>
		Reserved2 = 0x9af1,
		
		/// <summary>
		///		(Reserved for future use) - 01001101 01111000
		/// </summary>
		Reserved3 = 0x4d78,
		
		/// <summary>
		///		(Reserved for future use) - 00100100 10111101
		/// </summary>
		Reserved4 = 0x24bd,
		
		/// <summary>
		///		(Reserved for future use) - 00010011 01011110
		/// </summary>
		Reserved5 = 0x135e,
		
		/// <summary>
		///		(Reserved for future use) - 10001001 10101111
		/// </summary>
		Reserved6 = 0x89af,
		
		/// <summary>
		///		(Reserved for future use) - 11000100 11010110
		/// </summary>
		Reserved7 = 0xc4d6,
		
		/// <summary>
		///		(Reserved for future use) - 11100010 01101011
		/// </summary>
		Reserved8 = 0xe26B     
	}
	

	/// <summary>
	///		Timestamp option types.
	/// </summary>
	public enum IpV4TimestampType : int
	{
		
		/// <summary>
		///		Time stamps only, stored in consecutive 32-bit words.
		/// </summary>
		TimestampsOnly,
		
		/// <summary>
		///		Each timestamp is preceded with internet address of the
		///		registering entity.
		/// </summary>
		TimestampAndAddress,
		
		/// <summary>
		///		The internet address fields are prespecified.  An IP
		///		module only registers its timestamp if it matches its own
		///		address with the next specified internet address.
		/// </summary>
		PrespecifiedAddresses
	}


	#endregion

	#region Classes

	/// <summary>
	///		This option provides a way for hosts to send security,
	///		compartmentation, handling restrictions, and TCC (closed user
	///		group) parameters.  The format for this option is as follows:
	/// </summary>
	public class IpV4SecurityOption
	{
		/************************************************************\
		*                                                            *
		*	+--------+--------+---//---+---//---+---//---+---//---+  *
		*   |10000010|00001011|SSS  SSS|CCC  CCC|HHH  HHH|  TCC   |  *
		*   +--------+--------+---//---+---//---+---//---+---//---+  *
		*                                                            *
		*          Type=130 Length=11                                *
		*                                                            *
		\************************************************************/
		
		#region Private Fields
		
		/// <summary>
		///		Specifies one of 16 levels of security (eight of which are
		///		reserved for future use).
		/// </summary>
		private IpV4SecurityLevelType m_level = IpV4SecurityLevelType.Unclassified;

		/// <summary>
		///		An all zero value is used when the information transmitted is
        ///		not compartmented. Other values for the compartments field
        ///		may be obtained from the Defense Intelligence Agency.
		/// </summary>
		private ushort m_compartment = 0;
	
		/// <summary>
		///		The values for the control and release markings are
        ///		alphanumeric digraphs and are defined in the Defense
        ///		Intelligence Agency Manual DIAM 65-19, "Standard Security
        ///		Markings".
		/// </summary>
		private ushort m_handling = 0;
	
		/// <summary>
		///		Provides a means to segregate traffic and define controlled
		///		communities of interest among subscribers. The TCC values are
        ///		trigraphs, and are available from HQ DCA Code 530.
		/// </summary>
		private uint m_tcc = 0;
	
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Specifies one of 16 levels of security (eight of which are
		///		reserved for future use).
		/// </summary>
		public IpV4SecurityLevelType SecurityLevel
		{
			get
			{
				return m_level;
			}
			set
			{
				m_level = value;
			}
		}
		
		
		/// <summary>
		///		An all zero value is used when the information transmitted is
        ///		not compartmented.  Other values for the compartments field
        ///		may be obtained from the Defense Intelligence Agency.
		/// </summary>
		public ushort Compartment
		{
			get
			{
				return m_compartment;
			}
			set
			{
				m_compartment = value;
			}
		}
		
		
		/// <summary>
		///		The values for the control and release markings are
        ///		alphanumeric digraphs and are defined in the Defense
        ///		Intelligence Agency Manual DIAM 65-19, "Standard Security
        ///		Markings".
		/// </summary>
		public ushort HandlingRestrictions
		{
			get
			{
				return m_handling;
			}
			set
			{
				m_handling = value;
			}
		}
		
		
		/// <summary>
		///		Provides a means to segregate traffic and define controlled
		///		communities of interest among subscribers. The TCC values are
        ///		trigraphs, and are available from HQ DCA Code 530.
		/// </summary>
		public uint TransmissionControlCode
		{
			get
			{
				return m_tcc;
			}
			set
			{
				m_tcc = value;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new security option.
		/// </summary>
		public IpV4SecurityOption ()
		{
		}
		
		
		/// <summary>
		///		Create a new security option.
		/// </summary>
		/// <param name="option">
		///		The option to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		The option argument is null.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The option argument is not a security option.
		///	</exception>
		public IpV4SecurityOption (IpV4Option option)
		{
			if (option == null)
			{
				throw new ArgumentNullException ("option");
			}
		
			if (option.OptionType != IpV4OptionNumber.Security)
			{
				throw new ArgumentException ("The option passed was not compatible with this class. This class accepts security options only", "option");
			}
			
			// extract the security level field
			m_level = (IpV4SecurityLevelType) BitConverter.ToUInt16 (option.Data, 0);
			
			// extract the compartment field
			m_compartment = BitConverter.ToUInt16 (option.Data, 2);
			
			// extract the handling restrictions field
			m_handling = BitConverter.ToUInt16 (option.Data, 4);
			
			// extract the 24 bits of the transmission control code
			
			m_tcc = (uint)(option.Data[6] << 0x10);
			m_tcc |= (uint)(option.Data[7] << 0x08);
			m_tcc |= option.Data[8];
			
		}
		
		
		/// <summary>
		///		Serlialize this routing option into something which
		///		can be passed to an IpV4Packet class.
		/// </summary>
		/// <returns>	
		///		An IpV4Option class which can be passed to an IpV4Packet.
		///	</returns>
		public IpV4Option Serialize ()
		{
			IpV4Option newOption = new IpV4Option ();
			
			// fill in basic fields
			newOption.Class			= IpV4OptionClass.Control;
			newOption.OptionType	= IpV4OptionNumber.Security;
			newOption.IsCopied		= true;
			newOption.Length		= 11;
			newOption.Data			= new byte[9];
			
			// fill in the security bytes
			newOption.Data[0] =		(byte)((int)m_level & 0x00ff);
			newOption.Data[1] =		(byte)(((int)m_level & 0xff00) >> 8);

			// fill in the compartment bytes
			newOption.Data[2] =		(byte)(m_compartment & 0x00ff);
			newOption.Data[3] =		(byte)((m_compartment & 0xff00) >> 8);
			
			// fill in the handling bytes
			newOption.Data[4] =		(byte)(m_handling & 0x00ff);
			newOption.Data[5] =		(byte)((m_handling & 0xff00) >> 8);
			
			// fill in the transmission control code bytes
			newOption.Data[6] =		(byte)((m_tcc & 0xff0000) >> 16);
			newOption.Data[7] =		(byte)((m_tcc & 0x00ff00) >> 8);
			newOption.Data[8] =		(byte)(m_tcc & 0x0000ff);
			
			return newOption;
		}
		
		
		#endregion
	}
	
	
	/// <summary>
	///		This option provides routing and route recording.
	/// </summary>
	public class IpV4RoutingOption
	{
		/*****************************************************\
		*                                                     *
		*	+--------+--------+--------+---------//--------+  *
        *   |10001001| length | pointer|     route data    |  *
        *   +--------+--------+--------+---------//--------+  *
        *                                                     * 
        *      Type=137                                       *
		*                                                     *
		\*****************************************************/
		
		#region Private Fields
		
		/// <summary>
		///		The third octet is the pointer into the route data
        ///		indicating the octet which begins the next source address to be
        ///		processed. The pointer is relative to this option, and the
        ///		smallest legal value for the pointer is 4.
        ///		If the pointer is greater than the length, the source route is empty 
        ///		(and the recorded route full) and the routing is to be based on the
        ///		destination address field.
		/// </summary>
		private byte m_pointer = 0;
		
		/// <summary>
		///		A route data is composed of a series of internet addresses.
        ///		Each internet address is 32 bits or 4 octets.
		/// </summary>
		private IPAddress[] m_routeData;
		
		#endregion 
		
		#region Public Fields
		
		/// <summary>
		///		The third octet is the pointer into the route data
        ///		indicating the octet which begins the next source address to be
        ///		processed. The pointer is relative to this option, and the
        ///		smallest legal value for the pointer is 4.
        ///		If the pointer is greater than the length, the source route is empty 
        ///		(and the recorded route full) and the routing is to be based on the
        ///		destination address field.
		/// </summary>
		public byte Pointer
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
		
		
		/// <summary>
		///		A route data is composed of a series of internet addresses.
        ///		Each internet address is 32 bits or 4 octets.
		/// </summary>
		public IPAddress[] Route
		{
			get
			{
				return m_routeData;
			}
			set
			{
				m_routeData = value;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new routing option.
		/// </summary>
		public IpV4RoutingOption ()
		{
		}
		
		
		/// <summary>
		///		Create a new routing option.
		/// </summary>
		/// <param name="option">
		///		The option to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		The option argument is null.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The option argument is not a routing option.
		///	</exception>
		public IpV4RoutingOption (IpV4Option option)
		{
			if (option == null)
			{
				throw new ArgumentNullException ("option");
			}
		
			if (option.OptionType != IpV4OptionNumber.LooseSourceRouting &&
				option.OptionType != IpV4OptionNumber.StrictSourceRouting &&
				option.OptionType != IpV4OptionNumber.RecordRoute)
			{
				throw new ArgumentException ("The option passed was not compatible with this class. This class accepts routing options only", "option");
			}
			
			// read the pointer
			m_pointer = (byte)option.Data[0];
			
			// get the number of addresses in the route
			// and allocate enough space
			int routeSize = (option.Length - 3) / 4;
			m_routeData = new IPAddress[routeSize];
			
			// extract each address from the data
			for (int i = 0; i < m_routeData.Length; i++)
			{
				uint address = BitConverter.ToUInt32 (option.Data, i * 4 + 1);
				m_routeData[i] = new IPAddress ((long)address);
			}
		}
		
		
		/// <summary>
		///		Serlialize this routing option into something which
		///		can be passed to an IpV4Packet class.
		/// </summary>
		/// <param name="optionType">
		///		There are 3 types of routing option: loose source, strict source and record.
		///	</param>
		/// <returns>	
		///		An IpV4Option class which can be passed to an IpV4Packet.
		///	</returns>
		///	<exception cref="ArgumentException">
		///		The option type is not a routing option.
		///	</exception>
		public IpV4Option Serialize (IpV4OptionNumber optionType)
		{
			IpV4Option newOption = new IpV4Option ();
			
			if (optionType != IpV4OptionNumber.LooseSourceRouting &&
				optionType != IpV4OptionNumber.StrictSourceRouting &&
				optionType != IpV4OptionNumber.RecordRoute)
			{
				throw new ArgumentException ("The option type must be a routing option (strict source routing, loose source routing or record route", "option");
			}
			
			// fill in basic fields
			newOption.Class			= IpV4OptionClass.Control;
			newOption.OptionType	= optionType;
			newOption.IsCopied		= true;
			newOption.Length		= (m_routeData.Length * 4) + 3;
			newOption.Data			= new byte[(m_routeData.Length * 4) + 1];
			
			// fill in the pointer
			newOption.Data [0] = (byte)m_pointer;
			
			// fill in the route data
			for (int i = 0; i < m_routeData.Length; i++)
			{
				Array.Copy (m_routeData[i].GetAddressBytes (), 0, newOption.Data, (i * 4) + 1, 4);
			}
			
			return newOption;
		}
		
		
		#endregion
	}
	
	
	/// <summary>
	///		This option provides a way for the 16-bit SATNET stream
    ///		identifier to be carried through networks that do not support
    ///		the stream concept.
	/// </summary>
	public class IpV4StreamIdentifierOption
	{
		/******************************************\
		*                                          *
		*   +--------+--------+--------+--------+  *
        *   |10001000|00000010|    Stream ID    |  *
        *   +--------+--------+--------+--------+  *
        *                                          * 
        *      Type=136 Length=4                   *
		*                                          *
		\******************************************/
		
		#region Private Fields
		
		/// <summary>
		///		Used for generating random numbers.
		/// </summary>
		private Random m_random = new Random ();
		
		/// <summary>
		///		A 16-bit SATNET stream identifier.
		/// </summary>
		private ushort m_id;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		A 16-bit SATNET stream identifier.
		/// </summary>
		public ushort StreamIdentifier
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
	
	
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new stream identifier option.
		/// </summary>
		public IpV4StreamIdentifierOption ()
		{
			m_id = (ushort)m_random.Next(ushort.MaxValue);
		}
		
		
		/// <summary>
		///		Create a new stream identifier option.
		/// </summary>
		/// <param name="option">
		///		The option to parse.
		///	</param>
		public IpV4StreamIdentifierOption (IpV4Option option)
		{
			if (option == null)
			{
				throw new ArgumentNullException ("option");
			}
		
			if (option.OptionType != IpV4OptionNumber.StreamId)
			{
				throw new ArgumentException ("The option passed was not compatible with this class. This class accepts stream id options only", "option");
			}
			
			// extract the stream id
			m_id = BitConverter.ToUInt16 (option.Data, 0);
		}
		
		
		/// <summary>
		///		Serlialize this routing option into something which
		///		can be passed to an IpV4Packet class.
		/// </summary>
		/// <returns>	
		///		An IpV4Option class which can be passed to an IpV4Packet.
		///	</returns>
		public IpV4Option Serialize ()
		{
			IpV4Option newOption = new IpV4Option ();
			
			// fill in basic fields
			newOption.Class			= IpV4OptionClass.Control;
			newOption.OptionType	= IpV4OptionNumber.StreamId;
			newOption.IsCopied		= true;
			newOption.Length		= 4;
			newOption.Data			= new byte[2];
			
			// copy in the stream id
			newOption.Data = BitConverter.GetBytes (m_id);
			
			return newOption;
		}
		
		
		#endregion
	}
	
	
	/// <summary>
	///		Internet Timestamp.
	/// </summary>
	public class IpV4TimeStampOption
	{
		/******************************************\
		*                                          *
		*   +--------+--------+--------+--------+  *
		*	|01000100| length | pointer|oflw|flg|  *
		*	+--------+--------+--------+--------+  *
		*	|         internet address          |  *
		*	+--------+--------+--------+--------+  *
		*	|             timestamp             |  *
		*	+--------+--------+--------+--------+  *
		*	|                 .                 |  *
		*					  .                    *
		*					  .                    *
        *                                          * 
        *      Type=68                             *
		*                                          *
		\******************************************/
		
		#region Private Fields
		
		/// <summary>
		///		The Pointer is the number of octets from the beginning of this
		///		option to the end of timestamps plus one (i.e., it points to the
		///		octet beginning the space for next timestamp). The smallest
		///		legal value is 5. The timestamp area is full when the pointer
		///		is greater than the length.
		/// </summary>
		private byte m_pointer = 0;
		
		/// <summary>
		///		The Overflow (oflw) [4 bits] is the number of IP modules that
        ///		cannot register timestamps due to lack of space.
		/// </summary>
		private byte m_overflow = 0;
		
		/// <summary>
		///		The type of the time stamp data.
		/// </summary>
		private IpV4TimestampType m_type = IpV4TimestampType.TimestampsOnly;
		
		/// <summary>
		///		The Timestamp is a right-justified, 32-bit timestamp in
		///		milliseconds since midnight UT. If the time is not available in
		///		milliseconds or cannot be provided with respect to midnight UT
		///		then any time may be inserted as a timestamp provided the high
		///		order bit of the timestamp field is set to one to indicate the
		///		use of a non-standard value.
		/// </summary>
		private uint[] m_timestamps;
		
		/// <summary>
		///		The address fields matching up to the time stamp fields
		/// </summary>
		private IPAddress[] m_addressstamps;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The Pointer is the number of octets from the beginning of this
		///		option to the end of timestamps plus one (i.e., it points to the
		///		octet beginning the space for next timestamp). The smallest
		///		legal value is 5. The timestamp area is full when the pointer
		///		is greater than the length.
		/// </summary>
		public byte Pointer
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
		
		
		/// <summary>
		///		The Overflow (oflw) [4 bits] is the number of IP modules that
        ///		cannot register timestamps due to lack of space.
		/// </summary>
		public byte Overflow
		{
			get
			{
				return m_overflow;
			}
			set
			{
				if (value > 16) 
				{
					throw new OverflowException ("The maximum value of the overflow field is 15");
				}
				
				m_overflow = value;
			}	
		}
		
		
		/// <summary>
		///		The type of the time stamp data.
		/// </summary>
		public IpV4TimestampType OptionType
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
		///		The Timestamp is a right-justified, 32-bit timestamp in
		///		milliseconds since midnight UT. If the time is not available in
		///		milliseconds or cannot be provided with respect to midnight UT
		///		then any time may be inserted as a timestamp provided the high
		///		order bit of the timestamp field is set to one to indicate the
		///		use of a non-standard value.
		/// </summary>
		public uint[] Timestamps
		{
			get
			{
				return m_timestamps;
			}
			set
			{
				m_timestamps = value;
			}	
		}
		
		
		/// <summary>
		///		The address fields matching up to the time stamp fields
		/// </summary>
		public IPAddress[] Addressstamps
		{
			get
			{
				return m_addressstamps;
			}
			set
			{
				m_addressstamps = value;
			}	
		}
		
		
		#endregion
	
		#region Public Methods
		
		/// <summary>
		///		Create a new time stamp option.
		/// </summary>
		public IpV4TimeStampOption ()
		{
		}
		
		
		/// <summary>
		///		Create a new time stamp option.
		/// </summary>
		/// <param name="option">
		///		The option to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		The option argument is null.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The option argument is not a timestamp option.
		///	</exception>
		public IpV4TimeStampOption (IpV4Option option)
		{
			if (option == null)
			{
				throw new ArgumentNullException ("option");
			}
		
			if (option.OptionType != IpV4OptionNumber.InternetTimestamp)
			{
				throw new ArgumentException ("The option passed was not compatible with this class. This class accepts timestamp options only", "option");
			}
			
			// extract the pointer
			m_pointer = (byte)option.Data[0];
			
			// extract the overflow field
			m_overflow = (byte)((option.Data[1] & 0xf0) >> 4);
			
			// extract the type
			m_type = (IpV4TimestampType)(option.Data[1] & 0xf);

			// calculate how many fields there are and allocate space
			int numberStamps = (option.Data.Length - 2) / 4;
			
			if (m_type == IpV4TimestampType.TimestampAndAddress)
			{
				numberStamps /= 2;
				m_addressstamps = new IPAddress[numberStamps];
			}
			
			m_timestamps = new uint[numberStamps];
			
			// extract the time stamps and addresses
			for (int i = 0; i < numberStamps; i++)
			{
				if (m_type == IpV4TimestampType.TimestampAndAddress)
				{
					uint address = BitConverter.ToUInt32 (option.Data, (i * 8) + 2);
					m_addressstamps[i] = new IPAddress ((long)address);
					
					m_timestamps[i] = BitConverter.ToUInt32 (option.Data, (i * 8) + 6);
				}
				else
				{
					m_timestamps[i] = BitConverter.ToUInt32 (option.Data, (i * 4) + 2);
				}
			}
		}
		
		
		/// <summary>
		///		Serlialize this routing option into something which
		///		can be passed to an IpV4Packet class.
		/// </summary>
		/// <returns>	
		///		An IpV4Option class which can be passed to an IpV4Packet.
		///	</returns>
		public IpV4Option Serialize ()
		{
			IpV4Option newOption = new IpV4Option ();
			
			// fill in basic fields
			newOption.Class			= IpV4OptionClass.DebuggingAndMeasurement;
			newOption.OptionType	= IpV4OptionNumber.InternetTimestamp;
			newOption.IsCopied		= false;
			newOption.Length		= (m_timestamps.Length * (m_type == IpV4TimestampType.TimestampAndAddress ? 8 : 4)) + 4;
			newOption.Data			= new byte[newOption.Length - 2];
				
			// first copy in the pointer
			newOption.Data[0] = (byte)m_pointer;
			
			// then the overflow and type
			newOption.Data[1] =  (byte)(m_overflow << 4);
			newOption.Data[1] |= (byte)m_type;
			
			// loop through each field and add that
			for (int i = 0; i < m_timestamps.Length; i++)
			{
				if (m_type == IpV4TimestampType.TimestampAndAddress)
				{
				
					Array.Copy (m_addressstamps[i].GetAddressBytes (), 0, newOption.Data, (i * 8) + 2, 4);
					Array.Copy (BitConverter.GetBytes (m_timestamps[i]), 0, newOption.Data, (i * 8) + 6, 4);
				}	
				else
				{
					Array.Copy (BitConverter.GetBytes (m_timestamps[i]), 0, newOption.Data, (i * 4) + 2, 4);
				}
			}
			
			return newOption;
		}
		
		
		#endregion
	}
	
	
	#endregion
}