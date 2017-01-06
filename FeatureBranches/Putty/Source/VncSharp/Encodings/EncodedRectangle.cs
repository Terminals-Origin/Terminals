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
using System.Runtime.InteropServices;
using System.IO;

namespace VncSharp.Encodings
{
	/// <summary>
	/// Abstract class representing an Encoded Rectangle to be read, decoded, and drawn.
	/// </summary>
	public abstract class EncodedRectangle : IDesktopUpdater
	{
		protected RfbProtocol	rfb;
		protected Rectangle		rectangle;
		protected Framebuffer	framebuffer;
		protected PixelReader	preader;

		public EncodedRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle, int encoding)
		{
			this.rfb = rfb;
			this.framebuffer = framebuffer;
			this.rectangle = rectangle;

			//Select appropriate reader
			BinaryReader reader = (encoding == RfbProtocol.ZRLE_ENCODING) ? rfb.ZrleReader : rfb.Reader;

			// Create the appropriate PixelReader depending on screen size and encoding
			switch (framebuffer.BitsPerPixel)
			{
				case 32:
					if (encoding == RfbProtocol.ZRLE_ENCODING)
					{
						preader = new CPixelReader(reader, framebuffer);
					}
					else
					{
						preader = new PixelReader32(reader, framebuffer);
					}
					break;
				case 16:
					preader = new PixelReader16(reader, framebuffer);
					break;
				case 8:
					preader = new PixelReader8(reader, framebuffer, rfb);
					break;
				default:
					throw new ArgumentOutOfRangeException("BitsPerPixel", framebuffer.BitsPerPixel, "Valid VNC Pixel Widths are 8, 16 or 32 bits.");
			}
		}

		/// <summary>
		/// Gets the rectangle that needs to be decoded and drawn.
		/// </summary>
		public Rectangle UpdateRectangle {
			get {
				return rectangle;
			}
		}

		/// <summary>
		/// Obtain all necessary information from VNC Host (i.e., read) in order to Draw the rectangle, and store in colours[].
		/// </summary>
		public abstract void Decode();
		
		/// <summary>
		/// After calling Decode() an EncodedRectangle can be drawn to a Bitmap, which is the local representation of the remote desktop.
		/// </summary>
		/// <param name="desktop">The image the represents the remote desktop. NOTE: this image will be altered.</param>
		public unsafe virtual void Draw(Bitmap desktop)
		{
			// Lock the bitmap's scan-lines in RAM so we can iterate over them using pointers and update the area
			// defined in rectangle.
			BitmapData bmpd = desktop.LockBits(new Rectangle(new Point(0,0), desktop.Size), ImageLockMode.ReadWrite, desktop.PixelFormat);

			try {
				// For speed I'm using pointers to manipulate the desktop bitmap, which is unsafe.
				// Get a pointer to the start of the bitmap in memory (IntPtr) and cast to a 
				// Byte pointer (need void* first) so desktop can be traversed as GDI+ 
				// colour values form.
				int* pInt = (int*)(void*)bmpd.Scan0; 

				// Move pointer to position in desktop bitmap where rectangle begins
				pInt += rectangle.Y * desktop.Width + rectangle.X;
				
				int offset = desktop.Width - rectangle.Width;
				int row = 0;
				
				for (int y = 0; y < rectangle.Height; ++y) {
					row = y * rectangle.Width;

					for (int x = 0; x < rectangle.Width; ++x) {
						*pInt++ = framebuffer[row + x];
					}

					// Move pointer to beginning of next row in rectangle
					pInt += offset;
				}
			} finally {
				desktop.UnlockBits(bmpd);
				bmpd = null;
			}		
		}

		/// <summary>
		/// Fills the given Rectangle with a solid colour (i.e., all pixels will have the same value--colour).
		/// </summary>
		/// <param name="rect">The rectangle to be filled.</param>
		/// <param name="colour">The colour to use when filling the rectangle.</param>
		protected void FillRectangle(Rectangle rect, int colour)
		{
			int ptr = 0;
			int offset = 0;

			// If the two rectangles don't match, then rect is contained within rectangle, and
			// ptr and offset need to be adjusted to position things at the proper starting point.
			if (rect != rectangle) {
				ptr = rect.Y * rectangle.Width + rect.X;	// move to the start of the rectangle in pixels
				offset = rectangle.Width - rect.Width;		// calculate the offset to get to the start of the next row
			}

			for (int y = 0; y < rect.Height; ++y) {
				for (int x = 0; x < rect.Width; ++x) {
					framebuffer[ptr++] = colour;			// colour every pixel the same
				}
				ptr += offset;								// advance to next row within pixels
			}
		}

		protected void FillRectangle(Rectangle rect, int[] tile)
		{
			int ptr = 0;
			int offset = 0;

			// If the two rectangles don't match, then rect is contained within rectangle, and
			// ptr and offset need to be adjusted to position things at the proper starting point.
			if (rect != rectangle) {
				ptr = rect.Y * rectangle.Width + rect.X;	// move to the start of the rectangle in pixels
				offset = rectangle.Width - rect.Width;		// calculate the offset to get to the start of the next row
			}

			int idx = 0;
			for (int y = 0; y < rect.Height; ++y) {
				for (int x = 0; x < rect.Width; ++x) {
					framebuffer[ptr++] = tile[idx++];
				}
				ptr += offset;								// advance to next row within pixels
			}
		}
		
		/// <summary>
		/// Fills the given Rectangle with pixel values read from the server (i.e., each pixel may have its own value).
		/// </summary>
		/// <param name="rect">The rectangle to be filled.</param>
		protected void FillRectangle(Rectangle rect)
		{
			int ptr = 0;
			int offset = 0;

			// If the two rectangles don't match, then rect is contained within rectangle, and
			// ptr and offset need to be adjusted to position things at the proper starting point.
			if (rect != rectangle) {
				ptr = rect.Y * rectangle.Width + rect.X;	// move to the start of the rectangle in pixels
				offset = rectangle.Width - rect.Width;		// calculate the offset to get to the start of the next row
			}

			for (int y = 0; y < rect.Height; ++y) {
				for (int x = 0; x < rect.Width; ++x) {
					framebuffer[ptr++] = preader.ReadPixel();	// every pixel needs to be read from server
				}
				ptr += offset;								    // advance to next row within pixels
			}
		}
	}
}
