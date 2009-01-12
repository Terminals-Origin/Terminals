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
	/// Used to read the appropriate number of bytes from the server based on the
	/// width of pixels and convert to a GDI+ colour value (i.e., BGRA).
	/// </summary>
	public abstract class PixelReader
	{
		protected BinaryReader reader;
		protected Framebuffer framebuffer;

		protected PixelReader(BinaryReader reader, Framebuffer framebuffer)
		{
			this.reader = reader;
			this.framebuffer = framebuffer;
		}

		public abstract int ReadPixel();

		protected int ToGdiPlusOrder(byte red, byte green, byte blue)
		{
			// Put colour values into proper order for GDI+ (i.e., BGRA, where Alpha is always 0xFF)
			return (int) (blue & 0xFF | green << 8 | red << 16 | 0xFF << 24);			
		}
	}
}
