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
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    [ToolboxItem(false)]
    public class Window : ContainerControl
    {
        // Instance fields
        protected State _state;
        protected Zone _parentZone;
        protected WindowDetailCollection _windowDetails;
        protected Decimal _zoneArea;
        protected Size _minimalSize;
        protected DockingManager _manager;
        protected bool _autoDispose;
        protected bool _redockAllowed;
        protected bool _floatingCaption;
        protected bool _contentCaption;
        protected string _fullTitle;

        // Instance events
        public event EventHandler FullTitleChanged; 

        public Window(DockingManager manager)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Default object state
            _state = State.Floating;
            _parentZone = null;
            _zoneArea = 100m;
            _minimalSize = new Size(0,0);
            _manager = manager;
            _autoDispose = true;
            _fullTitle = "";
            _redockAllowed = true;
            _floatingCaption = true;
            _contentCaption = true;

            // Create collection of window details
            _windowDetails = new WindowDetailCollection();

            // We want notification when window details are added/removed/cleared
            _windowDetails.Clearing += new CollectionClear(OnDetailsClearing);
            _windowDetails.Inserted += new CollectionChange(OnDetailInserted);
            _windowDetails.Removing += new CollectionChange(OnDetailRemoving);
        }

        public DockingManager DockingManager
        {
            get { return _manager; }
        }

        public State State
        {
            get { return _state; }
			
            set 
            {
                if (_state != value)
                {
                    _state = value;

                    // Inform each window detail of the change in state
                    foreach(WindowDetail wd in _windowDetails)
                        wd.ParentStateChanged(_state);
                }
            }
        }

        public Zone ParentZone
        {
            get { return _parentZone; }
			
            set 
            { 
                if (_parentZone != value)
                {
                    _parentZone = value; 

                    // Inform each window detail of the change in zone
                    foreach(WindowDetail wd in _windowDetails)
                        wd.ParentZone = _parentZone;
                }
            }
        }

        public WindowDetailCollection WindowDetails
        {
            get { return _windowDetails; }
			
            set
            {
                _windowDetails.Clear();
                _windowDetails = value;
            }
        }

        public Decimal ZoneArea
        {
            get { return _zoneArea; }
            set { _zoneArea = value; }
        }

        public Size MinimalSize
        {
            get { return _minimalSize; }
            set { _minimalSize = value; }
        }

        public bool AutoDispose
        {
            get { return _autoDispose; }
            set { _autoDispose = value; }
        }

        public string FullTitle
        {
            get { return _fullTitle; }
        }

        public bool RedockAllowed
        {
            get { return _redockAllowed; }
            set { _redockAllowed = value; }
        }

        protected void OnDetailsClearing()
        {
            // Inform each detail it no longer has a parent
            foreach(WindowDetail wd in _windowDetails)
            {
                // Inform each detail it no longer has a parent
                wd.ParentWindow = null;

                // Inform object that it is no longer in a Zone
                wd.ParentZone = null;
            }
        }

        protected void OnDetailInserted(int index, object value)
        {
            WindowDetail wd = value as WindowDetail;

            // Inform object we are the new parent
            wd.ParentWindow = this;

            // Inform object that it is in a Zone
            wd.ParentZone = _parentZone;
        }

        protected void OnDetailRemoving(int index, object value)
        {
            WindowDetail wd = value as WindowDetail;

            // Inform object it no longer has a parent
            wd.ParentWindow = null;
			
            // Inform object that it is no longer in a Zone
            wd.ParentZone = null;
        }
		
        public virtual void NotifyFullTitleText(string title)
        {
            // Inform each detail of change in focus
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyFullTitleText(title);
                
            OnFullTitleChanged(title);
        }

        public virtual void NotifyAutoHideImage(bool autoHidden)
        {
            // Inform each detail of change in caption bar
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyAutoHideImage(autoHidden);
        }

        public virtual void NotifyShowCaptionBar(bool show)
        {
            // Remember the per-content requested caption
            _contentCaption = show;
        
            // If priority value always showing then we can let the
            // individual content decide on visibility. Otherwise
            // the priority forces it to remain hidden
            if (_floatingCaption)
            {
                // Inform each detail of change in caption bar
                foreach(WindowDetail wd in _windowDetails)
                    wd.NotifyShowCaptionBar(show);
            }
        }

        public virtual void NotifyCloseButton(bool show)
        {
            // Inform each detail of change close button
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyCloseButton(show);
        }

        public virtual void NotifyHideButton(bool show)
        {
            // Inform each detail of change close button
            foreach(WindowDetail wd in _windowDetails)
                wd.NotifyHideButton(show);
        }

        public virtual void NotifyContentGotFocus()
        {
            // Inform each detail of change in focus
            foreach(WindowDetail wd in _windowDetails)
                wd.WindowGotFocus();
        }

        public virtual void NotifyContentLostFocus()
        {
            // Inform each detail of change in focus
            foreach(WindowDetail wd in _windowDetails)
                wd.WindowLostFocus();
        }

        public virtual void WindowDetailGotFocus(WindowDetail wd)
        {
            NotifyContentGotFocus();
        }
		
        public virtual void WindowDetailLostFocus(WindowDetail wd)
        {
            NotifyContentLostFocus();
        }
        
        public void HideDetails()
        {
            // Inform each detail of change in visibility
            foreach(WindowDetail wd in _windowDetails)
                wd.Hide();
                
            // Remember priority state for caption
            _floatingCaption = false;
        }

        public void ShowDetails()
        {
            // Inform each detail of change in visibility
            foreach(WindowDetail wd in _windowDetails)
                wd.Show();

            // Remember priority state for caption
            _floatingCaption = true;
            
            // If the content requested the caption be hidden
            if (!_contentCaption)
                NotifyShowCaptionBar(_contentCaption);
        }
        
        public virtual void OnFullTitleChanged(String fullTitle)
        {
            _fullTitle = fullTitle;
            
            if (FullTitleChanged != null)
                FullTitleChanged((object)fullTitle, EventArgs.Empty);
        }

		public virtual Restore RecordRestore(object child) 
		{
			// Do we have a Zone as our parent?
			if (_parentZone != null)
			{
				// Delegate to the Zone as we cannot help out
				return _parentZone.RecordRestore(this, child, null);
			}

			return null;
		}

        public virtual void PropogateNameValue(PropogateName name, object value)
        {
            if (name == PropogateName.BackColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }

            // Pass onto each of our child Windows
            foreach(WindowDetail wd in _windowDetails)
                wd.PropogateNameValue(name, value);
        }
    }
}