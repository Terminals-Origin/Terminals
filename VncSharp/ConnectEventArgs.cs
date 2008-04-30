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

namespace VncSharp
{
	/// <summary>
	/// Used in connection with the ConnectComplete event. Contains information about the remote desktop useful for setting-up the client's GUI.
	/// </summary>
	public class ConnectEventArgs : EventArgs
	{
		int width;
		int height;
		string name;
		
		/// <summary>
		/// Constructor for ConnectEventArgs
		/// </summary>
		/// <param name="width">An Integer indicating the Width of the remote framebuffer.</param>
		/// <param name="height">An Integer indicating the Height of the remote framebuffer.</param>
		/// <param name="name">A String containing the name of the remote Desktop.</param>
		public ConnectEventArgs(int width, int height, string name) : base()
		{
			this.width = width;
			this.height = height;
			this.name = name;
		}
		
		/// <summary>
		/// Gets the Width of the remote Desktop.
		/// </summary>
		public int DesktopWidth {
			get { 
				return width; 
			}
		}
		
		/// <summary>
		/// Gets the Height of the remote Desktop.
		/// </summary>
		public int DesktopHeight {
			get { 
				return height; 
			}
		}
		
		/// <summary>
		/// Gets the name of the remote Desktop, if any.
		/// </summary>
		public string DesktopName {
			get { 
				return name; 
			}
		}
	}
}
