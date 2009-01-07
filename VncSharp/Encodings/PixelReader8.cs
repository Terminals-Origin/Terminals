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

namespace VncSharp.Encodings
{
	/// <summary>
	/// An 8-bit PixelReader--currently unsupported.
	/// </summary>
	public sealed class PixelReader8 : PixelReader
	{
		public PixelReader8(RfbProtocol rfb, Framebuffer framebuffer) : base(rfb, framebuffer)
		{
		}
	
		/// <summary>
		/// Reads a pixel. Not currently implemented for 8 bits.
		/// </summary>
		/// <returns>Returns an Integer value representing the pixel in GDI+ format.</returns>
		/// <exception cref="NotImplementedException">Always thrown.</exception>
		public override int ReadPixel()
		{
			// TODO: 8-Bit colour maps are not implemented yet.  If you want to do it,
			// you'll need to read the colour map from the server, which will be 32-bit
			// colour values that need to be keyed by position in an array.  Then when
			// you read a byte, it will be the position of the colour to use in the map.
			byte pixel = rfb.ReadByte();
			throw new NotImplementedException("8 bpp colour maps are not currently supported.");
		}
	}
}
