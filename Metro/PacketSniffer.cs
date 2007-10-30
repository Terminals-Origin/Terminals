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
using System.Threading;
using System.Collections;

namespace Metro
{
	#region Enumerations

	/// <summary>
	///		The mode to work in.
	/// </summary>
	public enum SnifferModeType : int
	{
		/// <summary>
		///		Blocking mode, for console applications or seperate threads.
		/// </summary>
		Blocking,
	
		/// <summary>
		///		Asynchronous mode, for GUI threads.
		/// </summary>
		Asynchronous
	}	


	#endregion
	
	#region Delegates
	
	/// <summary>
	///		The handler for the event which occurs when a new raw packet arrives
	/// </summary>
	public delegate void NewPacketHandler (byte[] packet);
	
	#endregion
	
	#region Classes

	/// <summary>
	///		A packet sniffer class, allowing packet sniffing on an interface down to the
	///		network layer.
	/// </summary>
	public class PacketSniffer
	{
		#region Events
		
		/// <summary>
		///		This event is raised whenever a new packet arrives.
		/// </summary>
		public event NewPacketHandler NewPacket;
		
		#endregion
	
		#region Classes
	
		/// <summary>
		///		The state object,
		/// </summary>
		private class StateObject
		{
			#region Private Fields
		
			/// <summary>
			///		Buffer size.
			/// </summary>
			private const int BUFFER_SIZE = 0x1000;
		
			/// <summary>
			///		The socket object.
			/// </summary>
			private Socket m_socketObject;
			
			/// <summary>
			///		The recieve buffer.
			/// </summary>
			private byte[] m_recieveBuffer = new byte[BUFFER_SIZE];
			
			/// <summary>
			///		The wait object.
			/// </summary>
			private AutoResetEvent m_waitObject = new AutoResetEvent (false);
			
			#endregion
			
			#region Public Fields
			
			/// <summary>
			///		The socket object.
			/// </summary>
			public Socket SocketObject
			{
				get
				{
					return m_socketObject;
				}
				set
				{
					m_socketObject = value;
				}
			}
			
			
			/// <summary>
			///		The recieve buffer.
			/// </summary>
			public byte[] RecieveBuffer
			{
				get
				{
					return m_recieveBuffer;
				}
				set
				{
					m_recieveBuffer = value;
				}
			}
			
			
			/// <summary>
			///		The wait object.
			/// </summary>
			public AutoResetEvent WaitObject
			{
				get
				{
					return m_waitObject;
				}
				set
				{
					m_waitObject = value;
				}
			}
			
			
			#endregion
		}
		
	
		#endregion

		#region Private Fields
		
		/// <summary>
		///		SECURITY_BUILTIN_DOMAIN_RID.
		/// </summary>
		private const int SECURITY_BUILTIN_DOMAIN_RID = 0x20;
		
		/// <summary>
		///		DOMAIN_ALIAS_RID_ADMINS.
		/// </summary>
		private const int DOMAIN_ALIAS_RID_ADMINS = 0x220;
		
		/// <summary>
		///		The option for recieving all packets.
		/// </summary>
		private const int RecieveAll = unchecked((int)0x98000001);
		
		/// <summary>
		///		Whether or not the sniffer is running.
		/// </summary>
		private bool m_isRunning;
		
		/// <summary>
		///		The mode to run in.
		/// </summary>
		private SnifferModeType m_mode;
	
		/// <summary>
		///		The number of bytes recieved.
		/// </summary>
		private int m_bytesReceived;
		
		/// <summary>
		///		The state object.
		/// </summary>
		private StateObject m_state = new StateObject ();
		
		#endregion
		
		#region Imports
		
		/// <summary>
		/// The AllocateAndInitializeSid function allocates and initializes a security 
		///	identifier (SID) with up to eight subauthorities.
		/// </summary>
		/// <param name="pIdentifierAuthority">
		///		Pointer to a SID_IDENTIFIER_AUTHORITY structure, giving the top-level 
		///		identifier authority value to set in the SID.
		///	</param>
		/// <param name="nSubAuthorityCount">
		///		Specifies the number of subauthorities to place in the SID. This parameter 
		///		also identifies how many of the subauthority parameters have meaningful 
		///		values. This parameter must contain a value from 1 to 8.
		///	</param>
		/// <param name="dwSubAuthority0">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority1">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority2">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority3">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority4">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority5">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority6">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="dwSubAuthority7">
		///		Subauthority value to place in the SID.
		///	</param>
		/// <param name="pSid">
		///		Pointer to a variable that receives the pointer to the allocated and 
		///		initialized SID structure.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is nonzero. If the function 
		///		fails, the return value is zero. To get extended error information, call 
		///		GetLastError.
		///	</returns>
		[DllImport("advapi32.dll")]
		private extern static int AllocateAndInitializeSid(byte[] pIdentifierAuthority, byte nSubAuthorityCount, int dwSubAuthority0, int dwSubAuthority1, int dwSubAuthority2, int dwSubAuthority3, int dwSubAuthority4, int dwSubAuthority5, int dwSubAuthority6, int dwSubAuthority7, out IntPtr pSid);
		
		/// <summary>
		///		The CheckTokenMembership function determines whether a specified SID 
		///		is enabled in an access token.
		/// </summary>
		/// <param name="TokenHandle">
		///		Handle to an access token. The handle must have TOKEN_QUERY access to 
		///		the token. The token must be an impersonation token.
		///</param>
		/// <param name="SidToCheck">
		///		Pointer to a SID structure. The CheckTokenMembership function checks 
		///		for the presence of this SID in the user and group SIDs of the access 
		///		token.
		///	</param>
		/// <param name="IsMember">
		///		Pointer to a variable that receives the results of the check. If the 
		///		SID is present and has the SE_GROUP_ENABLED attribute, IsMember returns
		///		TRUE; otherwise, it returns FALSE.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is nonzero. If the function fails,
		///		the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("advapi32.dll")]
		private extern static int CheckTokenMembership(IntPtr TokenHandle, IntPtr SidToCheck, ref int IsMember);
		
		/// <summary>
		///		The FreeSid function frees a security identifier (SID) previously allocated 
		///		by using the AllocateAndInitializeSid function.
		/// </summary>
		/// <param name="pSid">
		///		Pointer to the SID structure to free.
		///	</param>
		[DllImport("advapi32.dll")]
		private extern static IntPtr FreeSid(IntPtr pSid);
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Whether or not the sniffer is running.
		/// </summary>
		public bool IsRunning
		{
			get
			{
				return m_isRunning;
			}
			set
			{
				m_isRunning = value;
			}
		}
		
		
		/// <summary>
		///		The mode to run the sniffer in.
		/// </summary>
		public SnifferModeType SnifferMode
		{
			get
			{
				return m_mode;
			}
			set
			{
				m_mode = value;
			}
		}
	
	
		/// <summary>
		///		The number of bytes recieved.
		/// </summary>
		public int BytesReceived
		{
			get
			{
				return m_bytesReceived;
			}
			set
			{
				m_bytesReceived = value;
			}
		}
		

		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new packet sniffer class.
		/// </summary>
		/// <exception cref="NotSupportedException">
		///		This program requires administrative privilages on 
		///		Windows 2000, Windows XP or Windows .NET Server
		///	</exception>
		public PacketSniffer ()
		{
			// make sure the user runs this program on Windows NT 5.0 or higher
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				throw new NotSupportedException("This program requires Windows 2000, Windows XP or Windows .NET Server!");
			}
			
			byte[] NtAuthority = new byte[6];
			IntPtr AdministratorsGroup;
			
			NtAuthority[5] = 5; // SECURITY_NT_AUTHORITY
			
			
			int ret = AllocateAndInitializeSid(NtAuthority, 2, SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, out AdministratorsGroup); 
		
			if (ret != 0) 
			{
				if (CheckTokenMembership(IntPtr.Zero, AdministratorsGroup, ref ret) == 0) 
				{
					ret = 0;
				}
				
				FreeSid(AdministratorsGroup); 
			}

			if (ret == 0)
			{
				throw new NotSupportedException("You must have administrative privilages to run this program.");
			}
		}
		
		
		/// <summary>
		///		Stop listening.
		/// </summary>
		public void StopListening ()
		{
			if (m_isRunning)
			{
				m_state.SocketObject.Close ();
				m_isRunning = false;
			}
		}
		
		
		/// <summary>
		///		Start listening for packets.
		/// </summary>
		/// <param name="networkInterface">
		///		The network interface to listen on.
		///	</param>
		/// <param name="mode">
		///		The mode to run in.
		///	</param>
		/// <param name="protocol">
		///		The protocl to listen for.
		///	</param>
		public void StartListening (IPEndPoint networkInterface, SnifferModeType mode, ProtocolType protocol)
		{
		
			// make sure the socket is closed
			if (m_isRunning)
			{
				m_state.SocketObject.Close ();
			}
		
			// create a raw ip socket
			m_state.SocketObject = new Socket (AddressFamily.InterNetwork,
											   SocketType.Raw,
											   protocol);
			
			// bind the socket to the interface
			m_state.SocketObject.Bind (networkInterface);

			int inBuff		= 1; 
			int outBuff		= 1;
			
			// set the socket to receive all packets
			m_state.SocketObject.IOControl (RecieveAll, 
											BitConverter.GetBytes(inBuff), 
											BitConverter.GetBytes(outBuff));
											
			// save some state variables
			m_isRunning = true;
			m_mode		= mode;
			
			// reset the wait object for blocking mode
			m_state.WaitObject.Reset();
			
			// start the recieve loop
			m_state.SocketObject.BeginReceive (m_state.RecieveBuffer, 
											   0, 
											   m_state.RecieveBuffer.Length,
											   SocketFlags.None,
											   new AsyncCallback (ReceiveCallback),
											   m_state);
			
			// if in blocking mode then block until the wait object is triggered
			// by the callback
			if (mode == SnifferModeType.Blocking)
			{
				m_state.WaitObject.WaitOne ();
			}
		}
		
		
		/// <summary>
		///		Start listening for packets.
		/// </summary>
		/// <param name="networkInterface">
		///		The network interface to listen on.
		///	</param>
		/// <param name="mode">
		///		The mode to run in.
		///	</param>
		public void StartListening (IPEndPoint networkInterface, SnifferModeType mode)
		{	
			StartListening (networkInterface, mode, ProtocolType.IP);
		}
		
				
		/// <summary>
		///		Start listening for packets.
		/// </summary>
		/// <param name="networkInterface">
		///		The network interface to listen on.
		///	</param>
		public void StartListening (IPEndPoint networkInterface)
		{	
			StartListening (networkInterface, SnifferModeType.Blocking, ProtocolType.IP);
		}
				
				
		#endregion
		
		#region Private Methods
		
		/// <summary>
		///		The callback for when a new packet has arrived.
		/// </summary>
		/// <param name="ar">
		///		Async result.
		///	</param>
		private void ReceiveCallback (IAsyncResult ar)
		{
		
			// extract the state object
			StateObject state = ar.AsyncState as StateObject;
		
			int bytesReceived = 0;
		
			try
			{
	
				// finish the recieve
				bytesReceived = state.SocketObject.EndReceive (ar);
		
				// keep a count of the number of bytes received
				m_bytesReceived += bytesReceived;
			
				// create a new buffer of the correct size
				byte[] packet = new byte[bytesReceived];
				
				// copy the data in to it
				Array.Copy (state.RecieveBuffer, 0, packet, 0, bytesReceived);

				// keep recieving data
				m_state.SocketObject.BeginReceive (state.RecieveBuffer, 
												0, 
												state.RecieveBuffer.Length,
												SocketFlags.None,
												new AsyncCallback (ReceiveCallback),
												state);							
				// raise the event			   
				if (NewPacket != null)
				{
					NewPacket (packet);
				}
				
			}
			catch (Exception exc)
			{	
				// if the object is disposed it has been closed because StopListen has been called
                
				m_isRunning = false;
				state.SocketObject = null;
				m_bytesReceived = 0;
				
				// set the wait object to stop the thread from being blocked if the class is
				// using blocking mode
				if (m_mode == SnifferModeType.Blocking)
				{ 
					state.WaitObject.Set ();
				}
				
				return;
			}
		}
		
		
		#endregion
	}
	
	
	#endregion
}