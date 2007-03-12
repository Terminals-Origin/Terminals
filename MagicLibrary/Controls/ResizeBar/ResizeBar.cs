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
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Controls
{
    public interface IResizeSource
    {
        bool CanResize(ResizeBar bar);
        bool StartResizeOperation(ResizeBar bar, ref Rectangle screenBoundary);
        void EndResizeOperation(ResizeBar bar, int delta);
        Color ResizeBarColor { get; }
        int ResizeBarVector { get; }
        VisualStyle Style { get; }
        Color BackgroundColor { get; }
    }

    [ToolboxItem(false)]
    public class ResizeBar : Control
    {
        // Class constants
        protected static int _ideLength = 4;
        protected static int _plainLength = 6;

        // Instance fields
        protected VisualStyle _style;
        protected Direction _direction;
        protected bool _resizing;
        protected Point _pointStart;
        protected Point _pointCurrent;
        protected Rectangle _boundary;
        protected IResizeSource _resizeSource;

        public ResizeBar(Direction direction, IResizeSource resizeSource)
        {
            // Define initial state
            _direction = direction;
            _resizing = false;
            _resizeSource = resizeSource;

			// Always default to Control color
			this.BackColor = _resizeSource.ResizeBarColor;
			this.ForeColor = SystemColors.ControlText;

            UpdateStyle(_resizeSource.Style);
        }

        public VisualStyle Style
        {
            get { return _style; }

            set
            {
                if (_style != value)
                    UpdateStyle(value);
            }
        }

        public Direction Direction
        {
            get { return _direction; }

            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    UpdateStyle(_style);
                }
            }
        }

        public int Length
        {
            get
            {
                int vector = _resizeSource.ResizeBarVector;
                
                if (vector == -1)
                {
                    if (_style == VisualStyle.IDE)
                        vector = _ideLength;
                    else 
                        vector = _plainLength;
                }
               
                return vector;
            }
            
            set
            {
                // If a change in vector...
                if (value != this.Length)
                {
                    // Force update of the height/width
                    UpdateStyle(_resizeSource.Style);
                }
            }
        }

        public virtual void PropogateNameValue(PropogateName name, object value)
        {
            if (name == PropogateName.ResizeBarVector)
                this.Length = (int)value;

            if (name == PropogateName.ResizeBarColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }
        }
        
        protected void UpdateStyle(VisualStyle newStyle)
        {
            _style = newStyle;

            int vector = this.Length;

            if (_direction == Direction.Vertical)
                this.Height = vector;
            else
                this.Width = vector;

            Invalidate();
        }

        protected bool StartResizeOperation(MouseEventArgs e)
        {
            if (_resizeSource != null)
            {
                if (_resizeSource.CanResize(this))
                {
                    if (_resizeSource.StartResizeOperation(this, ref _boundary))
                    {
                        // Record the starting screen position
                        _pointStart = PointToScreen(new Point(e.X, e.Y));

                        // Record the current position being drawn
                        _pointCurrent = _pointStart;

                        // Draw the starting position
                        DrawResizeIndicator(_pointCurrent);

                        return true;
                    }
                }
            }

            return false;
        }

        protected void EndResizeOperation(MouseEventArgs e)
        {
            if (_resizeSource != null)
            {
                // Undraw the current position
                DrawResizeIndicator(_pointCurrent);

                // Find new screen position
                Point newCurrent = PointToScreen(new Point(e.X, e.Y));

                // Limit the extend the bar can be moved
                ApplyBoundaryToPoint(ref newCurrent);

                // Calculate delta from initial resize
                Point delta = new Point(newCurrent.X - _pointStart.X, 
                                        newCurrent.Y - _pointStart.Y);

                // Inform the Zone of requested change
                if (_direction == Direction.Horizontal)
                    _resizeSource.EndResizeOperation(this, delta.X);
                else
                    _resizeSource.EndResizeOperation(this, delta.Y);
            }

            _resizing = false;
        }

        protected void UpdateResizePosition(MouseEventArgs e)
        {
            // Find new screen position
            Point newCurrent = PointToScreen(new Point(e.X, e.Y));

            // Limit the extend the bar can be moved
            ApplyBoundaryToPoint(ref newCurrent);

            // Has change in position occured?
            if (newCurrent != _pointCurrent)
            {
                // Undraw the old position
                DrawResizeIndicator(_pointCurrent);

                // Record the new screen position
                _pointCurrent = newCurrent;

                // Draw the new position
                DrawResizeIndicator(_pointCurrent);
            }
        }
	
        protected void ApplyBoundaryToPoint(ref Point newCurrent)
        {
            // Calculate mouse position delta from mouse down
            Point delta = new Point(newCurrent.X - _pointStart.X, 
                newCurrent.Y - _pointStart.Y);
			
            // Get our dimensions in screen coordinates
            Rectangle client = RectangleToScreen(this.ClientRectangle);

            if (_direction == Direction.Horizontal)
            {
                client.Offset(delta.X, 0);

                // Test against left hand edge
                if (client.X < _boundary.X)
                    newCurrent.X += _boundary.X - client.X;
				
                // Test against right hand edge
                if (client.Right > _boundary.Right)
                    newCurrent.X -= client.Right - _boundary.Right;
            }
            else
            {
                client.Offset(0, delta.Y);

                // Test against top edge
                if (client.Y < _boundary.Y)
                    newCurrent.Y += _boundary.Y - client.Y;
				
                // Test against bottom edge
                if (client.Bottom > _boundary.Bottom)
                    newCurrent.Y -= client.Bottom - _boundary.Bottom;
            }		
        }

        protected void DrawResizeIndicator(Point screenPosition)
        {
            // Calculate mouse position delta from mouse down
            Point delta = new Point(screenPosition.X - _pointStart.X, 
                                    screenPosition.Y - _pointStart.Y);

            // Get our dimensions in screen coordinates
            Rectangle client = RectangleToScreen(this.ClientRectangle);

            if (_direction == Direction.Horizontal)
                client.Offset(delta.X, 0);
            else
                client.Offset(0, delta.Y);

            DrawHelper.DrawDragRectangle(client, 0);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Mouse down occured inside control
            _resizing = StartResizeOperation(e);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Currently in a resizing operation?
            if (_resizing)
            {
                // Reset resizing state
                EndResizeOperation(e);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((_resizeSource != null) && (_resizeSource.CanResize(this)))
            {
                // Display the correct mouse shape
                if (_direction == Direction.Vertical)
                    this.Cursor = Cursors.SizeNS;
                else
                    this.Cursor = Cursors.SizeWE;
            }
            else
                this.Cursor = Cursors.Arrow;

            // Currently in a resizing operation?
            if (_resizing)
            {
                UpdateResizePosition(e);
            }

            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Plain style draws a 3D effect around edges
            if (_style == VisualStyle.Plain)
            {
                // Drawing is relative to client area
                Size ourSize = this.ClientSize;

                Point[] light = new Point[2];
                Point[] dark = new Point[2];
                Point[] black = new Point[2];

                // Painting depends on orientation
                if (_direction == Direction.Vertical)
                {
                    // Draw as a horizontal bar
                    dark[0].Y = dark[1].Y = ourSize.Height - 2;
                    black[0].Y = black[1].Y = ourSize.Height - 1;
                    light[1].X = dark[1].X = black[1].X = ourSize.Width;
                }
                else
                {
                    // Draw as a vertical bar
                    dark[0].X = dark[1].X = ourSize.Width - 2;
                    black[0].X = black[1].X = ourSize.Width - 1;
                    light[1].Y = dark[1].Y = black[1].Y = ourSize.Height;
                }

                using (Pen penLightLight = new Pen(ControlPaint.LightLight(_resizeSource.BackgroundColor)),
                           penDark = new Pen(ControlPaint.Dark(_resizeSource.BackgroundColor)),
                           penBlack = new Pen(ControlPaint.DarkDark(_resizeSource.BackgroundColor)))
                {
                    e.Graphics.DrawLine(penLightLight, light[0], light[1]);
                    e.Graphics.DrawLine(penDark, dark[0], dark[1]);
                    e.Graphics.DrawLine(penBlack, black[0], black[1]);
                }	
            }
				
            // Let delegates fire through base
            base.OnPaint(e);
        }
    }
    
    [ToolboxItem(false)]
    public class ResizeAutoBar : ResizeBar
    {
        public ResizeAutoBar(Direction direction, IResizeSource resizeSource)
            : base(direction, resizeSource) 
        {
        }    
            
        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor = this.BackColor;
        
            switch(this.Dock)
            {
                case DockStyle.Right:
                    using(Pen penD = new Pen(ControlPaint.Dark(backColor)),
                              penDD = new Pen(ControlPaint.DarkDark(backColor)))
                    {
                        e.Graphics.DrawLine(penD, this.Width - 2, 0, this.Width - 2, this.Height);
                        e.Graphics.DrawLine(penDD, this.Width - 1, 0, this.Width - 1, this.Height);
                    }
                    break;
                case DockStyle.Left:
                    using(Pen penLL = new Pen(ControlPaint.LightLight(backColor)))
                        e.Graphics.DrawLine(penLL, 1, 0, 1, this.Height);
                    break;
                case DockStyle.Bottom:
                    using(Pen penD = new Pen(ControlPaint.Dark(backColor)),
                              penDD = new Pen(ControlPaint.DarkDark(backColor)))
                    {
                        e.Graphics.DrawLine(penD, 0, this.Height - 2, this.Width, this.Height - 2);
                        e.Graphics.DrawLine(penDD, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    break;
                case DockStyle.Top:
                    using(Pen penLL = new Pen(ControlPaint.LightLight(backColor)))
                        e.Graphics.DrawLine(penLL, 0, 1, this.Width, 1);
                    break;
            }
        }
    }
}