using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace rpaulo.toolbar
{
	public class ToolBarDockArea : System.Windows.Forms.UserControl
	{
		ToolBarManager _dockManager = null;
		public ToolBarManager DockManager 
		{
			get { return _dockManager; }
		}

        public Terminals.Docking.DockSavePosition Position
        {
            get
            {
                Terminals.Docking.DockSavePosition p = new Terminals.Docking.DockSavePosition();
                p.Left = this.Left;
                p.Top = this.Top;
                return p;
            }
            set
            {
                if (value != null)
                {
                    this.Left = value.Left;
                    this.Top = value.Top;
                }
            }
        }
		private System.ComponentModel.Container components = null;

		public ToolBarDockArea(ToolBarManager dockManager, DockStyle dockStyle)
		{
			InitializeComponent();

			this.SetStyle(	
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer, true);

			_dockManager = dockManager;
			DockManager.DockStation.Controls.Add(this);
			if(dockStyle == DockStyle.Fill || dockStyle == DockStyle.None)
				dockStyle = DockStyle.Top;

			this.Dock = dockStyle;
			this.SendToBack();

			FitHolders();

			this.Layout += new LayoutEventHandler(LayoutHandler);
		}

		public bool Horizontal { get { return this.Dock != DockStyle.Left && this.Dock != DockStyle.Right; } }

		class LineHolder 
		{
			public LineHolder(int index)
			{
				Index = index;
			}
			public ArrayList Columns = new ArrayList();			
			public int Index = 0;		
			public int Size = 0;

			public void AddColumn(ColumnHolder column) 
			{
				int indx = 0;
				foreach(ColumnHolder col in Columns) 
				{
					if(col.Position > column.Position) 
					{
						Columns.Insert(indx, column);
						break;
					}
					indx++;
				}
				if(indx == Columns.Count)
					Columns.Add(column);
			}

			public void Distribute() 
			{
				int pos = 0;
				foreach(ColumnHolder col in Columns) 
				{
					if(col.Position < pos)
						col.Position = pos;	
					pos = col.Position + col.Size;
				}
			}
		}

		class ColumnHolder 
		{
			public ColumnHolder(int pos, ToolBarDockHolder holder, int size)
			{
				Position = pos;
				Holder = holder;
				Size = size;
			}			
			public int Position = 0;	
			public int Size = 0;		
			public ToolBarDockHolder Holder;
		}


		int _lastLineCount = 1;
		public void LayoutHandler(object sender, LayoutEventArgs e)
		{
			this.SuspendLayout();
			int lineSzForCalc = 23;

			SortedList lineList = new SortedList();
			foreach(ToolBarDockHolder holder in this.Controls) 
			{
				if(holder.Visible) 
				{
					int prefLine = GetPreferredLine(lineSzForCalc, holder);	
					int prefPos = GetPreferredPosition(holder);	
					LineHolder line = (LineHolder)lineList[prefLine];
					if(line == null) 
					{
						line = new LineHolder(prefLine);
						lineList.Add(prefLine, line);
					}
					int csize = GetHolderWidth(holder);
					int lsize = GetHolderLineSize(holder);
					line.AddColumn(new ColumnHolder(prefPos, holder, csize+1));
					if(line.Size-1 < lsize)
						line.Size = lsize+1;
				}
			}

			int pos = 0;
			_lastLineCount = lineList.Count;
			if(_lastLineCount == 0)
				_lastLineCount = 1;
			for(int ndx = 0; ndx < lineList.Count; ndx++) 
			{
				LineHolder line = (LineHolder)lineList.GetByIndex(ndx);
				if(line != null) 
				{
					line.Distribute();
					foreach(ColumnHolder col in line.Columns) 
					{
						if(Horizontal) 
						{
							col.Holder.Location = new Point(col.Position, pos);
							col.Holder.PreferredDockedLocation = new Point(col.Holder.PreferredDockedLocation.X, pos + col.Holder.Height/2);
						}
						else
						{
							col.Holder.Location = new Point(pos, col.Position);
							col.Holder.PreferredDockedLocation = new Point(pos + col.Holder.Width/2, col.Holder.PreferredDockedLocation.Y);
						}
					}
					pos += line.Size+1;
				}
			}
			FitHolders();
			this.ResumeLayout();
		}

		protected int GetPreferredLine(int lineSz, ToolBarDockHolder holder) 
		{
			int pos, sz;
			if(Horizontal) 
			{
				pos = holder.PreferredDockedLocation.Y;
				sz = holder.Size.Height;
				if(pos < 0) 
					return Int32.MinValue;
				if(pos > this.Height) 
					return Int32.MaxValue;
			} 
			else 
			{
				pos = holder.PreferredDockedLocation.X;
				sz = holder.Size.Width;
				if(pos < 0) 
					return Int32.MinValue;
				if(pos > this.Width) 
					return Int32.MaxValue;
			}
			int line = pos / lineSz;
			int posLine = line * lineSz;
			if(posLine + 3 > pos)
				return line*2;
			if(posLine + lineSz - 3 < pos)
				return line*2+2;
			return line*2 + 1;
		}

		protected int GetPreferredPosition(ToolBarDockHolder holder) 
		{
			if(Horizontal) 
				return holder.PreferredDockedLocation.X;
			else 
				return holder.PreferredDockedLocation.Y;
		}

		protected int GetHolderLineSize(ToolBarDockHolder holder) 
		{
			if(Horizontal) 
				return holder.Height;
			else 
				return holder.Width;
		}
		protected int GetMyLineSize() 
		{
			if(Horizontal) 
				return Height;
			else 
				return Width;
		}
		protected int GetHolderWidth(ToolBarDockHolder holder) 
		{
			if(Horizontal) 
				return holder.Width;
			else 
				return holder.Height;
		}
		
		protected void FitHolders() 
		{
			Size sz = new Size(0,0);
			foreach(Control c in Controls) 
				if(c.Visible) 
				{
					if(c.Right > sz.Width)
						sz.Width = c.Right;
					if(c.Bottom > sz.Height)
						sz.Height = c.Bottom;
				}			
			if(Horizontal) 
				this.Height = sz.Height;
			else 
				this.Width = sz.Width;
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
			// 
			// ToolBarDockArea
			// 
			this.Name = "ToolBarDockArea";
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ToolBarDockArea_MouseUp);
		}
		#endregion


		private void ToolBarDockArea_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right) 
			{				
				DockManager.ShowContextMenu(this.PointToScreen(new Point(e.X, e.Y)));
			}
		}
	}
}
