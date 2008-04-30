// VncSharp - .NET VNC Client Library
// Copyright (C) 2004  David Humphrey, Chuck Borgh, Matt Cyr
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

namespace VncSharp.Encodings
{
	/// <summary>
	/// Implementation of Hextile encoding, as well as drawing support. See RFB Protocol document v. 3.8 section 6.5.5.
	/// </summary>
	public sealed class HextileRectangle : EncodedRectangle 
	{
		private const int RAW					= 0x01;
		private const int BACKGROUND_SPECIFIED	= 0x02;
		private const int FOREGROUND_SPECIFIED	= 0x04;
		private const int ANY_SUBRECTS			= 0x08;
		private const int SUBRECTS_COLOURED		= 0x10;

		public HextileRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle) : base(rfb, framebuffer, rectangle) 
		{
		}

		public override void Decode()
		{
			// Subrectangle co-ordinates and info
			int sx;
			int sy;
			int sw;
			int sh;
			int numSubrects = 0;
			int xANDy;
			int widthANDheight;

			// Colour values to be used--black by default.
			int backgroundPixelValue = 0;
			int foregroundPixelValue = 0;
			
			// NOTE: the way that this is set-up, a Rectangle can be anywhere within the bounds
			// of the framebuffer (i.e., its x and y may not be (0,0)).  However, I ignore this
			// since the pixels for the tiles and subrectangles are all relative to this rectangle.
			// When the rectangle is drawn to the desktop later, its (x,y) position will become
			// significant again.  All of this to say that in the two main loops below, ty=0 and
			// tx=0, and all calculations are based on a (0,0) origin.
			for (int ty = 0; ty < rectangle.Height; ty += 16) {			
				// Tiles in the last row will often be less than 16 pixels high.
				// All others will be 16 high.
				int th = (rectangle.Height - ty < 16) ? rectangle.Height - ty : 16;

				for (int tx = 0; tx < rectangle.Width; tx += 16) {				
					// Tiles in the list column will often be less than 16 pixels wide.
					// All others will be 16 wide.
					int tw = (rectangle.Width - tx < 16) ? rectangle.Width - tx : 16;

					int tlStart = ty * rectangle.Width + tx;
					int tlOffset = rectangle.Width - tw;

					byte subencoding = rfb.ReadByte();

					// See if Raw bit is set in subencoding, and if so, ignore all other bits
					if ((subencoding & RAW) != 0) {
						FillRectangle(new Rectangle(tx, ty, tw, th));
					} else {
						if ((subencoding & BACKGROUND_SPECIFIED) != 0) {
							backgroundPixelValue = preader.ReadPixel();
						}

						// Fill-in background colour
						FillRectangle(new Rectangle(tx, ty, tw, th), backgroundPixelValue);
												
						if ((subencoding & FOREGROUND_SPECIFIED) != 0) {
							foregroundPixelValue = preader.ReadPixel();
						}

						if ((subencoding & ANY_SUBRECTS) != 0) {
							// Get the number of sub-rectangles in this tile
							numSubrects = rfb.ReadByte();

							for (int i = 0; i < numSubrects; i++) {
								if ((subencoding & SUBRECTS_COLOURED) != 0) {
									foregroundPixelValue = preader.ReadPixel();	// colour of this sub rectangle
								}

								xANDy = rfb.ReadByte();					// X-position (4 bits) and Y-Postion (4 bits) of this sub rectangle in the tile
								widthANDheight = rfb.ReadByte();		// Width (4 bits) and Height (4 bits) of this sub rectangle
								
								// Get the proper x, y, w, and h values out of xANDy and widthANDheight
								sx = (xANDy >> 4) & 0xf;
								sy = xANDy & 0xf;
								sw = ((widthANDheight >> 4) & 0xf) + 1;	// have to add 1 to get width
								sh = (widthANDheight & 0xf) + 1;		// same for height.

								FillRectangle(new Rectangle(tx + sx, ty + sy, sw, sh), foregroundPixelValue);
							}
						}
					}
				}
			}
		}
	}
}