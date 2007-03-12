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
using System.Drawing.Text;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Menus
{
    [ToolboxItem(false)]
    [DefaultProperty("MenuCommands")]
    public class PopupMenu : NativeWindow
    {
        // Enumeration of Indexes into positioning constants array
        protected enum PI
        {
            BorderTop		= 0,
            BorderLeft		= 1,
            BorderBottom	= 2, 
            BorderRight		= 3,
            ImageGapTop		= 4,
            ImageGapLeft	= 5,
            ImageGapBottom	= 6,
            ImageGapRight	= 7,
            TextGapLeft		= 8,
            TextGapRight	= 9,
            SubMenuGapLeft	= 10,
            SubMenuWidth	= 11,
            SubMenuGapRight	= 12,
            SeparatorHeight	= 13,
            SeparatorWidth	= 14,
            ShortcutGap		= 15,
            ShadowWidth		= 16,
            ShadowHeight	= 17,
            ExtraWidthGap	= 18,
            ExtraHeightGap	= 19,
            ExtraRightGap	= 20,
            ExtraReduce		= 21
        }

        // Class constants for sizing/positioning each style
        protected static readonly int[,] _position = { 
                                                        {2, 1, 0, 1, 2, 3, 3, 5, 4, 4, 2, 6, 5, 5, 1, 10, 4, 4, 2, 2, 0, 0},	// IDE
                                                        {1, 0, 1, 2, 2, 1, 3, 4, 3, 3, 2, 8, 5, 5, 5, 10, 0, 0, 2, 2, 2, 5}		// Plain
                                                     };
        // Other class constants
        protected static int _selectionDelay = 400;
        protected static int _expansionDelay = 1100;
        protected static int _imageWidth = 16;
        protected static int _imageHeight = 16;
        protected static int _shadowLength = 4;
        protected static int _shadowHalf = 2;
        protected static int _blendSteps = 6;
        protected static Bitmap _shadowCache = null;
        protected static int _shadowCacheWidth = 0;
        protected static int _shadowCacheHeight = 0;
		
        // Class fields
        protected static ImageList _menuImages = null;
        protected static bool _supportsLayered = false;
		
        // Indexes into the menu images strip
        protected enum ImageIndex
        {
            Check			= 0,
            Radio			= 1,
            SubMenu			= 2,
            CheckSelected	= 3,
            RadioSelected	= 4,
            SubMenuSelected	= 5,
            Expansion		= 6,
            ImageError		= 7
        }

        // Operation of DrawShadowHorizontal
        protected enum Shadow
        {
            Left,
            Right,
            All
        }

        // Class constants that are marked as 'readonly' are allowed computed initialization
        protected readonly int WM_DISMISS = (int)Win32.Msgs.WM_USER + 1;
        protected readonly int WM_OPERATE_SUBMENU = (int)Win32.Msgs.WM_USER + 2;

        // Instance fields
        protected Timer _timer;
        protected Font _textFont;
        protected int _popupItem;
        protected int _trackItem;
        protected int _borderGap;
        protected int _returnDir;
        protected int _extraSize;
        protected int _excludeOffset;
		protected int _animateTime;
		protected bool _animateFirst;
		protected bool _animateIn;
        protected bool _layered;
        protected bool _exitLoop;
        protected bool _mouseOver;
        protected bool _popupDown;
        protected bool _popupRight;
        protected bool _excludeTop;
        protected bool _showInfrequent;
        protected bool _rememberExpansion;
        protected bool _highlightInfrequent;
        protected Color _backColor;
        protected Color _textColor;
        protected Color _highlightTextColor;
        protected Color _highlightColor;
        protected Color _highlightColorDark;
        protected Color _highlightColorLight;
        protected Color _highlightColorLightLight;
        protected Color _controlLL;
        protected Color _controlLLight;
        protected Size _currentSize;
        protected VisualStyle _style;
        protected Point _screenPos;
        protected Point _lastMousePos;
        protected Point _currentPoint;
        protected Point _leftScreenPos;
        protected Point _aboveScreenPos;
        protected Direction _direction;
        protected PopupMenu _parentMenu;
        protected PopupMenu _childMenu;
        protected SolidBrush _controlLBrush;
        protected SolidBrush _controlEBrush;
        protected SolidBrush _controlLLBrush;
        protected Animate _animate;
        protected Animate _animateTrack;
        protected Animation _animateStyle;
        protected ArrayList _drawCommands;
        protected MenuControl _parentControl;
        protected MenuCommand _returnCommand;
        protected MenuCommandCollection _menuCommands;

        // Instance fields - events
        public event CommandHandler Selected;
        public event CommandHandler Deselected;

        static PopupMenu()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _menuImages = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.Magic.Menus.PopupMenu"),
                                                         "Crownwood.Magic.Resources.ImagesPopupMenu.bmp",
                                                         new Size(16,16),
                                                         new Point(0,0));

            // We need to know if the OS supports layered windows
            _supportsLayered = (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null);
        }

        public PopupMenu()
        {
            // Create collection objects
            _drawCommands = new ArrayList();
            _menuCommands = new MenuCommandCollection(); 

            // Default the properties
            _returnDir = 0;
            _extraSize = 0;
            _popupItem = -1;
            _trackItem = -1;
            _childMenu = null;
            _exitLoop = false;
            _popupDown = true;
            _mouseOver = false;
            _excludeTop = true;
            _popupRight = true;
            _parentMenu = null;
            _excludeOffset = 0;
            _parentControl = null;
            _returnCommand = null;
            _controlLBrush = null;
            _controlEBrush = null;
            _controlLLBrush = null;
            _highlightInfrequent = false;
            _showInfrequent = false;
            _style = VisualStyle.IDE;
            _rememberExpansion = true;
            _lastMousePos = new Point(-1,-1);
            _direction = Direction.Horizontal;
            _textFont = SystemInformation.MenuFont;

			// Animation details
            _animateTime = 100;
            _animate = Animate.System;
			_animateStyle = Animation.System;
			_animateFirst = true;
			_animateIn = true;

            // Create and initialise the timer object (but do not start it running!)
            _timer = new Timer();
            _timer.Interval = _selectionDelay;
            _timer.Tick += new EventHandler(OnTimerExpire);

            // Define default colors
            _textColor = SystemColors.MenuText;
            _highlightTextColor = SystemColors.HighlightText;
            DefineHighlightColors(SystemColors.Highlight);
            DefineColors(SystemColors.Control);
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MenuCommandCollection MenuCommands
        {
            get { return _menuCommands; }
        } 

        [Category("Appearance")]
        [DefaultValue(typeof(VisualStyle), "IDE")]
        public VisualStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        [Category("Appearance")]
        public Font Font
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        [Category("Behaviour")]
        [DefaultValue(false)]
        public bool ShowInfrequent
        {
            get { return _showInfrequent; }
            set { _showInfrequent = value; }
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
        public bool HighlightInfrequent
        {
            get { return _highlightInfrequent; }
            set { _highlightInfrequent = value; }
        }

        [Category("Behaviour")]
        public Color BackColor
        {
            get { return _backColor; }
            set { DefineColors(value); }
        }
       
        [Category("Behaviour")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        [Category("Behaviour")]
        public Color HighlightTextColor
        {
            get { return _highlightTextColor; }
            set { _highlightTextColor = value; }
        }

        [Category("Behaviour")]
        public Color HighlightColor
        {
            get { return _highlightColor; }
            set { DefineHighlightColors(value); }
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

        public MenuCommand TrackPopup(Point screenPos)
        {
            return TrackPopup(screenPos, false);
        }

        public MenuCommand TrackPopup(Point screenPos, bool selectFirst)
        {
            // No point in showing PopupMenu if there are no entries
            if (_menuCommands.VisibleItems())
            {
                // Default the drawing direction
                _direction = Direction.Horizontal;

                // Remember screen positions
                _screenPos = screenPos;
                _aboveScreenPos = screenPos;
                _leftScreenPos = screenPos;

                return InternalTrackPopup(selectFirst);
            }

            return null;
        }

        internal MenuCommand TrackPopup(Point screenPos, 
                                        Point aboveScreenPos, 
                                        Direction direction,
                                        MenuCommandCollection menuCollection, 
                                        int borderGap, 
                                        bool selectFirst, 
                                        MenuControl parentControl,
                                        bool animateIn,
                                        ref int returnDir)
        {
            // Remember which direction the MenuControl is drawing in
            _direction = direction;

            // Remember the MenuControl that initiated us
            _parentControl = parentControl;

            // Remember the gap in drawing the top border
            _borderGap = borderGap;

            // Is this the first time a menu at this level has been animated
            _animateIn = animateIn;

            // Remember any currect menu item collection
            MenuCommandCollection oldCollection = _menuCommands;

            // Use the passed in collection of menu commands
            _menuCommands = menuCollection;

            // Remember screen positions
            _screenPos = screenPos;
            _aboveScreenPos = aboveScreenPos;
            _leftScreenPos = screenPos;

            MenuCommand ret = InternalTrackPopup(selectFirst);

            // Restore to original collection
            _menuCommands = oldCollection;

            // Remove reference no longer required
            _parentControl = null;

            // Return the direction key that caused dismissal
            returnDir = _returnDir;

            return ret;
        }

        protected MenuCommand InternalTrackPopup(Point screenPosTR, 
                                                 Point screenPosTL, 
                                                 MenuCommandCollection menuCollection, 
                                                 PopupMenu parentMenu, 
                                                 bool selectFirst, 
                                                 MenuControl parentControl, 
                                                 bool popupRight,
                                                 bool popupDown, 
                                                 bool animateIn,
                                                 ref int returnDir)
        {
            // Default the drawing direction
            _direction = Direction.Horizontal;

            // Remember the MenuControl that initiated us
            _parentControl = parentControl;

            // We have a parent popup menu that should be consulted about operation
            _parentMenu = parentMenu;
            
            // Is this the first time a menu at this level has been animated
            _animateIn = animateIn;

            // Remember any currect menu item collection
            MenuCommandCollection oldCollection = _menuCommands;

            // Use the passed in collection of menu commands
            _menuCommands = menuCollection;

            // Remember screen positions
            _screenPos = screenPosTR;
            _aboveScreenPos = screenPosTR;
            _leftScreenPos = screenPosTL;

            // Remember display directions
            _popupRight = popupRight;
            _popupDown = popupDown;

            MenuCommand ret = InternalTrackPopup(selectFirst);

            // Restore to original collection
            _menuCommands = oldCollection;

            // Remove references no longer required
            _parentControl = null;
            _parentMenu = null;

            // Return the direction key that caused dismissal
            returnDir = _returnDir;

            return ret;
        }

        protected MenuCommand InternalTrackPopup(bool selectFirst)
        {
            // MenuCommand to return as method result
            _returnCommand = null;

            // No item is being tracked
            _trackItem = -1;

            // Flag to indicate when to exit the message loop
            _exitLoop = false;
			
            // Assume the mouse does not start over our window
            _mouseOver = false;

            // Direction of key press if this caused dismissal
            _returnDir = 0;

            // Flag to indicate if the message should be dispatched
            bool leaveMsg = false;

            // First time a submenu is shown we pass in our value
            _animateFirst = true;

            // Create and show the popup window (without taking the focus)
            CreateAndShowWindow();
		
            // Create an object for storing windows message information
            Win32.MSG msg = new Win32.MSG();

            // Pretend user pressed key down to get the first valid item selected
            if (selectFirst)
                ProcessKeyDown();

            // Always use the arrow cursor
            User32.SetCursor(User32.LoadCursor(IntPtr.Zero, (uint)Win32.Cursors.IDC_ARROW));

            // Must hide caret so user thinks focus has changed
            bool hideCaret = User32.HideCaret(IntPtr.Zero);

            // Process messages until exit condition recognised
            while(!_exitLoop)
            {
                // Suspend thread until a windows message has arrived
                if (User32.WaitMessage())
                {
                    // Take a peek at the message details without removing from queue
                    while(!_exitLoop && User32.PeekMessage(ref msg, 0, 0, 0, (int)Win32.PeekMessageFlags.PM_NOREMOVE))
                    {
//						Console.WriteLine("Track {0} {1}", this.Handle, ((Win32.Msgs)msg.message).ToString());

                        bool eatMessage = false;

						int localWidth = _currentSize.Width - _position[(int)_style, (int)PI.ShadowWidth];
						int localHeight = _currentSize.Height - _position[(int)_style, (int)PI.ShadowHeight];

                        // Mouse was pressed in a window of this application
                        if ((msg.message == (int)Win32.Msgs.WM_LBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_MBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_RBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_XBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_NCLBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_NCMBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_NCRBUTTONUP) ||
                            (msg.message == (int)Win32.Msgs.WM_NCXBUTTONUP))
                        {
                            Win32.POINT screenPos = MousePositionToScreen(msg);

                            // Is the POINT inside the Popup window rectangle
                            if ((screenPos.x >= _currentPoint.X) && (screenPos.x <= (_currentPoint.X + localWidth)) &&
                                (screenPos.y >= _currentPoint.Y) && (screenPos.y <= (_currentPoint.Y + localHeight)))
                            {
                                OnWM_YBUTTONUP(screenPos.x, screenPos.y);

                                // Eat the message to prevent the intended destination getting it
                                eatMessage = true;								
                            }
                            else
                            {
                                PopupMenu target = ParentPopupMenuWantsMouseMessage(screenPos, ref msg);
                            
                                // Let the parent chain of PopupMenu's decide if they want it
                                if (target != null)
                                {
                                    target.OnWM_YBUTTONUP(screenPos.x, screenPos.y);
                                
                                    // Eat the message to prevent the intended destination getting it
                                    eatMessage = true;								
                                }
                            }
                        }
                                          
                        // Mouse was pressed in a window of this application
                        if ((msg.message == (int)Win32.Msgs.WM_LBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_MBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_RBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_XBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_NCLBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_NCMBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_NCRBUTTONDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_NCXBUTTONDOWN))
                        {
                            Win32.POINT screenPos = MousePositionToScreen(msg);

                            // Is the POINT inside the Popup window rectangle
                            if ((screenPos.x >= _currentPoint.X) && (screenPos.x <= (_currentPoint.X + localWidth)) &&
                                (screenPos.y >= _currentPoint.Y) && (screenPos.y <= (_currentPoint.Y + localHeight)))
                            {
                                // Eat the message to prevent the intended destination getting it
                                eatMessage = true;								
                            }
                            else
                            {
                                // Let the parent chain of PopupMenu's decide if they want it
                                if (ParentPopupMenuWantsMouseMessage(screenPos, ref msg) == null)
                                {
                                    if (ParentControlWantsMouseMessage(screenPos, ref msg))
                                    {
                                        // Let the MenuControl do its business
                                        _parentControl.OnWM_MOUSEDOWN(screenPos);

                                        // Eat the message to prevent the intended destination getting it
                                        eatMessage = true;								
                                    }
                                    else
                                    {
                                        // No, then we need to exit the popup menu tracking
                                        _exitLoop = true;

                                        // DO NOT process the message, leave it on the queue
                                        // and let the real destination window handle it.
                                        leaveMsg = true;

                                        // Is a parent control specified?
                                        if (_parentControl != null)
                                        {
                                            // Is the mouse event destination the parent control?
                                            if (msg.hwnd == _parentControl.Handle)
                                            {
                                                // Then we want to consume the message so it does not get processed 
                                                // by the parent control. Otherwise, pressing down will cause this 
                                                // popup to disappear but the message will then get processed by 
                                                // the parent and cause a popup to reappear again. When we actually
                                                // want the popup to disappear and nothing more.
                                                leaveMsg = false;
                                            }
                                        }
                                    }
                                }
                                else
                                {                                   
                                    // Eat the message to prevent the intended destination getting it
                                    eatMessage = true;								
                                }
                            }
                        }

                        // Mouse move occured
                        if (msg.message == (int)Win32.Msgs.WM_MOUSEMOVE) 
                        {
                            Win32.POINT screenPos = MousePositionToScreen(msg);

                            // Is the POINT inside the Popup window rectangle
                            if ((screenPos.x >= _currentPoint.X) && (screenPos.x <= (_currentPoint.X + localWidth)) &&
                                (screenPos.y >= _currentPoint.Y) && (screenPos.y <= (_currentPoint.Y + localHeight)))
                            {
                                OnWM_MOUSEMOVE(screenPos.x, screenPos.y);
                            }
                            else
                            {
                                // Do we still think the mouse is over our window?
                                if (_mouseOver)
                                {
                                    // Process mouse leaving situation
                                    OnWM_MOUSELEAVE();
                                }
							
                                // Let the parent chain of PopupMenu's decide if they want it
                                PopupMenu target = ParentPopupMenuWantsMouseMessage(screenPos, ref msg);

                                if (target != null)
                                {
                                    // Let parent target process the message
                                    target.OnWM_MOUSEMOVE(screenPos.x, screenPos.y);	
                                }
                                else
                                {
                                    if (ParentControlWantsMouseMessage(screenPos, ref msg))
                                    {
                                        // Let the MenuControl do its business
                                        _parentControl.OnWM_MOUSEMOVE(screenPos);
                                    }
                                }
                            }

                            // Eat the message to prevent the intended destination getting it
                            eatMessage = true;								
                        }

                        if (msg.message == (int)Win32.Msgs.WM_SETCURSOR) 
                        {
                            OnWM_SETCURSOR();

                            // Eat the message to prevent the intended destination getting it
                            eatMessage = true;								
                        }

                        // Was the alt key pressed?
                        if (msg.message == (int)Win32.Msgs.WM_SYSKEYDOWN)
                        {
                            // Alt key pressed on its own
                            if((int)msg.wParam == (int)Win32.VirtualKeys.VK_MENU)	// ALT key
                            {
                                // Then we should dimiss ourself
                                _exitLoop = true;
                            }
							else
							{
								// Pretend it is a normal keypress for processing
								msg.message = (int)Win32.Msgs.WM_KEYDOWN;
							}
                        }

                        // Was a non-alt key pressed?
                        if (msg.message == (int)Win32.Msgs.WM_KEYDOWN)
                        {
                            switch((int)msg.wParam)
                            {
                                case (int)Win32.VirtualKeys.VK_UP:
                                    ProcessKeyUp();
                                    break;
                                case (int)Win32.VirtualKeys.VK_DOWN:
                                    ProcessKeyDown();
                                    break;
                                case (int)Win32.VirtualKeys.VK_LEFT:
                                    ProcessKeyLeft();
                                    break;
                                case (int)Win32.VirtualKeys.VK_RIGHT:
                                    if(ProcessKeyRight())
                                    {
                                        // Do not attempt to pull a message off the queue as the
                                        // ProcessKeyRight has eaten the message for us
                                        leaveMsg = true;
                                    }
                                    break;
                                case (int)Win32.VirtualKeys.VK_RETURN:
                                    // Is an item currently selected
                                    if (_trackItem != -1)
                                    {
                                        DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                                        // Does this item have a submenu?
                                        if (dc.SubMenu)
                                        {
                                            // Handle the submenu
                                            OperateSubMenu(_trackItem, false);

                                            // Do not attempt to pull a message off the queue as it has already
                                            // been eaten by us in the above code
                                            leaveMsg = true;
                                        }
                                        else
                                        {
                                            // Is this item the expansion command?
                                            if (dc.Expansion)
                                            {
                                                RegenerateExpansion();
                                            }
                                            else
                                            {
                                                // Define the selection to return to caller
                                                _returnCommand = dc.MenuCommand;

                                                // Finish processing messages
                                                _exitLoop = true;	
                                            }
                                        }
                                    }
                                    break;
                                case (int)Win32.VirtualKeys.VK_ESCAPE:
                                    // User wants to exit the menu, so set the flag to exit the message loop but 
                                    // let the message get processed. This way the key press is thrown away.
                                    _exitLoop = true;
                                    break;
                                default:
                                    // Any other key is treated as a possible mnemonic
                                    int selectItem = ProcessMnemonicKey((char)msg.wParam);

                                    if (selectItem != -1)
                                    {
                                        DrawCommand dc = _drawCommands[selectItem] as DrawCommand;

                                        // Define the selection to return to caller
                                        _returnCommand = dc.MenuCommand;

                                        // Finish processing messages
                                        _exitLoop = true;	

                                        // Do not attempt to pull a message off the queue as it has already
                                        // been eaten by us in the above code
                                        leaveMsg = true;
                                    }
                                    break;
                            }
                        }

                        // We consume all keyboard input
                        if ((msg.message == (int)Win32.Msgs.WM_KEYDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_KEYUP) ||
                            (msg.message == (int)Win32.Msgs.WM_SYSKEYDOWN) ||
                            (msg.message == (int)Win32.Msgs.WM_SYSKEYUP))					
                        {
                            // Eat the message to prevent the intended destination getting it
                            eatMessage = true;								
                        }						

                        // Should the message be eaten to prevent intended destination getting it?
                        if (eatMessage)
                        {
                            Win32.MSG eat = new Win32.MSG();
                            User32.GetMessage(ref eat, 0, 0, 0);
                        }
                        else
                        {	
                            // Should the message we pulled from the queue?
                            if (!leaveMsg)
                            {
                                if (User32.GetMessage(ref msg, 0, 0, 0))
                                {
                                    User32.TranslateMessage(ref msg);
                                    User32.DispatchMessage(ref msg);
                                }
                            }
                            else
                                leaveMsg = false;
                        }
                    }
                }
            }	

            // If caret was hidden then show it again now
            if (hideCaret)
                User32.ShowCaret(IntPtr.Zero);

            // Remove tracking of any item, this ensure 'Deselected' event is generated if required
            SwitchSelection(_trackItem, -1, false, false);

            // Hide the window from view before killing it, as sometimes there is a
            // short delay between killing it and it disappearing because of the time
            // it takes for the destroy messages to get processed
			HideMenuWindow();

            // Commit suicide
            DestroyHandle();

            // Was a command actually selected in a top level PopupMenu AND we
            // are not here at the request of a MenuControl
            if ((_parentMenu == null) && (_returnCommand != null) && (_parentControl == null))
            {
                // Pulse the selected event for the command
                _returnCommand.OnClick(EventArgs.Empty);
            }

            return _returnCommand;
        }

        public void Dismiss()
        {
            if (this.Handle != IntPtr.Zero)
            {
                // Prevent the timer from expiring
                _timer.Stop();

                // Kill any child menu
                if (_childMenu != null)
                    _childMenu.Dismiss();

                // Finish processing messages
                _exitLoop = true;	

                // Hide ourself
				HideMenuWindow();

                // Cause our own message loop to exit
                User32.PostMessage(this.Handle, WM_DISMISS, 0, 0); 
            }
        }
		
		protected void HideMenuWindow()
		{
	        User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_HIDE);
		}

		protected void ManualAnimateBlend(bool show)
		{
            // Set the image to be completely transparent so the following command
            // to show the window does not actual show anything.
            UpdateLayeredWindow(0);

            // Show the window without activating it (i.e. do not take focus)
            User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);

            int stepDelay = (int)(_animateTime / _blendSteps);
            
            for(int i=0; i<_blendSteps; i++)
		    {
		        // Calculate increasing values of opaqueness
		        byte alpha = (byte)(63 + (192 / _blendSteps * (i + 1)));
		        
		        DateTime beforeTime = DateTime.Now;
		    
                // Update the image for display
                UpdateLayeredWindow(alpha);

                DateTime afterTime = DateTime.Now;

                // Need to subtract elapsed time from required step delay                
                TimeSpan elapsed = afterTime.Subtract(beforeTime);
    
                // Short delay before showing next frame, but test if delay is actually needed
                // because sometimes the time to update layered window is longer than delay
                if ((_animateTime > 0) && (elapsed.Milliseconds < stepDelay))
                    System.Threading.Thread.Sleep(stepDelay - elapsed.Milliseconds);
            }
		}

        protected void CreateAndShowWindow()
        {
            // Decide if we need layered windows
            _layered = (_supportsLayered && (_style == VisualStyle.IDE));

            // Process the menu commands to determine where each one needs to be
            // drawn and return the size of the window needed to display it.
            Size winSize = GenerateDrawPositions();

            Point screenPos = CorrectPositionForScreen(winSize);

			// Special case, if there are no menu options to show then show nothing by
			// making the window 0,0 in size.
			if (_menuCommands.Count == 0)
				winSize = new Size(0,0);

            CreateParams cp = new CreateParams();

            // Any old title will do as it will not be shown
            cp.Caption = "NativePopupMenu";
			
            // Define the screen position/size
            cp.X = screenPos.X;
            cp.Y = screenPos.Y;
            cp.Height = winSize.Height;
            cp.Width = winSize.Width;

            // As a top-level window it has no parent
            cp.Parent = IntPtr.Zero;
			
            // Appear as a top-level window
            cp.Style = unchecked((int)(uint)Win32.WindowStyles.WS_POPUP);
			
            // Set styles so that it does not have a caption bar and is above all other 
            // windows in the ZOrder, i.e. TOPMOST
            cp.ExStyle = (int)Win32.WindowExStyles.WS_EX_TOPMOST + 
                         (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW;

            // OS specific style
            if (_layered)
            {
                // If not on NT then we are going to use alpha blending on the shadow border
                // and so we need to specify the layered window style so the OS can handle it
                cp.ExStyle += (int)Win32.WindowExStyles.WS_EX_LAYERED;
            }

            // Is this the plain style of appearance?
            if (_style == VisualStyle.Plain)
            {
                // We want the tradiditonal 3D border
                cp.Style += unchecked((int)(uint)Win32.WindowStyles.WS_DLGFRAME);
            }

            // Create the actual window
            this.CreateHandle(cp);

            // Update the window clipping region
            if (!_layered)
                SetWindowRegion(winSize);			

            // Remember the correct screen drawing details
            _currentSize = winSize;
            _currentPoint = screenPos;

			bool animated = false;

            if (_layered)
            {
                // Update the image for display
                UpdateLayeredWindow();

                bool animate = false;
				
				switch(_animate)
				{
				    case Animate.No:
				        animate = false;
				        break;
				    case Animate.Yes:
                        animate = true;
                        break;
                    case Animate.System:
                        int bRetValue = 0;

                        // Does the system want animation to occur?
                        User32.SystemParametersInfo((uint)Win32.SPIActions.SPI_GETMENUANIMATION, 0, ref bRetValue, 0);

                        animate = (bRetValue != 0);
                        break;
                }

				// Should the menu be shown with animation?
				if (animate && _animateIn)
				{
                    uint animateFlags = (uint)_animateStyle;

					if (_animateStyle == Animation.System)
					{
						int bRetValue = 0;

						// Does the system want fading or sliding?
						User32.SystemParametersInfo((uint)Win32.SPIActions.SPI_GETMENUFADE, 0, ref bRetValue, 0);

						// Use appropriate flags to match request
						if (bRetValue != 0)
							animateFlags = (uint)Animation.Blend;
						else
							animateFlags = (uint)Animation.SlideHorVerPositive;
					}

					// Animate the appearance of the window
					if ((animateFlags & (uint)Win32.AnimateFlags.AW_BLEND) != 0)
					{
						// Cannot use Win32.AnimateWindow to blend a layered window
						ManualAnimateBlend(true);
					}
					else
					{
                        // Animate the appearance of the window
						User32.AnimateWindow(this.Handle, (uint)_animateTime, animateFlags); 
					}

					animated = true;
				}
            }

			// Did any animation take place?
			if (!animated)
			{
                // Show the window without activating it (i.e. do not take focus)
				User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
			}
        }

        protected void UpdateLayeredWindow()
        {
            UpdateLayeredWindow(_currentPoint, _currentSize, 255);
        }

        protected void UpdateLayeredWindow(byte alpha)
        {
            UpdateLayeredWindow(_currentPoint, _currentSize, alpha);
        }

        protected void UpdateLayeredWindow(Point point, Size size, byte alpha)
        {
            // Create bitmap for drawing onto
            Bitmap memoryBitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);

            using(Graphics g = Graphics.FromImage(memoryBitmap))
            {
                Rectangle area = new Rectangle(0, 0, size.Width, size.Height);
		
                // Draw the background area
                DrawBackground(g, area);

                // Draw the actual menu items
                DrawAllCommands(g);

                // Get hold of the screen DC
                IntPtr hDC = User32.GetDC(IntPtr.Zero);
				
                // Create a memory based DC compatible with the screen DC
                IntPtr memoryDC = Gdi32.CreateCompatibleDC(hDC);
				
                // Get access to the bitmap handle contained in the Bitmap object
                IntPtr hBitmap = memoryBitmap.GetHbitmap(Color.FromArgb(0));

                // Select this bitmap for updating the window presentation
                IntPtr oldBitmap = Gdi32.SelectObject(memoryDC, hBitmap);

                // New window size
                Win32.SIZE ulwsize;
                ulwsize.cx = size.Width;
                ulwsize.cy = size.Height;

                // New window position
                Win32.POINT topPos;
                topPos.x = point.X;
                topPos.y = point.Y;

                // Offset into memory bitmap is always zero
                Win32.POINT pointSource;
                pointSource.x = 0;
                pointSource.y = 0;

                // We want to make the entire bitmap opaque 
                Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
                blend.BlendOp             = (byte)Win32.AlphaFlags.AC_SRC_OVER;
                blend.BlendFlags          = 0;
                blend.SourceConstantAlpha = alpha;
                blend.AlphaFormat         = (byte)Win32.AlphaFlags.AC_SRC_ALPHA;

                // Tell operating system to use our bitmap for painting
                User32.UpdateLayeredWindow(Handle, hDC, ref topPos, ref ulwsize, 
                                           memoryDC, ref pointSource, 0, ref blend, 
                                           (int)Win32.UpdateLayeredWindowsFlags.ULW_ALPHA);

                // Put back the old bitmap handle
                Gdi32.SelectObject(memoryDC, oldBitmap);

                // Cleanup resources
                User32.ReleaseDC(IntPtr.Zero, hDC);
                Gdi32.DeleteObject(hBitmap);
                Gdi32.DeleteDC(memoryDC);
            }
        }

        protected void SetWindowRegion(Size winSize)
        {
            // Style specific handling
            if (_style == VisualStyle.IDE)
            {
                int shadowHeight = _position[(int)_style, (int)PI.ShadowHeight];
                int shadowWidth = _position[(int)_style, (int)PI.ShadowWidth];

                // Create a new region object
                Region drawRegion = new Region();

                // Can draw anywhere
                drawRegion.MakeInfinite();

                // Remove the area above the right hand shadow
                drawRegion.Xor(new Rectangle(winSize.Width - shadowWidth, 0, shadowWidth, shadowHeight));

                // When drawing upwards from a vertical menu we need to allow a connection between the 
                // MenuControl selection box and the PopupMenu shadow
                if (!((_direction == Direction.Vertical) && !_excludeTop))
                {
                    // Remove the area left of the bottom shadow
                    drawRegion.Xor(new Rectangle(0, winSize.Height - shadowHeight, shadowWidth, shadowHeight));
                }

                // Define a region to prevent drawing over exposed corners of shadows
                using(Graphics g = Graphics.FromHwnd(this.Handle))
                    User32.SetWindowRgn(this.Handle, drawRegion.GetHrgn(g), false);
            }

        }

        protected Point CorrectPositionForScreen(Size winSize)
        {
            Rectangle screenRect = Screen.GetWorkingArea (_screenPos);
            Point screenPos = _screenPos;

            int screenWidth = screenRect.Width;
            int screenLeft  = screenRect.Left;
            int screenRight = screenRect.Right;
            int screenHeight = screenRect.Height;
            int screenBottom = screenRect.Bottom;
            int screenTop    = screenRect.Top;

            // Default to excluding menu border from top
            _excludeTop = true;
            _excludeOffset = 0;

            // Shadow area does not count as part of width
            winSize.Width -= _position[(int)_style, (int)PI.ShadowWidth];

            // Calculate the downward position first
            if (_popupDown)
            {
                // Ensure the end of the menu is not off the bottom of the screen
                if ((screenPos.Y + winSize.Height) > screenBottom)
                {
                    // If the parent control exists then try and position upwards instead
                    if ((_parentControl != null) && (_parentMenu == null))
                    {
                        // Is there space above the required position?
                        if ((_aboveScreenPos.Y - winSize.Height) > screenTop)
                        {
                            // Great...do that instead
                            screenPos.Y = _aboveScreenPos.Y - winSize.Height;

                            // Reverse direction of drawing this and submenus
                            _popupDown = false;

                            // Remember to exclude border from bottom of menu and not the top
                            _excludeTop = false;

                            // Inform parent it needs to redraw the selection upwards
                            _parentControl.DrawSelectionUpwards();
                        }	
                    }

                    // Did the above logic still fail?
                    if ((screenPos.Y + winSize.Height) > screenBottom)
                    {
                        // If not a top level PopupMenu then..
                        if (_parentMenu != null)
                        {
                            // Reverse direction of drawing this and submenus
                            _popupDown = false;

                            // Is there space above the required position?
                            if ((_aboveScreenPos.Y - winSize.Height) > screenTop)
                                screenPos.Y = _aboveScreenPos.Y - winSize.Height;
                            else
                                screenPos.Y = screenTop;
                        }
                        else
                            screenPos.Y = screenBottom - winSize.Height - 1;
                    }
                }
            }
            else
            {
                // Ensure the end of the menu is not off the top of the screen
                if ((screenPos.Y - winSize.Height) < screenTop)
                {
                    // Reverse direction
                    _popupDown = true;

                    // Is there space below the required position?
                    if ((screenPos.Y + winSize.Height) > screenBottom)
                        screenPos.Y = screenBottom - winSize.Height - 1;
                }
                else
                    screenPos.Y -= winSize.Height;
            }

            // Calculate the across position next
            if (_popupRight)
            {
                // Ensure that right edge of menu is not off right edge of screen
                if ((screenPos.X + winSize.Width) > screenRight)
                {
                    // If not a top level PopupMenu then...
                    if (_parentMenu != null)
                    {
                        // Reverse direction
                        _popupRight = false;

                        // Adjust across position						
                        screenPos.X = _leftScreenPos.X - winSize.Width;				
		
                        if (screenPos.X < screenLeft)
                            screenPos.X = screenLeft;
                    }
                    else
                    {	
                        // Find new position of X coordinate
                        int newX = screenRight - winSize.Width - 1;

                        // Modify the adjust needed when drawing top/bottom border
                        _excludeOffset = screenPos.X - newX;

                        // Use new position for popping up menu
                        screenPos.X = newX;
                    }
                }
            }
            else
            {
                // Start by using the left screen pos instead
                screenPos.X = _leftScreenPos.X;

                // Ensure the left edge of the menu is not off the left of the screen
                if ((screenPos.X - winSize.Width) < screenLeft)
                {
                    // Reverse direction
                    _popupRight = true;

                    // Is there space below the required position?
                    if ((_screenPos.X + winSize.Width) > screenRight)
                        screenPos.X = screenRight - winSize.Width - 1;
                    else
                        screenPos.X = _screenPos.X;
                }
                else
                    screenPos.X -= winSize.Width;
            }

            return screenPos;
        }

        protected void RegenerateExpansion()
        {
            // Remove all existing draw commands
            _drawCommands.Clear();

            // Move into the expanded mode
            _showInfrequent = true;

            // Show we remember the expansion to the collection?
            if (_rememberExpansion)
                _menuCommands.ShowInfrequent = true;
            
            // Generate new ones
            Size newSize = GenerateDrawPositions();

            // Find the new screen location for the window
            Point newPos = CorrectPositionForScreen(newSize);

            // Remember the correct screen drawing details
            _currentPoint = newPos;
            _currentSize = newSize;

            // Update the window clipping region
            if (!_layered)
            {
                SetWindowRegion(newSize);

                // Alter size and location of window
                User32.MoveWindow(this.Handle, newPos.X, newPos.Y, newSize.Width, newSize.Height, true);			

				Win32.RECT clientRect = new Win32.RECT();

				clientRect.left = 0;
				clientRect.top = 0;
				clientRect.right = newSize.Width;
				clientRect.bottom = newSize.Height;

				// Get the client area redrawn after MoveWindow has been processed
				User32.InvalidateRect(this.Handle, ref clientRect, true);
            }
            else
            {
                // Update the image for display
                UpdateLayeredWindow();

				// Lets repaint everything
				RefreshAllCommands();
            }
        }

        protected Size GenerateDrawPositions()
        {
            // Create a collection of drawing objects
            _drawCommands = new ArrayList();

            // Calculate the minimum cell width and height
            int cellMinHeight = _position[(int)_style, (int)PI.ImageGapTop] +
                                _imageHeight + 
                                _position[(int)_style, (int)PI.ImageGapBottom];
			
            int cellMinWidth = _position[(int)_style, (int)PI.ImageGapLeft] +
                               _imageWidth + 
                               _position[(int)_style, (int)PI.ImageGapRight] + 
                               _position[(int)_style, (int)PI.TextGapLeft] +
                               _position[(int)_style, (int)PI.TextGapRight] +
                               _position[(int)_style, (int)PI.SubMenuGapLeft] +
                               _position[(int)_style, (int)PI.SubMenuWidth] +
                               _position[(int)_style, (int)PI.SubMenuGapRight];

            // Find cell height needed to draw text
            int textHeight = _textFont.Height;

            // If height needs to be more to handle image then use image height
            if (textHeight < cellMinHeight)
                textHeight = cellMinHeight;

            // Make sure no column in the menu is taller than the screen
            int screenHeight = SystemInformation.WorkingArea.Height;
	
            // Define the starting positions for calculating cells
            int xStart = _position[(int)_style, (int)PI.BorderLeft];
            int yStart =_position[(int)_style, (int)PI.BorderTop];
            int yPosition = yStart;

            // Largest cell for column defaults to minimum cell width
            int xColumnMaxWidth = cellMinWidth;

            int xPreviousColumnWidths = 0;
            int xMaximumColumnHeight = 0;

            // Track the row/col of each cell
            int row = 0;
            int col = 0;

            // Are there any infrequent items
            bool infrequent = false;
            bool previousInfrequent = false;

            // Get hold of the DC for the desktop
            IntPtr hDC = User32.GetDC(IntPtr.Zero);

            // Contains the collection of items in the current column
            ArrayList columnItems = new ArrayList();

            using(Graphics g = Graphics.FromHdc(hDC))
            {
                // Handle any extra text drawing
                if (_menuCommands.ExtraText.Length > 0)
                {
                    // Calculate the column width needed to show this text
                    SizeF dimension = g.MeasureString(_menuCommands.ExtraText, _menuCommands.ExtraFont);

                    // Always add 1 to ensure that rounding is up and not down
                    int extraHeight = (int)dimension.Height + 1;

                    // Find the total required as the text requirement plus style specific spacers
                    _extraSize = extraHeight + 
                                 _position[(int)_style, (int)PI.ExtraRightGap] +
                                 _position[(int)_style, (int)PI.ExtraWidthGap] * 2;

                    // Push first column of items across from the extra text
                    xStart += _extraSize;
				
                    // Add this extra width to the total width of the window
                    xPreviousColumnWidths = _extraSize;
                }

                foreach(MenuCommand command in _menuCommands)
                {
                    // Give the command a chance to update its state
                    command.OnUpdate(EventArgs.Empty);

                    // Ignore items that are marked as hidden
                    if (!command.Visible)
                        continue;

                    // If this command has menu items (and so it a submenu item) then check
                    // if any of the submenu items are visible. If none are visible then there
                    // is no point in showing this submenu item
                    if ((command.MenuCommands.Count > 0) && (!command.MenuCommands.VisibleItems()))
                        continue;

                    // Ignore infrequent items unless flag set to show them
                    if (command.Infrequent && !_showInfrequent)
                    {
                        infrequent = true;
                        continue;
                    }

                    int cellWidth = 0;
                    int cellHeight = 0;

                    // Shift across to the next column?
                    if (command.Break)
                    {		
                        // Move row/col tracking to the next column				
                        row = 0;
                        col++;

                        // Apply cell width to the current column entries
                        ApplySizeToColumnList(columnItems, xColumnMaxWidth);

                        // Move cell position across to start of separator position
                        xStart += xColumnMaxWidth;

                        // Get width of the separator area
                        int xSeparator = _position[(int)_style, (int)PI.SeparatorWidth];
						
                        DrawCommand dcSep = new DrawCommand(new Rectangle(xStart, 0, xSeparator, 0), false);

                        // Add to list of items for drawing
                        _drawCommands.Add(dcSep);

                        // Move over the separator
                        xStart += xSeparator;						

                        // Reset cell position to top of column
                        yPosition = yStart;

                        // Accumulate total width of previous columns
                        xPreviousColumnWidths += xColumnMaxWidth + xSeparator;
						
                        // Largest cell for column defaults to minimum cell width
                        xColumnMaxWidth  = cellMinWidth;
                    }

                    // Is this a horizontal separator?
                    if (command.Text == "-")
                    {
                        cellWidth = cellMinWidth;
                        cellHeight = _position[(int)_style, (int)PI.SeparatorHeight];
                    }
                    else
                    {
                        // Use precalculated height
                        cellHeight = textHeight;

                        // Calculate the text width portion of the cell
                        SizeF dimension = g.MeasureString(command.Text, _textFont);
	
                        // Always add 1 to ensure that rounding is up and not down
                        cellWidth = cellMinWidth + (int)dimension.Width + 1;

                        // Does the menu command have a shortcut defined?
                        if (command.Shortcut != Shortcut.None)
                        {
                            // Find the width of the shortcut text
                            dimension = g.MeasureString(GetShortcutText(command.Shortcut), _textFont);

                            // Add to the width of the cell
                            cellWidth += _position[(int)_style, (int)PI.ShortcutGap] + (int)dimension.Width + 1;
                        }
                    }

                    // If the new cell expands past the end of the screen...
                    if ((yPosition + cellHeight) >= screenHeight)
                    {
                        // .. then need to insert a column break

                        // Move row/col tracking to the next column				
                        row = 0;
                        col++;

                        // Apply cell width to the current column entries
                        ApplySizeToColumnList(columnItems, xColumnMaxWidth);

                        // Move cell position across to start of separator position
                        xStart += xColumnMaxWidth;

                        // Get width of the separator area
                        int xSeparator = _position[(int)_style, (int)PI.SeparatorWidth];
						
                        DrawCommand dcSep = new DrawCommand(new Rectangle(xStart, yStart, xSeparator, 0), false);

                        // Add to list of items for drawing
                        _drawCommands.Add(dcSep);

                        // Move over the separator
                        xStart += xSeparator;						

                        // Reset cell position to top of column
                        yPosition = yStart;

                        // Accumulate total width of previous columns
                        xPreviousColumnWidths += xColumnMaxWidth + xSeparator;
						
                        // Largest cell for column defaults to minimum cell width
                        xColumnMaxWidth  = cellMinWidth;

                        // Show this be drawn in the infrequent colors
                        dcSep.Infrequent = previousInfrequent;
                    }
	
                    // Create a new position rectangle (the width will be reset later once the 
                    // width of the column has been determined but the other values are correct)
                    Rectangle cellRect = new Rectangle(xStart, yPosition, cellWidth, cellHeight);
	
                    // Create a drawing object
                    DrawCommand dc = new DrawCommand(command, cellRect, row, col);

                    // Is the infrequent state different from previous commands?
                    if (previousInfrequent != command.Infrequent)
                    {
                        // If was not infrequent but now is
                        if (command.Infrequent)
                        {
                            // Then draw this infrequent item with a top border
                            dc.TopBorder = true;
                        }
                        else
                        {
                            if (_drawCommands.Count > 0)
                            {
                                // Find the previous command (excluding separators) and make it as needed a border
                                for(int index = _drawCommands.Count - 1; index>=0; index--)
                                {
                                    if (!( _drawCommands[index] as DrawCommand).Separator)
                                    {
                                        (_drawCommands[index] as DrawCommand).BottomBorder = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    
                    // Remember the state of previous command only if not a separator
                    if (!dc.Separator)
                        previousInfrequent = command.Infrequent;
                    
                    // Add to list of items for drawing
                    _drawCommands.Add(dc);

                    // Add to list of items in this column
                    columnItems.Add(dc);					

                    // Remember the biggest cell width in this column
                    if (cellWidth > xColumnMaxWidth)
                        xColumnMaxWidth = cellWidth;

                    // Move down to start of next cell in column
                    yPosition += cellHeight;

                    // Remember the tallest column in the menu
                    if (yPosition > xMaximumColumnHeight)
                        xMaximumColumnHeight = yPosition;

                    row++;
                }

                // Check if we need to add an infrequent expansion item
                if (infrequent)
                {
                    // Create a minimum size cell
                    Rectangle cellRect = new Rectangle(xStart, yPosition, cellMinWidth, cellMinHeight);

                    // Create a draw command to represent the drawing of the expansion item
                    DrawCommand dc = new DrawCommand(cellRect, true);

                    // Must be last item
                    _drawCommands.Add(dc);

                    // Add to list of items in this column
                    columnItems.Add(dc);					

                    yPosition += cellMinHeight;

                    // Remember the tallest column in the menu
                    if (yPosition > xMaximumColumnHeight)
                        xMaximumColumnHeight = yPosition;
                }

                // Apply cell width to the current column entries
                ApplySizeToColumnList(columnItems, xColumnMaxWidth);
            }

            // Must remember to release the HDC resource!
            User32.ReleaseDC(IntPtr.Zero, hDC);

            // Find width/height of window
            int windowWidth = _position[(int)_style, (int)PI.BorderLeft] +
                              xPreviousColumnWidths + 
                              xColumnMaxWidth + 
                              _position[(int)_style, (int)PI.BorderRight];

            int windowHeight = _position[(int)_style, (int)PI.BorderTop] +
                               xMaximumColumnHeight + 
                               _position[(int)_style, (int)PI.BorderBottom];

            // Define the height of the vertical separators
            ApplyVerticalSeparators(xMaximumColumnHeight);

            // Style specific modification of window size
            int xAdd = _position[(int)_style, (int)PI.ShadowHeight];
            int yAdd = _position[(int)_style, (int)PI.ShadowWidth];

            if (_style == VisualStyle.Plain)
            {
                xAdd += SystemInformation.Border3DSize.Width * 2;
                yAdd += SystemInformation.Border3DSize.Height * 2;
            }

            return new Size(windowWidth + xAdd, windowHeight + yAdd);
        }

        protected void DefineHighlightColors(Color baseColor)
        {
            _highlightColor = baseColor;
        
            if (_highlightColor == SystemColors.Highlight)
            {
                _highlightColorDark = SystemColors.Highlight;
                _highlightColorLight = Color.FromArgb(70, _highlightColorDark);
                _highlightColorLightLight = Color.FromArgb(20, _highlightColorDark);
            }
            else
            {
                _highlightColorDark = ControlPaint.Dark(baseColor);
                _highlightColorLight = baseColor;
                _highlightColorLightLight = Color.FromArgb(70, baseColor);
            }
        }
            
        protected void DefineColors(Color backColor)
        {
            _backColor = backColor;
            _controlLL = ControlPaint.LightLight(_backColor);
            _controlLBrush = new SolidBrush(Color.FromArgb(200, _backColor));
            _controlEBrush = new SolidBrush(Color.FromArgb(150, _backColor));
            _controlLLBrush = new SolidBrush(_controlLL);
        }

        protected void ApplyVerticalSeparators(int sepHeight)
        {
            // Each vertical separator needs to be the same height, this has already 
            // been calculated and passed in from the tallest column in the menu
            foreach(DrawCommand dc in _drawCommands)
            {
                if (dc.VerticalSeparator)
                {
                    // Grab the current drawing rectangle
                    Rectangle cellRect = dc.DrawRect;

                    // Modify the height to that requested
                    dc.DrawRect = new Rectangle(cellRect.Left, cellRect.Top, cellRect.Width, sepHeight);
                }
            }
        }

        protected void ApplySizeToColumnList(ArrayList columnList, int cellWidth)
        {
            // Each cell in the same column needs to be the same width, this has already
            // been calculated and passed in as the widest cell in the column
            foreach(DrawCommand dc in columnList)
            {
                // Grab the current drawing rectangle
                Rectangle cellRect = dc.DrawRect;

                // Modify the width to that requested
                dc.DrawRect = new Rectangle(cellRect.Left, cellRect.Top, cellWidth, cellRect.Height);
            }

            // Clear collection out ready for reuse
            columnList.Clear();
        }

        protected void RefreshAllCommands()
        {
            Win32.RECT rectRaw = new Win32.RECT();

            // Grab the screen rectangle of the window
            User32.GetWindowRect(this.Handle, ref rectRaw);

            // Convert from screen to client sizing
            Rectangle rectWin = new Rectangle(0, 0, 
                                              rectRaw.right - rectRaw.left, 
                                              rectRaw.bottom - rectRaw.top);

            using(Graphics g = Graphics.FromHwnd(this.Handle))
            {
                // Draw the background area
                DrawBackground(g, rectWin);

                // Draw the actual menu items
                DrawAllCommands(g);
            }
        }

        protected void DrawBackground(Graphics g, Rectangle rectWin)
        { 
            Rectangle main = new Rectangle(0, 0, 
                                           rectWin.Width - 1 - _position[(int)_style, (int)PI.ShadowWidth], 
                                           rectWin.Height - 1 - _position[(int)_style, (int)PI.ShadowHeight]);

            // Style specific drawing
            switch(_style)
            {
                case VisualStyle.IDE:
                    // Calculate some common values
                    int imageColWidth = _position[(int)_style, (int)PI.ImageGapLeft] + 
                                        _imageWidth + 
                                        _position[(int)_style, (int)PI.ImageGapRight];

                    int xStart = _position[(int)_style, (int)PI.BorderLeft];
                    int yStart = _position[(int)_style, (int)PI.BorderTop];
                    int yHeight = main.Height - yStart - _position[(int)_style, (int)PI.BorderBottom] - 1;

                    // Paint the main area background
                    g.FillRectangle(_controlLLBrush, main);

                    // Draw single line border around the main area
                    using(Pen mainBorder = new Pen(ControlPaint.Dark(_backColor)))
                    {
                        g.DrawRectangle(mainBorder, main);

                        // Should the border be drawn with part of the border missing?
                        if (_borderGap > 0)
                        {
                            // Remove the appropriate section of the border
                            if (_direction == Direction.Horizontal)
                            {
                                if (_excludeTop)
                                {
                                    g.FillRectangle(Brushes.White, main.Left + 1 + _excludeOffset, main.Top, _borderGap - 1, 1);
                                    g.FillRectangle(_controlLBrush, main.Left + 1 + _excludeOffset, main.Top, _borderGap - 1, 1);
                                }
                                else
                                {
                                    g.FillRectangle(Brushes.White, main.Left + 1 + _excludeOffset, main.Bottom, _borderGap - 1, 1);
                                    g.FillRectangle(_controlLBrush, main.Left + 1 + _excludeOffset, main.Bottom, _borderGap - 1, 1);
                                }
                            }
                            else
                            {
                                if (_excludeTop)
                                {
                                    g.FillRectangle(Brushes.White, main.Left, main.Top + 1 + _excludeOffset, 1, _borderGap - 1);
                                    g.FillRectangle(_controlLBrush, main.Left, main.Top + 1 + _excludeOffset, 1, _borderGap - 1);
                                }
                                else
                                {
                                    if (_popupDown)
                                    {
                                        g.FillRectangle(Brushes.White, main.Left, main.Bottom - 1 - _excludeOffset, 1, _borderGap - 1);
                                        g.FillRectangle(_controlLBrush, main.Left, main.Bottom - 1 - _excludeOffset, 1, _borderGap - 1);
                                    }
                                    else
                                    {
                                        g.FillRectangle(Brushes.White, main.Left, main.Bottom - _borderGap + 1, 1, _borderGap - 1);
                                        g.FillRectangle(_controlLBrush, main.Left, main.Bottom - _borderGap + 1, 1, _borderGap - 1);
                                    }
                                }
                            }
                        }
                    }

                    // Draw the first image column
                    Rectangle imageRect = new Rectangle(xStart, yStart, imageColWidth, yHeight);

                    g.FillRectangle(Brushes.White, imageRect);
                    g.FillRectangle(_controlLBrush, imageRect);

                    // Draw image column after each vertical separator
                    foreach(DrawCommand dc in _drawCommands)
                    {
                        if (dc.Separator && dc.VerticalSeparator)
                        {
                            // Recalculate starting position (but height remains the same)
                            imageRect.X = dc.DrawRect.Right;

                            g.FillRectangle(Brushes.White, imageRect);
                            g.FillRectangle(_controlLBrush, imageRect);
                        }
                    }

                    // Draw shadow around borders
                    int rightLeft = main.Right + 1;
                    int rightTop = main.Top + _position[(int)_style, (int)PI.ShadowHeight];
                    int rightBottom = main.Bottom + 1;
                    int leftLeft = main.Left + _position[(int)_style, (int)PI.ShadowWidth];
                    int xExcludeStart = main.Left + _excludeOffset;
                    int xExcludeEnd = main.Left + _excludeOffset + _borderGap;

                    if ((_borderGap > 0) && (!_excludeTop) && (_direction == Direction.Horizontal))
                    {
                        int rightright = rectWin.Width;

                        if (xExcludeStart >= leftLeft)
                            DrawShadowHorizontal(g, leftLeft, rightBottom, xExcludeStart - leftLeft, _position[(int)_style, (int)PI.ShadowHeight], Shadow.Left);

                        if (xExcludeEnd <= rightright)
                            DrawShadowHorizontal(g, xExcludeEnd, rightBottom, rightright - xExcludeEnd, _position[(int)_style, (int)PI.ShadowHeight], Shadow.Right);
                    }
                    else
                    {
                        if ((_direction == Direction.Vertical) && (!_excludeTop))
                            leftLeft = 0;
								
                        DrawShadowHorizontal(g, leftLeft, rightBottom, rightLeft, _position[(int)_style, (int)PI.ShadowHeight], Shadow.All);
                    }

                    DrawShadowVertical(g, rightLeft, rightTop, _position[(int)_style, (int)PI.ShadowWidth], rightBottom - rightTop - 1);
                    break;
                case VisualStyle.Plain:
                    // Paint the main area background
                    using(SolidBrush mainBrush = new SolidBrush(_backColor))
                        g.FillRectangle(mainBrush, rectWin);
                    break;
            }

            // Is there an extra title text to be drawn?
            if (_menuCommands.ExtraText.Length > 0)
                DrawColumn(g, main);
        }

        protected void DrawShadowVertical(Graphics g, int left, int top, int width, int height)
        {
            if (_layered)
            {
                Color extraColor = Color.FromArgb(64, 0, 0, 0);
                Color darkColor = Color.FromArgb(48, 0, 0, 0);
                Color lightColor = Color.FromArgb(0, 0, 0, 0);
            
                // Enough room for top and bottom shades?
                if (height >= _shadowLength)
                {
                    using(LinearGradientBrush topBrush = new LinearGradientBrush(new Point(left - _shadowLength, top + _shadowLength), 
                                                                                 new Point(left + _shadowLength, top),
                                                                                 extraColor, lightColor))
                    {
                        // Draw top shade
                        g.FillRectangle(topBrush, left, top, _shadowLength, _shadowLength);
                        
                        top += _shadowLength;
                        height -= _shadowLength;
                    }
                }
               
               using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Point(left, 0), 
                                                                               new Point(left + width, 0),
                                                                               darkColor, lightColor))
                {                                                                               
                    // Draw middle shade
                    g.FillRectangle(middleBrush, left, top, width, height + 1);
                }
            }
            else
            {
                using(SolidBrush shadowBrush = new SolidBrush(ControlPaint.Dark(_backColor)))
                    g.FillRectangle(shadowBrush, left, top, width, height + 1);
            }
        }

        protected void DrawShadowHorizontal(Graphics g, int left, int top, int width, int height, Shadow op)
        {
            if (_layered)
            {
                Color extraColor = Color.FromArgb(64, 0, 0, 0);
                Color darkColor = Color.FromArgb(48, 0, 0, 0);
                Color lightColor = Color.FromArgb(0, 0, 0, 0);

                // Do we need to draw the left shadow?
                if (op != Shadow.Right)
                {
                    if (width >= _shadowLength)
                    {
                        // Draw the remaining middle
                        using(LinearGradientBrush leftBrush = new LinearGradientBrush(new Point(left + _shadowLength, top - _shadowLength), 
                                                                                      new Point(left, top + height),
                                                                                      extraColor, lightColor))
                        {                                                                               
                            // Draw middle shade
                            g.FillRectangle(leftBrush, left, top, _shadowLength, height);
                            
                            left += _shadowLength;
                            width -= _shadowLength;
                        }
                    }
                }
                
                // Do we need to draw the right shadow?
                if (op != Shadow.Left)
                {
                    if (width >= _shadowLength)
                    {
                        try
                        {
                            g.DrawImageUnscaled(GetShadowCache(_shadowLength, height), left + width - _shadowLength, top);
                        }
                        catch
                        {
                            //	just to be on the safe side...
                        }
                        width -= _shadowLength;
                    }
                }

                // Draw the remaining middle
                using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Point(9999, top), 
                                                                                new Point(9999, top + height),
                                                                                darkColor, lightColor))
                {                                                                               
                    // Draw middle shade
                    g.FillRectangle(middleBrush, left, top, width, height);
                }
            }
            else
            {
                using(SolidBrush shadowBrush = new SolidBrush(ControlPaint.Dark(_backColor)))
                    g.FillRectangle(shadowBrush, left, top, width, height);
            }
        }
        
        protected void DrawColumn(Graphics g, Rectangle main)
        {
            // Create the rectangle that encloses the drawing
            Rectangle rectText = new Rectangle(main.Left, 
                                               main.Top, 
                                               _extraSize - _position[(int)_style, (int)PI.ExtraRightGap], 
                                               main.Height);

            Brush backBrush = null;
            bool disposeBack = true;

            if (_menuCommands.ExtraBackBrush != null)
            {
                backBrush = _menuCommands.ExtraBackBrush;
                disposeBack = false;
                rectText.Width++;
            }
            else
                backBrush = new SolidBrush(_menuCommands.ExtraBackColor);

            // Fill background using brush
            g.FillRectangle(backBrush, rectText);

            // Do we need to dispose of the brush?
            if (disposeBack)
                backBrush.Dispose();

            // Adjust rectangle for drawing the text into
            rectText.X += _position[(int)_style, (int)PI.ExtraWidthGap];
            rectText.Y += _position[(int)_style, (int)PI.ExtraHeightGap];
            rectText.Width -= _position[(int)_style, (int)PI.ExtraWidthGap] * 2;
            rectText.Height -= _position[(int)_style, (int)PI.ExtraHeightGap] * 2;

            // For Plain style we need to take into account the border sizes
            if (_style == VisualStyle.Plain)
                rectText.Height -= SystemInformation.Border3DSize.Height * 2;

            // Draw the text into this rectangle
            StringFormat format = new StringFormat();
            format.FormatFlags = StringFormatFlags.DirectionVertical | 
                                 StringFormatFlags.NoClip | 
                                 StringFormatFlags.NoWrap;
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;

            Brush textBrush = null;
            bool disposeText = true;

            if (_menuCommands.ExtraTextBrush != null)
            {
                textBrush = _menuCommands.ExtraTextBrush;
                disposeText = false;
            }
            else
                textBrush = new SolidBrush(_menuCommands.ExtraTextColor);

            // Draw string from bottom of area towards the top using the given Font/Brush
            DrawHelper.DrawReverseString(g, _menuCommands.ExtraText, _menuCommands.ExtraFont, rectText, textBrush, format);

            // Do we need to dispose of the brush?
            if (disposeText)
                textBrush.Dispose();
        }

        internal void DrawSingleCommand(Graphics g, DrawCommand dc, bool hotCommand)
        {
            Rectangle drawRect = dc.DrawRect;
            MenuCommand mc = dc.MenuCommand;
	
            // Remember some often used values
            int textGapLeft = _position[(int)_style, (int)PI.TextGapLeft];
            int imageGapLeft = _position[(int)_style, (int)PI.ImageGapLeft];
            int imageGapRight = _position[(int)_style, (int)PI.ImageGapRight];
            int imageLeft = drawRect.Left + imageGapLeft;

            // Calculate some common values
            int imageColWidth = imageGapLeft + _imageWidth + imageGapRight;

            int subMenuWidth = _position[(int)_style, (int)PI.SubMenuGapLeft] +
                               _position[(int)_style, (int)PI.SubMenuWidth] +
                               _position[(int)_style, (int)PI.SubMenuGapRight];

            int subMenuX = drawRect.Right - 
                           _position[(int)_style, (int)PI.SubMenuGapRight] -
                           _position[(int)_style, (int)PI.SubMenuWidth];

            // Text drawing rectangle needs to know the right most position for drawing
            // to stop. This is the width of the window minus the relevant values
            int shortCutX = subMenuX - 
                            _position[(int)_style, (int)PI.SubMenuGapLeft] -
                            _position[(int)_style, (int)PI.TextGapRight];

            // Is this item an expansion command?
            if (dc.Expansion)
            {
                Rectangle box = drawRect;

                // In IDE style the box is next to the image column
                if (_style == VisualStyle.IDE)
                {
                    // Reduce the box to take into account the column
                    box.X += imageColWidth;
                    box.Width -= imageColWidth;
                }

                // Find centre for drawing the image
                int xPos = box.Left + ((box.Width - _imageHeight) / 2);
                int yPos = box.Top + ((box.Height - _imageHeight) / 2);
				
                // Should the item look selected
                if (hotCommand)
                {
                    switch(_style)
                    {
                        case VisualStyle.IDE:
                            Rectangle selectArea = new Rectangle(drawRect.Left + 1, drawRect.Top,
                                                                 drawRect.Width - 3, drawRect.Height - 1);

                            using (Pen selectPen = new Pen(_highlightColorDark))
                            {
                                // Draw the selection area white, because we are going to use an alpha brush
                                using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                                    g.FillRectangle(whiteBrush, selectArea);

                                using (SolidBrush selectBrush = new SolidBrush(_highlightColorLight))
                                {
                                    // Draw the selection area
                                    g.FillRectangle(selectBrush, selectArea);

                                    // Draw a border around the selection area
                                    g.DrawRectangle(selectPen, selectArea);
                                }
                            }
                            break;
                        case VisualStyle.Plain:
                            // Shrink the box to provide a small border
                            box.Inflate(-2, -2);

                            using (Pen lightPen = new Pen(ControlPaint.LightLight(_backColor)),
                                       darkPen = new Pen(ControlPaint.DarkDark(_backColor)))
                            {
                                g.DrawLine(lightPen, box.Right, box.Top, box.Left, box.Top);
                                g.DrawLine(lightPen, box.Left, box.Top, box.Left, box.Bottom);
                                g.DrawLine(darkPen, box.Left, box.Bottom, box.Right, box.Bottom);
                                g.DrawLine(darkPen, box.Right, box.Bottom, box.Right, box.Top);
                            }
                            break;
                    }
                }
                else
                {
                    switch(_style)
                    {
                        case VisualStyle.IDE:
                            // Fill the entire drawing area with white
                            g.FillRectangle(_controlLLBrush, new Rectangle(drawRect.Left + 1, drawRect.Top,
                                drawRect.Width - 1, drawRect.Height));

                            // Draw the image column background
                            g.FillRectangle(Brushes.White, new Rectangle(drawRect.Left, drawRect.Top,
                                                                         imageColWidth, drawRect.Height));
        
                            g.FillRectangle(_controlLBrush, new Rectangle(drawRect.Left, drawRect.Top,
                                                                          imageColWidth, drawRect.Height));
                            break;
                        case VisualStyle.Plain:
                            using(SolidBrush drawBrush = new SolidBrush(_backColor))
                                g.FillRectangle(drawBrush, new Rectangle(drawRect.Left, drawRect.Top,
                                                                         drawRect.Width, drawRect.Height));
                            break;
                    }

                }

                // Always draw the expansion bitmap
                g.DrawImage(_menuImages.Images[(int)ImageIndex.Expansion], xPos, yPos);
            }
            else
            {
                // Is this item a separator?
                if (dc.Separator)
                {
                    if (dc.VerticalSeparator)
                    {
                        switch(_style)
                        {
                            case VisualStyle.IDE:
                                // Draw the separator as a single line
                                using (Pen separatorPen = new Pen(ControlPaint.Dark(_backColor)))
                                    g.DrawLine(separatorPen, drawRect.Left, drawRect.Top, drawRect.Left, drawRect.Bottom);
                                break;
                            case VisualStyle.Plain:
                                ButtonBorderStyle bsInset = ButtonBorderStyle.Inset;
                                ButtonBorderStyle bsNone = ButtonBorderStyle.Inset;
							
                                Rectangle sepRect = new Rectangle(drawRect.Left + 1, drawRect.Top, 2, drawRect.Height);
							
                                // Draw the separator as two lines using Inset style
                                ControlPaint.DrawBorder(g, sepRect, 
                                                        _backColor, 1, bsInset, _backColor, 0, bsNone,
                                                        _backColor, 1, bsInset, _backColor, 0, bsNone);
                                break;
                        }
                    }
                    else
                    {
                        switch(_style)
                        {
                            case VisualStyle.IDE:
                                // Draw the image column background
                                Rectangle imageCol = new Rectangle(drawRect.Left, drawRect.Top, imageColWidth, drawRect.Height);

                                g.FillRectangle(Brushes.White, imageCol);
                                g.FillRectangle(_controlLBrush, imageCol);

                                // Draw a separator
                                using (Pen separatorPen = new Pen(Color.FromArgb(75, _textColor)))
                                {
                                    // Draw the separator as a single line
                                    g.DrawLine(separatorPen, 
                                               drawRect.Left + imageColWidth + textGapLeft, drawRect.Top + 2, 
                                               drawRect.Right, 
                                               drawRect.Top + 2);
                                }
                                break;
                            case VisualStyle.Plain:
                                if (dc.Infrequent && _highlightInfrequent)
                                {
                                    // Change background to be a lighter shade
                                    using(Brush drawBrush = new SolidBrush(ControlPaint.Light(_backColor)))
                                        g.FillRectangle(drawBrush, drawRect);
                                }
                                    
                                ButtonBorderStyle bsInset = ButtonBorderStyle.Inset;
                                ButtonBorderStyle bsNone = ButtonBorderStyle.Inset;
							
                                Rectangle sepRect = new Rectangle(drawRect.Left + 2, drawRect.Top + 2, drawRect.Width - 4, 2);
							
                                // Draw the separator as two lines using Inset style
                                ControlPaint.DrawBorder(g, sepRect, 
                                                        _backColor, 0, bsNone, _backColor, 1, bsInset,
                                                        _backColor, 0, bsNone, _backColor, 1, bsInset);
                                break;
                        }
                    }
                }
                else
                {
                    int leftPos = drawRect.Left + imageColWidth + textGapLeft;

                    // Should the command be drawn selected?
                    if (hotCommand)
                    {
                        switch(_style)
                        {
                            case VisualStyle.IDE:
                                Rectangle selectArea = new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 3, drawRect.Height - 1);

                                using (Pen selectPen = new Pen(_highlightColorDark))
                                {
                                    // Draw the selection area white, because we are going to use an alpha brush
                                    using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                                        g.FillRectangle(whiteBrush, selectArea);

                                    using (SolidBrush selectBrush = new SolidBrush(_highlightColorLight))
                                    {
                                        // Draw the selection area
                                        g.FillRectangle(selectBrush, selectArea);

                                        // Draw a border around the selection area
                                        g.DrawRectangle(selectPen, selectArea);
                                    }
                                }
                                break;
                            case VisualStyle.Plain:
                                using (SolidBrush selectBrush = new SolidBrush(_highlightColorDark))
                                    g.FillRectangle(selectBrush, drawRect);
                                break;
                        }
                    }
                    else
                    {
                        switch(_style)
                        {
                            case VisualStyle.IDE:
                                // Fill the entire drawing area with ControlLightLight
                                g.FillRectangle(_controlLLBrush, new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 1, drawRect.Height));

                                if (dc.Infrequent && _highlightInfrequent)
                                {
                                    // Draw the text area in a darker shade
                                    g.FillRectangle(Brushes.White, new Rectangle(leftPos, drawRect.Top, drawRect.Right - leftPos - textGapLeft, drawRect.Height));
                                    g.FillRectangle(_controlEBrush, new Rectangle(leftPos, drawRect.Top, drawRect.Right - leftPos - textGapLeft, drawRect.Height));
                                }

                                Rectangle imageCol = new Rectangle(drawRect.Left, drawRect.Top,imageColWidth, drawRect.Height);

                                // Draw the image column background
                                g.FillRectangle(Brushes.White, imageCol);
                                g.FillRectangle(_controlLBrush, imageCol);
                                break;
                            case VisualStyle.Plain:
                                if (dc.Infrequent && _highlightInfrequent)
                                {
                                    using(Brush drawBrush = new SolidBrush(ControlPaint.Light(_backColor)))
                                        g.FillRectangle(drawBrush, new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width, drawRect.Height));
                                }
                                else                                
                                {
                                    using(Brush drawBrush = new SolidBrush(_backColor))
                                        g.FillRectangle(drawBrush, new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width, drawRect.Height));
                                }
                                
                                if (dc.TopBorder && _highlightInfrequent)
                                    using(Pen drawPen = new Pen(ControlPaint.Dark(_backColor)))
                                        g.DrawLine(drawPen, drawRect.Left, drawRect.Top, drawRect.Right, drawRect.Top);
                                
                                if (dc.BottomBorder && _highlightInfrequent)
                                    using(Pen drawPen = new Pen(ControlPaint.LightLight(_backColor)))
                                        g.DrawLine(drawPen, drawRect.Left, drawRect.Bottom - 1, drawRect.Right, drawRect.Bottom - 1);
                                break;
                        }
                    }

                    // Calculate text drawing rectangle
                    Rectangle strRect = new Rectangle(leftPos, drawRect.Top, shortCutX - leftPos, drawRect.Height);

                    // Left align the text drawing on a single line centered vertically
                    // and process the & character to be shown as an underscore on next character
                    StringFormat format = new StringFormat();
                    format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.HotkeyPrefix = HotkeyPrefix.Show;

                    SolidBrush textBrush;

                    // Create brush depending on enabled state
                    if (mc.Enabled)
                    {
                        if (!hotCommand || (_style == VisualStyle.IDE))
                            textBrush = new SolidBrush(_textColor);
                        else
                            textBrush = new SolidBrush(_highlightTextColor);
                    }
                    else 
                        textBrush = new SolidBrush(SystemColors.GrayText);

                    // Helper values used when drawing grayed text in plain style
                    Rectangle rectDownRight = strRect;
                    rectDownRight.Offset(1,1);

                    if (mc.Enabled || (_style == VisualStyle.IDE))
                        g.DrawString(mc.Text, _textFont, textBrush, strRect, format);
                    else
                    {
                        if (_style == VisualStyle.Plain)
                        {
                            // Draw grayed text by drawing white string offset down and right
                            using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                                g.DrawString(mc.Text, _textFont, whiteBrush, rectDownRight, format);
                        }

                        // And then draw in correct color offset up and left
                        g.DrawString(mc.Text, _textFont, textBrush, strRect, format);
                    }

                    if (mc.Shortcut != Shortcut.None)
                    {
                        // Right align the shortcut drawing
                        format.Alignment = StringAlignment.Far;

                        if (mc.Enabled || (_style == VisualStyle.IDE))
                        {
                            // Draw the shortcut text 
                            g.DrawString(GetShortcutText(mc.Shortcut), _textFont, textBrush, strRect, format);
                        }
                        else
                        {
                            if (_style == VisualStyle.Plain)
                            {
                                // Draw grayed text by drawing white string offset down and right
                                using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                                    g.DrawString(GetShortcutText(mc.Shortcut), _textFont, whiteBrush, rectDownRight, format);
                            }

                            // And then draw in corret color offset up and left
                            g.DrawString(GetShortcutText(mc.Shortcut), _textFont, textBrush, strRect, format);
                        }
                    }

                    // The image offset from top of cell is half the space left after
                    // subtracting the height of the image from the cell height
                    int imageTop = drawRect.Top + (drawRect.Height - _imageHeight) / 2;

                    Image image = null;

                    // Should a check mark be drawn?
                    if (mc.Checked)
                    {
                        switch(_style)
                        {
                            case VisualStyle.IDE:
                                Pen boxPen;
                                Brush boxBrush;

                                if (mc.Enabled)
                                {
                                    boxPen = new Pen(_highlightColorDark);
                                    boxBrush = new SolidBrush(_highlightColorLightLight);
                                }
                                else
                                {
                                    boxPen = new Pen(SystemColors.GrayText);
                                    boxBrush = new SolidBrush(Color.FromArgb(20, SystemColors.GrayText));
                                }

                                Rectangle boxRect = new Rectangle(imageLeft - 1, imageTop - 1, _imageHeight + 2, _imageWidth + 2);

                                // Fill the checkbox area very slightly
                                g.FillRectangle(boxBrush, boxRect);

                                // Draw the box around the checkmark area
                                g.DrawRectangle(boxPen, boxRect);

                                boxPen.Dispose();
                                boxBrush.Dispose();
                                break;
                            case VisualStyle.Plain:
                                break;
                        }

                        // Grab either tick or radio button image
                        if (mc.RadioCheck)
                        {
                            if (hotCommand && (_style == VisualStyle.Plain))
                                image = _menuImages.Images[(int)ImageIndex.RadioSelected];
                            else
                                image = _menuImages.Images[(int)ImageIndex.Radio];
                        }
                        else
                        {
                            if (hotCommand && (_style == VisualStyle.Plain))
                                image = _menuImages.Images[(int)ImageIndex.CheckSelected];
                            else
                                image = _menuImages.Images[(int)ImageIndex.Check];
                        }
                    }
                    else
                    {
                        try
                        {
                            // Always use the Image property in preference to the ImageList
                            if (mc.Image != null)
                                image = mc.Image;
                            else
                            {
                                if ((mc.ImageList != null) && (mc.ImageIndex >= 0))
                                    image = mc.ImageList.Images[mc.ImageIndex];
                            }
                        }
                        catch(Exception)
                        {
                            // User supplied ImageList/ImageIndex are invalid, use an error image instead
                            image = _menuImages.Images[(int)ImageIndex.ImageError];
                        }
                    }

					// Is there an image to be drawn?
					if (image != null)
					{
						if (mc.Enabled)
						{
							if ((hotCommand) && (!mc.Checked) && (_style == VisualStyle.IDE))
							{
								// Draw a gray icon offset down and right
								Bitmap shadowImage = new Bitmap((Bitmap) image);
								Color shadowColor = Color.FromArgb(154, 156, 146);
								Color transparent = Color.FromArgb(0, 0, 0, 0);

								for(int pixelX = 0; pixelX < image.Width; pixelX++)
								{
									for(int pixelY = 0; pixelY < image.Height; pixelY++)
									{
										if (shadowImage.GetPixel(pixelX, pixelY) != transparent)
											shadowImage.SetPixel(pixelX, pixelY, shadowColor);
									}
								}
		
								g.DrawImage(shadowImage, imageLeft + 1, imageTop + 1);

								// Draw an enabled icon offset up and left
								g.DrawImage(image, imageLeft - 1, imageTop - 1);
							}
							else
							{
								// Draw an faded enabled icon
								// A new bitmap so we don't change the actual image
								Bitmap fadedImage = new Bitmap(image);
								Color transparent = Color.FromArgb(0, 0, 0, 0);

								for(int pixelX = 0; pixelX < image.Width; pixelX++)
								{
									for(int pixelY = 0; pixelY < image.Height; pixelY++)
									{
										Color pixelColor = fadedImage.GetPixel(pixelX, pixelY);
										if (pixelColor != transparent) 
										{
											Color newPixelColor = Color.FromArgb((pixelColor.R + 76) - (((pixelColor.R + 32) / 64) * 19), 			
																				 (pixelColor.G + 76) - (((pixelColor.G + 32) / 64) * 19), 
																				 (pixelColor.B + 76) - (((pixelColor.B + 32) / 64) * 19));
									
											fadedImage.SetPixel(pixelX, pixelY, newPixelColor);
										}
									}
								}

								g.DrawImage(fadedImage, imageLeft, imageTop);

								// Draw an enabled icon
								//g.DrawImage(image, imageLeft, imageTop);
							}
						}
						else
						{
							// Draw a image disabled
							ControlPaint.DrawImageDisabled(g, image, imageLeft, imageTop, SystemColors.HighlightText);
						}
					}    

                    // Does the menu have a submenu defined?
                    if (dc.SubMenu)
                    {
                        // Is the item enabled?
                        if (mc.Enabled)
                        {
                            int subMenuIndex = (int)ImageIndex.SubMenu;

                            if (hotCommand && (_style == VisualStyle.Plain))
                                subMenuIndex = (int)ImageIndex.SubMenuSelected;

                            // Draw the submenu arrow 
                            g.DrawImage(_menuImages.Images[subMenuIndex], subMenuX, imageTop);
                        }
                        else
                        {
                            // Draw a image disabled
                            ControlPaint.DrawImageDisabled(g, _menuImages.Images[(int)ImageIndex.SubMenu], 
                                                           subMenuX, imageTop, _highlightTextColor);
                        }
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

        protected string GetShortcutText(Shortcut shortcut)
        {
            // Get the key code
            char keycode = (char)((int)shortcut & 0x0000FFFF);

            // The type converter does not work for numeric values as it returns
            // Alt+D0 instad of Alt+0. So check for numeric keys and construct the
            // return string ourself.
            if ((keycode >= '0') && (keycode <= '9'))
            {
                string display = "";

                // Get the modifier
                int modifier = (int)((int)shortcut & 0xFFFF0000);
		
                if ((modifier & 0x00010000) != 0)
                    display += "Shift+";

                if ((modifier & 0x00020000) != 0)
                    display += "Ctrl+";
				
                if ((modifier & 0x00040000) != 0)
                    display += "Alt+";

                display += keycode;

                return display;
            }
			else
			{
				return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys)shortcut);
			}
        }

        protected bool ProcessKeyUp()
        {
            int newItem = _trackItem;
            int startItem = newItem;

            for(int i=0; i<_drawCommands.Count; i++)
            {
                // Move to previous item
                newItem--;

                // Have we looped all the way around all the choices
                if (newItem == startItem)
                    return false;

                // Check limits
                if (newItem < 0)
                    newItem = _drawCommands.Count - 1;

                DrawCommand dc = _drawCommands[newItem] as DrawCommand;

                // Can we select this item?
                if (!dc.Separator && dc.Enabled)
                {
                    // If a change has occured
                    if (newItem != _trackItem)
                    {
                        // Modify the display of the two items 
                        SwitchSelection(_trackItem, newItem, false, false);

                        return true;
                    }
                }
            }

            return false;
        }

        protected bool ProcessKeyDown()
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
                if (!dc.Separator && dc.Enabled)
                {
                    // If a change has occured
                    if (newItem != _trackItem)
                    {
                        // Modify the display of the two items 
                        SwitchSelection(_trackItem, newItem, false, false);

                        return true;
                    }
                }
            }

            return false;
        }

        protected void ProcessKeyLeft()
        {
            // Are we the first submenu of a parent control?
            bool autoLeft = (_parentMenu != null) || (_parentControl != null);
            bool checkKeys = false;

            if (_trackItem != -1)
            {
                // Get the col this item is in
                DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;
	
                // Grab the current column/row values
                int col = dc.Col;
                int row = dc.Row;

                // If not in the first column then move left one
                if (col > 0)
                {
                    int newItem = -1;
                    int newRow = -1;
                    int findCol = col - 1;
                    DrawCommand newDc = null;

                    for(int i=0; i<_drawCommands.Count; i++)
                    {
                        DrawCommand listDc = _drawCommands[i] as DrawCommand;
					
                        // Interested in cells in the required column
                        if (listDc.Col == findCol)
                        {
                            // Is this Row nearer to the one required than those found so far?							
                            if ((listDc.Row <= row) && (listDc.Row > newRow) && 
                                !listDc.Separator && listDc.Enabled)
                            {
                                // Remember this item
                                newRow = listDc.Row;
                                newDc = listDc;
                                newItem = i;
                            }
                        }
                    }

                    if (newDc != null)
                    {
                        // Track the new item
                        // Modify the display of the two items 
                        SwitchSelection(_trackItem, newItem, false, false);
                    }
                    else
                        checkKeys = true;
                }
                else
                    checkKeys = true;
            }
            else
            {
                if (_parentMenu != null)
                {
                    if (!ProcessKeyUp())
                        checkKeys = true;
                }
                else
                    checkKeys = true;
            }

            // If we have a parent control and nothing to move right into
            if (autoLeft && checkKeys)
            {
                _returnCommand = null;

                // Finish processing messages
                _timer.Stop();
                _exitLoop = true;	

                // Only a top level PopupMenu should cause the MenuControl to select the 
                // next left menu command. A submenu should just become dismissed.
                if (_parentMenu == null)
                    _returnDir = -1;
            }
        }

        protected bool ProcessKeyRight()
        {
            // Are we the first submenu of a parent control?
            bool autoRight = (_parentControl != null);
            bool checkKeys = false;
            bool ret = false;

            // Is an item currently selected?
            if (_trackItem != -1)
            {
	            DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                // Does this item have a submenu?
                if (dc.SubMenu)
                {
                    // Handle the submenu
                    OperateSubMenu(_trackItem, true);

                    ret = true;
                }
                else
                {
                    // Grab the current column/row values
                    int col = dc.Col;
                    int row = dc.Row;

                    // If not in the first column then move left one
                    int newItem = -1;
                    int newRow = -1;
                    int findCol = col + 1;
                    DrawCommand newDc = null;

                    for(int i=0; i<_drawCommands.Count; i++)
                    {
                        DrawCommand listDc = _drawCommands[i] as DrawCommand;
					
                        // Interesting in cells in the required column
                        if (listDc.Col == findCol)
                        {
                            // Is this Row nearer to the one required than those found so far?							
                            if ((listDc.Row <= row) && (listDc.Row > newRow) && 
                                !listDc.Separator && listDc.Enabled)
                            {
                                // Remember this item
                                newRow = listDc.Row;
                                newDc = listDc;
                                newItem = i;
                            }
                        }
                    }

                    if (newDc != null)
                    {
                        // Track the new item
                        // Modify the display of the two items 
                        SwitchSelection(_trackItem, newItem, false, false);
                    }
                    else
                        checkKeys = true;
                }
            }
            else
            {
                if (_parentMenu != null)
                {
                    if (!ProcessKeyDown())
                        checkKeys = true;
                }
                else
                    checkKeys = true;
            }

            // If we have a parent control and nothing to move right into
            if (autoRight && checkKeys)
            {
                _returnCommand = null;

                // Finish processing messages
                _timer.Stop();
                _exitLoop = true;	

                _returnDir = 1;
            }

            return ret;
        }

        protected int ProcessMnemonicKey(char key)
        {
            // Check against each draw command mnemonic
            for(int i=0; i<_drawCommands.Count; i++)
            {
                DrawCommand dc = _drawCommands[i] as DrawCommand;

                if (dc.Enabled)
                {
                    // Does the character match?
                    if (key == dc.Mnemonic)
					{
						// Does this have any submenu?
						if (dc.SubMenu)
						{
							// Is there a change in selected item?
							if (_trackItem != i)
							{
								// Modify the display of the two items 
								SwitchSelection(_trackItem, i, true, false);
							}

							return -1;
						}
						else
						{
							// No submenu so just selected the item which
							// will cause the PopupMenu to exit
							return i;
						}
					}
                }
            }

            // No match found
            return -1;
        }

        protected Win32.POINT MousePositionToScreen(Win32.MSG msg)
        {
            Win32.POINT screenPos;
            screenPos.x = (short)((uint)msg.lParam & 0x0000FFFFU);
            screenPos.y = (short)(((uint)msg.lParam & 0xFFFF0000U) >> 16);

            // Convert the mouse position to screen coordinates
            User32.ClientToScreen(msg.hwnd, ref screenPos);

            return screenPos;
        }

        protected bool ParentControlWantsMouseMessage(Win32.POINT screenPos, ref Win32.MSG msg)
        {
            // Special case the MOUSEMOVE so if we are part of a MenuControl
            // then we should let the MenuControl process that message
            if ((msg.message == (int)Win32.Msgs.WM_MOUSEMOVE) && (_parentControl != null))
            {
                Win32.RECT rectRaw = new Win32.RECT();

                // Grab the screen rectangle of the parent control
                User32.GetWindowRect(_parentControl.Handle, ref rectRaw);
				
                if ((screenPos.x >= rectRaw.left) && (screenPos.x <= rectRaw.right) &&
                    (screenPos.y >= rectRaw.top) && (screenPos.y <= rectRaw.bottom))
                    return true;
            }

            return false;
        }
				
        internal PopupMenu ParentPopupMenuWantsMouseMessage(Win32.POINT screenPos, ref Win32.MSG msg)
        {
            if (_parentMenu != null)
                return _parentMenu.WantMouseMessage(screenPos);

            return null;
        }

        protected PopupMenu WantMouseMessage(Win32.POINT screenPos)
        {
            Win32.RECT rectRaw = new Win32.RECT();

            // Grab the screen rectangle of the window
            User32.GetWindowRect(this.Handle, ref rectRaw);

            if ((screenPos.x >= rectRaw.left) && (screenPos.x <= rectRaw.right) &&
                (screenPos.y >= rectRaw.top) && (screenPos.y <= rectRaw.bottom))
                return this;

            if (_parentMenu != null)
                return _parentMenu.WantMouseMessage(screenPos);

            return null;
        }

        protected void SwitchSelection(int oldItem, int newItem, bool mouseChange, bool reverting)
        {
            bool updateWindow = false;

            // Create a graphics object for drawing with
            using(Graphics g = Graphics.FromHwnd(this.Handle))
            {
                // Deselect the old draw command
                if (oldItem != -1)
                {
                    DrawCommand dc = _drawCommands[oldItem] as DrawCommand;

                    // Draw old item not selected
                    if (_layered)
                        updateWindow = true;
                    else
                        DrawSingleCommand(g, _drawCommands[oldItem] as DrawCommand, false);

                    // Generate an unselect event
                    if (dc.MenuCommand != null)
                        OnDeselected(dc.MenuCommand);
                }
				
                if (newItem != -1)
                {
                    // Stop the timer as a new selection has occured
                    _timer.Stop();

                    // Do we have a child menu?
                    if (!reverting && (_childMenu != null))
                    {
                        // Start timer to test if it should be dismissed
			            _timer.Interval = _selectionDelay;
                        _timer.Start();
                    }

                    DrawCommand dc = _drawCommands[newItem] as DrawCommand;

                    // Select the new draw command
                    if (!dc.Separator && dc.Enabled)
                    {
                        // Draw the newly selected item
                        if (_layered)
                            updateWindow = true;
                        else
                            DrawSingleCommand(g, dc, true);

                        // Only is mouse movement caused the selection change...
                        if (!reverting && mouseChange)
                        {
                            //...should we start a timer to test for sub menu displaying
							if (dc.Expansion)
								_timer.Interval = _expansionDelay;
							else
								_timer.Interval = _selectionDelay;

                            _timer.Start();
                        }

                        // Generate an unselect event
                        if (dc.MenuCommand != null)
                            OnSelected(dc.MenuCommand);
                    }
                    else
                    {
                        // Cannot become selected
                        newItem = -1;
                    }
                }
	
                // Remember the new selection
                _trackItem = newItem;
				
                if (_layered && updateWindow)
                {
                    // Update the image for display
                    UpdateLayeredWindow();
                }
            }
        }

        protected void OnTimerExpire(object sender, EventArgs e)
        {
            // Prevent it expiring again
            _timer.Stop();

            bool showPopup = true;

            // Is a popup menu already being displayed?
            if (_childMenu != null)
            {
                // If the submenu popup is for a different item?
                if (_popupItem != _trackItem)
                {
                    // Then need to kill the submenu
                    User32.PostMessage(_childMenu.Handle, WM_DISMISS, 0, 0); 
                }
                else
                    showPopup = false;
            }

            // Should we show the popup for this item
            if (showPopup)
            {
                // Check an item really is selected
                if (_trackItem != -1)
                {
                    DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                    // Does this item have a submenu?
                    if (dc.SubMenu)
                        OperateSubMenu(_trackItem, false);
                    else
                    {
                        if (dc.Expansion)
                            RegenerateExpansion();
                    }
                }
            }
        }

        protected void OperateSubMenu(int popupItem, bool selectFirst)
		{
			User32.PostMessage(this.Handle, WM_OPERATE_SUBMENU, (uint)popupItem, (uint)(selectFirst ? 1 : 0));
		}

        protected void OnWM_OPERATE_SUBMENU(ref Message m)
        {
			int popupItem = (int)m.WParam;
			bool selectFirst = (m.LParam != IntPtr.Zero);

            _popupItem = popupItem;
            _childMenu = new PopupMenu();

            DrawCommand dc = _drawCommands[popupItem] as DrawCommand;

            // Find screen coordinate of Top right of item cell
            Win32.POINT screenPosTR;
            screenPosTR.x = dc.DrawRect.Right;
            screenPosTR.y = dc.DrawRect.Top;
            User32.ClientToScreen(this.Handle, ref screenPosTR);
			
            // Find screen coordinate of top left of item cell
            Win32.POINT screenPosTL;
            screenPosTL.x = dc.DrawRect.Left;
            screenPosTL.y = dc.DrawRect.Top;
            User32.ClientToScreen(this.Handle, ref screenPosTL);

            // Ensure the child has the same properties as ourself
            _childMenu.Style = this.Style;
            _childMenu.Font = this.Font;
            _childMenu.BackColor = this.BackColor;
            _childMenu.TextColor = this.TextColor;
            _childMenu.HighlightTextColor = this.HighlightTextColor;
            _childMenu.HighlightColor = this.HighlightColor;
			_childMenu.Animate = this.Animate;
			_childMenu.AnimateStyle = this.AnimateStyle;
			_childMenu.AnimateTime = this.AnimateTime;
			
            // Record keyboard direction
            int returnDir = 0;

            // Propogate the remembering of expansion state
            _childMenu.RememberExpansion = _rememberExpansion;

            // Honour the collections request for showing infrequent items
            _childMenu._showInfrequent = dc.MenuCommand.MenuCommands.ShowInfrequent;

            // Propogate the highlight property
            _childMenu.HighlightInfrequent = _highlightInfrequent;
            
            // Generate event so that caller has chance to modify MenuCommand contents
            dc.MenuCommand.OnPopupStart();
            
            _returnCommand = _childMenu.InternalTrackPopup(new Point(screenPosTR.x, screenPosTR.y),
                                                           new Point(screenPosTL.x, screenPosTL.y), 
                                                           dc.MenuCommand.MenuCommands, 
                                                           this, 
                                                           selectFirst,
                                                           _parentControl,
                                                           _popupRight,
                                                           _popupDown,
                                                           _animateFirst,
                                                           ref returnDir);

            // Generate event so that caller has chance to modify MenuCommand contents
            dc.MenuCommand.OnPopupEnd();
            
            _popupItem = -1;;
            _childMenu = null;

            // Subsequent times a submenu is shown we do not want it to animate
            _animateFirst = false;

            if ((_returnCommand != null) || (returnDir != 0))
            {
                // Finish processing messages
                _timer.Stop();
                _exitLoop = true;	
                _returnDir = returnDir;
            }
        }
        
        public virtual void OnSelected(MenuCommand mc)
        {
            // If initiated by a MenuControl item then let the control handle this
            if (_parentControl != null)
                _parentControl.OnSelected(mc);
            else
            {
                // If we have an event defined then fire it
                if (Selected != null)
                    Selected(mc);
                else
                {
                    // Maybe our parent has an event defined instead
                    if (_parentMenu != null)
                        _parentMenu.OnSelected(mc);
                }
            }
        }

        public virtual void OnDeselected(MenuCommand mc)
        {
            // If initiated by a MenuControl item then let the control handle this
            if (_parentControl != null)
                _parentControl.OnDeselected(mc);
            else
            {
                // If we have an event defined then fire it
                if (Deselected != null)
                    Deselected(mc);
                else
                {
                    // Maybe our parent has an event defined instead
                    if (_parentMenu != null)
                        _parentMenu.OnDeselected(mc);
                }
            }
        }

        protected void OnWM_PAINT(ref Message m)
        {
            Win32.PAINTSTRUCT ps = new Win32.PAINTSTRUCT();

            // Have to call BeginPaint whenever processing a WM_PAINT message
            IntPtr hDC = User32.BeginPaint(m.HWnd, ref ps);

            Win32.RECT rectRaw = new Win32.RECT();

            // Grab the screen rectangle of the window
            User32.GetWindowRect(this.Handle, ref rectRaw);

            // Convert to a client size rectangle
            Rectangle rectWin = new Rectangle(0, 0, 
                                              rectRaw.right - rectRaw.left, 
                                              rectRaw.bottom - rectRaw.top);

            // Create a graphics object for drawing
            using(Graphics g = Graphics.FromHdc(hDC))
            {
                // Create bitmap for drawing onto
                Bitmap memoryBitmap = new Bitmap(rectWin.Width, rectWin.Height);

                using(Graphics h = Graphics.FromImage(memoryBitmap))
                {
                    // Draw the background area
                    DrawBackground(h, rectWin);

                    // Draw the actual menu items
                    DrawAllCommands(h);
                }

                // Blit bitmap onto the screen
                g.DrawImageUnscaled(memoryBitmap, 0, 0);
            }

            // Don't forget to end the paint operation!
            User32.EndPaint(m.HWnd, ref ps);
        }

        protected void OnWM_ACTIVATEAPP(ref Message m)
        {
            // Another application has been activated, so we need to kill ourself	
            _timer.Stop();
            _exitLoop = true;
        }

        protected void SubMenuMovement()
        {
            // Cancel timer to prevent auto closing of an open submenu
            _timer.Stop();

            // Has the selected item changed since child menu shown?
            if (_popupItem != _trackItem)
            {
                // Need to put it back again
                SwitchSelection(_trackItem, _popupItem, false, true);
            }

            // Are we a submenu?
            if (_parentMenu != null)
            {
                // Inform parent that we have movement and so do not
                // use a timer to close us up
                _parentMenu.SubMenuMovement();
            }
        }

        protected void OnWM_MOUSEMOVE(int xPos, int yPos)
        {
            // Convert from screen to client coordinates
            xPos -= _currentPoint.X;
            yPos -= _currentPoint.Y;

            // Are we a submenu?
            if (_parentMenu != null)
            {
                // Inform parent that we have movement and so do not
                // use a timer to close us up
                _parentMenu.SubMenuMovement();
            }

            // Yes, we know the mouse is over window
            _mouseOver = true;

            Point pos = new Point(xPos, yPos);

            // Has mouse position really changed since last time?
            if (_lastMousePos != pos)
            {
                for(int i=0; i<_drawCommands.Count; i++)
                {
                    DrawCommand dc = _drawCommands[i] as DrawCommand;

                    if (dc.DrawRect.Contains(pos))
                    {
                        // Is there a change in selected item?
                        if (_trackItem != i)
                        {
                            // Modify the display of the two items 
                            SwitchSelection(_trackItem, i, true, false);
                        }
                    }
                }

                // Remember for next time around
                _lastMousePos = pos;
            }
        }

        protected void OnWM_MOUSELEAVE()
        {
            // Deselect the old draw command if not showing a child menu
            if ((_trackItem != -1) && (_childMenu == null))
            {
                // Modify the display of the two items 
                SwitchSelection(_trackItem, -1, false, false);
            }

            // Reset flag so that next mouse move start monitor for mouse leave message
            _mouseOver = false;

            // No point having a last mouse position
            _lastMousePos = new Point(-1,-1);
        }

        protected void OnWM_YBUTTONUP(int xPos, int yPos)
        {
            // Convert from screen to client coordinates
            xPos -= _currentPoint.X;
            yPos -= _currentPoint.Y;

            Point pos = new Point(xPos, yPos);

            for(int i=0; i<_drawCommands.Count; i++)
            {
                DrawCommand dc = _drawCommands[i] as DrawCommand;

                if (dc.DrawRect.Contains(pos))
                {
                    // Is there a change in selected item?
                    if (_trackItem != i)
                    {
                        // Modify the display of the two items 
                        SwitchSelection(_trackItem, i, false, false);
                    }
                }
            }

            // Is an item selected?			
            if (_trackItem != -1)
            {
                DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

                // Does this item have a submenu?
                if (dc.SubMenu)
                {
                    // If we are not already showing this submenu...
                    if (_popupItem != _trackItem)
                    {
                        // Is a submenu for a different item showing?
                        if (_childMenu != null)
                        {
                            // Inform the child menu it is no longer needed
                            User32.PostMessage(_childMenu.Handle, WM_DISMISS, 0, 0); 
                        }

                        // Handle the submenu
                        OperateSubMenu(_trackItem, false);
                    }
                }
                else
                {
                    if (dc.Expansion)
                        RegenerateExpansion();
                    else
                    {
                        // Kill any child menus open
                        if (_childMenu != null)
                        {
                            // Inform the child menu it is no longer needed
                            User32.PostMessage(_childMenu.Handle, WM_DISMISS, 0, 0); 
                        }

                        // Define the selection to return to caller
                        _returnCommand = dc.MenuCommand;

                        // Finish processing messages
                        _timer.Stop();
                        _exitLoop = true;	
                    }
                }
            }
        }

        protected void OnWM_MOUSEACTIVATE(ref Message m)
        {
            // Do not allow the mouse down to activate the window, but eat 
            // the message as we still want the mouse down for processing
            m.Result = (IntPtr)Win32.MouseActivateFlags.MA_NOACTIVATE;
        }

        protected void OnWM_SETCURSOR()
        {
            // Always use the arrow cursor
            User32.SetCursor(User32.LoadCursor(IntPtr.Zero, (uint)Win32.Cursors.IDC_ARROW));
        }
		
        protected void OnWM_DISMISS()
        {	
            // Pass on to any child menu of ours
            if (_childMenu != null)
            {
                // Inform the child menu it is no longer needed
                User32.PostMessage(_childMenu.Handle, WM_DISMISS, 0, 0); 
            }
			
            // Define the selection to return to caller
            _returnCommand = null;

            // Finish processing messages
            _timer.Stop();
            _exitLoop = true;	

			// Hide ourself
            HideMenuWindow();

            // Kill ourself
            DestroyHandle();
        }

        protected bool OnWM_NCHITTEST(ref Message m)
        {
			// Get mouse coordinates
            Win32.POINT screenPos;
            screenPos.x = (short)((uint)m.LParam & 0x0000FFFFU);
            screenPos.y = (short)(((uint)m.LParam & 0xFFFF0000U) >> 16);

			// Only the IDE style has shadows
			if (_style == VisualStyle.IDE)
			{	
				Win32.POINT popupPos;
				popupPos.x = _currentSize.Width - _position[(int)_style, (int)PI.ShadowWidth];
				popupPos.y = _currentSize.Height - _position[(int)_style, (int)PI.ShadowHeight];

				// Convert the mouse position to screen coordinates
				User32.ClientToScreen(this.Handle, ref popupPos);

				// Is the mouse in the shadow areas?
				if ((screenPos.x > popupPos.x) ||
				    (screenPos.y > popupPos.y))
				{
					// Allow actions to occur to window beneath us
					m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;

					return true;
				}
			}

			return false;
		}

        protected override void WndProc(ref Message m)
        {
//			Console.WriteLine("WndProc(PopupMenu) {0} {1}", this.Handle, ((Win32.Msgs)m.Msg).ToString());

            // WM_DISMISS is not a constant and so cannot be in a switch
            if (m.Msg == WM_DISMISS)
                OnWM_DISMISS();
			else if (m.Msg == WM_OPERATE_SUBMENU)
				OnWM_OPERATE_SUBMENU(ref m);
            else
            {
                // Want to notice when the window is maximized
                switch(m.Msg)
                {
                    case (int)Win32.Msgs.WM_ACTIVATEAPP:
                        OnWM_ACTIVATEAPP(ref m);
                        break;
                    case (int)Win32.Msgs.WM_MOUSEACTIVATE:
                        OnWM_MOUSEACTIVATE(ref m);
                        break;
                    case (int)Win32.Msgs.WM_PAINT:
                        OnWM_PAINT(ref m);
                        break;
					case (int)Win32.Msgs.WM_SETCURSOR:
						OnWM_SETCURSOR();
						break;
					case (int)Win32.Msgs.WM_NCHITTEST:
						if (!OnWM_NCHITTEST(ref m))
	                        base.WndProc(ref m);
						break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
        }

        protected static Bitmap GetShadowCache(int width, int height)
        {
            // Do we already have a cached bitmap of the correct size?
            if ((_shadowCacheWidth == width) && (_shadowCacheHeight == height) && (_shadowCache != null))
                return _shadowCache;

            // Dispose of any previously cached bitmap	
            if (_shadowCache != null)
                _shadowCache.Dispose();
	
	        // Create our new bitmap with 32bpp so we have an alpha channel
            Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            
            // We want direct access to the bits so we can change values
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				
            unsafe
            {
                // Direct pointer to first line
                uint* pixptr = (uint*)(data.Scan0);
					
			    // For each row
                for (int y = 0; y < height; y++)
                {
                    int offset = data.Stride * y / 4;
                    
                    // Fade each line as we go down
                    int alphay = 64 * (height - y) / (height + 1);
						
				    // For each column pixel
                    for (int x = 0; x < width; x++)
                    {
                        // Fade each pixel as we go across
                        int alphax = alphay * (width - x) / (width + 1);
                        pixptr[offset+x] = (uint)alphax << 24;
                    }
                }
            }
	
            image.UnlockBits(data);

            // Cache values for next time around	
            _shadowCache = image;
            _shadowCacheWidth = width;
            _shadowCacheHeight = height;
	
            return _shadowCache;
        }
    }
}

