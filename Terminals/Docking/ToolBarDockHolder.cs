using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace rpaulo.toolbar
{
	public class ToolBarDockHolder : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;
		
		// Added by mav
		private AllowedBorders _allowedBorders = AllowedBorders.All;
		public AllowedBorders AllowedBorders
		{
			get {  return _allowedBorders; }
			set { _allowedBorders = value; }    
		}

		public bool IsAllowed (DockStyle dock)
		{
			switch (dock)
			{
				case DockStyle.Fill:
					return false;
				case DockStyle.Top:
					return (AllowedBorders & AllowedBorders.Top) == AllowedBorders.Top;
				case DockStyle.Left:
					return (AllowedBorders & AllowedBorders.Left) == AllowedBorders.Left;
				case DockStyle.Bottom:
					return (AllowedBorders & AllowedBorders.Bottom) == AllowedBorders.Bottom;
				case DockStyle.Right:
					return (AllowedBorders & AllowedBorders.Right) == AllowedBorders.Right;
				case DockStyle.None:
					return true;
			}
			return false;
		}

		Control _control;
		public Control Control { get { return _control; } }

		Point _preferredDockedLocation = new Point(0,0);
		public Point PreferredDockedLocation 
		{
			get { return _preferredDockedLocation; }
			set { _preferredDockedLocation = value; }
		}

		ToolBarDockArea _preferredDockedArea;
		public ToolBarDockArea PreferredDockedArea 
		{
			get { return _preferredDockedArea; }
			set { _preferredDockedArea = value; }
		}

		Form _form = new Form();
		public Form FloatForm 
		{
			get { return _form; }
		}

		// Added by mav
		private string _toolbarTitle = string.Empty;
		public string ToolbarTitle
		{
			get {  return _toolbarTitle; }
			set
			{ 
				if (_toolbarTitle != value)
				{
					_toolbarTitle = value;
					TitleTextChanged();
				}

			}    
		}

		DockStyle _style = DockStyle.Top;
		private System.Windows.Forms.Panel _panel;
	
		public DockStyle DockStyle 
		{
			get { return _style; }
			set 
			{
				_style = value;
				Create();
			}
		}

		ToolBarManager _dockManager = null;
		public ToolBarManager DockManager 
		{
			get { return _dockManager; }
			set { _dockManager = value; }
		}

		public ToolBarDockHolder(ToolBarManager dm, Control c, DockStyle style)
		{
			InitializeComponent();
			this.SetStyle(	
				ControlStyles.AllPaintingInWmPaint | 
				ControlStyles.UserPaint | 
				ControlStyles.DoubleBuffer, true);

			_panel.Controls.AddRange(new Control[]{c});
			DockManager = dm;
			if(style == DockStyle.Left) 
			{
				_preferredDockedArea = dm.Left;
			} 
			else if(style == DockStyle.Right) 
			{
				_preferredDockedArea = dm.Right;
			}
			else if(style == DockStyle.Bottom) 
			{
				_preferredDockedArea = dm.Bottom;
			} 
			else
			{
				_preferredDockedArea = dm.Top;
			}
			_control = c;
			FloatForm.Visible = false;
			FloatForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			FloatForm.MaximizeBox = false;
			FloatForm.MinimizeBox = false;
			FloatForm.ShowInTaskbar = false;
			FloatForm.ClientSize = new Size(10,10);
			// Added by mav
			DockManager.MainForm.AddOwnedForm(FloatForm); 
			DockStyle = style;
			ToolbarTitle = c.Text;
//			_control.TextChanged += new EventHandler(control_TextChanged);
		}

		// Added by mav
//		private void control_TextChanged(object sender, EventArgs e)
//		{
//			ToolbarTitle = _control.Text;
//		}

		private void TitleTextChanged() 
		{
			if(FloatForm.Visible)
				this.Invalidate(false);
		}

		void Create() 
		{
			Control c = _control;

			Size sz = new Size(0,0);
			if(typeof(System.Windows.Forms.ToolBar).IsAssignableFrom(c.GetType())) 
			{
				ToolBar tb = (ToolBar)c;
				int w = 0;
				int h = 0;
				if(DockStyle != DockStyle.Right && DockStyle != DockStyle.Left) 
				{
					c.Dock = DockStyle.Top;
					foreach(System.Windows.Forms.ToolBarButton but in tb.Buttons)
						w += but.Rectangle.Width;
					h = tb.ButtonSize.Height;
					sz = new Size(w, h);
				}
				else
				{
					c.Dock = DockStyle.Left;
					foreach(System.Windows.Forms.ToolBarButton but in tb.Buttons)
						// Added by mav
						if(but.Style == ToolBarButtonStyle.Separator) 
							h += 2*but.Rectangle.Width;
						else 
							h += but.Rectangle.Height; 
					w = tb.ButtonSize.Width + 2;
					sz = new Size(w, h);
				}
			} 
			else 
			{
				sz = c.Size;
				c.Dock = DockStyle.Fill;	
			}

			this.DockPadding.All = 0;
			if(DockStyle == DockStyle.None) 
			{
				this.DockPadding.Left = 2;
				this.DockPadding.Bottom = 2;
				this.DockPadding.Right = 2;
				this.DockPadding.Top = 15;
				sz = new Size(sz.Width+4, sz.Height+18);
			}
			else if(DockStyle != DockStyle.Right && DockStyle != DockStyle.Left) 
			{
				this.DockPadding.Left = 8;
				sz = new Size(sz.Width+8, sz.Height);
			}
			else
			{
				this.DockPadding.Top = 8;
				sz = new Size(sz.Width, sz.Height+8);
			}
			this.Size = sz;
		}


		public bool CanDrag(Point p) 
		{
			if(DockStyle == DockStyle.None) 
			{
				return p.Y < 16 && p.X < Width-16;
			}
			else 
			{
				if(DockStyle != DockStyle.Right && DockStyle != DockStyle.Left)
					return p.X < 8 && ClientRectangle.Contains(p);
				return p.Y < 8 && ClientRectangle.Contains(p);
			}
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._panel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// _panel
			// 
			this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel.Name = "_panel";
			this._panel.Size = new System.Drawing.Size(384, 40);
			this._panel.TabIndex = 0;
			// 
			// ToolBarDockHolder
			// 
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._panel});
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ToolBarDockHolder";
			this.Size = new System.Drawing.Size(384, 40);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ToolBarDockHolder_MouseUp);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ToolBarDockHolder_Paint);
			this.MouseEnter += new System.EventHandler(this.ToolBarDockHolder_MouseEnter);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolBarDockHolder_MouseMove);
			this.MouseLeave += new System.EventHandler(this.ToolBarDockHolder_MouseLeave);
			this.ResumeLayout(false);

		}
		#endregion

		private void ToolBarDockHolder_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(DockStyle == DockStyle.None) 
			{
				e.Graphics.FillRectangle(SystemBrushes.ControlDark, ClientRectangle);
				// Added by mav
				DrawString(e.Graphics, ToolbarTitle, new Rectangle(0,0, this.Width - 16, 14), SystemBrushes.ControlText);
				Rectangle closeRect = new Rectangle(this.Width-15, 2, 10, 10);
				Pen pen = new Pen(SystemColors.ControlText);
				DrawCloseButton(e.Graphics, closeRect, pen);
				if(closeRect.Contains(PointToClient(MousePosition)))
					e.Graphics.DrawRectangle(pen, closeRect);
				Rectangle r = ClientRectangle;
				r.Width--;
				r.Height--;
				e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDarkDark), r);
			}
			else
			{
				e.Graphics.FillRectangle(SystemBrushes.ControlLight, ClientRectangle);
				int off = 2;
				Pen pen = new Pen(SystemColors.ControlDark);
				if(DockStyle != DockStyle.Right && DockStyle != DockStyle.Left) 
				{
					for(int i=3; i<this.Size.Height-3; i+=off) 
						e.Graphics.DrawLine(pen, new Point(off, i), new Point(off+off, i));
				} 
				else 
				{
					for(int i=3; i<this.Size.Width-3; i+=off) 
						e.Graphics.DrawLine(pen, new Point(i, off), new Point(i, off+off));
				}
			}
		}

		private void ToolBarDockHolder_MouseEnter(object sender, System.EventArgs e)
		{
			if(DockStyle != DockStyle.None && CanDrag(PointToClient(MousePosition)))
				this.Cursor = Cursors.SizeAll;
			else
				this.Cursor = Cursors.Default;		
			this.Invalidate(false);
		}

		private void ToolBarDockHolder_MouseLeave(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.Default;
			this.Invalidate(false);
		}

		private void ToolBarDockHolder_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(DockStyle != DockStyle.None && CanDrag(new Point(e.X, e.Y)))
				this.Cursor = Cursors.SizeAll;
			else
				this.Cursor = Cursors.Default;
			this.Invalidate(false);
		}

		private void ToolBarDockHolder_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right && CanDrag(new Point(e.X, e.Y))) 
			{				
				DockManager.ShowContextMenu(this.PointToScreen(new Point(e.X, e.Y)));
			} 
			// Floating Form Close Button Clicked
			if(e.Button == MouseButtons.Left 
				&& DockStyle == DockStyle.None
				&& e.Y < 16 && e.X > Width-16)
			{
				FloatForm.Visible = false;
			}
		}

		static int _mininumStrSize = 0;
		private void DrawString(Graphics g, string s, Rectangle area, Brush brush) 
		{
			if(_mininumStrSize == 0) 
			{
				_mininumStrSize = (int)g.MeasureString("....", this.Font).Width;
			}
			if(area.Width < _mininumStrSize) 
				return;
			StringFormat drawFormat = new StringFormat();
			drawFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;
			drawFormat.Trimming = StringTrimming.EllipsisCharacter;
			SizeF ss = g.MeasureString(s, this.Font);
			if(ss.Height < area.Height) 
			{
				int offset = (int)(area.Height - ss.Height)/2;
				area.Y += offset;
				area.Height -= offset;
			}
			g.DrawString(s, this.Font, brush, area, drawFormat);
		}

		private void DrawCloseButton(Graphics g, Rectangle cross, Pen pen) 
		{
			cross.Inflate(-2, -2);

			g.DrawLine(pen, cross.X, cross.Y, cross.Right, cross.Bottom);
			g.DrawLine(pen, cross.X+1, cross.Y, cross.Right, cross.Bottom-1);
			g.DrawLine(pen, cross.X, cross.Y+1, cross.Right-1, cross.Bottom);
			g.DrawLine(pen, cross.Right, cross.Y, cross.Left, cross.Bottom);
			g.DrawLine(pen, cross.Right-1, cross.Y, cross.Left, cross.Bottom-1);
			g.DrawLine(pen, cross.Right, cross.Y+1, cross.Left+1, cross.Bottom);
		}
	}
}
