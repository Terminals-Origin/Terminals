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

namespace Metro.TransportLayer.Tcp
{
	#region Classes

	/// <summary>
	///		If this option is present, then it communicates the maximum
    ///		receive segment size at the TCP which sends this segment.
	///		This field must only be sent in the initial connection request
    ///		(i.e., in segments with the SYN control bit set). If this
    ///		option is not used, any segment size is allowed.
	/// </summary>
	public class TcpMaxSegmentSizeOption
	{
		/*******************************************\
		*                                           *
		*   +--------+--------+---------+--------+  *
        *   |00000010|00000100|   max seg size   |  *
        *   +--------+--------+---------+--------+  *
		*                                           *
		*          Kind=2   Length=4                *
		*                                           *
		\*******************************************/
	
		#region Private Fields
		
		/// <summary>
		///		If this option is present, then it communicates the maximum
        ///		receive segment size at the TCP which sends this segment.
		/// </summary>
		private ushort m_maxSegmentSize;
	
		#endregion
		
		#region Public Fields

		/// <summary>
		///		If this option is present, then it communicates the maximum
        ///		receive segment size at the TCP which sends this segment.
		/// </summary>
		public ushort MaxSegmentSize
		{
			get
			{
				return m_maxSegmentSize;
			}
			set
			{
				m_maxSegmentSize = value;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new max segment size option.
		/// </summary>
		public TcpMaxSegmentSizeOption ()
		{
		}
		
		
		/// <summary>
		///		Create a new max segment size option.
		/// </summary>
		/// <param name="option">
		///		The option to parse.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		The option argument is null.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The option argument is not a max segment size option.
		///	</exception>
		public TcpMaxSegmentSizeOption (TcpOption option)
		{
			if (option == null)
			{
				throw new ArgumentNullException ("option");
			}
		
			if (option.OptionType != TcpOptionNumber.MaximumSegmentSize)
			{
				throw new ArgumentException ("The option passed was not compatible with this class. This class accepts max segment size options only", "option");
			}
			
			m_maxSegmentSize = BitConverter.ToUInt16 (option.Data, 0);
		}
		
		
		/// <summary>
		///		Serlialize this routing option into something which
		///		can be passed to a TcpPacket class.
		/// </summary>
		/// <returns>	
		///		An InternetProtocolOption class which can be passed to an IpV4Packet.
		///	</returns>
		public TcpOption Serialize ()
		{
			TcpOption newOption = new TcpOption ();
			
			newOption.OptionType = TcpOptionNumber.MaximumSegmentSize;
			newOption.Length = 4;
			newOption.Data = BitConverter.GetBytes (m_maxSegmentSize);
			
			return newOption;
		}
		
		
		#endregion
	}

	#endregion
}