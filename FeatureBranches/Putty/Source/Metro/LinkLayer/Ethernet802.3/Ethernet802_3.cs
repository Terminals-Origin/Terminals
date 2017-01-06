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

namespace Metro.LinkLayer.Ethernet802_3
{	
	#region Enumerations
	
	/// <summary>
	///		Network layer protocols as they are represented in the ethernet
	///		header.
	/// </summary>
	public enum NetworkLayerProtocol : int
	{
		/// <summary>
		///		Address Resolution Protocol (for IP and CHAOS).
		/// </summary>
		ARP = 0x0608,
		
		/// <summary>
		///		AppleTalk Address Resolution Protocol.
		/// </summary>
		AARP = 0xf380,
		
		/// <summary>
		///		AppleTalk over Ethernet.
		/// </summary>
		EtherTalk = 0x9b80,
		
		/// <summary>
		///		Internet Protocol version 4.
		/// </summary>
		IP = 0x0008,
		
		/// <summary>
		///		Internet Protocol version 6.
		/// </summary>
		IPv6 = 0xDD86,
		
		/// <summary>
		///		Reverse Address Resolution Protocol
		/// </summary>
		RARP = 0x3580
	}	


	#endregion
	
	#region Classes
	
	/// <summary>
	///		The following is a class for using the Ethernet Frame Format described in 
	///		the IEEE 802.3 Specification. The 802.3 Specification defines a 14 byte 
	///		Data Link Header followed by a Logical Link Control Header that is defined 
	///		by the 802.2 Specification.
	/// </summary>
	public class Ethernet802_3
	{
		/******************************************************\
		*                                                      *
		*	       1               2               3           *
		*    0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7   *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  *
		*   |            Desintation MAC Address            |  *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  *
		*   |                                               |  *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  *
		*   |              Source MAC Address               |  *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  *
		*   |                                               |  *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+  *
		*   |             Type              |                  *
		*   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+                  *
		*                                                      *
		\******************************************************/
	
		#region Private Fields
		
		/// <summary>
		///		The source MAC address. The Source address specifies from which 
		///		adapter the message originated.
		/// </summary>
		private MACAddress m_sourceMACAddress = new MACAddress (new byte[6] {0,0,0,0,0,0});
		
		/// <summary>
		///		The destination MAC address. The Destination address specifies to 
		///		which adapter the data frame is being sent. A Destination Address of 
		///		all ones specifies a Broadcast Message that is read in
		/// </summary>
		private MACAddress m_destinationMACAddress = new MACAddress (new byte[6] {0,0,0,0,0,0});
		
		/// <summary>
		///		The network layer protocol.
		/// </summary>
		private NetworkLayerProtocol m_networkProtocol = NetworkLayerProtocol.IP;
		
		/// <summary>
		///		Data.
		/// </summary>
		private byte[] m_data;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The source MAC address. The Source address specifies from which 
		///		adapter the message originated.
		/// </summary>
		public MACAddress SourceMACAddress
		{
			get
			{
				return m_sourceMACAddress;
			}
			set
			{
				m_sourceMACAddress = value;
			}
		}
		
		
		/// <summary>
		///		The destination MAC address. The Destination address specifies to 
		///		which adapter the data frame is being sent. A Destination Address of 
		///		all ones specifies a Broadcast Message that is read in
		/// </summary>
		public MACAddress DestinationMACAddress
		{
			get
			{
				return m_destinationMACAddress;
			}
			set
			{
				m_destinationMACAddress = value;
			}
		}
		
		
		/// <summary>
		///		The network layer protocol.
		/// </summary>
		public NetworkLayerProtocol NetworkProtocol
		{
			get
			{
				return m_networkProtocol;
			}
			set
			{
				m_networkProtocol = value;
			}
		}
		
		
		/// <summary>
		///		The data above the ethernet layer.
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
		///		Create a new ethernet 802.3 packet.
		/// </summary>
		public Ethernet802_3 ()
		{
		}
		
		
		/// <summary>
		///		Create a new ethernet 802.3 packet.
		/// </summary>
		/// <param name="packet">
		///		The ethernet packet to parse.
		///	</param>
		public Ethernet802_3 (byte[] packet)
		{
			// copy out the fields
			Array.Copy (packet, 0, m_destinationMACAddress.Address, 0, 6);
			Array.Copy (packet, 6, m_sourceMACAddress.Address, 0, 6);
			m_networkProtocol = (NetworkLayerProtocol)BitConverter.ToInt16 (packet, 12);
			
			if (packet.Length > 14)
			{
				m_data = new byte[packet.Length - 14];
				Array.Copy (packet, 14, m_data, 0, m_data.Length);
			}
		}
				
				
		/// <summary>
		///		Serialize the packet in to a byte array suitable for transmitting over
		///		the network.
		/// </summary>
		/// <returns>
		///		A byte array containing the fields.
		///	</returns>
		public byte[] Serialize ()
		{
			byte[] packet = new byte[14 + m_data.Length];
			
			// copy in the fields
			Array.Copy (m_destinationMACAddress.Address, 0, packet, 0, 6);
			Array.Copy (m_sourceMACAddress.Address, 0, packet, 6, 6);
			Array.Copy (BitConverter.GetBytes((short)m_networkProtocol), 0, packet, 12, 2);
			
			if (m_data != null)
			{
				Array.Copy (m_data, 0, packet, 14, m_data.Length);
			}
			
			return packet;
		}
	
	
		#endregion
	}
	
	
	#endregion
}