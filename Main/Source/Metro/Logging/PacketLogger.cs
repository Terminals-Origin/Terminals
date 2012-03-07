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
//using System.Threading;

namespace Metro.Logging
{
	#region Classes

	/// <summary>
	///		Provides very basic packet logging methods.
	/// </summary>
	public class PacketLogger : IDisposable
	{	
		#region Private Fields
		
		/// <summary>
		///		The xml text writer to use to write to the log file.
		/// </summary>
		private XmlTextWriter m_writer;
		
		/// <summary>
		///		Whether or not the class has been disposed or not.
		/// </summary>
		private bool m_disposed = false;

		/// <summary>
		///		Used to synchronize access to packets.
		/// </summary>
		private bool m_inUse = false;
		
		/// <summary>
		///		Used by Dispose methods to wait for all packets to finish writing.
		/// </summary>
		private int m_packetCount = 0;

		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		The xml text writer to use to write to the log file.
		/// </summary>
		public XmlTextWriter XmlWriter
		{
			get
			{
				return m_writer;
			}
		}
		
		
		/// <summary>
		///		Whether or not the class has been disposed or not.
		/// </summary>
		public bool Disposed
		{
			get
			{
				return m_disposed;
			}
		}
		
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Create a new packet logger.
		/// </summary>
		/// <param name="writer">
		///		The xml writer to use to write to the log.
		///	</param>
		public PacketLogger (XmlTextWriter writer)
		{
			m_writer = writer;
			m_writer.Formatting = Formatting.Indented;
			
			// write the start of the document
			m_writer.WriteStartDocument ();
			
			// write the root element
			m_writer.WriteStartElement ("PacketLog");
			
				// write log data
				m_writer.WriteStartElement ("LogData");
					m_writer.WriteElementString ("UserName", System.Environment.UserName);
					m_writer.WriteElementString ("MachineName", Environment.MachineName);
					m_writer.WriteElementString ("CLRVersion", Environment.Version.ToString());
					m_writer.WriteElementString ("StartDate", DateTime.Now.ToLongDateString ());
					m_writer.WriteElementString ("StartTime", DateTime.Now.ToLongTimeString ());
				m_writer.WriteEndElement ();
		}
		
		
		/// <summary>
		///		Create a new packet logger.
		/// </summary>
		/// <param name="stream">
		///		The stream to write the log to.
		///	</param>
		public PacketLogger (Stream stream) : this (new XmlTextWriter (stream, System.Text.Encoding.Default))
		{
		}		
		
		
		/// <summary>
		///		Create a new packet logger.
		/// </summary>
		/// <param name="fileName">
		///		The name of the file to log to.
		///	</param>
		public PacketLogger (string fileName) : this (new FileStream (fileName, FileMode.OpenOrCreate))
		{
		}
		
		
		/// <summary>
		///		Write the start tags of a new packet. This should be paired with a
		///		EndPacket call.
		/// </summary>
		public void BeginPacket ()
		{	
		
			// increment the packet count
			m_packetCount++;
		
			// wait for the logger to finish the last packet
			while (true)
			{
				if (!m_inUse)
				{
					m_inUse = true;
					break;
				}
			}
		
			lock (m_writer)
			{
				m_writer.WriteStartElement ("Packet");
					m_writer.WriteStartElement ("PacketData");
						m_writer.WriteElementString ("ArrivalDate", DateTime.Now.ToLongDateString ());
						m_writer.WriteElementString ("ArrivalTime", DateTime.Now.ToLongTimeString ());
					m_writer.WriteEndElement ();
			}
		}
		
		
		/// <summary>
		///		Write the end tags of the packet. This should be paired with a
		///		BeginPacket call.
		/// </summary>
		public void EndPacket ()
		{
			m_writer.WriteEndElement ();
			m_inUse = false;
			m_packetCount--;
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
					
					// wait for all packets to be logged
					while (m_packetCount > 0)
					{
					}
					
					// write the end of the log
					lock (m_writer)
					{
						m_writer.WriteEndElement ();
						m_writer.WriteEndDocument ();
						m_writer.Close ();
						m_writer = null;
					}
				}
				
				
				// clean up unmanaged resources
			}
			
			m_disposed = true;
		}
		
		
		/// <summary>
		///		Destructor.
		/// </summary>
		~PacketLogger ()
		{
			Dispose (false);
		}
		
		
		#endregion
	}


	#endregion
}
