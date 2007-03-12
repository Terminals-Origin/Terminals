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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
	public class FloatingForm : Form, IHotZoneSource, IMessageFilter
	{
		// Class constants
		private const int HITTEST_CAPTION = 2;

		// Instance variables
        protected Zone _zone;
        protected bool _intercept;
        protected RedockerContent _redocker;
        protected DockingManager _dockingManager;

        // Instance events
        public event ContextHandler Context;
        
        public FloatingForm(DockingManager dockingManager, Zone zone, ContextHandler contextHandler)
        {
            // The caller is responsible for setting our initial screen location
            this.StartPosition = FormStartPosition.Manual;

            // Not in task bar to prevent clutter
            this.ShowInTaskbar = false;
        
            // Make sure the main Form owns us
            this.Owner = dockingManager.Container.FindForm();
            
            // Need to know when the Zone is removed
            this.ControlRemoved += new ControlEventHandler(OnZoneRemoved);
            
            // Add the Zone as the only content of the Form
            Controls.Add(zone);

            // Default state
            _redocker = null;
            _intercept = false;
            _zone = zone;
            _dockingManager = dockingManager;

            // Assign any event handler for context menu
            if (contextHandler != null)
                this.Context += contextHandler;	

            // Default color
            this.BackColor = _dockingManager.BackColor;
            this.ForeColor = _dockingManager.InactiveTextColor;

            // Monitor changes in the Zone content
            _zone.Windows.Inserted += new CollectionChange(OnWindowInserted);
            _zone.Windows.Removing += new CollectionChange(OnWindowRemoving);
            _zone.Windows.Removed += new CollectionChange(OnWindowRemoved);

            if (_zone.Windows.Count == 1)
            {
                // The first Window to be added. Tell it to hide details
                _zone.Windows[0].HideDetails(); 
                
                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged += new EventHandler(OnFullTitleChanged);  

                // Grab any existing title
                this.Text = _zone.Windows[0].FullTitle;
            }
            
            // Need to hook into message pump so that the ESCAPE key can be 
            // intercepted when in redocking mode
            Application.AddMessageFilter(this);
        }

        public DockingManager DockingManager
        {
            get { return _dockingManager; }
        }

        public Zone Zone
        {
            get { return this.Controls[0] as Zone; }
		}

        public void PropogateNameValue(PropogateName name, object value)
        {
            if (this.Zone != null)
                this.Zone.PropogateNameValue(name, value);
        }
        
        public void AddHotZones(Redocker redock, HotZoneCollection collection)
        {
            RedockerContent redocker = redock as RedockerContent;

            // Allow the contained Zone a chance to expose HotZones
            foreach(Control c in this.Controls)
            {
                IHotZoneSource ag = c as IHotZoneSource;

                // Does this control expose an interface for its own HotZones?
                if (ag != null)
                    ag.AddHotZones(redock, collection);
            }
        }
           
        protected void OnWindowInserted(int index, object value)
        {
            if (_zone.Windows.Count == 1)
            {
                // The first Window to be added. Tell it to hide details
                _zone.Windows[0].HideDetails();                
                
                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged += new EventHandler(OnFullTitleChanged);  

                // Grab any existing title
                this.Text = _zone.Windows[0].FullTitle;
            }
            else if (_zone.Windows.Count == 2)
            {
				int pos = 0;
			
				// If the new Window is inserted at beginning then update the second Window
				if (index == 0)
					pos++;

                // The second Window to be added. Tell the first to now show details
                _zone.Windows[pos].ShowDetails();                
                
                // Monitor change in window title
                _zone.Windows[pos].FullTitleChanged -= new EventHandler(OnFullTitleChanged);  

                // Remove any caption title
                this.Text = "";
            }
        }
           
        protected void OnWindowRemoving(int index, object value)
        {
            if (_zone.Windows.Count == 1)
            {   
                // The first Window to be removed. Tell it to show details as we want 
                // to restore the Window state before it might be moved elsewhere
                _zone.Windows[0].ShowDetails();                
                
                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged -= new EventHandler(OnFullTitleChanged);  
                
                // Remove any existing title text
                this.Text = "";
            }
        }

        protected void OnWindowRemoved(int index, object value)
        {
            if (_zone.Windows.Count == 1)
            {   
                // Window removed leaving just one left. Tell it to hide details
                _zone.Windows[0].HideDetails();                

                // Monitor change in window title
                _zone.Windows[0].FullTitleChanged += new EventHandler(OnFullTitleChanged);  
                
                // Grab any existing title text
                this.Text = _zone.Windows[0].FullTitle;
            }
        }
        
        protected void OnFullTitleChanged(object sender, EventArgs e)
        {
            // Unbox sent string
            this.Text = (string)sender;
        }
        
        protected void OnZoneRemoved(object sender, ControlEventArgs e)
        {
			// Is it the Zone being removed for a hidden button used to help
			// remove controls without hitting the 'form refuses to close' bug
			if (e.Control == _zone)
			{
				if (_zone.Windows.Count == 1)
				{   
					// The first Window to be removed. Tell it to show details as we want 
					// to restore the Window state before it might be moved elsewhere
					_zone.Windows[0].ShowDetails();                

					// Remove monitor change in window title
					_zone.Windows[0].FullTitleChanged -= new EventHandler(OnFullTitleChanged);  
				}
        
				// Monitor changes in the Zone content
				_zone.Windows.Inserted -= new CollectionChange(OnWindowInserted);
				_zone.Windows.Removing -= new CollectionChange(OnWindowRemoving);
				_zone.Windows.Removed -= new CollectionChange(OnWindowRemoved);

				// No longer required, commit suicide
				this.Dispose();
			}
        }
        
        protected override CreateParams CreateParams 
        {
            get 
            {
                // Let base class fill in structure first
                CreateParams cp = base.CreateParams;

                // The only way to get a caption bar with only small 
                // close button is by providing this extended style
                cp.ExStyle |= (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW;

                return cp;
            }
        }

        public virtual void OnContext(Point screenPos)
        {
            // Any attached event handlers?
            if (Context != null)
                Context(screenPos);
        }

		public void ExitFloating()
		{
			if (_zone != null)
			{
				ContentCollection cc = ZoneHelper.Contents(_zone);

				// Record restore object for each Content
				foreach(Content c in cc)
				{
					c.RecordFloatingRestore();
					c.Docked = true;
				}
			}
		}

        protected void Restore()
        {
			if (_zone != null)
			{
				ContentCollection cc = ZoneHelper.Contents(_zone);

				// Record restore object for each Content
				foreach(Content c in cc)
				{
					c.RecordFloatingRestore();
					c.Docked = true;
				}

				// Ensure each content is removed from any Parent
				foreach(Content c in cc)
					_dockingManager.HideContent(c, false, true);
				
				// Now restore each of the Content
				foreach(Content c in cc)
					_dockingManager.ShowContent(c);

				_dockingManager.UpdateInsideFill();
			}

			this.Close();
        }

		protected override void OnMove(EventArgs e)
		{
			Point newPos = this.Location;
			
			// Grab the aggregate collection of all Content objects in the Zone
			ContentCollection cc = ZoneHelper.Contents(_zone);
			
			// Update each one with the new FloatingForm location
			foreach(Content c in cc)
				c.DisplayLocation = newPos;			

			base.OnMove(e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (_zone != null)
			{
				ContentCollection cc = ZoneHelper.Contents(_zone);

				// Record restore object for each Content
				foreach(Content c in cc)
					c.RecordRestore();

				// Ensure each content is removed from any Parent
				foreach(Content c in cc)
                {
					// Is content allowed to be hidden?
                    if (!_dockingManager.OnContentHiding(c))
						_dockingManager.HideContent(c, false, true);
					else
					{
						// At least one Content refuses to die, so do not
						// let the whole floating form be closed down
						e.Cancel = true;
					}
                }
			}

			// Must set focus back to the main application Window
			if (this.Owner != null)
				this.Owner.Activate();

			base.OnClosing(e);
		}

        protected override void OnResize(System.EventArgs e)
        {
            // Grab the aggregate collection of all Content objects in the Zone
            ContentCollection cc = ZoneHelper.Contents(_zone);
			
			// Do not include the caption height of the tool window in the saved height
			Size newSize = new Size(this.Width, this.Height - SystemInformation.ToolWindowCaptionHeight);
			
            // Update each one with the new FloatingForm location
            foreach(Content c in cc)
                c.FloatingSize = newSize;

            base.OnResize(e);
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Has a key been pressed?
            if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN)
            {
                // Is it the ESCAPE key?
                if ((int)m.WParam == (int)Win32.VirtualKeys.VK_ESCAPE)
                {                   
                    // Are we in a redocking activity?
                    if (_intercept)
                    {
                        // Quite redocking
                        _redocker.QuitTrackingMode(null);

                        // Release capture
                        this.Capture = false;
                    
                        // Reset state
                        _intercept = false;

                        return true;
                    }
                }
            }
            
            return false;
        }

        protected override void WndProc(ref Message m)
		{
			// Want to notice when the window is maximized
			if (m.Msg == (int)Win32.Msgs.WM_NCLBUTTONDBLCLK)
			{
				// Redock and kill ourself
				Restore();

				// We do not want to let the base process the message as the 
				// restore might fail due to lack of permission to restore to 
				// old state.  In that case we do not want to maximize the window
				return;
			}
			else if (m.Msg == (int)Win32.Msgs.WM_NCLBUTTONDOWN)
			{
				if (!_intercept)
				{
					// Perform a hit test against our own window to determine 
					// which area the mouse press is over at the moment.
					uint result = User32.SendMessage(this.Handle, (int)Win32.Msgs.WM_NCHITTEST, 0, (uint)m.LParam);
                
					// Only want to override the behviour of moving the window via the caption box
					if (result == HITTEST_CAPTION)
					{
						// Remember new state
						_intercept = true;
                    
						// Capture the mouse until the mouse us is received
						this.Capture = true;
                        
						// Ensure that we gain focus and look active
						this.Activate();

						// Get mouse position to inscreen coordinates
						Win32.POINT mousePos;
						mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
						mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);

						// Find adjustment to bring screen to client coordinates
						Point topLeft = PointToScreen(new Point(0, 0));
						topLeft.Y -= SystemInformation.CaptionHeight;
						topLeft.X -= SystemInformation.BorderSize.Width;

						// Begin a redocking activity
						_redocker = new RedockerContent(this, new Point(mousePos.x - topLeft.X, 
							                            mousePos.y - topLeft.Y));
                        
                        
						return;
					}
				}
			}
			else if (m.Msg == (int)Win32.Msgs.WM_MOUSEMOVE)
			{
				if (_intercept)
				{
					Win32.POINT mousePos;
					mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
					mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);

					_redocker.OnMouseMove(new MouseEventArgs(MouseButtons.Left, 
						                  0, 
						                  mousePos.x, 
						                  mousePos.y, 
						                  0));
                
					return;
				}
			}
			else if (m.Msg == (int)Win32.Msgs.WM_LBUTTONUP)
			{
				if (_intercept)
				{
					Win32.POINT mousePos;
					mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
					mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);
		
					_redocker.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 0, 
						                                   mousePos.x, mousePos.y, 0));

					// Release capture
					this.Capture = false;
                    
					// Reset state
					_intercept = false;

					return;
				}
			} 
			else if ((m.Msg == (int)Win32.Msgs.WM_NCRBUTTONUP) ||
				     (m.Msg == (int)Win32.Msgs.WM_NCMBUTTONDOWN) ||
				     (m.Msg == (int)Win32.Msgs.WM_NCMBUTTONUP) ||
			         (m.Msg == (int)Win32.Msgs.WM_RBUTTONDOWN) ||
				     (m.Msg == (int)Win32.Msgs.WM_RBUTTONUP) ||
			         (m.Msg == (int)Win32.Msgs.WM_MBUTTONDOWN) ||
				     (m.Msg == (int)Win32.Msgs.WM_MBUTTONUP))
			{
			    // Prevent middle and right mouse buttons from interrupting
			    // the correct operation of left mouse dragging
			    return;
			} 
			else if (m.Msg == (int)Win32.Msgs.WM_NCRBUTTONDOWN)
			{
			    if (!_intercept)
			    {
				    // Get screen coordinates of the mouse
                    Win32.POINT mousePos;
                    mousePos.x = (short)((uint)m.LParam & 0x0000FFFFU);
                    mousePos.y = (short)(uint)(((uint)m.LParam & 0xFFFF0000U) >> 16);
        			
				    // Box to transfer as parameter
				    OnContext(new Point(mousePos.x, mousePos.y));

                    return;		
                }
			}

			base.WndProc(ref m);
		}
	}
}
