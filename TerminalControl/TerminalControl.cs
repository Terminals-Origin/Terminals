using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;

namespace WalburySoftware
{
	public class TerminalEmulator : Control
	{
		#region Public Properties
		public int Rows
		{
			get
			{
				return this._rows;
			}
			set
			{
			}
		}
		public int Columns
		{
			get
			{
				return this._cols;
			}
			set
			{
			}
		}
		public ConnectionTypes ConnectionType
		{
			get
			{
				return this._ConnectionType;
			}
			set
			{
				this._ConnectionType = value;
			}
		}
		public string Hostname
		{
			get
			{
				return this._hostname;
			}
			set
			{
				this._hostname = value;
			}
		}
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
			}
		}
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}
		#endregion
		#region Public Methods
		public StringCollection ScreenScrape(int StartRow, int StartColumn, int EndRow, int EndColumn)
		{
			StringCollection ScrapedText = new StringCollection();


			return ScrapedText;
		}
		public void Write (byte[] data, int offset, int length)
		{
            string sReceived = Encoding.Default.GetString(data, offset, length);
			

			
			this.Invoke(this.RxdTextEvent, new System.String[] {System.String.Copy (sReceived)});
			this.Invoke(this.RefreshEvent);
		}
		public void Connect ()
		{
			switch (this.ConnectionType)
			{
				case ConnectionTypes.Telnet:
				{
					this.ConnectTelnet(this.Hostname);
					break;
				}
				case ConnectionTypes.SSH1:
				{
					break;
				}
				case ConnectionTypes.SSH2:
				{
					this.ConnectSSH2(this.Hostname, this.Username, this.Password);
					break;
				}
				default:
				{
					break;
				}
			}
		}
		#endregion
		#region public enums
		public enum ConnectionTypes
		{
			Telnet,
			SSH1,
			SSH2,
		}
		#endregion
		#region Fields
		private ConnectionTypes             _ConnectionType;
		private string						_hostname;       // used for connecting to SSH
		private string						_username;       // maybe
		private string						_password;
		private ContextMenu					contextMenu1;    // rightclick menu
		private MenuItem					mnuCopy;
		private MenuItem					mnuPaste;
		private MenuItem					mnuCopyPaste;
		private System.Drawing.Point		BeginDrag;       // used in Mouse Selecting Text
		private System.Drawing.Point		EndDrag;         // used in mouse selecting text
		private string						TextAtCursor;    // used to store Cursortext while scrolling
		private int							LastVisibleLine; // used for scrolling
		private System.AsyncCallback		callbackProc;
		private System.AsyncCallback		callbackEndDispatch;
		private System.Net.Sockets.Socket	CurSocket;
		private System.Boolean				XOFF    = false;
		private System.String				OutBuff = "";
		private Reader						reader;
		private int							ScrollbackBufferSize;
		private StringCollection			ScrollbackBuffer;
		private uc_Parser                    Parser         = null;
		private uc_TelnetParser              NvtParser      = null;
		private uc_Keyboard                  Keyboard       = null;
		private uc_TabStops                  TabStops       = null;
		private System.Drawing.Bitmap        EraseBitmap    = null;
		private System.Drawing.Graphics      EraseBuffer    = null;
		private System.Char[][]              CharGrid       = null;
		private CharAttribStruct[][]         AttribGrid     = null;
		private CharAttribStruct             CharAttribs;
		private System.Int32                 _cols;
		private System.Int32                 _rows;
		private System.Int32                 TopMargin;
		private System.Int32                 BottomMargin;
		private System.String                TypeFace      = FontFamily.GenericMonospace.GetName(0);
		private System.Drawing.FontStyle     TypeStyle     = System.Drawing.FontStyle.Regular;
		private System.Int32                 TypeSize      = 8;
		private System.Drawing.Size          CharSize;
		private System.Int32                 UnderlinePos; 
		private uc_Caret                     Caret;
		private System.Collections.ArrayList SavedCarets;
		private System.Drawing.Point         DrawStringOffset;
		private System.Drawing.Color         FGColor;
		private System.Drawing.Color         BoldColor;
		private System.Drawing.Color         BlinkColor;
		private uc_Chars                     G0;
		private uc_Chars                     G1;
		private uc_Chars                     G2;
		private uc_Chars                     G3;
		private  uc_Mode                      Modes;
		private uc_VertScrollBar             VertScrollBar;
		#endregion
		#region Delegates
		private delegate void NvtParserEventHandler (object Sender, NvtParserEventArgs e);
		private delegate void KeyboardEventHandler (object Sender, System.String e);
		private delegate void RefreshEventHandler ();
		private delegate void RxdTextEventHandler (System.String sReceived);
		private delegate void CaretOffEventHandler ();
		private delegate void CaretOnEventHandler ();
		private delegate void ParserEventHandler (object Sender, ParserEventArgs e);

        public delegate void Disconnected(object Sender, string Message);

		#endregion
		#region Events
		private event RefreshEventHandler RefreshEvent;
		private event RxdTextEventHandler RxdTextEvent;
		private event CaretOffEventHandler CaretOffEvent;
		private event CaretOnEventHandler CaretOnEvent;

        public event Disconnected OnDisconnected;
		#endregion
		#region Constructors

		public TerminalEmulator ()
		{ 
		
			this.ScrollbackBufferSize = 3000;
			this.ScrollbackBuffer = new StringCollection();

			// set the display options
			this.SetStyle (ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

			this.Keyboard       = new uc_Keyboard (this);
			this.Parser         = new uc_Parser   ();
			this.NvtParser      = new uc_TelnetParser   ();
			this.Caret          = new uc_Caret    ();
			this.Modes          = new uc_Mode ();
			this.TabStops       = new uc_TabStops ();
			this.SavedCarets    = new System.Collections.ArrayList ();

			//this.Name       = "ACK-TERM";
			//this.Text       = "ACK-TERM";

			this.Caret.Pos  = new System.Drawing.Point (0, 0); 
			this.CharSize   = new System.Drawing.Size ();
			this.Font       = new System.Drawing.Font (this.TypeFace, this.TypeSize, this.TypeStyle);
			//this.Font       = new System.Drawing.Font(FontFamily.GenericMonospace, 8.5F);

			//this.FGColor      = System.Drawing.Color.FromArgb (200, 200, 200);
			this.FGColor      = System.Drawing.Color.GreenYellow;
			this.BackColor    = System.Drawing.Color.FromArgb (0, 0, 160);
			this.BoldColor    = System.Drawing.Color.FromArgb (255, 255, 255);
			this.BlinkColor   = System.Drawing.Color.Red;

            this.G0 = new uc_Chars(uc_Chars.Sets.ASCII);
            this.G1 = new uc_Chars(uc_Chars.Sets.ASCII);
			this.G2 = new uc_Chars (uc_Chars.Sets.DECSG);
			this.G3 = new uc_Chars (uc_Chars.Sets.DECSG);

			this.CharAttribs.GL = G0;
			this.CharAttribs.GR = G2;
			this.CharAttribs.GS = null;

			this.GetFontInfo ();

			// Create and initialize contextmenu
			this.contextMenu1 = new ContextMenu();
			this.mnuCopy = new MenuItem("Copy");
			this.mnuPaste = new MenuItem("Paste");
			this.mnuCopyPaste = new MenuItem("Copy and Paste");
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnuCopyPaste,
																						 this.mnuPaste,
																						 this.mnuCopy});
			this.mnuCopy.Index = 0;
			this.mnuPaste.Index = 1;
			this.mnuCopyPaste.Index = 2;
			this.ContextMenu = this.contextMenu1;
			this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
			this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
			this.mnuCopyPaste.Click += new System.EventHandler(this.mnuCopyPaste_Click);


			// Create and initialize a VScrollBar.
			VertScrollBar = new uc_VertScrollBar();
			VertScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);

			// Dock the scroll bar to the right side of the form.
			VertScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
    
			// Add the scroll bar to the form.
			Controls.Add (VertScrollBar);

			// create the character grid (rows by columns). This is a shadow of what's displayed
			// Set the window size to match
			this.SetSize (24, 80);

			this.Parser.ParserEvent       += new ParserEventHandler    (CommandRouter);
			this.Keyboard.KeyboardEvent   += new KeyboardEventHandler  (DispatchMessage);
			this.NvtParser.NvtParserEvent += new NvtParserEventHandler (TelnetInterpreter);
			this.RefreshEvent             += new RefreshEventHandler   (ShowBuffer);
			this.CaretOffEvent            += new CaretOffEventHandler  (this.CaretOff);
			this.CaretOnEvent             += new CaretOnEventHandler   (this.CaretOn);
			this.RxdTextEvent             += new RxdTextEventHandler   (this.NvtParser.ParseString);

			this.BeginDrag = new System.Drawing.Point();
			this.EndDrag   = new System.Drawing.Point();
			
			this.ConnectionType = ConnectionTypes.Telnet; // default
			
		}

		#endregion
		#region Overrides
		protected override void OnResize (System.EventArgs e)
		{
			this.Font       = new System.Drawing.Font (this.TypeFace, this.TypeSize, this.TypeStyle);
			// reset scrollbar values
			this.SetScrollBarValues();

			// capture text at cursor b/c it's not in the scrollback buffer yet
			string TextAtCursor = "";
			for (int x = 0; x < this._cols; x++)
			{
				char CurChar = this.CharGrid[this.Caret.Pos.Y][x];

				if (CurChar == '\0')
				{
					continue;
				}
				TextAtCursor = TextAtCursor + Convert.ToString(CurChar);
			}

			
			
			// calculate new rows and columns
			int columns = this.ClientSize.Width / this.CharSize.Width - 1;
			int rows = this.ClientSize.Height / this.CharSize.Height;

			// make sure at least 1 row and 1 col or Control will throw
			if (rows < 5)
			{
				rows = 5;
				this.Height = this.CharSize.Height * rows;
			}

			// make sure at least 1 row and 1 col or Control will throw
			if (columns < 5)
			{
				columns = 5;
				this.Width = this.CharSize.Width * columns;
			}
			
			// make sure the bottom of this doesn't exceed bottom of parent client area
			// for some reason it was getting stuck like that
			if (this.Parent != null)
				if (this.Bottom > this.Parent.ClientSize.Height)
					this.Height = this.Parent.ClientSize.Height - this.Top;

			// reset the char grid
			this.SetSize(rows, columns);

			//Console.WriteLine(Convert.ToString(rows) + " rows. " + Convert.ToString(this.ScrollbackBuffer.Count + " buffer lines"));
			
			// populate char grid from ScrollbackBuffer

			// parse through ScrollbackBuffer from the end
			// ScrollbackBuffer[0] is the "oldest" string
			// chargrid[0] is the top row on the display
			StringCollection visiblebuffer = new StringCollection();
			for (int i = this.ScrollbackBuffer.Count - 1; i >= 0; i--)
			{
				
				
				visiblebuffer.Insert(0, this.ScrollbackBuffer[i]);

				// don't parse more strings than our display can show
				if (visiblebuffer.Count >= rows - 1) // rows -1 to leave line for cursor space
					break;
			}

			int lastline  = 0;
			for (int i = 0; i < visiblebuffer.Count; i++)
			{
				//Console.WriteLine("Writing string to display: " + visiblebuffer[i]);
				for (int column = 0; column < columns; column++)
				{
					//this.CharGrid[i][column] = '0';
					if (column > visiblebuffer[i].Length - 1)
						continue;
					this.CharGrid[i][column] = visiblebuffer[i].ToCharArray()[column];

				}
				lastline = i;
			
			}

			// replace cursor text
			for (int column = 0; column < columns; column++)
			{
				if (column > TextAtCursor.Length - 1)
					continue;
				this.CharGrid[lastline+1][column] = TextAtCursor.ToCharArray()[column];
			
			}

			this.CaretToAbs(lastline+1, TextAtCursor.Length);
			this.Refresh();
			
			base.OnResize(e);
			
		}
			
		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.SmoothingMode     = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
			e.Graphics.TextContrast      = 0;
			e.Graphics.PixelOffsetMode   = System.Drawing.Drawing2D.PixelOffsetMode.Half;

			this.WipeScreen (e.Graphics);
			this.Redraw     (e.Graphics);
			this.ShowCaret  (e.Graphics);
		}

		protected override void OnPaintBackground (System.Windows.Forms.PaintEventArgs e)
		{
		}

		protected override void WndProc (ref System.Windows.Forms.Message m)
		{
			// Listen for operating system messages and handle the key events.
			switch (m.Msg)
			{
				case WMCodes.WM_KEYDOWN:
				case WMCodes.WM_SYSKEYDOWN:
				case WMCodes.WM_KEYUP:
				case WMCodes.WM_SYSKEYUP:
				case WMCodes.WM_SYSCHAR:
				case WMCodes.WM_CHAR:
					this.Keyboard.KeyDown (m);
					break;

				default:
					// don't do any default handling for the aforementioned events
					// this means things like keyboard shortcut events are ignored
					base.WndProc (ref m);
					break;               
			}
		}

		protected override void OnMouseMove (MouseEventArgs CurArgs)
		{
			if (CurArgs.Button != MouseButtons.Left)
				return;

			//Console.WriteLine(Convert.ToString(CurArgs.X) + "," + Convert.ToString(CurArgs.Y));
			this.EndDrag.X = CurArgs.X;
			this.EndDrag.Y = CurArgs.Y;

			int endCol = this.EndDrag.X / this.CharSize.Width;
			int endRow = this.EndDrag.Y / this.CharSize.Height;

			int begCol = this.BeginDrag.X / this.CharSize.Width;
			int begRow = this.BeginDrag.Y / this.CharSize.Height;


			// reset highlights
			for (int iRow = 0; iRow < this._rows; iRow++)
				for (int iCol = 0; iCol < this._cols; iCol++)
					this.AttribGrid[iRow][iCol].IsInverse = false;


			if (endRow < begRow) // we're parsing backwards
			{
				int i = endRow;
				endRow = begRow;
				begRow = i;
				for (int curRow = begRow; curRow <= endRow; curRow++)
				{
					if (curRow <= 0)
						continue;

					for (int curCol = 0; curCol < this._cols; curCol++)
					{
						// don't select if nothing is there
						if (this.CharGrid[curRow][curCol] == '\0')
							continue;

						// on first row, make sure we start at begCol
						if (curRow == begRow && curCol < endCol)
							continue;		
						
						// on last row, don't pass the end col
						if (curRow == endRow && curCol == begCol)
						{
							this.AttribGrid[curRow][curCol].IsInverse = true;
							break;
						}

						this.AttribGrid[curRow][curCol].IsInverse = true;
					}
				}
				this.Refresh();
				return;
			}

			if (endCol < begCol && begRow == endRow) // we're parsing backwards, but only on one row
			{
				int i = endCol;
				endCol = begCol;
				begCol = i;
			}

			// parse the rows affected and highlight them
			// endRow/endCol are where the mouse is now
			// begRow/begCol are where the mouse was when the button was pushed
			for (int curRow = begRow; curRow <= endRow; curRow++)
			{
				if (curRow >= this._rows)
					break;

				for (int curCol = 0; curCol < this._cols; curCol++)
				{			
					// don't select if nothing is there
					if (this.CharGrid[curRow][curCol] == '\0')
						continue;

					// on first row, make sure we start at begCol
					if (curRow == begRow && curCol < begCol)
						continue;		

					// on last row, don't pass the end col
					if (curRow == endRow && curCol == endCol)
					{
						this.AttribGrid[curRow][curCol].IsInverse = true;
						break;
					}
					this.AttribGrid[curRow][curCol].IsInverse = true;
				}
			}
			this.Refresh();
		}
		
		protected override void OnMouseUp (MouseEventArgs CurArgs)
		{
			if (CurArgs.Button == System.Windows.Forms.MouseButtons.Left)
			{

				if (this.BeginDrag.X == CurArgs.X && this.BeginDrag.Y == CurArgs.Y)
				{	// reset highlights
					for (int iRow = 0; iRow < this._rows; iRow++)
						for (int iCol = 0; iCol < this._cols; iCol++)
							this.AttribGrid[iRow][iCol].IsInverse = false;
					this.Refresh();
				}				
			}
		
		}

		protected override void OnMouseDown (System.Windows.Forms.MouseEventArgs CurArgs)
		{
			this.Focus();
			
			//Font tmp      = new System.Drawing.Font(FontFamily.GenericMonospace, 8.5F);
			//Graphics g = this.CreateGraphics();
			//g.DrawString("the quick brown fox jumps over the lazy dog", this.Font,Brushes.GreenYellow, 0,0);
			//g.DrawString("*******************************************", tmp,Brushes.GreenYellow, 0,0);
			//g.DrawString("This system is private and may only be accessed if authorized.", this.Font,Brushes.GreenYellow, 0,0);
			
			//
			//if (CurArgs.Button == System.Windows.Forms.MouseButtons.Right)
			//{
			//	// Get the clipboard text
			//	System.Windows.Forms.IDataObject CurDataObject = System.Windows.Forms.Clipboard.GetDataObject ();
			  
			//	if(CurDataObject.GetDataPresent (System.Windows.Forms.DataFormats.Text)) 
			//	{
			//		if (CurDataObject.GetData (System.Windows.Forms.DataFormats.Text) != null)
			//		{
			//			this.DispatchMessage (
			//				this, 
			//				CurDataObject.GetData (System.Windows.Forms.DataFormats.Text).ToString ()); 
			//		}
			//	}
			//}
 
			if (CurArgs.Button == System.Windows.Forms.MouseButtons.Left)
			{
				// begin select
				this.BeginDrag.X = CurArgs.X;
				this.BeginDrag.Y = CurArgs.Y;
			}
			
			
			base.OnMouseDown (CurArgs);
		}

		protected override void OnFontChanged (EventArgs e)
		{
			//MessageBox.Show(this.Font.Name + " " + Convert.ToString(this.Font.Size));


		}
		#endregion
		#region Private Methods
		private void ConnectTelnet(string HostName)
		{
			this.Focus();

			System.Int32           port    = 23;
			//System.Net.IPHostEntry IPHost  = System.Net.Dns.GetHostEntry(HostName); 
			System.Net.IPHostEntry IPHost = System.Net.Dns.GetHostByName(HostName);
			System.Net.IPAddress[] addr    = IPHost.AddressList; 
        
			try
			{
				// Create New Socket 
				this.CurSocket = new System.Net.Sockets.Socket (
					System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, 
					System.Net.Sockets.ProtocolType.Tcp);

				// Create New EndPoint
				System.Net.IPEndPoint iep  = new System.Net.IPEndPoint(addr[0],port);  

				// This is a non blocking IO
				this.CurSocket.Blocking        = false ;    

				// Begin Asyncronous Connection
				this.CurSocket.BeginConnect (iep,  new System.AsyncCallback (ConnectCallback), CurSocket) ;                
			}
			catch (System.Exception CurException)
			{
                //System.Console.WriteLine ("Connect: " + CurException.Message);
				MessageBox.Show(CurException.Message);
			}
		}
		private void ConnectSSH2(string hostname, string username, string password)
		{
			// connect ssh
			this.Focus();
			Routrek.SSHC.SSHConnection _conn;
			Routrek.SSHC.SSHConnectionParameter f = new Routrek.SSHC.SSHConnectionParameter();
			f.UserName = username;
			f.Password = password;
			f.Protocol = Routrek.SSHC.SSHProtocol.SSH2;
			
			f.AuthenticationType = Routrek.SSHC.AuthenticationType.Password;
			f.WindowSize = 0x1000;
			this.reader = new Reader(this);
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//s.Blocking = false;
			IPAddress ip;
			try
			{
				ip = Dns.GetHostByName(hostname).AddressList[0];
			}
			catch(Exception exc)
			{
                MessageBox.Show("Unable to resolve HostName");
				return;
			}

			//s.Connect(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 22));
			s.Connect(new IPEndPoint(ip, 22));
			_conn = Routrek.SSHC.SSHConnection.Connect(f, reader, s);
			reader._conn = _conn;
			Routrek.SSHC.SSHChannel ch = _conn.OpenShell(reader);
			reader._pf = ch;
			Routrek.SSHC.SSHConnectionInfo ci = _conn.ConnectionInfo;
		}
		private void mnuCopy_Click (object sender, System.EventArgs e)
		{
			Point start = new Point();
			Point stop  = new Point();
			bool FoundStart = false;
			bool FoundStop = false;

			// find coordinates of Highlighted Text
			for (int row = 0; row < this._rows; row++)
			{
				for (int col = 0; col < this._cols; col++)
				{
					if (FoundStart == false && this.AttribGrid[row][col].IsInverse == true)
					{
						start.X = col;
						start.Y = row;
						FoundStart = true;
					}

					// this next check will first find the first non-inverse coord with a
					// character in it. If it happens to be at the beginning of a line, 
					// then we'll back up and stop at the last char in the prev line
					if (FoundStart == true && 
						FoundStop == false && 
						this.AttribGrid[row][col].IsInverse == false &&
						this.CharGrid[row][col] != '\0'
						)
					{
						stop.X = col - 1;
						stop.Y = row;
						FoundStop = true;
						// here we back up if col == 0
						if (col == 0)
						{
							// make sure we don't have a null row
							// this shouldn't throw
							row--;
							while (this.CharGrid[row][0] == '\0')
								row--;
							for (col = 0; col < this._cols; col++) // parse the row
							{
								if (this.CharGrid[row][col] == '\0') // we found the end
								{
									stop.X = col - 1;
									stop.Y = row;
								}
							}
						}
						break;
					}

					if (FoundStop == true && FoundStart == true)
						break;
				} // column parse
				// if we get to this point without finding a match, and we're on the last row
				// we should include the last row and stop here
				if (FoundStart == true && FoundStop == false && row == this._rows -1)
				{
					for (int col = 0; col < this._cols; col++) // parse the row
					{
						if (this.CharGrid[row][col] == '\0') // we found the end
						{
							stop.X = col - 1;
							stop.Y = row;
						}
					}
				}
			} // row parse
			Console.WriteLine("start.Y " + Convert.ToString(start.Y) +
				             " start.X " + Convert.ToString(start.X) +
				             " stop.Y "  + Convert.ToString(stop.Y)  +
				             " stop.X "  + Convert.ToString(stop.X));

			StringCollection sc = this.ScreenScrape (start.Y, start.X, stop.Y, stop.X);
			foreach (string s in sc)
			{
				Console.WriteLine(s);
			}

		}
		private void mnuPaste_Click (object sender, System.EventArgs e)
		{}
		private void mnuCopyPaste_Click (object sender, System.EventArgs e)
		{}
		private void HandleScroll(Object sender, ScrollEventArgs se)
		{
			// capture text at cursor
			if (this.Caret.IsOff)
			{
			}
			else
			{
				this.TextAtCursor = "";
				for (int x = 0; x < this._cols; x++)
				{
					char CurChar = this.CharGrid[this.Caret.Pos.Y][x];
					if (CurChar == '\0')
					{
						continue;
					}
					this.TextAtCursor = this.TextAtCursor + Convert.ToString(CurChar);
				}
			}
	
			switch (se.Type)
			{
				case ScrollEventType.SmallIncrement: // down
					this.LastVisibleLine += 1;
					break;
				case ScrollEventType.SmallDecrement: // up
					this.LastVisibleLine += -1;
					break;
				default:
					return;
			}

			// make sure we don't set LastVisibleLine past the end of the StringCollection
			// LastVisibleLine is relative to the last line in the StringCollection
			// 0 is the Last element
			if (this.LastVisibleLine > 0)
			{
				this.LastVisibleLine = 0;
			}

			if (this.LastVisibleLine < (0 - this.ScrollbackBuffer.Count) + (this._rows))
			{
				this.LastVisibleLine = (0 - this.ScrollbackBuffer.Count) + (this._rows)-1;
			}


			int columns = this._cols;
			int rows    = this._rows;
			
			this.SetSize(rows, columns);

			StringCollection visiblebuffer = new StringCollection();
			for (int i = this.ScrollbackBuffer.Count - 1 + this.LastVisibleLine; i >= 0; i--)
			{
				visiblebuffer.Insert(0, this.ScrollbackBuffer[i]);

				// don't parse more strings than our display can show
				if (visiblebuffer.Count >= rows - 1) // rows -1 to leave line for cursor space
					break;
			}

			//int lastline = (0 - this.ScrollbackBuffer.Count) + (this.Rows);
			//int lastline = this.LastVisibleLine;
			for (int i = 0; i < visiblebuffer.Count; i++)
			{
				//Console.WriteLine("Writing string to display: " + visiblebuffer[i]);
				for (int column = 0; column < columns; column++)
				{
					//this.CharGrid[i][column] = '0';
					if (column > visiblebuffer[i].Length - 1)
						continue;
					this.CharGrid[i][column] = visiblebuffer[i].ToCharArray()[column];

				}
				// if we're displaying the last line in scrollbackbuffer, then
				// replace the cursor and the text on the cursor line
				//System.Console.WriteLine(Convert.ToString(lastline) + " " + Convert.ToString(this.ScrollbackBuffer.Count));
				if (this.LastVisibleLine == 0)
				{
					this.CaretOn();
					//this.CaretToAbs(0,0);
					for (int column = 0; column < this._cols; column++)
					{
						if (column > this.TextAtCursor.Length - 1)
							continue;
						this.CharGrid[this._rows - 1][column] = this.TextAtCursor.ToCharArray()[column];
					}
					this.CaretToAbs(this._rows - 1, this.TextAtCursor.Length);
				}
				else
				{
					this.CaretOff();
				}
			}

			this.Refresh();
		}
		private void SetScrollBarValues()
		{
			// Set the Maximum, Minimum, LargeChange and SmallChange properties.
			this.VertScrollBar.Minimum = 0;
			
			// if the scrollbackbuffer is empty, there's nothing to scroll
			if (this.ScrollbackBuffer.Count == 0)
			{
				this.VertScrollBar.Maximum = 0;
				return;
			}

			// If the offset does not make the Maximum less than zero, set its value.    
			if ((this.ScrollbackBuffer.Count * this.CharSize.Height) - this.Height > 0)
			{

				this.VertScrollBar.Maximum = this.ScrollbackBuffer.Count * this.CharSize.Height - this.Height;
			}

			// If the HScrollBar is visible, adjust the Maximum of the 
			// VSCrollBar to account for the width of the HScrollBar.
			//if(this.hScrollBar1.Visible)
			//{
			//	this.vScrollBar1.Maximum += this.hScrollBar1.Height;
			//}
			this.VertScrollBar.LargeChange = this.VertScrollBar.Maximum / this.CharSize.Height * 10;
			this.VertScrollBar.SmallChange = this.VertScrollBar.Maximum / this.CharSize.Height;
			// Adjust the Maximum value to make the raw Maximum value 
			// attainable by user interaction.
			this.VertScrollBar.Maximum += this.VertScrollBar.LargeChange;

		}
		private void ConnectCallback (System.IAsyncResult ar)
		{
			try
			{
				// Get The connection socket from the callback
				System.Net.Sockets.Socket sock1 = (System.Net.Sockets.Socket) ar.AsyncState;

				if (sock1.Connected) 
				{
					uc_CommsStateObject StateObject = new uc_CommsStateObject ();

					StateObject.Socket = sock1;
    
					// Assign Callback function to read from Asyncronous Socket
					callbackProc = new System.AsyncCallback (OnReceivedData);

					// Begin reading data asyncronously
					sock1.BeginReceive (StateObject.Buffer, 0, StateObject.Buffer.Length, 
						System.Net.Sockets.SocketFlags.None, callbackProc, StateObject);
				}
			}

			catch (System.Exception CurException)
			{
                //System.Console.WriteLine ("ConnectCallback: " + CurException.Message);
				MessageBox.Show(CurException.Message);
			}
		}

		private void OnReceivedData (System.IAsyncResult ar)
		{
			// Get The connection socket from the callback
			uc_CommsStateObject StateObject = (uc_CommsStateObject) ar.AsyncState;

			// Get The data , if any
			int nBytesRec = StateObject.Socket.EndReceive (ar);        

			if ( nBytesRec > 0 )
			{
				string sReceived = "";

				for (int i = 0; i < nBytesRec; i++)
				{
					sReceived += System.Convert.ToChar (StateObject.Buffer[i]).ToString ();
				}

				this.Invoke (this.RxdTextEvent, new System.String[] {System.String.Copy (sReceived)});
				this.Invoke (this.RefreshEvent);

				// Re-Establish the next asyncronous receveived data callback as
				StateObject.Socket.BeginReceive (StateObject.Buffer, 0, StateObject.Buffer.Length, 
					System.Net.Sockets.SocketFlags.None, new System.AsyncCallback (OnReceivedData) , StateObject);
			}
			else
			{
				// If no data was recieved then the connection is probably dead
				//System.Console.WriteLine ("Disconnected", StateObject.Socket.RemoteEndPoint);

				StateObject.Socket.Shutdown (System.Net.Sockets.SocketShutdown.Both);

				StateObject.Socket.Close ();
			}
		}
		
		private void DispatchMessage (System.Object sender, string strText)
		{
			//Console.WriteLine(strText);
			if (this.XOFF == true)
			{
				// store the characters in the outputbuffer
				OutBuff += strText;
				return;
			}

			int i = 0;

			try
			{
				System.Byte[] smk = new System.Byte[strText.Length];

				if (OutBuff != "")
				{
					strText = OutBuff + strText;
					OutBuff = "";
				}

				for (i=0; i < strText.Length ; i++)
				{
					System.Byte ss = System.Convert.ToByte (strText[i]);
					smk[i] = ss ;
				}

				if (callbackEndDispatch == null)
				{
					callbackEndDispatch = new System.AsyncCallback (EndDispatchMessage);
				}

				if (this.CurSocket == null)
				{
					try
					{
						reader._pf.Transmit(smk);
					}
					catch(Exception exc)
					{
                        if (this.OnDisconnected != null) this.OnDisconnected(this, exc.Message);
					}				
				}
				else
				{
					try
					{
						System.IAsyncResult ar = this.CurSocket.BeginSend (
							smk, 
							0, 
							smk.Length, 
							System.Net.Sockets.SocketFlags.None, 
							callbackEndDispatch, 
							this.CurSocket);
					}
                    catch (Exception exc)
                    {
                        if (this.OnDisconnected != null) this.OnDisconnected(this, exc.Message);
                    }
				}

			}
			catch (System.Exception CurException)
			{
                //System.Console.WriteLine ("DispatchMessage: " + CurException.Message);
				//System.Console.WriteLine ("DispatchMessage: Character is " + System.Convert.ToInt32 (strText[i]));
				//System.Console.WriteLine ("DispatchMessage: String is " + strText);
                //MessageBox.Show("DispatchMessage: " + CurException.Message);
                //MessageBox.Show("DispatchMessage: Character is " + System.Convert.ToInt32 (strText[i]));
                //MessageBox.Show("DispatchMessage: String is " + strText);
			}
		}

		private void EndDispatchMessage (System.IAsyncResult ar)
		{
			try
			{
				System.Net.Sockets.Socket Sock =  (System.Net.Sockets.Socket) ar.AsyncState;

				Sock.EndSend (ar);
			}
			catch (System.Exception CurException)
			{
                //System.Console.WriteLine ("EndDispatchMessage: " + CurException.Message);
				MessageBox.Show("EndDispatchMessage: " + CurException.Message);
			}
		}		
		private void PrintChar (System.Char CurChar)
		{
			if (this.Caret.EOL == true)
			{
				if ((this.Modes.Flags & uc_Mode.AutoWrap) == uc_Mode.AutoWrap)
				{
					this.LineFeed ();
					this.CarriageReturn ();
					this.Caret.EOL = false;
				}
			}

			System.Int32 X = this.Caret.Pos.X;
			System.Int32 Y = this.Caret.Pos.Y;

			this.AttribGrid[Y][X] = this.CharAttribs;

			if (this.CharAttribs.GS != null)
			{
				CurChar = uc_Chars.Get (CurChar, this.AttribGrid[Y][X].GS.Set, this.AttribGrid[Y][X].GR.Set);

				if (this.CharAttribs.GS.Set == uc_Chars.Sets.DECSG) this.AttribGrid[Y][X].IsDECSG = true;

				this.CharAttribs.GS = null;
			}
			else
			{
				CurChar = uc_Chars.Get (CurChar, this.AttribGrid[Y][X].GL.Set, this.AttribGrid[Y][X].GR.Set);

				if (this.CharAttribs.GL.Set == uc_Chars.Sets.DECSG) this.AttribGrid[Y][X].IsDECSG = true;
			}   

			this.CharGrid[Y][X] = CurChar;

			this.CaretRight ();
		}

		private System.Drawing.Point GetDrawStringOffset (System.Drawing.Graphics CurGraphics, System.Int32 X, System.Int32 Y, System.Char CurChar)
		{
			// DrawString doesn't actually print where you tell it to but instead consistently prints
			// with an offset. This is annoying when the other draw commands do not print with an offset
			// this method returns a point defining the offset so we can take it off the printstring command.

			System.Drawing.CharacterRange[] characterRanges =
		   {
			   new System.Drawing.CharacterRange(0, 1)
		   };
 
			System.Drawing.RectangleF layoutRect = new System.Drawing.RectangleF (X, Y, 100, 100);

			System.Drawing.StringFormat stringFormat = new System.Drawing.StringFormat();

			stringFormat.SetMeasurableCharacterRanges(characterRanges);

			System.Drawing.Region[] stringRegions = new System.Drawing.Region[1];

			stringRegions = CurGraphics.MeasureCharacterRanges(
				CurChar.ToString (),
				this.Font,
				layoutRect,
				stringFormat);

			System.Drawing.RectangleF measureRect1 = stringRegions[0].GetBounds (CurGraphics);

			return new System.Drawing.Point ((int) (measureRect1.X + 0.5), (int) (measureRect1.Y + 0.5));
		}
        
		private System.Drawing.Point GetCharSize (System.Drawing.Graphics CurGraphics)
		{
			// DrawString doesn't actually print where you tell it to but instead consistently prints
			// with an offset. This is annoying when the other draw commands do not print with an offset
			// this method returns a point defining the offset so we can take it off the printstring command.

			System.Drawing.CharacterRange[] characterRanges =
		   {
			   new System.Drawing.CharacterRange(0, 1)
		   };
 
			System.Drawing.RectangleF layoutRect = new System.Drawing.RectangleF (0, 0, 100, 100);

			System.Drawing.StringFormat stringFormat = new System.Drawing.StringFormat();

			stringFormat.SetMeasurableCharacterRanges(characterRanges);

			System.Drawing.Region[] stringRegions = new System.Drawing.Region[1];

			stringRegions = CurGraphics.MeasureCharacterRanges(
				"A",
				this.Font,
				layoutRect,
				stringFormat);

			System.Drawing.RectangleF measureRect1 = stringRegions[0].GetBounds (CurGraphics);

			return new System.Drawing.Point ((int) (measureRect1.Width + 0.5), (int) (measureRect1.Height + 0.5));
		}

		private void AssignColors (CharAttribStruct CurAttribs, ref System.Drawing.Color CurFGColor, ref System.Drawing.Color CurBGColor)
		{

			CurFGColor = this.FGColor; 
			CurBGColor = this.BackColor;

			if (CurAttribs.IsBlinking == true)
			{
				CurFGColor = this.BlinkColor;
			}

			// bold takes precedence over the blink color
			if (CurAttribs.IsBold == true)
			{
				CurFGColor = this.BoldColor;
			}

			if (CurAttribs.UseAltColor == true)
			{
				CurFGColor = CurAttribs.AltColor;
			}

			// alternate color takes precedence over the bold color
			if (CurAttribs.UseAltBGColor == true)
			{
				CurBGColor = CurAttribs.AltBGColor;
			}

			if (CurAttribs.IsInverse == true)
			{
				System.Drawing.Color TmpColor = CurBGColor;

				CurBGColor = CurFGColor;
				CurFGColor = TmpColor;
			}

			// If light background is on and we're not using alt colors
			// reverse the colors
			if ((this.Modes.Flags & uc_Mode.LightBackground) > 0 &&
				CurAttribs.UseAltColor == false && CurAttribs.UseAltBGColor == false)
			{

				System.Drawing.Color TmpColor = CurBGColor;

				CurBGColor = CurFGColor;
				CurFGColor = TmpColor;
			}
		}

		private void ShowChar (System.Drawing.Graphics CurGraphics, System.Char CurChar, System.Int32 Y, System.Int32 X, CharAttribStruct CurAttribs)
		{
			if (CurChar == '\0')
			{
				return;
			}

			System.Drawing.Color CurFGColor = System.Drawing.Color.White; 
			System.Drawing.Color CurBGColor = System.Drawing.Color.Black;

			this.AssignColors (CurAttribs, ref CurFGColor, ref CurBGColor);

			if ((CurBGColor != this.BackColor && (this.Modes.Flags & uc_Mode.LightBackground) == 0) ||
				(CurBGColor != this.FGColor   && (this.Modes.Flags & uc_Mode.LightBackground) > 0))
			{

				// Erase the current Character underneath the cursor postion
				this.EraseBuffer.Clear (CurBGColor);

				// paint a rectangle over the cursor position in the character's BGColor
				CurGraphics.DrawImageUnscaled (
					this.EraseBitmap, 
					X, 
					Y);
			}

			if (CurAttribs.IsUnderscored)
			{
				CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
					X,                       Y + this.UnderlinePos,   
					X + this.CharSize.Width, Y + this.UnderlinePos);
			}   

			if ((CurAttribs.IsDECSG == true) &&
				(CurChar == 'l' ||
				CurChar == 'q' ||
				CurChar == 'w' ||
				CurChar == 'k' ||
				CurChar == 'x' ||
				CurChar == 't' ||
				CurChar == 'n' ||
				CurChar == 'u' ||
				CurChar == 'm' ||
				CurChar == 'v' ||
				CurChar == 'j' ||
				CurChar == '`'))
			{
				this.ShowSpecialChar (
					CurGraphics,
					CurChar, 
					Y,
					X,
					CurFGColor,
					CurBGColor);

				return;               
			}

			CurGraphics.DrawString (
				CurChar.ToString (), 
				this.Font, 
				new System.Drawing.SolidBrush (CurFGColor),
				X - this.DrawStringOffset.X,
				Y - this.DrawStringOffset.Y);

		}

		private void ShowSpecialChar (System.Drawing.Graphics CurGraphics, System.Char CurChar, System.Int32 Y, System.Int32 X, System.Drawing.Color CurFGColor, System.Drawing.Color CurBGColor)
		{
			if (CurChar == '\0')
			{
				return;
			}

			switch (CurChar)
			{
				case '`': // diamond
					System.Drawing.Point[] CurPoints = new System.Drawing.Point[4];

					CurPoints[0] = new System.Drawing.Point (X + this.CharSize.Width/2, Y + this.CharSize.Height/6);
					CurPoints[1] = new System.Drawing.Point (X + 5*this.CharSize.Width/6, Y + this.CharSize.Height/2);
					CurPoints[2] = new System.Drawing.Point (X + this.CharSize.Width/2, Y + 5*this.CharSize.Height/6);
					CurPoints[3] = new System.Drawing.Point (X + this.CharSize.Width/6, Y + this.CharSize.Height/2);

					CurGraphics.FillPolygon (
						new System.Drawing.SolidBrush (CurFGColor),
						CurPoints);   
					break;

				case 'l': // top left bracket
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2 -1,   Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,     Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2,   Y + this.CharSize.Height/2,   
						X + this.CharSize.Width/2,   Y + this.CharSize.Height);
					break;

				case 'q': // horizontal line
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                           Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,     Y + this.CharSize.Height/2);
					break;

				case 'w': // top tee-piece
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                         Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,   Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height);
					break;

				case 'k': // top right bracket
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                         Y + this.CharSize.Height/2,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height);
					break;

				case 'x': // vertical line
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height);
					break;

				case 't': // left hand tee-piece
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,   Y + this.CharSize.Height/2);
					break;

				case 'n': // cross piece
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                         Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,   Y + this.CharSize.Height/2);
					break;

				case 'u': // right hand tee-piece
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                         Y + this.CharSize.Height/2,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height);
					break;

				case 'm': // bottom left bracket
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,   Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2);
					break;

				case 'v': // bottom tee-piece
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                         Y + this.CharSize.Height/2,   
						X + this.CharSize.Width,   Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2);
					break;

				case 'j': // bottom right bracket
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X,                         Y + this.CharSize.Height/2,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2);
					CurGraphics.DrawLine (new System.Drawing.Pen (CurFGColor, 1),
						X + this.CharSize.Width/2, Y,   
						X + this.CharSize.Width/2, Y + this.CharSize.Height/2);
					break;

				default:
					break;
			}

		}

		private void WipeScreen (System.Drawing.Graphics CurGraphics)
		{
			// clear the screen buffer area
			if ((this.Modes.Flags & uc_Mode.LightBackground) > 0)
			{
				CurGraphics.Clear (this.FGColor);
			}
			else
			{
				CurGraphics.Clear (this.BackColor);
			}
		}

		private void ClearDown (System.Int32 Param)
		{
			System.Int32 X = this.Caret.Pos.X;
			System.Int32 Y = this.Caret.Pos.Y;

			switch (Param)
			{
				case 0: // from cursor to bottom inclusive

					System.Array.Clear (this.CharGrid[Y],   X, this.CharGrid[Y].Length   - X);
					System.Array.Clear (this.AttribGrid[Y], X, this.AttribGrid[Y].Length - X);

					for (int i = Y + 1; i < this._rows; i++)
					{
						System.Array.Clear (this.CharGrid[i],   0, this.CharGrid[i].Length);
						System.Array.Clear (this.AttribGrid[i], 0, this.AttribGrid[i].Length);
					}
					break;

				case 1: // from top to cursor inclusive

					System.Array.Clear (this.CharGrid[Y],   0, X + 1);
					System.Array.Clear (this.AttribGrid[Y], 0, X + 1);

					for (int i = 0; i < Y; i++)
					{
						System.Array.Clear (this.CharGrid[i],   0, this.CharGrid[i].Length);
						System.Array.Clear (this.AttribGrid[i], 0, this.AttribGrid[i].Length);
					}
					break;

				case 2: // entire screen

					for (int i = 0; i < this._rows; i++)
					{
						System.Array.Clear (this.CharGrid[i],   0, this.CharGrid[i].Length);
						System.Array.Clear (this.AttribGrid[i], 0, this.AttribGrid[i].Length);
					}
					break;

				default:
					break;
			}
		}

		private void ClearRight (System.Int32 Param)
		{
			System.Int32 X = this.Caret.Pos.X;
			System.Int32 Y = this.Caret.Pos.Y;

			switch (Param)
			{
				case 0: // from cursor to end of line inclusive

					System.Array.Clear (this.CharGrid[Y],   X, this.CharGrid[Y].Length   - X);
					System.Array.Clear (this.AttribGrid[Y], X, this.AttribGrid[Y].Length - X);
					break;

				case 1: // from beginning to cursor inclusive
					System.Array.Clear (this.CharGrid[Y],   0, X + 1);
					System.Array.Clear (this.AttribGrid[Y], 0, X + 1);
					break;

				case 2: // entire line
					System.Array.Clear (this.CharGrid[Y],   0, this.CharGrid[Y].Length);
					System.Array.Clear (this.AttribGrid[Y], 0, this.AttribGrid[Y].Length);
					break;

				default:
					break;
			}
		}

		private void ShowBuffer ()
		{
			this.Invalidate ();
		}

		private void Redraw (System.Drawing.Graphics CurGraphics)
		{
			System.Drawing.Point CurPoint;

			System.Char CurChar;


			// refresh the screen
			for (System.Int32 Y = 0; Y < this._rows; Y++)
			{
				for (System.Int32 X = 0; X < this._cols; X++)
				{
					CurChar = this.CharGrid[Y][X];

					if (CurChar == '\0')
					{
						continue;
					}

					CurPoint = new System.Drawing.Point (
						X * this.CharSize.Width,
						Y * this.CharSize.Height);

					this.ShowChar (
						CurGraphics,
						CurChar, 
						CurPoint.Y,
						CurPoint.X,
						this.AttribGrid[Y][X]);		
				}
				
			} 
		}		

		private void NvtSendWill (System.Char CurChar)
		{
			DispatchMessage (this, "\xFF\xFB" + CurChar.ToString ());
		}

		private void NvtSendWont (System.Char CurChar)
		{
			DispatchMessage (this, "\xFF\xFC" + CurChar.ToString ());
		}

		private void NvtSendDont (System.Char CurChar)
		{
			DispatchMessage (this, "\xFF\xFE" + CurChar.ToString ());
		}

		private void NvtSendDo (System.Char CurChar)
		{
			DispatchMessage (this, "\xFF\xFD" + CurChar.ToString ());
		}

		private void NvtSendSubNeg (System.Char CurChar, System.String CurString)
		{
			DispatchMessage (this, "\xFF\xFA" + CurChar.ToString () + "\x00" + CurString + "\xFF\xF0");
		}

		private void NvtExecuteChar (System.Char CurChar)
		{
			switch (CurChar)
			{
				default:
					//System.Console.WriteLine ("Telnet command {0} encountered", CurChar);
					break;
			}
		}            	
		private void TelnetInterpreter (object Sender, NvtParserEventArgs e)
		{
			switch (e.Action)
			{
				case NvtActions.SendUp:
					this.Parser.ParseString (e.CurChar.ToString ());
					break;
				case NvtActions.Execute:
					this.NvtExecuteChar (e.CurChar);
					break;
				default:
					break;
			}

			// if the sequence is a DO message
			if (e.CurSequence.StartsWith ("\xFD"))
			{
				System.Char CurCmd = System.Convert.ToChar (e.CurSequence.Substring (1, 1));

				switch (CurCmd)
				{
						// 24 - terminal type 
					case '\x18':
						NvtSendWill (CurCmd);
						break;

					default:
						NvtSendWont (CurCmd);
						//System.Console.Write ("unsupported telnet DO sequence {0} happened\n", 
						//System.Convert.ToInt32 (System.Convert.ToChar (e.CurSequence.Substring (1,1))));
						break;
				}
			}

			// if the sequence is a WILL message
			if (e.CurSequence.StartsWith ("\xFB"))
			{
				System.Char CurCmd = System.Convert.ToChar (e.CurSequence.Substring (1, 1));

				switch (CurCmd)
				{
					case '\x01': // echo
						NvtSendDo (CurCmd);
						break;

					default:
						NvtSendDont (CurCmd);
						//System.Console.Write ("unsupported telnet WILL sequence {0} happened\n", 
						//System.Convert.ToInt32 (System.Convert.ToChar (e.CurSequence.Substring (1,1))));
						break;
				}
			}

			// if the sequence is a SUBNEGOTIATE message
			if (e.CurSequence.StartsWith ("\xFA"))
			{
				if (e.CurSequence[2] != '\x01')
				{
					// not interested in data from host just yet as we don't ask for it at the moment
					return;
				}

				System.Char CurCmd = System.Convert.ToChar (e.CurSequence.Substring (1, 1));

				switch (CurCmd)
				{
						// 24 - terminal type 
					case '\x18':
						NvtSendSubNeg (CurCmd, "vt220");
						break;

					default:
						NvtSendSubNeg (CurCmd, "");
						System.Console.Write ("unsupported telnet SUBNEG sequence {0} happened\n", 
							System.Convert.ToInt32 (System.Convert.ToChar (e.CurSequence.Substring (1,1))));
						break;
				}
			}
		}

		private void CarriageReturn () 
		{
			this.CaretToAbs (this.Caret.Pos.Y, 0);
		}

		private void Tab () 
		{
			for (System.Int32 i = 0; i < this.TabStops.Columns.Length; i++)
			{
				if (i > this.Caret.Pos.X && this.TabStops.Columns[i]  == true)
				{
					this.CaretToAbs (this.Caret.Pos.Y, i);
					return;
				}
			}

			this.CaretToAbs (this.Caret.Pos.Y, this._cols -1);
			return;
		}

		private void TabSet ()
		{
			this.TabStops.Columns[this.Caret.Pos.X] = true;
		}

		private void ClearTabs (uc_Params CurParams) // TBC 
		{
			System.Int32 Param = 0;

			if (CurParams.Count () > 0)
			{
				Param = System.Convert.ToInt32 (CurParams.Elements[0]);
			}

			switch (Param)
			{
				case 0: // Current Position
					this.TabStops.Columns[this.Caret.Pos.X] = false;
					break;

				case 3: // All Tabs
					for (int i = 0; i < this.TabStops.Columns.Length; i++)
					{
						this.TabStops.Columns[i] = false;
					}
					break;

				default:
					break;
			}
		}

		private void ReverseLineFeed ()
		{

			// if we're at the top of the scroll region (top margin)
			if (this.Caret.Pos.Y == this.TopMargin)
			{
				// we need to add a new line at the top of the screen margin
				// so shift all the rows in the scroll region down in the array and
				// insert a new row at the top
                
				int i;

   
				for (i = this.BottomMargin; i > this.TopMargin; i--)
				{
					this.CharGrid[i]   = this.CharGrid[i - 1];
					this.AttribGrid[i] = this.AttribGrid[i - 1];
				}

				this.CharGrid[this.TopMargin] = new System.Char[this._cols];

				this.AttribGrid[this.TopMargin] = new CharAttribStruct[this._cols];
			}

			this.CaretUp ();
		}

		private void InsertLine (uc_Params CurParams)
		{

			// if we're not in the scroll region then bail
			if (this.Caret.Pos.Y < this.TopMargin ||
				this.Caret.Pos.Y > this.BottomMargin)
			{
				return;
			}

			System.Int32 NbrOff = 1;

			if (CurParams.Count () > 0)
			{
				NbrOff = System.Convert.ToInt32 (CurParams.Elements[0]);
			}

			while (NbrOff > 0)
			{ 

				// Shift all the rows from the current row to the bottom margin down one place
				for (int i = this.BottomMargin; i > this.Caret.Pos.Y; i--)
				{
					this.CharGrid[i]   = this.CharGrid[i - 1];
					this.AttribGrid[i] = this.AttribGrid[i - 1];
				}
                 

				this.CharGrid[this.Caret.Pos.Y]   = new System.Char[this._cols];
				this.AttribGrid[this.Caret.Pos.Y] = new CharAttribStruct[this._cols];

				NbrOff--;
			}

		}

		private void DeleteLine (uc_Params CurParams)
		{
			// if we're not in the scroll region then bail
			if (this.Caret.Pos.Y < this.TopMargin ||
				this.Caret.Pos.Y > this.BottomMargin)
			{
				return;
			}

			System.Int32 NbrOff = 1;

			if (CurParams.Count () > 0)
			{
				NbrOff = System.Convert.ToInt32 (CurParams.Elements[0]);
			}

			while (NbrOff > 0)
			{ 

				// Shift all the rows from below the current row to the bottom margin up one place
				for (int i = this.Caret.Pos.Y; i < this.BottomMargin; i++)
				{
					this.CharGrid[i]   = this.CharGrid[i + 1];
					this.AttribGrid[i] = this.AttribGrid[i + 1];
				}

				this.CharGrid[this.BottomMargin]   = new System.Char[this._cols];
				this.AttribGrid[this.BottomMargin] = new CharAttribStruct[this._cols];

				NbrOff--;
			}
		}

		private void LineFeed ()
		{
			this.SetScrollBarValues();

			// capture the new line into the scrollback buffer
			if (this.ScrollbackBuffer.Count < this.ScrollbackBufferSize)
			{
				
			}
			else 
			{
				this.ScrollbackBuffer.RemoveAt(0);
			}


			string s = "";
			for (int x = 0; x < this._cols; x++)
			{
				char CurChar = this.CharGrid[this.Caret.Pos.Y][x];

				if (CurChar == '\0')
				{
					continue;
				}
				s = s + Convert.ToString(CurChar);
			}
			this.ScrollbackBuffer.Add(s);
			//System.Console.WriteLine("there are " + Convert.ToString(this.ScrollbackBuffer.Count) + " lines in the scrollback buffer");
			//System.Console.WriteLine(s);



			if (this.Caret.Pos.Y == this.BottomMargin || this.Caret.Pos.Y == this._rows - 1)
			{
				// we need to add a new line so shift all the rows up in the array and
				// insert a new row at the bottom
                
				int i;

				for (i = this.TopMargin; i < this.BottomMargin; i++)
				{
					this.CharGrid[i]   = this.CharGrid[i + 1];
					this.AttribGrid[i] = this.AttribGrid[i + 1];
				}

				this.CharGrid[i]   = new System.Char[this._cols];
				this.AttribGrid[i] = new CharAttribStruct[this._cols];

			}

			this.CaretDown ();




		}

		private void Index (System.Int32 Param)
		{
			if (Param == 0) Param = 1;

			for (int i = 0; i < Param; i++)
			{
				this.LineFeed ();
			}
		}

		private void ReverseIndex (System.Int32 Param)
		{
			if (Param == 0) Param = 1;

			for (int i = 0; i < Param; i++)
			{
				this.ReverseLineFeed ();
			}
		}

		private void CaretOff ()
		{
			if (this.Caret.IsOff == true)
			{
				return;
			}

			this.Caret.IsOff = true;
		}

		private void CaretOn ()
		{
			if (this.Caret.IsOff == false)
			{
				return;
			}

			this.Caret.IsOff = false;

		}

		private void ShowCaret (System.Drawing.Graphics CurGraphics)
		{
			System.Int32 X = this.Caret.Pos.X;
			System.Int32 Y = this.Caret.Pos.Y;

			if (this.Caret.IsOff == true)
			{
				return;
			}

			// paint a rectangle over the cursor position
			CurGraphics.DrawImageUnscaled (
				this.Caret.Bitmap, 
				X * (int) this.CharSize.Width, 
				Y * (int) this.CharSize.Height);

			// if we don't have a char to redraw then leave
			if (this.CharGrid[Y][X] == '\0')
			{
				return;
			}

			CharAttribStruct CurAttribs = new CharAttribStruct ();

			CurAttribs.UseAltColor = true;

			CurAttribs.GL = this.AttribGrid[Y][X].GL;
			CurAttribs.GR = this.AttribGrid[Y][X].GR;
			CurAttribs.GS = this.AttribGrid[Y][X].GS;

			if (this.AttribGrid[Y][X].UseAltBGColor == false)
			{
				CurAttribs.AltColor = this.BackColor;
			}
			else if (this.AttribGrid[Y][X].UseAltBGColor == true)
			{
				CurAttribs.AltColor = this.AttribGrid[Y][X].AltBGColor;
			}

			CurAttribs.IsUnderscored = this.AttribGrid[Y][X].IsUnderscored;
			CurAttribs.IsDECSG       = this.AttribGrid[Y][X].IsDECSG;
 
			// redispay the current char in the background colour
			this.ShowChar (
				CurGraphics,
				this.CharGrid[Y][X], 
				Caret.Pos.Y * this.CharSize.Height,
				Caret.Pos.X * this.CharSize.Width,
				CurAttribs);
		}

		private void CaretUp ()
		{
			this.Caret.EOL = false;

			if ((this.Caret.Pos.Y > 0              && (this.Modes.Flags & uc_Mode.OriginRelative) == 0) ||
				(this.Caret.Pos.Y > this.TopMargin && (this.Modes.Flags & uc_Mode.OriginRelative) >  0))
			{
				this.Caret.Pos.Y -= 1;
			}
		}

		private void CaretDown ()
		{
			this.Caret.EOL = false;

			if ((this.Caret.Pos.Y < this._rows - 1     && (this.Modes.Flags & uc_Mode.OriginRelative) == 0) ||
				(this.Caret.Pos.Y < this.BottomMargin && (this.Modes.Flags & uc_Mode.OriginRelative)  > 0))
			{
				this.Caret.Pos.Y += 1;
			}
		}

		private void CaretLeft ()
		{
			this.Caret.EOL = false;

			if (this.Caret.Pos.X > 0)
			{
				this.Caret.Pos.X -= 1;
			}
		}

		private void CaretRight ()
		{
			if (this.Caret.Pos.X < this._cols - 1)
			{
				this.Caret.Pos.X += 1;
				this.Caret.EOL = false;
			}
			else
			{
				this.Caret.EOL = true;
			}
		}

		private void CaretToRel (System.Int32 Y, System.Int32 X) 
		{
 
			this.Caret.EOL = false;
			/* This code is used when we get a cursor position command from
				   the host. Even if we're not in relative mode we use this as this will
				   sort that out for us. The ToAbs code is used internally by this prog 
				   but is smart enough to stay within the margins if the originrelative 
				   flagis set. */

			if ((this.Modes.Flags & uc_Mode.OriginRelative) == 0)
			{
				this.CaretToAbs (Y, X);
				return;
			}

			/* the origin mode is relative so add the top and left margin
				   to Y and X respectively */
			Y += this.TopMargin;

			if (X < 0)
			{
				X = 0;
			}

			if (X > this._cols - 1)
			{
				X = this._cols - 1;
			}
 
			if (Y < this.TopMargin)
			{
				Y = this.TopMargin;
			}

			if (Y > this.BottomMargin)
			{
				Y = this.BottomMargin;
			}

			this.Caret.Pos.Y = Y;
			this.Caret.Pos.X = X;
		}

		private void CaretToAbs (System.Int32 Y, System.Int32 X)
		{ 
			this.Caret.EOL = false;

			if (X < 0)
			{
				X = 0;
			}

			if (X > this._cols - 1)
			{
				X = this._cols - 1;
			}

			if (Y < 0 && (this.Modes.Flags & uc_Mode.OriginRelative) == 0)
			{
				Y = 0;
			}

			if (Y < this.TopMargin && (this.Modes.Flags & uc_Mode.OriginRelative) > 0)
			{
				Y = this.TopMargin;
			}

			if (Y > this._rows - 1 && (this.Modes.Flags & uc_Mode.OriginRelative) == 0)
			{
				Y = this._rows - 1;
			}

			if (Y > this.BottomMargin && (this.Modes.Flags & uc_Mode.OriginRelative) > 0)
			{
				Y = this.BottomMargin;
			}

			this.Caret.Pos.Y = Y;
			this.Caret.Pos.X = X;
		}

		private void CommandRouter (object Sender, ParserEventArgs e)
		{


			switch (e.Action)
			{
				case Actions.Print:
					this.PrintChar (e.CurChar);
					//System.Console.Write ("{0}", e.CurChar);
					break;

				case Actions.Execute:
					this.ExecuteChar (e.CurChar);
					break;

				case Actions.Dispatch:
					break;

				default:
					break;
			}

			System.Int32 Param = 0;

			System.Int32 Inc = 1; // increment

			switch (e.CurSequence)
			{
				case "":
					break;

				case "\x1b" + "7": //DECSC Save Cursor position and attributes
					this.SavedCarets.Add (new uc_CaretAttribs (
						this.Caret.Pos,
						this.G0.Set, 
						this.G1.Set, 
						this.G2.Set, 
						this.G3.Set, 
						this.CharAttribs));
                                              
					break;

				case "\x1b" + "8": //DECRC Restore Cursor position and attributes
					this.Caret.Pos   = ((uc_CaretAttribs) this.SavedCarets[this.SavedCarets.Count - 1]).Pos;
					this.CharAttribs = ((uc_CaretAttribs) this.SavedCarets[this.SavedCarets.Count - 1]).Attribs;

					this.G0.Set = ((uc_CaretAttribs) this.SavedCarets[this.SavedCarets.Count - 1]).G0Set;
					this.G1.Set = ((uc_CaretAttribs) this.SavedCarets[this.SavedCarets.Count - 1]).G1Set;
					this.G2.Set = ((uc_CaretAttribs) this.SavedCarets[this.SavedCarets.Count - 1]).G2Set;
					this.G3.Set = ((uc_CaretAttribs) this.SavedCarets[this.SavedCarets.Count - 1]).G3Set;

					this.SavedCarets.RemoveAt (this.SavedCarets.Count - 1);

					break;

				case "\x1b~": //LS1R Locking Shift G1 -> GR
					this.CharAttribs.GR = G1;
					break;

				case "\x1bn": //LS2 Locking Shift G2 -> GL
					this.CharAttribs.GL = G2;
					break;

				case "\x1b}": //LS2R Locking Shift G2 -> GR
					this.CharAttribs.GR = G2;
					break;

				case "\x1bo": //LS3 Locking Shift G3 -> GL
					this.CharAttribs.GL = G3;
					break;

				case "\x1b|": //LS3R Locking Shift G3 -> GR
					this.CharAttribs.GR = G3;
					break;

				case "\x1b#8": //DECALN
					e.CurParams.Elements.Add ("1");
					e.CurParams.Elements.Add (this._rows.ToString ());
					this.SetScrollRegion (e.CurParams);

					// put E's on the entire screen
					for (int y = 0; y < this._rows; y++)
					{
						this.CaretToAbs (y, 0);

						for (int x = 0; x < this._cols; x++)
						{
							this.PrintChar ('E');
						}
					}
					break; 

				case "\x1b=": // Keypad to Application mode
					this.Modes.Flags = this.Modes.Flags | uc_Mode.KeypadAppln;
					break;

				case "\x1b>": // Keypad to Numeric mode
					this.Modes.Flags = this.Modes.Flags ^ uc_Mode.KeypadAppln;
					break;

				case "\x1b[B": // CUD

					if (e.CurParams.Count () > 0) 
					{
						Inc = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					if (Inc == 0) Inc = 1;

					this.CaretToAbs (this.Caret.Pos.Y + Inc, this.Caret.Pos.X);
					break;

				case "\x1b[A": // CUU

					if (e.CurParams.Count () > 0) 
					{
						Inc = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					if (Inc == 0) Inc = 1;

					this.CaretToAbs (this.Caret.Pos.Y - Inc, this.Caret.Pos.X);
					break;

				case "\x1b[C": // CUF

					if (e.CurParams.Count () > 0) 
					{
						Inc = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					if (Inc == 0) Inc = 1;

					this.CaretToAbs (this.Caret.Pos.Y, this.Caret.Pos.X + Inc);
					break;

				case "\x1b[D": // CUB

					if (e.CurParams.Count () > 0) 
					{
						Inc = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					if (Inc == 0) Inc = 1;

					this.CaretToAbs (this.Caret.Pos.Y, this.Caret.Pos.X - Inc);
					break;

				case "\x1b[H": // CUP
				case "\x1b[f": // HVP

					System.Int32 X = 0;
					System.Int32 Y = 0;

					if (e.CurParams.Count () > 0) 
					{
						Y = System.Convert.ToInt32 (e.CurParams.Elements[0]) - 1; 
					}

					if (e.CurParams.Count () > 1)
					{
						X = System.Convert.ToInt32 (e.CurParams.Elements[1]) - 1;
					}

					this.CaretToRel (Y, X);
					break;

				case "\x1b[J":

					if (e.CurParams.Count () > 0) 
					{
						Param = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					this.ClearDown (Param);
					break;

				case "\x1b[K":

					if (e.CurParams.Count () > 0) 
					{
						Param = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					this.ClearRight (Param);
					break;

				case "\x1b[L": // INSERT LINE
					this.InsertLine (e.CurParams);
					break;

				case "\x1b[M": // DELETE LINE
					this.DeleteLine (e.CurParams);
					break;

				case "\x1bN": // SS2 Single Shift (G2 -> GL)
					this.CharAttribs.GS = this.G2;
					break;

				case "\x1bO": // SS3 Single Shift (G3 -> GL)
					this.CharAttribs.GS = this.G3;
					//System.Console.WriteLine ("SS3: GS = {0}", this.CharAttribs.GS);
					break;

				case "\x1b[m":
					this.SetCharAttribs (e.CurParams);
					break;

				case "\x1b[?h":
					this.SetqmhMode (e.CurParams);
					break;

				case "\x1b[?l":
					this.SetqmlMode (e.CurParams);
					break;

				case "\x1b[c": // DA Device Attributes
					//                    this.DispatchMessage (this, "\x1b[?64;1;2;6;7;8;9c");
					this.DispatchMessage (this, "\x1b[?6c");
					break;

				case "\x1b[g":
					this.ClearTabs (e.CurParams);
					break;

				case "\x1b[h":
					this.SethMode (e.CurParams);
					break;

				case "\x1b[l":
					this.SetlMode (e.CurParams);
					break;

				case "\x1b[r": // DECSTBM Set Top and Bottom Margins
					this.SetScrollRegion (e.CurParams);
					break;

				case "\x1b[t": // DECSLPP Set Lines Per Page

					if (e.CurParams.Count () > 0) 
					{
						Param = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					if (Param > 0) this.SetSize (Param, this._cols);

					break;

				case "\x1b" + "D": // IND

					if (e.CurParams.Count () > 0) 
					{
						Param = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					this.Index (Param);
					break;

				case "\x1b" + "E": // NEL
					this.LineFeed ();
					this.CarriageReturn ();
					break;

				case "\x1bH": // HTS
					this.TabSet ();
					break;

				case "\x1bM": // RI
					if (e.CurParams.Count () > 0) 
					{
						Param = System.Convert.ToInt32 (e.CurParams.Elements[0]); 
					}

					this.ReverseIndex (Param);
					break;

				default:
					//System.Console.Write ("unsupported VT sequence {0} happened\n", e.CurSequence);
					break;
                
			}

			if (e.CurSequence.StartsWith ("\x1b("))
			{
				this.SelectCharSet (ref this.G0.Set, e.CurSequence.Substring (2));
			}
			else if (e.CurSequence.StartsWith ("\x1b-") ||
				e.CurSequence.StartsWith ("\x1b)"))
			{
				this.SelectCharSet (ref this.G1.Set, e.CurSequence.Substring (2));
			}
			else if (e.CurSequence.StartsWith ("\x1b.") ||
				e.CurSequence.StartsWith ("\x1b*"))
			{
				this.SelectCharSet (ref this.G2.Set, e.CurSequence.Substring (2));
			}
			else if (e.CurSequence.StartsWith ("\x1b/") ||
				e.CurSequence.StartsWith ("\x1b+"))
			{
				this.SelectCharSet (ref this.G3.Set, e.CurSequence.Substring (2));
			}
		}

		private void SelectCharSet (ref uc_Chars.Sets CurTarget, System.String CurString)
		{
			switch (CurString)
			{
				case "B":
					CurTarget = uc_Chars.Sets.ASCII;
					break;

				case "%5":
					CurTarget = uc_Chars.Sets.DECS;
					break;

				case "0":
					CurTarget = uc_Chars.Sets.DECSG;
					break;

				case ">":
					CurTarget = uc_Chars.Sets.DECTECH;
					break;

				case "<":
					CurTarget = uc_Chars.Sets.DECSG;
					break;

				case "A":
					if ((this.Modes.Flags & uc_Mode.National) == 0)
					{
						CurTarget = uc_Chars.Sets.ISOLatin1S;
					}
					else
					{
						CurTarget = uc_Chars.Sets.NRCUK;
					}
					break;

				case "4":
					//                    CurTarget = uc_Chars.Sets.NRCDutch;
					break;

				case "5":
					CurTarget = uc_Chars.Sets.NRCFinnish;
					break;

				case "R":
					CurTarget = uc_Chars.Sets.NRCFrench;
					break;

				case "9":
					CurTarget = uc_Chars.Sets.NRCFrenchCanadian;
					break;

				case "K":
					CurTarget = uc_Chars.Sets.NRCGerman;
					break;

				case "Y":
					CurTarget = uc_Chars.Sets.NRCItalian;
					break;

				case "6":
					CurTarget = uc_Chars.Sets.NRCNorDanish;
					break;

				case "'":
					CurTarget = uc_Chars.Sets.NRCNorDanish;
					break;

				case "%6":
					CurTarget = uc_Chars.Sets.NRCPortuguese;
					break;

				case "Z":
					CurTarget = uc_Chars.Sets.NRCSpanish;
					break;

				case "7":
					CurTarget = uc_Chars.Sets.NRCSwedish;
					break;

				case "=":
					CurTarget = uc_Chars.Sets.NRCSwiss;
					break;

				default:
					break;
			}
		}

		private void SetqmhMode (uc_Params CurParams) // set mode for ESC?h command
		{
			System.Int32 OptInt = 0;

			foreach (System.String CurOption in CurParams.Elements)
			{
				try 
				{
					OptInt = System.Convert.ToInt32 (CurOption);
				}
				catch (System.Exception CurException)
				{
					//System.Console.WriteLine (CurException.Message);
					MessageBox.Show(CurException.Message);
				}

				switch (OptInt) 
				{
					case 1: // set cursor keys to application mode
						this.Modes.Flags = this.Modes.Flags | uc_Mode.CursorAppln;
						break;

					case 2: // lock the keyboard
						this.Modes.Flags = this.Modes.Flags | uc_Mode.Locked;
						break;

					case 3: // set terminal to 132 column mode
						this.SetSize (this._rows, 132);
						break;

					case 5: // Light Background Mode
						this.Modes.Flags = this.Modes.Flags | uc_Mode.LightBackground;
						this.RefreshEvent ();
						break;

					case 6: // Origin Mode Relative
						this.Modes.Flags = this.Modes.Flags | uc_Mode.OriginRelative;
						this.CaretToRel (0, 0);
						break;

					case 7: // Autowrap On
						this.Modes.Flags = this.Modes.Flags | uc_Mode.AutoWrap;
						break;

					case 8: // AutoRepeat On
						this.Modes.Flags = this.Modes.Flags | uc_Mode.Repeat;
						break;

					case 42: // DECNRCM Multinational Charset
						this.Modes.Flags = this.Modes.Flags | uc_Mode.National;
						break;

					case 66: // Numeric Keypad Application Mode On
						this.Modes.Flags = this.Modes.Flags | uc_Mode.KeypadAppln;
						break;

					default:
						break;
				} 
			}
		}

		private void SetqmlMode (uc_Params CurParams) // set mode for ESC?l command
		{
			System.Int32 OptInt = 0;

			foreach (System.String CurOption in CurParams.Elements)
			{
				try 
				{
					OptInt = System.Convert.ToInt32 (CurOption);
				}
				catch (System.Exception CurException)
				{
                    //System.Console.WriteLine (CurException.Message);
					MessageBox.Show(CurException.Message);
				}

				switch (OptInt) 
				{
					case 1: // set cursor keys to normal cursor mode
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.CursorAppln;
						break;

					case 2: // unlock the keyboard
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.Locked;
						break;

					case 3: // set terminal to 80 column mode
						this.SetSize (this._rows, 80);
						break;

					case 5: // Dark Background Mode
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.LightBackground;
						this.RefreshEvent ();
						break;

					case 6: // Origin Mode Absolute
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.OriginRelative;
						this.CaretToAbs (0, 0);
						break;

					case 7: // Autowrap Off
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.AutoWrap;
						break;

					case 8: // AutoRepeat Off
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.Repeat;
						break;

					case 42: // DECNRCM National Charset
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.National;
						break;

					case 66: // Numeric Keypad Application Mode On
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.KeypadAppln;
						break;

					default:
						break;
				} 
			}
		}

		private void SethMode (uc_Params CurParams) // set mode for ESC?h command
		{
			System.Int32 OptInt = 0;

			foreach (System.String CurOption in CurParams.Elements)
			{
				try 
				{
					OptInt = System.Convert.ToInt32 (CurOption);
				}
				catch (System.Exception CurException)
				{
                    //System.Console.WriteLine (CurException.Message);
					MessageBox.Show(CurException.Message);
				}

				switch (OptInt) 
				{
					case 1: // set local echo off
						this.Modes.Flags = this.Modes.Flags | uc_Mode.LocalEchoOff;
						break;

					default:
						break;
				} 
			}
		}

		private void SetlMode (uc_Params CurParams) // set mode for ESC?l command
		{
			System.Int32 OptInt = 0;

			foreach (System.String CurOption in CurParams.Elements)
			{
				try 
				{
					OptInt = System.Convert.ToInt32 (CurOption);
				}
				catch (System.Exception CurException)
				{
                    //System.Console.WriteLine (CurException.Message);
					MessageBox.Show(CurException.Message);
				}

				switch (OptInt) 
				{
					case 1: // set LocalEcho on
						this.Modes.Flags = this.Modes.Flags & ~uc_Mode.LocalEchoOff;
						break;

					default:
						break;
				} 
			}
		}

		private void SetScrollRegion (uc_Params CurParams)
		{
			if (CurParams.Count () > 0) 
			{
				this.TopMargin = System.Convert.ToInt32 (CurParams.Elements[0]) - 1; 
			}

			if (CurParams.Count () > 1)
			{
				this.BottomMargin = System.Convert.ToInt32 (CurParams.Elements[1]) - 1;
			}

			if (this.BottomMargin == 0)
			{
				this.BottomMargin = this._rows - 1;
			}

			if (this.TopMargin < 0)
			{
				this.BottomMargin = 0;
			}
		}

		private void ClearCharAttribs ()
		{
			this.CharAttribs.IsBold          = false;
			this.CharAttribs.IsDim           = false;
			this.CharAttribs.IsUnderscored   = false;
			this.CharAttribs.IsBlinking      = false;
			this.CharAttribs.IsInverse       = false;
			this.CharAttribs.IsPrimaryFont   = false;
			this.CharAttribs.IsAlternateFont = false;
			this.CharAttribs.UseAltBGColor   = false;
			this.CharAttribs.UseAltColor     = false;
			this.CharAttribs.AltColor        = System.Drawing.Color.White;
			this.CharAttribs.AltBGColor      = System.Drawing.Color.Black;
		}

		private void SetCharAttribs (uc_Params CurParams)
		{
			if (CurParams.Count () < 1)
			{
				this.ClearCharAttribs ();
				return;
			}

			for (int i = 0; i < CurParams.Count (); i++)
			{  
				switch (System.Convert.ToInt32 (CurParams.Elements[i]))
				{
					case 0:
						this.ClearCharAttribs ();
						break;

					case 1:
						this.CharAttribs.IsBold = true;
						break;

					case 4:
						this.CharAttribs.IsUnderscored = true;
						break;

					case 5:
						this.CharAttribs.IsBlinking = true;
						break;

					case 7:
						this.CharAttribs.IsInverse = true;
						break;

					case 22:
						this.CharAttribs.IsBold = false;
						break;

					case 24:
						this.CharAttribs.IsUnderscored = false;
						break;

					case 25:
						this.CharAttribs.IsBlinking = false;
						break;

					case 27:
						this.CharAttribs.IsInverse = false;
						break;

					case 30:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Black;
						break;

					case 31:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Red;
						break;

					case 32:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Green;
						break;

					case 33:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Yellow;
						break;

					case 34:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Blue;
						break;

					case 35:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Magenta;
						break;

					case 36:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.Cyan;
						break;

					case 37:
						this.CharAttribs.UseAltColor = true;
						this.CharAttribs.AltColor = System.Drawing.Color.White;
						break;

					case 40:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Black;
						break;

					case 41:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Red;
						break;

					case 42:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Green;
						break;

					case 43:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Yellow;
						break;

					case 44:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Blue;
						break;

					case 45:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Magenta;
						break;

					case 46:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.Cyan;
						break;

					case 47:
						this.CharAttribs.UseAltBGColor = true;
						this.CharAttribs.AltBGColor = System.Drawing.Color.White;
						break;

					default:
						break;
				}
			}                            
		}

		private void ExecuteChar (System.Char CurChar)
		{
			switch (CurChar)
			{
				case '\x05': // ENQ request for the answerback message
					this.DispatchMessage (this, "vt220");
					break;

				case '\x07': // BEL ring my bell
					// this.BELL;
					break;

				case '\x08': // BS back space
					this.CaretLeft ();
					break;

				case '\x09': // HT Horizontal Tab
					this.Tab ();
					break;

				case '\x0A': // LF  LineFeed
				case '\x0B': // VT  VerticalTab
				case '\x0C': // FF  FormFeed
				case '\x84': // IND Index
					this.LineFeed ();
					break;

				case '\x0D': // CR CarriageReturn
					this.CarriageReturn ();
					break;

				case '\x0E': // SO maps G1 into GL
					this.CharAttribs.GL = this.G1;
					break;

				case '\x0F': // SI maps G0 into GL
					this.CharAttribs.GL = this.G0;
					break;

				case '\x11': // DC1/XON continue sending characters
					this.XOFF = false;
					this.DispatchMessage (this, "");
					break;                    

				case '\x13': // DC3/XOFF stop sending characters
					this.XOFF = true;
					break;                    

				case '\x85': // NEL Next line (same as line feed and carriage return)
					this.LineFeed ();
					this.CaretToAbs (this.Caret.Pos.Y, 0);
					break;

				case '\x88': // HTS Horizontal tab set 
					this.TabSet ();
					break;

				case '\x8D': // RI Reverse Index 
					this.ReverseLineFeed ();
					break;

				case '\x8E': // SS2 Single Shift (G2 -> GL)
					this.CharAttribs.GS = this.G2;
					break;

				case '\x8F': // SS3 Single Shift (G3 -> GL)
					this.CharAttribs.GS = this.G3;
					break;

				default:
					break;
			}
		}            		
		
		private void SetSize (System.Int32 Rows, System.Int32 Columns)
		{
			this._rows    = Rows;
			this._cols = Columns;

			this.TopMargin    = 0;
			this.BottomMargin = Rows - 1;

			//this.ClientSize = new System.Drawing.Size (
			//	System.Convert.ToInt32 (this.CharSize.Width  * this.Columns + 2) + this.VertScrollBar.Width,
			//	System.Convert.ToInt32 (this.CharSize.Height * this.Rows    + 2));

			// create the character grid (rows by columns) this is a shadow of what's displayed
			this.CharGrid = new System.Char[Rows][];

			this.Caret.Pos.X = 0;
			this.Caret.Pos.Y = 0;

			for (int i = 0; i < this.CharGrid.Length; i++)
			{
				this.CharGrid[i]   = new System.Char[Columns];
			}

			this.AttribGrid = new CharAttribStruct[Rows][];

			for (int i = 0; i < this.AttribGrid.Length; i++)
			{
				this.AttribGrid[i]   = new CharAttribStruct[Columns];
			}
		}

		private void GetFontInfo ()
		{
			System.Drawing.Graphics tmpGraphics = this.CreateGraphics ();

			// get the offset that the moron Graphics.Drawstring method adds by default
			this.DrawStringOffset = this.GetDrawStringOffset (tmpGraphics, 0, 0, 'A');

			// get the size of the character using the same type of method
			System.Drawing.Point tmpPoint = this.GetCharSize (tmpGraphics);

			this.CharSize.Width  = tmpPoint.X; // make a little breathing room
			this.CharSize.Height = tmpPoint.Y;
			
			//Graphics g = this.CreateGraphics();
			//SizeF size = g.MeasureString("_", this.Font);
			//this.CharSize.Width = (int) size.Width;
			//this.CharSize.Height = (int) size.Height;

			tmpGraphics.Dispose ();

			this.UnderlinePos = this.CharSize.Height - 2;

			this.Caret.Bitmap    =  new System.Drawing.Bitmap (this.CharSize.Width, this.CharSize.Height);
			this.Caret.Buffer    =  System.Drawing.Graphics.FromImage (this.Caret.Bitmap);
			this.Caret.Buffer.Clear (System.Drawing.Color.FromArgb (255, 181, 106));
			this.EraseBitmap     =  new System.Drawing.Bitmap (this.CharSize.Width, this.CharSize.Height);
			this.EraseBuffer     =  System.Drawing.Graphics.FromImage (this.EraseBitmap);

			
		}

		private void OnClickFont (System.Object Sender, System.EventArgs e)
		{
			System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog ();

			fontDialog.FixedPitchOnly = true;
			fontDialog.ShowEffects    = false;
			fontDialog.Font           = this.Font;

			if (fontDialog.ShowDialog () != System.Windows.Forms.DialogResult.Cancel)
			{
				// Change the font
				this.Font = fontDialog.Font;

				this.GetFontInfo ();

				this.ClientSize = new System.Drawing.Size (
					System.Convert.ToInt32 (this.CharSize.Width  * this._cols + 2) + this.VertScrollBar.Width,
					System.Convert.ToInt32 (this.CharSize.Height * this._rows    + 2));
			};
		}

		#endregion

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                Message m = new Message();
                m.Msg = WMCodes.WM_CHAR;
                m.WParam = new IntPtr((int)keyData);

                Keyboard.KeyDown(m);
                return true;
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
        }


		#region Private Classes
		private class uc_CommsStateObject
		{
			public System.Net.Sockets.Socket Socket;
			public System.Byte[]             Buffer;

			public uc_CommsStateObject ()
			{
				this.Buffer = new System.Byte[8192];
			}
		}
		
		private class uc_TabStops
		{
			public System.Boolean[] Columns;

			public uc_TabStops ()
			{
				Columns = new System.Boolean[256];

				Columns[8]   = true;
				Columns[16]  = true;
				Columns[24]  = true;
				Columns[32]  = true;
				Columns[40]  = true;
				Columns[48]  = true;
				Columns[56]  = true;
				Columns[64]  = true;
				Columns[72]  = true;
				Columns[80]  = true;
				Columns[88]  = true;
				Columns[96]  = true;
				Columns[104] = true;
				Columns[112] = true;
				Columns[120] = true;
				Columns[128] = true;
			}
		}
		private class uc_CaretAttribs
		{
			public System.Drawing.Point Pos;
			public uc_Chars.Sets        G0Set;
			public uc_Chars.Sets        G1Set;
			public uc_Chars.Sets        G2Set;
			public uc_Chars.Sets        G3Set;
			public CharAttribStruct     Attribs;

			public uc_CaretAttribs (
				System.Drawing.Point p1,
				uc_Chars.Sets        p2,
				uc_Chars.Sets        p3,
				uc_Chars.Sets        p4,
				uc_Chars.Sets        p5,
				CharAttribStruct          p6)
			{
				this.Pos     = p1;
				this.G0Set   = p2;
				this.G1Set   = p3;
				this.G2Set   = p4;
				this.G3Set   = p5;
				this.Attribs = p6;
			}
		}

		private class uc_Chars
		{
			public struct uc_CharSet 
			{
				public uc_CharSet (System.Int32 p1, System.Int16 p2)
				{
					Location  = p1;
					UnicodeNo = p2;
				}

				public System.Int32 Location;
				public System.Int16 UnicodeNo;
			}

			public uc_Chars.Sets Set;

			public uc_Chars (uc_Chars.Sets p1)
			{
				this.Set = p1;
			}

			public static System.Char Get (System.Char CurChar, uc_Chars.Sets GL, uc_Chars.Sets GR)
			{
				uc_CharSet[] CurSet;

				// I'm assuming the left hand in use table will always have a 00-7F char set in it
				if (System.Convert.ToInt32 (CurChar) < 128)
				{
					switch (GL)
					{
						case uc_Chars.Sets.ASCII:
							CurSet = uc_Chars.ASCII;
							break;

						case uc_Chars.Sets.DECSG:
							CurSet = uc_Chars.DECSG;
							break;

						case uc_Chars.Sets.NRCUK:
							CurSet = uc_Chars.NRCUK;
							break;

						case uc_Chars.Sets.NRCFinnish:
							CurSet = uc_Chars.NRCFinnish;
							break;

						case uc_Chars.Sets.NRCFrench:
							CurSet = uc_Chars.NRCFrench;
							break;

						case uc_Chars.Sets.NRCFrenchCanadian:
							CurSet = uc_Chars.NRCFrenchCanadian;
							break;

						case uc_Chars.Sets.NRCGerman:
							CurSet = uc_Chars.NRCGerman;
							break;

						case uc_Chars.Sets.NRCItalian:
							CurSet = uc_Chars.NRCItalian;
							break;

						case uc_Chars.Sets.NRCNorDanish:
							CurSet = uc_Chars.NRCNorDanish;
							break;

						case uc_Chars.Sets.NRCPortuguese:
							CurSet = uc_Chars.NRCPortuguese;
							break;

						case uc_Chars.Sets.NRCSpanish:
							CurSet = uc_Chars.NRCSpanish;
							break;

						case uc_Chars.Sets.NRCSwedish:
							CurSet = uc_Chars.NRCSwedish;
							break;

						case uc_Chars.Sets.NRCSwiss:
							CurSet = uc_Chars.NRCSwiss;
							break;

						default:
							CurSet = uc_Chars.ASCII;
							break;
					}
				}
					// I'm assuming the right hand in use table will always have a 80-FF char set in it
				else
				{
					switch (GR)
					{
						case uc_Chars.Sets.ISOLatin1S:
							CurSet = uc_Chars.ISOLatin1S;
							break;

						case uc_Chars.Sets.DECS:
							CurSet = uc_Chars.DECS;
							break;

						default:
							CurSet = uc_Chars.DECS;
							break;
					}
				}

				for (System.Int32 i = 0; i < CurSet.Length; i++)
				{
					if (CurSet[i].Location == System.Convert.ToInt32 (CurChar))
					{
						System.Byte[] CurBytes = System.BitConverter.GetBytes (CurSet[i].UnicodeNo);
						System.Char[] NewChars = System.Text.Encoding.Unicode.GetChars (CurBytes);

						return NewChars[0];
					}

				}

				return CurChar;
			}

			public static uc_CharSet[] DECSG =
		{
			new uc_CharSet (0x5F, 0x0020), // Blank (I've used space here so you may want to change this)
			//            new uc_CharSet (0x60, 0x25C6), // Filled Diamond 
			new uc_CharSet (0x61, 0x0000), // Pi over upsidedown Pi ?  
			new uc_CharSet (0x62, 0x2409), // HT symbol 
			new uc_CharSet (0x63, 0x240C), // LF Symbol  
			new uc_CharSet (0x64, 0x240D), // CR Symbol  
			new uc_CharSet (0x65, 0x240A), // LF Symbol  
			new uc_CharSet (0x66, 0x00B0), // Degree  
			new uc_CharSet (0x67, 0x00B1), // Plus over Minus  
			new uc_CharSet (0x68, 0x2424), // NL Symbol  
			new uc_CharSet (0x69, 0x240B), // VT Symbol 
			//            new uc_CharSet (0x6A, 0x2518), // Bottom Right Box 
			//            new uc_CharSet (0x6B, 0x2510), // Top Right Box
			//            new uc_CharSet (0x6C, 0x250C), // TopLeft Box
			//            new uc_CharSet (0x6D, 0x2514), // Bottom Left Box
			//            new uc_CharSet (0x6E, 0x253C), // Cross Piece
			new uc_CharSet (0x6F, 0x23BA), // Scan Line 1
			new uc_CharSet (0x70, 0x25BB), // Scan Line 3
			//            new uc_CharSet (0x71, 0x2500), // Horizontal Line (scan line 5 as well?)
			new uc_CharSet (0x72, 0x23BC), // Scan Line 7 
			new uc_CharSet (0x73, 0x23BD), // Scan Line 9 
			//            new uc_CharSet (0x74, 0x251C), // Left Tee Piece
			//            new uc_CharSet (0x75, 0x2524), // Right Tee Piece
			//            new uc_CharSet (0x76, 0x2534), // Bottom Tee Piece
			//            new uc_CharSet (0x77, 0x252C), // Top Tee Piece
			//            new uc_CharSet (0x78, 0x2502), // Vertical Line
			new uc_CharSet (0x79, 0x2264), // Less than or equal  
			new uc_CharSet (0x7A, 0x2265), // Greater than or equal 
			new uc_CharSet (0x7B, 0x03A0), // Capital Pi
			new uc_CharSet (0x7C, 0x2260), // Not Equal 
			new uc_CharSet (0x7D, 0x00A3), // Pound Sign 
			new uc_CharSet (0x7E, 0x00B7), // Middle Dot 
			};

			public static uc_CharSet[] DECS =
		{
			new uc_CharSet (0xA8, 0x0020), // Currency Sign
			new uc_CharSet (0xD7, 0x0152), // latin small ligature OE 
			new uc_CharSet (0xDD, 0x0178), // Capital Y with diaeresis
			new uc_CharSet (0xF7, 0x0153), // latin small ligature oe 
			new uc_CharSet (0xFD, 0x00FF), // Lowercase y with diaeresis
			};

			public static uc_CharSet[] ASCII = // same as Basic Latin
		{
			new uc_CharSet (0x00, 0x0000), //
			};

			public static uc_CharSet[] NRCUK = // UK National Replacement
		{
			new uc_CharSet (0x23, 0x00A3), //
			};

			public static uc_CharSet[] NRCFinnish = // Finnish National Replacement
		{
			new uc_CharSet (0x5B, 0x00C4), // A with diearesis
			new uc_CharSet (0x5C, 0x00D6), // O with diearesis
			new uc_CharSet (0x5D, 0x00C5), // A with hollow dot above
			new uc_CharSet (0x5E, 0x00DC), // U with diearesis
			new uc_CharSet (0x60, 0x00E9), // e with accute accent
			new uc_CharSet (0x7B, 0x00E4), // a with diearesis
			new uc_CharSet (0x7C, 0x00F6), // o with diearesis
			new uc_CharSet (0x7D, 0x00E5), // a with hollow dot above
			new uc_CharSet (0x7E, 0x00FC), // u with diearesis
			};

			public static uc_CharSet[] NRCFrench = // French National Replacement
		{
			new uc_CharSet (0x23, 0x00A3), // Pound Sign
			new uc_CharSet (0x40, 0x00E0), // a with grav accent
			new uc_CharSet (0x5B, 0x00B0), // Degree Symbol
			new uc_CharSet (0x5C, 0x00E7), // little cedila
			new uc_CharSet (0x5D, 0x00A7), // funny s (technical term)
			new uc_CharSet (0x7B, 0x00E9), // e with accute accent
			new uc_CharSet (0x7C, 0x00F9), // u with grav accent
			new uc_CharSet (0x7D, 0x00E8), // e with grav accent
			new uc_CharSet (0x7E, 0x00A8), // diearesis
			};

			public static uc_CharSet[] NRCFrenchCanadian = // French Canadian National Replacement
		{
			new uc_CharSet (0x40, 0x00E0), // a with grav accent
			new uc_CharSet (0x5B, 0x00E2), // a with circumflex
			new uc_CharSet (0x5C, 0x00E7), // little cedila
			new uc_CharSet (0x5D, 0x00EA), // e with circumflex
			new uc_CharSet (0x5E, 0x00CE), // i with circumflex
			new uc_CharSet (0x60, 0x00F4), // o with circumflex
			new uc_CharSet (0x7B, 0x00E9), // e with accute accent
			new uc_CharSet (0x7C, 0x00F9), // u with grav accent
			new uc_CharSet (0x7D, 0x00E8), // e with grav accent
			new uc_CharSet (0x7E, 0x00FB), // u with circumflex
			};

			public static uc_CharSet[] NRCGerman = // German National Replacement
		{
			new uc_CharSet (0x40, 0x00A7), // funny s
			new uc_CharSet (0x5B, 0x00C4), // A with diearesis
			new uc_CharSet (0x5C, 0x00D6), // O with diearesis
			new uc_CharSet (0x5D, 0x00DC), // U with diearesis
			new uc_CharSet (0x7B, 0x00E4), // a with diearesis
			new uc_CharSet (0x7C, 0x00F6), // o with diearesis
			new uc_CharSet (0x7D, 0x00FC), // u with diearesis
			new uc_CharSet (0x7E, 0x00DF), // funny B
			};

			public static uc_CharSet[] NRCItalian = // Italian National Replacement
		{
			new uc_CharSet (0x23, 0x00A3), // pound sign
			new uc_CharSet (0x40, 0x00A7), // funny s
			new uc_CharSet (0x5B, 0x00B0), // Degree Symbol
			new uc_CharSet (0x5C, 0x00E7), // little cedilla
			new uc_CharSet (0x5D, 0x00E9), // e with accute accent
			new uc_CharSet (0x60, 0x00F9), // u with grav accent
			new uc_CharSet (0x7B, 0x00E0), // a with grav accent
			new uc_CharSet (0x7C, 0x00F2), // o with grav accent
			new uc_CharSet (0x7D, 0x00E8), // e with grav accent
			new uc_CharSet (0x7E, 0x00CC), // I with grav accent
			};

			public static uc_CharSet[] NRCNorDanish = // Norwegian Danish National Replacement
		{
			new uc_CharSet (0x5B, 0x00C6), // AE ligature
			new uc_CharSet (0x5C, 0x00D8), // O with strikethough
			new uc_CharSet (0x5D, 0x00D8), // O with strikethough
			new uc_CharSet (0x5D, 0x00C5), // A with hollow dot above
			new uc_CharSet (0x7B, 0x00E6), // ae ligature
			new uc_CharSet (0x7C, 0x00F8), // o with strikethough
			new uc_CharSet (0x7D, 0x00F8), // o with strikethough
			new uc_CharSet (0x7D, 0x00E5), // a with hollow dot above
			};

			public static uc_CharSet[] NRCPortuguese = // Portuguese National Replacement
		{
			new uc_CharSet (0x5B, 0x00C3), // A with tilde
			new uc_CharSet (0x5C, 0x00C7), // big cedilla
			new uc_CharSet (0x5D, 0x00D5), // O with tilde
			new uc_CharSet (0x7B, 0x00E3), // a with tilde
			new uc_CharSet (0x7C, 0x00E7), // little cedilla
			new uc_CharSet (0x7D, 0x00F6), // o with tilde
			};

			public static uc_CharSet[] NRCSpanish = // Spanish National Replacement
		{
			new uc_CharSet (0x23, 0x00A3), // pound sign
			new uc_CharSet (0x40, 0x00A7), // funny s
			new uc_CharSet (0x5B, 0x00A1), // I with dot
			new uc_CharSet (0x5C, 0x00D1), // N with tilde
			new uc_CharSet (0x5D, 0x00BF), // Upside down question mark
			new uc_CharSet (0x7B, 0x0060), // back single quote
			new uc_CharSet (0x7C, 0x00B0), // Degree Symbol
			new uc_CharSet (0x7D, 0x00F1), // n with tilde
			new uc_CharSet (0x7E, 0x00E7), // small cedilla
			};

			public static uc_CharSet[] NRCSwedish = // Swedish National Replacement
		{
			new uc_CharSet (0x40, 0x00C9), // E with acute
			new uc_CharSet (0x5B, 0x00C4), // A with diearesis
			new uc_CharSet (0x5C, 0x00D6), // O with diearesis
			new uc_CharSet (0x5D, 0x00C5), // A with hollow dot above
			new uc_CharSet (0x5E, 0x00DC), // U with diearesis
			new uc_CharSet (0x60, 0x00E9), // e with accute accent
			new uc_CharSet (0x7B, 0x00E4), // a with diearesis
			new uc_CharSet (0x7C, 0x00F6), // o with diearesis
			new uc_CharSet (0x7D, 0x00E5), // a with hollow dot above
			new uc_CharSet (0x7E, 0x00FC), // u with diearesis
			};

			public static uc_CharSet[] NRCSwiss = // Swiss National Replacement
		{
			new uc_CharSet (0x23, 0x00F9), // u with grav accent
			new uc_CharSet (0x40, 0x00E0), // a with grav accent
			new uc_CharSet (0x5B, 0x00E9), // e with accute accent
			new uc_CharSet (0x5C, 0x00E7), // small cedilla
			new uc_CharSet (0x5D, 0x00EA), // e with circumflex
			new uc_CharSet (0x5E, 0x00CE), // i with circumflex
			new uc_CharSet (0x5F, 0x00E8), // e with grav accent
			new uc_CharSet (0x60, 0x00F4), // o with circumflex
			new uc_CharSet (0x7B, 0x00E4), // a with diearesis
			new uc_CharSet (0x7C, 0x00F6), // o with diearesis
			new uc_CharSet (0x7D, 0x00FC), // u with diearesis
			new uc_CharSet (0x7E, 0x00FB), // u with circumflex
			};

			public static uc_CharSet[] ISOLatin1S = // Same as Latin-1 Supplemental
		{
			new uc_CharSet (0x00, 0x0000) //
		};

			public enum Sets
			{
				None,
				DECSG,
				DECTECH,
				DECS,
				ASCII,
				ISOLatin1S,
				NRCUK,
				NRCFinnish,
				NRCFrench,
				NRCFrenchCanadian,
				NRCGerman,
				NRCItalian,
				NRCNorDanish,
				NRCPortuguese,
				NRCSpanish,
				NRCSwedish,
				NRCSwiss
			}
		}

		private class uc_Caret
		{
			public System.Drawing.Point    Pos;
			public System.Drawing.Color    Color  = System.Drawing.Color.FromArgb (255, 181,106);
			public System.Drawing.Bitmap   Bitmap = null;
			public System.Drawing.Graphics Buffer = null;
			public System.Boolean          IsOff  = false;
			public System.Boolean          EOL    = false;

			public uc_Caret ()
			{
				this.Pos = new System.Drawing.Point (0, 0);
			}
		}

		private class WMCodes
		{
			public const int WM_KEYFIRST    = 0x0100;
			public const int WM_KEYDOWN     = 0x0100;
			public const int WM_KEYUP       = 0x0101;
			public const int WM_CHAR        = 0x0102;
			public const int WM_DEADCHAR    = 0x0103;
			public const int WM_SYSKEYDOWN  = 0x0104;
			public const int WM_SYSKEYUP    = 0x0105;
			public const int WM_SYSCHAR     = 0x0106;
			public const int WM_SYSDEADCHAR = 0x0107;
			public const int WM_KEYLAST     = 0x0108;
		}

		private class uc_Mode
		{
			public static System.UInt32 Locked          = 1;           // Unlocked           = off 
			public static System.UInt32 BackSpace       = 2;           // Delete             = off 
			public static System.UInt32 NewLine         = 4;           // Line Feed          = off 
			public static System.UInt32 Repeat          = 8;           // No Repeat          = off 
			public static System.UInt32 AutoWrap        = 16;          // No AutoWrap        = off 
			public static System.UInt32 CursorAppln     = 32;          // Std Cursor Codes   = off 
			public static System.UInt32 KeypadAppln     = 64;          // Std Numeric Codes  = off 
			public static System.UInt32 DataProcessing  = 128;         // Typewriter         = off 
			public static System.UInt32 PositionReports = 256;         // CharacterCodes     = off
			public static System.UInt32 LocalEchoOff    = 512;         // LocalEchoOn        = off
			public static System.UInt32 OriginRelative  = 1024;        // OriginAbsolute     = off
			public static System.UInt32 LightBackground = 2048;        // DarkBackground     = off
			public static System.UInt32 National        = 4096;        // Multinational      = off
			public static System.UInt32 Any             = 0x80000000;  // Any Flags

			public System.UInt32 Flags;

			public uc_Mode (System.UInt32 InitialFlags)
			{
				this.Flags = InitialFlags;
			} 

			public uc_Mode ()
			{
				this.Flags = 0;
			} 
		}

		private class uc_Keyboard 
		{
			public event KeyboardEventHandler KeyboardEvent;

			private System.Boolean LastKeyDownSent  = false; // next WM_CHAR ignored if true 
			private bool AltIsDown    = false;
			private bool ShiftIsDown  = false;
			private bool CtrlIsDown   = false;

			private TerminalEmulator Parent;
    
			private uc_KeyMap KeyMap = new uc_KeyMap ();
    
			public uc_Keyboard (TerminalEmulator p1)
			{
				this.Parent = p1;
			}
    
			public void KeyDown (System.Windows.Forms.Message KeyMess)
			{
				System.Byte[] lBytes;
				System.Byte[] wBytes;
				System.UInt16 KeyValue    = 0; 
				System.UInt16 RepeatCount = 0; 
				System.Byte   ScanCode    = 0; 
				System.Byte   AnsiChar    = 0;
				System.UInt16 UniChar     = 0;
				System.Byte   Flags       = 0;
    
    
				lBytes = System.BitConverter.GetBytes (KeyMess.LParam.ToInt64 ());
                wBytes = System.BitConverter.GetBytes(KeyMess.WParam.ToInt64());
				RepeatCount = System.BitConverter.ToUInt16 (lBytes, 0);
				ScanCode    = lBytes[2];
				Flags       = lBytes[3];
    
				// key down messages send the scan code in wParam whereas
				// key press messages send the char and unicode values in this word
				if (KeyMess.Msg == WMCodes.WM_SYSKEYDOWN ||
					KeyMess.Msg == WMCodes.WM_KEYDOWN)
				{
					KeyValue = System.BitConverter.ToUInt16 (wBytes, 0);
    
					// set the key down flags. I know you can get alt from lparam flags
					// but this feels more consistent
					switch (KeyValue)
					{
						case 16:  // Shift Keys
						case 160: // L Shift Key
						case 161: // R Shift Keys
							this.ShiftIsDown = true;
							return;
    
						case 17:  // Ctrl Keys
						case 162: // L Ctrl Key
						case 163: // R Ctrl Key
							this.CtrlIsDown = true; 
							return;
    
						case 18:  // Alt Keys (Menu)
						case 164: // L Alt Key
						case 165: // R Ctrl Key
							this.AltIsDown = true; 
							return;
    
						default:
							break;
					}
    
					System.String Modifier = "Key";
    
					if (this.ShiftIsDown)
					{
						Modifier = "Shift"; 
					}
					else if (this.CtrlIsDown)
					{
						Modifier = "Ctrl";
					} 
					else if (this.AltIsDown)
					{
						Modifier = "Alt"; 
					}
    
					// the key pressed was not a modifier so check for an override string
					System.String OutString = KeyMap.Find (ScanCode, System.Convert.ToBoolean (Flags & 0x01), Modifier, Parent.Modes.Flags);

					this.LastKeyDownSent = false;

					if (OutString != "")
					{
						// Flag the event so that the associated WM_CHAR event (if any) is ignored
						this.LastKeyDownSent = true;

                    
						//Parent.NvtParser.ParseString (OutString); 
						//this.Parent.Invalidate ();
                    

						KeyboardEvent (this, OutString); 
					} 
    
				}
				else if (KeyMess.Msg == WMCodes.WM_SYSKEYUP ||
					KeyMess.Msg == WMCodes.WM_KEYUP)
				{
					KeyValue = System.BitConverter.ToUInt16 (wBytes, 0);
    
					switch (KeyValue)
					{
						case 16:  // Shift Keys
						case 160: // L Shift Key
						case 161: // R Shift Keys
							this.ShiftIsDown = false; 
							break;
    
						case 17:  // Ctrl Keys
						case 162: // L Ctrl Key
						case 163: // R Ctrl Key
							this.CtrlIsDown = false; 
							break;
    
						case 18:  // Alt Keys (Menu)
						case 164: // L Alt Key
						case 165: // R Ctrl Key
							this.AltIsDown = false; 
							break;
    
						default:
							break;
					}
				}
    
				else if (KeyMess.Msg == WMCodes.WM_SYSCHAR ||
					KeyMess.Msg == WMCodes.WM_CHAR)
				{
					AnsiChar    = wBytes[0];
					UniChar     = System.BitConverter.ToUInt16 (wBytes, 0);
    
    
					// if there's a string mapped to this key combo we want to ignore the character
					// as it has been overriden in the keydown event
    
					// only send the windows generated char if there was no custom
					// string sent by the keydown event
					if (this.LastKeyDownSent == false)
					{
						// send the character straight to the host if we haven't already handled the actual key press
						KeyboardEvent (this, System.Convert.ToChar (AnsiChar).ToString ()); 
					}
				}
    
				//System.Console.Write ("AnsiChar = {0} Result = {1} ScanCode = {2} KeyValue = {3} Flags = {4} Repeat = {5}\n ", 
				//AnsiChar, KeyMess.Result, ScanCode, KeyValue, Flags, RepeatCount);
			}
    
			private class uc_KeyInfo
			{
				public System.UInt16  ScanCode;
				public System.Boolean ExtendFlag;
				public System.String  Modifier;
				public System.String  OutString;
				public System.UInt32  Flag;
				public System.UInt32  FlagValue;
        
				public uc_KeyInfo (
					System.UInt16  p1,
					System.Boolean p2,
					System.String  p3,
					System.String  p4,
					System.UInt32  p5,
					System.UInt32  p6)
				{
					ScanCode    = p1;
					ExtendFlag  = p2;
					Modifier    = p3;
					OutString   = p4;
					Flag        = p5;
					FlagValue   = p6;
				}
			}

			private class uc_KeyMap
			{
				public System.Collections.ArrayList Elements = new System.Collections.ArrayList ();
        
				public uc_KeyMap () 
				{
					this.SetToDefault ();
				}
        
				// set the key mapping up to emulate most keys on a vt420
				public void SetToDefault ()
				{
					// add the default key mappings here
					Elements.Clear ();
        
					// TOPNZ Customisations these should be commented out
					//Elements.Add (new uc_KeyInfo (15,  false, "Shift", "\x1B[OI~", uc_Mode.Any,       0)); //ShTab
					//Elements.Add (new uc_KeyInfo (63,  false, "Key",   "\x1BOT",   uc_Mode.Any,       0)); //F5
					//Elements.Add (new uc_KeyInfo (64,  false, "Key",   "\x1BOU",   uc_Mode.Any,       0)); //F6
					//Elements.Add (new uc_KeyInfo (65,  false, "Key",   "\x1BOV",   uc_Mode.Any,       0)); //F7
					//Elements.Add (new uc_KeyInfo (66,  false, "Key",   "\x1BOW",   uc_Mode.Any,       0)); //F8
					//Elements.Add (new uc_KeyInfo (67,  false, "Key",   "\x1BOX",   uc_Mode.Any,       0)); //F9
					//Elements.Add (new uc_KeyInfo (68,  false, "Key",   "\x1BOY",   uc_Mode.Any,       0)); //F10
					//Elements.Add (new uc_KeyInfo (59,  false, "Shift", "\x1B[25~", uc_Mode.Any,       0)); //ShF1
					//Elements.Add (new uc_KeyInfo (60,  false, "Shift", "\x1B[26~", uc_Mode.Any,       0)); //ShF2
					//Elements.Add (new uc_KeyInfo (61,  false, "Shift", "\x1B[28~", uc_Mode.Any,       0)); //ShF3
					//Elements.Add (new uc_KeyInfo (62,  false, "Shift", "\x1B[29~", uc_Mode.Any,       0)); //ShF4
					//Elements.Add (new uc_KeyInfo (63,  false, "Shift", "\x1B[31~", uc_Mode.Any,       0)); //ShF5
					//Elements.Add (new uc_KeyInfo (64,  false, "Shift", "\x1B[32~", uc_Mode.Any,       0)); //ShF6
					//Elements.Add (new uc_KeyInfo (65,  false, "Shift", "\x1B[33~", uc_Mode.Any,       0)); //ShF7
					//Elements.Add (new uc_KeyInfo (66,  false, "Shift", "\x1B[34~", uc_Mode.Any,       0)); //ShF8
					//Elements.Add (new uc_KeyInfo (67,  false, "Shift", "\x1B[36~", uc_Mode.Any,       0)); //ShF9
					//Elements.Add (new uc_KeyInfo (68,  false, "Shift", "\x1B[37~", uc_Mode.Any,       0)); //ShF10
					//Elements.Add (new uc_KeyInfo (87,  false, "Shift", "\x1B[38~", uc_Mode.Any,       0)); //ShF11
					//Elements.Add (new uc_KeyInfo (88,  false, "Shift", "\x1B[39~", uc_Mode.Any,       0)); //ShF12


					// this is the initial list of keyboard codes
					Elements.Add (new uc_KeyInfo (15,  false, "Shift", "\x1B[Z",   uc_Mode.Any,         0)); //ShTab
					Elements.Add (new uc_KeyInfo (28,  false, "Key",   "\x0D",     uc_Mode.Any,         0)); //Return
					Elements.Add (new uc_KeyInfo (59,  false, "Key",   "\x1BOP",   uc_Mode.Any,         0)); //F1->PF1
					Elements.Add (new uc_KeyInfo (60,  false, "Key",   "\x1BOQ",   uc_Mode.Any,         0)); //F2->PF2
					Elements.Add (new uc_KeyInfo (61,  false, "Key",   "\x1BOR",   uc_Mode.Any,         0)); //F3->PF3
					Elements.Add (new uc_KeyInfo (62,  false, "Key",   "\x1BOS",   uc_Mode.Any,         0)); //F4->PF4
					Elements.Add (new uc_KeyInfo (63,  false, "Key",   "\x1B[15~", uc_Mode.Any,         0)); //F5
					Elements.Add (new uc_KeyInfo (64,  false, "Key",   "\x1B[17~", uc_Mode.Any,         0)); //F6
					Elements.Add (new uc_KeyInfo (65,  false, "Key",   "\x1B[18~", uc_Mode.Any,         0)); //F7
					Elements.Add (new uc_KeyInfo (66,  false, "Key",   "\x1B[19~", uc_Mode.Any,         0)); //F8
					Elements.Add (new uc_KeyInfo (67,  false, "Key",   "\x1B[20~", uc_Mode.Any,         0)); //F9
					Elements.Add (new uc_KeyInfo (68,  false, "Key",   "\x1B[21~", uc_Mode.Any,         0)); //F10
					Elements.Add (new uc_KeyInfo (87,  false, "Key",   "\x1B[23~", uc_Mode.Any,         0)); //F11
					Elements.Add (new uc_KeyInfo (88,  false, "Key",   "\x1B[24~", uc_Mode.Any,         0)); //F12
					Elements.Add (new uc_KeyInfo (61,  false, "Shift", "\x1B[25~", uc_Mode.Any,         0)); //ShF3 ->F13
					Elements.Add (new uc_KeyInfo (62,  false, "Shift", "\x1B[26~", uc_Mode.Any,         0)); //ShF4 ->F14
					Elements.Add (new uc_KeyInfo (63,  false, "Shift", "\x1B[28~", uc_Mode.Any,         0)); //ShF5 ->F15
					Elements.Add (new uc_KeyInfo (64,  false, "Shift", "\x1B[29~", uc_Mode.Any,         0)); //ShF6 ->F16
					Elements.Add (new uc_KeyInfo (65,  false, "Shift", "\x1B[31~", uc_Mode.Any,         0)); //ShF7 ->F17
					Elements.Add (new uc_KeyInfo (66,  false, "Shift", "\x1B[32~", uc_Mode.Any,         0)); //ShF8 ->F18
					Elements.Add (new uc_KeyInfo (67,  false, "Shift", "\x1B[33~", uc_Mode.Any,         0)); //ShF9 ->F19
					Elements.Add (new uc_KeyInfo (68,  false, "Shift", "\x1B[34~", uc_Mode.Any,         0)); //ShF10->F20
					Elements.Add (new uc_KeyInfo (87,  false, "Shift", "\x1B[28~", uc_Mode.Any,         0)); //ShF11->Help
					Elements.Add (new uc_KeyInfo (88,  false, "Shift", "\x1B[29~", uc_Mode.Any,         0)); //ShF12->Do
					Elements.Add (new uc_KeyInfo (71,  true,  "Key",   "\x1B[1~",  uc_Mode.Any,         0)); //Home
					Elements.Add (new uc_KeyInfo (82,  true,  "Key",   "\x1B[2~",  uc_Mode.Any,         0)); //Insert
					Elements.Add (new uc_KeyInfo (83,  true,  "Key",   "\x1B[3~",  uc_Mode.Any,         0)); //Delete
					Elements.Add (new uc_KeyInfo (79,  true,  "Key",   "\x1B[4~",  uc_Mode.Any,         0)); //End
					Elements.Add (new uc_KeyInfo (73,  true,  "Key",   "\x1B[5~",  uc_Mode.Any,         0)); //PageUp
					Elements.Add (new uc_KeyInfo (81,  true,  "Key",   "\x1B[6~",  uc_Mode.Any,         0)); //PageDown
					Elements.Add (new uc_KeyInfo (72,  true,  "Key",   "\x1B[A",   uc_Mode.CursorAppln, 0)); //CursorUp
					Elements.Add (new uc_KeyInfo (80,  true,  "Key",   "\x1B[B",   uc_Mode.CursorAppln, 0)); //CursorDown
					Elements.Add (new uc_KeyInfo (77,  true,  "Key",   "\x1B[C",   uc_Mode.CursorAppln, 0)); //CursorKeyRight
					Elements.Add (new uc_KeyInfo (75,  true,  "Key",   "\x1B[D",   uc_Mode.CursorAppln, 0)); //CursorKeyLeft
					Elements.Add (new uc_KeyInfo (72,  true,  "Key",   "\x1BOA",   uc_Mode.CursorAppln, 1)); //CursorUp
					Elements.Add (new uc_KeyInfo (80,  true,  "Key",   "\x1BOB",   uc_Mode.CursorAppln, 1)); //CursorDown
					Elements.Add (new uc_KeyInfo (77,  true,  "Key",   "\x1BOC",   uc_Mode.CursorAppln, 1)); //CursorKeyRight
					Elements.Add (new uc_KeyInfo (75,  true,  "Key",   "\x1BOD",   uc_Mode.CursorAppln, 1)); //CursorKeyLeft
					Elements.Add (new uc_KeyInfo (82,  false, "Key",   "\x1BOp",   uc_Mode.KeypadAppln, 1)); //Keypad0
					Elements.Add (new uc_KeyInfo (79,  false, "Key",   "\x1BOq",   uc_Mode.KeypadAppln, 1)); //Keypad1
					Elements.Add (new uc_KeyInfo (80,  false, "Key",   "\x1BOr",   uc_Mode.KeypadAppln, 1)); //Keypad2
					Elements.Add (new uc_KeyInfo (81,  false, "Key",   "\x1BOs",   uc_Mode.KeypadAppln, 1)); //Keypad3
					Elements.Add (new uc_KeyInfo (75,  false, "Key",   "\x1BOt",   uc_Mode.KeypadAppln, 1)); //Keypad4
					Elements.Add (new uc_KeyInfo (76,  false, "Key",   "\x1BOu",   uc_Mode.KeypadAppln, 1)); //Keypad5
					Elements.Add (new uc_KeyInfo (77,  false, "Key",   "\x1BOv",   uc_Mode.KeypadAppln, 1)); //Keypad6
					Elements.Add (new uc_KeyInfo (71,  false, "Key",   "\x1BOw",   uc_Mode.KeypadAppln, 1)); //Keypad7
					Elements.Add (new uc_KeyInfo (72,  false, "Key",   "\x1BOx",   uc_Mode.KeypadAppln, 1)); //Keypad8
					Elements.Add (new uc_KeyInfo (73,  false, "Key",   "\x1BOy",   uc_Mode.KeypadAppln, 1)); //Keypad9
					Elements.Add (new uc_KeyInfo (74,  false, "Key",   "\x1BOm",   uc_Mode.KeypadAppln, 1)); //Keypad-
					Elements.Add (new uc_KeyInfo (78,  false, "Key",   "\x1BOl",   uc_Mode.KeypadAppln, 1)); //Keypad+ (use instead of comma)
					Elements.Add (new uc_KeyInfo (83,  false, "Key",   "\x1BOn",   uc_Mode.KeypadAppln, 1)); //Keypad.
					Elements.Add (new uc_KeyInfo (28,  true,  "Key",   "\x1BOM",   uc_Mode.KeypadAppln, 1)); //Keypad Enter
					Elements.Add (new uc_KeyInfo (03,  false, "Ctrl",  "\x00",     uc_Mode.Any,         0)); //Ctrl2->Null
					Elements.Add (new uc_KeyInfo (57,  false, "Ctrl",  "\x00",     uc_Mode.Any,         0)); //CtrlSpaceBar->Null
					Elements.Add (new uc_KeyInfo (04,  false, "Ctrl",  "\x1B",     uc_Mode.Any,         0)); //Ctrl3->Escape
					Elements.Add (new uc_KeyInfo (05,  false, "Ctrl",  "\x1C",     uc_Mode.Any,         0)); //Ctrl4->FS
					Elements.Add (new uc_KeyInfo (06,  false, "Ctrl",  "\x1D",     uc_Mode.Any,         0)); //Ctrl5->GS
					Elements.Add (new uc_KeyInfo (07,  false, "Ctrl",  "\x1E",     uc_Mode.Any,         0)); //Ctrl6->RS
					Elements.Add (new uc_KeyInfo (08,  false, "Ctrl",  "\x1F",     uc_Mode.Any,         0)); //Ctrl7->US
					Elements.Add (new uc_KeyInfo (09,  false, "Ctrl",  "\x7F",     uc_Mode.Any,         0)); //Ctrl8->DEL
				}
        
				public System.String Find (
					System.UInt16  ScanCode, 
					System.Boolean ExtendFlag, 
					System.String  Modifier,
					System.UInt32  ModeFlags)
				{
					System.String OutString = "";
        
					for (int i=0; i < Elements.Count; i++)
					{
						uc_KeyInfo Element = (uc_KeyInfo) Elements[i];
        
						if (Element.ScanCode      == ScanCode   && 
							Element.ExtendFlag    == ExtendFlag &&
							Element.Modifier      == Modifier   &&
							(Element.Flag == uc_Mode.Any ||
							((Element.Flag & ModeFlags) ==  Element.Flag && Element.FlagValue == 1) ||
							((Element.Flag & ModeFlags) ==  0            && Element.FlagValue == 0)))
						{
							OutString = Element.OutString;
							return OutString;
						}
					}
        
					return OutString;
				}
			}
		}

		private class uc_VertScrollBar : System.Windows.Forms.VScrollBar
		{
			public uc_VertScrollBar ()
			{
				this.SetStyle (System.Windows.Forms.ControlStyles.Selectable, false);
				this.Maximum = 0;
				
			}
		}

		private class uc_Params
		{
			public System.Collections.ArrayList Elements = new System.Collections.ArrayList ();
  
			public uc_Params ()
			{
			}
    
			public System.Int32 Count () 
			{
				return this.Elements.Count;
			}
    
			public void Clear ()
			{
				this.Elements.Clear ();
			}
    
			public void Add (System.Char CurChar)
			{
				if (this.Count () < 1)
				{
					this.Elements.Add ("0");
				}
 
				if (CurChar == ';')
				{
					this.Elements.Add ("0");
				}
				else
				{
					int i = this.Elements.Count - 1;
					this.Elements[i] = ((System.String) this.Elements[i] + CurChar.ToString ());
				}
			}
		}
		/// <summary>
		/// This class provides functionality to parse the VT control characters fed
		/// to the terminal from the host machine and fire off the appropriate actions
		/// It implements Paul William's excellent VT500-Series Parser Model. 
		/// Paul's model can be found at vt100.net
		/// </summary>
		private class uc_Parser
		{
			public event ParserEventHandler ParserEvent;

			States State = States.Ground;
			System.Char   CurChar     = '\0';
			System.String CurSequence = "";
        
			System.Collections.ArrayList ParamList = new System.Collections.ArrayList ();
			uc_CharEvents        CharEvents        = new uc_CharEvents ();
			uc_StateChangeEvents StateChangeEvents = new uc_StateChangeEvents ();
			uc_Params            CurParams         = new uc_Params ();

			public uc_Parser ()
			{
			}
        
			// Every character received is treated as an event which could change the state of
			// the parser. The following section finds out which event or state change this character
			// should trigger and also finds out where we should store the incoming character.
			// The character may be a command, part of a sequence or a parameter; or it might just need
			// binning.
			// The sequence is: state change, store character, do action.
			public void ParseString (System.String InString)
			{
				States        NextState        = States.None;
				Actions       NextAction       = Actions.None;
				Actions       StateExitAction  = Actions.None;
				Actions       StateEntryAction = Actions.None;
    
				foreach (System.Char C in InString)
				{
					this.CurChar = C;
   
					// Get the next state and associated action based 
					// on the current state and char event
					CharEvents.GetStateEventAction (State, CurChar, ref NextState, ref NextAction);
    
					// execute any actions arising from leaving the current state
					if  (NextState != States.None && NextState != this.State)
					{
						// check for state exit actions
						StateChangeEvents.GetStateChangeAction (this.State, Transitions.Exit, ref StateExitAction);
    
						// Process the exit action
						if (StateExitAction != Actions.None) DoAction (StateExitAction);
    
					}
    
					// process the action specified
					if (NextAction != Actions.None) DoAction (NextAction);
    
					// set the new parser state and execute any actions arising entering the new state
					if  (NextState != States.None && NextState != this.State)
					{
						// change the parsers state attribute
						this.State = NextState;
    
						// check for state entry actions
						StateChangeEvents.GetStateChangeAction (this.State, Transitions.Entry, ref StateExitAction);
    
						// Process the entry action
						if (StateEntryAction != Actions.None) DoAction (StateEntryAction);
					}
				}
			}
        
			private void DoAction (Actions NextAction)
			{
				// Manage the contents of the Sequence and Param Variables
				switch (NextAction)
				{
					case Actions.Dispatch:
					case Actions.Collect:
						this.CurSequence += CurChar.ToString ();
						break;
    
					case Actions.NewCollect:
						this.CurSequence = CurChar.ToString ();
						this.CurParams.Clear ();
						break;
    
					case Actions.Param:
						this.CurParams.Add (CurChar);
						break;
    
					default:
						break;
				}
    
				// send the external event requests
				switch (NextAction)
				{
					case Actions.Dispatch:
					case Actions.Execute:
					case Actions.Put:
					case Actions.OscStart:
					case Actions.OscPut:
					case Actions.OscEnd:
					case Actions.Hook:
					case Actions.Unhook:
					case Actions.Print:

						//                    System.Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
						//                        this.CurSequence, this.CurChar.ToString (), this.CurParams.Count ().ToString (), 
						//                        this.State.ToString (), NextAction.ToString ());

						ParserEvent (this, new ParserEventArgs (NextAction, CurChar, CurSequence, CurParams));
						break;
    
					default:
						break; 
				}
    
    
				switch (NextAction)
				{
					case Actions.Dispatch:
						this.CurSequence = "";
						this.CurParams.Clear ();
						break;
					default:
						break;
				}
			}
    
			private enum States 
			{
				None               = 0,
				Ground             = 1,
				EscapeIntrmdt      = 2,
				Escape             = 3,
				CsiEntry           = 4,
				CsiIgnore          = 5,
				CsiParam           = 6,
				CsiIntrmdt         = 7,
				OscString          = 8,
				SosPmApcString     = 9,
				DcsEntry           = 10,
				DcsParam           = 11,
				DcsIntrmdt         = 12,
				DcsIgnore          = 13,
				DcsPassthrough     = 14,
				Anywhere           = 16
			}
        
			private enum Transitions 
			{
				None  = 0,
				Entry = 1,
				Exit  = 2
			}
        
			private struct uc_StateChangeInfo
			{
				public States        State;
				public Transitions   Transition;    // the next state we are going to 
				public Actions       NextAction;
        
				public uc_StateChangeInfo (
					States      p1,
					Transitions p2,
					Actions     p3)
				{
					this.State      = p1;
					this.Transition = p2;
					this.NextAction = p3;
				}
			}
    
			private class uc_StateChangeEvents 
			{
				private uc_StateChangeInfo[] Elements = 
			{
				new uc_StateChangeInfo (States.OscString,      Transitions.Entry, Actions.OscStart),
				new uc_StateChangeInfo (States.OscString,      Transitions.Exit,  Actions.OscEnd),
				new uc_StateChangeInfo (States.DcsPassthrough, Transitions.Entry, Actions.Hook),
				new uc_StateChangeInfo (States.DcsPassthrough, Transitions.Exit,  Actions.Unhook)
			};
        
				public uc_StateChangeEvents ()
				{
				}
        
				public System.Boolean GetStateChangeAction (
					States      State, 
					Transitions Transition,
					ref Actions     NextAction)
				{
					uc_StateChangeInfo Element;
        
					for (System.Int32 i = 0; i < Elements.Length; i++)
					{
						Element = Elements[i];
        
						if (State      == Element.State &&
							Transition == Element.Transition)
						{
							NextAction = Element.NextAction;
							return true;
						}
					}
                    
					return false;
				} 
			}
    
			private struct uc_CharEventInfo
			{
				public States        CurState;
				public System.Char   CharFrom;
				public System.Char   CharTo;
				public Actions       NextAction;
				public States        NextState;  // the next state we are going to 
        
				public uc_CharEventInfo (
					States  p1,
					System.Char   p2,
					System.Char   p3,
					Actions p4,
					States  p5)
				{
					this.CurState   = p1;
					this.CharFrom   = p2;
					this.CharTo     = p3;
					this.NextAction = p4;
					this.NextState  = p5;
				}
			}
        
			private class uc_CharEvents 
			{
				public System.Boolean GetStateEventAction (
					States        CurState, 
					System.Char   CurChar, 
					ref States    NextState,
					ref Actions   NextAction)
				{
					uc_CharEventInfo Element;
    
					// Codes A0-FF are treated exactly the same way as 20-7F
					// so we can keep are state table smaller by converting before we look
					// up the event associated with the character
    
					if (CurChar >= '\xA0' &&
						CurChar <= '\xFF')
					{
						CurChar -= '\x80';
					}
        
					for (System.Int32 i = 0; i < uc_CharEvents.Elements.Length; i++)
					{
						Element = uc_CharEvents.Elements[i];
        
						if (CurChar  >= Element.CharFrom &&
							CurChar  <= Element.CharTo   &&
							(CurState == Element.CurState || Element.CurState == States.Anywhere))
						{
							NextState  = Element.NextState;
							NextAction = Element.NextAction;
							return true;
						}
					}
                    
					return false;
				}

				public uc_CharEvents ()
				{
				}

				public static uc_CharEventInfo[] Elements = 
			{
				new uc_CharEventInfo (States.Anywhere,      '\x1B', '\x1B', Actions.NewCollect, States.Escape),
				new uc_CharEventInfo (States.Anywhere,      '\x18', '\x18', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x1A', '\x1A', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x1A', '\x1A', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x80', '\x8F', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x91', '\x97', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x99', '\x99', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x9A', '\x9A', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x9C', '\x9C', Actions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Anywhere,      '\x98', '\x98', Actions.None,       States.SosPmApcString),
				new uc_CharEventInfo (States.Anywhere,      '\x9E', '\x9F', Actions.None,       States.SosPmApcString),
				new uc_CharEventInfo (States.Anywhere,      '\x90', '\x90', Actions.NewCollect, States.DcsEntry),
				new uc_CharEventInfo (States.Anywhere,      '\x9D', '\x9D', Actions.None,       States.OscString),
				new uc_CharEventInfo (States.Anywhere,      '\x9B', '\x9B', Actions.NewCollect, States.CsiEntry),
				new uc_CharEventInfo (States.Ground,        '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Ground,        '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Ground,        '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Ground,        '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Ground,        '\x20', '\x7F', Actions.Print,      States.None),
				new uc_CharEventInfo (States.Ground,        '\x80', '\x8F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Ground,        '\x91', '\x9A', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Ground,        '\x9C', '\x9C', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.EscapeIntrmdt, '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.EscapeIntrmdt, '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.EscapeIntrmdt, '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.EscapeIntrmdt, '\x20', '\x2F', Actions.Collect,    States.None),
				new uc_CharEventInfo (States.EscapeIntrmdt, '\x30', '\x7E', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.Escape,        '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Escape,        '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Escape,        '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.Escape,        '\x58', '\x58', Actions.None,       States.SosPmApcString),
				new uc_CharEventInfo (States.Escape,        '\x5E', '\x5F', Actions.None,       States.SosPmApcString),
				new uc_CharEventInfo (States.Escape,        '\x50', '\x50', Actions.Collect,    States.DcsEntry),
				new uc_CharEventInfo (States.Escape,        '\x5D', '\x5D', Actions.None,       States.OscString),
				new uc_CharEventInfo (States.Escape,        '\x5B', '\x5B', Actions.Collect,    States.CsiEntry),
				new uc_CharEventInfo (States.Escape,        '\x30', '\x4F', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.Escape,        '\x51', '\x57', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.Escape,        '\x59', '\x5A', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.Escape,        '\x5C', '\x5C', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.Escape,        '\x60', '\x7E', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.Escape,        '\x20', '\x2F', Actions.Collect,    States.EscapeIntrmdt),
				new uc_CharEventInfo (States.CsiEntry,      '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiEntry,      '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiEntry,      '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiEntry,      '\x20', '\x2F', Actions.Collect,    States.CsiIntrmdt),
				new uc_CharEventInfo (States.CsiEntry,      '\x3A', '\x3A', Actions.None,       States.CsiIgnore),
				new uc_CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', Actions.Collect,    States.CsiParam),
				new uc_CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', Actions.Collect,    States.CsiParam),
				new uc_CharEventInfo (States.CsiEntry,      '\x30', '\x39', Actions.Param,      States.CsiParam),
				new uc_CharEventInfo (States.CsiEntry,      '\x3B', '\x3B', Actions.Param,      States.CsiParam),
				new uc_CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', Actions.Collect,    States.CsiParam),
				new uc_CharEventInfo (States.CsiEntry,      '\x40', '\x7E', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.CsiParam,      '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiParam,      '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiParam,      '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiParam,      '\x30', '\x39', Actions.Param,      States.None),
				new uc_CharEventInfo (States.CsiParam,      '\x3B', '\x3B', Actions.Param,      States.None),
				new uc_CharEventInfo (States.CsiParam,      '\x3A', '\x3A', Actions.None,       States.CsiIgnore),
				new uc_CharEventInfo (States.CsiParam,      '\x3C', '\x3F', Actions.None,       States.CsiIgnore),
				new uc_CharEventInfo (States.CsiParam,      '\x20', '\x2F', Actions.Collect,    States.CsiIntrmdt),
				new uc_CharEventInfo (States.CsiParam,      '\x40', '\x7E', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.CsiIgnore,     '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiIgnore,     '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiIgnore,     '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiIgnore,     '\x40', '\x7E', Actions.None,       States.Ground),
				new uc_CharEventInfo (States.CsiIntrmdt,    '\x00', '\x17', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiIntrmdt,    '\x19', '\x19', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiIntrmdt,    '\x1C', '\x1F', Actions.Execute,    States.None),
				new uc_CharEventInfo (States.CsiIntrmdt,    '\x20', '\x2F', Actions.Collect,    States.None),
				new uc_CharEventInfo (States.CsiIntrmdt,    '\x30', '\x3F', Actions.None,       States.CsiIgnore),
				new uc_CharEventInfo (States.CsiIntrmdt,    '\x40', '\x7E', Actions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.SosPmApcString,'\x9C', '\x9C', Actions.None,       States.Ground),
				new uc_CharEventInfo (States.DcsEntry,      '\x20', '\x2F', Actions.Collect,    States.DcsIntrmdt),
				new uc_CharEventInfo (States.DcsEntry,      '\x3A', '\x3A', Actions.None,       States.DcsIgnore),
				new uc_CharEventInfo (States.DcsEntry,      '\x30', '\x39', Actions.Param,      States.DcsParam),
				new uc_CharEventInfo (States.DcsEntry,      '\x3B', '\x3B', Actions.Param,      States.DcsParam),
				new uc_CharEventInfo (States.DcsEntry,      '\x3C', '\x3F', Actions.Collect,    States.DcsParam),
				new uc_CharEventInfo (States.DcsEntry,      '\x40', '\x7E', Actions.None,       States.DcsPassthrough),
				new uc_CharEventInfo (States.DcsIntrmdt,    '\x30', '\x3F', Actions.None,       States.DcsIgnore),
				new uc_CharEventInfo (States.DcsIntrmdt,    '\x40', '\x7E', Actions.None,       States.DcsPassthrough),
				new uc_CharEventInfo (States.DcsIgnore,     '\x9C', '\x9C', Actions.None,       States.Ground),
				new uc_CharEventInfo (States.DcsParam,      '\x30', '\x39', Actions.Param,      States.None),
				new uc_CharEventInfo (States.DcsParam,      '\x3B', '\x3B', Actions.Param,      States.None),
				new uc_CharEventInfo (States.DcsParam,      '\x20', '\x2F', Actions.Collect,    States.DcsIntrmdt),
				new uc_CharEventInfo (States.DcsParam,      '\x3A', '\x3A', Actions.None,       States.DcsIgnore),
				new uc_CharEventInfo (States.DcsParam,      '\x3C', '\x3F', Actions.None,       States.DcsIgnore),
				new uc_CharEventInfo (States.DcsPassthrough,'\x00', '\x17', Actions.Put,        States.None),
				new uc_CharEventInfo (States.DcsPassthrough,'\x19', '\x19', Actions.Put,        States.None),
				new uc_CharEventInfo (States.DcsPassthrough,'\x1C', '\x1F', Actions.Put,        States.None),
				new uc_CharEventInfo (States.DcsPassthrough,'\x20', '\x7E', Actions.Put,        States.None),
				new uc_CharEventInfo (States.DcsPassthrough,'\x9C', '\x9C', Actions.None,       States.Ground),
				new uc_CharEventInfo (States.OscString,     '\x20', '\x7F', Actions.OscPut,     States.None),
				new uc_CharEventInfo (States.OscString,     '\x9C', '\x9C', Actions.None,       States.Ground)
			};
			}
		}    

		private class uc_TelnetParser
		{
			public event NvtParserEventHandler NvtParserEvent;
			States State = States.Ground;
			System.Char   CurChar     = '\0';
			System.String CurSequence = "";
        
			System.Collections.ArrayList ParamList = new System.Collections.ArrayList ();
			uc_CharEvents        CharEvents        = new uc_CharEvents ();
			uc_StateChangeEvents StateChangeEvents = new uc_StateChangeEvents ();
			uc_Params            CurParams         = new uc_Params ();

			public uc_TelnetParser ()
			{
			}

			public void ParseString (System.String InString)
			{
				States        NextState        = States.None;
				NvtActions    NextAction       = NvtActions.None;
				NvtActions    StateExitAction  = NvtActions.None;
				NvtActions    StateEntryAction = NvtActions.None;
    
				foreach (System.Char C in InString)
				{
					this.CurChar = C;
   
					// Get the next state and associated action based 
					// on the current state and char event
					CharEvents.GetStateEventAction (State, CurChar, ref NextState, ref NextAction);
    
					// execute any actions arising from leaving the current state
					if  (NextState != States.None && NextState != this.State)
					{
						// check for state exit actions
						StateChangeEvents.GetStateChangeAction (this.State, Transitions.Exit, ref StateExitAction);
    
						// Process the exit action
						if (StateExitAction != NvtActions.None) DoAction (StateExitAction);
    
					}
    
					// process the action specified
					if (NextAction != NvtActions.None) DoAction (NextAction);
    
					// set the new parser state and execute any actions arising entering the new state
					if  (NextState != States.None && NextState != this.State)
					{
						// change the parsers state attribute
						this.State = NextState;
    
						// check for state entry actions
						StateChangeEvents.GetStateChangeAction (this.State, Transitions.Entry, ref StateExitAction);
    
						// Process the entry action
						if (StateEntryAction != NvtActions.None) DoAction (StateEntryAction);
					}
				}
			}
        
			private void DoAction (NvtActions NextAction)
			{
				// Manage the contents of the Sequence and Param Variables
				switch (NextAction)
				{
					case NvtActions.Dispatch:
					case NvtActions.Collect:
						this.CurSequence += CurChar.ToString ();
						break;
    
					case NvtActions.NewCollect:
						this.CurSequence = CurChar.ToString ();
						this.CurParams.Clear ();
						break;
    
					case NvtActions.Param:
						this.CurParams.Add (CurChar);
						break;
    
					default:
						break;
				}
    
				// send the external event requests
				switch (NextAction)
				{
					case NvtActions.Dispatch:

						//                    System.Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
						//                        this.CurSequence, (int) this.CurChar, this.CurParams.Count (), 
						//                        this.State, NextAction);

						NvtParserEvent (this, new NvtParserEventArgs (NextAction, CurChar, CurSequence, CurParams));
						break;

					case NvtActions.Execute:
					case NvtActions.SendUp:
						NvtParserEvent (this, new NvtParserEventArgs (NextAction, CurChar, CurSequence, CurParams));
    
						//                    System.Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
						//                        this.CurSequence, (int) this.CurChar, this.CurParams.Count (), 
						//                        this.State, NextAction);
						break;
					default:
						break; 
				}
    
    
				switch (NextAction)
				{
					case NvtActions.Dispatch:
						this.CurSequence = "";
						this.CurParams.Clear ();
						break;

					default:
						break;
				}
			}
    
			private enum States 
			{
				None              = 0,
				Ground            = 1,
				Command           = 2,
				Anywhere          = 3,
				Synch             = 4,
				Negotiate         = 5,  
				SynchNegotiate    = 6,  
				SubNegotiate      = 7,  
				SubNegValue       = 8,  
				SubNegParam       = 9,  
				SubNegEnd         = 10,  
				SynchSubNegotiate = 11,  
			}
        
			private enum Transitions 
			{
				None  = 0,
				Entry = 1,
				Exit  = 2,
			}
        
			private struct uc_StateChangeInfo
			{
				public States        State;
				public Transitions   Transition;    // the next state we are going to 
				public NvtActions    NextAction;
        
				public uc_StateChangeInfo (
					States      p1,
					Transitions p2,
					NvtActions  p3)
				{
					this.State      = p1;
					this.Transition = p2;
					this.NextAction = p3;
				}
			}
    
			private class uc_StateChangeEvents 
			{
				private uc_StateChangeInfo[] Elements = 
			{
				new uc_StateChangeInfo (States.None, Transitions.None, NvtActions.None),
				};
        
				public uc_StateChangeEvents ()
				{
				}
        
				public System.Boolean GetStateChangeAction (
					States      State, 
					Transitions Transition,
					ref NvtActions  NextAction)
				{
					uc_StateChangeInfo Element;
        
					for (System.Int32 i = 0; i < Elements.Length; i++)
					{
						Element = Elements[i];
        
						if (State      == Element.State &&
							Transition == Element.Transition)
						{
							NextAction = Element.NextAction;
							return true;
						}
					}
                    
					return false;
				} 
			}
    
			private struct uc_CharEventInfo
			{
				public States        CurState;
				public System.Char   CharFrom;
				public System.Char   CharTo;
				public NvtActions    NextAction;
				public States        NextState;  // the next state we are going to 
        
				public uc_CharEventInfo (
					States  p1,
					System.Char   p2,
					System.Char   p3,
					NvtActions    p4,
					States  p5)
				{
					this.CurState   = p1;
					this.CharFrom   = p2;
					this.CharTo     = p3;
					this.NextAction = p4;
					this.NextState  = p5;
				}
			}
        
			private class uc_CharEvents 
			{
				public System.Boolean GetStateEventAction (
					States         CurState, 
					System.Char    CurChar, 
					ref States     NextState,
					ref NvtActions NextAction)
				{
					uc_CharEventInfo Element;
    
        
					for (System.Int32 i = 0; i < uc_CharEvents.Elements.Length; i++)
					{
						Element = uc_CharEvents.Elements[i];
        
						if (CurChar  >= Element.CharFrom &&
							CurChar  <= Element.CharTo   &&
							(CurState == Element.CurState || Element.CurState == States.Anywhere))
						{
							NextState  = Element.NextState;
							NextAction = Element.NextAction;
							return true;
						}
					}
                    
					return false;
				}

				public uc_CharEvents ()
				{
				}

				public static uc_CharEventInfo[] Elements = 
			{
				new uc_CharEventInfo (States.Ground,       (char) 000, (char) 254, NvtActions.SendUp,     States.None),
				new uc_CharEventInfo (States.Ground,       (char) 255, (char) 255, NvtActions.None,       States.Command),
				new uc_CharEventInfo (States.Command,      (char) 000, (char) 239, NvtActions.SendUp,     States.Ground),
				new uc_CharEventInfo (States.Command,      (char) 240, (char) 241, NvtActions.None,       States.Ground),
				new uc_CharEventInfo (States.Command,      (char) 242, (char) 249, NvtActions.Execute,    States.Ground),
				new uc_CharEventInfo (States.Command,      (char) 250, (char) 250, NvtActions.NewCollect, States.SubNegotiate),
				new uc_CharEventInfo (States.Command,      (char) 251, (char) 254, NvtActions.NewCollect, States.Negotiate),
				new uc_CharEventInfo (States.Command,      (char) 255, (char) 255, NvtActions.SendUp,     States.Ground),
				new uc_CharEventInfo (States.SubNegotiate, (char) 000, (char) 255, NvtActions.Collect,    States.SubNegValue),
				new uc_CharEventInfo (States.SubNegValue,  (char) 000, (char) 001, NvtActions.Collect,    States.SubNegParam),
				new uc_CharEventInfo (States.SubNegValue,  (char) 002, (char) 255, NvtActions.None,       States.Ground),
				new uc_CharEventInfo (States.SubNegParam,  (char) 000, (char) 254, NvtActions.Param,      States.None),
				new uc_CharEventInfo (States.SubNegParam,  (char) 255, (char) 255, NvtActions.None,       States.SubNegEnd),
				new uc_CharEventInfo (States.SubNegEnd,    (char) 000, (char) 239, NvtActions.None,       States.Ground),
				new uc_CharEventInfo (States.SubNegEnd,    (char) 240, (char) 240, NvtActions.Dispatch,   States.Ground),
				new uc_CharEventInfo (States.SubNegEnd,    (char) 241, (char) 254, NvtActions.None,       States.Ground),
				new uc_CharEventInfo (States.SubNegEnd,    (char) 255, (char) 255, NvtActions.Param,      States.SubNegParam),
				new uc_CharEventInfo (States.Negotiate,    (char) 000, (char) 255, NvtActions.Dispatch,   States.Ground),
				};
			}
		}

		#endregion
		#region Private Structs
		private struct ParserEventArgs 
		{
			public Actions           Action;
			public System.Char       CurChar;
			public System.String     CurSequence;
			public uc_Params         CurParams;

			public ParserEventArgs (
				Actions           p1,
				System.Char       p2,
				System.String     p3,
				uc_Params         p4)
			{
				Action       = p1;
				CurChar      = p2;
				CurSequence  = p3;
				CurParams    = p4;
			}
		}

		private struct CharAttribStruct
		{
			public System.Boolean       IsBold;
			public System.Boolean       IsDim;
			public System.Boolean       IsUnderscored;
			public System.Boolean       IsBlinking;
			public System.Boolean       IsInverse;
			public System.Boolean       IsPrimaryFont;
			public System.Boolean       IsAlternateFont;
			public System.Boolean       UseAltColor;
			public System.Drawing.Color AltColor;
			public System.Boolean       UseAltBGColor;
			public System.Drawing.Color AltBGColor;
			public uc_Chars             GL;
			public uc_Chars             GR;
			public uc_Chars             GS;
			public System.Boolean       IsDECSG;

			public CharAttribStruct ( 
				System.Boolean       p1,
				System.Boolean       p2,
				System.Boolean       p3,
				System.Boolean       p4,
				System.Boolean       p5,
				System.Boolean       p6,
				System.Boolean       p7,
				System.Boolean       p12,
				System.Drawing.Color p13,
				System.Boolean       p14,
				System.Drawing.Color p15,
				uc_Chars             p16,
				uc_Chars             p17,
				uc_Chars             p18,
				System.Boolean       p19)
			{
				IsBold          = p1;
				IsDim           = p2;
				IsUnderscored   = p3;
				IsBlinking      = p4;
				IsInverse       = p5;
				IsPrimaryFont   = p6;
				IsAlternateFont = p7;
				UseAltColor     = p12;
				AltColor        = p13;
				UseAltBGColor   = p14;
				AltBGColor      = p15;
				GL              = p16;
				GR              = p17;
				GS              = p18;
				IsDECSG         = p19;
			}

		}    

		private struct NvtParserEventArgs 
		{
			public NvtActions        Action;
			public System.Char       CurChar;
			public System.String     CurSequence;
			public uc_Params         CurParams;

			public NvtParserEventArgs (
				NvtActions        p1,
				System.Char       p2,
				System.String     p3,
				uc_Params         p4)
			{
				Action       = p1;
				CurChar      = p2;
				CurSequence  = p3;
				CurParams    = p4;
			}
		}

		#endregion
		#region Private Enums
		private enum NvtCommand 
		{
			SE    = 240, 
			NOP   = 241, 
			DM    = 242, 
			BREAK = 243, 
			IP    = 244, 
			AO    = 245, 
			AYT   = 246, 
			EC    = 247, 
			EL    = 248, 
			GA    = 249, 
			SB    = 250, 
			WILL  = 251, 
			WONT  = 252, 
			DO    = 253, 
			DONT  = 254, 
			IAC   = 255, 
		}
    
		private enum NvtOption
		{
			ECHO     = 1,   // echo
			SGA      = 3,   // suppress go ahead
			STATUS   = 5,   // status
			TM       = 6,   // timing mark
			TTYPE    = 24,  // terminal type
			NAWS     = 31,  // window size
			TSPEED   = 32,  // terminal speed
			LFLOW    = 33,  // remote flow control
			LINEMODE = 34,  // linemode
			ENVIRON  = 36,  // environment variables
		}

		private enum NvtActions 
		{
			None       = 0,
			SendUp     = 1,
			Dispatch   = 2,
			Collect    = 4,
			NewCollect = 5,
			Param      = 6,
			Execute    = 7,
		}

		private enum Actions 
		{
			None       = 0,
			Dispatch   = 1,
			Execute    = 2,
			Ignore     = 3,
			Collect    = 4,
			NewCollect = 5,
			Param      = 6,
			OscStart   = 8,
			OscPut     = 9,
			OscEnd     = 10,
			Hook       = 11,
			Unhook     = 12,
			Put        = 13,
			Print      = 14
		}

		#endregion
	}
	#region Routrek SSH Reader Class
	class Reader : Routrek.SSHC.ISSHConnectionEventReceiver, Routrek.SSHC.ISSHChannelEventReceiver 
	{
		private TerminalEmulator Terminal;
		public Reader (object obj)
		{
			Terminal = (TerminalEmulator) obj;
		}
		public Routrek.SSHC.SSHConnection _conn;
		public bool _ready;
		public void OnData(byte[] data, int offset, int length) 
		{
			Terminal.Write(data, offset, length);
            //rtb.AppendText(Encoding.Default.GetString(data, offset, length));
            //System.Console.Write(Encoding.Default.GetString(data, offset, length));
		}
		public void OnDebugMessage(bool always_display, byte[] data) 
		{
            //Debug.WriteLine("DEBUG: "+ Encoding.Default.GetString(data));
		}
		public void OnIgnoreMessage(byte[] data) 
		{
            //Debug.WriteLine("Ignore: "+ Encoding.Default.GetString(data));
		}
		public void OnAuthenticationPrompt(string[] msg) 
		{
			//Debug.WriteLine("Auth Prompt "+msg[0]);
		}

		public void OnError(Exception error, string msg) 
		{
			//Debug.WriteLine("ERROR: "+ msg);
		}
		public void OnChannelClosed() 
		{
			//Debug.WriteLine("Channel closed");
			_conn.Disconnect("");
			//_conn.AsyncReceive(this);
		}
		public void OnChannelEOF() 
		{
			_pf.Close();
			//Debug.WriteLine("Channel EOF");
		}
		public void OnExtendedData(int type, byte[] data) 
		{
			//Debug.WriteLine("EXTENDED DATA");
		}
		public void OnConnectionClosed() 
		{
			//Debug.WriteLine("Connection closed");
		}
		public void OnUnknownMessage(byte type, byte[] data) 
		{
			//Debug.WriteLine("Unknown Message " + type);
		}
		public void OnChannelReady() 
		{
			_ready = true;
		}
		public void OnChannelError(Exception error, string msg) 
		{
			//Debug.WriteLine("Channel ERROR: "+ msg);
		}
		public void OnMiscPacket(byte type, byte[] data, int offset, int length) 
		{
		}

		public Routrek.SSHC.PortForwardingCheckResult CheckPortForwardingRequest(string host, int port, string originator_host, int originator_port) 
		{
			Routrek.SSHC.PortForwardingCheckResult r = new Routrek.SSHC.PortForwardingCheckResult();
			r.allowed = true;
			r.channel = this;
			return r;
		}
		public void EstablishPortforwarding(Routrek.SSHC.ISSHChannelEventReceiver rec, Routrek.SSHC.SSHChannel channel) 
		{
			_pf = channel;
		}

		public Routrek.SSHC.SSHChannel _pf;
	}
	#endregion
}
