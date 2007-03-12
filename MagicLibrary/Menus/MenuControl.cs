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
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Menus
{
    [ToolboxBitmap(typeof(MenuControl))]
    [DefaultProperty("MenuCommands")]
    [DefaultEvent("PopupSelected")]
    [Designer(typeof(Crownwood.Magic.Menus.MenuControlDesigner))]
    public class MenuControl : ContainerControl, IMessageFilter
    {
        internal class MdiClientSubclass : NativeWindow
        {
            protected override void WndProc(ref Message m)
            {
                switch(m.Msg)
                {
                    case (int)Win32.Msgs.WM_MDISETMENU:
                    case (int)Win32.Msgs.WM_MDIREFRESHMENU:
                        return;
                }

                base.WndProc(ref m);
            }			
        }

        // Class constants
        protected const int _lengthGap = 3;
        protected const int _boxExpandUpper = 1;
        protected const int _boxExpandSides = 2;
        protected const int _shadowGap = 4;
        protected const int _shadowYOffset = 4;
        protected const int _separatorWidth = 15;
        protected const int _subMenuBorderAdjust = 2;
        protected const int _minIndex = 0;
        protected const int _restoreIndex = 1;
        protected const int _closeIndex = 2;
        protected const int _chevronIndex = 3;
        protected const int _buttonLength = 16;
        protected const int _chevronLength = 12;
        protected const int _pendantLength = 48;
        protected const int _pendantOffset = 3;

        // Class constant is marked as 'readonly' to allow non constant initialization
        protected readonly int WM_OPERATEMENU = (int)Win32.Msgs.WM_USER + 1;

        // Class fields
        protected static ImageList _menuImages = null;
        protected static bool _supportsLayered = false;

        // Instance fields
        protected int _rowWidth;
        protected int _rowHeight;
        protected int _trackItem;
        protected int _breadthGap;
        protected int _animateTime;
        protected IntPtr _oldFocus;
        protected Pen _controlLPen;
        protected bool _animateFirst;
        protected bool _selected;
        protected bool _multiLine;
        protected bool _mouseOver;
        protected bool _manualFocus;
        protected bool _drawUpwards;
		protected bool _defaultFont;
        protected bool _defaultBackColor;
        protected bool _defaultTextColor;
        protected bool _defaultHighlightBackColor;
        protected bool _defaultHighlightTextColor;
        protected bool _defaultSelectedBackColor;
        protected bool _defaultSelectedTextColor;
        protected bool _defaultPlainSelectedTextColor;
        protected bool _plainAsBlock;
        protected bool _dismissTransfer;
        protected bool _ignoreMouseMove;
        protected bool _expandAllTogether;
        protected bool _rememberExpansion;
        protected bool _deselectReset;
        protected bool _highlightInfrequent;
		protected bool _exitLoop;
        protected Color _textColor;
        protected Color _highlightBackColor;
        protected Color _useHighlightBackColor;
        protected Color _highlightTextColor;
        protected Color _highlightBackDark;
        protected Color _highlightBackLight;
        protected Color _selectedBackColor;
        protected Color _selectedTextColor;
        protected Color _plainSelectedTextColor;
        protected Form _activeChild;
        protected Form _mdiContainer;
        protected VisualStyle _style;
        protected Direction _direction;
        protected PopupMenu _popupMenu;
        protected ArrayList _drawCommands;
        protected SolidBrush _controlLBrush;
        protected SolidBrush _backBrush;
        protected Animate _animate;
        protected Animation _animateStyle;
        internal MdiClientSubclass _clientSubclass;
        protected MenuCommand _chevronStartCommand;
        protected MenuCommandCollection _menuCommands;

        // Instance fields - pendant buttons
        protected InertButton _minButton;
        protected InertButton _restoreButton;
        protected InertButton _closeButton;

        // Instance fields - events
        public event CommandHandler Selected;
        public event CommandHandler Deselected;

        static MenuControl()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _menuImages = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.Magic.Menus.MenuControl"),
                                                         "Crownwood.Magic.Resources.ImagesMenuControl.bmp",
                                                         new Size(_buttonLength, _buttonLength),
                                                         new Point(0,0));

            // We need to know if the OS supports layered windows
            _supportsLayered = (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null);
        }

        public MenuControl()
        {
            // Set default values
            _trackItem = -1;
            _oldFocus = IntPtr.Zero;
            _minButton = null;
            _popupMenu = null;
            _activeChild = null;
            _closeButton = null;
            _controlLPen = null;
            _mdiContainer = null;
            _restoreButton = null;
            _controlLBrush = null;
            _chevronStartCommand = null;
            _animateFirst = true;
			_exitLoop = false;
            _selected = false;
            _multiLine = false;
            _mouseOver = false;
			_defaultFont = true;
            _manualFocus = false;
            _drawUpwards = false;
            _plainAsBlock = false;
            _clientSubclass = null;
            _ignoreMouseMove = false;
            _deselectReset = true;
            _expandAllTogether = true;
            _rememberExpansion = true;
            _highlightInfrequent = true;
            _dismissTransfer = false;
            _style = VisualStyle.IDE;
            _direction = Direction.Horizontal;
            _menuCommands = new MenuCommandCollection();
            this.Dock = DockStyle.Top;
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			
            // Animation details
            _animateTime = 100;
            _animate = Animate.System;
            _animateStyle = Animation.System;

            // Prevent flicker with double buffering and all painting inside WM_PAINT
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Should not be allowed to select this control
            SetStyle(ControlStyles.Selectable, false);

            // Hookup to collection events
            _menuCommands.Cleared += new CollectionClear(OnCollectionCleared);
            _menuCommands.Inserted += new CollectionChange(OnCollectionInserted);
            _menuCommands.Removed += new CollectionChange(OnCollectionRemoved);

			// Need notification when the MenuFont is changed
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
			    new UserPreferenceChangedEventHandler(OnPreferenceChanged);

            DefineColors();
            
            // Set the starting Font
            DefineFont(SystemInformation.MenuFont);

            // Do not allow tab key to select this control
            this.TabStop = false;

            // Default to one line of items
            this.Height = _rowHeight;

            // Add ourself to the application filtering list
            Application.AddMessageFilter(this);
        }

        protected override void Dispose(bool disposing)
        {
            if( disposing )
            {
                // Remove notifications
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
                    new UserPreferenceChangedEventHandler(OnPreferenceChanged);
            }
            base.Dispose( disposing );
        }

        [Category("Behaviour")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MenuCommandCollection MenuCommands
        {
            get { return _menuCommands; }
        } 

        [Category("Appearance")]
        public VisualStyle Style
        {
            get { return _style; }
			
            set
            {
                if (_style != value)
                {
                    _style = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

        public override Font Font
        {
            get { return base.Font; }
			
            set
            {
				if (value != base.Font)
				{
					_defaultFont = (value == SystemInformation.MenuFont);

					DefineFont(value);

					Recalculate();
					Invalidate();
				}
            }
        }

		public override Color BackColor
		{
			get { return base.BackColor; }

			set
			{
				if (value != base.BackColor)
				{
					_defaultBackColor = (value == SystemColors.Control);
                    base.BackColor = value;
                    _backBrush = new SolidBrush(base.BackColor);
                    
					Recalculate();
					Invalidate();
				}
			}
		}

        private bool ShouldSerializeBackColor()
        {
            return this.BackColor != SystemColors.Control;
        }

        [Category("Appearance")]
        public Color TextColor
        {
            get { return _textColor; }

            set
            {
                if (value != _textColor)
                {
                    _textColor = value;
                    _defaultTextColor = (value == SystemColors.MenuText);

                    Recalculate();
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeTextColor()
        {
            return _textColor != SystemColors.MenuText;
        }

        [Category("Appearance")]
        public Color HighlightBackColor
        {
            get { return _highlightBackColor; }

            set
            {
                if (value != _highlightBackColor)
                {
                    _defaultHighlightBackColor = (value == SystemColors.Highlight);
                    DefineHighlightBackColors(value);                    

                    Recalculate();
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeHighlightBackColor()
        {
            return _highlightBackColor != SystemColors.Highlight;
        }

        [Category("Appearance")]
        public Color HighlightTextColor
        {
            get { return _highlightTextColor; }

            set
            {
                if (value != _highlightTextColor)
                {
                    _highlightTextColor = value;
                    _defaultHighlightTextColor = (value == SystemColors.MenuText);

                    Recalculate();
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeHighlightTextColor()
        {
            return _highlightTextColor != SystemColors.HighlightText;
        }

        [Category("Appearance")]
        public Color SelectedBackColor
        {
            get { return _selectedBackColor; }

            set
            {
                if (value != _selectedBackColor)
                {
                    DefineSelectedBackColors(value);
                    _defaultSelectedBackColor = (value == SystemColors.Control);

                    Recalculate();
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeSelectedBackColor()
        {
            return _selectedBackColor != SystemColors.Control;
        }

        [Category("Appearance")]
        public Color SelectedTextColor
        {
            get { return _selectedTextColor; }

            set
            {
                if (value != _selectedTextColor)
                {
                    _selectedTextColor = value;
                    _defaultSelectedTextColor = (value == SystemColors.MenuText);

                    Recalculate();
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeSelectedTextColor()
        {
            return _selectedTextColor != SystemColors.MenuText;
        }

        [Category("Appearance")]
        public Color PlainSelectedTextColor
        {
            get { return _plainSelectedTextColor; }

            set
            {
                if (value != _plainSelectedTextColor)
                {
                    _plainSelectedTextColor = value;
                    _defaultPlainSelectedTextColor = (value == SystemColors.ActiveCaptionText);

                    Recalculate();
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializePlainSelectedTextColor()
        {
            return _plainSelectedTextColor != SystemColors.ActiveCaptionText;
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool PlainAsBlock
        {
            get { return _plainAsBlock; }

            set
            {
                if (_plainAsBlock != value)
                {
	                _plainAsBlock = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool MultiLine
        {
            get { return _multiLine; }

            set
            {
                if (_multiLine != value)
                {
                    _multiLine = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        public Direction Direction
        {
            get { return _direction; }

            set
            {
                if (_direction != value)
                {
                    _direction = value;

                    Recalculate();
                    Invalidate();
                }
            }
        }

        [Category("Behaviour")]
        [DefaultValue(true)]
        public bool RememberExpansion
        {
            get { return _rememberExpansion; }
            set { _rememberExpansion = value; }
        }

        [Category("Behaviour")]
        [DefaultValue(true)]
        public bool ExpandAllTogether
        {
            get { return _expandAllTogether; }
            set { _expandAllTogether = value; }
        }
        
        [Category("Behaviour")]
        [DefaultValue(true)]
        public bool DeselectReset
        {
            get { return _deselectReset; }
            set { _deselectReset = value; }
        }
        
        [Category("Behaviour")]
        [DefaultValue(true)]
        public bool HighlightInfrequent
        {
            get { return _highlightInfrequent; }
            set { _highlightInfrequent = value; }
        }

        public override DockStyle Dock
        {
            get { return base.Dock; }

            set
            {
                base.Dock = value;

                switch(value)
                {
                    case DockStyle.None:
                        _direction = Direction.Horizontal;
                        break;
                    case DockStyle.Top:
                    case DockStyle.Bottom:
                        this.Height = 0;
                        _direction = Direction.Horizontal;
                        break;
                    case DockStyle.Left:
                    case DockStyle.Right:
                        this.Width = 0;
                        _direction = Direction.Vertical;
                        break;
                }

                Recalculate();
                Invalidate();
            }
        }

        [Category("Animate")]
        [DefaultValue(typeof(Animate), "System")]
        public Animate Animate
        {
            get { return _animate; }
            set { _animate = value; }
        }

        [Category("AnimateTime")]
        public int AnimateTime
        {
            get { return _animateTime; }
            set { _animateTime = value; }
        }

        [Category("AnimateStyle")]
        public Animation AnimateStyle
        {
            get { return _animateStyle; }
            set { _animateStyle = value; }
        }

        [Category("Behaviour")]
        [DefaultValue(null)]
        public Form MdiContainer
        {
            get { return _mdiContainer; }
			
            set
            {
                if (_mdiContainer != value)
                {
                    if (_mdiContainer != null)
                    {
                        // Unsubclass from MdiClient and then remove object reference
                        _clientSubclass.ReleaseHandle();
                        _clientSubclass = null;

                        // Remove registered events
                        _mdiContainer.MdiChildActivate -= new EventHandler(OnMdiChildActivate);

                        RemovePendantButtons();
                    }

                    _mdiContainer = value;

                    if (_mdiContainer != null)
                    {
                        CreatePendantButtons();

                        foreach(Control c in _mdiContainer.Controls)
                        {
                            MdiClient client = c as MdiClient;

                            if (client != null)
                            {
                                // We need to subclass the MdiClient to prevent any attempt
                                // to change the menu for the container Form. This prevents
                                // the system from automatically adding the pendant.
                                _clientSubclass = new MdiClientSubclass();
                                _clientSubclass.AssignHandle(client.Handle);
                            }
                        }

                        // Need to be informed of the active child window
                        _mdiContainer.MdiChildActivate += new EventHandler(OnMdiChildActivate);
                    }
                }
            }
        }

        protected void DefineColors()
        {
            // Define starting text colors
            _defaultTextColor = true;
            _defaultHighlightTextColor = true;
            _defaultSelectedTextColor = true;
            _defaultPlainSelectedTextColor = true;
            _textColor = SystemColors.MenuText;
            _highlightTextColor = SystemColors.MenuText;
            _selectedTextColor = SystemColors.MenuText;
			_plainSelectedTextColor = SystemColors.ActiveCaptionText;

            // Define starting back colors
            _defaultBackColor = true;
            _defaultHighlightBackColor = true;
            _defaultSelectedBackColor = true;
            base.BackColor = SystemColors.Control;
            _backBrush = new SolidBrush(base.BackColor);
            _highlightBackColor = SystemColors.Highlight;            
            DefineHighlightBackColors(SystemColors.Highlight);
            DefineSelectedBackColors(SystemColors.Control);
        }
        
        public void ResetColors()
        {
            this.BackColor = SystemColors.Control;
            this.TextColor = SystemColors.MenuText;
            this.HighlightBackColor = SystemColors.Highlight;            
            this.HighlightTextColor = SystemColors.MenuText;
            this.SelectedBackColor = SystemColors.Control;
            this.SelectedTextColor = SystemColors.MenuText;
        }

		protected void DefineFont(Font newFont)
		{
			base.Font = newFont;

			_breadthGap = (this.Font.Height / 3) + 1;

            // Calculate the initial height/width of the control
            _rowWidth = _rowHeight = this.Font.Height + _breadthGap * 2 + 1;
		}

        protected void DefineSelectedBackColors(Color baseColor)
        {
            _selectedBackColor = baseColor;
            _controlLPen = new Pen(Color.FromArgb(200, baseColor));
            _controlLBrush = new SolidBrush(Color.FromArgb(200, baseColor));
        }

        protected void DefineHighlightBackColors(Color baseColor)
        {
            _highlightBackColor = baseColor;
        
            if (_defaultHighlightBackColor)
            {
                _highlightBackDark = SystemColors.Highlight;
                _highlightBackLight = Color.FromArgb(70, _highlightBackDark);
            }
            else
            {
                _highlightBackDark = ControlPaint.Dark(baseColor);
                _highlightBackLight = baseColor;
            }
        }

        public virtual void OnSelected(MenuCommand mc)
        {
            // Any attached event handlers?
            if (Selected != null)
                Selected(mc);
        }

        public virtual void OnDeselected(MenuCommand mc)
        {
            // Any attached event handlers?
            if (Deselected != null)
                Deselected(mc);
        }

        protected void OnCollectionCleared()
        {
            // Reset state ready for a recalculation
            Deselect();
            RemoveItemTracking();

            Recalculate();
            Invalidate();
        }

        protected void OnCollectionInserted(int index, object value)
        {
            MenuCommand mc = value as MenuCommand;

            // We need notification whenever the properties of this command change
            mc.PropertyChanged += new MenuCommand.PropChangeHandler(OnCommandChanged);
				
            // Reset state ready for a recalculation
            Deselect();
            RemoveItemTracking();

            Recalculate();
            Invalidate();
        }

        protected void OnCollectionRemoved(int index, object value)
        {
            // Reset state ready for a recalculation
            Deselect();
            RemoveItemTracking();

            Recalculate();
            Invalidate();
        }

        protected void OnCommandChanged(MenuCommand item, MenuCommand.Property prop)
        {
            Recalculate();
            Invalidate();
        }

        protected void OnMdiChildActivate(object sender, EventArgs e)
        {
            // Unhook from event
            if (_activeChild != null)
                _activeChild.SizeChanged -= new EventHandler(OnMdiChildSizeChanged);

            // Remember the currently active child form
            _activeChild = _mdiContainer.ActiveMdiChild;

            // Need to know when window becomes maximized
            if (_activeChild != null)
                _activeChild.SizeChanged += new EventHandler(OnMdiChildSizeChanged);

            // Might be a change in pendant requirements
            Recalculate();
            Invalidate();
        }

        protected void OnMdiChildSizeChanged(object sender, EventArgs e)
        {
            // Has window changed to become maximized?
            if (_activeChild.WindowState == FormWindowState.Maximized)
            {
                // Reflect change in menu
                Recalculate();
                Invalidate();
            }
        }

        protected void OnMdiMin(object sender, EventArgs e)
        {
            if (_activeChild != null)
            {
                _activeChild.WindowState = FormWindowState.Minimized;

                // Reflect change in menu
                Recalculate();
                Invalidate();
            }
        }

        protected void OnMdiRestore(object sender, EventArgs e)
        {
            if (_activeChild != null)
            {
                _activeChild.WindowState = FormWindowState.Normal;

                // Reflect change in menu
                Recalculate();
                Invalidate();
            }
        }

        protected void OnMdiClose(object sender, EventArgs e)
        {
            if (_activeChild != null)
            {
                _activeChild.Close();

                // Reflect change in menu
                Recalculate();
                Invalidate();
            }
        }

        protected void CreatePendantButtons()
        {
            // Create the objects
            _minButton = new InertButton(_menuImages, _minIndex);
            _restoreButton = new InertButton(_menuImages, _restoreIndex);
            _closeButton = new InertButton(_menuImages, _closeIndex);

            // Define the constant sizes
            _minButton.Size = new Size(_buttonLength, _buttonLength);
            _restoreButton.Size = new Size(_buttonLength, _buttonLength);
            _closeButton.Size = new Size(_buttonLength, _buttonLength);

            // Default their position so they are not visible
            _minButton.Location = new Point(-_buttonLength, -_buttonLength);
            _restoreButton.Location = new Point(-_buttonLength, -_buttonLength);
            _closeButton.Location = new Point(-_buttonLength, -_buttonLength);

            // Hookup event handlers
            _minButton.Click += new EventHandler(OnMdiMin);
            _restoreButton.Click += new EventHandler(OnMdiRestore);
            _closeButton.Click += new EventHandler(OnMdiClose);

            // Add to display
            this.Controls.AddRange(new Control[]{_minButton, _restoreButton, _closeButton});
        }

        protected void RemovePendantButtons()
        {
            // Unhook event handlers
            _minButton.Click -= new EventHandler(OnMdiMin);
            _restoreButton.Click -= new EventHandler(OnMdiRestore);
            _closeButton.Click -= new EventHandler(OnMdiClose);

            // Remove from display

			// Use helper method to circumvent form Close bug
			ControlHelper.Remove(this.Controls, _minButton);
			ControlHelper.Remove(this.Controls, _restoreButton);
			ControlHelper.Remove(this.Controls, _closeButton);

            // Release resources
            _minButton.Dispose();
            _restoreButton.Dispose();
            _closeButton.Dispose();

            // Remove references
            _minButton = null;
            _restoreButton = null;
            _closeButton = null;
        }
        
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            // Have we become disabled?
            if (!this.Enabled)
            {
                // Is an item selected?
                if (_selected)
                {
                    // Is a popupMenu showing?
                    if (_popupMenu != null)
                    {
                        // Dismiss the submenu
                        _popupMenu.Dismiss();

                        // No reference needed
                        _popupMenu = null;
                    }

                    // Reset state
                    Deselect();
                    _drawUpwards = false;

                    SimulateReturnFocus();
                }
            }

            // Do not draw any item as being tracked
            RemoveItemTracking();

            // Change in state changes the way items are drawn
            Invalidate();
        }

        internal void OnWM_MOUSEDOWN(Win32.POINT screenPos)
        {
            // Convert the mouse position to screen coordinates
            User32.ScreenToClient(this.Handle, ref screenPos);

            OnProcessMouseDown(screenPos.x, screenPos.y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnProcessMouseDown(e.X, e.Y);

            base.OnMouseDown(e);
        }
		
        protected void OnProcessMouseDown(int xPos, int yPos)
        {
            Point pos = new Point(xPos, yPos);

            for(int i=0; i<_drawCommands.Count; i++)
            {
                DrawCommand dc = _drawCommands[i] as DrawCommand;

                // Find the DrawCommand this is over
                if (dc.SelectRect.Contains(pos) && dc.Enabled)
                {
                    // Is an item already selected?
                    if (_selected)
                    {
                        // Is it this item that is already selected?
                        if (_trackItem == i)
                        {
                            // Is a popupMenu showing
                            if (_popupMenu != null)
                            {
                                // Dismiss the submenu
                                _popupMenu.Dismiss();

                                // No reference needed
                                _popupMenu = null;
                            }
                        }
                    }
                    else
                    {
                        // Select the tracked item
                        _selected = true;
                        _drawUpwards = false;
								
                        // Is there a change in tracking?
                        if (_trackItem != i)
                        {
                            // Modify the display of the two items 
                            _trackItem = SwitchTrackingItem(_trackItem, i);
                        }
                        else
                        {
                            // Update display to show as selected
                            DrawCommand(_trackItem, true);
                        }

                        // Is there a submenu to show?
                        if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
                            User32.PostMessage(this.Handle, WM_OPERATEMENU, 1, 0);
                    }

                    break;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Is an item currently being tracked?
            if (_trackItem != -1)
            {
                // Is it also selected?
                if (_selected == true)
                {
                    // Is it also showing a submenu
                    if (_popupMenu == null)
                    {
                        // Deselect the item
                        Deselect();
                        _drawUpwards = false;

                        DrawCommand(_trackItem, true);

                        SimulateReturnFocus();
                    }
                }
            }

            base.OnMouseUp(e);
        }

        internal void OnWM_MOUSEMOVE(Win32.POINT screenPos)
        {
            // Convert the mouse position to screen coordinates
            User32.ScreenToClient(this.Handle, ref screenPos);

            OnProcessMouseMove(screenPos.x, screenPos.y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Sometimes we need to ignore this message
            if (_ignoreMouseMove)
                _ignoreMouseMove = false;
            else
                OnProcessMouseMove(e.X, e.Y);

            base.OnMouseMove(e);
        }

        protected void OnProcessMouseMove(int xPos, int yPos)
        {
            // Sometimes we need to ignore this message
            if (_ignoreMouseMove)
                _ignoreMouseMove = false;
            else
            {
                // Is the first time we have noticed a mouse movement over our window
                if (!_mouseOver)
                {
                    // Create the structure needed for User32 call
                    Win32.TRACKMOUSEEVENTS tme = new Win32.TRACKMOUSEEVENTS();

                    // Fill in the structure
                    tme.cbSize = 16;									
                    tme.dwFlags = (uint)Win32.TrackerEventFlags.TME_LEAVE;
                    tme.hWnd = this.Handle;								
                    tme.dwHoverTime = 0;								

                    // Request that a message gets sent when mouse leaves this window
                    User32.TrackMouseEvent(ref tme);

                    // Yes, we know the mouse is over window
                    _mouseOver = true;
                }

                Form parentForm = this.FindForm();

                // Only hot track if this Form is active
                if ((parentForm != null) && parentForm.ContainsFocus)
                {
                    Point pos = new Point(xPos, yPos);

                    int i = 0;

                    for(i=0; i<_drawCommands.Count; i++)
                    {
                        DrawCommand dc = _drawCommands[i] as DrawCommand;

                        // Find the DrawCommand this is over
                        if (dc.SelectRect.Contains(pos) && dc.Enabled)
                        {
                            // Is there a change in selected item?
                            if (_trackItem != i)
                            {
                                // We are currently selecting an item
                                if (_selected)
                                {
                                    if (_popupMenu != null)
                                    {
                                        // Note that we are dismissing the submenu but not removing
                                        // the selection of items, this flag is tested by the routine
                                        // that will return because the submenu has been finished
                                        _dismissTransfer = true;

                                        // Dismiss the submenu
                                        _popupMenu.Dismiss();
		
                                        // Default to downward drawing
                                        _drawUpwards = false;
                                    }

                                    // Modify the display of the two items 
                                    _trackItem = SwitchTrackingItem(_trackItem, i);

                                    // Does the newly selected item have a submenu?
                                    if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))	
                                        User32.PostMessage(this.Handle, WM_OPERATEMENU, 1, 0);
                                }
                                else
                                {
                                    // Modify the display of the two items 
                                    _trackItem = SwitchTrackingItem(_trackItem, i);
                                }
                            }

                            break;
                        }
                    }

                    // If not in selected mode
                    if (!_selected)
                    {
                        // None of the commands match?
                        if (i == _drawCommands.Count)
                        {
                            // If we have the focus then do not change the tracked item
                            if (!_manualFocus)
                            {
                                // Modify the display of the two items 
                                _trackItem = SwitchTrackingItem(_trackItem, -1);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseOver = false;

            // If we manually grabbed focus then do not switch
            // selection when the mouse leaves the control area
            if (!_manualFocus)
            {
                if (_trackItem != -1)
                {
                    // If an item is selected then do not change tracking item when the 
                    // mouse leaves the control area, as a popup menu might be showing and 
                    // so keep the tracking and selection indication visible
                    if (_selected == false)
                        _trackItem = SwitchTrackingItem(_trackItem, -1);
                }
            }

            base.OnMouseLeave(e);
        }

        protected override void OnResize(EventArgs e)
        {
            Recalculate();

            // Any resize of control should redraw all of it otherwise when you 
            // stretch to the right it will not paint correctly as we have a one
            // pixel gap between text and min button which is not honoured otherwise
            this.Invalidate();

            base.OnResize(e);
        }

        internal void DrawSelectionUpwards()
        {
            // Double check the state is correct for this method to be called
            if ((_trackItem != -1) && (_selected))
            {
                // This flag is tested in the DrawCommand method
                _drawUpwards = true;

                // Force immediate redraw of the item
                DrawCommand(_trackItem, true);
            }
        }

        protected void Deselect()
        {
            // The next submenu should be animated
            _animateFirst = true;

            // Remove selection state
            _selected = false;
            
            // Should expansion items be reset on deselection?
            if (_deselectReset)
            {
                // Set everything to expanded
                SetAllCommandsExpansion(_menuCommands, false);
            }
        }

        protected void Recalculate()
        {
            int length;

            if (_direction == Direction.Horizontal)
                length = this.Width;
            else 
                length = this.Height;

            // Is there space for any commands?
            if (length > 0)
            {
                // Count the number of rows needed
                int rows = 0;

                // Number of items on this row
                int columns = 0;

                // Create a collection of drawing objects
                _drawCommands = new ArrayList();

                // Minimum length is a gap on either side of the text
                int cellMinLength = _lengthGap * 2;

                // Each cell is as broad as the whole control
                int cellBreadth = this.Height;
				
                // Accumulate starting position of each cell
                int lengthStart = 0;

				// Allow enough space to draw a chevron
                length -= (cellMinLength + _chevronLength);

                bool showPendant = ((rows == 0) && (_activeChild != null));

				// If we are showing on a single line but the active child is not 
				// currently maximized then we can show a menu item in pendant space
				if (showPendant && !_multiLine && (_activeChild.WindowState != FormWindowState.Maximized))
					showPendant = false;

                // Pendant positioning information
                int xPos = 0; 
                int yPos = 0;
                int xInc = 0;
                int yInc = 0;

                // First line needs extra space for pendant
                if (showPendant)
                {
                    length -= (_pendantLength + _pendantOffset + _shadowGap);

                    bool popupStyle = (_style == VisualStyle.IDE);
                    int borderWidth = (_style == VisualStyle.IDE) ? 1 : 2;

                    // Define the correct visual style
                    _minButton.PopupStyle = popupStyle;
                    _restoreButton.PopupStyle = popupStyle;
                    _closeButton.PopupStyle = popupStyle;

                    // Define correct border width
                    _minButton.BorderWidth = borderWidth;
                    _restoreButton.BorderWidth = borderWidth;
                    _closeButton.BorderWidth = borderWidth;

                    if (_direction == Direction.Horizontal)
                    {
                        xPos = this.Width - _pendantOffset - _buttonLength;
                        yPos = _pendantOffset;
                        xInc = -_buttonLength;
                    }
                    else
                    {
                        xPos = _pendantOffset;
                        yPos = this.Height - _pendantOffset - _buttonLength;
                        yInc = -_buttonLength;
                    }
                }

                // Assume chevron is not needed by default
                _chevronStartCommand = null;

                using(Graphics g = this.CreateGraphics())
                {
                    // Count the item we are processing
                    int index = 0;

                    foreach(MenuCommand command in _menuCommands)
                    {
                        // Give the command a chance to update its state
                        command.OnUpdate(EventArgs.Empty);

                        // Ignore items that are marked as hidden
                        if (!command.Visible)
                            continue;

                        int cellLength = 0;

                        // Is this a separator?
                        if (command.Text == "-")
                            cellLength = _separatorWidth;
                        else
                        {
                            // Calculate the text width of the cell
                            SizeF dimension = g.MeasureString(command.Text, this.Font);

                            // Always add 1 to ensure that rounding is up and not down
                            cellLength = cellMinLength + (int)dimension.Width + 1;
                        }

                        Rectangle cellRect;

                        // Create a new position rectangle
                        if (_direction == Direction.Horizontal)
                            cellRect = new Rectangle(lengthStart, _rowHeight * rows, cellLength, _rowHeight);
                        else
                            cellRect = new Rectangle(_rowWidth * rows, lengthStart, _rowWidth, cellLength);

                        lengthStart += cellLength;
                        columns++;

                        // If this item is overlapping the control edge and it is not the first
                        // item on the line then we should wrap around to the next row.
                        if ((lengthStart > length) && (columns > 1))
                        {
                            if (_multiLine)
                            {
                                // Move to next row
                                rows++;

                                // Reset number of items on this column
                                columns = 1;

                                // Reset starting position of next item
                                lengthStart = cellLength;

                                // Reset position of this item
                                if (_direction == Direction.Horizontal)
                                {
                                    cellRect.X = 0;
                                    cellRect.Y += _rowHeight;
                                }
                                else
                                {
                                    cellRect.X += _rowWidth;
                                    cellRect.Y = 0;
                                }

                                // Only the first line needs extra space for pendant
                                if (showPendant && (rows == 1))
                                    length += (_pendantLength + _pendantOffset);
                            }
                            else
                            {
                                // Is a tracked item being make invisible
                                if (index <= _trackItem)
                                {
                                    // Need to remove tracking of this item
                                    RemoveItemTracking();
                                }

                                // Remember which item is first for the chevron submenu
                                _chevronStartCommand = command;

                                if (_direction == Direction.Horizontal)
                                {
                                    cellRect.Y = 0;
                                    cellRect.Width = cellMinLength + _chevronLength;
                                    cellRect.X = this.Width - cellRect.Width;
                                    cellRect.Height = _rowHeight;
                                    xPos -= cellRect.Width;
                                }
                                else
                                {
                                    cellRect.X = 0;
                                    cellRect.Height = cellMinLength + _chevronLength;
                                    cellRect.Y = this.Height - (cellMinLength + _chevronLength);
                                    cellRect.Width = _rowWidth;
                                    yPos -= cellRect.Height;
                                }

                                // Create a draw command for this chevron
                                _drawCommands.Add(new DrawCommand(cellRect));

                                // Exit, do not add the current item or any afterwards
                                break;
                            }
                        }

						Rectangle selectRect = cellRect;

						// Selection rectangle differs from drawing rectangle with IDE, because pressing the
						// mouse down causes the menu to appear and because the popup menu appears drawn slightly
						// over the drawing area the mouse up might select the first item in the popup. 
						if (_style == VisualStyle.IDE)
						{
							// Modify depending on orientation
							if (_direction == Direction.Horizontal)
								selectRect.Height -= (_lengthGap + 2);
							else
								selectRect.Width -= _breadthGap;
						}

                        // Create a drawing object
                        _drawCommands.Add(new DrawCommand(command, cellRect, selectRect));

                        index++;
                    }
                }

                // Position the pendant buttons
                if (showPendant)
                {
                    if (_activeChild.WindowState == FormWindowState.Maximized)
                    {
                        // Window maximzied, must show the buttons
                        if (!_minButton.Visible)
                        {
                            _minButton.Show();
                            _restoreButton.Show();
                            _closeButton.Show();
                        }
	
                        // Only enabled minimize box if child is allowed to be minimized
                        _minButton.Enabled = _activeChild.MinimizeBox;

                        _closeButton.Location = new Point(xPos, yPos);

                        xPos += xInc; yPos += yInc;
                        _restoreButton.Location = new Point(xPos, yPos);

                        xPos += xInc; yPos += yInc;
                        _minButton.Location = new Point(xPos, yPos);
                    }
                    else
                    {
                        // No window is maximized, so hide the buttons
                        if (_minButton.Visible)
                        {
                            _minButton.Hide();
                            _restoreButton.Hide();
                            _closeButton.Hide();
                        }
                    }
                }
                else
                {
                    // No window is maximized, so hide the buttons
                    if ((_minButton != null) && _minButton.Visible)
                    {
                        _minButton.Hide();
                        _restoreButton.Hide();
                        _closeButton.Hide();
                    }
                }

                if (_direction == Direction.Horizontal)
                {
                    int controlHeight = (rows + 1) * _rowHeight;

                    // Ensure the control is the correct height
                    if (this.Height != controlHeight)
                        this.Height = controlHeight;
                }
                else
                {
                    int controlWidth = (rows + 1) * _rowWidth;

                    // Ensure the control is the correct width
                    if (this.Width != controlWidth)
                        this.Width = controlWidth;
                }				
            }
        }

        protected void DrawCommand(int drawItem, bool tracked)
        {
            // Create a graphics object for drawing
            using(Graphics g = this.CreateGraphics())
                DrawSingleCommand(g, _drawCommands[drawItem] as DrawCommand, tracked);
        }

        internal void DrawSingleCommand(Graphics g, DrawCommand dc, bool tracked)
        {
            Rectangle drawRect = dc.DrawRect;
            MenuCommand mc = dc.MenuCommand;

            // Copy the rectangle used for drawing cell
            Rectangle shadowRect = drawRect;

            // Expand to right and bottom to cover the area used to draw shadows
            shadowRect.Width += _shadowGap;
            shadowRect.Height += _shadowGap;

            // Draw background color over cell and shadow area to the right
            g.FillRectangle(_backBrush, shadowRect);

            if (!dc.Separator)
            {
                Rectangle textRect;

                // Text rectangle size depends on type of draw command we are drawing
                if (dc.Chevron)
                {
                    // Create chevron drawing rectangle
                    textRect = new Rectangle(drawRect.Left + _lengthGap, drawRect.Top + _boxExpandUpper,
                                             drawRect.Width - _lengthGap * 2, drawRect.Height - (_boxExpandUpper * 2));
                }
                else
                {
                    // Create text drawing rectangle
                    textRect = new Rectangle(drawRect.Left + _lengthGap, drawRect.Top + _lengthGap,
                                             drawRect.Width - _lengthGap * 2, drawRect.Height - _lengthGap * 2);
                }

                if (dc.Enabled)
                {
                    // Draw selection 
                    if (tracked)
                    {
                        Rectangle boxRect;

                        // Create the rectangle for box around the text
                        if (_direction == Direction.Horizontal)
                        {
                            boxRect = new Rectangle(textRect.Left - _boxExpandSides,
                                                    textRect.Top - _boxExpandUpper,
                                                    textRect.Width + _boxExpandSides * 2,
                                                    textRect.Height + _boxExpandUpper);
                        }
                        else
                        {					
                            if (!dc.Chevron)
                            {
                                boxRect = new Rectangle(textRect.Left,
                                                        textRect.Top - _boxExpandSides,
                                                        textRect.Width - _boxExpandSides,
                                                        textRect.Height + _boxExpandSides * 2);
                            }
                            else
                                boxRect = textRect;
                        }

                        switch(_style)
                        {
                            case VisualStyle.IDE:
                                if (_selected)
                                {
                                    // Fill the entire inside
                                    g.FillRectangle(Brushes.White, boxRect);
                                    g.FillRectangle(_controlLBrush, boxRect);
								
                                    Color extraColor = Color.FromArgb(64, 0, 0, 0);
                                    Color darkColor = Color.FromArgb(48, 0, 0, 0);
                                    Color lightColor = Color.FromArgb(0, 0, 0, 0);
                
                                    int rightLeft = boxRect.Right + 1;
                                    int rightBottom = boxRect.Bottom;

									if (_drawUpwards && (_direction == Direction.Horizontal))                                    
									{
                                        // Draw the box around the selection area
                                        using(Pen dark = new Pen(ControlPaint.Dark(_selectedBackColor)))
                                            g.DrawRectangle(dark, boxRect);

										if (dc.SubMenu)
										{
											// Right shadow
											int rightTop = boxRect.Top;
											int leftLeft = boxRect.Left + _shadowGap;

											// Bottom shadow
											int top = boxRect.Bottom + 1;
											int left = boxRect.Left + _shadowGap;
											int width = boxRect.Width + 1;
											int height = _shadowGap;

											Brush rightShadow;
											Brush bottomLeftShadow;
											Brush bottomShadow;
											Brush bottomRightShadow;

											// Decide if we need to use an alpha brush
											if (_supportsLayered)
											{
												// Create brushes
												rightShadow = new LinearGradientBrush(new Point(rightLeft, 9999),
																					  new Point(rightLeft + _shadowGap, 9999),
																					  darkColor, lightColor);

												bottomLeftShadow = new LinearGradientBrush(new Point(left + _shadowGap, top - _shadowGap),
																						   new Point(left, top + height),
																						   extraColor, lightColor);

												bottomRightShadow = new LinearGradientBrush(new Point(left + width - _shadowGap - 2, top - _shadowGap - 2),
																							new Point(left + width, top + height),
																							extraColor, lightColor);

												bottomShadow = new LinearGradientBrush(new Point(9999, top),
																					   new Point(9999, top + height),
																					   darkColor, lightColor);
											}
											else
											{
												rightShadow = new SolidBrush(SystemColors.ControlDark);
												bottomLeftShadow = rightShadow;
												bottomShadow = rightShadow;
												bottomRightShadow = rightShadow;
											}

											// Draw each part of the shadow area
											g.FillRectangle(rightShadow, new Rectangle(rightLeft, rightTop, _shadowGap,  rightBottom - rightTop + 1));
											g.FillRectangle(bottomLeftShadow, left, top, _shadowGap, height);
											g.FillRectangle(bottomRightShadow, left + width - _shadowGap, top, _shadowGap, height);
											g.FillRectangle(bottomShadow, left + _shadowGap, top, width - _shadowGap * 2, height);

											// Dispose of brush objects		
											if (_supportsLayered)
											{
												rightShadow.Dispose();
												bottomLeftShadow.Dispose();
												bottomShadow.Dispose();
												bottomRightShadow.Dispose();
											}
											else
												rightShadow.Dispose();
										}
                                    }
                                    else
                                    {
                                        // Draw the box around the selection area
                                        using(Pen dark = new Pen(ControlPaint.Dark(_selectedBackColor)))
                                            g.DrawRectangle(dark, boxRect);

										if (dc.SubMenu)
										{
											if (_direction == Direction.Horizontal)
											{
												// Remove the bottom line of the selection area
												g.DrawLine(Pens.White, boxRect.Left, boxRect.Bottom, boxRect.Right, boxRect.Bottom);
												g.DrawLine(_controlLPen, boxRect.Left, boxRect.Bottom, boxRect.Right, boxRect.Bottom);

												int rightTop = boxRect.Top + _shadowYOffset;

												Brush shadowBrush;

												// Decide if we need to use an alpha brush
												if (_supportsLayered && (_style == VisualStyle.IDE))
												{
													using(LinearGradientBrush topBrush = new LinearGradientBrush(new Point(rightLeft - _shadowGap, rightTop + _shadowGap), 
																												 new Point(rightLeft + _shadowGap, rightTop),
																												 extraColor, lightColor))
													{
														g.FillRectangle(topBrush, new Rectangle(rightLeft, rightTop, _shadowGap, _shadowGap));
                        
														rightTop += _shadowGap;
													}

													shadowBrush = new LinearGradientBrush(new Point(rightLeft, 9999), 
																						  new Point(rightLeft + _shadowGap, 9999),
																						  darkColor, lightColor);
												}
												else
													shadowBrush = new SolidBrush(SystemColors.ControlDark);

												g.FillRectangle(shadowBrush, new Rectangle(rightLeft, rightTop, _shadowGap, rightBottom - rightTop));

												shadowBrush.Dispose();
											}
											else
											{
												// Remove the right line of the selection area
												g.DrawLine(Pens.White, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
												g.DrawLine(_controlLPen, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);

												int leftLeft = boxRect.Left + _shadowYOffset;

												Brush shadowBrush;

												// Decide if we need to use an alpha brush
												if (_supportsLayered && (_style == VisualStyle.IDE))
												{
													using(LinearGradientBrush topBrush = new LinearGradientBrush(new Point(leftLeft + _shadowGap, rightBottom + 1 - _shadowGap), 
																												 new Point(leftLeft, rightBottom + 1 + _shadowGap),
																												 extraColor, lightColor))
													{
														g.FillRectangle(topBrush, new Rectangle(leftLeft, rightBottom + 1, _shadowGap, _shadowGap));
                        
														leftLeft += _shadowGap;
													}

													shadowBrush = new LinearGradientBrush(new Point(9999, rightBottom + 1), 
																						  new Point(9999, rightBottom + 1 + _shadowGap),
																						  darkColor, lightColor);
												}
												else
													shadowBrush = new SolidBrush(SystemColors.ControlDark);

												g.FillRectangle(shadowBrush, new Rectangle(leftLeft, rightBottom + 1, rightBottom - leftLeft - _shadowGap, _shadowGap));

												shadowBrush.Dispose();
											}
										}
                                    }
                                }
                                else
                                {
                                    using (Pen selectPen = new Pen(_highlightBackDark))
                                    {
                                        // Draw the selection area in white so can alpha draw over the top
                                        g.FillRectangle(Brushes.White, boxRect);

                                        using (SolidBrush selectBrush = new SolidBrush(_highlightBackLight))
                                        {
                                            // Draw the selection area
                                            g.FillRectangle(selectBrush, boxRect);

                                            // Draw a border around the selection area
                                            g.DrawRectangle(selectPen, boxRect);
                                        }
                                    }
                                }
                                break;
                            case VisualStyle.Plain:
                                if (_plainAsBlock)
                                {
                                    using (SolidBrush selectBrush = new SolidBrush(_highlightBackDark))
                                        g.FillRectangle(selectBrush, drawRect);
                                }
                                else
                                {
                                    if (_selected)
                                    {
                                        using(Pen lighlight = new Pen(ControlPaint.LightLight(this.BackColor)),
                                                  dark = new Pen(ControlPaint.DarkDark(this.BackColor)))
                                        {                                            
                                            g.DrawLine(dark, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
                                            g.DrawLine(dark, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
                                            g.DrawLine(lighlight, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                                            g.DrawLine(lighlight, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
                                        }
                                    }
                                    else
                                    {
                                        using(Pen lighlight = new Pen(ControlPaint.LightLight(this.BackColor)),
                                                  dark = new Pen(ControlPaint.DarkDark(this.BackColor)))
                                        {
                                            g.DrawLine(lighlight, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
                                            g.DrawLine(lighlight, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
                                            g.DrawLine(dark, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                                            g.DrawLine(dark, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }

                if (dc.Chevron)
                {
                    // Draw the chevron image in the centre of the text area
                    int yPos = drawRect.Top;
                    int xPos = drawRect.X + ((drawRect.Width - _chevronLength) / 2);

                    // When selected...
                    if (_selected)
                    {
                        // ...offset down and to the right
                        xPos += 1;
                        yPos += 1;
                    }

                    g.DrawImage(_menuImages.Images[_chevronIndex], xPos, yPos);
                }
                else
                {	
                    // Left align the text drawing on a single line centered vertically
                    // and process the & character to be shown as an underscore on next character
                    StringFormat format = new StringFormat();
                    format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    format.HotkeyPrefix = HotkeyPrefix.Show;

                    if (_direction == Direction.Vertical)
                        format.FormatFlags |= StringFormatFlags.DirectionVertical;

                    if (dc.Enabled && this.Enabled)
                    {
                        if (tracked && (_style == VisualStyle.Plain) && _plainAsBlock)
                        {
                            // Is the item selected as well as tracked?
                            if (_selected)
                            {
                                // Offset to show it is selected
                                textRect.X += 2;
                                textRect.Y += 2;
                            }

                            using (SolidBrush textBrush = new SolidBrush(_plainSelectedTextColor))
                                g.DrawString(mc.Text, this.Font, textBrush, textRect, format);
                        }
                        else
						{
                            if (_selected && tracked)
                            {
                                using (SolidBrush textBrush = new SolidBrush(_selectedTextColor))
                                    g.DrawString(mc.Text, this.Font, textBrush, textRect, format);
                            }
                            else
                            {
                                if (tracked)
                                {
									using (SolidBrush textBrush = new SolidBrush(_highlightTextColor))
										g.DrawString(mc.Text, this.Font, textBrush, textRect, format);
                                }
                                else
                                {
                                    using (SolidBrush textBrush = new SolidBrush(_textColor))
                                        g.DrawString(mc.Text, this.Font, textBrush, textRect, format);
                                }
                            }
						}
                    }
                    else 
                    {
                        // Helper values used when drawing grayed text in plain style
                        Rectangle rectDownRight = textRect;
                        rectDownRight.Offset(1,1);

                        // Draw the text offset down and right
                        g.DrawString(mc.Text, this.Font, Brushes.White, rectDownRight, format);

                        // Draw then text offset up and left
                        using (SolidBrush grayBrush = new SolidBrush(SystemColors.GrayText))
                            g.DrawString(mc.Text, this.Font, grayBrush, textRect, format);
                    }
                }
            }
        }

        protected void DrawAllCommands(Graphics g)
        {
            for(int i=0; i<_drawCommands.Count; i++)
            {
                // Grab some commonly used values				
                DrawCommand dc = _drawCommands[i] as DrawCommand;

                // Draw this command only
                DrawSingleCommand(g, dc, (i == _trackItem));
            }
        }

        protected int SwitchTrackingItem(int oldItem, int newItem)
        {
            // Create a graphics object for drawinh
            using(Graphics g = this.CreateGraphics())
            {
                // Deselect the old draw command
                if (oldItem != -1)
                {
                    DrawCommand dc = _drawCommands[oldItem] as DrawCommand;

                    // Draw old item not selected
                    DrawSingleCommand(g, _drawCommands[oldItem] as DrawCommand, false);

                    // Generate an unselect event
                    if (dc.MenuCommand != null)
                        OnDeselected(dc.MenuCommand);
                }

                _trackItem = newItem;

                // Select the new draw command
                if (_trackItem != -1)
                {
                    DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                    // Draw new item selected
                    DrawSingleCommand(g, _drawCommands[_trackItem] as DrawCommand, true);
					
                    // Generate an select event
                    if (dc.MenuCommand != null)
                        OnSelected(dc.MenuCommand);
                }

                // Force redraw of all items to prevent strange bug where some items
                // never get redrawn correctly and so leave blank spaces when using the
                // mouse/keyboard to shift between popup menus
                DrawAllCommands(g);
            }

            return _trackItem;
        }

        protected void RemoveItemTracking()
        {
            if (_trackItem != -1)
            {
                DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                // Generate an unselect event
                if (dc.MenuCommand != null)
                    OnDeselected(dc.MenuCommand);

                // Remove tracking value
                _trackItem = -1;
            }		
        }

        internal void OperateSubMenu(DrawCommand dc, bool selectFirst, bool trackRemove)
        {
            if (this.IsDisposed)
                return;
                
            Rectangle drawRect = dc.DrawRect;

            // Find screen positions for popup menu
            Point screenPos;
			
            if (_style == VisualStyle.IDE)
            {
                if (_direction == Direction.Horizontal)
                    screenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Bottom - _lengthGap - 2));
                else
                    screenPos = PointToScreen(new Point(dc.DrawRect.Right - _breadthGap, drawRect.Top + _boxExpandSides - 1));
            }
            else
            {
                if (_direction == Direction.Horizontal)
                    screenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Bottom));
                else
                    screenPos = PointToScreen(new Point(dc.DrawRect.Right, drawRect.Top));
            }

            Point aboveScreenPos;
			
            if (_style == VisualStyle.IDE)
            {
                if (_direction == Direction.Horizontal)
                    aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Top + _breadthGap + _lengthGap - 1));
                else
                    aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Right - _breadthGap, drawRect.Bottom + _lengthGap + 1));
            }
            else
            {
                if (_direction == Direction.Horizontal)
                    aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Top));
                else
                    aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Right, drawRect.Bottom));
            }

            int borderGap;

            // Calculate the missing gap in the PopupMenu border
            if (_direction == Direction.Horizontal)
                borderGap = dc.DrawRect.Width - _subMenuBorderAdjust;
            else
                borderGap = dc.DrawRect.Height - _subMenuBorderAdjust;		
	
            _popupMenu = new PopupMenu();

            // Define the correct visual style based on ours
            _popupMenu.Style = this.Style;

            // Key direction when keys cause dismissal
            int returnDir = 0;

            // Command selected by the PopupMenu
            MenuCommand returnCommand = null;

            // Should the PopupMenu tell the collection to remember expansion state
            _popupMenu.RememberExpansion = _rememberExpansion;

            // Propogate our highlight setting
            _popupMenu.HighlightInfrequent = _highlightInfrequent;

            // Might need to define custom colors
            if (!_defaultSelectedBackColor)
                _popupMenu.BackColor = _selectedBackColor;
            
            if (!_defaultSelectedTextColor)
                _popupMenu.TextColor = _selectedTextColor;

            if (!_defaultHighlightTextColor)
                _popupMenu.HighlightTextColor = _highlightTextColor;

            if (!_defaultHighlightBackColor)
                _popupMenu.HighlightColor = _highlightBackColor;

			if (!_defaultFont)
				_popupMenu.Font = base.Font;

            // Pass on the animation values
            _popupMenu.Animate = _animate;
            _popupMenu.AnimateStyle = _animateStyle;
            _popupMenu.AnimateTime = _animateTime;
    
            if (dc.Chevron)
            {
                MenuCommandCollection mcc = new MenuCommandCollection();

                bool addCommands = false;

                // Generate a collection of menu commands for those not visible
                foreach(MenuCommand command in _menuCommands)
                {
                    if (!addCommands && (command == _chevronStartCommand))
                        addCommands = true;

                    if (addCommands)
                        mcc.Add(command);
                }

                // Track the popup using provided menu item collection
                returnCommand = _popupMenu.TrackPopup(screenPos, 
                                                      aboveScreenPos,
                                                      _direction,
                                                      mcc, 
                                                      borderGap,
                                                      selectFirst, 
                                                      this,
                                                      _animateFirst,
                                                      ref returnDir);
            }
            else
            {
                // Generate event so that caller has chance to modify MenuCommand contents
                dc.MenuCommand.OnPopupStart();
                
                // Honour the collections request for showing infrequent items
                _popupMenu.ShowInfrequent = dc.MenuCommand.MenuCommands.ShowInfrequent;

                // Track the popup using provided menu item collection
                returnCommand = _popupMenu.TrackPopup(screenPos, 
                                                      aboveScreenPos,
                                                      _direction,
                                                      dc.MenuCommand.MenuCommands, 
                                                      borderGap,
                                                      selectFirst,
                                                      this,
                                                      _animateFirst,
                                                      ref returnDir);
            }
            
            // No more animation till simulation ends
            _animateFirst = false;

            // If we are supposed to expand all items at the same time
            if (_expandAllTogether)
            {   
                // Is anything we have shown now in the expanded state
                if (AnythingExpanded(_menuCommands))
                {
                    // Set everything to expanded
                    SetAllCommandsExpansion(_menuCommands, true);
                }
            }
            
            // Was arrow key not used to dismiss the submenu?
            if (returnDir == 0)
            {
                // The submenu may have eaten the mouse leave event
                _mouseOver = false;

                // Only if the submenu was dismissed at the request of the submenu
                // should the selection mode be cancelled, otherwise keep selection mode
                if (!_dismissTransfer)
                {
                    // This item is no longer selected
                    Deselect();
                    _drawUpwards = false;

                    if (!this.IsDisposed)
                    {
                        // Should we stop tracking this item
                        if (trackRemove)
                        {
			                // Unselect the current item
                            _trackItem = SwitchTrackingItem(_trackItem, -1);
                        }
                        else
                        {
                            if (_trackItem != -1)
                            {
                                // Repaint the item
                                DrawCommand(_trackItem, true);
                            }
                        }
                    }
                }
                else
                {
                    // Do not change _selected status
                    _dismissTransfer = false;
                }
            }

            if (!dc.Chevron)
            {
                // Generate event so that caller has chance to modify MenuCommand contents
                dc.MenuCommand.OnPopupEnd();
            }

            // Spin the message loop so the messages dealing with destroying
            // the PopupMenu window are processed and cause it to disappear from
            // view before events are generated
            Application.DoEvents();

            // Remove unwanted object
            _popupMenu = null;

            // Was arrow key used to dismiss the submenu?
            if (returnDir != 0)
            {
                if (returnDir < 0)
                {
                    // Shift selection left one
                    ProcessMoveLeft(true);
                }
                else
                {
                    // Shift selection right one
                    ProcessMoveRight(true);
                }

                // A WM_MOUSEMOVE is generated when we open up the new submenu for 
                // display, ignore this as it causes the selection to move
                _ignoreMouseMove = true;
            }
            else
            {
                // Was a MenuCommand returned?
                if (returnCommand != null)
                {
					// Remove 

					// Are we simulating having the focus?
					if (_manualFocus)
					{
						// Always return focus to original when a selection is made
						SimulateReturnFocus();
					}

                    // Pulse the selected event for the command
                    returnCommand.OnClick(EventArgs.Empty);
                }
            }
        }

        protected bool AnythingExpanded(MenuCommandCollection mcc)
        {
            foreach(MenuCommand mc in mcc)
            {
                if (mc.MenuCommands.ShowInfrequent)
                    return true;
                    
                if (AnythingExpanded(mc.MenuCommands))
                    return true;
            }
            
            return false;
        }
        
        protected void SetAllCommandsExpansion(MenuCommandCollection mcc, bool show)
        {
            foreach(MenuCommand mc in mcc)
            {
                // Set the correct value for this collection
                mc.MenuCommands.ShowInfrequent = show;
                    
                // Recurse into all lower level collections
                SetAllCommandsExpansion(mc.MenuCommands, show);
            }
        }

        protected void SimulateGrabFocus()
        {	
			if (!_manualFocus)
			{
				_manualFocus = true;
				_animateFirst = true;

				Form parentForm = this.FindForm();

				// Want notification when user selects a different Form
				parentForm.Deactivate += new EventHandler(OnParentDeactivate);

				// Must hide caret so user thinks focus has changed
				bool hideCaret = User32.HideCaret(IntPtr.Zero);

				// Create an object for storing windows message information
				Win32.MSG msg = new Win32.MSG();

				_exitLoop = false;

				// Process messages until exit condition recognised
				while(!_exitLoop)
				{
					// Suspend thread until a windows message has arrived
					if (User32.WaitMessage())
					{
						// Take a peek at the message details without removing from queue
						while(!_exitLoop && User32.PeekMessage(ref msg, 0, 0, 0, (int)Win32.PeekMessageFlags.PM_NOREMOVE))
						{
//							Console.WriteLine("Loop {0} {1}", this.Handle, ((Win32.Msgs)msg.message).ToString());
    
	                        if (User32.GetMessage(ref msg, 0, 0, 0))
                            {
								// Should this method be dispatched?
								if (!ProcessInterceptedMessage(ref msg))
								{
									User32.TranslateMessage(ref msg);
									User32.DispatchMessage(ref msg);
								}
                            }
						}
					}
				}

				// Remove notification when user selects a different Form
				parentForm.Deactivate -= new EventHandler(OnParentDeactivate);

				// If caret was hidden then show it again now
				if (hideCaret)
					User32.ShowCaret(IntPtr.Zero);

				// We lost the focus
				_manualFocus = false;
			}
        }

        protected void SimulateReturnFocus()
        {
			if (_manualFocus)
				_exitLoop = true;

			// Remove any item being tracked
			if (_trackItem != -1)
			{
				// Unselect the current item
				_trackItem = SwitchTrackingItem(_trackItem, -1);
			}
        }

		protected void OnParentDeactivate(object sender, EventArgs e)
		{
			SimulateReturnFocus();
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawAllCommands(e.Graphics);
            base.OnPaint(e);
        }

        protected void ProcessMoveLeft(bool select)
        {
            if (_popupMenu == null)
            {
                int newItem = _trackItem;
                int startItem = newItem;

                for(int i=0; i<_drawCommands.Count; i++)
                {
                    // Move to previous item
                    newItem--;

                    // Have we looped all the way around all choices
                    if (newItem == startItem)
                        return;

                    // Check limits
                    if (newItem < 0)
                        newItem = _drawCommands.Count - 1;

                    DrawCommand dc = _drawCommands[newItem] as DrawCommand;

                    // Can we select this item?
                    if (!dc.Separator && (dc.Chevron || dc.MenuCommand.Enabled))
                    {
                        // If a change has occured
                        if (newItem != _trackItem)
                        {
                            // Modify the display of the two items 
                            _trackItem = SwitchTrackingItem(_trackItem, newItem);
							
                            if (_selected)
                            {
                                if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
                                    User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
                            }

                            break;
                        }
                    }
                }
            }
        }

        protected void ProcessMoveRight(bool select)
        {
            if (_popupMenu == null)
            {
                int newItem = _trackItem;
                int startItem = newItem;

                for(int i=0; i<_drawCommands.Count; i++)
                {
                    // Move to previous item
                    newItem++;

                    // Check limits
                    if (newItem >= _drawCommands.Count)
                        newItem = 0;

                    DrawCommand dc = _drawCommands[newItem] as DrawCommand;

                    // Can we select this item?
                    if (!dc.Separator && (dc.Chevron || dc.MenuCommand.Enabled))
                    {
                        // If a change has occured
                        if (newItem != _trackItem)
                        {
                            // Modify the display of the two items 
                            _trackItem = SwitchTrackingItem(_trackItem, newItem);

                            if (_selected)
                            {
                                if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
                                    User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
                            }

                            break;
                        }
                    }
                }
            }
        }

		protected void ProcessEnter()
		{
            if (_popupMenu == null)
            {
				// Are we tracking an item?
				if (_trackItem != -1)
				{
					// The item must not already be selected
					if (!_selected)
					{
						DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

						// Is there a submenu to show?
						if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count >= 0))
						{
							// Select the tracked item
							_selected = true;
							_drawUpwards = false;
										
							// Update display to show as selected
							DrawCommand(_trackItem, true);

							// Show the submenu

							if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
								User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
						}
						else
						{
                            // No, pulse the Click event for the command
                            dc.MenuCommand.OnClick(EventArgs.Empty);

							int item = _trackItem;

							// Not tracking anymore
							RemoveItemTracking();

							// Update display to show as not selected
							DrawCommand(item, false);

							// Finished, so return focus to origin
							SimulateReturnFocus();
						}
					}
					else
					{
                        // Must be showing a submenu less item as selected
						DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

						// Pulse the event
                        dc.MenuCommand.OnClick(EventArgs.Empty);

						int item = _trackItem;

						RemoveItemTracking();

						// Not selected anymore
                        Deselect();
                        
						// Update display to show as not selected
						DrawCommand(item, false);

						// Finished, so return focus to origin
						SimulateReturnFocus();
					}
				}
			}
		}

        protected void ProcessMoveDown()
        {
            if (_popupMenu == null)
            {
                // Are we tracking an item?
                if (_trackItem != -1)
                {
                    // The item must not already be selected
                    if (!_selected)
                    {
                        DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                        // Is there a submenu to show?
                        if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count >= 0))
                        {
                            // Select the tracked item
                            _selected = true;
                            _drawUpwards = false;
										
                            // Update display to show as selected
                            DrawCommand(_trackItem, true);

                            // Show the submenu
	                        if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
		                        User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
                        }
                    }
                }
            }
        }

        protected bool ProcessMnemonicKey(char key)
        {
            // No current selection
            if (!_selected)
            {
                // Search for an item that matches
                for(int i=0; i<_drawCommands.Count; i++)
                {
                    DrawCommand dc = _drawCommands[i] as DrawCommand;

                    // Only interested in enabled items
                    if ((dc.MenuCommand != null) && dc.MenuCommand.Enabled && dc.MenuCommand.Visible)
                    {
                        // Does the character match?
                        if (key == dc.Mnemonic)
                        {
                            // Select the tracked item
                            _selected = true;
                            _drawUpwards = false;
										
                            // Is there a change in tracking?
                            if (_trackItem != i)
                            {
                                // Modify the display of the two items 
                                _trackItem = SwitchTrackingItem(_trackItem, i);
                            }
                            else
                            {
                                // Update display to show as selected
                                DrawCommand(_trackItem, true);
                            }

                            // Is there a submenu to show?
                            if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count >= 0))
							{
	                            if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
		                            User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);

	                            return true;
							}
                            else
                            {			
                                // No, pulse the Click event for the command
                                dc.MenuCommand.OnClick(EventArgs.Empty);
	
								int item = _trackItem;

								RemoveItemTracking();

								// No longer seleted
                                Deselect();
                                
                                // Update display to show as not selected
                                DrawCommand(item, false);

								// Finished, so return focus to origin
								SimulateReturnFocus();

								return false;
                            }
                        }
                    }
                }
            }

            return false;
        }

		protected void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			// Are we using the default menu or a user defined value?
			if (_defaultFont)
			{
				DefineFont(SystemInformation.MenuFont);

				Recalculate();
				Invalidate();
			}
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			if (_defaultBackColor)
				this.BackColor = SystemColors.Control;
			
            if (_defaultHighlightBackColor)
                this.HighlightBackColor = SystemColors.Highlight;

            if (_defaultSelectedBackColor)
                this.SelectedBackColor = SystemColors.Control;
            
            if (_defaultTextColor)
			    _textColor = SystemColors.MenuText;

            if (_defaultHighlightTextColor)
                _highlightTextColor = SystemColors.MenuText;

            if (_defaultSelectedTextColor)
                _selectedTextColor = SystemColors.MenuText;
            
            Recalculate();
            Invalidate();
    
            base.OnSystemColorsChanged(e);
		}
        
		public bool PreFilterMessage(ref Message msg)
        {
            Form parentForm = this.FindForm();

            // Only interested if the Form we are on is activate (i.e. contains focus)
            if ((parentForm != null) && (parentForm == Form.ActiveForm) && parentForm.ContainsFocus)
            {		
                switch(msg.Msg)
                {
                    case (int)Win32.Msgs.WM_KEYDOWN:
                        // Ignore keyboard input if the control is disabled
                        if (this.Enabled)
						{
                            // Find up/down state of shift and control keys
                            ushort shiftKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_SHIFT);
                            ushort controlKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_CONTROL);

                            // Basic code we are looking for is the key pressed...
                            int code = (int)msg.WParam;

                            // ...plus the modifier for SHIFT...
                            if (((int)shiftKey & 0x00008000) != 0)
                                code += 0x00010000;

                            // ...plus the modifier for CONTROL
                            if (((int)controlKey & 0x00008000) != 0)
                                code += 0x00020000;

							// Construct shortcut from keystate and keychar
							Shortcut sc = (Shortcut)(code);

							// Search for a matching command
							return GenerateShortcut(sc, _menuCommands);
                        }
                        break;
                    case (int)Win32.Msgs.WM_SYSKEYUP:
                        // Ignore keyboard input if the control is disabled
                        if (this.Enabled)
                        {
                            if ((int)msg.WParam == (int)Win32.VirtualKeys.VK_MENU)
                            {
                                // Are there any menu commands?
                                if (_drawCommands.Count > 0)
                                {
                                    // If no item is currently tracked then...
                                    if (_trackItem == -1)
                                    {
                                        // ...start tracking the first valid command
                                        for(int i=0; i<_drawCommands.Count; i++)
                                        {
                                            DrawCommand dc = _drawCommands[i] as DrawCommand;
											
                                            if (!dc.Separator && (dc.Chevron || dc.MenuCommand.Enabled))
                                            {
                                                _trackItem = SwitchTrackingItem(-1, i);
                                                break;
                                            }
                                        }
                                    }
											
                                    // Grab the focus for key events						
                                    SimulateGrabFocus();							
                                }

                                return true;
                            }
                        }
                        break;
                    case (int)Win32.Msgs.WM_SYSKEYDOWN:
                        // Ignore keyboard input if the control is disabled
                        if (this.Enabled)
                        {
                            if ((int)msg.WParam != (int)Win32.VirtualKeys.VK_MENU)
                            {
                                // Construct shortcut from ALT + keychar
                                Shortcut sc = (Shortcut)(0x00040000 + (int)msg.WParam);
		
                                if (GenerateShortcut(sc, _menuCommands))
                                    return true;
								
                                // Last resort is treat as a potential mnemonic
                                if (ProcessMnemonicKey((char)msg.WParam))
                                {
									if (!_manualFocus)
										SimulateGrabFocus();

                                    return true;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

		protected bool ProcessInterceptedMessage(ref Win32.MSG msg)
		{
			bool eat = false;
        
			switch(msg.message)
			{
				case (int)Win32.Msgs.WM_LBUTTONDOWN:
				case (int)Win32.Msgs.WM_MBUTTONDOWN:
				case (int)Win32.Msgs.WM_RBUTTONDOWN:
				case (int)Win32.Msgs.WM_XBUTTONDOWN:
				case (int)Win32.Msgs.WM_NCLBUTTONDOWN:
				case (int)Win32.Msgs.WM_NCMBUTTONDOWN:
				case (int)Win32.Msgs.WM_NCRBUTTONDOWN:
					// Mouse clicks cause the end of simulated focus unless they are
					// inside the client area of the menu control itself
					Point pt = new Point( (int)((uint)msg.lParam & 0x0000FFFFU), 
										  (int)(((uint)msg.lParam & 0xFFFF0000U) >> 16));
				
					if (!this.ClientRectangle.Contains(pt))	
						SimulateReturnFocus();
					break;
				case (int)Win32.Msgs.WM_KEYDOWN:
					// Find up/down state of shift and control keys
					ushort shiftKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_SHIFT);
					ushort controlKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_CONTROL);

					// Basic code we are looking for is the key pressed...
					int basecode = (int)msg.wParam;
					int code = basecode;

					// ...plus the modifier for SHIFT...
					if (((int)shiftKey & 0x00008000) != 0)
						code += 0x00010000;

					// ...plus the modifier for CONTROL
					if (((int)controlKey & 0x00008000) != 0)
						code += 0x00020000;

					if (code == (int)Win32.VirtualKeys.VK_ESCAPE)
					{
						// Is an item being tracked
						if (_trackItem != -1)
						{
							// Is it also showing a submenu
							if (_popupMenu == null)
							{
								// Unselect the current item
								_trackItem = SwitchTrackingItem(_trackItem, -1);

							}
						}

						SimulateReturnFocus();

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_LEFT)
					{
						if (_direction == Direction.Horizontal)
							ProcessMoveLeft(false);

						if (_selected)
							_ignoreMouseMove = true;

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_RIGHT)
					{
						if (_direction == Direction.Horizontal)
							ProcessMoveRight(false);
						else
							ProcessMoveDown();

						if (_selected)
							_ignoreMouseMove = true;

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_RETURN)
					{
						ProcessEnter();

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_DOWN)
					{
						if (_direction == Direction.Horizontal)
							ProcessMoveDown();
						else
							ProcessMoveRight(false);

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_UP)
					{
						ProcessMoveLeft(false);

						// Prevent intended destination getting message
						eat = true;
					}
					else
					{
						// Construct shortcut from keystate and keychar
						Shortcut sc = (Shortcut)(code);

						// Search for a matching command
						if (!GenerateShortcut(sc, _menuCommands))
						{
							// Last resort is treat as a potential mnemonic
							ProcessMnemonicKey((char)msg.wParam);

							if (_selected)
								_ignoreMouseMove = true;
						}
						else
						{
							SimulateReturnFocus();
						}

						// Always eat keyboard message in simulated focus
						eat = true;
					}
					break;
				case (int)Win32.Msgs.WM_KEYUP:
					eat = true;
					break;
				case (int)Win32.Msgs.WM_SYSKEYUP:
					// Ignore keyboard input if the control is disabled
					if ((int)msg.wParam == (int)Win32.VirtualKeys.VK_MENU)
					{
						if (_trackItem != -1)
						{
							// Is it also showing a submenu
							if (_popupMenu == null)
							{
								// Unselect the current item
								_trackItem = SwitchTrackingItem(_trackItem, -1);

							}
						}

						SimulateReturnFocus();

						// Always eat keyboard message in simulated focus
						eat = true;
					}
					break;
				case (int)Win32.Msgs.WM_SYSKEYDOWN:
					if ((int)msg.wParam != (int)Win32.VirtualKeys.VK_MENU)
					{
						// Construct shortcut from ALT + keychar
						Shortcut sc = (Shortcut)(0x00040000 + (int)msg.wParam);

						// Search for a matching command
						if (!GenerateShortcut(sc, _menuCommands))
						{
							// Last resort is treat as a potential mnemonic
							ProcessMnemonicKey((char)msg.wParam);

							if (_selected)
								_ignoreMouseMove = true;
						}
						else
						{
							SimulateReturnFocus();
						}

						// Always eat keyboard message in simulated focus
						eat = true;
					}
					break;
				default:
					break;
			}

			return eat;
		}

        protected bool GenerateShortcut(Shortcut sc, MenuCommandCollection mcc)
        {
            foreach(MenuCommand mc in mcc)
            {
                // Does the command match?
                if (mc.Enabled && (mc.Shortcut == sc))
                {
                    // Generate event for command
                    mc.OnClick(EventArgs.Empty);

                    return true;
                }
                else
                {
                    // Any child items to test?
                    if (mc.MenuCommands.Count > 0)
                    {
                        // Recursive descent of all collections
                        if (GenerateShortcut(sc, mc.MenuCommands))
                            return true;
                    }
                }
            }

            return false;
        }

        protected void OnWM_OPERATEMENU(ref Message m)
        {
            // Is there a valid item being tracted?
            if (_trackItem != -1)
            {
                DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                OperateSubMenu(dc, (m.LParam != IntPtr.Zero), (m.WParam != IntPtr.Zero));
            }
        }

        protected void OnWM_GETDLGCODE(ref Message m)
        {
            // We want to the Form to provide all keyboard input to us
            m.Result = (IntPtr)Win32.DialogCodes.DLGC_WANTALLKEYS;
        }

        protected override void WndProc(ref Message m)
        {
            // WM_OPERATEMENU is not a constant and so cannot be in a switch
            if (m.Msg == WM_OPERATEMENU)
                OnWM_OPERATEMENU(ref m);
            else
            {
                switch(m.Msg)
                {
                    case (int)Win32.Msgs.WM_GETDLGCODE:
                        OnWM_GETDLGCODE(ref m);
                        return;
                }
            }

            base.WndProc(ref m);
        }
    }
}
