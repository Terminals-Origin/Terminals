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
using Metro.NetworkLayer;
using Metro.NetworkLayer.ARP;

namespace Metro.Logging
{
	#region Classes
	
	/// <summary>
	///		Provides logging for ARP packets.
	/// </summary>
	public class ArpLogger
	{
		#region Private Fields
		
		/// <summary>
		///		The packet logger to use.
		/// </summary>
		PacketLogger m_logger;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new ARP logger.
		/// </summary>
		/// <param name="logger">
		///		The logger object to use.
		///	</param>
		public ArpLogger (PacketLogger logger)
		{
			m_logger = logger;
		}
		
		
		/// <summary>
		///		Log a new ARP packet.
		/// </summary>
		/// <param name="packet">
		///		The packet to log.
		///	</param>
		public void LogPacket (ArpPacket packet)
		{
			lock (m_logger.XmlWriter)
			{
				// <ArpHeader>
				m_logger.XmlWriter.WriteStartElement ("ArpHeader");
					
					m_logger.XmlWriter.WriteElementString ("Type",				packet.Type.ToString());
					m_logger.XmlWriter.WriteElementString ("MediaType",			packet.MediaType.ToString());
					m_logger.XmlWriter.WriteElementString ("Protocol",			packet.Protocol.ToString());
					m_logger.XmlWriter.WriteElementString ("SourceMAC",			packet.SourceMACAddress.ToString('-'));
					m_logger.XmlWriter.WriteElementString ("SourceIP",			packet.SourceIPAddress.ToString());
					m_logger.XmlWriter.WriteElementString ("TargetMAC",			packet.DestinationMACAddress.ToString('-'));
					m_logger.XmlWriter.WriteElementString ("TargetIP",			packet.DestinationIPAddress.ToString());
					
				m_logger.XmlWriter.WriteEndElement ();
				// </ArpHeader>

			}
		}
		
		
		#endregion
	}
	
	#endregion
}