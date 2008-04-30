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

// TODO: need to add support for Zrle Encodings below in Build().

using System;
using System.Drawing;
using VncSharp.Encodings;

namespace VncSharp
{
	/// <summary>
	/// Factory class used to create derived EncodedRectangle objects at runtime based on data sent by VNC Server.
	/// </summary>
	public class EncodedRectangleFactory
	{
		RfbProtocol rfb;
		Framebuffer framebuffer;
		
		/// <summary>
		/// Creates an instance of the EncodedRectangleFactory using the connected RfbProtocol object and associated Framebuffer object.
		/// </summary>
		/// <param name="rfb">An RfbProtocol object that will be passed to any created EncodedRectangle objects.  Must be non-null, already initialized, and connected.</param>
		/// <param name="framebuffer">A Framebuffer object which will be used by any created EncodedRectangle objects in order to decode and draw rectangles locally.</param>
		public EncodedRectangleFactory(RfbProtocol rfb, Framebuffer framebuffer)
		{
			System.Diagnostics.Debug.Assert(rfb != null, "RfbProtocol object must be non-null");
			System.Diagnostics.Debug.Assert(framebuffer != null, "Framebuffer object must be non-null");
			
			this.rfb = rfb;
			this.framebuffer = framebuffer;
		}
		
		/// <summary>
		/// Creates an object type derived from EncodedRectangle, based on the value of encoding.
		/// </summary>
		/// <param name="rectangle">A Rectangle object defining the bounds of the rectangle to be created</param>
		/// <param name="encoding">An Integer indicating the encoding type to be used for this rectangle.  Used to determine the type of EncodedRectangle to create.</param>
		/// <returns></returns>
		public EncodedRectangle Build(Rectangle rectangle, int encoding)
		{
			EncodedRectangle e = null;

			switch (encoding) {
				case RfbProtocol.RAW_ENCODING:
					e = new RawRectangle(rfb, framebuffer, rectangle);
					break;
				case RfbProtocol.COPYRECT_ENCODING:
					e = new CopyRectRectangle(rfb, framebuffer, rectangle);
					break;
				case RfbProtocol.RRE_ENCODING:
					e = new RreRectangle(rfb, framebuffer, rectangle);
					break;
				case RfbProtocol.CORRE_ENCODING:
					e = new CoRreRectangle(rfb, framebuffer, rectangle);
					break;
				case RfbProtocol.HEXTILE_ENCODING:
					e = new HextileRectangle(rfb, framebuffer, rectangle);
					break;
				case RfbProtocol.ZRLE_ENCODING:
// TODO: ZRLE is not written yet, but when it is, this needs to be changed.
//					e = new ZrleRectangle(rfb, framebuffer, rectangle);				
//					break;
					throw new NotImplementedException("Need to implement Zrle Encoding!!!");
				default:
					// Sanity check
					throw new VncProtocolException("Unsupported Encoding Format received: " + encoding.ToString() + ".");
			}
			return e;
		}
	}
}
