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
using System.Diagnostics;
using System.Drawing;

namespace VncSharp
{
	/// <summary>
	/// A view-only version of IVncInputPolicy.
	/// </summary>
	public sealed class VncDefaultInputPolicy : IVncInputPolicy
	{
		private RfbProtocol rfb;
		
		public VncDefaultInputPolicy(RfbProtocol rfb)
		{
			Debug.Assert(rfb != null);
			this.rfb = rfb;
		}

		// Let all exceptions get caught in VncClient

		public void WriteKeyboardEvent(uint keysym, bool pressed)
		{
			rfb.WriteKeyEvent(keysym, pressed);
		}

		public void WritePointerEvent(byte buttonMask, Point point)
		{
			rfb.WritePointerEvent(buttonMask, point);
		}
	}
}