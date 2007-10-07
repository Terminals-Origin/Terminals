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
using Metro.LinkLayer;
using Metro.LinkLayer.Ethernet802_3;

namespace Metro.NetworkLayer.ARP
{
	#region Enumerations
	
	/// <summary>
	///		The type of hardware.
	/// </summary>
	public enum MediaType : int
	{
		/// <summary>
		///		Ethernet.
		/// </summary>
		Ethernet = 1,
		
		/// <summary>
		///		Experimental Ethernet.
		/// </summary>
		ExperimentalEthernet,
		
		/// <summary>
		///		Amateur Radio AX.25.
		/// </summary>
		AmateurRadio,
		
		/// <summary>
		///		Proteon ProNET Token Ring.
		/// </summary>
		TokenRing,
		
		/// <summary>
		///		Chaos.
		/// </summary>
		Chaos,	
		
		/// <summary>
		///		IEEE 802
		/// </summary>
		IEEE802,
		
		/// <summary>
		///		ARCNET
		/// </summary>
		ARCNET,
		
		/// <summary>
		///		Hyper Channel.
		/// </summary>
		Hyperchannel,
		
		/// <summary>
		///		Lanstar
		/// </summary>
		Lanstar,
		
		/// <summary>
		///		Autonet Short Address.
		/// </summary>
		AutonetShortAddress,
		
		/// <summary>
		///			LocalTalk.
		/// </summary>
		LocalTalk,
		
		/// <summary>
		///		LocalNet (IBM PCNet or SYTEK LocalNET)
		/// </summary>
		LocalNet,
		
		/// <summary>
		///		Ultra Link.
		/// </summary>
		UltraLink,
		
		/// <summary>
		///		SMDS
		/// </summary>
		SMDS,	
		
		/// <summary>
		///		Frame Relay.
		/// </summary>
		FrameRelay,
		
		/// <summary>
		///		ATM, Asynchronous Transmission Mode.
		/// </summary>
		AsynchronousTransmissionMode,	
		
		/// <summary>
		///		HDLC.
		/// </summary>
		HDLC,
		
		/// <summary>
		///		Fibre Channel.
		/// </summary>
		FibreChannel,
		
		
		/// <summary>
		///		ATM, Asynchronous Transmission Mode.
		/// </summary>
		AsynchronousTransmissionMode2,
		
		/// <summary>
		///		Serial Line
		/// </summary>
		SerialLine,
		
		
		/// <summary>
		///		ATM, Asynchronous Transmission Mode.
		/// </summary>
		AsynchronousTransmissionMode3,	
		
		/// <summary>
		///		MIL-STD-188-220.
		/// </summary>
		MIL_STD_188_220,
			
		/// <summary>
		///		Metricom.
		/// </summary>
		Metricom,
		
		/// <summary>
		///		IEEE 1394.1995.
		/// </summary>
		IEEE_1394_1995,
		
		/// <summary>
		///		MAPOS.
		/// </summary>
		MAPOS,
		
		/// <summary>
		///		Twinaxial.
		/// </summary>
		Twinaxial,	
		
		/// <summary>
		///		EUI-64.
		/// </summary>
		EUI_64,
		
		/// <summary>
		///		HIPARP.
		/// </summary>
		HIPARP,	
		
		/// <summary>
		///		IP and ARP over ISO 7816-3.
		/// </summary>
		ISO_7816_3,
		
		/// <summary>
		///		ARPSec.
		/// </summary>
		ARPSec,
		
		/// <summary>
		///		IPsec tunnel
		/// </summary>
		IPsecTunnel,
		
		/// <summary>
		///		Infiniband.
		/// </summary>
		Infiniband
	}


	/// <summary>
	///		The ARP opcode.
	/// </summary>
	public enum ArpOpcode : int
	{
		/// <summary>
		///		RFC 826
		/// </summary>
		Request = 1,
		
		/// <summary>
		///		RFC 826, RFC 1868
		/// </summary>
		Reply,
		
		/// <summary>
		///			RFC 903
		/// </summary>
		ReverseRequest,
		
		/// <summary>
		///			RFC 903
		/// </summary>
		ReverseReply
	}


	#endregion
	
	#region Classes
	
	/// <summary>
	///		ARP is used to translate protocol addresses to hardware interface addresses.
	/// </summary>
	public class ArpPacket
	{
		#region Private Fields
		
		/// <summary>
		///		Source MAC address.
		/// </summary>
		private MACAddress m_sourceMac = MACAddress.BroadcastAddress;
		
		/// <summary>
		///		Destination MAC address.
		/// </summary>
		private MACAddress m_destMac = MACAddress.BroadcastAddress;
		
		/// <summary>
		///		Source IP address.
		/// </summary>
		private IPAddress m_sourceIP = IPAddress.Any;
		
		/// <summary>
		///		Destination IP address.
		/// </summary>
		private IPAddress m_destIP = IPAddress.Any;
		
		/// <summary>
		///		Protocol type.
		/// </summary>
		private NetworkLayerProtocol m_protocol = NetworkLayerProtocol.IP;
		
		/// <summary>
		///		Hardware type.
		/// </summary>
		private MediaType m_hardware = MediaType.Ethernet;
		
		/// <summary>
		///		ARP Type.
		/// </summary>
		private ArpOpcode m_type = ArpOpcode.Request;
		
		#endregion
	
		#region Public Fields
		
		/// <summary>
		///		Source MAC address.
		/// </summary>
		public MACAddress SourceMACAddress
		{
			get
			{
				return m_sourceMac;
			}
			set
			{
				m_sourceMac = value;
			}
		}
		
		
		/// <summary>
		///		Destination MAC address.
		/// </summary>
		public MACAddress DestinationMACAddress
				{
			get
			{
				return m_destMac;
			}
			set
			{
				m_destMac = value;
			}
		}
		
		
		/// <summary>
		///		Source IP address.
		/// </summary>
		public IPAddress SourceIPAddress
		{
			get
			{
				return m_sourceIP;
			}
			set
			{
				m_sourceIP = value;
			}
		}
			
		
		/// <summary>
		///		Destination IP address.
		/// </summary>
		public IPAddress DestinationIPAddress
		{
			get
			{
				return m_destIP;
			}
			set
			{
				m_destIP = value;
			}
		}
		
		
		/// <summary>
		///		Media type.
		/// </summary>
		public MediaType MediaType
		{
			get
			{
				return m_hardware;
			}
			set
			{
				m_hardware = value;
			}
		}
		
		
		/// <summary>
		///		Protocol.
		/// </summary>
		public NetworkLayerProtocol Protocol
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
		///		ARP Type.
		/// </summary>
		public ArpOpcode Type
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


		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new ARP packet.
		/// </summary>
		public ArpPacket ()
		{
		}
		
		
		/// <summary>
		///		Create a new ARP packet. 
		/// </summary>
		/// <param name="data">
		///		The data representing the ARP packet.
		///	</param>
		public ArpPacket (byte[] data)
		{
			int position = 0;
		
			m_hardware = (MediaType)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			m_protocol = (NetworkLayerProtocol)BitConverter.ToInt16 (data, position);
			position += 2;
			
			int macLength = data[position];
			position ++;
			
			int ipLength = data[position];
			position++;
			
			m_type = (ArpOpcode)IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (data, position));
			position += 2;
			
			// copy out source MAC
			byte[] sourceMac	= new byte[macLength];
			Array.Copy (data, position, sourceMac, 0, macLength);
			m_sourceMac = new MACAddress (sourceMac);
			position += macLength;
			
			// copy out source IP
			byte[] sourceIP	= new byte[ipLength];
			Array.Copy (data, position, sourceIP, 0, ipLength);
			
			string sIP = string.Empty;
			
			for (int i = 0; i < ipLength; i++)
			{
				sIP = string.Concat(sIP, sourceIP[i], ".");
			}
			
			m_sourceIP = IPAddress.Parse(sIP.Substring (0, sIP.Length - 1));
			position += ipLength;

			// copy out destination MAC
			byte[] destMac		= new byte[macLength];
			Array.Copy (data, position, destMac, 0, macLength);
			m_destMac = new MACAddress (destMac);
			position += macLength;
			
			// copy out destination IP
			byte[] destIP	= new byte[ipLength];
			Array.Copy (data, position, destIP, 0, ipLength);
			
			sIP = string.Empty;
			
			for (int i = 0; i < ipLength; i++)
			{
				sIP = string.Concat(sIP, destIP[i], ".");
			}
			
			m_destIP = IPAddress.Parse(sIP.Substring (0, sIP.Length - 1));
			position += ipLength;
		}
		
		
		/// <summary>
		///		
		/// </summary>
		/// <returns>
		///	
		///	</returns>
		public byte[] Serialize ()
		{
			byte[] packet = new byte[8 + m_sourceIP.GetAddressBytes().Length * 2 + m_sourceMac.Address.Length * 2];
			int position = 0;
			
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_hardware)), 0, packet, position, 2);
			position += 2;
			
			Array.Copy (BitConverter.GetBytes ((int)m_protocol), 0, packet, position, 2);
			position += 2;
			
			packet[position] = (byte)m_sourceMac.Address.Length;
			position++;
			
			packet[position] =(byte) m_sourceIP.GetAddressBytes().Length;
			position++;
			
			Array.Copy (BitConverter.GetBytes (IPAddress.HostToNetworkOrder ((short)m_type)), 0, packet, position, 2);
			position += 2;
			
			Array.Copy (m_sourceMac.Address, 0, packet, position, m_sourceMac.Address.Length);
			position += m_sourceMac.Address.Length;
			
			Array.Copy (m_sourceIP.GetAddressBytes(), 0, packet, position, m_sourceIP.GetAddressBytes().Length);
			position += m_sourceIP.GetAddressBytes().Length;
			
			Array.Copy (m_destMac.Address, 0, packet, position, m_destMac.Address.Length);
			position += m_destMac.Address.Length;
			
			Array.Copy (m_destIP.GetAddressBytes(), 0, packet, position, m_destIP.GetAddressBytes().Length);
			position += m_destIP.GetAddressBytes().Length;
			
			return packet;
		}
		
		
		#endregion
	}
	
	#endregion
}