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
	/// A 32-bit PixelReader.
	/// </summary>
	public sealed class PixelReader32 : PixelReader
	{
		public PixelReader32(BinaryReader reader, Framebuffer framebuffer) : base(reader, framebuffer)
		{
		}
	
		public override int ReadPixel()
		{
			// Read the pixel value
			byte[] b = reader.ReadBytes(4);

            uint pixel = (uint)(((uint)b[0]) & 0xFF | 
                                ((uint)b[1]) << 8   | 
                                ((uint)b[2]) << 16  | 
                                ((uint)b[3]) << 24);

			// Extract RGB intensities from pixel
			byte red   = (byte) ((pixel >> framebuffer.RedShift)   & framebuffer.RedMax);
			byte green = (byte) ((pixel >> framebuffer.GreenShift) & framebuffer.GreenMax);
			byte blue  = (byte) ((pixel >> framebuffer.BlueShift)  & framebuffer.BlueMax);

			return ToGdiPlusOrder(red, green, blue);			
		}
	}
}
