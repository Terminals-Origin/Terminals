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
using System.Threading;
using System.Timers;
using Metro.NetworkLayer.IpV4;

namespace Metro.TransportLayer.Icmp
{	
	#region Delegates

	/// <summary>
	///		The handler for when a new ping has arrived.
	/// </summary>
	public delegate void IcmpPingReplyHandler (IpV4Packet ipHeader, IcmpPacket icmpHeader, int roundTripTime);

	/// <summary>
	///		The handler for when a ping request times out.
	/// </summary>
	public delegate void IcmpPingTimeOutHandler ();

	#endregion

	#region Classes
	
	/// <summary>
	///		This class will handle sending and recieving of
	///		a single ping packet, including matching up the correct reply and calculating
	///		the round trip time.
	/// </summary>
	public class IcmpPingManager : IDisposable
	{
		#region Events
		
		/// <summary>
		///		When the ping reply has been recieved, this event is raised.
		/// </summary>
		public event IcmpPingReplyHandler PingReply;
		
		/// <summary>
		///		When a ping request times out, this event is raised.
		/// </summary>
		public event IcmpPingTimeOutHandler PingTimeout;
		
		#endregion
	
		#region Private Fields
		
		/// <summary>
		///		Stores whether or not this object has been disposed of.
		/// </summary>
		private bool m_disposed = false;
		
		/// <summary>
		///		We need a new sniffer to sniff for incoming icmp packets so we can look for the
		///		replies.
		/// </summary>
		private PacketSniffer m_sniffer = new PacketSniffer ();
		
		/// <summary>
		///		The identification of the current ping packet is stored so we can match its
		///		reply.
		/// </summary>
		private ushort m_id = 0;
	
		/// <summary>
		///		The remote address of the current ping packet is stores so that we can match
		///		its reply.
		/// </summary>
		private IPAddress m_remoteAddress = IPAddress.Any;
		
		/// <summary>
		///		The wait object is used in blocking mode to stop the SendPing method
		///		from returning until the reply has been recieved.
		/// </summary>
		private AutoResetEvent m_waitObject = new AutoResetEvent (false);

		/// <summary>
		///		The time we sent the packet.
		/// </summary>
		private int m_start = 0;
		
		/// <summary>
		///		The time we recieved the reply.
		/// </summary>
		private int m_stop = 0;

		/// <summary>
		///		Create a new timer used for timeouts.
		/// </summary>
		private System.Timers.Timer m_timer = new System.Timers.Timer ();

		/// <summary>
		///		The local interface to bind to.
		/// </summary>
		private IPAddress m_networkInterface = IPAddress.Any;

		/// <summary>
		///		Whether or not to fragment IP packets.
		/// </summary>
		private bool m_fragment = false;
	
		/// <summary>
		///		The private socket for sending the packets with.
		/// </summary>
		private Socket m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
	
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Whether or not to fragment IP packets.
		/// </summary>
		public bool FragmentPackets
		{
			get
			{
				return m_fragment;
			}
			set
			{
				m_fragment = value;
			}
		}
		
		
		#endregion
		
		#region Private Methods
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data">
		///	
		///	</param>
		private void NewPacket (byte[] data)
		{
			// store the time the packet arrived
			m_stop = Environment.TickCount;
			
			// parse out the ip header
			IpV4Packet ipPacket = new IpV4Packet (data);
		
			// double check to make sure this is an icmp packet
			if (ipPacket.TransportProtocol != ProtocolType.Icmp)
			{
				return;
			}
			
			// parse out the icmp packet
			IcmpPacket icmpPacket = new IcmpPacket (ipPacket.Data);
			
			// if this is an echo reply
			if (icmpPacket.MessageType == IcmpMessageType.EchoReply)
			{
				// deserialize the packet
				IcmpEcho echo = new IcmpEcho (icmpPacket);
				
				// if this packet matches our identifier, and destination address
				if (echo.Identifier == m_id && 
					ipPacket.SourceAddress.Equals (m_remoteAddress))
				{
					// disable the timeout timer
					m_timer.Enabled = false;
				
					// raise the event
					if (PingReply != null)
					{
						PingReply (ipPacket, icmpPacket, (int)((uint)m_stop - (uint)m_start));
					}
					
					// reset the id back to 0 so we can resume sending packets.
					m_id = 0;
					
					// signal the wait event
					m_waitObject.Set ();
				}
			}
		}
		
		
		/// <summary>
		///		Occurs when the ping timed out
		/// </summary>
		private void TimeOut (object source, ElapsedEventArgs e)
		{
			// raise the event
			if (PingTimeout != null)
			{
				PingTimeout ();
			}
		
			m_id = 0;
			m_waitObject.Set ();
			m_timer.Enabled = false;
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new ping manager. This class will handle sending and recieving of
		///		a single ping packet, including matching up the correct reply and calculating
		///		the round trip time.
		/// </summary>
		/// <param name="networkInterface">
		///		The interface to send the ping packet on.
		///	</param>
		public IcmpPingManager (IPAddress networkInterface)
		{
			m_socket.SetSocketOption (SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);
			
			m_sniffer.NewPacket += new NewPacketHandler (NewPacket);
			m_sniffer.StartListening (new IPEndPoint (networkInterface, 0), SnifferModeType.Asynchronous, ProtocolType.Icmp);
			m_timer.Elapsed += new ElapsedEventHandler(TimeOut);
			m_networkInterface = networkInterface;
		}
		
				
		/// <summary>
		///		Send a new ping packet.
		/// </summary>
		/// <param name="destination">
		///		The address to send the packet to.
		///	</param>	
		/// <param name="payload">
		///		The data to send in the ping.
		///	</param>
		/// <param name="async">
		///		If this is true, SendPing will return immediately otherwise it will wait
		///	</param>
		/// <param name="timeOutTime">
		///		The number of milliseconds before the reply times out.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		If the class has been disposed then an ObjectDisposedException will be thrown.
		///	</exception>
		/// <exception cref="Exception">
		///		If the class is already waiting for a ping reply then an exception will be thrown.
		///		Use the CancelPing method first.
		///	</exception>
		public void SendPing (IPAddress destination, byte[] payload, bool async, double timeOutTime)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString(), "This object has already been disposed");
			}
			
			// check if a ping is already in process
			if (m_id != 0)
			{
				throw new Exception ("A ping request is already in process. Either wait for the reply, or cancel the request first");
			}
			
			Random random = new Random ();
			IcmpEcho echo = new IcmpEcho ();
			
			// generate random identifier and sequence number
			echo.Identifier			= (ushort)random.Next (ushort.MaxValue);
			echo.SequenceNumber		= (ushort)random.Next (ushort.MaxValue);
			
			m_id = echo.Identifier;
			m_remoteAddress = destination;
			m_timer.Interval = timeOutTime;
			
			// store the payload
			echo.Data = payload;
			
			// build the icmp header
			IcmpPacket icmpHeader = echo.Serialize (false);
			
			// build the ip header
			IpV4Packet ipHeader = new IpV4Packet ();
			
			ipHeader.TransportProtocol	= ProtocolType.Icmp;
			ipHeader.SourceAddress		= m_networkInterface;
			ipHeader.DestinationAddress	= destination;
			ipHeader.Data				= icmpHeader.Serialize ();
		
			if (m_fragment)
			{
				IpV4Packet[] fragments = ipHeader.Fragment (8);
				
				// send each fragment
				for (int i = 0; i < fragments.Length; i++)
				{
					byte[] packet = fragments[i].Serialize ();
					m_socket.SendTo (packet, 0, packet.Length, SocketFlags.None, new IPEndPoint (fragments[i].DestinationAddress, 0));
				}
			}
			else
			{
				// send the packet
				byte[] packet = ipHeader.Serialize ();
				m_socket.SendTo (packet, 0, packet.Length, SocketFlags.None, new IPEndPoint (ipHeader.DestinationAddress, 0));
			}
			
			// save the time and  start the timeout timer
			m_start = Environment.TickCount;
			m_timer.Enabled = true;

			// wait for the reply
			m_waitObject.Reset ();
			
			if (!async)
			{
				m_waitObject.WaitOne ();
			}
		}
		

		/// <summary>
		///		Send a new ping packet.
		/// </summary>
		/// <param name="destination">
		///		The address to send the packet to.
		///	</param>	
		/// <param name="payload">
		///		The data to send in the ping.
		///	</param>
		/// <param name="async">
		///		If this is true, SendPing will return immediately otherwise it will wait
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		If the class has been disposed then an ObjectDisposedException will be thrown.
		///	</exception>
		/// <exception cref="Exception">
		///		If the class is already waiting for a ping reply then an exception will be thrown.
		///		Use the CancelPing method first.
		///	</exception>
		public void SendPing (IPAddress destination, byte[] payload, bool async)
		{
			SendPing (destination, payload, async, 1000);
		}


		/// <summary>
		///		Send a new ping packet.
		/// </summary>
		/// <param name="destination">
		///		The address to send the packet to.
		///	</param>	
		/// <param name="payload">
		///		The data to send in the ping.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		If the class has been disposed then an ObjectDisposedException will be thrown.
		///	</exception>
		/// <exception cref="Exception">
		///		If the class is already waiting for a ping reply then an exception will be thrown.
		///		Use the CancelPing method first.
		///	</exception>
		public void SendPing (IPAddress destination, byte[] payload)
		{
			SendPing (destination, payload, false, 1000);
		}

		
		/// <summary>
		///		Send a new ping packet.
		/// </summary>
		/// <param name="destination">
		///		The address to send the packet to.
		///	</param>	
		/// <param name="payloadSize">
		///		The size of the payload.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		If the class has been disposed then an ObjectDisposedException will be thrown.
		///	</exception>
		/// <exception cref="Exception">
		///		If the class is already waiting for a ping reply then an exception will be thrown.
		///		Use the CancelPing method first.
		///	</exception>
		public void SendPing (IPAddress destination, int payloadSize)
		{
			string data = new string ('x', payloadSize);
			byte[] payload = System.Text.ASCIIEncoding.ASCII.GetBytes (data);
			SendPing (destination, payload, false, 1000);
		}
		
		
		/// <summary>
		///		Send a new ping packet.
		/// </summary>
		/// <param name="destination">
		///		The address to send the packet to.
		///	</param>	
		/// <exception cref="ObjectDisposedException">
		///		If the class has been disposed then an ObjectDisposedException will be thrown.
		///	</exception>
		/// <exception cref="Exception">
		///		If the class is already waiting for a ping reply then an exception will be thrown.
		///		Use the CancelPing method first.
		///	</exception>
		public void SendPing (IPAddress destination)
		{
			SendPing (destination, 32);
		}
		
		
		/// <summary>
		///		Cancel a ping request.
		/// </summary>
		public void CancelPing ()
		{
			if (m_id != 0)
			{
				m_id = 0;
				m_waitObject.Set ();
			}
		}


		/// <summary>
		///		Dispose of this class.
		/// </summary>
		public void Dispose()
		{
			m_sniffer.StopListening ();
			m_timer.Dispose ();
			m_disposed = true;
		}


		#endregion
	}
	
	
	#endregion
}