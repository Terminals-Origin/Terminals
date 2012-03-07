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
using Metro.NetworkLayer.IpV4;
using System.Timers;
using System.Threading;

namespace Metro.TransportLayer.Icmp
{	
	#region Delegates
	
	/// <summary>
	///		The handler for a route update event.
	/// </summary>
	public delegate void RouteUpdateHandler (IPAddress gateway, int roundTripTime, byte currentHop);
	
	/// <summary>
	///		The handler for the hops exceeded event.
	/// </summary>
	public delegate void MaxHopsExceededHandler ();
	
	/// <summary>
	///		The handler for the trace finished event.
	/// </summary>
	public delegate void TraceFinishedHandler ();
	
	#endregion

	#region Classes
	
	/// <summary>
	/// 
	/// </summary>
	public class IcmpTraceRoute : IDisposable
	{
		#region Events
		
		/// <summary>
		///		This event occurs whenever the next hop is found.
		/// </summary>
		public event RouteUpdateHandler RouteUpdate;
		
		/// <summary>
		///		This event occurs when the mex hop count is reached.
		/// </summary>
		public event MaxHopsExceededHandler MaxHopsExceeded;
		
		/// <summary>
		///		This event occurs when the trace is complete.
		/// </summary>
		public event TraceFinishedHandler TraceFinished;
		
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
		///		Ip identification.
		/// </summary>
		private ushort m_ipId = 0;

		/// <summary>
		///		The remote address of the current ping packet is stores so that we can match
		///		its reply.
		/// </summary>
		private IPAddress m_remoteAddress = IPAddress.Any;
		
		/// <summary>
		///		The local network interface.
		/// </summary>
		private IPAddress m_localAddress = IPAddress.Any;
		
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
		///		Store the time to live.
		/// </summary>
		private byte m_ttl = 1;

		/// <summary>
		///		The maximum number of hops.
		/// </summary>
		private byte m_maxHops = 30;

		/// <summary>
		///		The number of milliseconds that must pass before
		///		a request times out.
		/// </summary>
		private int m_timeoutTime = 2000;

		/// <summary>
		///		Create a new timer used for timeouts.
		/// </summary>
		private System.Timers.Timer m_timer = new System.Timers.Timer ();

		/// <summary>
		///		Whether or not the trace is running.
		/// </summary>
		private bool m_working = false;

		/// <summary>
		///		Whether or not to fragment IP packets.
		/// </summary>
		private bool m_fragment = false;

		/// <summary>
		///		The route to take.
		/// </summary>
		private IpV4RoutingOption m_route;
		
		/// <summary>
		///		If strict rource routing is turned on the the exact route specified
		///		will be taken.
		/// </summary>
		private bool m_strictRouting = false;
		
		/// <summary>
		///		The private socket for sending the packets with.
		/// </summary>
		private Socket m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Whether or not a trace is currently running.
		/// </summary>
		public bool Running
		{
			get
			{
				return m_working;
			}
		}
		
		
		/// <summary>
		///		The time out time.
		/// </summary>
		public int TimeOutInterval
		{
			get
			{
				return m_timeoutTime;
			}
			set
			{
				m_timeoutTime = value;
			}
		}
		
		
		/// <summary>
		///		The maximum number of hops.
		/// </summary>
		public byte MaxHops
		{
			get
			{
				return m_maxHops;
			}
			set
			{
				m_maxHops = value;
			}
		}
				
				
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
		
		
		/// <summary>
		///		If strict rource routing is turned on the the exact route specified
		///		will be taken.
		/// </summary>
		public bool UseStrictRouting
		{
			get
			{	
				return m_strictRouting;
			}
			set
			{
				m_strictRouting = value;
			}
		}
		
		
		/// <summary>
		///		The route to take.
		/// </summary>
		public IpV4RoutingOption Route
		{
			get
			{
				return m_route;
			}
			set
			{
				m_route = value;
			}
		}
		
		
		#endregion
		
		#region Private Methods

		/// <summary>
		///		Send the next request in order to find the next hop.
		/// </summary>
		private void SendRequest ()
		{
			// generate a 32 byte payload
			string data = new string ('x', 32);
			byte[] payload = System.Text.ASCIIEncoding.ASCII.GetBytes (data);
			
			// fill in ip header fields
			IpV4Packet ipPacket = new IpV4Packet ();
			ipPacket.DestinationAddress = m_remoteAddress;
			ipPacket.SourceAddress		= m_localAddress;
			ipPacket.TransportProtocol	= ProtocolType.Icmp;
			ipPacket.TimeToLive = m_ttl;
			
			// save the identification
			m_ipId = ipPacket.Identification;
			
			// add routing options if any
			if (m_route != null)
			{
				ipPacket.Options = new IpV4Option[2];
				
				ipPacket.Options[0] = m_route.Serialize (m_strictRouting ? IpV4OptionNumber.StrictSourceRouting : IpV4OptionNumber.LooseSourceRouting);
				
				ipPacket.Options[1] = new IpV4Option ();
				ipPacket.Options[1].OptionType = IpV4OptionNumber.EndOfOptions;
				ipPacket.Options[1].IsCopied = false;
				ipPacket.Options[1].Length = 1;
				ipPacket.Options[1].Class = IpV4OptionClass.Control;
				ipPacket.Options[1].Data = null;
			}
			
			// create a new echo packet
			IcmpEcho echo = new IcmpEcho ();
			echo.Identifier = m_id;
			echo.Data = payload;
			
			// serialize the icmp packet
			IcmpPacket icmpPacket = echo.Serialize (false);
			ipPacket.Data = icmpPacket.Serialize ();
			
			
			if (m_fragment)
			{
				IpV4Packet[] fragments = ipPacket.Fragment (8);
				
				// send each fragment
				for (int i = 0; i < fragments.Length; i++)
				{
					byte[] packet = fragments[i].Serialize ();
					m_socket.SendTo (packet, 0, packet.Length, SocketFlags.None, new IPEndPoint (fragments[i].DestinationAddress, 0));
				}
			}
			else
			{
				// send the ip packet
				byte[] packet = ipPacket.Serialize ();
				m_socket.SendTo (packet, 0, packet.Length, SocketFlags.None, new IPEndPoint (ipPacket.DestinationAddress, 0));
			}
			
			
			// grab the current time
			m_start = Environment.TickCount;
			
			// start the timeout timer
			m_timer.Enabled = true;
		}
		

		/// <summary>
		///		This event occurs whenever a new packet arrives.
		/// </summary>
		/// <param name="data">
		///		The data in the packet.
		///	</param>
		private void NewPacket (byte[] data)
		{
			if (!m_working)
			{
				return;
			}
		
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
				#region Reply From Destination
			
				// deserialize the packet
				IcmpEcho echo = new IcmpEcho (icmpPacket);
				
				// if this packet matches our identifier, and destination address
				if (echo.Identifier == m_id)
				{
					// disable the timeout timer
					m_timer.Enabled = false;
					
					// raise the event
					if (RouteUpdate != null)
					{
						RouteUpdate (ipPacket.SourceAddress, (int)((uint)m_stop - (uint)m_start), m_ttl);
					}
					
					// raise the event
					if (TraceFinished != null)
					{
						TraceFinished ();
					}
					
					// signal the wait event
					m_waitObject.Set ();
					
					// allow new trace routes
					m_working = false;
				}
				
				#endregion
			}
			
			// if the time to live exceeded then we have a new hop
			else if (icmpPacket.MessageType == IcmpMessageType.TimeExceeded)
			{		
				#region Reply From Router

				IcmpTimeExceeded icmpTimeExceeded = new IcmpTimeExceeded (icmpPacket);
				
				// make sure this packet is for us
				if (m_ipId != icmpTimeExceeded.BadPacket.Identification)
				{
					return;
				}

				// disable the timeout timer
				m_timer.Enabled = false;
			
				// raise the event
				if (RouteUpdate != null && m_working)
				{
					RouteUpdate (ipPacket.SourceAddress, (int)((uint)m_stop - (uint)m_start), m_ttl);
				}
				
				// increment the time to live.
				m_ttl++;
				
				// if the max hop count is exceeded
				if (m_ttl > m_maxHops)
				{
					// disable the timeout timer
					m_timer.Enabled = false;
				
					// allow new trace routes
					m_working = false;
					
					// raise the event
					if (MaxHopsExceeded != null)
					{
						MaxHopsExceeded ();
					}
					
					// signal the wait event
					m_waitObject.Set ();
				}
				else
				{
					// send the next request
					SendRequest ();
				}
				
				#endregion
			}
		
		}
		
		
		/// <summary>
		///		Occurs when the ping timed out
		/// </summary>
		/// <param name="source">
		///	
		///	</param>
		/// <param name="e">
		///	
		///	</param>
		private void TimeOut (object source, ElapsedEventArgs e)
		{

			// disable the timer
			m_timer.Enabled = false;
			

			// raise the event
			if (RouteUpdate != null)
			{
				RouteUpdate (IPAddress.None, -1, m_ttl);
			}
			
			// increment the time to live.
			m_ttl++;
				
			// if the max hop count is exceeded
			if (m_ttl > m_maxHops)
			{
				// disable the timeout timer
				m_timer.Enabled = false;
			
				// allow new trace routes
				m_working = false;
				
				// raise the event
				if (MaxHopsExceeded != null)
				{
					MaxHopsExceeded ();
				}
				
				// signal the wait event
				m_waitObject.Set ();
			}
			else
			{
				SendRequest ();
			}
		}
		
		
		#endregion
		
		#region Public methods
		
		/// <summary>
		///		Create a new IcmpTraceRoute class for performing route traces to hosts using
		///		ICMP ping packets.
		/// </summary>
		/// <param name="networkInterface">
		///		The local network interface to use.
		///	</param>
		public IcmpTraceRoute (IPAddress networkInterface)
		{
			m_socket.SetSocketOption (SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);
			
			m_sniffer.NewPacket += new NewPacketHandler (NewPacket);
			m_sniffer.StartListening (new IPEndPoint (networkInterface, 0), SnifferModeType.Asynchronous, ProtocolType.Icmp);
			m_timer.Elapsed += new ElapsedEventHandler(TimeOut);
			
			m_localAddress	 = networkInterface;
		}
		
		
		/// <summary>
		///		Perform a new route trace.
		/// </summary>
		/// <param name="destination">
		///		The remote host to trace the route to.
		///	</param>
		/// <param name="async">
		///		Whether or not to use blocking mode. If this is true then the
		///		method will not block.
		///	</param>
		/// <param name="timeOutTime">
		///		Time time out time.
		///	</param>
		/// <param name="maxHops">
		///		The maximum number of hops.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		/// <exception cref="Exception">
		///			An exception may occur if a trace is already in progress.
		///	</exception>
		public void TraceRoute (IPAddress destination, bool async, int timeOutTime, byte maxHops)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString(), "This object has already been disposed");
			}
		
			if (m_working)
			{
				throw new Exception ("There is alredy a traceroute in progress");
			}
		
			Random random	 = new Random ();
			
			m_timer.Interval = timeOutTime;
			
			// store state variables
			m_timeoutTime	 = timeOutTime;
			m_maxHops		 = maxHops;
			m_ttl			 = 1;
			m_id			 = (ushort)random.Next (ushort.MaxValue);
			m_remoteAddress  = destination;
			m_working		 = true;
			
			// send the first request
			SendRequest ();
			
			// wait, if we are in blocking mode
			m_waitObject.Reset ();
			
			if (!async)
			{
				m_waitObject.WaitOne ();
			}
		}
		
		
		/// <summary>
		///		Perform a new route trace.
		/// </summary>
		/// <param name="destination">
		///		The remote host to trace the route to.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		/// <exception cref="Exception">
		///			An exception may occur if a trace is already in progress.
		///	</exception>
		public void TraceRoute (IPAddress destination)
		{
			TraceRoute (destination, false, m_timeoutTime, m_maxHops);
		}
		
		
		/// <summary>
		///		Cancel a current trace.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		public void CancelTrace ()
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString(), "This object has already been disposed");
			}
		
			m_working = false;
			m_timer.Enabled = false;
			
			// raise the event
			if (TraceFinished != null)
			{
				TraceFinished ();
			}
			
			m_waitObject.Set ();
		}
		
		
		/// <summary>
		///		Dispose of this class.
		/// </summary>
		public void Dispose()
		{
			if (m_working)
			{
				CancelTrace ();
			}
		
			m_sniffer.StopListening ();
			m_timer.Dispose ();
			m_disposed = true;
		}


		#endregion
	}
	
	
	#endregion
}