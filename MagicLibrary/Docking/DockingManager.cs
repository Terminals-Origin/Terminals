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
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Win32;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    public enum PropogateName
    {
        BackColor,
        ActiveColor,
        ActiveTextColor,
        InactiveTextColor,
        ResizeBarColor,
        ResizeBarVector,
        CaptionFont,
		TabControlFont,
        ZoneMinMax,
        PlainTabBorder
    }

    public class DockingManager
    {
        // Instance fields
        protected bool _zoneMinMax;
        protected bool _insideFill;
        protected bool _autoResize;
        protected bool _firstHalfWidth;
        protected bool _firstHalfHeight;
        protected int _surpressVisibleEvents;
        protected int _resizeBarVector;
        protected Size _innerMinimum;
        protected Color _backColor;
        protected Color _activeColor;
        protected Color _activeTextColor;
        protected Color _inactiveTextColor;
        protected Color _resizeBarColor;
        protected Font _captionFont;
		protected Font _tabControlFont;
        protected bool _defaultBackColor;
        protected bool _defaultActiveColor;
        protected bool _defaultActiveTextColor;
        protected bool _defaultInactiveTextColor;
        protected bool _defaultResizeBarColor;
        protected bool _defaultCaptionFont;
		protected bool _defaultTabControlFont;
        protected bool _plainTabBorder;
        protected Control _innerControl;
        protected Control _outerControl;
        protected AutoHidePanel _ahpTop;
        protected AutoHidePanel _ahpLeft;
        protected AutoHidePanel _ahpBottom;
        protected AutoHidePanel _ahpRight;
        protected VisualStyle _visualStyle;
        protected ContainerControl _container;
        protected ManagerContentCollection _contents;

        public delegate void ContentHandler(Content c, EventArgs cea);
        public delegate void ContentHidingHandler(Content c, CancelEventArgs cea);
        public delegate void ContextMenuHandler(PopupMenu pm, CancelEventArgs cea);
		public delegate void TabControlCreatedHandler(Magic.Controls.TabControl tabControl);
		public delegate void SaveCustomConfigHandler(XmlTextWriter xmlOut);
        public delegate void LoadCustomConfigHandler(XmlTextReader xmlIn);

        // Exposed events
        public event ContentHandler ContentShown;
        public event ContentHandler ContentHidden;
        public event ContentHidingHandler ContentHiding;
        public event ContextMenuHandler ContextMenu;
		public event TabControlCreatedHandler TabControlCreated;
		public event SaveCustomConfigHandler SaveCustomConfig;
        public event LoadCustomConfigHandler LoadCustomConfig;

        public DockingManager(ContainerControl container, VisualStyle vs)
        {
            // Must provide a valid container instance
            if (container == null)
                throw new ArgumentNullException("Container");

            // Default state
            _container = container;
            _visualStyle = vs;
            _innerControl = null;
			_zoneMinMax = true;
			_insideFill = false;
			_autoResize = true;
			_firstHalfWidth = true;
			_firstHalfHeight = true;
			_plainTabBorder = false;
			_surpressVisibleEvents = 0;
			_innerMinimum = new Size(20, 20);
	
            // Default font/resize
			_resizeBarVector = -1;
			_captionFont = SystemInformation.MenuFont;
			_tabControlFont = SystemInformation.MenuFont;
			_defaultCaptionFont = true;
			_defaultTabControlFont = true;

			// Create and add hidden auto hide panels
			AddAutoHidePanels();

            // Define initial colors
            ResetColors();

            // Create an object to manage the collection of Content
            _contents = new ManagerContentCollection(this);

            // We want notification when contents are removed/cleared
            _contents.Clearing += new CollectionClear(OnContentsClearing);
            _contents.Removed += new CollectionChange(OnContentRemoved);

			// We want to perform special action when container is resized
			_container.Resize += new EventHandler(OnContainerResized);
			
			// A Form can cause the child controls to be reordered after the initialisation
			// but before the Form.Load event. To handle this we hook into the event and force
			// the auto hide panels to be ordered back into their proper place.
			if (_container is Form)
			{   
			    Form formContainer = _container as Form;			    
			    formContainer.Load += new EventHandler(OnFormLoaded);
			}

            // Need notification when colors change
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(OnPreferenceChanged);
        }

        public ContainerControl Container
        {
            get { return _container; }
        }

        public Control InnerControl
        {
            get { return _innerControl; }
            set { _innerControl = value; }
        }

        public Control OuterControl
        {
            get { return _outerControl; }
            set 
			{
			    if (_outerControl != value)
			    {
				    _outerControl = value;
				    
				    // Use helper routine to ensure panels are in correct positions
                    ReorderAutoHidePanels();
		        }
			}
        }

        public ManagerContentCollection Contents
        {
            get { return _contents; }
			
            set 
            {
                _contents.Clear();
                _contents = value;	
            }
        }

		public bool ZoneMinMax
		{
			get { return _zoneMinMax; }

			set 
			{ 
			    if (value != _zoneMinMax)
			    {
			        _zoneMinMax = value;
                
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.ZoneMinMax, (object)_zoneMinMax);
                } 
			}
		}

		public bool InsideFill
		{
			get { return _insideFill; }

			set
			{
				if (_insideFill != value)
				{
					_insideFill = value;

					if (_insideFill)
					{
					    // Add Fill style to innermost docking window
						AddInnerFillStyle();
			        }
					else
					{
					    // Remove Fill style from innermost docking window
						RemoveAnyFillStyle();
						
						// Ensure that inner control can be seen
                        OnContainerResized(null, EventArgs.Empty);
					}
				}
			}
		}

		public bool AutoResize
		{
			get { return _autoResize; }
			set { _autoResize = value; }
		}

		public Size InnerMinimum
		{
			get { return _innerMinimum; }
			set { _innerMinimum = value; }
		}

        public VisualStyle Style
        {
            get { return _visualStyle; }
        }

        public int ResizeBarVector
        {
            get { return _resizeBarVector; }
            
            set 
            {
                if (value != _resizeBarVector)
                {
                    _resizeBarVector = value;
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.ResizeBarVector, (object)_resizeBarVector);
                }
            }
        }

        public Color BackColor
        {
            get { return _backColor; }
            
            set 
            {
                if (value != _backColor)
                {
                    _backColor = value;
                    _defaultBackColor = (_backColor == SystemColors.Control);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.BackColor, (object)_backColor);
                }
            }
        }
    
        public Color ActiveColor
        {
            get { return _activeColor; }
            
            set 
            {
                if (value != _activeColor)
                {
                    _activeColor = value;
                    _defaultActiveColor = (_activeColor == SystemColors.ActiveCaption);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.ActiveColor, (object)_activeColor);
                }
            }
        }
        
        public Color ActiveTextColor
        {
            get { return _activeTextColor; }
            
            set 
            {
                if (value != _activeTextColor)
                {
                    _activeTextColor = value;
                    _defaultActiveTextColor = (_activeTextColor == SystemColors.ActiveCaptionText);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.ActiveTextColor, (object)_activeTextColor);
                }
            }
        }

        public Color InactiveTextColor
        {
            get { return _inactiveTextColor; }
            
            set 
            {
                if (value != _inactiveTextColor)
                {
                    _inactiveTextColor = value;
                    _defaultInactiveTextColor = (_inactiveTextColor == SystemColors.ControlText);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.InactiveTextColor, (object)_inactiveTextColor);
                }
            }
        }

        public Color ResizeBarColor
        {
            get { return _resizeBarColor; }
            
            set 
            {
                if (value != _resizeBarColor)
                {
                    _resizeBarColor = value;
                    _defaultResizeBarColor = (_resizeBarColor == SystemColors.Control);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.ResizeBarColor, (object)_resizeBarColor);
                }
            }
        }
        
        public Font CaptionFont
        {
            get { return _captionFont; }
            
            set 
            {
                if (value != _captionFont)
                {
                    _captionFont = value;
                    _defaultCaptionFont = (_captionFont == SystemInformation.MenuFont);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.CaptionFont, (object)_captionFont);
                }
            }
        }

        public Font TabControlFont
        {
            get { return _tabControlFont; }
            
            set 
            {
                if (value != _tabControlFont)
                {
                    _tabControlFont = value;
                    _defaultTabControlFont = (_captionFont == SystemInformation.MenuFont);
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.TabControlFont, (object)_tabControlFont);
                }
            }
        }

        public bool PlainTabBorder
        {
            get { return _plainTabBorder; }
            
            set 
            {
                if (value != _plainTabBorder)
                {
                    _plainTabBorder = value;
                    
                    // Notify each object in docking hierarchy in case they need to know new value
                    PropogateNameValue(PropogateName.PlainTabBorder, (object)_plainTabBorder);
                }
            }
        }

        public void ResetColors()
        {
            _backColor = SystemColors.Control;
            _inactiveTextColor = SystemColors.ControlText;
            _activeColor = SystemColors.ActiveCaption;
            _activeTextColor = SystemColors.ActiveCaptionText;
            _resizeBarColor = SystemColors.Control;
            _defaultBackColor = true;
            _defaultActiveColor = true;
            _defaultActiveTextColor = true;
            _defaultInactiveTextColor = true;
            _defaultResizeBarColor = true;

            PropogateNameValue(PropogateName.BackColor, (object)_backColor);
            PropogateNameValue(PropogateName.ActiveColor, (object)_activeColor);
            PropogateNameValue(PropogateName.ActiveTextColor, (object)_activeTextColor);
            PropogateNameValue(PropogateName.InactiveTextColor, (object)_inactiveTextColor);
            PropogateNameValue(PropogateName.ResizeBarColor, (object)_resizeBarColor);
        }

        public void UpdateInsideFill()
		{
			// Is inside fill ability enabled?
			if (_insideFill)
			{
				// Reduce flicker
				_container.SuspendLayout();
				
				// Ensure correct zone has the Fill style
				RemoveAnyFillStyle();
				AddInnerFillStyle();

				_container.ResumeLayout();
			}
		}

		public virtual bool ShowContent(Content c)
		{
            // Validate the incoming Content instance is a valid reference
            // and is a current instance within our internal collection
            if ((c == null) || !_contents.Contains(c))
                return false;
		
			// Remove it from view by removing from current WindowContent container
            if (!c.Visible)
			{
                // Do not generate hiding/hidden/shown events
                _surpressVisibleEvents++;

                // Manageing Zones should remove display AutoHide windows
                RemoveShowingAutoHideWindows();
                               
                // Use the assigned restore object to position the Content appropriately
				if (c.Docked)
				{
				    if (c.AutoHidden)
				        c.AutoHideRestore.PerformRestore(this);
				    else
					    c.DockingRestore.PerformRestore(this);
			    }
				else
					c.FloatingRestore.PerformRestore(this);

                // Enable generation hiding/hidden/shown events
                _surpressVisibleEvents--;

                // Generate event
				OnContentShown(c);

				return true;
			}
			else
				return false;
		}

		public virtual void ShowAllContents()
		{
			_container.SuspendLayout();

			foreach(Content c in _contents)
				ShowContent(c);

			UpdateInsideFill();

			_container.ResumeLayout();
		}

		public virtual void HideContent(Content c)
		{
			HideContent(c, true, true);
		}

		public virtual void HideContent(Content c, bool record, bool reorder)
		{
            // Remove it from view by removing from current WindowContent container
            if (c.Visible)
            {
                // Do not generate hiding/hidden/shown events
                _surpressVisibleEvents++;

                // Manageing Zones should remove display AutoHide windows
                RemoveShowingAutoHideWindows();
                
                if (record)
				{
					// Tell the Content to create a new Restore object to record its current location
					c.RecordRestore();
				}

                if (c.AutoHidden)
                {
                    // Remove it from its current AutoHidePanel
                    c.AutoHidePanel.RemoveContent(c);
                }
                else
                {
                    // Remove the Content from its current WindowContent
                    c.ParentWindowContent.Contents.Remove(c);
                }
                
				if (reorder)
				{
					// Move the Content to the start of the list
					_contents.SetIndex(0, c); 
				}

				UpdateInsideFill();

                // Enable generation hiding/hidden/shown events
                _surpressVisibleEvents--;
                
                // Generate event
				OnContentHidden(c);
            }
		}

		public virtual void HideAllContents()
		{
			_container.SuspendLayout();

			int count = _contents.Count;

			// Hide in reverse order so that a ShowAll in forward order gives accurate restore
			for(int index=count-1; index>=0; index--)
			{
			    // Cannot hide something already hidden
			    if (_contents[index].Visible)
			    {
                    // Generate event
                    if (!OnContentHiding(_contents[index]))
                    {
                        HideContent(_contents[index], true, false);
                    }
                }
		    }

			UpdateInsideFill();

			_container.ResumeLayout();
		}

        public virtual Window CreateWindowForContent(Content c)
        {
            return CreateWindowForContent(c, new EventHandler(OnContentClose), 
										     new EventHandler(OnRestore),
                                             new EventHandler(OnInvertAutoHide),
                                             new ContextHandler(OnShowContextMenu));
        }

        public virtual Window CreateWindowForContent(Content c,
                                                     EventHandler contentClose,
                                                     EventHandler restore,
                                                     EventHandler invertAutoHide,
                                                     ContextHandler showContextMenu)
        {
            // Create new instance with correct style
            WindowContent wc = new WindowContentTabbed(this, _visualStyle);

            WindowDetailCaption wdc;

            // Create a style specific caption detail
            if (_visualStyle == VisualStyle.IDE)
                wdc = new WindowDetailCaptionIDE(this, contentClose, restore,
                                                 invertAutoHide, showContextMenu);
            else
                wdc = new WindowDetailCaptionPlain(this, contentClose, restore,
                                                   invertAutoHide, showContextMenu);

            // Add the caption to the window display
            wc.WindowDetails.Add(wdc);

            if (c != null)
            {
                // Add provided Content to this instance
                wc.Contents.Add(c);
            }

            return wc;
        }    
            
        public virtual Zone CreateZoneForContent(State zoneState)
        {
			return CreateZoneForContent(zoneState, _container);
		}

        protected virtual Zone CreateZoneForContent(State zoneState, ContainerControl destination)
        {
            DockStyle ds;
            Direction direction;

            // Find relevant values dependant on required state
            ValuesFromState(zoneState, out ds, out direction);

            // Create a new ZoneSequence which can host Content
            ZoneSequence zs = new ZoneSequence(this, zoneState, _visualStyle, direction, _zoneMinMax);

            // Set the appropriate docking style
            zs.Dock = ds;

			if (destination != null)
			{
				// Add this Zone to the display
				destination.Controls.Add(zs);
			}

            return zs;
        }

        public WindowContent AddContentWithState(Content c, State newState)
        {
            // Validate the incoming Content instance is a valid reference
            // and is a current instance within our internal collection
            if ((c == null) || !_contents.Contains(c))
                return null;
		
            // Do not generate hiding/hidden/shown events
            _surpressVisibleEvents++;

            // Manageing Zones should remove display AutoHide windows
            RemoveShowingAutoHideWindows();
                
            // Is the window already part of a WindowContent?
            if (c.ParentWindowContent != null)
            {
				// If it used to be in a floating mode, then record state change
				if (c.ParentWindowContent.ParentZone.State == State.Floating)
					c.ContentLeavesFloating();

                // Remove the Content from its current WindowContent
                c.ParentWindowContent.Contents.Remove(c);
            }

            // Create a new Window instance appropriate for hosting a Content object
            Window w = CreateWindowForContent(c);

			ContainerControl destination = null;

	        if (newState != State.Floating)
			{
				destination = _container;
		        destination.SuspendLayout();
			}

            // Create a new Zone capable of hosting a WindowContent
            Zone z = CreateZoneForContent(newState, destination);

	        if (newState == State.Floating)
			{
			    // Content is not in the docked state
			    c.Docked = false;
			
				// destination a new floating form
				destination = new FloatingForm(this, z, new ContextHandler(OnShowContextMenu));

				// Define its location
				destination.Location = c.DisplayLocation;
				
				// ...and its size, add the height of the caption bar to the requested content size
				destination.Size = new Size(c.FloatingSize.Width, 
				                            c.FloatingSize.Height + SystemInformation.ToolWindowCaptionHeight);
			}
			
            // Add the Window to the Zone
            z.Windows.Add(w);

	        if (newState != State.Floating)
			{
				// Set the Zone to be the least important of our Zones
				ReorderZoneToInnerMost(z);

				UpdateInsideFill();

	            destination.ResumeLayout();
			}
			else
				destination.Show();

            // Enable generation hiding/hidden/shown events
            _surpressVisibleEvents--;

            // Generate event to indicate content is now visible
            OnContentShown(c);

            return w as WindowContent;
        }

        public WindowContent AddContentToWindowContent(Content c, WindowContent wc)
        {
            // Validate the incoming Content instance is a valid reference
            // and is a current instance within our internal collection
            if ((c == null) || !_contents.Contains(c))
                return null;

            // Validate the incoming WindowContent instance is a valid reference
            if (wc == null)
                return null;

            // Is Content already part of given Window then nothing to do
            if (c.ParentWindowContent == wc)
                return wc;
            else
            {
                // Do not generate hiding/hidden/shown events
                _surpressVisibleEvents++;

                // Manageing Zones should remove display AutoHide windows
                RemoveShowingAutoHideWindows();
                
                if (c.ParentWindowContent != null)
                {
					// Is there a change in docking state?
					if (c.ParentWindowContent.ParentZone.State != wc.ParentZone.State)
					{
						// If it used to be in a floating mode, then record state change
						if (c.ParentWindowContent.ParentZone.State == State.Floating)
							c.ContentLeavesFloating();
						else
							c.ContentBecomesFloating();
					}

                    // Remove the Content from its current WindowContent
                    c.ParentWindowContent.Contents.Remove(c);
                }
                else
                {
                    // If adding to a floating window then it is not docked
                    if (wc.ParentZone.State == State.Floating)
                        c.Docked = false;
                }

                // Add the existing Content to this instance
                wc.Contents.Add(c);

                // Enable generation hiding/hidden/shown events
                _surpressVisibleEvents--;

                // Generate event to indicate content is now visible
                OnContentShown(c);

                return wc;
            }
        }

        public Window AddContentToZone(Content c, Zone z, int index)
        {
            // Validate the incoming Content instance is a valid reference
            // and is a current instance within our internal collection
            if ((c == null) || !_contents.Contains(c))
                return null;

            // Validate the incoming Zone instance is a valid reference
            if (z == null) 
                return null;

            // Do not generate hiding/hidden/shown events
            _surpressVisibleEvents++;

            // Manageing Zones should remove display AutoHide windows
            RemoveShowingAutoHideWindows();
                
            // Is the window already part of a WindowContent?
            if (c.ParentWindowContent != null)
            {
				// Is there a change in docking state?
				if (c.ParentWindowContent.ParentZone.State != z.State)
				{
					// If it used to be in a floating mode, then record state change
					if (c.ParentWindowContent.ParentZone.State == State.Floating)
						c.ContentLeavesFloating();
					else
						c.ContentBecomesFloating();
				}

                // Remove the Content from its current WindowContent
                c.ParentWindowContent.Contents.Remove(c);
            }
            else
            {
                // If target zone is floating window then we are no longer docked
                if (z.State == State.Floating)
                    c.Docked = false;
            }

            // Create a new WindowContent instance according to our style
            Window w = CreateWindowForContent(c);

            // Add the Window to the Zone at given position
            z.Windows.Insert(index, w);

            // Enable generation hiding/hidden/shown events
            _surpressVisibleEvents--;

            // Generate event to indicate content is now visible
            OnContentShown(c);

            return w;
        }

		public Rectangle InnerResizeRectangle(Control source)
		{
			// Start with a rectangle that represents the entire client area
			Rectangle client = _container.ClientRectangle;

			int count = _container.Controls.Count;
			int inner = _container.Controls.IndexOf(_innerControl);
			int sourceIndex = _container.Controls.IndexOf(source);

			// Process each control outside the inner control
			for(int index=count-1; index>inner; index--)
			{
				Control item = _container.Controls[index];

				bool insideSource = (index < sourceIndex);

				switch(item.Dock)
				{
				    case DockStyle.Left:
					    client.Width -= item.Width;
					    client.X += item.Width;

					    if (insideSource)
						    client.Width -= item.Width;
					    break;
				    case DockStyle.Right:
					    client.Width -= item.Width;

					    if (insideSource)
					    {
						    client.Width -= item.Width;
						    client.X += item.Width;
					    }
					    break;
				    case DockStyle.Top:
					    client.Height -= item.Height;
					    client.Y += item.Height;

					    if (insideSource)
						    client.Height -= item.Height;
					    break;
				    case DockStyle.Bottom:
					    client.Height -= item.Height;

					    if (insideSource)
					    {
						    client.Height -= item.Height;
						    client.Y += item.Height;
					    }
					    break;
				    case DockStyle.Fill:
				    case DockStyle.None:
					    break;
				}
			}

			return client;
		}

        public void ReorderZoneToInnerMost(Zone zone)
        {
            int index = 0;

            // If there is no control specified as the one for all Zones to be placed
            // in front of then simply add the Zone at the start of the list so it is
            // in front of all controls.
            if (_innerControl != null)
            {
                // Find position of specified control and place after it in the list 
                // (hence adding one to the returned value)
                index = _container.Controls.IndexOf(_innerControl) + 1;
            }

            // Find current position of the Zone to be repositioned
            int current = _container.Controls.IndexOf(zone);

            // If the old position is before the new position then we need to 
            // subtract one. As the collection will remove the Control from the
            // old position before inserting it in the new, thus reducing the index
            // by 1 before the insert occurs.
            if (current < index)
                index--;

            // Found a Control that is not a Zone, so need to insert straight it
            _container.Controls.SetChildIndex(zone, index);
            
            // Manageing Zones should remove display AutoHide windows
            RemoveShowingAutoHideWindows();
        }

        public void ReorderZoneToOuterMost(Zone zone)
        {
            // Get index of the outer control (minus AutoHidePanel's)
            int index = OuterControlIndex();

            // Find current position of the Zone to be repositioned
            int current = _container.Controls.IndexOf(zone);

            // If the old position is before the new position then we need to 
            // subtract one. As the collection will remove the Control from the
            // old position before inserting it in the new, thus reducing the index
            // by 1 before the insert occurs.
            if (current < index)
                index--;

            // Found a Control that is not a Zone, so need to insert straight it
            _container.Controls.SetChildIndex(zone, index);

            // Manageing Zones should remove display AutoHide windows
            RemoveShowingAutoHideWindows();
        }
        
        public int OuterControlIndex()
        {
            int index = _container.Controls.Count;

            // If there is no control specified as the one for all Zones to be placed behind 
            // then simply add the Zone at the end of the list so it is behind all controls.
            if (_outerControl != null)
            {
                // Find position of specified control and place before it in the list 
                index = _container.Controls.IndexOf(_outerControl);
            }

            // Adjust backwards to prevent being after any AutoHidePanels
            for(; index>0; index--)
                if (!(_container.Controls[index-1] is AutoHidePanel))
                    break;
                    
            return index;
        }

        public void RemoveShowingAutoHideWindows()
        {
            _ahpLeft.RemoveShowingWindow();
            _ahpRight.RemoveShowingWindow();
            _ahpTop.RemoveShowingWindow();
            _ahpBottom.RemoveShowingWindow();
        }
        
        internal void RemoveShowingAutoHideWindowsExcept(AutoHidePanel except)
        {
            if (except != _ahpLeft)
                _ahpLeft.RemoveShowingWindow();

            if (except != _ahpRight)
                _ahpRight.RemoveShowingWindow();
            
            if (except != _ahpTop)
                _ahpTop.RemoveShowingWindow();
            
            if (except != _ahpBottom)
                _ahpBottom.RemoveShowingWindow();
        }

        public void BringAutoHideIntoView(Content c)
        {
            if (_ahpLeft.ContainsContent(c))
                _ahpLeft.BringContentIntoView(c);     

            if (_ahpRight.ContainsContent(c))
                _ahpRight.BringContentIntoView(c);     

            if (_ahpTop.ContainsContent(c))
                _ahpTop.BringContentIntoView(c);     

            if (_ahpBottom.ContainsContent(c))
                _ahpBottom.BringContentIntoView(c);     
        }            
        
        public void ValuesFromState(State newState, out DockStyle dockState, out Direction direction)
        {
            switch(newState)
            {
                case State.Floating:
                    dockState = DockStyle.Fill;
                    direction = Direction.Vertical;
                    break;
                case State.DockTop:
                    dockState = DockStyle.Top;
                    direction = Direction.Horizontal;
                    break;
                case State.DockBottom:
                    dockState = DockStyle.Bottom;
                    direction = Direction.Horizontal;
                    break;
                case State.DockRight:
                    dockState = DockStyle.Right;
                    direction = Direction.Vertical;
                    break;
                case State.DockLeft:
                default:
                    dockState = DockStyle.Left;
                    direction = Direction.Vertical;
                    break;
            }
        }

		public byte[] SaveConfigToArray()
		{
			return SaveConfigToArray(Encoding.Unicode);	
		}

		public byte[] SaveConfigToArray(Encoding encoding)
		{
			// Create a memory based stream
			MemoryStream ms = new MemoryStream();
			
			// Save into the file stream
			SaveConfigToStream(ms, encoding);

			// Must remember to close
			ms.Close();

			// Return an array of bytes that contain the streamed XML
			return ms.GetBuffer();
		}

		public void SaveConfigToFile(string filename)
		{
			SaveConfigToFile(filename, Encoding.Unicode);
		}

		public void SaveConfigToFile(string filename, Encoding encoding)
		{
			// Create/Overwrite existing file
			FileStream fs = new FileStream(filename, FileMode.Create);
			
			// Save into the file stream
			SaveConfigToStream(fs, encoding);		

			// Must remember to close
			fs.Close();
		}

		public void SaveConfigToStream(Stream stream, Encoding encoding)
		{
			XmlTextWriter xmlOut = new XmlTextWriter(stream, encoding); 

			// Use indenting for readability
			xmlOut.Formatting = Formatting.Indented;
			
			// Always begin file with identification and warning
			xmlOut.WriteStartDocument();
			xmlOut.WriteComment(" Magic, The User Interface library for .NET (www.dotnetmagic.com) ");
			xmlOut.WriteComment(" Modifying this generated file will probably render it invalid ");

			// Associate a version number with the root element so that future version of the code
			// will be able to be backwards compatible or at least recognise out of date versions
			xmlOut.WriteStartElement("DockingConfig");
			xmlOut.WriteAttributeString("FormatVersion", "5");
			xmlOut.WriteAttributeString("InsideFill", _insideFill.ToString());
			xmlOut.WriteAttributeString("InnerMinimum", ConversionHelper.SizeToString(_innerMinimum));

			// We need to hide all content during the saving process, but then restore
			// them back again before leaving so the user does not see any change
			_container.SuspendLayout();

			// Store a list of those content hidden during processing
			ContentCollection hideContent = new ContentCollection();

			// Let create a copy of the current contents in current order, because
			// we cannot 'foreach' a collection that is going to be altered during its
			// processing by the 'HideContent'.
			ContentCollection origContents = _contents.Copy();

            // Do not generate hiding/hidden/shown events
            _surpressVisibleEvents++;
            
            int count = origContents.Count;

            // Hide in reverse order so that a ShowAll in forward order gives accurate restore
            for(int index=count-1; index>=0; index--)
            {
                Content c = origContents[index];
            
				c.RecordRestore();
				c.SaveToXml(xmlOut);

				// If visible then need to hide so that subsequent attempts to 
				// RecordRestore will not take its position into account
				if (c.Visible)
				{
					hideContent.Insert(0, c);
					HideContent(c);
				}
			}
			
			// Allow an event handler a chance to add custom information after ours
			OnSaveCustomConfig(xmlOut);

			// Put content we hide back again
			foreach(Content c in hideContent)
				ShowContent(c);

            // Enable generation of hiding/hidden/shown events
            _surpressVisibleEvents--;
            
            // Reapply any fill style required
			AddInnerFillStyle();

			_container.ResumeLayout();

			// Terminate the root element and document        
			xmlOut.WriteEndElement();
			xmlOut.WriteEndDocument();

			// This should flush all actions and close the file
			xmlOut.Close();			
		}

		public void LoadConfigFromArray(byte[] buffer)
		{
			// Create a memory based stream
			MemoryStream ms = new MemoryStream(buffer);
			
			// Save into the file stream
			LoadConfigFromStream(ms);

			// Must remember to close
			ms.Close();
		}

		public void LoadConfigFromFile(string filename)
		{
			// Open existing file
			FileStream fs = new FileStream(filename, FileMode.Open);
			
			// Load from the file stream
			LoadConfigFromStream(fs);		

			// Must remember to close
			fs.Close();
		}

		public void LoadConfigFromStream(Stream stream)
		{
			XmlTextReader xmlIn = new XmlTextReader(stream); 

			// Ignore whitespace, not interested
			xmlIn.WhitespaceHandling = WhitespaceHandling.None;

			// Moves the reader to the root element.
			xmlIn.MoveToContent();

			// Double check this has the correct element name
			if (xmlIn.Name != "DockingConfig")
				throw new ArgumentException("Root element must be 'DockingConfig'");

			// Load the format version number
			string version = xmlIn.GetAttribute(0);
			string insideFill = xmlIn.GetAttribute(1);
			string innerSize = xmlIn.GetAttribute(2);

            // Convert format version from string to double
            int formatVersion = (int)Convert.ToDouble(version);
            
            // We can only load 3 upward version formats
            if (formatVersion < 3)
                throw new ArgumentException("Can only load Version 3 and upwards Docking Configuration files");

            // Convert from string to proper types
			_insideFill = (bool)Convert.ToBoolean(insideFill);
			_innerMinimum = ConversionHelper.StringToSize(innerSize);

			ContentCollection cc = new ContentCollection();

			do
			{
                // Read the next Element
				if (!xmlIn.Read())
					throw new ArgumentException("An element was expected but could not be read in");

				// Have we reached the end of root element?
				if ((xmlIn.NodeType == XmlNodeType.EndElement) && (xmlIn.Name == "DockingConfig"))
					break;

				// Is the element name 'Content'
				if (xmlIn.Name == "Content")
				{
                    // Process this Content element
                    cc.Insert(0, new Content(xmlIn, formatVersion));
                }
				else
				{
				    // Must have reached end of our code, let the custom handler deal with this
                    OnLoadCustomConfig(xmlIn);

                    // Ignore anything else that might be in the XML
                    xmlIn.Close();			
                   
                    // Exit
                    break;
                }

			} while(!xmlIn.EOF);

			xmlIn.Close();			

			// Reduce flicker during window operations
			_container.SuspendLayout();

			// Hide all the current content items
			HideAllContents();

			// Attempt to apply loaded settings
			foreach(Content loaded in cc)
			{
				Content c = _contents[loaded.Title];

				// Do we have any loaded information for this item?
				if (c != null)
				{
					// Copy across the loaded values of interest
                    c.Docked = loaded.Docked;
                    c.AutoHidden = loaded.AutoHidden;
                    c.CaptionBar = loaded.CaptionBar;
                    c.CloseButton = loaded.CloseButton;
                    c.DisplaySize = loaded.DisplaySize;
					c.DisplayLocation = loaded.DisplayLocation;
					c.AutoHideSize = loaded.AutoHideSize;
					c.FloatingSize = loaded.FloatingSize;
					c.DefaultRestore = loaded.DefaultRestore;
					c.AutoHideRestore = loaded.AutoHideRestore;
					c.DockingRestore = loaded.DockingRestore;
					c.FloatingRestore = loaded.FloatingRestore;

					// Allow the Restore objects a chance to rehook into object instances
					c.ReconnectRestore();					

					// Was the loaded item visible?
					if (loaded.Visible)
					{
						// Make it visible now
						ShowContent(c);
					}
				}
			}

			// Reapply any fill style required
			AddInnerFillStyle();

			// Reduce flicker during window operations
			_container.ResumeLayout();
			
			// If any AutoHostPanel's have become visible we need to force a repaint otherwise
			// the area not occupied by the TabStub instances will be painted the correct color
			_ahpLeft.Invalidate();
            _ahpRight.Invalidate();
            _ahpTop.Invalidate();
            _ahpBottom.Invalidate();
        }
        
        public void PropogateNameValue(PropogateName name, object value)
        {
            foreach(Control c in _container.Controls)
            {
                Zone z = c as Zone;

                // Only interested in our Zones
                if (z != null)
                    z.PropogateNameValue(name, value);
            }

            // If the docking manager is created for a Container that does not
            // yet have a parent control then we need to double check before
            // trying to enumerate the owned forms.
            if (_container.FindForm() != null)
            {
                foreach(Form f in _container.FindForm().OwnedForms)
                {
                    FloatingForm ff = f as FloatingForm;
                    
                    // Only interested in our FloatingForms
                    if (ff != null)
                        ff.PropogateNameValue(name, value);
                }
            }
            
            // Propogate into the AutoHidePanel objects
            _ahpTop.PropogateNameValue(name, value);
            _ahpLeft.PropogateNameValue(name, value);
            _ahpRight.PropogateNameValue(name, value);
            _ahpBottom.PropogateNameValue(name, value);
        }

		public virtual bool OnContentHiding(Content c)
        {
            CancelEventArgs cea = new CancelEventArgs();

            if (_surpressVisibleEvents == 0)
            {
                // Allow user to prevent hide operation                
                if (ContentHiding != null)
                    ContentHiding(c, cea);
            }
            
            // Was action cancelled?                        
            return cea.Cancel;
        }

		public virtual void OnContentHidden(Content c)
        {
            if (_surpressVisibleEvents == 0)
            {
                // Notify operation has completed
                if (ContentHidden != null)
                    ContentHidden(c, EventArgs.Empty);
            }
        }

		public virtual void OnContentShown(Content c)
        {
            if (_surpressVisibleEvents == 0)
            {
                // Notify operation has completed
                if (ContentShown != null)
                    ContentShown(c, EventArgs.Empty);
            }
        }

		public virtual void OnTabControlCreated(Magic.Controls.TabControl tabControl)
		{ 
			// Notify interested parties about creation of a new TabControl instance
			if (TabControlCreated != null)
				TabControlCreated(tabControl);
		}
		
		public virtual void OnSaveCustomConfig(XmlTextWriter xmlOut)
		{
            // Notify interested parties that they can add their own custom data
            if (SaveCustomConfig != null)
                SaveCustomConfig(xmlOut);
        }

        public virtual void OnLoadCustomConfig(XmlTextReader xmlIn)
        {
            // Notify interested parties that they can add their own custom data
            if (LoadCustomConfig != null)
                LoadCustomConfig(xmlIn);
        }
        
        protected virtual void OnContentsClearing()
        {
            _container.SuspendLayout();

            // Remove each Content from any WindowContent it is inside
            foreach(Content c in _contents)
            {
                // Is the Content inside a WindowContent?
                if (c.ParentWindowContent != null)
                    c.ParentWindowContent.Contents.Remove(c);
            }

            _container.ResumeLayout();
        }

        protected virtual void OnContentRemoved(int index, object value)
        {
            _container.SuspendLayout();

            Content c = value as Content;

            if (c != null)
            {
                // Is the Content inside a WindowContent?
                if (c.ParentWindowContent != null)
                    c.ParentWindowContent.Contents.Remove(c);
            }

            _container.ResumeLayout();
        }

        protected virtual void OnContentClose(object sender, EventArgs e)
        {
            WindowDetailCaption wdc = sender as WindowDetailCaption;
            
            // Was Close generated by a Caption detail?
            if (wdc != null)
            {
                WindowContentTabbed wct = wdc.ParentWindow as WindowContentTabbed;
                
                // Is the Caption part of a WindowContentTabbed object?
                if (wct != null)
                {
                    // Find the Content object that is the target
                    Content c = wct.CurrentContent;
                    
                    if (c != null)
                    {
                        // Was action cancelled?                        
                        if (!OnContentHiding(c))
                            wct.HideCurrentContent();
                    }
                }
            }
        }
        
        protected virtual void OnInvertAutoHide(object sender, EventArgs e)
        {
            // Do not generate hiding/hidden/shown events
            _surpressVisibleEvents++;
        
            WindowDetail detail = sender as WindowDetail;

            // Get access to Content that initiated AutoHide for its Window
            WindowContent wc = detail.ParentWindow as WindowContent;
                        
            // Create a collection of the Content in the same window
            ContentCollection cc = new ContentCollection();
            
            // Add all Content into collection
            foreach(Content c in wc.Contents)
                cc.Add(c);

            // Add to the correct AutoHidePanel
            AutoHideContents(cc, wc.State);

            // Enable generate hiding/hidden/shown events
            _surpressVisibleEvents--;
        }
        
        internal AutoHidePanel AutoHidePanelForState(State state)
        {
            AutoHidePanel ahp = null;

            // Grab the correct hosting panel
            switch(state)
            {
                case State.DockLeft:
                    ahp = _ahpLeft;
                    break;
                case State.DockRight:
                    ahp = _ahpRight;
                    break;
                case State.DockTop:
                    ahp = _ahpTop;
                    break;
                case State.DockBottom:
                    ahp = _ahpBottom;
                    break;
            }

            return ahp;
        }
        
        internal void AutoHideContents(ContentCollection cc, State state)
        {
            // Hide all the Content instances. This will cause the restore objects to be 
            // created and so remember the docking positions for when they are restored
            foreach(Content c in cc)
                HideContent(c);

            AutoHidePanel ahp = AutoHidePanelForState(state);

            // Pass management of Contents into the panel            
            ahp.AddContentsAsGroup(cc);
        }

        internal AutoHidePanel AutoHidePanelForContent(Content c)
        {
            if (_ahpLeft.ContainsContent(c))
                return _ahpLeft;     

            if (_ahpRight.ContainsContent(c))
                return _ahpRight;     

            if (_ahpTop.ContainsContent(c))
                return _ahpTop;     

            if (_ahpBottom.ContainsContent(c))
                return _ahpBottom;     
                
            return null;
        }

        internal int SurpressVisibleEvents
        {
            get { return _surpressVisibleEvents; }
            set { _surpressVisibleEvents = value; }
        }

        protected void AddAutoHidePanels()
        {
            // Create an instance for each container edge (they default to being hidden)
            _ahpTop = new AutoHidePanel(this, DockStyle.Top);
            _ahpLeft = new AutoHidePanel(this, DockStyle.Left);
            _ahpBottom = new AutoHidePanel(this, DockStyle.Bottom);
            _ahpRight = new AutoHidePanel(this, DockStyle.Right);
        
			_ahpTop.Name = "Top";
			_ahpLeft.Name = "Left";
			_ahpBottom.Name = "Bottom";
			_ahpRight.Name = "Right";
		    
            // Add to the end of the container we manage
            _container.Controls.AddRange(new Control[]{_ahpBottom, _ahpTop, _ahpRight, _ahpLeft});
		}
		            
        protected void RepositionControlBefore(Control target, Control source)
        {
            // Find indexs of the two controls
            int targetPos = _container.Controls.IndexOf(target);
            int sourcePos = _container.Controls.IndexOf(source);

            // If the source is being moved further up the list then we must decrement the target index 
            // as the move is carried out in two phases. First the source control is removed from the 
            // collection and then added at the given requested index. So when insertion point needs 
            // ahjusting to reflec the fact the control has been removed before being inserted.
            if (targetPos >= sourcePos)
                targetPos--;

            _container.Controls.SetChildIndex(source, targetPos);			
        }

        protected virtual void OnRestore(object sender, EventArgs e)
        {
            WindowDetailCaption wdc = sender as WindowDetailCaption;

			// Was Restore generated by a Caption detail?
			if (wdc != null)
			{
				RemoveAnyFillStyle();

                WindowContent wc = wdc.ParentWindow as WindowContent;

				// Is the Caption part of a WindowContent object?
				if (wc != null)
				{
					ContentCollection copy = new ContentCollection();

					// Make every Content of the WindowContent record its
					// current position and remember it for when the future
					foreach(Content c in wc.Contents)
					{
						c.RecordRestore();

						// Invert docked status
						c.Docked = (c.Docked == false);

						copy.Add(c);
					}

					int copyCount = copy.Count;

					// Must have at least one!
					if (copyCount >= 1)
					{
						// Remove from current WindowContent and restore its position
						HideContent(copy[0], false, true);
						ShowContent(copy[0]);

						// Any other content to be moved along with it?
						if (copyCount >= 2)
						{
							WindowContent newWC = copy[0].ParentWindowContent;

							if (newWC != null)
							{
								// Transfer each one to its new location
								for(int index=1; index<copyCount; index++)
								{
									HideContent(copy[index], false, true);
									newWC.Contents.Add(copy[index]);
								}
							}
						}
					}
				}

				AddInnerFillStyle();
			}
		}

		protected void AddInnerFillStyle()
		{
			if (_insideFill)
			{
				// Find the innermost Zone which must be the first one in the collection
				foreach(Control c in _container.Controls)
				{
					Zone z = c as Zone;

					// Only interested in our Zones
					if (z != null)
					{
						// Make it fill all remaining space
						z.Dock = DockStyle.Fill;

						// Exit
						break;
					}
				}
			}
		}

		protected void RemoveAnyFillStyle()
		{
			// Check each Zone in the container
			foreach(Control c in _container.Controls)
			{
				Zone z = c as Zone;

				if (z != null)
				{
					// Only interested in ones with the Fill dock style
					if (z.Dock == DockStyle.Fill)
					{
						DockStyle ds;
						Direction direction;

						// Find relevant values dependant on required state
						ValuesFromState(z.State, out ds, out direction);
			
						// Reassign its correct Dock style
						z.Dock = ds;
					}
				}
			}
		}

        protected void OnFormLoaded(object sender, EventArgs e)
        {
            // A Form can cause the child controls to be reordered after the initialisation
            // but before the Form.Load event. To handle this we reorder the auto hide panels
            // on the Form.Load event to ensure they are correctly positioned.
            ReorderAutoHidePanels();
        }

        protected void ReorderAutoHidePanels()
        {
            if (_outerControl == null)
            {
                int count = _container.Controls.Count;

                // Position the AutoHidePanel's at end of controls
                _container.Controls.SetChildIndex(_ahpLeft, count - 1);
                _container.Controls.SetChildIndex(_ahpRight, count - 1);
                _container.Controls.SetChildIndex(_ahpTop, count - 1);
                _container.Controls.SetChildIndex(_ahpBottom, count - 1);
            }
            else
            {
                // Position the AutoHidePanel's as last items before OuterControl
                RepositionControlBefore(_outerControl, _ahpBottom);
                RepositionControlBefore(_outerControl, _ahpTop);
                RepositionControlBefore(_outerControl, _ahpRight);
                RepositionControlBefore(_outerControl, _ahpLeft);
            }
        }
        
        protected void OnContainerResized(object sender, EventArgs e)
		{
		    if (_autoResize)
		    {
			    Rectangle inner = InnerResizeRectangle(null);			

			    // Shrink by the minimum size
			    inner.Width -= _innerMinimum.Width;
			    inner.Height -= _innerMinimum.Height;
    			
			    Form f = _container as Form;

			    // If the container is a Form then ignore resizing because of becoming Minimized
			    if ((f == null) || ((f != null) && (f.WindowState != FormWindowState.Minimized)))
			    {
				    if ((inner.Width < 0) || (inner.Height < 0))
				    {
					    _container.SuspendLayout();

					    ZoneCollection zcLeft = new ZoneCollection();
					    ZoneCollection zcRight = new ZoneCollection();
					    ZoneCollection zcTop = new ZoneCollection();
					    ZoneCollection zcBottom = new ZoneCollection();

					    // Construct a list of the docking windows on the left and right edges
					    foreach(Control c in _container.Controls)
					    {
						    Zone z = c as Zone;

						    if (z != null)
						    {
							    switch(z.State)
							    {
							        case State.DockLeft:
								        zcLeft.Add(z);
								        break;
							        case State.DockRight:
								        zcRight.Add(z);
								        break;
							        case State.DockTop:
								        zcTop.Add(z);
								        break;
							        case State.DockBottom:
								        zcBottom.Add(z);
								        break;
							    }
						    }
					    }

					    if (inner.Width < 0)
						    ResizeDirection(-inner.Width, zcLeft, zcRight, Direction.Horizontal);

					    if (inner.Height < 0)
						    ResizeDirection(-inner.Height, zcTop, zcBottom, Direction.Vertical);

					    _container.ResumeLayout();
				    }
			    }
	        }
		}

		protected void ResizeDirection(int remainder, ZoneCollection zcAlpha, ZoneCollection zcBeta, Direction dir)
		{
			bool alter;
			int available;
			int half1, half2;

			// Keep going till all space found or nowhere to get it from
			while((remainder > 0) && ((zcAlpha.Count > 0) || (zcBeta.Count > 0)))
			{
				if (dir == Direction.Horizontal)
				{
					_firstHalfWidth = (_firstHalfWidth != true);
					alter = _firstHalfWidth;
				}
				else
				{
					_firstHalfHeight = (_firstHalfHeight != true);
					alter = _firstHalfHeight;
				}

				// Alternate between left and right getting the remainder
				if (alter)
				{
					half1 = (remainder / 2) + 1;
					half2 = remainder - half1;
				}
				else
				{
					half2 = (remainder / 2) + 1;
					half1 = remainder - half2;
				}

				// Any Zone of the left to use?
				if (zcAlpha.Count > 0)
				{
					Zone z = zcAlpha[0];

					// Find how much space it can offer up
					if (dir == Direction.Horizontal)
						available = z.Width - z.MinimumWidth;
					else
						available = z.Height - z.MinimumHeight;

					if (available > 0)
					{
						// Only take away the maximum we need
						if (available > half1)
							available = half1;
						else
							zcAlpha.Remove(z);

						// Resize the control accordingly
						if (dir == Direction.Horizontal)
							z.Width = z.Width - available;
						else
							z.Height = z.Height - available;

						// Reduce total amount left to allocate
						remainder -= available;
					}
					else
						zcAlpha.Remove(z);
				}

				// Any Zone of the left to use?
				if (zcBeta.Count > 0)
				{
					Zone z = zcBeta[0];

					// Find how much space it can offer up
					if (dir == Direction.Horizontal)
						available = z.Width - z.MinimumWidth;
					else
						available = z.Height - z.MinimumHeight;

					if (available > 0)
					{
						// Only take away the maximum we need
						if (available > half2)
							available = half2;
						else
							zcBeta.Remove(z);

						// Resize the control accordingly
						if (dir == Direction.Horizontal)
							z.Width = z.Width - available;
						else
							z.Height = z.Height - available;

						// Reduce total amount left to allocate
						remainder -= available;
					}
					else
						zcBeta.Remove(z);
				}
			}
		}

        protected void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (_defaultBackColor)
            {
                _backColor = SystemColors.Control;
                PropogateNameValue(PropogateName.BackColor, (object)SystemColors.Control);
            }

            if (_defaultActiveColor)
            {
                _activeColor = SystemColors.ActiveCaption;
                PropogateNameValue(PropogateName.ActiveColor, (object)SystemColors.ActiveCaption);
            }
            
            if (_defaultActiveTextColor)
            {
                _activeTextColor = SystemColors.ActiveCaptionText;
                PropogateNameValue(PropogateName.ActiveTextColor, (object)SystemColors.ActiveCaptionText);
            }

            if (_defaultInactiveTextColor)
            {
                _inactiveTextColor = SystemColors.ControlText;
                PropogateNameValue(PropogateName.InactiveTextColor, (object)SystemColors.ControlText);
            }

            if (_defaultResizeBarColor)
            {
                _resizeBarColor = SystemColors.Control;
                PropogateNameValue(PropogateName.ResizeBarColor, (object)SystemColors.Control);
            }

            if (_defaultCaptionFont)
            {
                _captionFont = SystemInformation.MenuFont;
                PropogateNameValue(PropogateName.CaptionFont, (object)SystemInformation.MenuFont);
            }

            if (_defaultTabControlFont)
            {
                _tabControlFont = SystemInformation.MenuFont;
                PropogateNameValue(PropogateName.TabControlFont, (object)SystemInformation.MenuFont);
            }
        }

		public virtual void OnShowContextMenu(Point screenPos)
		{
            PopupMenu context = new PopupMenu();

			// The order of Content displayed in the context menu is not the same as
			// the order of Content in the _contents collection. The latter has its
			// ordering changed to enable Restore functionality to work.
			ContentCollection temp = new ContentCollection();

			foreach(Content c in _contents)
			{
				int count = temp.Count;
				int index = 0;

				// Find best place to add into the temp collection
				for(; index<count; index++)
				{
					if (c.Order < temp[index].Order)
						break;
				}

				temp.Insert(index, c);
			}

			// Create a context menu entry per Content
			foreach(Content t in temp)
			{
				MenuCommand mc = new MenuCommand(t.Title, new EventHandler(OnToggleContentVisibility));
				mc.Checked = t.Visible;
				mc.Tag = t;

				context.MenuCommands.Add(mc);
			}

			// Add a separator 
			context.MenuCommands.Add(new MenuCommand("-"));

			// Add fixed entries to end to effect all content objects
			context.MenuCommands.Add(new MenuCommand("Show All", new EventHandler(OnShowAll)));
			context.MenuCommands.Add(new MenuCommand("Hide All", new EventHandler(OnHideAll)));

			// Ensure menu has same style as the docking windows
            context.Style = _visualStyle;

            if (OnContextMenu(context))
            {
                // Show it!
                context.TrackPopup(screenPos);
            }
		}

        protected bool OnContextMenu(PopupMenu context)
        {
            CancelEventArgs cea = new CancelEventArgs();
        
            if (ContextMenu != null)
                ContextMenu(context, cea);
                
            return !cea.Cancel;
        }

		protected void OnToggleContentVisibility(object sender, EventArgs e)
		{
			MenuCommand mc = sender as MenuCommand;

			if (mc != null)
			{
				Content c = mc.Tag as Content;

				if (c != null)
				{
					if (c.Visible)
						HideContent(c);
					else
						ShowContent(c);
				}
			}
		}

		protected void OnShowAll(object sender, EventArgs e)
		{
			ShowAllContents();
		}

		protected void OnHideAll(object sender, EventArgs e)
		{
			HideAllContents();
		}
    }
}