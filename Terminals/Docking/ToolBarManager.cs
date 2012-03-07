using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;


namespace rpaulo.toolbar
{
	public class ToolBarManager : IMessageFilter
	{
		// Added by mav
		ScrollableControl _dockStation = null;
		public ScrollableControl DockStation 
		{
			get { return _dockStation; }
			set { _dockStation = value; }
		}

		private Form _mainForm = null;
		public System.Windows.Forms.Form MainForm
		{
			get {  return _mainForm; }
			set { _mainForm = value; }    
		}

		ToolBarDockArea _left; 
		public ToolBarDockArea Left { get { return _left; } }
		ToolBarDockArea _right;
		public ToolBarDockArea Right { get { return _right; } }
		ToolBarDockArea _top;
		public ToolBarDockArea Top { get { return _top; } }
		ToolBarDockArea _bottom;
		public ToolBarDockArea Bottom { get { return _bottom; } }

		public ToolBarManager(ScrollableControl dockStation, Form mainForm)
		{
			this.DockStation = dockStation;
			this.MainForm = mainForm;
			_left = new ToolBarDockArea(this, DockStyle.Left);
			_right = new ToolBarDockArea(this, DockStyle.Right);
			_top = new ToolBarDockArea(this, DockStyle.Top);
			_bottom = new ToolBarDockArea(this, DockStyle.Bottom);
			Application.AddMessageFilter(this);
		}


		protected ToolBarDockArea GetClosestArea(Point ptScreen, ToolBarDockArea preferred)
		{
			if(preferred != null) 
			{
				Rectangle p = preferred.RectangleToScreen(preferred.ClientRectangle);
				p.Inflate(8,8);
				if(p.Contains(ptScreen)) return preferred;
			}

			Rectangle l = _left.RectangleToScreen(_left.ClientRectangle); 
			l.Inflate(8,8);
			Rectangle r = _right.RectangleToScreen(_right.ClientRectangle);
			r.Inflate(8,8);
			Rectangle t = _top.RectangleToScreen(_top.ClientRectangle);
			t.Inflate(8,8);
			Rectangle b = _bottom.RectangleToScreen(_bottom.ClientRectangle);
			b.Inflate(8,8);

			if(t.Contains(ptScreen)) return _top;
			if(b.Contains(ptScreen)) return _bottom;
			if(l.Contains(ptScreen)) return _left;
			if(r.Contains(ptScreen)) return _right;

			return null;
		}

		private ArrayList _holders = new ArrayList();

		// Added by mav
		public ToolBarDockHolder GetHolder(Control c)
		{		
			foreach(ToolBarDockHolder holder in _holders) 
				if(holder.Control == c)
					return holder;
			return null;
		}
		public ToolBarDockHolder GetHolder(string title)
		{		
			foreach(ToolBarDockHolder holder in _holders) 
				if(holder.ToolbarTitle == title)
					return holder;
			return null;
		}

		public ArrayList GetControls()
		{
			ArrayList list = new ArrayList();			
			foreach(ToolBarDockHolder holder in _holders) 
				list.Add(holder.Control);
			return list;
		}

		public bool ContainsControl(Control c)
		{
			return GetControls().Contains(c);
		}

		public void ShowControl(Control c, bool show) 
		{
			ToolBarDockHolder holder = GetHolder(c);
			if(holder != null) 
			{
				if(holder.Visible != show) 
				{
					if(IsDocked(holder))
					{
						holder.Visible = show;
					}
					else
					{
						holder.FloatForm.Visible = show;
					}
				}
			}
		}

		// Added by mav
		public ToolBarDockHolder AddControl(Control c, DockStyle site) 
		{
			return AddControl(c, site, null, DockStyle.Right);
		}

		public ToolBarDockHolder AddControl(Control c) 
		{
			return AddControl(c, DockStyle.Top, null, DockStyle.Right);
		}

		public ToolBarDockHolder AddControl(Control c, DockStyle site, Control refControl, DockStyle refSite) 
		{
			if(site == DockStyle.Fill) 
				site = DockStyle.Top;

			ToolBarDockHolder holder = new ToolBarDockHolder(this, c, site);

			if(refControl != null) 
			{
				ToolBarDockHolder refHolder = GetHolder(refControl);
				if(refHolder != null) 
				{
					Point p = refHolder.PreferredDockedLocation;
					if(refSite == DockStyle.Left) 
					{
						p.X -= 1;
					} 
					else if(refSite == DockStyle.Right) 
					{
						p.X += refHolder.Width+1;
					}
					else if(refSite == DockStyle.Bottom) 
					{
						p.Y += refHolder.Height+1;
					} 
					else
					{
						p.Y -= 1;
					}
					holder.PreferredDockedLocation = p;
				}
			}


			_holders.Add(holder);
			if(site != DockStyle.None) 
			{
				holder.DockStyle = site;
				holder.Parent = holder.PreferredDockedArea;
			} 
			else 
			{
				holder.Parent = holder.FloatForm;
				holder.Location = new Point(0,0);
				holder.DockStyle = DockStyle.None;
				holder.FloatForm.Size = holder.Size;
				holder.FloatForm.Visible = true;
			}

			holder.MouseUp += new MouseEventHandler(this.ToolBarMouseUp);
			holder.DoubleClick += new EventHandler(this.ToolBarDoubleClick);
			holder.MouseMove += new MouseEventHandler(this.ToolBarMouseMove);
			holder.MouseDown += new MouseEventHandler(this.ToolBarMouseDown);

			return holder;
		}

		public void RemoveControl(Control c) 
		{
			ToolBarDockHolder holder = GetHolder(c);
			if(holder != null)
			{
				holder.MouseUp -= new MouseEventHandler(this.ToolBarMouseUp);
				holder.DoubleClick -= new EventHandler(this.ToolBarDoubleClick);
				holder.MouseMove -= new MouseEventHandler(this.ToolBarMouseMove);
				holder.MouseDown -= new MouseEventHandler(this.ToolBarMouseDown);
				
				_holders.Remove(holder);
				holder.Parent = null;
				holder.FloatForm.Close();
			}			
		}

		ToolBarDockHolder _dragged;
		Point _ptStart;
		Point _ptOffset;

		private void ToolBarMouseDown(object sender, MouseEventArgs e)
		{
			ToolBarDockHolder holder = (ToolBarDockHolder)sender;

			if(_dragged==null 
				&& e.Button.Equals(MouseButtons.Left) 
				&& e.Clicks == 1
				&& holder.CanDrag(new Point(e.X, e.Y)) )
			{
				_ptStart = Control.MousePosition;
				_dragged = (ToolBarDockHolder)sender;
				_ptOffset = new Point(e.X, e.Y);
			}
		}

		private bool IsDocked(ToolBarDockHolder holder)
		{
			return holder.Parent == Top
				|| holder.Parent == Left
				|| holder.Parent == Right
				|| holder.Parent == Bottom;
		}

		private ToolBarDockArea GetDockedArea(ToolBarDockHolder holder)
		{
			if(holder.Parent == Top) return Top;
			if(holder.Parent == Left) return Left;
			if(holder.Parent == Right) return Right;
			if(holder.Parent == Bottom) return Bottom;
			return null;
		}

		private void ToolBarMouseMove(object sender, MouseEventArgs e)
		{
			Point ptPos = new Point(e.X, e.Y);

			if(_dragged != null)
			{
				Point ptDelta = new Point(_ptStart.X - Control.MousePosition.X, _ptStart.Y - Control.MousePosition.Y);

				Point newLoc = _dragged.PointToScreen(new Point(0,0));
				newLoc = new Point(newLoc.X - ptDelta.X, newLoc.Y - ptDelta.Y);
				ToolBarDockArea closest = GetClosestArea(Control.MousePosition, _dragged.PreferredDockedArea);
				// Added by mav
				if(closest != null && !_dragged.IsAllowed(closest.Dock))
					closest = null;

				ToolBarDockArea docked = GetDockedArea(_dragged);

				if(_ctrlDown)
					closest = null;

				if(docked != null)
				{
					if(closest == null) 
					{
						docked.SuspendLayout();
						_dragged.Parent = _dragged.FloatForm;
						_dragged.Location = new Point(0,0);
						_dragged.DockStyle = DockStyle.None;
						_dragged.FloatForm.Visible = true;
						_dragged.FloatForm.Location = new Point(Control.MousePosition.X-_ptOffset.X, Control.MousePosition.Y-8);
						_dragged.FloatForm.Size = _dragged.Size;
						docked.ResumeLayout();
						docked.PerformLayout();
					} 
					else if(closest != docked) 
					{
						closest.SuspendLayout();
						newLoc = closest.PointToClient(Control.MousePosition);
						_dragged.DockStyle = closest.Dock;
						_dragged.Parent = closest;
						_dragged.PreferredDockedLocation = newLoc;
						_dragged.FloatForm.Visible = false;
						_dragged.PreferredDockedArea = closest;
						closest.ResumeLayout();
						closest.PerformLayout();
					} 
					else 
					{
						closest.SuspendLayout();
						newLoc = closest.PointToClient(Control.MousePosition);
//						if(closest.Horizontal)
//							newLoc = new Point(newLoc.X - 4, newLoc.Y - _dragged.Height/2);
//						else
//							newLoc = new Point(newLoc.X - _dragged.Width/2, newLoc.Y - 4);
						_dragged.PreferredDockedLocation = newLoc;
						closest.ResumeLayout();
						closest.PerformLayout();
					}
				}
				else
				{
					if(closest == null) 
					{
						_dragged.FloatForm.Location = newLoc;
					}
					else
					{
						closest.SuspendLayout();
						newLoc = closest.PointToClient(Control.MousePosition);
						_dragged.DockStyle = closest.Dock;
						_dragged.Parent = closest;
						_dragged.PreferredDockedLocation = newLoc;
						_dragged.FloatForm.Visible = false;
						_dragged.PreferredDockedArea = closest;
						closest.ResumeLayout();
						closest.PerformLayout();
					}
				}
				_ptStart = Control.MousePosition;
			}
		}

		private void ToolBarMouseUp(object sender, MouseEventArgs e)
		{
			if(_dragged != null)
			{
				_dragged = null;
				_ptOffset.X = 8;
				_ptOffset.Y = 8;
			}
		}

		private void ToolBarDoubleClick(object sender, System.EventArgs e)
		{
			ToolBarDockHolder holder = (ToolBarDockHolder)sender;
			if(IsDocked(holder))
			{
				ToolBarDockArea docked = GetDockedArea(holder);
				docked.SuspendLayout();
				holder.Parent = holder.FloatForm;
				holder.Location = new Point(0,0);
				holder.DockStyle = DockStyle.None;
				holder.FloatForm.Visible = true;
				holder.FloatForm.Size = holder.Size;
				docked.ResumeLayout();
				docked.PerformLayout();
			}
			else
			{
				ToolBarDockArea area = holder.PreferredDockedArea;
				area.SuspendLayout();
				Point newLoc = holder.PreferredDockedLocation;
				holder.DockStyle = area.Dock;
				holder.Parent = area;
				holder.PreferredDockedLocation = newLoc;
				holder.FloatForm.Visible = false;
				holder.PreferredDockedArea = area;
				area.ResumeLayout();				
				area.PerformLayout();
			}
		}


		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101; 
		bool _ctrlDown = false;

		public bool PreFilterMessage(ref Message m) 
		{
			if(m.Msg == WM_KEYDOWN) 
			{
				Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;
				if(keyCode == Keys.ControlKey) 
				{
					if(!_ctrlDown && _dragged!=null && IsDocked(_dragged)) 
					{
						ToolBarDockArea docked = GetDockedArea(_dragged);
						docked.SuspendLayout();
						_dragged.Parent = _dragged.FloatForm;
						_dragged.Location = new Point(0,0);
						_dragged.DockStyle = DockStyle.None;
						_dragged.FloatForm.Visible = true;
						_dragged.FloatForm.Location = new Point(Control.MousePosition.X-_ptOffset.X, Control.MousePosition.Y-8);
						_dragged.FloatForm.Size = _dragged.Size;
						docked.ResumeLayout();
						docked.PerformLayout();
					}
					_ctrlDown = true;
				}
			} 
			else if(m.Msg == WM_KEYUP) 
			{
				Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;
				if(keyCode == Keys.ControlKey) 
				{
					if(_ctrlDown && _dragged!=null && !IsDocked(_dragged)) 
					{
						ToolBarDockArea closest = GetClosestArea(Control.MousePosition, _dragged.PreferredDockedArea);
						if(closest != null)  
						{
							closest.SuspendLayout();
							Point newLoc = closest.PointToClient(Control.MousePosition);
							_dragged.DockStyle = closest.Dock;
							_dragged.Parent = closest;
							_dragged.PreferredDockedLocation = newLoc;
							_dragged.FloatForm.Visible = false;
							_dragged.PreferredDockedArea = closest;
							closest.ResumeLayout();
							closest.PerformLayout();
						}
					}
					_ctrlDown = false;
				}
			}
			return false;
		}

		class MyMenuItem : System.Windows.Forms.MenuItem
		{
			public Control Control;
		}

		// Added by mav
		public virtual void ShowContextMenu(Point ptScreen) 
		{
			System.Windows.Forms.ContextMenu cm = new System.Windows.Forms.ContextMenu();
			ArrayList al = new ArrayList();
			al.AddRange(_holders);
			al.Sort(new HolderSorter());

			MyMenuItem [] items = new MyMenuItem[al.Count];
			for(int i=0; i<al.Count; i++) 
			{	
				ToolBarDockHolder holder = al[i] as ToolBarDockHolder;
				Control c = holder.Control;
				items[i] = new MyMenuItem();
				items[i].Checked = c.Visible;
				items[i].Text = holder.ToolbarTitle;
				items[i].Click += new EventHandler(MenuClickEventHandler);
				items[i].Control = c;
				cm.MenuItems.Add(items[i]);
			}
			cm.Show(DockStation, DockStation.PointToClient(ptScreen));
		}

		protected void MenuClickEventHandler(object sender, EventArgs e) 
		{
			MyMenuItem item = (MyMenuItem)sender;
			ShowControl(item.Control, !item.Control.Visible);
		}

		private class HolderSorter : IComparer
		{
			#region IComparer Member

			public int Compare(object x, object y)
			{
				ToolBarDockHolder h1 = x as ToolBarDockHolder;
				ToolBarDockHolder h2 = y as ToolBarDockHolder;

				return h1.ToolbarTitle.CompareTo(h2.ToolbarTitle);
			}

			#endregion
		}

        public void Save(StreamWriter streamWriter)
        {
            //			int	index	= 0;
            //			int	count	= holders.Count;

            XmlTextWriter writer = new XmlTextWriter(streamWriter);

            writer.Formatting = Formatting.Indented;

            writer.WriteStartElement("root");

            foreach (ToolBarDockHolder holder in _holders)
            {
                Debug.Assert(holder != null);
                if (holder != null)
                {
                    Point prefLocation = holder.PreferredDockedLocation;
                    ToolBarDockArea area = holder.PreferredDockedArea;
                    Control parent = holder.Parent;
                    Point location = holder.Location;
                    DockStyle style = holder.DockStyle;
                    string areaString = "null";
                    string parentString = "null";
                    Point floatFormLocation = holder.FloatForm.Location;
                    Size floatFormSize = holder.FloatForm.Size;

                    // FloatForm

                    // Area
                    if (area == this._left)
                        areaString = "Left";
                    else if (area == this._right)
                        areaString = "Right";
                    else if (area == this._top)
                        areaString = "Top";
                    else if (area == this._bottom)
                        areaString = "Bottom";

                    // Parent
                    if (parent == this._left)
                        parentString = "Left";
                    else if (parent == this._right)
                        parentString = "Right";
                    else if (parent == this._top)
                        parentString = "Top";
                    else if (parent == this._bottom)
                        parentString = "Bottom";
                    else if (parent == holder.FloatForm)
                        parentString = "FloatForm";

                    WriteToolBarDockHolder(
                        writer,
                        holder.ToolbarTitle,
                        holder.Visible,
                        prefLocation,
                        location,
                        areaString,
                        parentString,
                        style,
                        holder.FloatForm.Visible,
                        floatFormLocation,
                        floatFormSize);

                    // holder
                    /*
                                        // holder.FloatForm
                                        writer.WriteLine(holder.FloatForm.Visible.ToString());
                                        writer.WriteLine(floatFormLocation.X + "," + floatFormLocation.Y);
                                        writer.WriteLine(floatFormSize.Width + "," + floatFormSize.Height);
                    */
                    //					index++;
                }
            }

            writer.WriteEndElement();

            writer.Flush();
            writer.Close();
        }

        public void WriteToolBarDockHolder(
            XmlTextWriter writer,
            string title,
            bool visible,
            Point prefLocation,
            Point location,
            string areaString,
            string parentString,
            DockStyle style,
            bool floatFormVisible,
            Point floatFormLocation,
            Size floatFormSize)
        {
            writer.WriteStartElement("ToolBarDockHolder");
            writer.WriteAttributeString("Title", title);
            writer.WriteElementString("Visible", XmlConvert.ToString(visible));

            //writer.WriteElementString("PrefLocation", XmlConvert.ToString(prefLocation));
            writer.WriteStartElement("PrefLocation");
            writer.WriteElementString("X", XmlConvert.ToString(prefLocation.X));
            writer.WriteElementString("Y", XmlConvert.ToString(prefLocation.Y));
            writer.WriteEndElement();

            //writer.WriteElementString("Location", XmlConvert.ToString(location));
            writer.WriteStartElement("Location");
            writer.WriteElementString("X", XmlConvert.ToString(location.X));
            writer.WriteElementString("Y", XmlConvert.ToString(location.Y));
            writer.WriteEndElement();

            writer.WriteElementString("Area", areaString);
            writer.WriteElementString("Parent", parentString);
            writer.WriteElementString("Style", style.ToString());


            writer.WriteStartElement("FloatForm");

            writer.WriteElementString("Visible", XmlConvert.ToString(floatFormVisible));

            writer.WriteStartElement("Location");
            writer.WriteElementString("X", XmlConvert.ToString(floatFormLocation.X));
            writer.WriteElementString("Y", XmlConvert.ToString(floatFormLocation.Y));
            writer.WriteEndElement();

            writer.WriteStartElement("Size");
            writer.WriteElementString("Width", XmlConvert.ToString(floatFormSize.Width));
            writer.WriteElementString("Height", XmlConvert.ToString(floatFormSize.Height));
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public void Load(StreamReader streamReader)
        {
            Debug.Assert(streamReader != null);

            try
            {
                XmlTextReader reader = new XmlTextReader(streamReader);

                reader.WhitespaceHandling = WhitespaceHandling.None;

                _left.SuspendLayout();
                _right.SuspendLayout();
                _top.SuspendLayout();
                _bottom.SuspendLayout();

                reader.Read();			// <root>
                if (reader.NodeType == XmlNodeType.Element)
                {
                    while (!reader.EOF)
                    {
                        string title = "";
                        bool visible = false;
                        Point prefLocation = new Point();
                        Point location = new Point();
                        string areaString = "";
                        string parentString = "";
                        string styleString = "";
                        bool floatFormVisible = false;
                        Point floatFormLocation = new Point();
                        Size floatFormSize = new Size();

                        ReadToolBarDockHolder(
                            reader,
                            ref title,
                            ref visible,
                            ref prefLocation,
                            ref location,
                            ref areaString,
                            ref parentString,
                            ref styleString,
                            ref floatFormVisible,
                            ref floatFormLocation,
                            ref floatFormSize);


                        if (title.Length > 0)
                        {
                            ToolBarDockHolder holder = this.GetHolder(title);
                            if (holder != null)
                            {
                                ToolBarDockArea area = null;
                                Control parent = null;

                                switch (areaString)
                                {
                                    case "Left":
                                        area = this._left;
                                        break;

                                    case "Right":
                                        area = this._right;
                                        break;

                                    case "Top":
                                        area = this._top;
                                        break;

                                    case "Bottom":
                                        area = this._bottom;
                                        break;
                                }

                                switch (parentString)
                                {
                                    case "Left":
                                        parent = this._left;
                                        break;

                                    case "Right":
                                        parent = this._right;
                                        break;

                                    case "Top":
                                        parent = this._top;
                                        break;

                                    case "Bottom":
                                        parent = this._bottom;
                                        break;

                                    case "FloatForm":
                                        parent = holder.FloatForm;
                                        break;
                                }

                                //							TypeConverter pointConverter	= TypeDescriptor.GetConverter(typeof(Point));
                                //							TypeConverter sizeConverter		= TypeDescriptor.GetConverter(typeof(Size));

                                //							Point	prefLocation		= (Point)pointConverter.ConvertFromString(prefLocationString);
                                //							Point	location			= (Point)pointConverter.ConvertFromString(locationString);
                                //							Point	floatFormLocation	= (Point)pointConverter.ConvertFromString(floatFormLocationString);
                                //							Size	floatFormSize		= (Size)sizeConverter.ConvertFromString(floatFormSizeString);

                                if (parent == holder.FloatForm)
                                {
                                    // i.e. Floating Toolbar
                                    holder.Parent = parent;
                                    holder.Location = location;
                                    holder.DockStyle = DockStyle.None;
                                    //holder.Visible					= visible;
                                    //holder.Size						= floatFormSize;
                                    holder.FloatForm.Visible = true;
                                    holder.FloatForm.Location = floatFormLocation;
                                    holder.FloatForm.Size = holder.Size;
                                    //holder.FloatForm.Size			= floatFormSize;

                                    holder.FloatForm.Visible = floatFormVisible;
                                }
                                else
                                {
                                    //holder.DockStyle				= (DockStyle)Enum.Parse(typeof(DockStyle), styleString, true);
                                    holder.DockStyle = parent.Dock;
                                    holder.Parent = parent;
                                    holder.PreferredDockedLocation = prefLocation;
                                    //holder.Location					= location;
                                    holder.Visible = visible;
                                    holder.FloatForm.Visible = floatFormVisible;
                                    holder.PreferredDockedArea = area;
                                    //holder.FloatForm.Location		= floatFormLocation;
                                    //holder.FloatForm.Size			= floatFormSize;
                                }
                            }
                        }
                    }
                }

                _left.ResumeLayout();
                _right.ResumeLayout();
                _top.ResumeLayout();
                _bottom.ResumeLayout();

                _left.PerformLayout();
                _right.PerformLayout();
                _top.PerformLayout();
                _bottom.PerformLayout();
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Info("", ex);
            }
        }

        private bool ReadToolBarDockHolder(
            XmlTextReader reader,
            ref string title,
            ref bool visible,
            ref Point prefLocation,
            ref Point location,
            ref string areaString,
            ref string parentString,
            ref string styleString,
            ref bool floatFormVisible,
            ref Point floatFormLocation,
            ref Size floatFormSize)
        {
            reader.Read();
            if (reader.NodeType == XmlNodeType.Element)
            {
                title = reader.GetAttribute("Title");

                visible = Convert.ToBoolean(reader.ReadElementString());

                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string prefLocationXString = reader.ReadElementString();
                    string prefLocationYString = reader.ReadElementString();

                    prefLocation = new Point(Convert.ToInt32(prefLocationXString), Convert.ToInt32(prefLocationYString));

                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        string locationXString = reader.ReadElementString();
                        string locationYString = reader.ReadElementString();

                        location = new Point(Convert.ToInt32(locationXString), Convert.ToInt32(locationYString));

                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            areaString = reader.ReadElementString();
                            parentString = reader.ReadElementString();
                            styleString = reader.ReadElementString();

                            reader.Read();
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                floatFormVisible = Convert.ToBoolean(reader.ReadElementString());

                                reader.Read();
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    string floatFormLocationXString = reader.ReadElementString();
                                    string floatFormLocationYString = reader.ReadElementString();

                                    floatFormLocation = new Point(Convert.ToInt32(floatFormLocationXString), Convert.ToInt32(floatFormLocationYString));

                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        string floatFormSizeWidthString = reader.ReadElementString();
                                        string floatFormSizeHeightString = reader.ReadElementString();

                                        floatFormSize = new Size(Convert.ToInt32(floatFormSizeWidthString), Convert.ToInt32(floatFormSizeHeightString));

                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }



	}
}
