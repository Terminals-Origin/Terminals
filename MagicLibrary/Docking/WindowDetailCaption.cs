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
using System.Drawing.Imaging;
using Microsoft.Win32;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Docking
{
    [ToolboxItem(false)]
    public class WindowDetailCaption : WindowDetail, IMessageFilter
    {
        // Class fields
        protected static ImageList _images;

        // Instance events
        public event EventHandler Close;
		public event EventHandler Restore;
		public event EventHandler InvertAutoHide;
        public event ContextHandler Context;

        // Instance fields
        protected InertButton _maxButton;
        protected InertButton _closeButton;
        protected InertButton _hideButton;
        protected RedockerContent _redocker;
        protected IZoneMaximizeWindow _maxInterface;
        protected bool _showCloseButton;
        protected bool _showHideButton;
        protected bool _ignoreHideButton;
        protected bool _pinnedImage;

        // Class fields
        protected static ImageAttributes _activeAttr = new ImageAttributes();
        protected static ImageAttributes _inactiveAttr = new ImageAttributes();

        public WindowDetailCaption(DockingManager manager, 
                                   Size fixedSize, 
                                   EventHandler closeHandler, 
                                   EventHandler restoreHandler, 
                                   EventHandler invertAutoHideHandler, 
                                   ContextHandler contextHandler)
            : base(manager)
        {
            // Setup correct color remapping depending on initial colors
            DefineButtonRemapping();

            // Default state
            _maxButton = null;
            _hideButton = null;
            _maxInterface = null;
            _redocker = null;
            _showCloseButton = true;
            _showHideButton = true;
            _ignoreHideButton = false;
            _pinnedImage = false;
            
            // Prevent flicker with double buffering and all painting inside WM_PAINT
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Our size is always fixed at the required length in both directions
            // as one of the sizes will be provided for us because of our docking
            this.Size = fixedSize;

            if (closeHandler != null)
                this.Close += closeHandler;	

            if (restoreHandler != null)
                this.Restore += restoreHandler;	

            if (invertAutoHideHandler != null)
                this.InvertAutoHide += invertAutoHideHandler;
    
            if (contextHandler != null)
                this.Context += contextHandler;	

            // Let derived classes override the button creation
            CreateButtons();

            // Need to hook into message pump so that the ESCAPE key can be 
            // intercepted when in redocking mode
            Application.AddMessageFilter(this);
        }

        public override Zone ParentZone
        {
            set
            {
                base.ParentZone = value;

                RecalculateMaximizeButton();
                RecalculateButtons();
            }
        }

        public virtual void OnClose()
        {
            // Any attached event handlers?
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        public virtual void OnInvertAutoHide()
        {
            // Any attached event handlers?
            if (InvertAutoHide != null)
                InvertAutoHide(this, EventArgs.Empty);
        }
        
        public virtual void OnRestore()
        {
            // Any attached event handlers?
            if (Restore != null)
                Restore(this, EventArgs.Empty);
        }

        public virtual void OnContext(Point screenPos)
        {
            // Any attached event handlers?
            if (Context != null)
                Context(screenPos);
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
				if (_closeButton != null)
				{
					_closeButton.Click -= new EventHandler(OnButtonClose);
					_closeButton.GotFocus -= new EventHandler(OnButtonGotFocus);
				}

                if (_hideButton != null)
                {
                    _hideButton.Click -= new EventHandler(OnButtonHide);
                    _hideButton.GotFocus -= new EventHandler(OnButtonGotFocus);
                }
                
                if (_maxButton != null)
				{
					_maxButton.Click -= new EventHandler(OnButtonMax);
					_maxButton.GotFocus -= new EventHandler(OnButtonGotFocus);
				}
            }
            base.Dispose( disposing );
        }

        public override void NotifyAutoHideImage(bool autoHidden)
        {
            _pinnedImage = autoHidden;
            UpdateAutoHideImage();
        }

        public override void NotifyCloseButton(bool show)
        {
            _showCloseButton = show;
            RecalculateButtons();
        }

        public override void NotifyHideButton(bool show)
        {
            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (_parentWindow.State == State.Floating);
            
            _showHideButton = show;
            RecalculateButtons();
        }

        public override void NotifyShowCaptionBar(bool show)
        {
            this.Visible = show;
        }
        
        protected void RecalculateMaximizeButton()
        {
            // Are we inside a Zone?
            if (this.ParentZone != null)
            {
                // Does the Zone support the maximizing of a Window?
                IZoneMaximizeWindow zmw = this.ParentZone as IZoneMaximizeWindow;

                if (zmw != null)
                {
                    AddMaximizeInterface(zmw);
                    return;
                }
            }

            RemoveMaximizeInterface();
        }

        protected void AddMaximizeInterface(IZoneMaximizeWindow zmw)
        {
            // Has the maximize button already been created?
            if (_maxInterface == null)
            {
                // Create the InertButton
                _maxButton = new InertButton(_images, 0);

                // Hook into button events
                _maxButton.Click += new EventHandler(OnButtonMax);
                _maxButton.GotFocus += new EventHandler(OnButtonGotFocus);

                // Define the default remapping
                _maxButton.ImageAttributes = _inactiveAttr;

                OnAddMaximizeInterface();

                Controls.Add(_maxButton);

                // Remember the interface reference
                _maxInterface = zmw;

                // Hook into the interface change events
                _maxInterface.RefreshMaximize += new EventHandler(OnRefreshMaximize);

                RecalculateButtons();
            }
        }

        protected void RemoveMaximizeInterface()
        {
            if (_maxInterface != null)
            {
                // Unhook from the interface change events
                _maxInterface.RefreshMaximize -= new EventHandler(OnRefreshMaximize);

                // Remove the interface reference
                _maxInterface = null;

				// Use helper method to circumvent form Close bug
				ControlHelper.Remove(this.Controls, _maxButton);

                OnRemoveMaximizeInterface();

                // Unhook into button events
                _maxButton.Click -= new EventHandler(OnButtonMax);
                _maxButton.GotFocus -= new EventHandler(OnButtonGotFocus);

                // Kill the button which is no longer needed
                _maxButton.Dispose();
                _maxButton = null;

                RecalculateButtons();
            }
        }

        protected void OnRefreshMaximize(object sender, EventArgs e)
        {
            UpdateMaximizeImage();
        }
	
        protected void OnButtonMax(object sender, EventArgs e)
        {
            if (this.ParentWindow != null)
            {
                if (_maxInterface.IsMaximizeAvailable())
                {
                    // Are we already maximized?
                    if (_maxInterface.IsWindowMaximized(this.ParentWindow))
                        _maxInterface.RestoreWindow();
                    else
                        _maxInterface.MaximizeWindow(this.ParentWindow);
                }
            }			
        }

        protected void OnButtonClose(Object sender, EventArgs e)
        {
            if (_showCloseButton)
                OnClose();
        }

        protected void OnButtonHide(Object sender, EventArgs e)
        {
            // Plain button can still be pressed when disabled, so double check 
            // that an event should actually be generated
            if (_showHideButton && !_ignoreHideButton)
                OnInvertAutoHide();
        }

        protected void OnButtonGotFocus(Object sender, EventArgs e)
        {
            // Inform parent window we have now got the focus
            if (this.ParentWindow != null)
                this.ParentWindow.WindowDetailGotFocus(this);
        }

		protected override void OnDoubleClick(EventArgs e)
		{
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Are we currently in a redocking state?
                if (_redocker != null)
                {
                    // No longer need the object
                    _redocker = null;
                }
            }

			// Fire attached event handlers
			OnRestore();
		}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Left mouse down begins a redocking action
                if (e.Button == MouseButtons.Left)
                {
                    if (this.ParentWindow.RedockAllowed)
                    {
                        WindowContent wc = this.ParentWindow as WindowContent;

                        // Is our parent a WindowContent instance?				
                        if (wc != null)
                        {
                            // Start redocking activity for the whole WindowContent
                            _redocker = new RedockerContent(this, wc, new Point(e.X, e.Y));
                        }
                    }
                }

                this.Focus();
            }
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Redocker object needs mouse movements
                if (_redocker != null)
                    _redocker.OnMouseMove(e);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // The double click event will cause the control to be destroyed as 
            // the Contents are restored to their alternative positions, so need to
            // double check the control is not already dead
            if (!IsDisposed)
            {
                // Are we currently in a redocking state?
                if (_redocker != null)
                {
                    // Let the redocker finish off
                    _redocker.OnMouseUp(e);

                    // No longer need the object
                    _redocker = null;
                }

                // Right mouse button can generate a Context event
                if (e.Button == MouseButtons.Right)
                {
					// Get screen coordinates of the mouse
					Point pt = this.PointToScreen(new Point(e.X, e.Y));
    				
					// Box to transfer as parameter
					OnContext(pt);
                }
            }
            
            base.OnMouseUp(e);
        }

        protected override void OnResize(EventArgs e)
        {
            // Any resize of control should redraw all of it
            Invalidate();
            base.OnResize(e);
        }

        protected virtual void DefineButtonRemapping() {}
        protected virtual void OnAddMaximizeInterface() {}
        protected virtual void OnRemoveMaximizeInterface() {}
        protected virtual void UpdateMaximizeImage() {}
        protected virtual void UpdateAutoHideImage() {}
        protected virtual void RecalculateButtons() {}
        
        protected virtual void CreateButtons() 
        {
            // Attach events to button
            if (_closeButton != null)
            {
                _closeButton.Click += new EventHandler(OnButtonClose);
                _closeButton.GotFocus += new EventHandler(OnButtonGotFocus);
            }

            if (_hideButton != null)
            {
                _hideButton.Click += new EventHandler(OnButtonHide);
                _hideButton.GotFocus += new EventHandler(OnButtonGotFocus);
            }
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

                        // No longer need the object
                        _redocker = null;
                        
                        return true;
                    }
                }
            }
            
            return false;
        }
    }

    [ToolboxItem(false)]
    public class WindowDetailCaptionPlain : WindowDetailCaption
    {
        protected enum ImageIndex
        {
            Close					= 0,
            EnabledHorizontalMax	= 1,
            EnabledHorizontalMin	= 2,
            EnabledVerticalMax		= 3,
            EnabledVerticalMin		= 4, 
            AutoHide		        = 5, 
            AutoShow		        = 6 
        }

        // Class constants
        protected const int _inset = 3;
        protected const int _offset = 5;
        protected const int _fixedLength = 14;
        protected const int _imageWidth = 10;
        protected const int _imageHeight = 10;
        protected const int _buttonWidth = 12;
        protected const int _buttonHeight = 12;
        protected const int _insetButton = 2;

        // Instance fields
        protected bool _dockLeft;
        protected int _buttonOffset;

        static WindowDetailCaptionPlain()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _images = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.Magic.Docking.WindowDetailCaptionPlain"),
                                                     "Crownwood.Magic.Resources.ImagesCaptionPlain.bmp",
                                                     new Size(_imageWidth, _imageHeight),
                                                     new Point(0,0));
        }

        public WindowDetailCaptionPlain(DockingManager manager, 
                                        EventHandler closeHandler, 
                                        EventHandler restoreHandler, 
                                        EventHandler invertAutoHideHandler, 
                                        ContextHandler contextHandler)
            : base(manager, 
                   new Size(_fixedLength, _fixedLength), 
                   closeHandler, 
                   restoreHandler, 
                   invertAutoHideHandler, 
                   contextHandler)
        {
            // Default to thinking we are docked on a left edge
            _dockLeft = true;

            // Modify painting to prevent overwriting the button control
            _buttonOffset = 1 + (_buttonWidth + _insetButton) * 2;
        }
        
        public override void ParentStateChanged(State newState)
        {
            // Should we dock to the left or top of our container?
            switch(newState)
            {
                case State.DockTop:
                case State.DockBottom:
                    _dockLeft = true;
                    break;
                case State.Floating:
                case State.DockLeft:
                case State.DockRight:
                    _dockLeft = false;
                    break;
            }

            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (_parentWindow.State == State.Floating);

            RecalculateButtons();
        }

        public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Remove our width from the minimum size of the parent
                        minSize.Width -= _fixedLength;
                    }
                    else
                    {
                        // Remove our height from the minimum size of the parent
                        minSize.Height -= _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }

        public override void AddedToParent(Window parent)
        {
            if (parent != null)
            {
                if (this.Dock != DockStyle.None)
                {
                    Size minSize = parent.MinimalSize;

                    if (this.Dock == DockStyle.Left)
                    {
                        // Add our width from the minimum size of the parent
                        minSize.Width += _fixedLength;
                    }
                    else
                    {
                        // Add our height from the minimum size of the parent
                        minSize.Height += _fixedLength;
                    }

                    parent.MinimalSize = minSize;
                }
            }
        }
        
        protected override void DefineButtonRemapping()
        {
            // Define use of current system colors
            ColorMap activeMap = new ColorMap();
            ColorMap inactiveMap = new ColorMap();
			
            activeMap.OldColor = Color.Black;
            activeMap.NewColor = _manager.InactiveTextColor;
            inactiveMap.OldColor = Color.Black;
            inactiveMap.NewColor = Color.FromArgb(128, _manager.InactiveTextColor);

            // Create remap attributes for use by button
            _activeAttr.SetRemapTable(new ColorMap[]{activeMap}, ColorAdjustType.Bitmap);
            _inactiveAttr.SetRemapTable(new ColorMap[]{inactiveMap}, ColorAdjustType.Bitmap);
        }

        protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);

                // Shows border all the time and not just when mouse is over control
                _maxButton.PopupStyle = false;

                // Define the fixed remapping
                _maxButton.ImageAttributes = _inactiveAttr;
                
                // Reduce the lines drawing
                _buttonOffset += (_buttonWidth + _insetButton);
            }
        }

        protected override void OnRemoveMaximizeInterface()
        {
            // Reduce the lines drawing
            _buttonOffset -= (_buttonWidth + _insetButton);
        }

        protected override void CreateButtons()
        {
            // Define the ImageList and which ImageIndex to show initially
            _closeButton = new InertButton(_images, (int)ImageIndex.Close);
            _hideButton = new InertButton(_images, (int)ImageIndex.AutoHide);
			
            _closeButton.Size = new Size(_buttonWidth, _buttonHeight);
            _hideButton.Size = new Size(_buttonWidth, _buttonHeight);

            // Shows border all the time and not just when mouse is over control
            _closeButton.PopupStyle = false;
            _hideButton.PopupStyle = false;

            // Define the fixed remapping
            _closeButton.ImageAttributes = _activeAttr;
            _hideButton.ImageAttributes = _activeAttr;

            // Add to the display
            Controls.Add(_closeButton);
            Controls.Add(_hideButton);
            
            // Let base class perform common actions
            base.CreateButtons();
        }

        protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.ImageIndexEnabled = (int)ImageIndex.AutoShow;
            else
                _hideButton.ImageIndexEnabled = (int)ImageIndex.AutoHide;
        }
        
        protected override void UpdateMaximizeImage()
        {
            if (_showCloseButton)
                _closeButton.ImageAttributes = _activeAttr;
            else    
                _closeButton.ImageAttributes = _inactiveAttr;

            if (_showHideButton && !_ignoreHideButton)
                _hideButton.ImageAttributes = _activeAttr;
            else
                _hideButton.ImageAttributes = _inactiveAttr;

            if (_maxButton != null)
            {
                if (_maxInterface.IsMaximizeAvailable())
                    _maxButton.ImageAttributes = _activeAttr;
                else
                    _maxButton.ImageAttributes = _inactiveAttr;

                bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                if (_maxInterface.Direction == Direction.Vertical)
                {
                    if (maximized)
                        _maxButton.ImageIndexEnabled = (int)ImageIndex.EnabledVerticalMin;	
                    else
                        _maxButton.ImageIndexEnabled = (int)ImageIndex.EnabledVerticalMax;	
                }
                else
                {
                    if (maximized)
                        _maxButton.ImageIndexEnabled = (int)ImageIndex.EnabledHorizontalMin;	
                    else
                        _maxButton.ImageIndexEnabled = (int)ImageIndex.EnabledHorizontalMax;	
                }
            }
        }

        protected override void RecalculateButtons()
        {
            if (_dockLeft)
            {
                if (this.Dock != DockStyle.Left)
                {
                    RemovedFromParent(this.ParentWindow);
                    this.Dock = DockStyle.Left;
                    AddedToParent(this.ParentWindow);
                }

                int iStart = _inset;

                // Button position is fixed, regardless of our size
                _closeButton.Location = new Point(_insetButton, iStart);
                _closeButton.Anchor = AnchorStyles.Top;
                _closeButton.Show();
                iStart += _buttonHeight + _insetButton;
                
                // Button position is fixed, regardless of our size
                _hideButton.Location = new Point(_insetButton, iStart);
                _hideButton.Anchor = AnchorStyles.Top;
                _hideButton.Show();
                iStart += _buttonHeight + _insetButton;

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.Location = new Point(_insetButton, iStart);
                    _maxButton.Anchor = AnchorStyles.Top;
                }
            }
            else
            {
                if (this.Dock != DockStyle.Top)
                {
                    RemovedFromParent(this.ParentWindow);
                    this.Dock = DockStyle.Top;
                    AddedToParent(this.ParentWindow);
                }

                Size client = this.ClientSize;
                int iStart = _inset;

                // Button is positioned to right hand side of bar
                _closeButton.Location = new Point(client.Width - iStart - _buttonWidth, _insetButton);
                _closeButton.Anchor = AnchorStyles.Right;
                _closeButton.Show();
                iStart += _buttonWidth + _insetButton;
                
                // Next the AutoHide button is positioned
                _hideButton.Location = new Point(client.Width - iStart - _buttonWidth, _insetButton);
                _hideButton.Anchor = AnchorStyles.Right;
                _hideButton.Show();
                iStart += _buttonWidth + _insetButton;

                if (_maxButton != null)
                {
                    // Button position is fixed, regardless of our size
                    _maxButton.Location = new Point(client.Width - iStart - _buttonWidth, _insetButton);
                    _maxButton.Anchor = AnchorStyles.Right;
                }
            }

            UpdateMaximizeImage();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Size ourSize = this.ClientSize;
            Point[] light = new Point[4];
            Point[] dark = new Point[4];
				
            // Depends on orientation
            if (_dockLeft)
            {
                int iBottom = ourSize.Height - _inset - 1;
                int iRight = _offset + 2;
                int iTop = _inset + _buttonOffset;

                light[3].X = light[2].X = light[0].X = _offset;
                light[2].Y = light[1].Y = light[0].Y = iTop;
                light[1].X = _offset + 1;
                light[3].Y = iBottom;
			
                dark[2].X = dark[1].X = dark[0].X = iRight;
                dark[3].Y = dark[2].Y = dark[1].Y = iBottom;
                dark[0].Y = iTop;
                dark[3].X = iRight - 1;
            }
            else
            {
                int iBottom = _offset + 2;
                int iRight = ourSize.Width - (_inset + _buttonOffset);
				
                light[3].X = light[2].X = light[0].X = _inset;
                light[1].Y = light[2].Y = light[0].Y = _offset;
                light[1].X = iRight;
                light[3].Y = _offset + 1;
			
                dark[2].X = dark[1].X = dark[0].X = iRight;
                dark[3].Y = dark[2].Y = dark[1].Y = iBottom;
                dark[0].Y = _offset;
                dark[3].X = _inset;
            }

            using (Pen lightPen = new Pen(ControlPaint.LightLight(_manager.BackColor)),
                       darkPen = new Pen(ControlPaint.Dark(_manager.BackColor)))
            {
                e.Graphics.DrawLine(lightPen, light[0], light[1]);
                e.Graphics.DrawLine(lightPen, light[2], light[3]);
                e.Graphics.DrawLine(darkPen, dark[0], dark[1]);
                e.Graphics.DrawLine(darkPen, dark[2], dark[3]);

                // Shift coordinates to draw section grab bar
                if (_dockLeft)
                {
                    for(int i=0; i<4; i++)
                    {
                        light[i].X += 4;
                        dark[i].X += 4;
                    }
                }
                else
                {
                    for(int i=0; i<4; i++)
                    {
                        light[i].Y += 4;
                        dark[i].Y += 4;
                    }
                }

                e.Graphics.DrawLine(lightPen, light[0], light[1]);
                e.Graphics.DrawLine(lightPen, light[2], light[3]);
                e.Graphics.DrawLine(darkPen, dark[0], dark[1]);
                e.Graphics.DrawLine(darkPen, dark[2], dark[3]);
            }

            base.OnPaint(e);
        }
    }

    public class WindowDetailCaptionIDE : WindowDetailCaption
    {
        protected enum ImageIndex
        {
            Close					= 0,
            EnabledVerticalMax		= 1,
            EnabledVerticalMin		= 2,
            AutoHide		        = 3, 
            AutoShow		        = 4 
        }

        // Class constants
        protected const int _yInset = 3;
        protected const int _yInsetExtra = 3;
        protected const int _imageWidth = 12;
        protected const int _imageHeight = 11;
        protected const int _buttonWidth = 14;
        protected const int _buttonHeight = 13;
        protected const int _buttonSpacer = 3;

        // Class fields
        protected static int _fixedLength;
        
        static WindowDetailCaptionIDE()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _images = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.Magic.Docking.WindowDetailCaptionIDE"),
                                                     "Crownwood.Magic.Resources.ImagesCaptionIDE.bmp",
                                                     new Size(_imageWidth, _imageHeight),
                                                     new Point(0,0));
        }

        public WindowDetailCaptionIDE(DockingManager manager, 
                                      EventHandler closeHandler, 
                                      EventHandler restoreHandler, 
                                      EventHandler invertAutoHideHandler, 
                                      ContextHandler contextHandler)
            : base(manager, 
                   new Size(_fixedLength, _fixedLength), 
                   closeHandler, 
                   restoreHandler, 
                   invertAutoHideHandler,
                   contextHandler)
        {
            // Use specificed font in the caption 
            UpdateCaptionHeight(manager.CaptionFont);
        }

        public override void PropogateNameValue(PropogateName name, object value)
        {
            base.PropogateNameValue(name, value);
        
            switch(name)
            {
                case PropogateName.CaptionFont:
                    UpdateCaptionHeight((Font)value);    
                    break;
                case PropogateName.ActiveTextColor:
                case PropogateName.InactiveTextColor:
                    DefineButtonRemapping();
                    Invalidate();
                    break;
            }
        }
        
        public override void WindowGotFocus()
        {
            SetButtonState();
            Invalidate();
        }

        public override void WindowLostFocus()
        {
            SetButtonState();
            Invalidate();
        }
      
        public override void NotifyFullTitleText(string title)
        {
            this.Text = title;
            Invalidate();
        }

        public override void ParentStateChanged(State newState)
        { 
            // Ignore the AutoHide feature when in floating form
            _ignoreHideButton = (_parentWindow.State == State.Floating);

            this.Dock = DockStyle.Top;
            RecalculateButtons();
            Invalidate();
        }

        public override void RemovedFromParent(Window parent)
        {
            if (parent != null)
            {
                Size minSize = parent.MinimalSize;

                // Remove our height from the minimum size of the parent
                minSize.Height -= _fixedLength;
                minSize.Width -= _fixedLength;

                parent.MinimalSize = minSize;
            }
        }

        protected override void DefineButtonRemapping()
        {
            // Define use of current system colors
            ColorMap activeMap = new ColorMap();
            ColorMap inactiveMap = new ColorMap();
			
            activeMap.OldColor = Color.Black;
            activeMap.NewColor = _manager.ActiveTextColor;
            inactiveMap.OldColor = Color.Black;
            inactiveMap.NewColor = _manager.InactiveTextColor;

            // Create remap attributes for use by button
            _activeAttr.SetRemapTable(new ColorMap[]{activeMap}, ColorAdjustType.Bitmap);
            _inactiveAttr.SetRemapTable(new ColorMap[]{inactiveMap}, ColorAdjustType.Bitmap);
        }

        public override void AddedToParent(Window parent)
        {
            if (parent != null)
            {
                Size minSize = parent.MinimalSize;

                // Remove our height from the minimum size of the parent
                minSize.Height += _fixedLength;
                minSize.Width += _fixedLength;

                parent.MinimalSize = minSize;
            }
        }

        protected override void OnAddMaximizeInterface()
        {
            if (_maxButton != null)
            {
                // Set the correct size for the button
                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);

                // Give the button a very thin button
                _maxButton.BorderWidth = 1;

                // Define correct color setup
                _maxButton.BackColor = this.BackColor;
                _maxButton.ImageAttributes = _inactiveAttr;

                // Force the ImageAttribute for the button to be set
                SetButtonState();
            }
        }

        protected override void UpdateAutoHideImage()
        {
            if (_pinnedImage)
                _hideButton.ImageIndexEnabled = (int)ImageIndex.AutoShow;
            else
                _hideButton.ImageIndexEnabled = (int)ImageIndex.AutoHide;
        }

        protected override void UpdateMaximizeImage()
        {
            if ((_maxButton != null) && (_maxInterface != null))
            {
                bool enabled = _maxInterface.IsMaximizeAvailable();

                if (!enabled)
                {
                    if (_maxButton.Visible)
                        _maxButton.Hide();
                }
                else
                {
                    bool maximized = _maxInterface.IsWindowMaximized(this.ParentWindow);

                    if (!_maxButton.Visible)
                        _maxButton.Show();

                    if (maximized)
                        _maxButton.ImageIndexEnabled = (int)ImageIndex.EnabledVerticalMin;	
                    else
                        _maxButton.ImageIndexEnabled = (int)ImageIndex.EnabledVerticalMax;	
                }
            }
        }

        protected void SetButtonState()
        {
            if (this.ParentWindow != null)
            {
                if (this.ParentWindow.ContainsFocus)
                {
                    if (_closeButton.BackColor != _manager.ActiveColor)
                    {
                        _closeButton.BackColor = _manager.ActiveColor;
                        _closeButton.ImageAttributes = _activeAttr;
                        _closeButton.Invalidate();
                    }

                    if (_hideButton != null)
                    {
                        if (_hideButton.BackColor != _manager.ActiveColor)
                        {
                            _hideButton.BackColor = _manager.ActiveColor;
                            _hideButton.ImageAttributes = _activeAttr;
                            _hideButton.Invalidate();
                        }
                    }

                    if (_maxButton != null)
                    {
                        if (_maxButton.BackColor != _manager.ActiveColor)
                        {
                            _maxButton.BackColor = _manager.ActiveColor;
                            _maxButton.ImageAttributes = _activeAttr;
                            _maxButton.Invalidate();
                        }
                    }
                }
                else
                {
                    if (_closeButton.BackColor != this.BackColor)
                    {
                        _closeButton.BackColor = this.BackColor;
                        _closeButton.ImageAttributes = _inactiveAttr;
                        _closeButton.Invalidate();
                    }

                    if (_hideButton != null)
                    {
                        if (_hideButton.BackColor != this.BackColor)
                        {
                            _hideButton.BackColor = this.BackColor;
                            _hideButton.ImageAttributes = _inactiveAttr;
                            _hideButton.Invalidate();
                        }
                    }

                    if (_maxButton != null)
                    {
                        if (_maxButton.BackColor != this.BackColor)
                        {
                            _maxButton.BackColor = this.BackColor;
                            _maxButton.ImageAttributes = _inactiveAttr;
                            _maxButton.Invalidate();
                        }
                    }
                }
            }
        }

        protected override void RecalculateButtons()
        {
            int buttonX = this.Width - _buttonWidth - _buttonSpacer;
            int buttonY = (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset;
        
            if (_showCloseButton)
            {
                // Button position is fixed, regardless of our size
                _closeButton.Location = new Point(buttonX, buttonY);

                _closeButton.Size = new Size(_buttonWidth, _buttonHeight);
    		
                // Give the button a very thin button
                _closeButton.BorderWidth = 1;

                // Let the location of the control be updated for us
                _closeButton.Anchor = AnchorStyles.Right;

                // Just in case currently hidden
                _closeButton.Show();
                
                // Define start of next button
                buttonX -= _buttonWidth;
            }
            else
                _closeButton.Hide();
                        
            if (_showHideButton && !_ignoreHideButton)
            {
                // Button position is fixed, regardless of our size
                _hideButton.Location = new Point(buttonX, buttonY);

                _hideButton.Size = new Size(_buttonWidth, _buttonHeight);
			
                // Give the button a very thin button
                _hideButton.BorderWidth = 1;

                // Let the location of the control be updated for us
                _hideButton.Anchor = AnchorStyles.Right;

                // Just in case currently hidden
                _hideButton.Show();

                // Define start of next button
                buttonX -= _buttonWidth;
                
                UpdateAutoHideImage();
            }
            else
                _hideButton.Hide();
            
            if (_maxButton != null)
            {
                // Button position is fixed, regardless of our size
                _maxButton.Location = new Point(buttonX, buttonY);

                _maxButton.Size = new Size(_buttonWidth, _buttonHeight);
			
                // Give the button a very thin button
                _maxButton.BorderWidth = 1;

                // Let the location of the control be updated for us
                _maxButton.Anchor = AnchorStyles.Right;

                // Define start of next button
                buttonX -= _buttonWidth;

                UpdateMaximizeImage();
            }
        }

        protected override void CreateButtons()
        {
            // Define the ImageList and which ImageIndex to show initially
            _closeButton = new InertButton(_images, (int)ImageIndex.Close);
            _hideButton = new InertButton(_images, (int)ImageIndex.AutoHide);
			
            _closeButton.Size = new Size(_buttonWidth, _buttonHeight);
            _hideButton.Size = new Size(_buttonWidth, _buttonHeight);

            // Let the location of the control be updated for us
            _closeButton.Anchor = AnchorStyles.Right;
            _hideButton.Anchor = AnchorStyles.Right;

            // Define the button position relative to the size set in the constructor
            _closeButton.Location = new Point(_fixedLength - _buttonWidth - _buttonSpacer, 
                                              (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);
            
            _hideButton.Location = new Point(_fixedLength - (_buttonWidth - _buttonSpacer) * 2, 
                                             (_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

            // Give the button a very thin button
            _closeButton.BorderWidth = 1;
            _hideButton.BorderWidth = 1;

            // Define correct color setup
            _closeButton.BackColor = this.BackColor;
            _closeButton.ImageAttributes = _inactiveAttr;
            _hideButton.BackColor = this.BackColor;
            _hideButton.ImageAttributes = _inactiveAttr;

            // Add to the display
            Controls.Add(_closeButton);
            Controls.Add(_hideButton);

            // Let base class perform common actions
            base.CreateButtons();
        }

		protected void UpdateCaptionHeight(Font captionFont)
		{
            // Dynamically calculate the required height of the caption area
            _fixedLength = (int)captionFont.GetHeight() + (_yInset + _yInsetExtra) * 2;
    
            int minHeight = _buttonHeight + _yInset * 4 + 1;

            // Maintain a minimum height to allow correct showing of buttons
            if (_fixedLength < minHeight)
                _fixedLength = minHeight;

			this.Size = new Size(_fixedLength, _fixedLength);
			
			RecalculateButtons();

			Invalidate();
		}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Overriden to prevent background being painted
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            bool focused = false;

            if (this.ParentWindow != null)
                focused = this.ParentWindow.ContainsFocus;

            // Sometimes the min/max button is created and then an attempt is made to 
            // hide the button. But for some reason it does not always manage to hide 
            // the button. So forced to check here everytime to ensure its hidden.
            UpdateMaximizeImage();

            SetButtonState();
            
            Size ourSize = this.ClientSize;

            int xEnd = ourSize.Width;
            int yEnd = ourSize.Height - _yInset * 2;

            Rectangle rectCaption = new Rectangle(0, _yInset, xEnd, yEnd - _yInset + 1);

            using(SolidBrush backBrush = new SolidBrush(this.BackColor),
                             activeBrush = new SolidBrush(_manager.ActiveColor),
                             activeTextBrush = new SolidBrush(_manager.ActiveTextColor),
                             inactiveBrush = new SolidBrush(_manager.InactiveTextColor))
            {
                // Is this control Active?
                if (focused)
                {
                    // Fill the entire background area
                    e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
    	
                    // Use a solid filled background for text
                    e.Graphics.FillRectangle(activeBrush, rectCaption);
    			
                    // Start drawing text a little from the left
                    rectCaption.X += _buttonSpacer;
                    rectCaption.Y += 1;
                    rectCaption.Height -= 2;

                    // Reduce the width to account for close button
                    rectCaption.Width -= _closeButton.Width + _buttonSpacer;

                    // Reduce width to account for the optional maximize button
                    if ((_maxButton != null) && (_maxButton.Visible))
                        rectCaption.Width -= _closeButton.Width;
    				
                    e.Graphics.DrawString(this.Text, _manager.CaptionFont, activeTextBrush, rectCaption);
                }
                else
                {
                    // Fill the entire background area
                    e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
    	            
                    // Inactive and so use a rounded rectangle
                    using (Pen dark = new Pen(ControlPaint.LightLight(_manager.InactiveTextColor)))
                    {
                        e.Graphics.DrawLine(dark, 1, _yInset, xEnd - 2, _yInset);
                        e.Graphics.DrawLine(dark, 1, yEnd, xEnd - 2, yEnd);
                        e.Graphics.DrawLine(dark, 0, _yInset + 1, 0, yEnd - 1);
                        e.Graphics.DrawLine(dark, xEnd - 1, _yInset + 1, xEnd - 1, yEnd - 1);

                        // Start drawing text a little from the left
                        rectCaption.X += _buttonSpacer;
                        rectCaption.Y += 1;
                        rectCaption.Height -= 2;

                        // Reduce the width to account for close button
                        rectCaption.Width -= _closeButton.Width + _buttonSpacer;

                        // Reduce width to account for the optional maximize button
                        if ((_maxButton != null) && (_maxButton.Visible))
                            rectCaption.Width -= _maxButton.Width;

                        e.Graphics.DrawString(this.Text, _manager.CaptionFont, inactiveBrush, rectCaption);
                    }
                }	
            }
            
            // Let delegates fire through base
            base.OnPaint(e);

            // Always get the button to repaint as we have painted over their area
            _closeButton.Refresh();
        }				
    }
}