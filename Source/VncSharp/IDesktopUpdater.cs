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

namespace VncSharp
{
	/// <summary>
	/// Classes that implement IDesktopUpdater are used to update and Draw on a local Bitmap representation of the remote desktop.
	/// </summary>
	public interface IDesktopUpdater
	{
		/// <summary>
		/// Given a desktop Bitmap that is a local representation of the remote desktop, updates sent by the server are drawn into the area specifed by UpdateRectangle.
		/// </summary>
		/// <param name="desktop">The desktop Bitmap on which updates should be drawn.</param>
		void Draw(Bitmap desktop);
		
		/// <summary>
		/// The region of the desktop Bitmap that needs to be re-drawn.
		/// </summary>
		Rectangle UpdateRectangle {
			get;
		}
	}
}