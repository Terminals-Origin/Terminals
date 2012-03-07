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
using System.Xml;

namespace Metro.LinkLayer
{	
	#region Classes
	
	/// <summary>
	///		Represents a MAC address.
	/// </summary>
	public class MACAddress
	{
		#region Private Fields
		
		/// <summary>
		///		The address.
		/// </summary>
		private byte[] m_address = new byte[6] {00, 00, 00, 00, 00, 00};
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Return a suitable broadcast address.
		/// </summary>
		public static MACAddress BroadcastAddress
		{
			get
			{
				return new MACAddress ("ff:ff:ff:ff:ff:ff");
			}
		}
		
		
		/// <summary>
		///		Return the address.
		/// </summary>
		public byte[] Address
		{
			get
			{
				return m_address;
			}
			set
			{
				m_address = value;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Returns the vendor given a MAC address.
		/// </summary>
		/// <param name="reader">
		///		The XML text reader to use to read the file.
		///		Use the oui.xml file included in the Data folder of the library or a
		///		file with the same format. If the format is not correct then the
		///		behaviour of this method is undefined. In other words, don't blame me
		///		if it freezes or raises an exception, if you pass an incorrectly
		///		formatted file.
		///	</param>
		/// <param name="address">
		///		The MAC address whose vendor to find.
		///	</param>
		/// <returns>
		///		The vendor name.
		///	</returns>
		public static string GetVender (XmlTextReader reader, MACAddress address)
		{
			// find the prefix
			string macPrefix = address.ToString().Substring (0, 8).Replace (":", "-").ToLower();
		
			// return value
			string retVal = string.Empty;

			// keep reading nodes
			while (reader.Read ())
			{
				// if this is a MAC
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "MAC")
				{
					// read the next part
					reader.Read ();
					
					// if this is the MAC prefix
					if (reader.NodeType == XmlNodeType.Text && reader.Value.ToLower() == macPrefix)
					{
						// keep reading until we find the vendor
						while (reader.NodeType != XmlNodeType.Element || reader.Name != "Vendor")
						{
							reader.Read ();
						}
						
						reader.Read ();
						retVal = reader.Value;
						break;
					}
					else
					{
						reader.Skip ();
					}
				}
			}
			
			reader.Close ();
			return retVal;
		}
		
		
		/// <summary>
		///		Create a new MAC address.
		/// </summary>
		/// <param name="address">
		///		The address.
		///	</param>
		public MACAddress (byte[] address)
		{
			m_address = address;
		}
		
		
		/// <summary>
		///		Create a new MAC address.
		/// </summary>
		/// <param name="address">
		///		The address.
		///	</param>
		public MACAddress (string address)
		{
			string[] strParts = null;
			
			if (address.IndexOf (":") > 0)
			{
				strParts = address.Split (':');
			}
			else if (address.IndexOf ("-") > 0)
			{
				strParts = address.Split ('-');
			}
			else
			{
				return;
			}
			
			m_address = new byte[strParts.Length];
			
			for (int i = 0; i < m_address.Length; i++)
			{
				m_address[i] = Convert.ToByte (strParts[i], 16);
			}

		}
		
		
		/// <summary>
		///		Convert the address to a string.
		/// </summary>
		/// <returns>
		///		The address in the format xx:xx:xx:xx:xx:xx
		///	</returns>
		public override string ToString()
		{
			return ToString (':');
		}
		
		
		/// <summary>
		///		Convert the address to a string.
		/// </summary>
		/// <param name="seperator">
		///		The character which seperates each byte in the IP.
		///	</param>
		/// <returns>
		///		The address in the format xx:xx:xx:xx:xx:xx
		///	</returns>
		public string ToString(char seperator)
		{
			string address = string.Empty;
			
			for (int i = 0; i < m_address.Length; i++)
			{
				if (m_address[i] < 10)
				{
					address += "0" + string.Format ("{0:x}", m_address[i]) + seperator.ToString();
				}
				else
				{
					address += string.Format ("{0:x}", m_address[i]) + seperator.ToString();
				}
			}
		
			return address.ToUpper().Substring(0, address.Length-1);
		}

		
		#endregion
	}
	
	
	#endregion
}