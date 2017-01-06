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


namespace Metro.Scanning
{
	#region Enumerations

	/// <summary>
	///		The TCP port state.
	/// </summary>
	public enum TcpPortState
	{
		/// <summary>
		///		The port is opened and accepts to connection requests.
		/// </summary>
		Closed,
		
		/// <summary>
		///		The port is closed and rejects connection requests.
		/// </summary>
		Opened,
		
		/// <summary>
		///		The port is filtered by a firewall of some kind and does not reply
		///		to any requests at all.
		/// </summary>
		Filtered
	}
	

	#endregion

	#region Delegates
	
	/// <summary>
	///		The handler for an event where a port state is found.
	/// </summary>
	public delegate void TcpPortReplyHandler (IPEndPoint remoteEndPoint, TcpPortState state);

	/// <summary>
	///		The handler for when a port scan is complete.
	/// </summary>
	public delegate void TcpPortScanComplete ();

	#endregion
}