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
using System.Runtime;
using System.Runtime.InteropServices;

namespace Metro.TransportLayer.Tcp
{
	#region Enumerations

	/// <summary>
	///		A connection progresses through a series of states during its
	///		lifetime. The states are: LISTEN, SYN-SENT, SYN-RECEIVED,
	///		ESTABLISHED, FIN-WAIT-1, FIN-WAIT-2, CLOSE-WAIT, CLOSING, LAST-ACK,
	///		TIME-WAIT, and the fictional state CLOSED. CLOSED is fictional
	///		because it represents the state when there is no TCB, and therefore,
	///		no connection.
	/// </summary>
	public enum TcpState : byte
	{
		/// <summary>
		///		Represents no connection state at all.
		/// </summary>
		Closed = 1,
		
		/// <summary>
		///		Represents waiting for a connection request from any remote TCP 
		///		and port.
		/// </summary>
		Listen = 2,
		
		/// <summary>
		///		Represents waiting for a matching connection request after having 
		///		sent a connection request.
		/// </summary>
		SynSent = 3,
		
		/// <summary>
		///		Represents waiting for a confirming connection equest acknowledgment 
		///		after having both received and sent a connection request.
		/// </summary>
		SynRecieved = 4,
		
		/// <summary>
		///		Represents an open connection, data received can be delivered to the 
		///		user. The normal state for the data transfer phase of the connection.
		/// </summary>
		Established = 5,
		
		/// <summary>
		///		Represents waiting for a connection termination request from the remote 
		///		TCP, or an acknowledgment of the connection termination request previously 
		///		sent.
		/// </summary>
		FinWait1 = 6,
		
		/// <summary>
		///		Represents waiting for a connection termination request from the remote TCP.
		/// </summary>
		FinWait2 = 7,
		
		/// <summary>
		///		Represents waiting for a connection termination request from the local user.
		/// </summary>
		CloseWait = 8,
		
		/// <summary>
		///		Represents waiting for a connection termination request acknowledgment 
		///		from the remote TCP.
		/// </summary>
		Closing = 9, 
		
		/// <summary>
		///		Represents waiting for an acknowledgment of the connection termination 
		///		request previously sent to the remote TCP (which includes an acknowledgment 
		///		of its connection termination request).
		/// </summary>
		LastAck = 10,
		
		/// <summary>
		///		Represents waiting for enough time to pass to be sure the remote TCP 
		///		received the acknowledgment of its connection termination request.
		/// </summary>
		TimeWait = 11,
		
		/// <summary>
		///		Delete TCB
		/// </summary>
		DeleteTcb = 12
	}
	

	#endregion
	
	#region Classes

	/// <summary>
	/// 
	/// </summary>
	public class TcpConnection
	{
		#region Private Fields
		
		/// <summary>
		///		The local end point.
		/// </summary>
		private IPEndPoint m_localEndPoint;
		
		/// <summary>
		///		The remote end point.
		/// </summary>
		private IPEndPoint m_remoteEndPoint;
		
		/// <summary>
		///		The connection state.
		/// </summary>
		private TcpState m_state;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The local end point.
		/// </summary>
		public IPEndPoint LocalEndPoint
		{
			get
			{
				return m_localEndPoint;
			}
			set
			{
				m_localEndPoint = value;
			}
		}
		
		
		/// <summary>
		///		The remote end point.
		/// </summary>
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return m_remoteEndPoint;
			}
			set
			{
				m_remoteEndPoint = value;
			}
		}
		
		
		/// <summary>
		///		The connection state.
		/// </summary>
		public TcpState State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}
		
		
		#endregion	
	}


	/// <summary>
	///		The TcpConnectionManager class allows the TCP connection table to be read.
	/// </summary>
	public class TcpConnectionManager
	{
		#region DLL Imports
	
		/// <summary>
		///		Read the TCP table.
		/// </summary>
		/// <param name="pTcpTable">
		///		The buffer to store the table in.
		///	</param>
		/// <param name="pdwSize">
		///		The size of the table in bytes.
		///	</param>
		/// <param name="bOrder">	
		///		Whether or not to order the table.
		///	</param>
		/// <returns>
		///		Return value.
		///	</returns>
		[DllImport("iphlpapi.dll",SetLastError=true)]
		private static extern int GetTcpTable(byte[] pTcpTable, out int pdwSize, bool bOrder); 

		#endregion

		#region Public Methods

		/// <summary>
		///		Retrieves the current table of tcp connections.
		/// </summary>
		/// <returns>
		///		An array of tcp connection entries representing the
		///		tcp table.
		///	</returns>
		public static TcpConnection[] GetCurrentTcpConnections ()
		{
		
			const int NO_ERROR = 0;
		
			// start with a buffer size of 20000 bytes
			int size = 20000;
			byte[] buffer = new byte[size];
			
			// call the function once to get the buffer size
			if (GetTcpTable (buffer, out size, true) != NO_ERROR)
			{
				return null;
			}
			
			// allocate the correct number of bytes for the buffer
			buffer = new byte[size];
			
			// call it again with the correct buffer size
			if (GetTcpTable (buffer, out size, true) != NO_ERROR)
			{
				return null;
			}
			
			// copy out the number of rows
			int rows = BitConverter.ToInt32 (buffer, 0);
			
			// allocate enough space in the table
			TcpConnection[] connection = new TcpConnection [rows];
			
			#region Read Table
			
			// loop through each row
			for (int i = 0; i < rows; i++)
			{
				connection[i] = new TcpConnection();
			
				// copy out the state
				connection[i].State = (TcpState)BitConverter.ToInt32 (buffer, (i * 20) + 4);
				
				// copy out the local address
				IPAddress localAddress = IPAddress.Parse (buffer[(i * 20) + 8].ToString() + "." +
														  buffer[(i * 20) + 9].ToString() + "." +
														  buffer[(i * 20) + 10].ToString() + "." +
														  buffer[(i * 20) + 11].ToString());
				
				// copy out the local port
				int localPort =							(((int) buffer[(i * 20) + 12]) <<8)  + 
														(((int) buffer[(i * 20) + 13]))      + 
														(((int) buffer[(i * 20) + 14]) <<24) + 
														(((int) buffer[(i * 20) + 15]) <<16);
				
				// store the local end point in the array
				connection[i].LocalEndPoint = new IPEndPoint (localAddress, localPort);
				
				// copy out the remote address
				IPAddress remoteAddress = IPAddress.Parse (buffer[(i * 20) + 16].ToString() + "." +
														   buffer[(i * 20) + 17].ToString() + "." +
														   buffer[(i * 20) + 18].ToString() + "." +
														   buffer[(i * 20) + 19].ToString());
				
				
				// copy out the remote port
				int remotePort =						(((int) buffer[(i * 20) + 20]) <<8)  + 
														(((int) buffer[(i * 20) + 21]))      + 
														(((int) buffer[(i * 20) + 22]) <<24) + 
														(((int) buffer[(i * 20) + 23]) <<16);
				
				// store the remote end point in the array
				connection[i].RemoteEndPoint = new IPEndPoint (remoteAddress, remotePort);
			}
			
			#endregion
			
			return connection;
		}


		#endregion
	}
	
	
	#endregion
}
