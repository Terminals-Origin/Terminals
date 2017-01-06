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
using Metro.NetworkLayer.IpV4;

namespace Metro.Logging
{
	#region Classes
	
	/// <summary>
	///		Provides logging for IP version 4 packets.
	/// </summary>
	public class IpV4Logger
	{
		#region Private Fields
		
		/// <summary>
		///		The packet logger to use.
		/// </summary>
		PacketLogger m_logger;
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new IP version 4 logger.
		/// </summary>
		/// <param name="logger">
		///		The logger object to use.
		///	</param>
		public IpV4Logger (PacketLogger logger)
		{
			m_logger = logger;
		}
		
		
		/// <summary>
		///		Log a new IP packet.
		/// </summary>
		/// <param name="packet">
		///		The packet to log.
		///	</param>
		public void LogPacket (IpV4Packet packet)
		{
			lock (m_logger.XmlWriter)
			{
				// <IpV4Header>
				m_logger.XmlWriter.WriteStartElement ("IpV4Header");

					m_logger.XmlWriter.WriteElementString ("SourceAddress",		packet.SourceAddress.ToString());
					m_logger.XmlWriter.WriteElementString ("DestinationAddress",packet.DestinationAddress.ToString());
					m_logger.XmlWriter.WriteElementString ("TransportProtocol",	packet.TransportProtocol.ToString());
					m_logger.XmlWriter.WriteElementString ("TimeToLive",		packet.TimeToLive.ToString());
					m_logger.XmlWriter.WriteElementString ("Identification",	packet.Identification.ToString());
					m_logger.XmlWriter.WriteElementString ("Checksum",			packet.Checksum.ToString());
					
					// <LengthFields>
					m_logger.XmlWriter.WriteStartElement ("IpLengthFields");
						m_logger.XmlWriter.WriteElementString ("TotalLength",	packet.TotalLength + " bytes");
						m_logger.XmlWriter.WriteElementString ("HeaderLength",	packet.HeaderLength + " bytes");
						m_logger.XmlWriter.WriteElementString ("OptionsLength",	(packet.TotalLength - (packet.Padding != null ? packet.Padding.Length : 0) - (packet.Data != null ? packet.Data.Length : 0) - 0x14) + " bytes");
						m_logger.XmlWriter.WriteElementString ("PaddingLength",	(packet.Padding != null ? packet.Padding.Length : 0) + " bytes");
						m_logger.XmlWriter.WriteElementString ("DataLength",	(packet.Data != null ? packet.Data.Length : 0) + " bytes");
					m_logger.XmlWriter.WriteEndElement ();
					// </LengthFields>
					
					// <Fragmentation>
					m_logger.XmlWriter.WriteStartElement ("Fragmentation");
						m_logger.XmlWriter.WriteElementString ("Fragments",		packet.Fragments.ToString());
						m_logger.XmlWriter.WriteElementString ("DontFragment",	packet.ControlFlags.DontFragment.ToString());
						m_logger.XmlWriter.WriteElementString ("MoreFragments",	packet.ControlFlags.MoreFragments.ToString());
						m_logger.XmlWriter.WriteElementString ("Offset",		packet.ControlFlags.Offset.ToString());
					m_logger.XmlWriter.WriteEndElement ();
					// </Fragmentation>
					
					// <TypeOfService>
					m_logger.XmlWriter.WriteStartElement ("TypeOfService");
						m_logger.XmlWriter.WriteElementString ("Precedence",	packet.TypeOfService.Precedence.ToString());
						m_logger.XmlWriter.WriteElementString ("Delay",			packet.TypeOfService.Delay.ToString());
						m_logger.XmlWriter.WriteElementString ("Reliability",	packet.TypeOfService.Reliability.ToString());
						m_logger.XmlWriter.WriteElementString ("Throughput",	packet.TypeOfService.Throughput.ToString());
					m_logger.XmlWriter.WriteEndElement ();
					// </TypeOfService>
					
					if (packet.Options != null)
					{
						// <Options>
						m_logger.XmlWriter.WriteStartElement ("IpOptions");
						
							foreach (IpV4Option option in packet.Options)
							{
								// <Option>
								m_logger.XmlWriter.WriteStartElement ("Option");
									m_logger.XmlWriter.WriteElementString ("Class",	option.Class.ToString());
									m_logger.XmlWriter.WriteElementString ("Type",	option.OptionType.ToString());
									m_logger.XmlWriter.WriteElementString ("Length",option.Length + " bytes");
									m_logger.XmlWriter.WriteElementString ("Copied",option.IsCopied.ToString());
									
									#region Specific Options
									
									switch (option.OptionType)
									{
										case IpV4OptionNumber.EndOfOptions:
											break;
										case IpV4OptionNumber.InternetTimestamp:
										
											IpV4TimeStampOption option1 = new IpV4TimeStampOption (option);
											
											m_logger.XmlWriter.WriteElementString ("TimestampType",	option1.OptionType.ToString());
											m_logger.XmlWriter.WriteElementString ("Overflow",		option1.Overflow.ToString());
											m_logger.XmlWriter.WriteElementString ("Pointer",			option1.Pointer.ToString());

											// TODO: Handle logging of time stamps and address stamps in time stamp IP option
											
											break;
										case IpV4OptionNumber.LooseSourceRouting:
										
											IpV4RoutingOption option2 = new IpV4RoutingOption (option);
											
											m_logger.XmlWriter.WriteElementString ("Pointer",			option2.Pointer.ToString());
										
											// TODO: Handle logging of route in routing IP options
											
											break;
										case IpV4OptionNumber.NoOperation:
											break;
										case IpV4OptionNumber.RecordRoute:
										
											IpV4RoutingOption option3 = new IpV4RoutingOption (option);
											
											m_logger.XmlWriter.WriteElementString ("Pointer",			option3.Pointer.ToString());
										
											// TODO: Handle logging of route in routing IP options
											
											break;
										case IpV4OptionNumber.Security:
											
											IpV4SecurityOption option4 = new IpV4SecurityOption (option);
										
											m_logger.XmlWriter.WriteElementString ("SecurityLevel",			option4.SecurityLevel.ToString());
											m_logger.XmlWriter.WriteElementString ("Compartment",				option4.Compartment.ToString());
											m_logger.XmlWriter.WriteElementString ("HandlingRestrictions",	option4.HandlingRestrictions.ToString());
											m_logger.XmlWriter.WriteElementString ("TransmissionControlCode",	option4.TransmissionControlCode.ToString());
										
											break;
										case IpV4OptionNumber.StreamId:
										
											IpV4StreamIdentifierOption option5 = new IpV4StreamIdentifierOption (option);
											
											m_logger.XmlWriter.WriteElementString ("StreamIdentifier",	option5.StreamIdentifier.ToString());
										
											break;
										case IpV4OptionNumber.StrictSourceRouting:
										
											IpV4RoutingOption option6 = new IpV4RoutingOption (option);
											
											m_logger.XmlWriter.WriteElementString ("Pointer",			option6.Pointer.ToString());
										
											// TODO: Handle logging of route in routing IP options
											
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
				// </IpV4Header>
			}
		}
		
		
		#endregion
	}
	
	#endregion
}