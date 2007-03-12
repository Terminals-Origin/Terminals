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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.Magic.Common;

namespace Crownwood.Magic.Docking
{
    [ToolboxItem(false)]
    public class WindowDetail : Control
    {
        // Instance fields
        protected Zone _parentZone;
        protected Window _parentWindow;
        protected DockingManager _manager;

        public WindowDetail(DockingManager manager)
        {
            // Default the state
            _parentZone = null;
            _parentWindow = null;
            _manager = manager;
            
            // Get correct starting state from manager
            this.BackColor = _manager.BackColor;            
            this.ForeColor = _manager.InactiveTextColor;
        }
		
        public virtual Zone ParentZone
        { 
            get { return _parentZone; }
            set { _parentZone = value; }
        }

        public Window ParentWindow
        { 
            get { return _parentWindow; }
			
            set 
            {
                // Call virtual function for derived classes to override
                RemovedFromParent(_parentWindow);

                if (value == null)
                {
                    if (_parentWindow != null)
                    {
                        // Remove ourself from old parent window

						// Use helper method to circumvent form Close bug
						ControlHelper.Remove(_parentWindow.Controls, this);
                    }
                }
                else
                {
                    if ((_parentWindow != null) && (_parentWindow != value))
                    {
                        // Call virtual function for derived classes to override
                        RemovedFromParent(_parentWindow);

                        // Remove ourself from old parent window

						// Use helper method to circumvent form Close bug
						ControlHelper.Remove(_parentWindow.Controls, this);
                    }
	
                    // Add ourself to the new parent window
                    value.Controls.Add(this);
                }

                // Remember the new parent identity
                _parentWindow = value;

                // Call virtual function for derived classes to override
                AddedToParent(_parentWindow);

                if (_parentWindow != null)
                {
                    // Update to reflect new parent window state
                    ParentStateChanged(_parentWindow.State);
                }
            }
        }

        public virtual void WindowGotFocus() {}
        public virtual void WindowLostFocus() {}
        public virtual void NotifyRedockAllowed(bool redockAllowed) {}
        public virtual void NotifyAutoHideImage(bool autoHidden) {}
        public virtual void NotifyCloseButton(bool show) {}
        public virtual void NotifyHideButton(bool show) {}
        public virtual void NotifyShowCaptionBar(bool show) {}
        public virtual void NotifyFullTitleText(string title) {}
        public virtual void ParentStateChanged(State newState) {}
        public virtual void RemovedFromParent(Window parent) {}
        public virtual void AddedToParent(Window parent) {}

        public virtual void PropogateNameValue(PropogateName name, object value)
        {
            if (name == PropogateName.BackColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            // Inform parent window we have the focus
            if (_parentWindow != null)
                _parentWindow.WindowDetailGotFocus(this);

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            // Inform parent window we have lost focus
            if (_parentWindow != null)
                _parentWindow.WindowDetailLostFocus(this);

            base.OnLostFocus(e);
        }		
    }
}