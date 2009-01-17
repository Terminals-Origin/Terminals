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
using System.Windows.Forms;
using System.Drawing;

namespace VncSharp
{
	/// <summary>
	/// Base class for desktop clipping/scaling policies.  Used by RemoteDesktop.
	/// </summary>
	public abstract class VncDesktopTransformPolicy
	{
        protected VncClient vnc;
        protected RemoteDesktop remoteDesktop;

        public VncDesktopTransformPolicy(VncClient vnc,
                                         RemoteDesktop remoteDesktop)
        {
            this.vnc = vnc;
            this.remoteDesktop = remoteDesktop;
        }

        public virtual bool AutoScroll
        {
            get
            {
                return false;
            }
        }

        public abstract Size AutoScrollMinSize { get; }

        public abstract Rectangle AdjustUpdateRectangle(Rectangle updateRectangle);

        public abstract Rectangle RepositionImage(Image desktopImage);

        public abstract Rectangle GetMouseMoveRectangle();

        public abstract Point GetMouseMovePoint(Point current);

        public abstract Point UpdateRemotePointer(Point current);
    }
}
