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
using Metro.LinkLayer;
using Metro.LinkLayer.Ethernet802_3;

namespace Metro.Logging
{
	#region Classes
	
	/// <summary>
	///		Provides logging for ethernet packets.
	/// </summary>
	public class EthernetLogger
	{
		#region Private Fields
		
		/// <summary>
		///		The packet logger to use.
		/// </summary>
		PacketLogger m_logger;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new ethernet  logger.
		/// </summary>
		/// <param name="logger">
		///		The logger object to use.
		///	</param>
		public EthernetLogger (PacketLogger logger)
		{
			m_logger = logger;
		}
		
		
		/// <summary>
		///		Log a new ethernet packet.
		/// </summary>
		/// <param name="packet">
		///		The packet to log.
		///	</param>
		public void LogPacket (Ethernet802_3 packet)
		{
			lock (m_logger.XmlWriter)
			{
				// <EthernetHeader>
				m_logger.XmlWriter.WriteStartElement ("EthernetHeader");
					
					m_logger.XmlWriter.WriteElementString ("SourceMAC",			packet.SourceMACAddress.ToString('-'));
					m_logger.XmlWriter.WriteElementString ("DestinationMAC",	packet.DestinationMACAddress.ToString('-'));
					m_logger.XmlWriter.WriteElementString ("NetworkProtocol",	packet.NetworkProtocol.ToString());
					
				m_logger.XmlWriter.WriteEndElement ();
				// </EthernetHeader>

			}
		}
		
		
		#endregion
	}
	
	#endregion
}