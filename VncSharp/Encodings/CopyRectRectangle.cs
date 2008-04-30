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
	/// Implementation of CopyRect encoding, as well as drawing support. See RFB Protocol document v. 3.8 section 6.5.2.
	/// </summary>
	public sealed class CopyRectRectangle : EncodedRectangle 
	{
		public CopyRectRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle) : base(rfb, framebuffer, rectangle) 
		{
		}

		// CopyRect Source Point (x,y) from which to copy pixels in Draw
		Point source;

		/// <summary>
		/// Decodes a CopyRect encoded rectangle.
		/// </summary>
		public override void Decode()
		{
			// Read the source point from which to begin copying pixels
			source = new Point();
			source.X = (int) rfb.ReadUInt16();
			source.Y = (int) rfb.ReadUInt16();
		}

		public unsafe override void Draw(Bitmap desktop)
		{
			// Given a source area, copy this region to the point specified by destination
			BitmapData bmpd = desktop.LockBits(new Rectangle(new Point(0,0), desktop.Size),
											   ImageLockMode.ReadWrite, 
											   desktop.PixelFormat);

			try {
				int* pSrc  = (int*)(void*)bmpd.Scan0;
				int* pDest = (int*)(void*)bmpd.Scan0;

                // Calculate the difference between the stride of the desktop, and the pixels we really copied. 
                int nonCopiedPixelStride = desktop.Width - rectangle.Width;

                // Move source and destination pointers
                pSrc += source.Y * desktop.Width + source.X;
                pDest += rectangle.Y * desktop.Width + rectangle.X;

                // BUG FIX (Peter Wentworth) EPW:  we need to guard against overwriting old pixels before
                // they've been moved, so we need to work out whether this slides pixels upwards in memeory,
                // or downwards, and run the loop backwards if necessary. 
                if (pDest < pSrc) {   // we can copy with pointers that increment
                    for (int y = 0; y < rectangle.Height; ++y) {
                        for (int x = 0; x < rectangle.Width; ++x) {
                            *pDest++ = *pSrc++;
                        }

                        // Move pointers to beginning of next row in rectangle
                        pSrc  += nonCopiedPixelStride;
                        pDest += nonCopiedPixelStride;
                    }
                } else {
                    // Move source and destination pointers to just beyond the furthest-from-origin 
                    // pixel to be copied.
                    pSrc  += (rectangle.Height * desktop.Width) + rectangle.Width;
                    pDest += (rectangle.Height * desktop.Width) + rectangle.Width;

                    for (int y = 0; y < rectangle.Height; ++y) {
                        for (int x = 0; x < rectangle.Width; ++x) {
                            *(--pDest) = *(--pSrc);
                        }

                        // Move pointers to end of previous row in rectangle
                        pSrc  -= nonCopiedPixelStride;
                        pDest -= nonCopiedPixelStride;
                    }
                }
			} finally {
				desktop.UnlockBits(bmpd);
				bmpd = null;
			}
		}
	}
}