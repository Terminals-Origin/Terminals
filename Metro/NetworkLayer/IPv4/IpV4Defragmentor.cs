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
using System.Collections;

namespace Metro.NetworkLayer.IpV4
{
	#region Classes

	/// <summary>
	///		Packets are passed to this class, reassembled if they are fragmented,
	///		and then passed on via the IpV4PacketArrived event.
	/// </summary>
	public class IpV4Defragmentor
	{
		#region Events
	
		/// <summary>
		///		The handler for when a new complete packet has arrived. This does not fire when
		///		fragmented packets arrive. It will fire once all the fragments have arrived and have
		///		been assembled back into a whole packet.
		/// </summary>
		public event IpV4PacketArrivedHandler IpV4PacketArrived;
	
		#endregion
		
		#region Classes
	
		/// <summary>
		///		This class stores information about a single set of IP fragments 
		///		that belong together.
		/// </summary>
		public class IpFragmentsType
		{
			#region Private Fields
		
			/// <summary>
			///		The fragments that are all part of one packet.
			/// </summary>
			private ArrayList m_fragments = new ArrayList();
			
			/// <summary>
			///		The total size of the data we are waiting on. This is calculated when
			///		we recieve the last fragment (which may not always arrive last since
			///		order is not guaranteed with IP).
			/// </summary>
			private int m_totalDataSize;
			
			/// <summary>
			///		The size of the data that has been recieved so far. Once this is equal to 
			///		the total size, all fragments have been recieved and can be pieced together.
			/// </summary>
			private int m_currentDataSize;
		
			#endregion
			
			#region Public Fields
			
			/// <summary>
			///		The fragments that are all part of one packet.
			/// </summary>
			public ArrayList Fragments
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


			/// <summary>
			///		The total size of the data we are waiting on. This is calculated when
			///		we recieve the last fragment (which may not always arrive last since
			///		order is not guaranteed with IP).
			/// </summary>
			public int TotalDataSize
			{
				get
				{
					return m_totalDataSize;
				}
				set
				{
					m_totalDataSize = value;
				}
			}
			
			
			/// <summary>
			///		The size of the data that has been recieved so far. Once this is equal to 
			///		the total size, all fragments have been recieved and can be pieced together.
			/// </summary>
			public int CurrentDataSize
			{
				get
				{
					return m_currentDataSize;
				}
				set
				{
					m_currentDataSize = value;
				}
			}
			
			
			#endregion
		}
	
	
		#endregion
		
		#region Private Fields
	
		/// <summary>
		/// 
		/// </summary>
		private ArrayList m_packets = new ArrayList ();
		
		#endregion
		
		#region Public Methods
	
		/// <summary>
		///		Whenever a new IP packet arrives it should be passed to this method.
		///		If the packet is fragmented it will be buffered until all of the other
		///		fragments have arrived, then it will be pieced back together into a whole
		///		packet.
		/// </summary>
		/// <param name="packet">
		///		The packet which has arrived.
		///	</param>
		public void HandleNewPacket(IpV4Packet packet)
		{
			#region Check for Packets we don't need to parse

			// first thing we do is check if this packet is whole or not
			if (packet.ControlFlags.Offset == 0 && !packet.ControlFlags.MoreFragments)
			{
				// it's whole so our job is done already. Just forward it on.
				if (IpV4PacketArrived != null) IpV4PacketArrived (packet);
				return;
			}
			
			#endregion
			
			// at this point we know the packet is a fragment. Now what we want to do
			// is check through the array list to see if it belongs to any other fragments.
			// If so, we store the index in the array list of the other packets for further
			// processing. If not, we add it to a new item in the array list and store the new
			// index for further processing.
			
			#region Get Packet Index
			
			int index = -1;
			
			for (int i = 0; i < m_packets.Count; i++)
			{
				IpV4Packet p = (IpV4Packet)((IpFragmentsType)m_packets[i]).Fragments[0];
				
				// we check if fragments belong by comparing their identification field, the
				// source and destination address, and the transport protocol
				if (p.Identification == packet.Identification && p.TransportProtocol == packet.TransportProtocol &&
					p.SourceAddress == packet.SourceAddress && p.DestinationAddress == packet.DestinationAddress)
				{
					index = i;
					break;
				}
			}
			
			// if the index is still -1 then this is a new fragment
			if (index == -1)
			{
				// create and add a new fragments entry to the packets array
				IpFragmentsType newFragment = new IpFragmentsType();
				index = m_packets.Add (newFragment);
			}
			
			#endregion
			
			// now we now need to add the new fragment to the packet array.
			
			#region Add Fragment
			
			IpFragmentsType currentPacket = (IpFragmentsType)m_packets[index];
			currentPacket.Fragments.Add (packet);
			
			#endregion

			// now we check if this packet has the More Fragments bit not set.
			// This indicates that it is the last fragment.
			// HOWEVER, since ip packets may arrive out of order, this does not mean we have all 
			// the fragments yet. But since it is the last packet, it does let us know how much 
			// data we are waiting on so we can identify when we have all the fragments.
			// so calculate how much data we are waiting on and check if we have recieved it all.
		
			#region Calculate Total Packet Size and Check For Completion
		
			// calculate data currently recieved
			currentPacket.CurrentDataSize += packet.TotalLength - packet.HeaderLength;
		
			if (!packet.ControlFlags.MoreFragments)
			{
				// the total length of all the data is caculated by adding the offset on to the data 
				// length of this packet, since it's the last one.
				currentPacket.TotalDataSize = packet.ControlFlags.Offset + packet.TotalLength - packet.HeaderLength;
			}
			
			// if we have recieved all the data then bingo, we have all the fragments
			// so reassemble them into a complete packet and remove these uneeded fragments
			if (currentPacket.CurrentDataSize == currentPacket.TotalDataSize)
			{
				IpV4Packet[] fragments = new IpV4Packet[currentPacket.Fragments.Count];
				
				for (int i = 0; i < fragments.Length; i++)
				{
					fragments[i] = (IpV4Packet)currentPacket.Fragments[i];
				}

				IpV4Packet WholePacket = new IpV4Packet (fragments);
				if (IpV4PacketArrived != null) IpV4PacketArrived (WholePacket);
				
				m_packets.RemoveAt (index);
			}
			
			#endregion
		}


		#endregion
	}
	
	
	#endregion
}