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
using Metro.TransportLayer.Tcp;

namespace Metro.Logging
{
	#region Classes
	
	/// <summary>
	///		Provides logging for TCP packets.
	/// </summary>
	public class TcpLogger
	{
		#region Private Fields
		
		/// <summary>
		///		The packet logger to use.
		/// </summary>
		PacketLogger m_logger;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new TCP  logger.
		/// </summary>
		/// <param name="logger">
		///		The logger object to use.
		///	</param>
		public TcpLogger (PacketLogger logger)
		{
			m_logger = logger;
		}
		
		
		/// <summary>
		///		Log a new TCP packet.
		/// </summary>
		/// <param name="packet">
		///		The packet to log.
		///	</param>
		public void LogPacket (TcpPacket packet)
		{
			lock (m_logger.XmlWriter)
			{
				// <TcpHeader>
				m_logger.XmlWriter.WriteStartElement ("TcpHeader");
				
					m_logger.XmlWriter.WriteElementString ("SourcePort",		packet.SourcePort.ToString());
					m_logger.XmlWriter.WriteElementString ("DestinationPort",	packet.DestinationPort.ToString());
					m_logger.XmlWriter.WriteElementString ("SequenceNumber",	packet.SequenceNumber.ToString());
					m_logger.XmlWriter.WriteElementString ("AcknowledgmentNumber",packet.AcknowledgmentNumber.ToString());
					m_logger.XmlWriter.WriteElementString ("Window",			packet.Window.ToString());
					m_logger.XmlWriter.WriteElementString ("UrgentPointer",		packet.UrgentPointer.ToString());
					m_logger.XmlWriter.WriteElementString ("Checksum",			packet.Checksum.ToString());
				
					// <LengthFields>
					m_logger.XmlWriter.WriteStartElement ("TcpLengthFields");
						
						int dataLength = (packet.Data != null ? packet.Data.Length : 0);
						int paddingLength = (packet.Padding != null ? packet.Padding.Length : 0);
					
						m_logger.XmlWriter.WriteElementString ("TotalLength",	(dataLength + paddingLength  + 0x14) + " bytes"); // + optionsLength
						m_logger.XmlWriter.WriteElementString ("HeaderLength",	packet.Offset + " bytes");
						//m_logger.XmlWriter.WriteElementString ("OptionsLength",	optionsLength + " bytes");
						m_logger.XmlWriter.WriteElementString ("PaddingLength",	paddingLength + " bytes");
						m_logger.XmlWriter.WriteElementString ("DataLength",	dataLength + " bytes");
					m_logger.XmlWriter.WriteEndElement ();
					// </LengthFields>
				
					// <Flags>
					m_logger.XmlWriter.WriteStartElement ("Flags");
						m_logger.XmlWriter.WriteElementString ("ACK",			packet.IsFlagSet (TcpFlags.Acknowledgment).ToString());
						m_logger.XmlWriter.WriteElementString ("FIN",			packet.IsFlagSet (TcpFlags.Finish).ToString());
						m_logger.XmlWriter.WriteElementString ("PSH",			packet.IsFlagSet (TcpFlags.Push).ToString());
						m_logger.XmlWriter.WriteElementString ("RST",			packet.IsFlagSet (TcpFlags.Reset).ToString());
						m_logger.XmlWriter.WriteElementString ("SYN",			packet.IsFlagSet (TcpFlags.Synchronize).ToString());
						m_logger.XmlWriter.WriteElementString ("URG",			packet.IsFlagSet (TcpFlags.Urgent).ToString());
					m_logger.XmlWriter.WriteEndElement ();
					// </Flags>
					
					if (packet.Options != null)
					{
						// <Options>
						m_logger.XmlWriter.WriteStartElement ("TcpOptions");
						
							foreach (TcpOption option in packet.Options)
							{
								// <Option>
								m_logger.XmlWriter.WriteStartElement ("Option");
									m_logger.XmlWriter.WriteElementString ("Type",	option.OptionType.ToString());
									m_logger.XmlWriter.WriteElementString ("Length",option.Length + " bytes");

									#region Specific Options
									
									switch (option.OptionType)
									{
										case TcpOptionNumber.EndOfOptions:
											break;
										case TcpOptionNumber.MaximumSegmentSize:
											TcpMaxSegmentSizeOption option1 = new TcpMaxSegmentSizeOption (option);
											m_logger.XmlWriter.WriteElementString ("MaximumSegmentSize",	option1.MaxSegmentSize.ToString());
											break;
										case TcpOptionNumber.NoOperation:
											break;
									}
									
									#endregion
									
								m_logger.XmlWriter.WriteEndElement ();
								// </Option>
							}
							
						m_logger.XmlWriter.WriteEndElement ();
						// </Options>
					}
					
				m_logger.XmlWriter.WriteEndElement ();
				// </TcpHeader>

			
			}
		}
		
		
		#endregion
	}
	
	#endregion
}