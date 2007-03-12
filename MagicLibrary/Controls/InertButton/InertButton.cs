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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;

namespace Crownwood.Magic.Controls
{
    [ToolboxBitmap(typeof(InertButton))]
    [DefaultProperty("PopupStyle")]
    public class InertButton : Control
    {
        // Instance fields
        protected int _borderWidth;
        protected bool _mouseOver;
        protected bool _mouseCapture;
        protected bool _popupStyle;
        protected int _imageIndexEnabled;
        protected int _imageIndexDisabled;
        protected ImageList _imageList;
        protected ImageAttributes _imageAttr;
        protected MouseButtons _mouseButton;
		
        public InertButton()
        {
            InternalConstruct(null, -1, -1, null);
        }

        public InertButton(ImageList imageList, int imageIndexEnabled)
        {
            InternalConstruct(imageList, imageIndexEnabled, -1, null);
        }

        public InertButton(ImageList imageList, int imageIndexEnabled, int imageIndexDisabled)
        {
            InternalConstruct(imageList, imageIndexEnabled, imageIndexDisabled, null);
        }

        public InertButton(ImageList imageList, int imageIndexEnabled, int imageIndexDisabled, ImageAttributes imageAttr)
        {
            InternalConstruct(imageList, imageIndexEnabled, imageIndexDisabled, imageAttr);
        }
		
        public void InternalConstruct(ImageList imageList, 
                                      int imageIndexEnabled, 
                                      int imageIndexDisabled, 
                                      ImageAttributes imageAttr)
        {
            // Remember parameters
            _imageList = imageList;
            _imageIndexEnabled = imageIndexEnabled;
            _imageIndexDisabled = imageIndexDisabled;
            _imageAttr = imageAttr;

            // Set initial state
            _borderWidth = 2;
            _mouseOver = false;
            _mouseCapture = false;
            _popupStyle = true;
            _mouseButton = MouseButtons.None;

            // Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Prevent base class from trying to generate double click events and
            // so testing clicks against the double click time and rectangle. Getting
            // rid of this allows the user to press then button very quickly.
            SetStyle(ControlStyles.StandardDoubleClick, false);

            // Should not be allowed to select this control
            SetStyle(ControlStyles.Selectable, false);
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public ImageList ImageList
        {
            get { return _imageList; }

            set
            {
                if (_imageList != value)
                {
                    _imageList = value;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(-1)]
        public int ImageIndexEnabled
        {
            get { return _imageIndexEnabled; }

            set
            {
                if (_imageIndexEnabled != value)
                {
                    _imageIndexEnabled = value;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(-1)]
        public int ImageIndexDisabled
        {
            get { return _imageIndexDisabled; }

            set
            {
                if (_imageIndexDisabled != value)
                {
                    _imageIndexDisabled = value;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public ImageAttributes ImageAttributes
        {
            get { return _imageAttr; }
			
            set
            {
                if (_imageAttr != value)
                {
                    _imageAttr = value;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(2)]
        public int BorderWidth
        {
            get { return _borderWidth; }

            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool PopupStyle
        {
            get { return _popupStyle; }

            set
            {
                if (_popupStyle != value)
                {
                    _popupStyle = value;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!_mouseCapture)
            {
                // Mouse is over the button and being pressed, so enter the button depressed 
                // state and also remember the original button that was pressed. As we only 
                // generate an event when the same button is released.
                _mouseOver = true;
                _mouseCapture = true;
                _mouseButton = e.Button;

                // Redraw to show button state
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Are we waiting for this button to go up?
            if (e.Button == _mouseButton)
            {
                // Set state back to become normal
                _mouseOver = false;
                _mouseCapture = false;

                // Redraw to show button state
                Invalidate();
            }
            else
            {
                // We don't want to lose capture of mouse
                Capture = true;
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Is mouse point inside our client rectangle
            bool over = this.ClientRectangle.Contains(new Point(e.X, e.Y));

            // If entering the button area or leaving the button area...
            if (over != _mouseOver)
            {
                // Update state
                _mouseOver = over;

                // Redraw to show button state
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            // Update state to reflect mouse over the button area
            _mouseOver = true;

            // Redraw to show button state
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // Update state to reflect mouse not over the button area
            _mouseOver = false;

            // Redraw to show button state
            Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Do we have an image list for use?
            if (_imageList != null)
            {
                // Is the button disabled?
                if (!this.Enabled)
                {
                    // Do we have an image for showing when disabled?
                    if (_imageIndexDisabled != -1)
                    {
                        // Any caller supplied attributes to modify drawing?
                        if (null == _imageAttr)
                        {
                            // No, so use the simple DrawImage method
                            e.Graphics.DrawImage(_imageList.Images[_imageIndexDisabled], new Point(1,1));
                        }
                        else
                        {
                            // Yes, need to use the more complex DrawImage method instead
                            Image image = _imageList.Images[_imageIndexDisabled];

                            // Three points provided are upper-left, upper-right and 
                            // lower-left of the destination parallelogram. 
                            Point[] pts = new Point[3];
                            pts[0].X = 1;
                            pts[0].Y = 1;
                            pts[1].X = pts[0].X + image.Width;
                            pts[1].Y = pts[0].Y;
                            pts[2].X = pts[0].X;
                            pts[2].Y = pts[1].Y + image.Height;

                            e.Graphics.DrawImage(_imageList.Images[_imageIndexDisabled], 
                                                 pts,
                                                 new Rectangle(0, 0, image.Width, image.Height),
                                                 GraphicsUnit.Pixel, _imageAttr);
                        }
                    }
                    else
                    {
                        // No disbled image, how about an enabled image we can draw grayed?
                        if (_imageIndexEnabled != -1)
                        {
                            // Yes, draw the enabled image but with color drained away
                            ControlPaint.DrawImageDisabled(e.Graphics, _imageList.Images[_imageIndexEnabled], 1, 1, this.BackColor);
                        }
                        else
                        {
                            // No images at all. Do nothing.
                        }
                    }
                }
                else
                {
                    // Button is enabled, any caller supplied attributes to modify drawing?
                    if (null == _imageAttr)
                    {
                        // No, so use the simple DrawImage method
                        e.Graphics.DrawImage(_imageList.Images[_imageIndexEnabled], 
                                             (_mouseOver &&  _mouseCapture ? new Point(2,2) : 
                                             new Point(1,1)));
                    }
                    else
                    {
                        // Yes, need to use the more complex DrawImage method instead
                        Image image = _imageList.Images[_imageIndexEnabled];

                        // Three points provided are upper-left, upper-right and 
                        // lower-left of the destination parallelogram. 
                        Point[] pts = new Point[3];
                        pts[0].X = (_mouseOver && _mouseCapture) ? 2 : 1;
                        pts[0].Y = (_mouseOver && _mouseCapture) ? 2 : 1;
                        pts[1].X = pts[0].X + image.Width;
                        pts[1].Y = pts[0].Y;
                        pts[2].X = pts[0].X;
                        pts[2].Y = pts[1].Y + image.Height;

                        e.Graphics.DrawImage(_imageList.Images[_imageIndexEnabled], 
                                             pts,
                                             new Rectangle(0, 0, image.Width, image.Height),
                                             GraphicsUnit.Pixel, _imageAttr);
                    }

                    ButtonBorderStyle bs;

                    // Decide on the type of border to draw around image
                    if (_popupStyle)
                    {
                        if (_mouseOver && this.Enabled)
                            bs = (_mouseCapture ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset);
                        else
                            bs = ButtonBorderStyle.Solid;
                    }
                    else
                    {
                        if (this.Enabled)
                            bs = ((_mouseOver && _mouseCapture) ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset);
                        else
                            bs = ButtonBorderStyle.Solid;
                    }

                    ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, 
                                            this.BackColor, _borderWidth, bs,
                                            this.BackColor, _borderWidth, bs,
                                            this.BackColor, _borderWidth, bs,
                                            this.BackColor, _borderWidth, bs);
                }
            }

            base.OnPaint(e);
        }
    }
}