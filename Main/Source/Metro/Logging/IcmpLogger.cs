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
using Metro.TransportLayer.Icmp;

namespace Metro.Logging
{
	#region Classes
	
	/// <summary>
	///		Provides logging for ICMP packets.
	/// </summary>
	public class IcmpLogger
	{
		#region Private Fields
		
		/// <summary>
		///		The packet logger to use.
		/// </summary>
		PacketLogger m_logger;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new ICMP logger.
		/// </summary>
		/// <param name="logger">
		///		The logger object to use.
		///	</param>
		public IcmpLogger (PacketLogger logger)
		{
			m_logger = logger;
		}
		
		
		/// <summary>
		///		Log a new ICMP packet.
		/// </summary>
		/// <param name="packet">
		///		The packet to log.
		///	</param>
		public void LogPacket (IcmpPacket packet)
		{
			lock (m_logger.XmlWriter)
			{
				// <IcmpHeader>
				m_logger.XmlWriter.WriteStartElement ("IcmpHeader");
				
					m_logger.XmlWriter.WriteElementString ("Type",		packet.MessageType.ToString());
					m_logger.XmlWriter.WriteElementString ("Checksum",	packet.Checksum.ToString());

					#region Specific ICMP Types
					
					switch (packet.MessageType)
					{
						case IcmpMessageType.DestinationUnreachable:
							IcmpDestinationUnreachable option1 = new IcmpDestinationUnreachable (packet);
							m_logger.XmlWriter.WriteElementString ("Code",	 option1.Code.ToString());
							break;
						case IcmpMessageType.Echo:
							IcmpEcho option2 = new IcmpEcho (packet);
							m_logger.XmlWriter.WriteElementString ("Identifier",		option2.Identifier.ToString());
							m_logger.XmlWriter.WriteElementString ("SequenceNumber",	option2.SequenceNumber.ToString());
							m_logger.XmlWriter.WriteElementString ("DataLength",		(option2.Data != null ? option2.Data.Length : 0) + " bytes");
							m_logger.XmlWriter.WriteElementString ("Data",				(option2.Data != null ? System.Text.ASCIIEncoding.ASCII.GetString (option2.Data) : string.Empty));
							break;
						case IcmpMessageType.EchoReply:
							IcmpEcho option3 = new IcmpEcho (packet);
							m_logger.XmlWriter.WriteElementString ("Identifier",		option3.Identifier.ToString());
							m_logger.XmlWriter.WriteElementString ("SequenceNumber",	option3.SequenceNumber.ToString());
							m_logger.XmlWriter.WriteElementString ("DataLength",		(option3.Data != null ? option3.Data.Length : 0) + " bytes");
							m_logger.XmlWriter.WriteElementString ("Data",				(option3.Data != null ? System.Text.ASCIIEncoding.ASCII.GetString (option3.Data) : string.Empty));
							break;
						case IcmpMessageType.ParameterProblem:
							IcmpParameterProblem option4 = new IcmpParameterProblem (packet);
							m_logger.XmlWriter.WriteElementString ("Pointer",			option4.ErrorPointer.ToString());
							break;
						case IcmpMessageType.Redirect:
							IcmpRedirect option5 = new IcmpRedirect (packet);
							m_logger.XmlWriter.WriteElementString ("Gateway",			option5.GatewayAddress.ToString());
							break;
						case IcmpMessageType.SourceQuench:
							break;
						case IcmpMessageType.TimeExceeded:
							break;
						case IcmpMessageType.TimeStamp:
							break;
						case IcmpMessageType.TimestampReply:
							break;
					}
					
					#endregion
					
				m_logger.XmlWriter.WriteEndElement ();
				// </IcmpHeader>
			
			}
		}
		
		
		#endregion
	}
	
	#endregion
}