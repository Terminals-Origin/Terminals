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
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
    public class Content
    {
        // Enumeration of property change events
        public enum Property
        {
            Control,
            Title,
            FullTitle,
            ImageList,
            ImageIndex,
            CaptionBar,
            CloseButton,
            HideButton,
            DisplaySize,
            AutoHideSize,
            FloatingSize,
            DisplayLocation
        }

        // Declare the property change event signature
        public delegate void PropChangeHandler(Content obj, Property prop);

        // Class constant
        protected static int _defaultDisplaySize = 150;
        protected static int _defaultAutoHideSize = 150;
        protected static int _defaultFloatingSize = 150;
        protected static int _defaultLocation = 150;
		protected static int _counter = 0;

        // Instance fields
        protected Control _control;
        protected string _title;
        protected string _fullTitle;
        protected ImageList _imageList;
        protected int _imageIndex;
        protected Size _displaySize;
        protected Size _autoHideSize;
        protected Size _floatingSize;
		protected Point _displayLocation;
		protected int _order;
        protected DockingManager _manager;
        protected bool _docked;
        protected bool _autoHidden;
        protected bool _visible;
        protected bool _captionBar;
        protected bool _closeButton;
        protected bool _hideButton;
        protected AutoHidePanel _autoHidePanel;
        protected WindowContent _parentWindowContent;
		protected Restore _defaultRestore;
		protected Restore _autoHideRestore;
		protected Restore _dockingRestore;
		protected Restore _floatingRestore;

        // Instance events
        public event PropChangeHandler PropertyChanging;
        public event PropChangeHandler PropertyChanged;

        public Content(XmlTextReader xmlIn, int formatVersion)
        {
            // Define the initial object state
            _control = null;
            _title = "";
            _fullTitle = "";
            _imageList = null;
            _imageIndex = -1;
            _manager = null;
            _parentWindowContent = null;
            _displaySize = new Size(_defaultDisplaySize, _defaultDisplaySize);
            _autoHideSize = new Size(_defaultAutoHideSize, _defaultAutoHideSize);
            _floatingSize = new Size(_defaultFloatingSize, _defaultFloatingSize);
            _displayLocation = new Point(_defaultLocation, _defaultLocation);
			_order = _counter++;
			_visible = false;
			_defaultRestore = null;
			_autoHideRestore = null;
			_floatingRestore = null;
			_dockingRestore = null;
			_autoHidePanel = null;
			_docked = true;
			_captionBar = true;
			_closeButton = true;
            _hideButton = true;
            _autoHidden = false;

			// Overwrite default with values read in
			LoadFromXml(xmlIn, formatVersion);
        }

        public Content(DockingManager manager)
        {
            InternalConstruct(manager, null, "", null, -1);
        }

        public Content(DockingManager manager, Control control)
        {
            InternalConstruct(manager, control, "", null, -1);
        }

        public Content(DockingManager manager, Control control, string title)
        {
            InternalConstruct(manager, control, title, null, -1);
        }

        public Content(DockingManager manager, Control control, string title, ImageList imageList, int imageIndex)
        {
            InternalConstruct(manager, control, title, imageList, imageIndex);
        }

        protected void InternalConstruct(DockingManager manager, 
                                         Control control, 
                                         string title, 
                                         ImageList imageList, 
                                         int imageIndex)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Define the initial object state
            _control = control;
            _title = title;
            _imageList = imageList;
            _imageIndex = imageIndex;
            _manager = manager;
            _parentWindowContent = null;
            _order = _counter++;
            _visible = false;
            _displaySize = new Size(_defaultDisplaySize, _defaultDisplaySize);
            _autoHideSize = new Size(_defaultAutoHideSize, _defaultAutoHideSize);
            _floatingSize = new Size(_defaultFloatingSize, _defaultFloatingSize);
            _displayLocation = new Point(_defaultLocation, _defaultLocation);
			_defaultRestore = new RestoreContentState(State.DockLeft, this);
			_floatingRestore = new RestoreContentState(State.Floating, this);
            _autoHideRestore = new RestoreAutoHideState(State.DockLeft, this);
            _dockingRestore = _defaultRestore;
            _autoHidePanel = null;
			_docked = true;
            _captionBar = true;
            _closeButton = true;
            _hideButton = true;
            _autoHidden = false;
            _fullTitle = title;
        }

        public DockingManager DockingManager
        {
            get { return _manager; }
			set { _manager = value; }
        }

        public Control Control
        {
            get { return _control; }
			
            set 
            {
                if (_control != value)
                {
                    OnPropertyChanging(Property.Control);
                    _control = value;
                    OnPropertyChanged(Property.Control);
                }
            }
        }

        public string Title
        {
            get { return _title; }

            set 
            {
                if (_title != value)
                {
                    OnPropertyChanging(Property.Title);
                    _title = value;
                    OnPropertyChanged(Property.Title);
                }
            }
        }
        
        public string FullTitle
        {
            get { return _fullTitle; }
            
            set
            {
                if (_fullTitle != value)
                {
                    OnPropertyChanging(Property.FullTitle);
                    _fullTitle = value;
                    OnPropertyChanged(Property.FullTitle);
                }
            }
        }

        public ImageList ImageList
        {
            get { return _imageList; }

            set 
            {
                if(_imageList != value) 
                {
                    OnPropertyChanging(Property.ImageList);
                    _imageList = value; 
                    OnPropertyChanged(Property.ImageList);
                }
            }
        }

        public int ImageIndex
        {
            get { return _imageIndex; }

            set 
            {
                if (_imageIndex != value)
                {
                    OnPropertyChanging(Property.ImageIndex);
                    _imageIndex = value;
                    OnPropertyChanged(Property.ImageIndex);
                }
            }
        }

        public bool CaptionBar
        {
            get { return _captionBar; }
            
            set
            {
                if (_captionBar != value)
                {
                    OnPropertyChanging(Property.CaptionBar);
                    _captionBar = value;
                    OnPropertyChanged(Property.CaptionBar);
                }
            }
        }

        public bool CloseButton
        {
            get { return _closeButton; }
            
            set
            {
                if (_closeButton != value)
                {
                    OnPropertyChanging(Property.CloseButton);
                    _closeButton = value;
                    OnPropertyChanged(Property.CloseButton);
                }
            }
        }

        public bool HideButton
        {
            get { return _hideButton; }
            
            set
            {
                if (_hideButton != value)
                {
                    OnPropertyChanging(Property.HideButton);
                    _hideButton = value;
                    OnPropertyChanged(Property.HideButton);
                }
            }
        }

        public Size DisplaySize
        {
            get { return _displaySize; }

            set 
            {
                if (_displaySize != value)
                {
                    OnPropertyChanging(Property.DisplaySize);
                    _displaySize = value;
                    OnPropertyChanged(Property.DisplaySize);
                }
            }
        }
        
        public Size AutoHideSize
        {
            get { return _autoHideSize; }
            
            set
            {
                if (_autoHideSize != value)
                {
                    OnPropertyChanging(Property.AutoHideSize);
                    _autoHideSize = value;
                    OnPropertyChanged(Property.AutoHideSize);
                }
            }
        }

        public Size FloatingSize
        {
            get { return _floatingSize; }

            set 
            {
                if (_floatingSize != value)
                {
                    OnPropertyChanging(Property.FloatingSize);
                    _floatingSize = value;
                    OnPropertyChanged(Property.FloatingSize);
                }
            }
        }

        public Point DisplayLocation
        {
            get { return _displayLocation; }

            set 
            {
                if (_displayLocation != value)
                {
                    OnPropertyChanging(Property.DisplayLocation);
                    _displayLocation = value;
                    OnPropertyChanged(Property.DisplayLocation);
                }
            }
        }
        
		public int Order
		{
			get { return _order; }
		}

		public bool Visible
		{
			get { return _visible; }
		}

		public Restore DefaultRestore
		{
			get { return _defaultRestore; }
			set { _defaultRestore = value; }
		}

        public Restore AutoHideRestore
        {
            get { return _autoHideRestore; }
            set { _autoHideRestore = value; }
        }
        
        public Restore DockingRestore
		{
			get { return _dockingRestore; }
			set { _dockingRestore = value; }
		}

		public Restore FloatingRestore
		{
			get { return _floatingRestore; }
			set { _floatingRestore = value; }
		}

        public bool Docked
        {
            get { return _docked; }
			set { _docked = value; }
        }

        public WindowContent ParentWindowContent
        {
            get { return _parentWindowContent; }
            
			set 
			{ 
				if (_parentWindowContent != value)
				{
					_parentWindowContent = value; 

                    // Recalculate the visibility value
                    UpdateVisibility();
				}
			}
        }

        public AutoHidePanel AutoHidePanel
        {
            get { return _autoHidePanel; }
            
            set 
            {
                if (_autoHidePanel != value)
                {
                    _autoHidePanel = value; 
                
                    // Recalculate the visibility value
                    UpdateVisibility();
                }
            }
        }

        internal bool AutoHidden
        {
            get { return _autoHidden; }

            set 
            { 
                if (_autoHidden != value)
                {
                    _autoHidden = value; 

                    // Recalculate the visibility value
                    UpdateVisibility();
                }                
            }
        }

        public void UpdateVisibility()
        {
            _visible = ((_parentWindowContent != null) || (_autoHidden && (_autoHidePanel != null)));
        }

        public virtual void OnPropertyChanging(Property prop)
        {
            // Any attached event handlers?
            if (PropertyChanging != null)
                PropertyChanging(this, prop);
        }

        public virtual void OnPropertyChanged(Property prop)
        {
            // Any attached event handlers?
            if (PropertyChanged != null)
                PropertyChanged(this, prop);
        }

		public void BringToFront()
		{
		    if (!_visible)
		    {
			    // Use docking manager to ensure we are Visible
			    _manager.ShowContent(this);
            }
            
            if (_autoHidden)
            {
                // Request docking manager bring to window into view
                _manager.BringAutoHideIntoView(this);
            }
            else
            {
			    // Ask the parent WindowContent to ensure we are the active Content
			    _parentWindowContent.BringContentToFront(this);
	        }
		}

		public Restore RecordRestore()
		{
			if (_parentWindowContent != null)
			{
			    if (_autoHidden)
                    return RecordAutoHideRestore();
                else
                {			    
				    Form parentForm = _parentWindowContent.ParentZone.FindForm();

				    // Cannot record restore information if not in a Form
				    if (parentForm != null)
				    {
					    // Decide which restore actually needs recording
					    if (parentForm is FloatingForm)
						    return RecordFloatingRestore();
					    else
						    return RecordDockingRestore();
				    }	
		        }
			}

			return null;
		}

        public Restore RecordAutoHideRestore()
        {
            // Remove any existing restore object
            _autoHideRestore = null;
                
            // We should be inside a parent window
            if (_parentWindowContent != null)
            {
                // And in the auto hidden state
                if (_autoHidden)
                {
                    // Get access to the AutoHostPanel that contains use
                    AutoHidePanel ahp = _parentWindowContent.DockingManager.AutoHidePanelForContent(this);
                    
                    // Request the ahp create a relevant restore object for us
                    _autoHideRestore = ahp.RestoreObjectForContent(this);
                }
            }
        
            return _autoHideRestore;
        }

		public Restore RecordDockingRestore()
		{
			// Remove any existing Restore object
			_dockingRestore = null;

			// Do we have a parent window we are inside?
			if (_parentWindowContent != null)
			{
				// Ask the parent to provide a Restore object for us
				_dockingRestore = _parentWindowContent.RecordRestore(this);
			}

			// If we cannot get a valid Restore object from the parent then we have no choice 
			// but to use the default restore which is less accurate but better than nothing
			if (_dockingRestore == null)
				_dockingRestore = _defaultRestore;

			return _dockingRestore;
		}
		
		public Restore RecordFloatingRestore()
		{
			// Remove any existing Restore object
			_floatingRestore = null;

			// Do we have a parent window we are inside?
			if (_parentWindowContent != null)
			{
				// Ask the parent to provide a Restore object for us
				_floatingRestore = _parentWindowContent.RecordRestore(this);
			}

			// If we cannot get a valid Restore object from the parent then we have no choice 
			// but to use the default restore which is less accurate but better than nothing
			if (_floatingRestore == null)
				_floatingRestore = _defaultRestore;

			return _floatingRestore;
		}

		internal void ContentBecomesFloating()
		{
			_docked = false;

			if (_parentWindowContent != null)
			{
				switch(_parentWindowContent.State)
				{
					case State.DockLeft:
					case State.DockRight:
					case State.DockTop:
					case State.DockBottom:
						// Record the current position before content is moved
						RecordDockingRestore();
						break;
					case State.Floating:
					default:
						// Do nothing, already floating
						break;
				}
			}
		}

        internal void ContentLeavesFloating()
        {
			_docked = true;

            if (_parentWindowContent != null)
            {
                switch(_parentWindowContent.State)
                {
                    case State.DockLeft:
                    case State.DockRight:
                    case State.DockTop:
                    case State.DockBottom:
                        // Do nothing, already floating
                        break;
                    case State.Floating:
                    default:
                        // Record the current position before content is moved
                        RecordFloatingRestore();
                        break;
                }
            }
        }

		internal void ReconnectRestore()
		{
			_defaultRestore.Reconnect(_manager);
			_autoHideRestore.Reconnect(_manager);
			_dockingRestore.Reconnect(_manager);
			_floatingRestore.Reconnect(_manager);
		}

		internal void SaveToXml(XmlTextWriter xmlOut)
		{
			// Output standard values appropriate for all Content 
			xmlOut.WriteStartElement("Content");
			xmlOut.WriteAttributeString("Name", _title);
			xmlOut.WriteAttributeString("Visible", _visible.ToString());
            xmlOut.WriteAttributeString("Docked", _docked.ToString());
            xmlOut.WriteAttributeString("AutoHidden", _autoHidden.ToString());
            xmlOut.WriteAttributeString("CaptionBar", _captionBar.ToString());
            xmlOut.WriteAttributeString("CloseButton", _closeButton.ToString());
            xmlOut.WriteAttributeString("DisplaySize", ConversionHelper.SizeToString(_displaySize));
			xmlOut.WriteAttributeString("DisplayLocation", ConversionHelper.PointToString(_displayLocation));
            xmlOut.WriteAttributeString("AutoHideSize", ConversionHelper.SizeToString(_autoHideSize));
            xmlOut.WriteAttributeString("FloatingSize", ConversionHelper.SizeToString(_floatingSize));
            xmlOut.WriteAttributeString("FullTitle", _fullTitle);

			// Save the Default Restore object to Xml
			xmlOut.WriteStartElement("DefaultRestore");
			_defaultRestore.SaveToXml(xmlOut);
			xmlOut.WriteEndElement();

            // Save the AutoHideRestore object to Xml
            xmlOut.WriteStartElement("AutoHideRestore");
            _autoHideRestore.SaveToXml(xmlOut);
            xmlOut.WriteEndElement();
            
            // Save the DockingRestore object to Xml
			xmlOut.WriteStartElement("DockingRestore");
			_dockingRestore.SaveToXml(xmlOut);
			xmlOut.WriteEndElement();

			// Save the floating Restore object to Xml
			xmlOut.WriteStartElement("FloatingRestore");
			_floatingRestore.SaveToXml(xmlOut);
			xmlOut.WriteEndElement();

			xmlOut.WriteEndElement();
		}

		internal void LoadFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			// Read in the attribute values
			string attrTitle = xmlIn.GetAttribute(0);
			string attrVisible = xmlIn.GetAttribute(1);
            string attrDocked = xmlIn.GetAttribute(2);
            string attrAutoHide = xmlIn.GetAttribute(3);
            string attrCaptionBar = xmlIn.GetAttribute(4);
            string attrCloseButton = xmlIn.GetAttribute(5);
            string attrDisplaySize = xmlIn.GetAttribute(6);
			string attrDisplayLocation = xmlIn.GetAttribute(7);
            string attrAutoHideSize = xmlIn.GetAttribute(8);
            string attrFloatingSize = xmlIn.GetAttribute(9);
            string attrFullTitle = attrTitle;
            
            // 'FullTitle' property added in version 5 format and above
            if (formatVersion >= 5)
                attrFullTitle = xmlIn.GetAttribute(10);

			// Convert to correct types
			_title = attrTitle;
			_visible = Convert.ToBoolean(attrVisible);
            _docked = Convert.ToBoolean(attrDocked);
            _autoHidden = Convert.ToBoolean(attrAutoHide);
            _captionBar = Convert.ToBoolean(attrCaptionBar);
            _closeButton = Convert.ToBoolean(attrCloseButton);
            _displaySize = ConversionHelper.StringToSize(attrDisplaySize);
            _displayLocation = ConversionHelper.StringToPoint(attrDisplayLocation);
            _autoHideSize = ConversionHelper.StringToSize(attrAutoHideSize);
            _floatingSize = ConversionHelper.StringToSize(attrFloatingSize);
            _fullTitle = attrFullTitle;

			// Load the Restore objects
			_defaultRestore = Restore.CreateFromXml(xmlIn, true, formatVersion);
			_autoHideRestore  = Restore.CreateFromXml(xmlIn, true, formatVersion);
            _dockingRestore  = Restore.CreateFromXml(xmlIn, true, formatVersion);
            _floatingRestore = Restore.CreateFromXml(xmlIn, true, formatVersion);

			// Move past the end element
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");
		
			// Check it has the expected name
			if (xmlIn.NodeType != XmlNodeType.EndElement)
				throw new ArgumentException("EndElement expected but not found");
		}
    } 
}
