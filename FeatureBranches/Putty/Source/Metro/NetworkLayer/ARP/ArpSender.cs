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
	#region Classes
	
	/// <summary>
	///		The ArpSender class provides methods for sending ARP and RARP requests in order
	///		to resolve an IP address to a physical address (using ARP) and vice versa (using
	///		RARP).
	/// </summary>
	public class ArpSender
	{
		#region Private Fields
	
		/// <summary>
		///		Create a new NDIS protocol driver interface class instance.
		/// </summary>
		private NdisProtocolDriverInterface m_driver;
	
		/// <summary>
		///		Whether or not the class has been disposed.
		/// </summary>
		private bool m_disposed = false;
		
		/// <summary>
		///		Whether or not a query is being run.
		/// </summary>
		private bool m_querying = false;
	
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Whether or not the class has been disposed yet,
		/// </summary>
		public bool Disposed
		{
			get
			{
				return m_disposed;
			}
		}

		
		/// <summary>
		///		Whether or not there is a query already running.
		/// </summary>
		public bool QueryPending
		{
			get
			{
				return m_querying;	
			}
		}
		

		#endregion
	
		#region Public Methods
	
		/// <summary>
		///		Create a new arp sender class.
		/// </summary>
		/// <param name="driver">
		///		The NDIS protocol driver to use. The driver should already be
		///		started up and bound to a device and be ready for use.
		///	</param>
		/// <exception cref="Exception">
		///		If the driver fails to open, then an exception is raised.
		///	</exception>
		public ArpSender (NdisProtocolDriverInterface driver)
		{
			m_driver = driver;
		}
				
		
		/// <summary>
		///		Resolve a physical address to an IP address.
		/// </summary>
		/// <param name="address">
		///		The physical address to resolve.
		///	</param>
		/// <returns>
		///		Returns the IP address belonging to the physical address.
		///	</returns>
		/// <exception cref="ObjectDisposedException">
		///		If the object has already been disposed then an ObjectDisposedException
		///		will be thrown
		///	</exception>
		/// <exception cref="Exception">
		///		If the driver failed to start or was not bound, an exception will be thrown.
		///	</exception>
		public IPAddress ResolveMACAddress (MACAddress address)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString());
			}
			
			if (!m_driver.DriverStarted)
			{
				throw new Exception ("The driver has not been started.");
			}
			
			if (!m_driver.DriverBound)
			{
				throw new Exception ("The driver has not yet been bound to a device.");
			}

			Ethernet802_3 ethernet = new Ethernet802_3 ();
			
			// construct the ethernet header
			ethernet.SourceMACAddress		= m_driver.BoundAdapter.MediaAccessControlAddress;
			ethernet.DestinationMACAddress	= MACAddress.BroadcastAddress;
			ethernet.NetworkProtocol		= NetworkLayerProtocol.ARP;
			
			ArpPacket arp = new ArpPacket ();
			
			// construct the ARP header
			arp.Type					= ArpOpcode.ReverseRequest;
			arp.Protocol				= NetworkLayerProtocol.IP;
			arp.MediaType				= MediaType.Ethernet;
			arp.SourceMACAddress		= ethernet.SourceMACAddress;
			arp.SourceIPAddress			= m_driver.BoundAdapter.Interfaces[0].Address;
			arp.DestinationMACAddress	= address;
			
			// serialize and send the packet
			ethernet.Data = arp.Serialize ();
			m_driver.SendPacket (ethernet.Serialize ());
	
			m_querying = true;
	
			// wait for the reply
			while (m_querying)
			{
				byte[] packet = m_driver.RecievePacket ();
				
				Ethernet802_3 ethReply = new Ethernet802_3 (packet);
				
				// if this is an ARP packet
				if (ethReply.NetworkProtocol == NetworkLayerProtocol.ARP)
				{
					ArpPacket arpReply = new ArpPacket (ethReply.Data);
					
					// if this is an ARP reply
					if (arpReply.Type == ArpOpcode.Reply)
					{
						// if the address matches the one we requested
						if (arpReply.DestinationIPAddress.Equals (address))
						{
							// return the IP address
							return arpReply.DestinationIPAddress;	
						}
					}
				}
			}
			
			return IPAddress.Any;
		}
		
		
		/// <summary>
		///		Resolve an IP address to a physical address.
		/// </summary>
		/// <param name="address">
		///		The IP address to resolve.
		///	</param>
		/// <returns>
		///		Returns the physical address belonging to the IP address.
		///	</returns>
		/// <exception cref="ObjectDisposedException">
		///		If the object has already been disposed then an ObjectDisposedException
		///		will be thrown
		///	</exception>
		/// <exception cref="Exception">
		///		If the driver failed to start or was not bound, an exception will be thrown.
		///	</exception>
		public MACAddress ResolveIPAddress (IPAddress address)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString());
			}
			
			if (!m_driver.DriverStarted)
			{
				throw new Exception ("The driver has not been started.");
			}
			
			if (!m_driver.DriverBound)
			{
				throw new Exception ("The driver has not yet been bound to a device.");
			}

			Ethernet802_3 ethernet = new Ethernet802_3 ();
			
			// construct the ethernet header
			ethernet.SourceMACAddress		= m_driver.BoundAdapter.MediaAccessControlAddress;
			ethernet.DestinationMACAddress	= MACAddress.BroadcastAddress;
			ethernet.NetworkProtocol		= NetworkLayerProtocol.ARP;
			
			ArpPacket arp = new ArpPacket ();
			
			// construct the ARP header
			arp.Type					= ArpOpcode.Request;
			arp.Protocol				= NetworkLayerProtocol.IP;
			arp.MediaType				= MediaType.Ethernet;
			arp.SourceMACAddress		= ethernet.SourceMACAddress;
			arp.SourceIPAddress			= m_driver.BoundAdapter.Interfaces[0].Address;
			arp.DestinationIPAddress	= address;
			
			// serialize and send the packet
			ethernet.Data = arp.Serialize ();
			m_driver.SendPacket (ethernet.Serialize ());
	
			m_querying = true;
	
			// wait for the reply
			while (m_querying)
			{
				byte[] packet = m_driver.RecievePacket ();
				
				Ethernet802_3 ethReply = new Ethernet802_3 (packet);
				
				// if this is an ARP packet
				if (ethReply.NetworkProtocol == NetworkLayerProtocol.ARP)
				{
					ArpPacket arpReply = new ArpPacket (ethReply.Data);
					
					// if this is an ARP reply
					if (arpReply.Type == ArpOpcode.Reply)
					{
						// if the address matches the one we requested
						if (arpReply.SourceIPAddress.Equals (address))
						{
							// return the MAC address
							return arpReply.SourceMACAddress;	
						}
					}
				}
			}
			
			return MACAddress.BroadcastAddress;
		}
		

		/// <summary>
		///		Cancel any pending queries.
		/// </summary>
		public void CancelQuery ()
		{	
			m_querying = false;
		}
	
	
		#endregion
	}
	
	
	#endregion
}