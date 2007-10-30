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
using System.Runtime.InteropServices;

namespace Metro
{
	#region Classes
	
	/// <summary>
	///		This class stores information about a single network interface.
	/// </summary>
	public class NetworkInterface
	{
		#region Private Fields
		
		/// <summary>
		///		The IP address of the interface.
		/// </summary>
		private IPAddress m_address = IPAddress.Any;
		
		/// <summary>
		///		The subnet mask.
		/// </summary>
		private IPAddress m_subnetMask = IPAddress.Any;
		
		/// <summary>
		///		The broadcast address.
		/// </summary>
		private IPAddress m_broadcastAddress = IPAddress.Broadcast;
		
		/// <summary>
		///		Whether or not this interface is up.
		/// </summary>
		private bool m_enabled = true;
		
		/// <summary>
		///		Whether or not this is the loopback interface.
		/// </summary>
		private bool m_loopback = true;
		
		/// <summary>
		///		Whether or not this is a point to point link.
		/// </summary>
		private bool m_pointToPointLink = true;
		
		/// <summary>
		///		Whether or not this interface supports broadcasting.
		/// </summary>
		private bool m_broadcast = true;
		
		/// <summary>
		///		Whether or not this interface supports multicasting.
		/// </summary>
		private bool m_multiCast = true;
		
		/// <summary>
		///		The interface index.
		/// </summary>
		private int m_interfaceIndex = 0;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The IP address of the interface.
		/// </summary>
		public IPAddress Address
		{
			get
			{
				return m_address;
			}
			set
			{
				m_address = value;
			}
		}
			
		
		/// <summary>
		///		The subnet mask.
		/// </summary>
		public IPAddress SubnetMask
		{
			get
			{
				return m_subnetMask;
			}
			set
			{
				m_subnetMask = value;
			}
		}
		
		
		/// <summary>
		///		The broadcast address.
		/// </summary>
		public IPAddress BroadcastAddress
		{
			get
			{
				return m_broadcastAddress;
			}
			set
			{
				m_broadcastAddress = value;
			}
		}
		
		
		/// <summary>
		///		Whether or not this interface is up.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;
			}
		}
		
		
		/// <summary>
		///		Whether or not this is the loopback interface.
		/// </summary>
		public bool isLoopback
		{
			get
			{
				return m_loopback;
			}
			set
			{
				m_loopback = value;
			}
		}
		
		
		/// <summary>
		///		Whether or not this is a point to point link.
		/// </summary>
		public bool IsPointToPointLink
		{
			get
			{
				return m_pointToPointLink;
			}
			set
			{
				m_pointToPointLink = value;
			}
		}
		
		
		/// <summary>
		///		Whether or not this interface supports broadcasting.
		/// </summary>
		public bool SupportsBroadcast
		{
			get
			{
				return m_broadcast;
			}
			set
			{
				m_broadcast = value;
			}
		}
		
		
		/// <summary>
		///		Whether or not this interface supports multicasting.
		/// </summary>
		public bool SupportMulticast
		{
			get
			{
				return m_multiCast;
			}
			set
			{
				m_multiCast = value;
			}
		}
		
		
		/// <summary>
		///		The interface index.
		/// </summary>
		public int InterfaceIndex
		{
			get
			{
				return m_interfaceIndex;
			}
			set
			{
				m_interfaceIndex = value;
			}
		}
		
		
		#endregion
	}
	

	/// <summary>
	///		This class will enumerate all network interfaces.
	/// </summary>
	public class NetworkInterfaceList
	{
		#region Enumerations
	
		/// <summary>
		///		Interface info flags.
		/// </summary>
		public enum InterfaceInfoFlags : int
		{
			/// <summary>
			///		Interface is up.
			/// </summary>
			IIF_UP = 0x1,
			
			/// <summary>
			///		Broadcast is supported.
			/// </summary>
			IIF_BROADCAST = 0x2,
			
			/// <summary>
			///		This is loopback interface.
			/// </summary>
			IIF_LOOPBACK = 0x4,
			
			/// <summary>
			///		This is point-to-point interface.
			/// </summary>
			IIF_POINTTOPOINT = 0x8,
			
			/// <summary>
			///		Multicast is supported.
			/// </summary>
			IIF_MULTICAST = 0x10
		}		


		#endregion
	
		#region Private Fields
		
		/// <summary>
		///		Insufficient buffer size.
		/// </summary>
		private const int ERROR_INSUFFICIENT_BUFFER = 122;
		
		/// <summary>
		///		The input output control code for retrieving the interface list.
		/// </summary>
		private const int SIO_GET_INTERFACE_LIST = 0x4004747f;
		
		/// <summary>
		///		The socket to perform the input output control on to retrieve the list.
		/// </summary>
		private Socket m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		
		/// <summary>
		///		The array of network interfaces.
		/// </summary>
		private NetworkInterface[] interfaces;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The array of network interfaces.
		/// </summary>
		public NetworkInterface[] Interfaces
		{
			get
			{
				return interfaces;
			}
		}
		
		
		/// <summary>
		///		Returns a network interface.
		/// </summary>
		public NetworkInterface this [int index]
		{
			get
			{
				return interfaces[index];
			}
		}
		
		
		#endregion
		
		#region Imports
		
		/// <summary>
		///		The GetIpAddrTable function retrieves the interface–to–IP address 
		///		mapping table.
		/// </summary>
		/// <param name="pIpAddrTable">
		///		Pointer to a buffer that receives the interface–to–IP address mapping 
		///		table as a MIB_IPADDRTABLE structure.
		///	</param>
		/// <param name="pdwSize">
		///		On input, specifies the size of the buffer pointed to by the 
		///		pIpAddrTable parameter.
		///		On output, if the buffer is not large enough to hold the returned 
		///		mapping table, the function sets this parameter equal to the required 
		///		buffer size.
		///	</param>
		/// <param name="bOrder">
		///		Specifies whether the returned mapping table should be sorted in 
		///		ascending order by IP address. If this parameter is TRUE, the table is 
		///		sorted.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport ("Iphlpapi.dll", SetLastError = true)]
		private static extern int GetIpAddrTable (
			IntPtr pIpAddrTable,
			ref int pdwSize,
			bool bOrder);
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new NetworkInterfaceList class.
		/// </summary>
		/// <exception cref="Exception">
		///		Thrown when the network interface list could not be read.
		///	</exception>
		public NetworkInterfaceList ()
		{
			// the length of a single interface is 76 bytes
			const int interfaceBufferLength = 76;
			
			// allocate enough space in the out buffer for up to 8 interfaces.
			// there should be no more than this
			byte[] outBuffer = new byte[interfaceBufferLength * 8];
			byte[] inBuffer = new byte[4];
			
			// get the interface list
			int bytesRecieved = m_socket.IOControl (SIO_GET_INTERFACE_LIST, inBuffer, outBuffer);
			
			// calculate the number of interfaces
			int numInterfaces = bytesRecieved / interfaceBufferLength;
			interfaces = new NetworkInterface [numInterfaces];
			
			// buffer pointer
			int position = 0;
			
			// loop through each interface
			for (int i = 0; i < numInterfaces; i++)
			{
				interfaces[i] = new NetworkInterface ();
			
				position = i*interfaceBufferLength;
			
				// extract the flags from the first 4 bytes
				int flags = BitConverter.ToInt32 (outBuffer, position);
				position += 4;
				
				// fill out the interface info flags
				interfaces[i].IsEnabled				= ((flags & (int)InterfaceInfoFlags.IIF_UP)			 != 0);
				interfaces[i].isLoopback			= ((flags & (int)InterfaceInfoFlags.IIF_LOOPBACK)	 != 0);
				interfaces[i].IsPointToPointLink	= ((flags & (int)InterfaceInfoFlags.IIF_POINTTOPOINT)!= 0);
				interfaces[i].SupportMulticast		= ((flags & (int)InterfaceInfoFlags.IIF_MULTICAST)	 != 0);
				interfaces[i].SupportsBroadcast		= ((flags & (int)InterfaceInfoFlags.IIF_BROADCAST)	 != 0);
				
				// skip over the next 4 bytes. These are the address family and port fields.
				// they are unused in this case.
				position += 4;
				
				uint address = BitConverter.ToUInt32 (outBuffer, position);
				position += 4;
				
				// skip over the next 16 bytes. These are all reserved for other address types.
				position += 16;

					// skip over the next 4 bytes. These are the address family and port fields.
				// they are unused in this case.
				position += 4;
				
				uint broadcastAddress = BitConverter.ToUInt32 (outBuffer, position);
				position += 4;
				
				// skip over the next 16 bytes. These are all reserved for other address types.
				position += 16;
				
				// skip over the next 4 bytes. These are the address family and port fields.
				// they are unused in this case.
				position += 4;
				
				uint netMaskAddress = BitConverter.ToUInt32 (outBuffer, position);
				position += 4;
				
				// skip over the next 16 bytes. These are all reserved for other address types.
				position += 16;
				
				// store the addresses in the interfaces
				interfaces[i].Address			= new IPAddress (address);
				interfaces[i].BroadcastAddress	= new IPAddress (broadcastAddress);
				interfaces[i].SubnetMask		= new IPAddress (netMaskAddress);
			}
			
			
			// pointer to buffer
			IntPtr pBuffer = IntPtr.Zero;
			
			// buffer size
			int bufferSize = 0;
			
			// return value
			int ret;
			
			
			// call the method once so we know the buffer size
			ret = GetIpAddrTable (pBuffer, ref bufferSize, true);
			
			// check for other errors
			if (ret != ERROR_INSUFFICIENT_BUFFER)
			{
				Marshal.FreeHGlobal (pBuffer);
				throw new Exception ("Unable to read IP address table");
			}
			
			// allocate enough memory to hold the ip table
			pBuffer = Marshal.AllocHGlobal (bufferSize);
			
			// call the method again for real this time
			ret = GetIpAddrTable (pBuffer, ref bufferSize, true);
			
			// check for error
			if (ret != 0)
			{
				Marshal.FreeHGlobal (pBuffer);
				throw new Exception ("Unable to read IP address table");
			}
			
			// read the number of entries
			int nEntries = Marshal.ReadInt32 (pBuffer);
			
			// loop through each entry
			for (int i = 0; i < nEntries; i++)
			{
                try
                {
                    // read the address
                    IPAddress ip = new IPAddress(Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32() + 4 + i * 24)));

                    // find the interface which this matches
                    for(int j = 0; j < interfaces.Length; j++)
                    {
                        if(interfaces[j].Address.Equals(ip))
                        {
                            // read the interface index
                            interfaces[j].InterfaceIndex = Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32() + 8 + i * 24));
                        }
                    }
                }
                catch(Exception exc)
                {
                    
                    //System.Diagnostics.Debug.Fail(exc.Message);
                }
			}
			
			// free the allocated memory
			Marshal.FreeHGlobal (pBuffer);
		}
		
		
		/// <summary>
		///		Returns a network interface by the interface index.
		/// </summary>
		/// <param name="interfaceIndex">
		///		The interface index.
		///	</param>
		/// <returns>
		///		The interface corresponding to the index or null if it couldn't be
		///		found.
		///	</returns>
		public NetworkInterface GetInterfaceByIndex (int interfaceIndex)
		{
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (interfaces[i].InterfaceIndex == interfaceIndex)
				{
					return interfaces[i];
				}
			}
			
			return null;
		}
		
		
		#endregion
	}
	
	
	#endregion
}