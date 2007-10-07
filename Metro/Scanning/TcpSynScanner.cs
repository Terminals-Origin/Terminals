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
using System.Timers;
using Metro.NetworkLayer.IpV4;
using Metro.TransportLayer.Tcp;

namespace Metro.Scanning
{
	#region Classes
	
	/// <summary>
	///		The TCP SYN scan technique is often referred to as "half-open"
	///		scanning, because you don't open a full TCP connection. You send
	///		a SYN packet, as if you are going to open a real connection and
	///		you wait for a response. A SYN|ACK indicates the port is listening. 
	///		A RST is indicative of a non-listener. No response means the port is
	///		probably filtered by a router or firewall. If a SYN|ACK is received,  
	///		a RST is immediately sent to tear down the connection (actually our 
	///		OS kernel does this for us). The primary advantage to this scanning  
	///		technique  is  that fewer sites will log it.
	/// </summary>
	public class TcpSynScanner : IDisposable
	{
		#region Events
		
		/// <summary>
		///		This event occurs when a ports status has been going.
		/// </summary>
		public event TcpPortReplyHandler PortReply;
	
		/// <summary>
		///		This event is raised when te trace is complete.
		/// </summary>
		public event TcpPortScanComplete ScanComplete;
	
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
		///		The local interface to use when sending packets.
		/// </summary>
		private IPEndPoint m_localEndPoint = new IPEndPoint(IPAddress.Any, 0);
		
		/// <summary>
		///		The remote end point to send packets to.
		/// </summary>
		private IPEndPoint m_remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
		
		/// <summary>
		///		The timer used for time out of replies.
		/// </summary>
		private Timer m_timeoutTimer = new Timer ();

		/// <summary>
		///		The timer used to time sending of packets so we don't accidentally
		///		SYN flood.
		/// </summary>
		private Timer m_sendTimer;

		/// <summary>
		///		The interval used for the send timer.
		/// </summary>
		private int m_sendInterval = 500;

		/// <summary>
		///		The array of ports to scan.
		/// </summary>
		private ushort[] m_ports;
		
		/// <summary>
		///		The current index in the port array.
		/// </summary>
		private int m_portIndex = 0;

		/// <summary>
		///		The wait object is used in blocking mode.
		/// </summary>
		private System.Threading.AutoResetEvent m_waitObject = new System.Threading.AutoResetEvent (false);

		/// <summary>
		///		Whether or not the scanner is running.
		/// </summary>
		private bool m_working = false;
		
		/// <summary>
		///		If set to true, the local port will be incremented each time a new request
		///		is sent.
		/// </summary>
		private bool m_incPorts = true;
		
		/// <summary>
		///		If true, packets will be fragmented.
		/// </summary>
		private bool m_fragment = false;
		
		/// <summary>
		///		The private socket for sending the packets with.
		/// </summary>
		private Socket m_socket = new Socket (AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
		
		#endregion

		#region Public Fields
		
		/// <summary>
		///		Whether or not a scan is currently running.
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
				return (int)m_timeoutTimer.Interval;
			}
			set
			{
				m_timeoutTimer.Interval = (double)value;
			}
		}
		
		
		/// <summary>
		///		The time to wait before sending each request. A very small time is more
		///		easily detectable, and may accidentally "SYN flood" the remote host. It will
		///		scan faster though. A larger time will be more stealthy, and will not be
		///		quite so resource consuming. But it will take longer.
		/// </summary>
		public int SendInterval
		{
			get
			{
				return m_sendInterval;
			}
			set
			{
				m_sendInterval = value;
			}
		}
		
		
		/// <summary>
		///		If true, the local port numbers will be incremented for each packet sent.
		/// </summary>
		public bool IncrementLocalPorts
		{
			get
			{
				return m_incPorts;
			}
			set
			{
				m_incPorts = value;
			}
		}
		
		
		/// <summary>
		///		If true, IP packets will be fragmented.
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
		///		Whether or not the class has been disposed.
		/// </summary>
		public bool Disposed
		{
			get
			{
				return m_disposed;
			}
		}
		
		
		#endregion

		#region private Methods
		
		/// <summary>
		///		Send a SYN request to the next port to check it's state.
		/// </summary>
		private void SendRequest ()
		{
			m_remoteEndPoint.Port = m_ports [m_portIndex];
			
			// increment the local port.
			if (m_incPorts)
			{	
				if (m_localEndPoint.Port == ushort.MaxValue)
				{
					m_localEndPoint.Port = 1024;
				}
				
				m_localEndPoint.Port++;
			}
		
			IpV4Packet ipHeader = new IpV4Packet ();
			
			// fill in the minimal IP fields
			ipHeader.DestinationAddress = m_remoteEndPoint.Address;
			ipHeader.SourceAddress		= m_localEndPoint.Address;
			
			TcpPacket tcpHeader = new TcpPacket ();
			
			// fill in the minimal tcp fields
			tcpHeader.DestinationPort	= (ushort)m_remoteEndPoint.Port;
			tcpHeader.SourcePort		= (ushort)m_localEndPoint.Port;
			tcpHeader.SetFlag (TcpFlags.Synchronize, true);
			
			// send the request
			ipHeader.Data = tcpHeader.Serialize (m_localEndPoint.Address ,m_remoteEndPoint.Address);
			
			// send each fragment
			if (m_fragment)
			{
				IpV4Packet[] fragments = ipHeader.Fragment (8);
				
				for (int i = 0; i < fragments.Length; i++)
				{
					byte[] packet = fragments[i].Serialize ();
					m_socket.SendTo (packet, 0, packet.Length, SocketFlags.None, new IPEndPoint (fragments[i].DestinationAddress, 0));
				}
			}
			
			// send the packet
			else
			{
				byte[] packet = ipHeader.Serialize ();
				m_socket.SendTo (packet, 0, packet.Length, SocketFlags.None, new IPEndPoint (ipHeader.DestinationAddress, 0));
			}

			// start the time out timer
			m_timeoutTimer.Enabled = true;
		}

		
		/// <summary>
		///		The timer event for sending the next packet.
		/// </summary>
		/// <param name="source">
		///		Source.
		///	</param>
		/// <param name="e">
		///		Event args.
		///	</param>
		private void SendTimer (object source, ElapsedEventArgs e)
		{
			m_sendTimer.Enabled = false;
			SendRequest ();
		}
		
		
		/// <summary>
		///		The timer event for the time out. If this triggers, we assume
		///		the port is filtered.
		/// </summary>
		/// <param name="source">
		///		Source.
		///	</param>
		/// <param name="e">
		///		Event args.
		///	</param>
		private void TimeOut (object source, ElapsedEventArgs e)
		{	
			m_timeoutTimer.Enabled = false;
		
			if (!m_working)
			{
				return;
			}
			
			if (PortReply != null)
			{
				PortReply (m_remoteEndPoint, TcpPortState.Filtered);
			}
			
			// increment the port
			m_portIndex++;
			
			// check to see if the port scan is complete
			if (m_portIndex == m_ports.Length)
			{
				if (ScanComplete != null)
				{
					ScanComplete ();
				}
				
				m_sendTimer = null;
				m_working = false;
				
				m_waitObject.Set ();
				
				return;
			}
			
			// check the next port
			if (m_sendTimer == null)
			{
				SendRequest ();
			}
			else
			{
				m_sendTimer.Enabled = true;
			}
		}
		
		
		/// <summary>
		///		A new packet has arrived.
		/// </summary>
		/// <param name="data">
		///		The packet.
		///	</param>
		private void NewPacket (byte[] data)
		{
			if (!m_working)
			{
				return;
			}
			
			IpV4Packet ipHeader = new IpV4Packet (data);

			// check to see if this packet doesn't belong to us
			if ((!ipHeader.SourceAddress.Equals(m_remoteEndPoint.Address)) ||
			    (!ipHeader.DestinationAddress.Equals(m_localEndPoint.Address)))
			{
				return;
			}
			
			
			TcpPacket tcpHeader = new TcpPacket (ipHeader.Data);
			
			// check to see if this packet doesn't belong to us
			if (tcpHeader.SourcePort != (ushort)m_remoteEndPoint.Port ||
				tcpHeader.DestinationPort != (ushort)m_localEndPoint.Port)
			{
				return;
			}
			
			
			// disable the timeout timer
			m_timeoutTimer.Enabled = false;
			
			
			// if this is a reset packet
			if (tcpHeader.IsFlagSet (TcpFlags.Reset))
			{
				// then the port is closed
				if (PortReply != null)
				{
					PortReply (m_remoteEndPoint, TcpPortState.Closed);
				}
			}
			
			// if it's a syn ack packet
			else if (tcpHeader.IsFlagSet (TcpFlags.Synchronize) && tcpHeader.IsFlagSet (TcpFlags.Acknowledgment))
			{
				// then the port is opened
				if (PortReply != null)
				{
					PortReply (m_remoteEndPoint, TcpPortState.Opened);
				}
				
				// at this point the OS will send back a reset packet to close the connection
			}
			
			// increment the port
			m_portIndex++;
			
			// check to see if the port scan is complete
			if (m_portIndex == m_ports.Length)
			{
				if (ScanComplete != null)
				{
					ScanComplete ();
				}
				
				m_sendTimer = null;
				m_working = false;
				
				m_waitObject.Set ();
				
				return;
			}
			
			// check the next port
			if (m_sendTimer == null)
			{
				SendRequest ();
			}
			else
			{
				m_sendTimer.Enabled = true;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new TCP SYN scanner
		/// </summary>
		/// <param name="networkInterface">
		///		The local network interface to use.
		///	</param>
		public TcpSynScanner (IPEndPoint networkInterface)
		{
			m_socket.SetSocketOption (SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);
			
			m_sniffer.NewPacket += new NewPacketHandler (NewPacket);
			m_sniffer.StartListening (new IPEndPoint (networkInterface.Address, 0), SnifferModeType.Asynchronous, ProtocolType.IP);
			m_timeoutTimer.Elapsed += new ElapsedEventHandler (TimeOut);
			m_localEndPoint	 = networkInterface;
			m_timeoutTimer.Interval = 1000.0;
		}
		
		
		/// <summary>
		///		Start a new SYN scan. This type of scan is not detected by some sites
		///		but most firewalls will easily pick it up. It is also commonly known as a
		///		"half open" scan.
		/// </summary>
		/// <param name="remoteAddress">
		///		The remote address to scan.
		///	</param>
		/// <param name="ports">
		///		An array of ports to scan.
		///	</param>
		/// <param name="timeoutTime">
		///		The time to wait before giving up waiting on a reply from a port.
		///		If a time out occurs, it is assumed that the port is filtered.
		///		A small time out will result in faster scanning, but with less accuracy 
		///		on slower connections, where a larger time out will result in more accurate
		///		results on slower connections, but will take longer.
		///	</param>
		/// <param name="waitTime">
		///		The time to wait before sending each request. A very small time is more
		///		easily detectable, and may accidentally "SYN flood" the remote host. It will
		///		scan faster though. A larger time will be more stealthy, and will not be
		///		quite so resource consuming. But it will take longer.
		///	</param>
		/// <param name="async">
		///		Whether or not to work in async or blocking mode.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		/// <exception cref="Exception">
		///			An exception may occur if a scan is already in progress.
		///	</exception>
		public void StartScan (IPAddress remoteAddress, ushort[] ports, int timeoutTime, int waitTime, bool async)
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString(), "This object has already been disposed");
			}
		
			if (m_working)
			{
				throw new Exception ("There is alredy a scan in progress");
			}
		
			m_remoteEndPoint = new IPEndPoint (remoteAddress, ports[0]);
			m_ports = ports;
			
			m_timeoutTimer.AutoReset = false;
			m_timeoutTimer.Interval = (double)timeoutTime;
		
			if (waitTime > 0)
			{
				m_sendTimer = new Timer ((double)waitTime);
				m_sendTimer.AutoReset = false;
				m_sendTimer.Elapsed += new ElapsedEventHandler (SendTimer);
			}
			
			m_working = true;
			SendRequest ();
			
			// wait, if we are in blocking mode
			m_waitObject.Reset ();
			
			if (!async)
			{
				m_waitObject.WaitOne ();
			}
		}
		
		
		/// <summary>
		///		Start a new SYN scan. This type of scan is not detected by some sites
		///		but most firewalls will easily pick it up. It is also commonly known as a
		///		"half open" scan.
		/// </summary>
		/// <param name="remoteAddress">
		///		The remote address to scan.
		///	</param>
		/// <param name="ports">
		///		An array of ports to scan.
		///	</param>
		/// <param name="timeoutTime">
		///		The time to wait before giving up waiting on a reply from a port.
		///		If a time out occurs, it is assumed that the port is filtered.
		///		A small time out will result in faster scanning, but with less accuracy 
		///		on slower connections, where a larger time out will result in more accurate
		///		results on slower connections, but will take longer.
		///	</param>
		/// <param name="waitTime">
		///		The time to wait before sending each request. A very small time is more
		///		easily detectable, and may accidentally "SYN flood" the remote host. It will
		///		scan faster though. A larger time will be more stealthy, and will not be
		///		quite so resource consuming. But it will take longer.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		/// <exception cref="Exception">
		///			An exception may occur if a scan is already in progress.
		///	</exception>
		public void StartScan (IPAddress remoteAddress, ushort[] ports, int timeoutTime, int waitTime)
		{
			StartScan (remoteAddress, ports, timeoutTime, waitTime, false);
		}
		
		
		/// <summary>
		///		Start a new SYN scan. This type of scan is not detected by some sites
		///		but most firewalls will easily pick it up. It is also commonly known as a
		///		"half open" scan.
		/// </summary>
		/// <param name="remoteAddress">
		///		The remote address to scan.
		///	</param>
		/// <param name="ports">
		///		An array of ports to scan.
		///	</param>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		/// <exception cref="Exception">
		///			An exception may occur if a scan is already in progress.
		///	</exception>
		public void StartScan (IPAddress remoteAddress, ushort[] ports)
		{
			StartScan (remoteAddress, ports, (int)m_timeoutTimer.Interval, m_sendInterval, false);
		}
		
		
		/// <summary>
		///		Cancel a running port scan.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		///		An ObjectDisposedException will occur if this class has already been disposed. 
		///	</exception>
		public void CancelScan ()
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException (this.ToString(), "This object has already been disposed");
			}
		
			if (m_working)
			{
				m_working = false;
			
				// disable all timers
				m_timeoutTimer.Enabled = false;
			
				if (m_sendTimer != null)
				{
					m_sendTimer.Enabled = false;
					m_sendTimer = null;
				}
				
				// raise the event
				if (ScanComplete != null)
				{
					ScanComplete ();
				}
				
				// set the wait object
				m_waitObject.Set ();
			}
		}
		
		
		/// <summary>
		///		Dispose.
		/// </summary>
        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }
               
		#endregion
		
		#region Private Methods
		
		/// <summary>
		///		Dispose.
		/// </summary>
		/// <param name="disposing">
		///		If disposing equals true, the method has been called directly
        ///		or indirectly by a user's code. Managed and unmanaged resources
        ///		can be disposed.
        ///		If disposing equals false, the method has been called by the 
        ///		runtime from inside the finalizer and you should not reference 
        ///		other objects. Only unmanaged resources can be disposed.
		///	</param>
        private void Dispose (bool disposing)
        {
			// Check to see if Dispose has already been called.
            if (!m_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
					// clean up managed resources
					if (m_working)
					{
						CancelScan ();
					}
				
					m_sniffer.StopListening ();
					
					// dispose of timers
					m_timeoutTimer.Dispose ();
					
					if (m_sendTimer != null)
					{
						m_sendTimer.Dispose ();
					}
				}
				
				// clean up unmanaged resources
			}
			
			m_disposed = true;
		}
		
		
		/// <summary>
		///		Destructor.
		/// </summary>
		~TcpSynScanner ()
		{
			Dispose (false);
		}
		
		
		#endregion
	}
	
	
	#endregion
}