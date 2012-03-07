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
using System.IO;
using System.Xml;
using Metro.TransportLayer;
using Metro.TransportLayer.Udp;

namespace Metro.Logging
{
	#region Classes
	
	/// <summary>
	///		Provides logging for UDP packets.
	/// </summary>
	public class UdpLogger
	{
		#region Private Fields
		
		/// <summary>
		///		The packet logger to use.
		/// </summary>
		PacketLogger m_logger;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new UDP logger.
		/// </summary>
		/// <param name="logger">
		///		The logger object to use.
		///	</param>
		public UdpLogger (PacketLogger logger)
		{
			m_logger = logger;
		}
		
		
		/// <summary>
		///		Log a new UDP packet.
		/// </summary>
		/// <param name="packet">
		///		The packet to log.
		///	</param>
		public void LogPacket (UdpPacket packet)
		{
			lock (m_logger.XmlWriter)
			{
				// <UdpHeader>
				m_logger.XmlWriter.WriteStartElement ("UdpHeader");
				
					m_logger.XmlWriter.WriteElementString ("SourcePort",		packet.SourcePort.ToString());
					m_logger.XmlWriter.WriteElementString ("DestinationPort",	packet.DestinationPort.ToString());
					m_logger.XmlWriter.WriteElementString ("Checksum",			packet.Checksum.ToString());

					// <LengthFields>
					m_logger.XmlWriter.WriteStartElement ("UdpLengthFields");
						
						int dataLength = (packet.Data != null ? packet.Data.Length : 0);

						m_logger.XmlWriter.WriteElementString ("TotalLength",	packet.Length + " bytes");
						m_logger.XmlWriter.WriteElementString ("HeaderLength",	(packet.Length - dataLength) + " bytes");
						m_logger.XmlWriter.WriteElementString ("DataLength",	dataLength + " bytes");
					m_logger.XmlWriter.WriteEndElement ();
					// </LengthFields>
					
				m_logger.XmlWriter.WriteEndElement ();
				// </UdpHeader>
			
			}
		}
		
		
		#endregion
	}
	
	#endregion
}