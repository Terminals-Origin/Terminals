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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using Metro.TransportLayer.Udp;

namespace Metro.Filtering
{
	#region Classes

	/// <summary>
	///		A class for filtering out UDP packets by the source and destination ports.
	/// </summary>
	public class UdpFilter
	{	
		#region Classes
		
		/// <summary>
		///		This class is used to provide an indexed property for the source port.
		/// </summary>
		public class UdpSourcePortFilterIndexer
		{
			#region Private Fields
		
			/// <summary>
			///		The owner class.
			/// </summary>
			private UdpFilter owner;

			#endregion
			
			#region Public Fields
			
			/// <summary>
			///		The indexer.
			/// </summary>
			public string this [int index]
			{
				get 
				{ 
					return (string)owner.m_sourcePortFilter[index]; 
				}
			}


			/// <summary>
			///		The length of the array.
			/// </summary>
			public int Length
			{
				get 
				{ 
					return owner.m_sourcePortFilter.Count; 
				}
			}


			#endregion
			
			#region Public Methods

			/// <summary>
			///		Create a new UdpSourcePortFilterIndexer class.
			/// </summary>
			/// <param name="owner">
			///		The owner.
			///	</param>
			public UdpSourcePortFilterIndexer (UdpFilter owner)
			{
				this.owner = owner;
			}

			
			#endregion
		}
	
	
		/// <summary>
		///		This class is used to provide an indexed property for the destination port.
		/// </summary>
		public class DestinationPortFilterIndexer
		{
			#region Private Fields
		
			/// <summary>
			///		The owner.
			/// </summary>
			private UdpFilter owner;

			#endregion
			
			#region Public Fields
			
			/// <summary>
			///		The indexer.
			/// </summary>
			public string this [int index]
			{
				get 
				{ 
					return (string)owner.m_destPortFilter[index]; 
				}
			}


			/// <summary>
			///		The length of the array.
			/// </summary>
			public int Length
			{
				get 
				{ 
					return owner.m_destPortFilter.Count; 
				}
			}


			#endregion
			
			#region Public Methods

			/// <summary>
			///		Create a new DestinationPortFilterIndexer class.
			/// </summary>
			/// <param name="owner">
			///		The owner class.
			///	</param>
			public DestinationPortFilterIndexer (UdpFilter owner)
			{
				this.owner = owner;
			}

			
			#endregion
		}

			
		#endregion
	
		#region Events
	
		/// <summary>
		///		The handler for when a new packet has arrived and has made it through
		///		the filters.
		/// </summary>
		public event UdpPacketArrivedHandler UdpPacketArrived;
	
		#endregion
	
		#region Private Fields
			
		/// <summary>
		///		The source port filter array. The packet must pass at least one of these to
		///		proceed.
		/// </summary>
		private ArrayList m_sourcePortFilter = new ArrayList ();

		/// <summary>
		///		The destination port filter array. The packet must pass at least one of these to
		///		proceed.
		/// </summary>
		private ArrayList m_destPortFilter = new ArrayList ();
	
		/// <summary>
		///		The source port filter array indexer.
		/// </summary>
		private UdpSourcePortFilterIndexer m_sourcePortFilterIndexer;
	
		/// <summary>
		///		The destination port filter array indexer.
		/// </summary>
		private DestinationPortFilterIndexer m_destPortFilterIndexer;
	
		#endregion
		
		#region Public Fields

		/// <summary>
		///		The source port filter array. The packet must pass at least one of these to
		///		proceed.
		/// </summary>
		public UdpSourcePortFilterIndexer SourcePortFilter
		{
			get
			{
				return m_sourcePortFilterIndexer;
			}
		}
		
		
		/// <summary>
		///		The source destination filter array. The packet must pass at least one of these to
		///		proceed. 
		/// </summary>
		public DestinationPortFilterIndexer DestinationPortFilter
		{
			get
			{
				return m_destPortFilterIndexer;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new UdpFilter class.
		/// </summary>
		public UdpFilter ()
		{
			m_sourcePortFilterIndexer	= new UdpSourcePortFilterIndexer (this);
			m_destPortFilterIndexer		= new DestinationPortFilterIndexer (this);
		}
		
		
		/// <summary>
		///		Add a new source port filter. To be let through, the port
		///		must match at least one of the filters.
		/// </summary>
		/// <param name="filter">
		///		A string containing the pattern that the port must match
		///		to be passed through. The filter can be either a number such as 6667, 80, 23; 
		///		a range of numbers of the form [min-max] such as [1-1000], [1000-65535], [23-100]; 
		///		or a combination of numbers and wild cards (?) such as ?10?, 666?, 2?, ??3.
		///	</param>
		/// <example>
		///		<code>
		///			// match ports such as 6667, 6668, 6669 etc
		///			string filter = "[6660-6669]"
		///
		///			int index = AddSourcePortFilter (filter);
		///		</code>
		///	</example>
		/// <returns>
		///		The index in the source port filters array.
		///	</returns>
		public int AddSourcePortFilter (string filter)
		{
			return m_sourcePortFilter.Add (filter);
		}
		
		
		/// <summary>
		///		Add a new destination prt filter. To be let through, the port
		///		must match at least one of the filters.
		/// </summary>
		/// <param name="filter">
		///		A string containing the pattern that the port must match
		///		to be passed through. The filter can be either a number such as 6667, 80, 23; 
		///		a range of numbers of the form [min-max] such as [1-1000], [1000-65535], [23-100]; 
		///		or a combination of numbers and wild cards (?) such as ?10?, 666?, 2?, ??3.
		///	</param>
		/// <example>
		///		<code>
		///			// match ports such as 6667, 6668, 6669 etc
		///			string filter = "[6660-6669]"
		///
		///			int index = AddSourcePortFilter (filter);
		///		</code>
		///	</example>
		/// <returns>
		///		The index in the destination port filters array.	
		///	</returns>
		public int AddDestinationPortFilter (string filter)
		{
			return m_destPortFilter.Add (filter);
		}
	
	
		/// <summary>
		///		Whenever a new UDP packet arrives it should be passed to this method.
		///		If the packet is to be filtered out it will be ignored otherwise the
		///		UdpPacketArrived will be raised with the packet
		/// </summary>
		/// <param name="packet">
		///		The packet which has arrived.
		///	</param>
		public void HandleNewPacket(UdpPacket packet)
		{
			bool match = true;
		
			// check the source port filter
			foreach (string filter in m_sourcePortFilter)
			{
				if (!UdpFilter.IsMatch (packet.SourcePort, filter))
				{
					match = false;
					break;
				}
			}
			
			// if no match then exit
			if (!match)
			{
				return;
			}
			
			// check the destination port filter
			foreach (string filter in m_destPortFilter)
			{
				if (!UdpFilter.IsMatch (packet.DestinationPort, filter))
				{
					match = false;
					break;
				}
			}
			
			// if no match then exit
			if (!match)
			{
				return;
			}
			
			// if we get here then the packet has passed through the filter and we
			// raise the event
			if (UdpPacketArrived != null)
			{
				UdpPacketArrived (packet);
			}
			
		}
		
	
		/// <summary>
		///		Check to see if an UDP matches a pattern.
		/// </summary>
		/// <param name="port">
		///		The UDP address to check.
		///	</param>
		/// <param name="filter">
		///		A string containing the pattern that the port must match
		///		to be passed through. The filter can be either a number such as 6667, 80, 23; 
		///		a range of numbers of the form [min-max] such as [1-1000], [1000-65535], [23-100]; 
		///		or a combination of numbers and wild cards (?) such as ?10?, 666?, 2?, ??3.
		///	</param>
		/// <returns>
		///		true, if they matched, false otherwise.
		///	</returns>
		public static bool IsMatch (ushort port, string filter)
		{

			// build the regex pattern for the range
			Regex rangePattern = new Regex (@"\[[0-9]+-[0-9]+\]", RegexOptions.Compiled | RegexOptions.Singleline);

			// if this is a range of the form: [xxx-yyy]
			if (rangePattern.IsMatch (filter))
			{
				// extract the min, xxx
				short min = Convert.ToInt16 (filter.Substring (1, filter.IndexOf ("-") - 1), 10);
				
				// extract the max, yyy
				short max = Convert.ToInt16 (filter.Substring (filter.IndexOf ("-") + 1, filter.IndexOf ("]") - filter.IndexOf ("-") - 1), 10);
				
				// check if the port is in the range xxx to yyy (min to max)
				if (port < min || port > max)
				{
					return false;	
				}
			}
			else
			{
				string portStr = (port.ToString()).PadLeft (5, '0');
				string filterPart = filter.PadLeft (5, '?');
				
				// check for a non match
				if (((filterPart.Substring (0, 1) != "?") && (filterPart.Substring (0, 1) != portStr.Substring (0, 1))) ||
					((filterPart.Substring (1, 1) != "?") && (filterPart.Substring (1, 1) != portStr.Substring (1, 1))) ||
					((filterPart.Substring (2, 1) != "?") && (filterPart.Substring (2, 1) != portStr.Substring (2, 1))) ||
					((filterPart.Substring (3, 1) != "?") && (filterPart.Substring (3, 1) != portStr.Substring (3, 1))) ||
					((filterPart.Substring (4, 1) != "?") && (filterPart.Substring (4, 1) != portStr.Substring (4, 1))))
				{
					return false;
				}
			}
			
			return true;
			
		}
		
		
		#endregion
	}
	
	
	#endregion
}