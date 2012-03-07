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
using Metro.NetworkLayer.IpV4;

namespace Metro.Filtering
{
	#region Classes

	/// <summary>
	///		A class for filtering out IP packets by the source and destination address.
	/// </summary>
	public class IpV4Filter
	{	
		#region Classes
		
		/// <summary>
		///		This class is used to provide an indexed property for the source address.
		/// </summary>
		public class SourceAddressFilterIndexer
		{
			#region Private Fields
		
			/// <summary>
			///		The owner of the indexer.
			/// </summary>
			private IpV4Filter owner;

			#endregion
			
			#region Public Fields
			
			/// <summary>
			///		The indexer itself returns a single filter.
			/// </summary>
			public string this [int index]
			{
				get 
				{ 
					return (string)owner.m_sourceAddressFilter[index]; 
				}
			}


			/// <summary>
			///		The length of the array.
			/// </summary>
			public int Length
			{
				get 
				{ 
					return owner.m_sourceAddressFilter.Count; 
				}
			}


			#endregion
			
			#region Public Methods

			/// <summary>
			///		Create a new SourceAddressFilterIndexer class.
			/// </summary>
			/// <param name="owner">
			///		The owner of the class.
			///	</param>
			public SourceAddressFilterIndexer (IpV4Filter owner)
			{
				this.owner = owner;
			}

			
			#endregion
		}
	
	
		/// <summary>
		///		This class is used to provide an indexed property for the destination address.
		/// </summary>
		public class DestinationAddressFilterIndexer
		{
			#region Private Fields
		
			/// <summary>
			///		The owner of this class.
			/// </summary>
			private IpV4Filter owner;

			#endregion
			
			#region Public Fields
			
			/// <summary>
			///		The indexer returning individual elements in the array.
			/// </summary>
			public string this [int index]
			{
				get 
				{ 
					return (string)owner.m_destAddressFilter[index]; 
				}
			}


			/// <summary>
			///		The length of the array.
			/// </summary>
			public int Length
			{
				get 
				{ 
					return owner.m_destAddressFilter.Count; 
				}
			}


			#endregion
			
			#region Public Methods

			/// <summary>
			///		Create a new DestinationAddressFilterIndexer.
			/// </summary>
			/// <param name="owner">
			///		The owner of the class.
			///	</param>
			public DestinationAddressFilterIndexer (IpV4Filter owner)
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
		public event IpV4PacketArrivedHandler IpV4PacketArrived;
	
		#endregion
	
		#region Private Fields
			
		/// <summary>
		///		The array of source address filters. A packet must pass at least one of
		///		these tests to mae it through.
		/// </summary>
		private ArrayList m_sourceAddressFilter = new ArrayList ();

		/// <summary>
		///		The array of destination address filters. A packet must pass at least one of
		///		these tests to mae it through.
		/// </summary>
		private ArrayList m_destAddressFilter = new ArrayList ();
	
		/// <summary>
		///		The indexer class for the source address, allowing an indexed property.
		/// </summary>
		private SourceAddressFilterIndexer m_sourceAddressFilterIndexer;
	
		/// <summary>
		///		The indexer class for the source address, allowing an indexed property.
		/// </summary>
		private DestinationAddressFilterIndexer m_destAddressFilterIndexer;
	
		#endregion
		
		#region Public Fields

		/// <summary>
		///		The array of source address filters. A packet must pass at least one of these
		///		to proceed.
		/// </summary>
		public SourceAddressFilterIndexer SourceAddressFilter
		{
			get
			{
				return m_sourceAddressFilterIndexer;
			}
		}
		
		
		/// <summary>
		///		The array of source address filters. A packet must pass at least one of these
		///		to proceed.
		/// </summary>
		public DestinationAddressFilterIndexer DestinationAddressFilter
		{
			get
			{
				return m_destAddressFilterIndexer;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new IpV4Filter class.
		/// </summary>
		public IpV4Filter ()
		{
			m_sourceAddressFilterIndexer	= new SourceAddressFilterIndexer (this);
			m_destAddressFilterIndexer		= new DestinationAddressFilterIndexer (this);
		}
		
		
		/// <summary>
		///		Add a new source address filter. To be let through, the address
		///		must match at least one of the filters.
		/// </summary>
		/// <param name="filter">
		///		A string containing the pattern that the ip address must match
		///		to be passed through. The filter must be in the same general form
		///		as an ip address, in that it has 4 parts, seperated by periods.
		///		Each part can be either a number such as 255, 128, 1; a range of
		///		numbers of the form [min-max] such as [1-10], [128-255], [2-5]; or
		///		a combination of numbers and wild cards (?) such as 1?, ?10, 20?, ??3.
		///	</param>
		/// <example>
		///		<code>
		///			// match ip's such as 192.168.1.1, 192.168.6.100, 193.168.10.0 etc
		///			string filter = "192.168.[1-10].?
		///
		///			int index = AddSourceAddressFilter (filter);
		///		</code>
		///	</example>
		/// <returns>
		///		The index in the source address filters array.
		///	</returns>
		public int AddSourceAddressFilter (string filter)
		{
			return m_sourceAddressFilter.Add (filter);
		}
		
		
		/// <summary>
		///		Add a new destination address filter. To be let through, the address
		///		must match at least one of the filters.
		/// </summary>
		/// <param name="filter">
		///		A string containing the pattern that the ip address must match
		///		to be passed through. The filter must be in the same general form
		///		as an ip address, in that it has 4 parts, seperated by periods.
		///		Each part can be either a number such as 255, 128, 1; a range of
		///		numbers of the form [min-max] such as [1-10], [128-255], [2-5]; or
		///		a combination of numbers and wild cards (?) such as 1?, ?10, 20?, ??3.
		///	</param>
		/// <example>
		///		<code>
		///			// match ip's such as 192.168.1.1, 192.168.6.100, 193.168.10.0 etc
		///			string filter = "192.168.[1-10].?
		///
		///			int index = AddDestinationAddressFilter (filter);
		///		</code>
		///	</example>
		/// <returns>
		///		The index in the destination address filters array.	
		///	</returns>
		public int AddDestinationAddressFilter (string filter)
		{
			return m_destAddressFilter.Add (filter);
		}
	
	
		/// <summary>
		///		Whenever a new IP packet arrives it should be passed to this method.
		///		If the packet is to be filtered out it will be ignored otherwise the
		///		IpV4PacketArrived will be raised with the packet
		/// </summary>
		/// <param name="packet">
		///		The packet which has arrived.
		///	</param>
		public void HandleNewPacket(IpV4Packet packet)
		{
			bool match = true;
		
			// check the source address filter
			foreach (string filter in m_sourceAddressFilter)
			{
				if (!IpV4Filter.IsMatch (packet.SourceAddress.ToString(), filter))
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
			
			// check the destination address filter
			foreach (string filter in m_destAddressFilter)
			{
				if (!IpV4Filter.IsMatch (packet.DestinationAddress.ToString(), filter))
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
			if (IpV4PacketArrived != null)
			{
				IpV4PacketArrived (packet);
			}
			
		}
		
	
		/// <summary>
		///		Check to see if an IP matches a pattern.
		/// </summary>
		/// <param name="address">
		///		The IP address to check.
		///	</param>
		/// <param name="filter">
		///		A string containing the pattern that the ip address must match
		///		to be passed through. The filter must be in the same general form
		///		as an ip address, in that it has 4 parts, seperated by periods.
		///		Each part can be either a number such as 255, 128, 1; a range of
		///		numbers of the form [min-max] such as [1-10], [128-255], [2-5]; or
		///		a combination of numbers and wild cards (?) such as 1?, ?10, 20?, ??3.
		///	</param>
		/// <returns>
		///		true, if they matched, false otherwise.
		///	</returns>
		/// <exception cref="ArgumentException">
		///		ArgumentException is thrown if the IP address is invalid or if
		///		the filter is invalid.
		///	</exception>
		public static bool IsMatch (string address, string filter)
		{
		
			// make sure the address is valid
			// Regex validIp = new Regex (@"(25[0-5]|2[0-4]\d|[0-1]{1}\d{2}|[1-9]{1}\d{1}|[1-9])\.(25[0-5]|2[0-4]\d|[0-1]{1}\d{2}|[1-9]{1}\d{1}|[1-9]|0)\.(25[0-5]|2[0-4]\d|[0-1]{1}\d{2}|[1-9]{1}\d{1}|[1-9]|0)\.(25[0-5]|2[0-4]\d|[0-1]{1}\d{2}|[1-9]{1}\d{1}|\d)", RegexOptions.Compiled | RegexOptions.Singleline);
			Regex validIp = new Regex (@"^(((25[0-5])|(2[0-4]\d)|([0-1]?\d?\d))\.){3}((25[0-5])|(2[0-4]\d)|([0-1]?\d?\d))$", RegexOptions.Compiled | RegexOptions.Singleline);
	
			if (!validIp.IsMatch (address))
			{
				throw new ArgumentException ("The ip address is not valid", "address");
			}
			
			// check that the filter actually contains a period before trying to split
			if (filter.IndexOf (".") == 0)
			{
				throw new ArgumentException ("The filter is not valid", "filter");
			}
			
			// split the filter into its seperate parts and make sure there are 4 of them
			string[] filterParts = filter.Split ('.');
			
			if (filterParts.Length != 4)
			{
				throw new ArgumentException ("The filter must contain 4 parts of the form *.*.*.*", "filter");
			}
			
			// split the ip address into its seperate parts
			string[] ipParts = address.Split ('.');

			// build the regex pattern for the range
			Regex rangePattern = new Regex (@"\[[0-9]+-[0-9]+\]", RegexOptions.Compiled | RegexOptions.Singleline);

			// check each part
			for (int i = 0; i < 4; i++)
			{
				// if this is a range of the form: [xxx-yyy]
				if (rangePattern.IsMatch (filterParts[i]))
				{
					// extract the min, xxx
					byte min = Convert.ToByte (filterParts[i].Substring (1, filterParts[i].IndexOf ("-") - 1), 10);
					
					// extract the max, yyy
					byte max = Convert.ToByte (filterParts[i].Substring (filterParts[i].IndexOf ("-") + 1, filterParts[i].IndexOf ("]") - filterParts[i].IndexOf ("-") - 1), 10);
					
					// check if this ip part is in the range xxx to yyy (min to max)
					if (Convert.ToByte (ipParts[i], 10) < min || Convert.ToByte (ipParts[i], 10) > max)
					{
						return false;	
					}
				}
				else
				{
					string ipPart = ipParts[i].PadLeft (3, '0');
					string filterPart = filterParts[i].PadLeft (3, '?');
					
					// check for a non match
					if (((filterPart.Substring (0, 1) != "?") && (filterPart.Substring (0, 1) != ipPart.Substring (0, 1))) ||
						((filterPart.Substring (1, 1) != "?") && (filterPart.Substring (1, 1) != ipPart.Substring (1, 1))) ||
						((filterPart.Substring (2, 1) != "?") && (filterPart.Substring (2, 1) != ipPart.Substring (2, 1))))
					{
						return false;
					}
				}
			}
			
			return true;
			
		}
		
		
		#endregion
	}
	
	
	#endregion
}