// VncSharp - .NET VNC Client Library
// Copyright (C) 2008 David Humphrey
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.IO;

namespace VncSharp.Encodings
{
	/// <summary>
	/// A 16-bit PixelReader.
	/// </summary>
	public sealed class PixelReader16 : PixelReader
	{
		public PixelReader16(BinaryReader reader, Framebuffer framebuffer) : base(reader, framebuffer)
		{
		}
	
		public override int ReadPixel()
		{
			// NOTE: the 16 bit pixel value uses a 565 layout (i.e., 5 bits
			// for Red, 6 for Green, 5 for Blue).  As such, I'm doing an extra
			// shift below for each colour to get from 565 to 888 in order to 
			// return a full 32-bit pixel value.
			byte[] b = reader.ReadBytes(2);

            ushort pixel = (ushort)(((uint)b[0]) & 0xFF | ((uint)b[1]) << 8);

			byte red   = (byte) (((pixel >> framebuffer.RedShift)   & framebuffer.RedMax)   << 3);  // 5 bits to 8
			byte green = (byte) (((pixel >> framebuffer.GreenShift) & framebuffer.GreenMax) << 2);  // 6 bits to 8
			byte blue  = (byte) (((pixel >> framebuffer.BlueShift)  & framebuffer.BlueMax)  << 3);  // 5 bits to 8

			return ToGdiPlusOrder(red, green, blue);			
		}
	}
}
