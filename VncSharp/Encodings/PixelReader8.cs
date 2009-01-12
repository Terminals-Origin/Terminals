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
	/// An 8-bit PixelReader
	/// </summary>
	public sealed class PixelReader8 : PixelReader
	{
		private RfbProtocol rfb = null;

		public PixelReader8(BinaryReader reader, Framebuffer framebuffer, RfbProtocol rfb) : base(reader, framebuffer)
		{
			this.rfb = rfb;
		}
	
		/// <summary>
		/// Reads an 8-bit pixel.
		/// </summary>
		/// <returns>Returns an Integer value representing the pixel in GDI+ format.</returns>
		public override int ReadPixel()
		{
			byte idx = reader.ReadByte();
			return ToGdiPlusOrder((byte)rfb.MapEntries[idx, 0], (byte)rfb.MapEntries[idx, 1], (byte)rfb.MapEntries[idx, 2]);
		}
	}
}
