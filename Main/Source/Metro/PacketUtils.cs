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

namespace Metro
{
	#region Classes
	
	/// <summary>
	///		A selection of useful procedures.
	/// </summary>
	public class PacketUtils
	{
		#region Public Methods
	
		/// <summary>
		///		The checksum field is the 16 bit one's complement of the one's
		///		complement sum of all 16 bit words in the header. For purposes of
		///		computing the checksum, the value of the checksum field is zero.
		///		This method implements this algorithm for computing the checksum
		///		in protocols like IP, TCP, UDP, ICMP etc.
		/// </summary>
		/// <param name="Buffer">
		///		The packet to calculate the checksum for.
		///	</param>
		/// <param name="size">
		///		The length of the packet.
		///	</param>
		/// <returns>
		///		The checksum.
		///	</returns>
		public static UInt16 CalculateChecksum (UInt16[] Buffer, int size) 
		{
		
			int cksum = 0;
			int counter = 0;

			// sum up the 16 bit words
			while (size > 0) {
				UInt16 val = Buffer[counter];
				cksum += Convert.ToInt32 (Buffer[counter]);
				counter ++; size--;
			}
		
			cksum = (cksum >> 16) + (cksum & 0xffff);
			cksum += (cksum >> 16);

			return (UInt16)(~cksum);
		}
	
	
		/// <summary>
		///		The checksum field is the 16 bit one's complement of the one's
		///		complement sum of all 16 bit words in the header. For purposes of
		///		computing the checksum, the value of the checksum field is zero.
		///		This method implements this algorithm for computing the checksum
		///		in protocols like IP, TCP, UDP, ICMP etc.
		/// </summary>
		/// <param name="buffer">
		///		The packet to calculate the checksum for.
		///	</param>
		/// <returns>
		///		The checksum.
		///	</returns>
		public static UInt16 CalculateChecksum (byte[] buffer) 
		{
		
			// Calculate the number of elements in the check sum buffer
			// This will be half of the number in the packet buffer
			// because the packet buffer is in bytes and this will be in UInt16's
			int checksumBufferLength = Convert.ToInt32 (Math.Ceiling (Convert.ToDouble(buffer.Length) / 2));

			// Actually create the array of UInt16's now to calculate the checksum with
			UInt16[] checksumBuffer = new UInt16[checksumBufferLength];
			
			int headerBufferIndex = 0;
			
			// Pack each 2 byte pair in to a single UInt16 element in the new buffer, checksumBuffer
			for (int i = 0; i < checksumBufferLength; i++) {
			
				if (buffer.Length - headerBufferIndex >= 2)
				{
					checksumBuffer[i] = (ushort) IPAddress.HostToNetworkOrder (BitConverter.ToInt16 (buffer, headerBufferIndex));
				}
				else
				{
					checksumBuffer[i] = (ushort)(buffer[headerBufferIndex] << 8);
				}
				
				headerBufferIndex += 2;
			}
		
			return CalculateChecksum (checksumBuffer, checksumBuffer.Length);
			
		}
		
		
		#endregion
	}
	
	
	#endregion
}
