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
using System.Runtime.InteropServices;

namespace Metro
{
	
	/// <summary>
	///		Allows formatting of win 32 errors
	/// </summary>
	public class Win32Error
	{
	
		[DllImport("kernel32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private unsafe static extern int FormatMessage (
			int dwFlags, 
			ref IntPtr lpSource, 
			int dwMessageId, 
			int dwLanguageId, 
			ref String lpBuffer, 
			int nSize, 
			IntPtr *Arguments);
		
		/// <summary>
		///		Formats a win32 error code to a string description.
		/// </summary>
		/// <param name="errorCode">
		///		The error code.
		///	</param>
		/// <returns>
		///		The error code description
		///	</returns>
		public unsafe static string GetErrorMessage (int errorCode)
		{
			int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
			int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
			int FORMAT_MESSAGE_FROM_SYSTEM  = 0x00001000;

			int messageSize = 255;
			string lpMsgBuf = "";
			int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

			IntPtr ptrlpSource = IntPtr.Zero;
			IntPtr prtArguments = IntPtr.Zero;
        
			int retVal = FormatMessage (dwFlags, ref ptrlpSource, errorCode, 0, ref lpMsgBuf, messageSize, &prtArguments);
			
			if (0 == retVal)
			{
			    throw new Exception("Failed to format message for error code " + errorCode + ". ");
			}

			return lpMsgBuf;
		}
		
	}
}
