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

namespace VncSharp
{
	/// <summary>
	/// Properties of a VNC Framebuffer, and its Pixel Format.
	/// </summary>
	public class Framebuffer
	{
		string name;

		int	 bpp;
		int	 depth;
		bool bigEndian;
		bool trueColour;
		int	 redMax;
		int	 greenMax;
		int	 blueMax;
		int	 redShift;
		int	 greenShift;
		int	 blueShift;

		readonly int width;
		readonly int height;
		readonly int[] pixels;	 // I'm reusing the same pixel buffer for all update rectangles.
								 // Pixel values will always be 32-bits to match GDI representation
		readonly int pixelCount; // The total number of pixels (w x h) assigned in SetSize()


		/// <summary>
		/// Creates a new Framebuffer with (width x height) pixels.
		/// </summary>
		/// <param name="width">The width in pixels of the remote desktop.</param>
		/// <param name="height">The height in pixels of the remote desktop.</param>
		public Framebuffer(int width, int height)
		{
			this.width = width;
			this.height = height;
			
			// Cache the total size of the pixel array and initialize
			pixelCount = width * height;
			pixels = new int[pixelCount];
		}

		/// <summary>
		/// An indexer to allow access to the internal pixel buffer.
		/// </summary>
		public int this[int index] {
			get {
				return pixels[index];
			}
			set {
				pixels[index] = value;				
			}
		}

		/// <summary>
		/// The Width of the Framebuffer, measured in Pixels.
		/// </summary>
		public int Width {
			get {
				return width;
			}
		}

		/// <summary>
		/// The Height of the Framebuffer, measured in Pixels.
		/// </summary>
		public int Height {
			get {
				return height;
			}
		}

		/// <summary>
		/// Gets a Rectangle object constructed out of the Width and Height for the Framebuffer.  Used as a convenience in other classes.
		/// </summary>
		public Rectangle Rectangle {
			get {
				return new Rectangle(0, 0, width, height);
			}
		}

		/// <summary>
		/// The number of Bits Per Pixel for the Framebuffer--one of 8, 24, or 32.
		/// </summary>
		public int BitsPerPixel {
			get {
				return bpp;
			}
			set {
				bpp = value;
			}
		}

		/// <summary>
		/// The Colour Depth of the Framebuffer.
		/// </summary>
		public int Depth {
			get {
				return depth;
			}
			set {
				depth = value;
			}
		}

		/// <summary>
		/// Indicates whether the remote host uses Big- or Little-Endian order when sending multi-byte values.
		/// </summary>
		public bool BigEndian {
			get {
				return bigEndian;
			}
			set {
				bigEndian = value;
			}
		}

		/// <summary>
		/// Indicates whether the remote host supports True Colour.
		/// </summary>
		public bool TrueColour {
			get {
				return trueColour;
			}
			set {
				trueColour = value;
			}
		}

		/// <summary>
		/// The maximum value for Red in a pixel's colour value.
		/// </summary>
		public int RedMax {
			get {
				return redMax;
			}
			set {
				redMax = value;
			}
		}

		/// <summary>
		/// The maximum value for Green in a pixel's colour value.
		/// </summary>
		public int GreenMax {
			get {
				return greenMax;
			}
			set {
				greenMax = value;
			}
		}

		/// <summary>
		/// The maximum value for Blue in a pixel's colour value.
		/// </summary>
		public int BlueMax {
			get {
				return blueMax;
			}
			set {
				blueMax = value;
			}
		}

		/// <summary>
		/// The number of bits to shift pixel values in order to obtain Red values.
		/// </summary>
		public int RedShift {
			get {
				return redShift;
			}
			set {
				redShift = value;
			}
		}

		/// <summary>
		/// The number of bits to shift pixel values in order to obtain Green values.
		/// </summary>
		public int GreenShift {
			get {
				return greenShift;
			}
			set {
				greenShift = value;
			}
		}

		/// <summary>
		/// The number of bits to shift pixel values in order to obtain Blue values.
		/// </summary>
		public int BlueShift {
			get {
				return blueShift;
			}
			set {
				blueShift = value;
			}
		}

		/// <summary>
		/// The name of the remote destkop, if any.  Must be non-null.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if a null string is used when setting DesktopName.</exception>
		public string DesktopName {
			get {
				return name;
			}
			set {
				if (value == null)
					throw new ArgumentNullException("DesktopName");
				name = value;
			}
		}

		/// <summary>
		/// When communicating with the VNC Server, bytes are used to represent many of the values above.  However, internally it is easier to use Integers.  This method provides a translation between the two worlds.
		/// </summary>
		/// <returns>A byte array of 16 bytes containing the properties of the framebuffer in a format ready for transmission to the VNC server.</returns>
		public byte[] ToPixelFormat()
		{
			byte[] b = new byte[16];
			
			b[0]  = (byte) bpp;
			b[1]  = (byte) depth;
			b[2]  = (byte) (bigEndian ? 1 : 0);
			b[3]  = (byte) (trueColour ? 1 : 0);
			b[4]  = (byte) ((redMax >> 8) & 0xff);
			b[5]  = (byte) (redMax & 0xff);
			b[6]  = (byte) ((greenMax >> 8) & 0xff);
			b[7]  = (byte) (greenMax & 0xff);
			b[8]  = (byte) ((blueMax >> 8) & 0xff);
			b[9]  = (byte) (blueMax & 0xff);
			b[10] = (byte) redShift;
			b[11] = (byte) greenShift;
			b[12] = (byte) blueShift;
			// plus 3 bytes padding = 16 bytes
			
			return b;
		}
		
		/// <summary>
		/// Given the dimensions and 16-byte PIXEL_FORMAT record from the VNC Host, deserialize this into a Framebuffer object.
		/// </summary>
		/// <param name="b">The 16-byte PIXEL_FORMAT record.</param>
		/// <param name="width">The width in pixels of the remote desktop.</param>
		/// <param name="height">The height in pixles of the remote desktop.</param>
		/// <returns>Returns a Framebuffer object matching the specification of b[].</returns>
		public static Framebuffer FromPixelFormat(byte[] b, int width, int height)
		{
			if (b.Length != 16)
				throw new ArgumentException("Length of b must be 16 bytes.");
			
			Framebuffer buffer = new Framebuffer(width, height);
			
			buffer.BitsPerPixel	= (int) b[0];
			buffer.Depth		= (int) b[1];
			buffer.BigEndian	= (b[2] != 0);
			buffer.TrueColour	= (b[3] != 0);
			buffer.RedMax		= (int) (b[5] | b[4] << 8);
			buffer.GreenMax		= (int) (b[7] | b[6] << 8);
			buffer.BlueMax		= (int) (b[9] | b[8] << 8);
			buffer.RedShift		= (int) b[10];
			buffer.GreenShift	= (int) b[11];
			buffer.BlueShift	= (int) b[12];
			// Last 3 bytes are padding, ignore									

			return buffer;
		}
	}
}
