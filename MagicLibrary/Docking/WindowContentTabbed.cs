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
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    [ToolboxItem(false)]
    public class WindowContentTabbed : WindowContent, IHotZoneSource, IMessageFilter
    {
        // Class constants
        protected static int _plainBorder = 3;
        protected static int _hotAreaInflate = -3;

        // Instance fields
        protected int _dragPageIndex;
        protected Content _activeContent;
        protected RedockerContent _redocker;
        protected Magic.Controls.TabControl _tabControl;

        public WindowContentTabbed(DockingManager manager, VisualStyle vs)
            : base(manager, vs)
        {
            _redocker = null;
            _activeContent = null;
            
            // Create the TabControl used for viewing the Content windows
            _tabControl = new Magic.Controls.TabControl();

            // It should always occupy the remaining space after all details
            _tabControl.Dock = DockStyle.Fill;

            // Show tabs only if two or more tab pages exist
            _tabControl.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.HideUsingLogic;
            
            // Hook into the TabControl notifications
            _tabControl.GotFocus += new EventHandler(OnTabControlGotFocus);
            _tabControl.LostFocus += new EventHandler(OnTabControlLostFocus);
            _tabControl.PageGotFocus += new EventHandler(OnTabControlGotFocus);
            _tabControl.PageLostFocus += new EventHandler(OnTabControlLostFocus);
            _tabControl.SelectionChanged += new EventHandler(OnSelectionChanged);
            _tabControl.PageDragStart += new MouseEventHandler(OnPageDragStart);
            _tabControl.PageDragMove += new MouseEventHandler(OnPageDragMove);
            _tabControl.PageDragEnd += new MouseEventHandler(OnPageDragEnd);
            _tabControl.PageDragQuit += new MouseEventHandler(OnPageDragQuit);
            _tabControl.DoubleClickTab += new Magic.Controls.TabControl.DoubleClickTabHandler(OnDoubleClickTab);
			_tabControl.Font = manager.TabControlFont;
            _tabControl.BackColor = manager.BackColor;
            _tabControl.ForeColor = manager.InactiveTextColor;

            // Define the visual style required
            _tabControl.Style = vs;

			// Allow developers a chance to override default settings
			manager.OnTabControlCreated(_tabControl);

            switch(vs)
            {
                case VisualStyle.IDE:
                    Controls.Add(_tabControl);
                    break;
                case VisualStyle.Plain:
                    // Only the border at the pages edge and not around the whole control
                    _tabControl.InsetBorderPagesOnly = !_manager.PlainTabBorder;

                    // We want a border around the TabControl so it is indented and looks consistent
                    // with the Plain look and feel, so use the helper Control 'BorderForControl'
                    BorderForControl bfc = new BorderForControl(_tabControl, _plainBorder);

                    // It should always occupy the remaining space after all details
                    bfc.Dock = DockStyle.Fill;

                    // Define the default border border
                    bfc.BackColor = _manager.BackColor;

                    // When in 'VisualStyle.Plain' we need to 
                    Controls.Add(bfc);
                    break;
            }

            // Need to hook into message pump so that the ESCAPE key can be 
            // intercepted when in redocking mode
            Application.AddMessageFilter(this);
        }

        public Content CurrentContent
        {
            get
            {
                Magic.Controls.TabPage tp = _tabControl.SelectedTab;
                
                if (tp != null)
                    return (Content)tp.Tag;
                else
                    return null;
            }
        }

		public Magic.Controls.TabControl TabControl
		{
			get { return _tabControl; } 
		}

        public void HideCurrentContent()
        {
            Magic.Controls.TabPage tp = _tabControl.SelectedTab;
            
			int count = _tabControl.TabPages.Count;

			// Find currently selected tab
			int index = _tabControl.SelectedIndex;

			// Decide which other tab to make selected instead
			if (count > 1)
			{
				// Move to the next control along
				int newSelect = index + 1;

				// Wrap around to first tab if at end
				if (newSelect == count)
					newSelect = 0;

				// Change selection
				_tabControl.SelectedIndex = newSelect;
			}
			else
			{
				// Hide myself as am about to die
				this.Hide();

				// Ensure the focus goes somewhere else
				_manager.Container.Focus();
			}

            if (tp != null)
			{
				// Have the manager perform the Hide operation for us
				_manager.HideContent(tp.Tag as Content);
			}
        }
        
		public override void BringContentToFront(Content c)
		{
            // Find the matching Page and select it
            foreach(Magic.Controls.TabPage page in _tabControl.TabPages)
                if (page.Tag == c)
                {
                    _tabControl.SelectedTab = page;
                    break;
                }
		}

        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);
        
            switch(name)
            {
                case PropogateName.BackColor:
                    Color newColor = (Color)value;
            
                    // In Plain style we need to color the intermidiate window as well    
                    if (_style == VisualStyle.Plain)
                    {
                        BorderForControl bfc = this.Controls[0] as BorderForControl;
                        bfc.BackColor = newColor;                               
                    }

                    _tabControl.BackColor = newColor;
                    this.BackColor = newColor;
                    
                    Invalidate();
                    break;
                case PropogateName.InactiveTextColor:
                    _tabControl.ForeColor = (Color)value;
                    break;
                case PropogateName.PlainTabBorder:
                    _tabControl.InsetBorderPagesOnly = !(bool)value;
                    break;
				case PropogateName.TabControlFont:
					_tabControl.Font = (Font)value;
					break;
            }
        }

        protected override void OnContentsClearing()
        {
            _tabControl.TabPages.Clear();

            base.OnContentsClearing();

            if (!this.AutoDispose)
            {
                // Inform each detail of the change in title text
                NotifyFullTitleText("");
            }
        }

        protected override void OnContentInserted(int index, object value)
        {
            base.OnContentInserted(index, value);

            Content content = value as Content;

            // Create TabPage to represent the Content
            Magic.Controls.TabPage newPage = new Magic.Controls.TabPage();

            // Reflect the Content properties int the TabPage
            newPage.Title = content.Title;
            newPage.ImageList = content.ImageList;
            newPage.ImageIndex = content.ImageIndex;
            newPage.Control = content.Control;
            newPage.Tag = content;
			
            // Reflect same order in TabPages collection as Content collection
            _tabControl.TabPages.Insert(index, newPage);
        }

        protected override void OnContentRemoving(int index, object value)
        {
            base.OnContentRemoving(index, value);

            Content c = value as Content;

            // Find the matching Page and remove it
            foreach(Magic.Controls.TabPage page in _tabControl.TabPages)
                if (page.Tag == c)
                {
                    _tabControl.TabPages.Remove(page);
                    break;
                }
        }

        public override void WindowDetailGotFocus(WindowDetail wd)
        {
            // Transfer focus from WindowDetail to the TabControl
            _tabControl.Focus();
        }

        protected void OnSelectionChanged(object sender, EventArgs e)
        {
            if (_tabControl.TabPages.Count == 0)
            {
                // Inform each detail of the change in title text
                NotifyFullTitleText("");
            }
            else
            {
                // Inform each detail of the new title text
                if (_tabControl.SelectedIndex != -1)
                {
                    Content selectedContent = _tabControl.SelectedTab.Tag as Content;
                    
                    NotifyAutoHideImage(selectedContent.AutoHidden);
                    NotifyCloseButton(selectedContent.CloseButton);
                    NotifyHideButton(selectedContent.HideButton);
                    NotifyFullTitleText(selectedContent.FullTitle);
                    NotifyShowCaptionBar(selectedContent.CaptionBar);
                }
            }
        }

        protected void OnTabControlGotFocus(object sender, EventArgs e)
        {
            NotifyContentGotFocus();
        }

        protected void OnTabControlLostFocus(object sender, EventArgs e)
        {
            NotifyContentLostFocus();
        }

        public void AddHotZones(Redocker redock, HotZoneCollection collection)
        {
            RedockerContent redocker = redock as RedockerContent;

            bool itself = false;
            bool nullZone = false;

            // We process differently for WindowContent to redock into itself!
            if ((redocker.WindowContent != null) && (redocker.WindowContent == this))
                itself = true;

            // We do not allow a Content to redock into its existing container
            if (itself && !_contents.Contains(redocker.Content))
                nullZone = true;

            Rectangle newSize = this.RectangleToScreen(this.ClientRectangle);
            Rectangle hotArea = _tabControl.RectangleToScreen(_tabControl.ClientRectangle);;

            // Find any caption detail and use that area as the hot area
            foreach(WindowDetail wd in _windowDetails)
            {
                WindowDetailCaption wdc = wd as WindowDetailCaption;

                if (wdc != null)
                {
                    hotArea = wdc.RectangleToScreen(wdc.ClientRectangle);
                    hotArea.Inflate(_hotAreaInflate, _hotAreaInflate);
                    break;
                }
            }

            if (nullZone)
                collection.Add(new HotZoneNull(hotArea));
            else
                collection.Add(new HotZoneTabbed(hotArea, newSize, this, itself));				
        }

		protected void OnDoubleClickTab(Magic.Controls.TabControl tc, Magic.Controls.TabPage page)
		{
			Content c = (Content)page.Tag;

			// Make Content record its current position and remember it for the future 
			c.RecordRestore();

			// Invert docked status
			c.Docked = (c.Docked == false);

			// Remove from current WindowContent and restore its position
			_manager.HideContent(c, false, true);
			_manager.ShowContent(c);
		}
		
        protected void OnPageDragStart(object sender, MouseEventArgs e)
        {
            if (this.RedockAllowed)
            {
                // There must be a selected page for this event to occur
                Magic.Controls.TabPage page = _tabControl.SelectedTab;

                // Event page must specify its Content object
                Content c = page.Tag as Content;

                // Remember the position of the tab before it is removed
                _dragPageIndex = _tabControl.TabPages.IndexOf(page);

                // Remove page from TabControl
                _tabControl.TabPages.Remove(page);

                // Force the entire window to redraw to ensure the Redocker does not start drawing
                // the XOR indicator before the window repaints itself. Otherwise the repainted
                // control will interfere with the XOR indicator.
                this.Refresh();

                // Start redocking activity for the single Content of this WindowContent
                _redocker = new RedockerContent(_tabControl, c, this, new Point(e.X, e.Y));
            }
        }

        protected void OnPageDragMove(object sender, MouseEventArgs e)
        {
            // Redocker object needs mouse movements
            if (_redocker != null)
                _redocker.OnMouseMove(e);
        }

        protected void OnPageDragEnd(object sender, MouseEventArgs e)
        {
            // Are we currently in a redocking state?
            if (_redocker != null)
            {
                // Let the redocker finish off
                bool moved = _redocker.OnMouseUp(e);

                // If the tab was not positioned somewhere else
                if (!moved)
                {
                    // Put back the page that was removed when dragging started
                    RestoreDraggingPage();
                }

                // No longer need the object
                _redocker = null;
            }
        }

        protected void OnPageDragQuit(object sender, MouseEventArgs e)
        {
            // Are we currently in a redocking state?
            if (_redocker != null)
            {
                // Put back the page that was removed when dragging started
                RestoreDraggingPage();

                // No longer need the object
                _redocker = null;
            }
        }

        protected override void OnContentChanged(Content obj, Content.Property prop)
        {
            // Find the matching TabPage entry
            foreach(Magic.Controls.TabPage page in _tabControl.TabPages)
            {
                // Does this page manage our Content?
                if (page.Tag == obj)
                {
                    // Property specific processing
                    switch(prop)
                    {
                        case Content.Property.Control:
                            page.Control = obj.Control;
                            break;
                        case Content.Property.Title:
                            page.Title = obj.Title;
                            break;
                        case Content.Property.FullTitle:
                            // Title changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                // Update displayed caption text
                                NotifyFullTitleText(obj.FullTitle);
                            }								
                            break;
                        case Content.Property.ImageList:
                            page.ImageList = obj.ImageList;
                            break;
                        case Content.Property.ImageIndex:
                            page.ImageIndex = obj.ImageIndex;
                            break;
                        case Content.Property.CaptionBar:
                            // Property changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                Content target = page.Tag as Content;
                        
                                // TODO
                                NotifyShowCaptionBar(target.CaptionBar);                                
                            }
                            break;
                        case Content.Property.CloseButton:
                            // Property changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                Content target = page.Tag as Content;
                        
                                NotifyCloseButton(target.CloseButton);   
                            }
                            break;
                        case Content.Property.HideButton:
                            // Property changed for the current page?
                            if (_tabControl.SelectedTab == page)
                            {
                                Content target = page.Tag as Content;
                        
                                NotifyHideButton(target.HideButton);   
                            }
                            break;
                    }

                    break;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            // Inform each Content of its actual displayed size
            foreach(Content c in _contents)
            {
                switch(_state)
                {
                    case State.DockLeft:
                    case State.DockRight:
						if (this.Dock != DockStyle.Fill)
						{
							// Only update the remembered width
							c.DisplaySize = new Size(this.ClientSize.Width, c.DisplaySize.Height);
						}
                        break;
                    case State.DockTop:
                    case State.DockBottom:
						if (this.Dock != DockStyle.Fill)
						{
							// Only update the remembered height
							c.DisplaySize = new Size(c.DisplaySize.Width, this.ClientSize.Height);
						}
                        break;
                    case State.Floating:
						{
							Form f = this.FindForm();

							// Update width and height
							if (f == null)
								c.DisplaySize = this.ClientSize;
							else
								c.DisplaySize = f.Size;
						}
                        break;
                }
            }

            base.OnResize(e);
        }
        
        public override Restore RecordRestore(object child) 
        {
            // Child of a WindowContent must be a Content object
            Content c = child as Content;

            StringCollection next = new StringCollection();
            StringCollection previous = new StringCollection();

            bool before = true;

            // Fill collections with list of Content before and after parameter
            foreach(Content content in _contents)
            {
                if (content == c)
                    before = false;
                else
                {
                    if (before)
                        previous.Add(content.Title);
                    else
                        next.Add(content.Title);
                }
            }

            bool selected = false;

            // Is there a selected tab?
            if (_tabControl.SelectedIndex != -1)
            {
                // Get access to the selected Content
                Content selectedContent = _tabControl.SelectedTab.Tag as Content;

                // Need to record if it is selected
                selected = (selectedContent == c);
            }

            // Create a Restore object to handle this WindowContent
            Restore thisRestore = new RestoreWindowContent(null, c, next, previous, selected);

            // Do we have a Zone as our parent?
            if (_parentZone != null)
            {
                // Get the Zone to prepend its own Restore knowledge
                thisRestore = _parentZone.RecordRestore(this, child, thisRestore);
            }

            return thisRestore;
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Has a key been pressed?
            if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN)
            {
                // Is it the ESCAPE key?
                if ((int)m.WParam == (int)Win32.VirtualKeys.VK_ESCAPE)
                {                   
                    // Are we in redocking mode?
                    if (_redocker != null)
                    {
                        // Cancel the redocking activity
                        _redocker.QuitTrackingMode(null);

                        // Put back the page that was removed when dragging started
                        RestoreDraggingPage();
                        
                        // No longer need the object
                        _redocker = null;
                        
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        protected void RestoreDraggingPage()
        {
            // Create TabPage to represent the Content
            Magic.Controls.TabPage newPage = new Magic.Controls.TabPage();

            Content content = _redocker.Content;

            // Reflect the Content properties int the TabPage
            newPage.Title = content.Title;

            newPage.ImageList = content.ImageList;
            newPage.ImageIndex = content.ImageIndex;
            newPage.Control = content.Control;
            newPage.Tag = content;
            newPage.Selected = true;
		
            // Put it back where it came from
            _tabControl.TabPages.Insert(_dragPageIndex, newPage);

            // Update the title displayed
            NotifyFullTitleText(content.FullTitle);
        }
    }
}