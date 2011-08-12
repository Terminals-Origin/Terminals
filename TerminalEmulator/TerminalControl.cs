using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        #region Fields

        public String TerminalType = "vt220";
        //private State						receiveState;

        private ContextMenu contextMenu1;    // rightclick menu
        private MenuItem mnuCopy;
        private MenuItem mnuPaste;
        private MenuItem mnuCopyPaste;
        private Point BeginDrag;       // used in Mouse Selecting Text
        private Point EndDrag;         // used in mouse selecting text
        private String TextAtCursor;    // used to store Cursortext while scrolling
        private Int32 LastVisibleLine; // used for scrolling
        private Boolean XOFF = false;
        private String OutBuff = String.Empty;
        private Int32 ScrollbackBufferSize;
        private StringCollection ScrollbackBuffer;
        private uc_Parser Parser = null;
        private uc_Keyboard Keyboard = null;
        private uc_TabStops TabStops = null;
        private Bitmap EraseBitmap = null;
        private Graphics EraseBuffer = null;
        private Char[][] CharGrid = null;
        private CharAttribStruct[][] AttribGrid = null;
        private CharAttribStruct CharAttribs;
        private Int32 _cols;
        private Int32 _rows;
        private Int32 TopMargin;
        private Int32 BottomMargin;
        private Size CharSize;
        private Int32 UnderlinePos;
        private uc_Caret Caret;
        private System.Collections.ArrayList SavedCarets;
        private Point DrawStringOffset;
        private Color BoldColor;
        
        private uc_Chars G0;
        private uc_Chars G1;
        private uc_Chars G2;
        private uc_Chars G3;
        private uc_Mode Modes;
        private uc_VertScrollBar VertScrollBar;

        private Color blinkColor;
        public Color BlinkColor
        {
            get { return blinkColor; }
            set 
            {
                blinkColor = value;
                UpdateCaret();
            }
        }

        #endregion

        #region Public Delegates

        public delegate void DataRequest(Byte[] data);

        private delegate void KeyboardEventHandler(Object Sender, String e);

        private delegate void RefreshEventHandler();

        private delegate void RxdTextEventHandler(String sReceived);

        private delegate void CaretOffEventHandler();

        private delegate void CaretOnEventHandler();

        private delegate void ParserEventHandler(Object Sender, ParserEventArgs e);

        #endregion

        #region Events

        public event DataRequest OnDataRequested;

        private event RefreshEventHandler RefreshEvent;

        private event RxdTextEventHandler RxdTextEvent;

        private event CaretOffEventHandler CaretOffEvent;

        private event CaretOnEventHandler CaretOnEvent;

        #endregion

        #region Constructors

        public TerminalEmulator()
        {
            this.ScrollbackBufferSize = 3000;
            this.ScrollbackBuffer = new StringCollection();

            // set the display options
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            this.Keyboard = new uc_Keyboard(this);
            this.Parser = new uc_Parser();
            this.Caret = new uc_Caret();
            this.Modes = new uc_Mode();
            this.TabStops = new uc_TabStops();
            this.SavedCarets = new System.Collections.ArrayList();

            ////this.Name = "ACK-TERM";
            ////this.Text = "ACK-TERM";

            this.Caret.Pos = new Point(0, 0);
            this.CharSize = new Size();

            AssignDefaultColors();

            this.G0 = new uc_Chars(uc_Chars.Sets.ASCII);
            this.G1 = new uc_Chars(uc_Chars.Sets.ASCII);
            this.G2 = new uc_Chars(uc_Chars.Sets.DECSG);
            this.G3 = new uc_Chars(uc_Chars.Sets.DECSG);

            this.CharAttribs.GL = G0;
            this.CharAttribs.GR = G2;
            this.CharAttribs.GS = null;

            this.GetFontInfo();

            // Create and initialize contextmenu
            ////this.contextMenu1 = new ContextMenu();
            ////this.mnuCopy = new MenuItem("Copy");
            ////this.mnuPaste = new MenuItem("Paste");
            ////this.mnuCopyPaste = new MenuItem("Copy and Paste");
            ////this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            ////                                                                            this.mnuCopyPaste,
            ////                                                                            this.mnuPaste,
            ////                                                                            this.mnuCopy
            ////                                                                          });
            ////this.mnuCopy.Index = 0;
            ////this.mnuPaste.Index = 1;
            ////this.mnuCopyPaste.Index = 2;
            ////this.ContextMenu = this.contextMenu1;
            ////this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            ////this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            ////this.mnuCopyPaste.Click += new System.EventHandler(this.mnuCopyPaste_Click);

            // Create and initialize a VScrollBar.
            this.VertScrollBar = new uc_VertScrollBar();
            this.VertScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);

            // Dock the scroll bar to the right side of the form.
            this.VertScrollBar.Dock = System.Windows.Forms.DockStyle.Right;

            // Add the scroll bar to the form.
            Controls.Add(this.VertScrollBar);

            // create the character grid (rows by columns). This is a shadow of what's displayed
            // Set the window size to match
            this.SetSize(24, 80);

            this.Parser.ParserEvent += new ParserEventHandler(this.CommandRouter);
            this.Keyboard.KeyboardEvent += new KeyboardEventHandler(this.DispatchMessage);
            this.RefreshEvent += new RefreshEventHandler(this.ShowBuffer);
            this.CaretOffEvent += new CaretOffEventHandler(this.CaretOff);
            this.CaretOnEvent += new CaretOnEventHandler(this.CaretOn);
            this.RxdTextEvent += new RxdTextEventHandler(this.Parser.ParseString);
            this.FontChanged += new EventHandler(OnFontChanged);

            this.BeginDrag = new Point();
            this.EndDrag = new Point();

            // Enable autowrap when reaching the end of columnwidth
            this.Modes.Flags = this.Modes.Flags | uc_Mode.AutoWrap;

            this.Cursor = Cursors.IBeam;
        }

      private  void OnFontChanged(object sender, EventArgs e)
      {
          System.Diagnostics.Debug.WriteLine(String.Format("Font changed to {0}", this.Font.ToString()));
          GetFontInfo();
      }

      private void AssignDefaultColors()
      {
        //this.FGColor      = System.Drawing.Color.FromArgb (200, 200, 200);
        this.ForeColor = Color.GreenYellow;
        this.BackColor = Color.FromArgb(0, 0, 160);
        this.BoldColor = Color.FromArgb(255, 255, 255);
        this.BlinkColor = Color.Red;
      }

      #endregion

        #region Public Properties

        public Int32 Rows
        {
            get
            {
                return this._rows;
            }

            set
            {
                SetSize(value, this._cols);
            }
        }

        public Int32 Columns
        {
            get
            {
                return this._cols;
            }

            set
            {
                SetSize(this._rows, value);
            }
        }

        #endregion

        #region Public Methods

        public void IndicateData (Byte[] data)
        {
            String sReceived = Encoding.Default.GetString(data, 0, data.Length);
            this.Invoke(this.RxdTextEvent, new String[] {String.Copy (sReceived)});
            this.Invoke(this.RefreshEvent);
        }

        public StringCollection ScreenScrape(Int32 StartRow, Int32 StartColumn, Int32 EndRow, Int32 EndColumn)
        {
            ////this.CharGrid[7][13] TODO what happened to this ?
            ////this.CharGrid[ROW][COL]
            
            StringCollection ScrapedText = new StringCollection();

            String row = String.Empty;
            Int32 cStart = 0;
            Int32 cEnd = this._cols;

            for (Int32 r = StartRow; r <= EndRow; r++)
            {
                if (r == StartRow)
                    cStart = StartColumn;

                if (r == EndRow)
                    cEnd = EndColumn;

                for (Int32 c = cStart; c <= cEnd; c++)
                {
                    Char val = this.CharGrid[r][c];;
                    if (val == '\0')
                        break;

                    row = row + val;                        
                }

                ScrapedText.Add(row);
                row = String.Empty;
                cStart = 0;
                cEnd = this._cols;
            }

            return ScrapedText;
        }

        #endregion

        #region Overrides

        protected override void OnResize (System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Before: {0}x{1}, Console: {2}x{3}",
                this.Width, this.Height, this.CharSize.Width, this.CharSize.Height));
            // reset scrollbar values);
            this.SetScrollBarValues();

            // capture text at cursor b/c it's not in the scrollback buffer yet
            String TextAtCursor = String.Empty;
            for (Int32 x = 0; x < this._cols; x++)
            {
                Char CurChar = this.CharGrid[this.Caret.Pos.Y][x];

                if (CurChar == '\0')
                {
                    continue;
                }

                TextAtCursor = TextAtCursor + Convert.ToString(CurChar);
            }
            
            // calculate new rows and columns
            Int32 columns = this.ClientSize.Width / this.CharSize.Width - 1;
            Int32 rows = this.ClientSize.Height / this.CharSize.Height;

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
            {
                if (this.Bottom > this.Parent.ClientSize.Height)
                    this.Height = this.Parent.ClientSize.Height - this.Top;
            }

            // reset the char grid
            this.SetSize(rows, columns);

            ////Console.WriteLine(Convert.ToString(rows) + " rows. " + Convert.ToString(this.ScrollbackBuffer.Count + " buffer lines"));
            
            // populate char grid from ScrollbackBuffer
            // parse through ScrollbackBuffer from the end
            // ScrollbackBuffer[0] is the "oldest" string
            // chargrid[0] is the top row on the display
            StringCollection visiblebuffer = new StringCollection();
            for (Int32 i = this.ScrollbackBuffer.Count - 1; i >= 0; i--)
            {
                visiblebuffer.Insert(0, this.ScrollbackBuffer[i]);

                // don't parse more strings than our display can show
                if (visiblebuffer.Count >= rows - 1) // rows -1 to leave line for cursor space
                    break;
            }

            Int32 lastline = 0;
            for (Int32 i = 0; i < visiblebuffer.Count; i++)
            {
                //Console.WriteLine("Writing string to display: " + visiblebuffer[i]);
                for (Int32 column = 0; column < columns; column++)
                {
                    //this.CharGrid[i][column] = '0';
                    if (column > visiblebuffer[i].Length - 1)
                        continue;

                    this.CharGrid[i][column] = visiblebuffer[i].ToCharArray()[column];
                }

                lastline = i;
            }

            // replace cursor text
            for (Int32 column = 0; column < columns; column++)
            {
                if (column > TextAtCursor.Length - 1)
                    continue;

                this.CharGrid[lastline+1][column] = TextAtCursor.ToCharArray()[column];
            }

            this.CaretToAbs(lastline+1, TextAtCursor.Length);
            this.Refresh();
            
            base.OnResize(e);
            System.Diagnostics.Debug.WriteLine(String.Format("After: {0}x{1}, Console: {2}x{3}",
                this.Width, this.Height, this.CharSize.Width, this.CharSize.Height));
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

            this.ResetSelectedTextHighlight();

            if (endRow < begRow) // we're parsing backwards
            {
                Int32 i = endRow;
                endRow = begRow;
                begRow = i;
                for (Int32 curRow = begRow; curRow <= endRow; curRow++)
                {
                    if (curRow <= 0)
                        continue;

                    for (Int32 curCol = 0; curCol < this._cols; curCol++)
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
                Int32 i = endCol;
                endCol = begCol;
                begCol = i;
            }

            // parse the rows affected and highlight them
            // endRow/endCol are where the mouse is now
            // begRow/begCol are where the mouse was when the button was pushed
            for (Int32 curRow = begRow; curRow <= endRow; curRow++)
            {
                if (curRow >= this._rows)
                    break;

                for (Int32 curCol = 0; curCol < this._cols; curCol++)
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
                {
                    this.ResetSelectedTextHighlight();
                }
                else
                {
                    // Mouse click and drag: copy selected text to clipboard
                    this.CopySelectedText();
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
            else if (CurArgs.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DispatchMessage(this, Clipboard.GetText());
            }
            
            base.OnMouseDown (CurArgs);
        }

        // Handle Keyboard Events:
        // These keys don't normally throw an OnKeyDown event. Returning true here fixes this.
        protected override Boolean IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Shift:
                // FIXME: still working on supporting the rest of the keyboard...
                case Keys.RWin:
                case Keys.LWin:
                    return true;

                default:
                    return base.IsInputKey(keyData);
            }
        }

        #endregion

        ////#region Private Event Methods

        ////private void mnuCopy_Click (object sender, System.EventArgs e)
        ////{
        ////    this.CopySelectedText();
        ////}

        ////private void mnuPaste_Click(object sender, System.EventArgs e)
        ////{
        ////    this.PasteClipboardText();
        ////}

        ////private void mnuCopyPaste_Click(object sender, System.EventArgs e)
        ////{
        ////    this.CopySelectedText();
        ////    this.PasteClipboardText();
        ////}

        ////#endregion

        #region Private Methods by developer
        
        private void CopySelectedText()
        {
            Point start = new Point();
            Point stop  = new Point();
            Boolean FoundStart = false;
            Boolean FoundStop = false;

            // find coordinates of Highlighted Text
            for (Int32 row = 0; row < this._rows; row++)
            {
                for (Int32 col = 0; col < this._cols; col++)
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
                            {
                                row--;
                            }

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
                    for (Int32 col = 0; col < this._cols; col++) // parse the row
                    {
                        if (this.CharGrid[row][col] == '\0') // we found the end
                        {
                            stop.X = col - 1;
                            stop.Y = row;
                        }
                    }
                }
            } // row parse

            ////Console.WriteLine("start.Y " + Convert.ToString(start.Y) +
            ////                 " start.X " + Convert.ToString(start.X) +
            ////                 " stop.Y "  + Convert.ToString(stop.Y)  +
            ////                 " stop.X "  + Convert.ToString(stop.X));

            StringCollection sc = this.ScreenScrape (start.Y, start.X, stop.Y, stop.X);

            if (sc != null && sc.Count > 0)
            {
                string[] lines = new string[sc.Count];
                sc.CopyTo(lines, 0);
                try
                {
                    Clipboard.SetDataObject(string.Join("\n", lines),false,5,10);
                }
                catch (Exception)
                {
                    //MessageBox.Show("Copy Error occured: " + err.ToString());
                }
            }
        }

        private void PasteClipboardText()
        {
            try
            {
                DispatchMessage(this, Clipboard.GetText());
            }
            catch (Exception)
            {
                //MessageBox.Show("Paste Error occured: " + err.ToString());
            }
        }

        /// <summary>
        /// Reset the text selection highlight.
        /// </summary>
        private void ResetSelectedTextHighlight(Boolean refresh = true)
        {
            for (Int32 iRow = 0; iRow < this._rows; iRow++)
            {
                for (Int32 iCol = 0; iCol < this._cols; iCol++)
                {
                    this.AttribGrid[iRow][iCol].IsInverse = false;
                }
            }

            if (refresh)
                this.Refresh();
        }

        private void HandleScroll(Object sender, ScrollEventArgs se)
        {
            // capture text at cursor
            if (this.Caret.IsOff)
            {
            }
            else
            {
                this.TextAtCursor = String.Empty;
                for (Int32 x = 0; x < this._cols; x++)
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

            Int32 columns = this._cols;
            Int32 rows = this._rows;
            
            this.SetSize(rows, columns);

            StringCollection visiblebuffer = new StringCollection();
            for (Int32 i = this.ScrollbackBuffer.Count - 1 + this.LastVisibleLine; i >= 0; i--)
            {
                visiblebuffer.Insert(0, this.ScrollbackBuffer[i]);

                // don't parse more strings than our display can show
                if (visiblebuffer.Count >= rows - 1) // rows -1 to leave line for cursor space
                    break;
            }

            ////Int32 lastline = (0 - this.ScrollbackBuffer.Count) + (this.Rows);
            ////Int32 lastline = this.LastVisibleLine;
            for (Int32 i = 0; i < visiblebuffer.Count; i++)
            {
                // Console.WriteLine("Writing string to display: " + visiblebuffer[i]);
                for (Int32 column = 0; column < columns; column++)
                {
                    // this.CharGrid[i][column] = '0';
                    if (column > visiblebuffer[i].Length - 1)
                        continue;

                    this.CharGrid[i][column] = visiblebuffer[i].ToCharArray()[column];

                }

                // if we're displaying the last line in scrollbackbuffer, then
                // replace the cursor and the text on the cursor line
                // System.Console.WriteLine(Convert.ToString(lastline) + " " + Convert.ToString(this.ScrollbackBuffer.Count));
                if (this.LastVisibleLine == 0)
                {
                    this.CaretOn();
                    // this.CaretToAbs(0,0);
                    for (Int32 column = 0; column < this._cols; column++)
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
            ////if(this.hScrollBar1.Visible)
            ////{
            ////	this.vScrollBar1.Maximum += this.hScrollBar1.Height;
            ////}
            this.VertScrollBar.LargeChange = this.VertScrollBar.Maximum / this.CharSize.Height * 10;
            this.VertScrollBar.SmallChange = this.VertScrollBar.Maximum / this.CharSize.Height;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.VertScrollBar.Maximum += this.VertScrollBar.LargeChange;
        }

        string keyboardBuffer = String.Empty;
        System.Collections.Generic.List<string> history = new System.Collections.Generic.List<string>();
        
        private void SetSize (Int32 Rows, Int32 Columns)
        {
            this._rows = Rows;
            this._cols = Columns;

            this.TopMargin    = 0;
            this.BottomMargin = Rows - 1;

            ////this.ClientSize = new System.Drawing.Size (
            ////	System.Convert.ToInt32 (this.CharSize.Width  * this.Columns + 2) + this.VertScrollBar.Width,
            ////	System.Convert.ToInt32 (this.CharSize.Height * this.Rows    + 2));

            // create the character grid (rows by columns) this is a shadow of what's displayed
            this.CharGrid = new System.Char[Rows][];

            this.Caret.Pos.X = 0;
            this.Caret.Pos.Y = 0;

            for (Int32 i = 0; i < this.CharGrid.Length; i++)
            {
                this.CharGrid[i]   = new System.Char[Columns];
            }

            this.AttribGrid = new CharAttribStruct[Rows][];

            for (Int32 i = 0; i < this.AttribGrid.Length; i++)
            {
                this.AttribGrid[i]   = new CharAttribStruct[Columns];
            }
        }

        private void GetFontInfo ()
        {
            Graphics tmpGraphics = this.CreateGraphics ();

            // get the offset that the moron Graphics.Drawstring method adds by default
            this.DrawStringOffset = this.GetDrawStringOffset (tmpGraphics, 0, 0, 'A');

            // get the size of the character using the same type of method
            System.Drawing.Point tmpPoint = this.GetCharSize (tmpGraphics);

            this.CharSize.Width  = tmpPoint.X; // make a little breathing room
            this.CharSize.Height = tmpPoint.Y;
            
            ////Graphics g = this.CreateGraphics();
            ////SizeF size = g.MeasureString("_", this.Font);
            ////this.CharSize.Width = (int) size.Width;
            ////this.CharSize.Height = (int) size.Height;

            tmpGraphics.Dispose ();
            this.UnderlinePos = this.CharSize.Height - 2;

            UpdateCaret();
            this.EraseBitmap     =  new Bitmap (this.CharSize.Width, this.CharSize.Height);
            this.EraseBuffer     =  Graphics.FromImage (this.EraseBitmap);
        }

        private void UpdateCaret()
        {
            if (this.CharSize.Width > 0 && this.CharSize.Height > 0)
            {
                this.Caret.Bitmap = new Bitmap(this.CharSize.Width, this.CharSize.Height);
                this.Caret.Buffer = Graphics.FromImage(this.Caret.Bitmap);
                this.Caret.Buffer.Clear(this.BlinkColor);
            }
        }

        private void OnClickFont (Object Sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog ();

            fontDialog.FixedPitchOnly = true;
            fontDialog.ShowEffects    = false;
            fontDialog.Font           = this.Font;

            if (fontDialog.ShowDialog () != DialogResult.Cancel)
            {
                // Change the font
                this.Font = fontDialog.Font;

                this.GetFontInfo ();

                this.ClientSize = new Size (
                    Convert.ToInt32 (this.CharSize.Width  * this._cols + 2) + this.VertScrollBar.Width,
                    Convert.ToInt32 (this.CharSize.Height * this._rows    + 2));
            }
        }

        #endregion

        #region Private Classes

        private class uc_VertScrollBar : System.Windows.Forms.VScrollBar
        {
            public uc_VertScrollBar()
            {
                this.SetStyle(System.Windows.Forms.ControlStyles.Selectable, false);
                this.Maximum = 0;
            }
        }

        #endregion
    }
}