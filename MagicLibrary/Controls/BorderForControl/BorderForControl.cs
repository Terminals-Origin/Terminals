// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Crownwood.Magic.Controls
{
    [ToolboxItem(false)]
    public class BorderForControl : UserControl 
    {
        // Instance fields
        protected int _borderWidth = 2;

        public BorderForControl()
        {
            InternalConstruct(null, _borderWidth);
        }

        public BorderForControl(Control control)
        {
            InternalConstruct(control, _borderWidth);
        }

        public BorderForControl(Control control, int borderWidth)
        {
            InternalConstruct(control, borderWidth);
        }

        protected void InternalConstruct(Control control, int borderWidth)
        {
            // Remember parameter
            _borderWidth = borderWidth;
			
            if (control != null)
            {
                // Remove any existing docking style for passed in Control
                control.Dock = DockStyle.None;

                // Add to appearance
                Controls.Add(control);
            }
        }	

        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        protected override void OnResize(EventArgs e)
        {
            ResizeOnlyTheChild();
            base.OnResize(e);
        }
		
        protected override void OnLayout(LayoutEventArgs e)	
        {
            ResizeOnlyTheChild();
            base.OnLayout(e);
        }

        protected void ResizeOnlyTheChild()
        {
            // Do we have a child control to resize? 
            if (Controls.Count >= 1)
            {
                Size ourSize = this.Size;

                // Get the first child (there should not be any others)
                Control child = this.Controls[0];

                // Define new position
                child.Location = new Point(_borderWidth, _borderWidth);

                // Define new size
                child.Size = new Size(ourSize.Width - _borderWidth * 2, ourSize.Height - _borderWidth * 2);
            }
        }	
    }
}
