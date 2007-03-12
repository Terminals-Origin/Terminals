//*****************************************************************************
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
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    [ToolboxItem(false)]
    public class TabStub : UserControl
    {
		private class DrawTab
		{
			protected int _index;
			protected Rectangle _drawRect;
            protected Crownwood.Magic.Controls.TabPage _tabPage;

			public DrawTab(Crownwood.Magic.Controls.TabPage tabPage, Rectangle drawRect, int index)
			{
				_index = index;
				_tabPage = tabPage;
				_drawRect = drawRect;
			}

			public Crownwood.Magic.Controls.TabPage TabPage  { get { return _tabPage; } }
            public Rectangle DrawRect                        { get { return _drawRect; } }
			public int Index                                 { get { return _index; } }
		}

        // Class constants
		protected static int _imageGap = 3;
		protected static int _imageGaps = 6;
        protected static int _imageVector = 16;
        protected static int _beginGap = 2;
        protected static int _endGap = 8;
        protected static int _sideGap = 2;
		protected static int _hoverInterval = 500;

		// Instance fields
		protected Edge _edge;
		protected int _hoverOver;
		protected int _hoverItem;
		protected int _selectedIndex;
    	protected bool _defaultFont;
		protected bool _defaultColor;
		protected Color _backIDE;
        protected Timer _hoverTimer;
        protected TabPageCollection _tabPages;
		protected WindowContentTabbed _wct;
		protected ArrayList _drawTabs;
        protected VisualStyle _style;

        public delegate void TabStubIndexHandler(TabStub sender, int pageIndex);
        public delegate void TabStubHandler(TabStub sender);

        // Exposed events
        public event TabStubIndexHandler PageClicked;
        public event TabStubIndexHandler PageOver;
        public event TabStubHandler PagesLeave;

		public TabStub(VisualStyle style)
		{
			// Default state
			_wct = null;
			_style = style;
            _hoverOver = -1;
            _hoverItem = -1;
            _selectedIndex = -1;
            _defaultFont = true;
			_defaultColor = true;
			_edge = Edge.None;
			_drawTabs = new ArrayList();
            _tabPages = new TabPageCollection();
            base.Font = SystemInformation.MenuFont;

            // Hookup to collection events
            _tabPages.Cleared += new CollectionClear(OnClearedPages);
            _tabPages.Inserted += new CollectionChange(OnInsertedPage);
            _tabPages.Removing += new CollectionChange(OnRemovingPage);
            _tabPages.Removed += new CollectionChange(OnRemovedPage);

            // Need notification when the MenuFont is changed
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += new 
                UserPreferenceChangedEventHandler(OnPreferenceChanged);

			// Default default colors
			DefineBackColor(SystemColors.Control);

			// Create the Timer for handling hovering over items
			_hoverTimer = new Timer();
			_hoverTimer.Interval = _hoverInterval;
			_hoverTimer.Tick += new EventHandler(OnTimerExpire);
		}

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                // Remove notifications
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -= new 
                    UserPreferenceChangedEventHandler(OnPreferenceChanged);
            }
            base.Dispose(disposing);
        }

        public TabPageCollection TabPages
        {
            get { return _tabPages; }

            set
            {
                _tabPages.Clear();
                _tabPages = value;
            }
        }

		public Edge Edging
		{
			get { return _edge; }

			set
			{
				if (value != _edge)
				{
					_edge = value;
					ResizeControl();
					Recalculate();
					Invalidate();
				}
		    }
		}

		public int SelectedIndex
		{
			get { return _selectedIndex; }

			set
			{
				if (value != _selectedIndex)
				{
					_selectedIndex = value;
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
				if (value != null)
				{
					if (value != base.Font)
					{
						_defaultFont = (value == SystemInformation.MenuFont);

						base.Font = value;
						ResizeControl();
						Recalculate();
						Invalidate();
					}
				}
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }

            set
            {
                if (this.BackColor != value)
                {
                    _defaultColor = (value == SystemColors.Control);
					DefineBackColor(value);
                    Invalidate();
                }
            }
        }

        public WindowContentTabbed WindowContentTabbed
        {
            get { return _wct; }
            set { _wct = value; }
        }
        
		public virtual void OnPageClicked(int pageIndex)
		{
            // Has anyone registered for the event?
			if (PageClicked != null)
				PageClicked(this, pageIndex);
		}

		public virtual void OnPageOver(int pageIndex)
		{
            // Has anyone registered for the event?
			if (PageOver != null)
				PageOver(this, pageIndex);
		}

        public virtual void OnPagesLeave()
        {
            // Has anyone registered for the event?
            if (PagesLeave != null)
                PagesLeave(this);
        }
        
        public void PropogateNameValue(PropogateName name, object value)
        {
            switch(name)
            {
                case PropogateName.BackColor:
                    this.BackColor = (Color)value;
                    Invalidate();
                    break;
                case PropogateName.InactiveTextColor:
                    this.ForeColor = (Color)value;
                    Invalidate();
                    break;
                case PropogateName.CaptionFont:
                    this.Font = (Font)value;
                    break;
            }
            
            // Pass onto the contained WCT
            _wct.PropogateNameValue(name, value);
        }
                
        protected void DefineBackColor(Color backColor)
		{
			base.BackColor = backColor;
			
            _backIDE = ColorHelper.TabBackgroundFromBaseColor(backColor);
		}

		protected void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			// Are we using the default menu or a user defined value?
			if (_defaultFont)
			{
				base.Font = SystemInformation.MenuFont;
				ResizeControl();
				Recalculate();
				Invalidate();
			}
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			// If still using the Default color when we were created
			if (_defaultColor)
			{
				this.BackColor = SystemColors.Control;
				Invalidate();
			}

			base.OnSystemColorsChanged(e);
		}

        protected void OnClearedPages()
        {
            // Cancel any hover selection
            CancelHoverItem();

			// Cancel any current selection
			_selectedIndex = -1;

			ResizeControl();
			Recalculate();
			Invalidate();
		}
		
        protected void OnInsertedPage(int index, object value)
		{
			// If no page is currently selected
			if (_selectedIndex == -1)
			{
				// Then make the inserted page selected
				_selectedIndex = index;
			}

			ResizeControl();
			Recalculate();
			Invalidate();
		}

        protected void OnRemovingPage(int index, object value)
        {
            // Removed page involved in hover calculations?
            if ((_hoverOver == index) || (_hoverItem == index))
                CancelHoverItem();
        
            // Removing the last page?
            if (_tabPages.Count == 1)
            {
                // Get rid of any selection
                _selectedIndex = -1;
            }
            else
            {
                // If removing a page before the selected one...
			    if (index < _selectedIndex)
			    {
                    // ...then the selected index must be decremented to match
					_selectedIndex--;
			    }
			    else
			    {
			        // If the selected page is the last one then...
			        if (_selectedIndex == (_tabPages.Count-1))
			        {
			            // Must reduce selected index
                        _selectedIndex--;
                    }
			    }
	        }
        }

        protected void OnRemovedPage(int index, object value)
        {
			ResizeControl();
			Recalculate();
			Invalidate();
		}

        protected void CancelHoverItem()
        {
            // Currently timing a hover change?
            if (_hoverOver != -1)
            {
                // Prevent timer from expiring
                _hoverTimer.Stop();
                
                // No item being timed
                _hoverOver = -1;
            }

            // Any current hover item?
            if (_hoverItem != -1)
            {
                // No item is being hovered
                _hoverItem = -1;
		        
                // Generate event for end of hover
                OnPagesLeave();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
			// Create a point representing current mouse position
			Point mousePos = new Point(e.X, e.Y);

			int index = 0;
			int count = _drawTabs.Count;

			// Search each draw cell
			for(; index<count; index++)
			{
				DrawTab dt = _drawTabs[index] as DrawTab;

				// Is mouse over this cell?
				if (dt.DrawRect.Contains(mousePos))
				{
					// If the mouse is not over the hover item
					if (_hoverItem != dt.Index)
					{
					    // And we are not already timing this change in hover
					    if (_hoverOver != dt.Index)
					    {
					        // Start timing the hover change
						    _hoverTimer.Start();
						    
						    // Remember which item we are timing
						    _hoverOver = dt.Index;
				        }
					}

    				break;
				}
			}

			// Failed to find an item?
			if (index == count)
			{
				// If we have a hover item or timing a hover change
				if ((_hoverOver != -1) || (_hoverItem != -1))
				{
				    // Stop any timing
				    CancelHoverItem();
				}
			}

			base.OnMouseMove(e);
		}

        protected override void OnMouseLeave(EventArgs e)
        {
            // Remove any hover state
            CancelHoverItem();
    
			base.OnMouseLeave(e);
		}

		protected void OnTimerExpire(object sender, EventArgs e)
		{
		    // Prevent the timer from firing again
			_hoverTimer.Stop();

            // A change in hover still valid?
            if (_hoverItem != _hoverOver)
            {
                // This item becomes the current hover item
                _hoverItem = _hoverOver;
                
                // No longer in a timing state
                _hoverOver = -1;

			    // Do we need a change in selection?
			    if (_selectedIndex != _hoverItem)
			    {
				    // Change selection and redraw
				    _selectedIndex = _hoverItem;

				    Recalculate();
				    Invalidate();
                }

			    // Generate event to notify where mouse is now hovering
			    OnPageOver(_selectedIndex);
		    }
		}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Only select a button or page when using left mouse button
            if (e.Button == MouseButtons.Left)
			{
                // Create a point representing current mouse position
                Point mousePos = new Point(e.X, e.Y);

				int count = _drawTabs.Count;

				// Search each draw cell
				for(int index=0; index<count; index++)
				{
					DrawTab dt = _drawTabs[index] as DrawTab;

					// Is mouse pressed in this draw cell?
					if (dt.DrawRect.Contains(mousePos))
					{
                        // Prevent any hover timer expiring
                        _hoverTimer.Stop();
						    
                        // This becomes the current hover item
                        _hoverItem = _selectedIndex;
						    
                        // Not timing a hover change
                        _hoverOver = _hoverItem;
                        
                        // Will this cause a change in selection?
						if (_selectedIndex != dt.Index)
						{
                            // Change selection and redraw
                            _selectedIndex = dt.Index;
						
							Recalculate();
							Invalidate();
                        }

                        // Generate event to notify a click occured on the selection
                        OnPageClicked(_selectedIndex);

                        break;
					}
				}
			}
       }

        public static int TabStubVector(Font font)
        {
            int fixedVector = _imageVector + _imageGaps;

            int minFontVector = font.Height + _imageGaps;

            // Make sure at least bit enough for the provided font
            if (fixedVector < minFontVector)
                fixedVector = minFontVector;
                
            return fixedVector + _sideGap;
        }

		protected void ResizeControl()
		{
			int textMax = 0;

			// Find largest space needed for drawing page text
			using(Graphics g = this.CreateGraphics())
			{
				foreach(Crownwood.Magic.Controls.TabPage page in _tabPages)
				{
					// Find width of the requested text
					SizeF dimension = g.MeasureString(page.Title, this.Font);

					if ((int)dimension.Width > textMax)
						textMax = (int)dimension.Width;
				}
			}

			// Calculate total width/height needed
			int variableVector = _tabPages.Count * (_imageVector + _imageGaps) + textMax + _imageGap;

            // Calculate the fixed direction value
			int fixedVector = TabStubVector(this.Font);

			// Resize the control as appropriate
			switch(_edge)
			{
				case Edge.Left:
				case Edge.Right:
					this.Size = new Size(fixedVector, variableVector + _beginGap + _endGap);
					break;
				case Edge.Top:
				case Edge.Bottom:
				case Edge.None:
				default:
					this.Size = new Size(variableVector + _beginGap + _endGap, fixedVector);
					break;
			}
		}

		protected void Recalculate()
		{
			// Create a fresh colleciton for drawing objects
			_drawTabs = new ArrayList();

			// Need start and end position markers
			int posEnd;
			int cellVector = _imageVector + _imageGaps;
			int posStart = _beginGap;

			switch(_edge)
			{
				case Edge.Left:
				case Edge.Right:
					posEnd = this.Height - _endGap;
					break;
				case Edge.Top:
				case Edge.Bottom:
				case Edge.None:
				default:
					posEnd = this.Width - _endGap;
					break;
			}

			int count = _tabPages.Count;

			// Process from start of list until we find the selected one
			for(int index=0; (index<count) && (index!=_selectedIndex); index++)
			{
				Rectangle drawRect;

				// Drawing rectangle depends on direction
				switch(_edge)
				{
					case Edge.Left:
						drawRect = new Rectangle(0, posStart, this.Width - _sideGap - 1, cellVector);
						break;
					case Edge.Right:
						drawRect = new Rectangle(_sideGap, posStart, this.Width - _sideGap, cellVector);
						break;
					case Edge.Bottom:
						drawRect = new Rectangle(posStart, _sideGap, cellVector, this.Height - _sideGap);
						break;
					case Edge.Top:
					case Edge.None:
					default:
						drawRect = new Rectangle(posStart, 0, cellVector, this.Height - _sideGap - 1);
						break;
				}

				// Move starting position
				posStart += cellVector;

				// Generate new drawing object for this tab
				_drawTabs.Add(new DrawTab(_tabPages[index], drawRect, index));
			}

			// Process from end of list until we find the selected one
			for(int index=count-1; (index>=0) && (index!=_selectedIndex); index--)
			{
				Rectangle drawRect;

				// Drawing rectangle depends on direction
				switch(_edge)
				{
					case Edge.Left:
						drawRect = new Rectangle(0, posEnd - cellVector, this.Width - _sideGap - 1, cellVector);
						break;
					case Edge.Right:
						drawRect = new Rectangle(_sideGap, posEnd - cellVector, this.Width - _sideGap, cellVector);
						break;
					case Edge.Bottom:
						drawRect = new Rectangle(posEnd - cellVector, _sideGap, cellVector, this.Height - _sideGap);
						break;
					case Edge.Top:
					case Edge.None:
					default:
						drawRect = new Rectangle(posEnd - cellVector, 0, cellVector, this.Height - _sideGap - 1);
						break;
				}

				// Move starting position
				posEnd -= cellVector;

				// Generate new drawing object for this tab
				_drawTabs.Add(new DrawTab(_tabPages[index], drawRect, index));
			}

			if (_selectedIndex != -1)
			{
				Rectangle drawRect;

				// Drawing rectangle depends on direction
				switch(_edge)
				{
					case Edge.Left:
						drawRect = new Rectangle(0, posStart, this.Width - _sideGap - 1, posEnd - posStart);
						break;
					case Edge.Right:
						drawRect = new Rectangle(_sideGap, posStart, this.Width - _sideGap, posEnd - posStart);
						break;
					case Edge.Bottom:
						drawRect = new Rectangle(posStart, _sideGap, posEnd - posStart, this.Height - _sideGap);
						break;
					case Edge.Top:
					case Edge.None:
					default:
						drawRect = new Rectangle(posStart, 0, posEnd - posStart, this.Height - _sideGap - 1);
						break;
				}

				// Generate new drawing object for this tab
				_drawTabs.Add(new DrawTab(_tabPages[_selectedIndex], drawRect, _selectedIndex));
			}
		}

		protected void AdjustRectForEdge(ref Rectangle rect)
		{
			// Adjust rectangle to exclude desired edge
			switch(_edge)
			{
				case Edge.Left:
					rect.X--;
					rect.Width++;
					break;
				case Edge.Right:
					rect.Width++;
					break;
				case Edge.Bottom:
					rect.Height++;
					break;
				case Edge.Top:
				case Edge.None:
				default:
					rect.Y--;
					rect.Height++;
					break;
			}
		}

        protected void DrawOutline(Graphics g, bool pre)
        {
            Rectangle borderRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            // Adjust for drawing area
            switch(_edge)
            {
                case Edge.Left:
                    borderRect.Y += _beginGap;
                    borderRect.Height -= _beginGap + _endGap - 1;
                    borderRect.Width -= _sideGap;
                    break;
                case Edge.Right:
                    borderRect.Y += _beginGap;
                    borderRect.Height -= _beginGap + _endGap - 1;
                    borderRect.X += _sideGap;
                    borderRect.Width -= _sideGap;
                    break;
                case Edge.Bottom:
                    borderRect.Y += _sideGap;
                    borderRect.Height -= _sideGap;
                    borderRect.X += _beginGap;
                    borderRect.Width -= _beginGap + _endGap - 1;
                    break;
                case Edge.Top:
                case Edge.None:
                default:
                    borderRect.Height -= _sideGap;
                    borderRect.X += _beginGap;
                    borderRect.Width -= _beginGap + _endGap - 1;
                    break;
            }

            // Remove unwated drawing edge
            AdjustRectForEdge(ref borderRect);

            if (pre)
            {
                if (_style == VisualStyle.IDE)
                {
			        // Fill tab area in required color
                    using(SolidBrush fillBrush = new SolidBrush(this.BackColor))
                        g.FillRectangle(fillBrush, borderRect);
                }
            }
            else
            {
                if (_style == VisualStyle.Plain)
                {
                    using(Pen penL = new Pen(ControlPaint.LightLight(this.BackColor)),
                          penD = new Pen(ControlPaint.Dark(this.BackColor)))
                    {
                        g.DrawLine(penL, borderRect.Left, borderRect.Top, borderRect.Right, borderRect.Top);
                        g.DrawLine(penL, borderRect.Left, borderRect.Top, borderRect.Left, borderRect.Bottom);
                        g.DrawLine(penD, borderRect.Right, borderRect.Top, borderRect.Right, borderRect.Bottom);
                        g.DrawLine(penD, borderRect.Right, borderRect.Bottom, borderRect.Left, borderRect.Bottom);
                    }
                }
            }
        }

        protected void DrawOutlineForCell(Graphics g, Pen pen, Rectangle rect)
        {
			// Draw border around the tab cell
			if (_style == VisualStyle.IDE)
				g.DrawRectangle(pen, rect);
			else
		    {
		        switch(_edge)
		        {
		            case Edge.Left:
                    case Edge.Right:
                        g.DrawLine(pen, rect.Left + 1, rect.Bottom, rect.Right, rect.Bottom);       
                        break;                    
                    case Edge.Top:
                    case Edge.Bottom:
                        g.DrawLine(pen, rect.Right, rect.Top + 1, rect.Right, rect.Bottom);       
                        break;                    
                }
		    }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Fill background in required color
            if (_style == VisualStyle.IDE)            
                using(SolidBrush fillBrush = new SolidBrush(_backIDE))
                    e.Graphics.FillRectangle(fillBrush, this.ClientRectangle);
            else
                using(SolidBrush fillBrush = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(fillBrush, this.ClientRectangle);

            // Style specific outline drawing
            DrawOutline(e.Graphics, true);

            // Draw border around area
			using(Pen borderPen = new Pen(ControlPaint.LightLight(this.ForeColor)))
			{
				// Draw each of the draw objects
				foreach(DrawTab dt in _drawTabs)
				{
					Rectangle drawRect = dt.DrawRect;

					AdjustRectForEdge(ref drawRect);

                    // Style specific cell outline drawing
                    DrawOutlineForCell(e.Graphics, borderPen, drawRect);

					// Draw the image in the left/top of the cell
					Crownwood.Magic.Controls.TabPage page = dt.TabPage;

                    int xDraw;
                    int yDraw;

                    switch(_edge)
                    {
                        case Edge.Left:
                        case Edge.Right:
                            xDraw = drawRect.Left + (drawRect.Width - _imageVector) / 2;
                            yDraw = drawRect.Top + _imageGap;
                            break;
                        case Edge.Top:
                        case Edge.Bottom:
                        case Edge.None:
                        default:
                            xDraw = drawRect.Left + _imageGap;
                            yDraw = drawRect.Top + (drawRect.Height - _imageVector) / 2;
                            break;
                    }

                    if ((page.ImageIndex != -1) && (page.ImageList != null))
					{
						// Draw the actual image
						e.Graphics.DrawImage(page.ImageList.Images[page.ImageIndex],
											 new Rectangle(xDraw, yDraw, _imageVector, _imageVector));
                    }

					// Is anything currently selected
					if (_selectedIndex != -1)
					{
						// Is this page selected?
						
						if (page == _tabPages[_selectedIndex])
						{
							Rectangle textRect;

							StringFormat drawFormat = new StringFormat();
						    drawFormat.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
							drawFormat.Alignment = StringAlignment.Center;
							drawFormat.LineAlignment = StringAlignment.Center;

							// Create text drawing rectangle
							switch(_edge)
							{
								case Edge.Left:
								case Edge.Right:
									textRect = new Rectangle(drawRect.Left, yDraw + _imageVector + _imageGap, 
										                     drawRect.Width, drawRect.Height - _imageVector - _imageGap * 2);
				                    drawFormat.FormatFlags |= StringFormatFlags.DirectionVertical;
									break;
								case Edge.Top:
								case Edge.Bottom:
								case Edge.None:
								default:
									textRect = new Rectangle(xDraw + _imageVector + _imageGap, drawRect.Top, 
										                     drawRect.Width - _imageVector - _imageGap * 2, drawRect.Height);
									break;
							}
							
							Color brushColor = this.ForeColor;
							
							if (_style == VisualStyle.IDE)
								brushColor = ControlPaint.Light(brushColor);

							using(SolidBrush drawBrush = new SolidBrush(brushColor))
								e.Graphics.DrawString(page.Title, this.Font, drawBrush, textRect, drawFormat);
						}
					}
				}
			}

            // Style specific outline drawing
            DrawOutline(e.Graphics, false);
            
            base.OnPaint(e);
		}
	}
}

