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
using System.Drawing;
using System.Drawing.Imaging;

// FIXME: I can't understand why yet, but under the Xvnc server in Unix (v. 3.3.7), this doesn't work.  
// Everything is fine using the Windows server!?

namespace VncSharp.Encodings
{
	/// <summary>
	/// Implementation of CoRRE encoding, as well as drawing support. See RFB Protocol document v. 3.8 section 6.5.4.
	/// </summary>
	public sealed class CoRreRectangle : EncodedRectangle 
	{
		public CoRreRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle) : base(rfb, framebuffer, rectangle, RfbProtocol.CORRE_ENCODING)
		{
		}

		/// <summary>
		/// Decodes a CoRRE Encoded Rectangle.
		/// </summary>
		public override void Decode()
		{
			int numSubRect = (int) rfb.ReadUint32();	// Number of sub-rectangles within this rectangle
			int bgPixelVal = preader.ReadPixel();		// Background colour
			int subRectVal = 0;							// Colour to be used for each sub-rectangle
			
			// Dimensions of each sub-rectangle will be read into these
			int x, y, w, h;

			// Initialize the full pixel array to the background colour
			FillRectangle(rectangle, bgPixelVal);

			// Colour in all the subrectangles, reading the properties of each one after another.
			for (int i = 0; i < numSubRect; i++) {
				subRectVal	= preader.ReadPixel();
				x			= (int) rfb.ReadByte();
				y			= (int) rfb.ReadByte();
				w			= (int) rfb.ReadByte();
				h			= (int) rfb.ReadByte();
				
				// Colour in this sub-rectangle with the colour provided.
				FillRectangle(new Rectangle(x, y, w, h), subRectVal);
			}
		}
	}
}