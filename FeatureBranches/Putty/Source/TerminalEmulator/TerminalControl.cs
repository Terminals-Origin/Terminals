using System;
using System.Collections.Generic;
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

        //private ContextMenu contextMenu1;    // rightclick menu
       // private MenuItem mnuCopy;
       // private MenuItem mnuPaste;
        //private MenuItem mnuCopyPaste;
        private Point BeginDrag;       // used in Mouse Selecting Text
        private Point EndDrag;         // used in mouse selecting text
        private String TextAtCursor;    // used to store Cursortext while scrolling
        private Int32 LastVisibleLine; // used for scrolling
        private Boolean XOFF = false;
        private String OutBuff = String.Empty;
        private ScrollBackBuffer ScrollbackBuffer;
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
            this.ScrollbackBuffer = new ScrollBackBuffer();

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

            InitializeVerticalScrollBar();

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
            this.MouseWheel += new MouseEventHandler(this.OnMouseWheel);

            this.BeginDrag = new Point();
            this.EndDrag = new Point();

            // Enable autowrap when reaching the end of columnwidth
            this.Modes.Flags = this.Modes.Flags | uc_Mode.AutoWrap;

            this.Cursor = Cursors.IBeam;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            int newValue = CalculateNewScrollBarValue(e);
            ScrollEventType increment = IdentifySrollDirection(e);
            ScroolToPosition(increment, newValue);
        }

        private static ScrollEventType IdentifySrollDirection(MouseEventArgs e)
        {
            if (e.Delta > 0)
                return ScrollEventType.SmallIncrement;
            return ScrollEventType.SmallDecrement;
        }

        private int CalculateNewScrollBarValue(MouseEventArgs e)
        {
            if (e.Delta > 0)
                return this.VertScrollBar.Value + 1;

            return this.VertScrollBar.Value - 1;
        }

        private void InitializeVerticalScrollBar()
        {
            this.VertScrollBar = new uc_VertScrollBar();
            this.VertScrollBar.Scroll += new ScrollEventHandler(this.HandleScroll);
            this.VertScrollBar.Cursor = Cursors.Default;
            this.VertScrollBar.Dock = DockStyle.Right;
            this.VertScrollBar.Enabled = false;

            // Add the scroll bar to the form.
            this.Controls.Add(this.VertScrollBar);
        }

        private void OnFontChanged(object sender, EventArgs e)
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

        public void IndicateData(Byte[] data)
        {
            String sReceived = Encoding.Default.GetString(data, 0, data.Length);
            this.Invoke(this.RxdTextEvent, new String[] { String.Copy(sReceived) });
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
                    Char val = this.CharGrid[r][c]; ;
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

        protected override void OnResize(System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Before: {0}x{1}, Console: {2}x{3}",
                this.Width, this.Height, this.CharSize.Width, this.CharSize.Height));
            // reset scrollbar values);
            this.SetScrollBarValues();
            this.TextAtCursor = CaptureTextAtCursor();

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
                visiblebuffer.Insert(0, this.ScrollbackBuffer.Characters[i]);

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

                this.CharGrid[lastline + 1][column] = TextAtCursor.ToCharArray()[column];
            }

            this.CaretToAbs(lastline + 1, TextAtCursor.Length);
            this.Refresh();

            base.OnResize(e);
            System.Diagnostics.Debug.WriteLine(String.Format("After: {0}x{1}, Console: {2}x{3}",
                this.Width, this.Height, this.CharSize.Width, this.CharSize.Height));
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            e.Graphics.TextContrast = 0;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            this.WipeScreen(e.Graphics);
            this.Redraw(e.Graphics);
            this.ShowCaret(e.Graphics);
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
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
                    ScrollBackToCrate();
                    this.Keyboard.KeyDown(m);
                    break;

                default:
                    // don't do any default handling for the aforementioned events
                    // this means things like keyboard shortcut events are ignored
                    base.WndProc(ref m);
                    break;
            }
        }

        private void ScrollBackToCrate()
        {
            int cratePos = this.ScrollbackBuffer.Count - this._rows + 1;
            if (cratePos > 0)
                ScroolToPosition(ScrollEventType.Last, cratePos);
        }

        private void ScroolToPosition(ScrollEventType scrollEventType, int newValue)
        {
            if (newValue == this.VertScrollBar.Value ||
                newValue <= this.VertScrollBar.Minimum || this.VertScrollBar.Maximum <= newValue)
                return;

            this.VertScrollBar.Value = newValue;
            var scrollEventArgs = new ScrollEventArgs(scrollEventType, this.VertScrollBar.Value, newValue);
            this.HandleScroll(this, scrollEventArgs);
        }

        protected override void OnMouseMove(MouseEventArgs CurArgs)
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

        protected override void OnMouseUp(MouseEventArgs CurArgs)
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

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs CurArgs)
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

            base.OnMouseDown(CurArgs);
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
            Point stop = new Point();
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
                if (FoundStart == true && FoundStop == false && row == this._rows - 1)
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

            StringCollection sc = this.ScreenScrape(start.Y, start.X, stop.Y, stop.X);

            if (sc != null && sc.Count > 0)
            {
                string[] lines = new string[sc.Count];
                sc.CopyTo(lines, 0);
                try
                {
                    Clipboard.SetDataObject(string.Join("\n", lines), false, 5, 10);
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
            if (!this.Caret.IsOff)
                this.TextAtCursor = this.CaptureTextAtCursor();

            if (!UpdateLastVisibleLine(se))
                return;

            ClearScreenChars();
            ScrollBackBuffer visiblebuffer = CreateVisiblebuffer();
            UpdateCurrentCharsFromVisibleBuffer(visiblebuffer);

            this.Refresh();
        }

        private void UpdateCurrentCharsFromVisibleBuffer(ScrollBackBuffer visiblebuffer)
        {
            for (Int32 visibleRowIndex = 0; visibleRowIndex < visiblebuffer.Count; visibleRowIndex++)
            {
                char[] lineChars = visiblebuffer.Characters[visibleRowIndex].ToCharArray();
                for (Int32 column = 0; column < this._cols; column++)
                {
                    if (column > visiblebuffer.Characters[visibleRowIndex].Length - 1)
                        continue;

                    this.CharGrid[visibleRowIndex][column] = lineChars[column];
                    this.AttribGrid[visibleRowIndex][column] = visiblebuffer.Attributes[visibleRowIndex][column];
                }

                this.UpdateCommandLineChars();
            }
        }

        /// <summary>
        /// if we're displaying the last line in scrollbackbuffer, then
        /// replace the cursor and the text on the cursor line
        /// </summary>
        private void UpdateCommandLineChars()
        {
            if (this.LastVisibleLine == 0)
            {
                this.CaretOn();
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

        private ScrollBackBuffer CreateVisiblebuffer()
        {
            ScrollBackBuffer visibleBuffer = new ScrollBackBuffer();

            int lastLineIndex = this.ScrollbackBuffer.Count - 1 + this.LastVisibleLine;
            for (Int32 lineIndex = lastLineIndex; lineIndex >= 0; lineIndex--)
            {
                visibleBuffer.Insert(0, this.ScrollbackBuffer.Characters[lineIndex], this.ScrollbackBuffer.Attributes[lineIndex]);

                // don't parse more strings than our display can show);
                if (visibleBuffer.Count >= this._rows - 1) // rows -1 to leave line for cursor space
                    break;
            }

            return visibleBuffer;
        }

        /// <summary>
        /// Updates last visible line depending on scrollbar step in event arguments.
        /// Returns true, if change is relevant; otherwise false.
        /// </summary>
        private bool UpdateLastVisibleLine(ScrollEventArgs arguments)
        {
            switch (arguments.Type) // calculate step relative increment
            {
                case ScrollEventType.SmallIncrement:
                    UpdateLastVisibleLine(1);
                    return true;
                case ScrollEventType.SmallDecrement:
                    UpdateLastVisibleLine(-1);
                    return true;
                case ScrollEventType.LargeIncrement:
                    UpdateLastVisibleLine(this._rows);
                    return true;
                case ScrollEventType.LargeDecrement:
                    UpdateLastVisibleLine(-this._rows);
                    return true;
                case ScrollEventType.ThumbTrack:
                    UpdateLastVisibleLine(arguments.NewValue - arguments.OldValue);
                    return true;
                case ScrollEventType.First:
                    UpdateLastVisibleLine(0 - this.VertScrollBar.Value);
                    return true;
                case ScrollEventType.Last:
                    UpdateLastVisibleLine(this.VertScrollBar.Maximum - this.VertScrollBar.Value);
                    return true;
                default:
                    return false; // unsuported step
            }
        }

        /// <summary>
        /// make sure we don't set LastVisibleLine past the end of the StringCollection
        /// LastVisibleLine is relative to the last line in the StringCollection
        /// 0 is the Last element
        /// </summary>
        private void UpdateLastVisibleLine(int increment)
        {
            this.LastVisibleLine += increment;

            if (this.LastVisibleLine > 0)
                this.LastVisibleLine = 0;

            if (this.LastVisibleLine < this._rows - this.ScrollbackBuffer.Count)
                this.LastVisibleLine = this._rows - this.ScrollbackBuffer.Count - 1;
        }

        /// <summary>
        /// capture text at cursor because it's not in the scrollback buffer yet
        /// </summary>
        /// <returns>captured text</returns>
        private string CaptureTextAtCursor()
        {
            StringBuilder textAtCursor = new StringBuilder();
            int lineIndex = this.Caret.Pos.Y;
            for (Int32 charIndex = 0; charIndex < this._cols; charIndex++)
            {
                char current = this.CharGrid[lineIndex][charIndex];
                if (current != '\0')
                {
                    textAtCursor.Append(current);
                }
            }

            return textAtCursor.ToString();
        }

        private void SetScrollBarValues()
        {
            // if the scrollbackbuffer is empty, there's nothing to scroll
            if (this.ScrollbackBuffer.Count == 0)
            {
                this.VertScrollBar.Maximum = 0;
                return;
            }

            if (this.ScrollbackBuffer.Count > this._rows)
            {
                this.VertScrollBar.Enabled = true;
                this.VertScrollBar.Maximum = this.ScrollbackBuffer.Count;
                this.VertScrollBar.Value = this.ScrollbackBuffer.Count - this._rows + 1;
            }

            this.VertScrollBar.LargeChange = this._rows;
            this.VertScrollBar.SmallChange = 1;
        }

        private string keyboardBuffer = String.Empty;
        private List<string> history = new List<string>();

        private void SetSize(Int32 Rows, Int32 Columns)
        {
            this._rows = Rows;
            this._cols = Columns;
            this.TopMargin = 0;
            this.BottomMargin = Rows - 1;
            this.Caret.Pos.X = 0;
            this.Caret.Pos.Y = 0;

            
            ClearScreenChars();
        }

        /// <summary>
        /// empty the character grid (rows by columns) this is a shadow of what's displayed
        /// </summary>
        private void ClearScreenChars()
        {
            this.CharGrid = new Char[Rows][];
            this.AttribGrid = new CharAttribStruct[Rows][];

            for (Int32 rowIndex = 0; rowIndex < this.CharGrid.Length; rowIndex++)
            {
                this.CharGrid[rowIndex] = new Char[Columns];
                this.AttribGrid[rowIndex] = new CharAttribStruct[Columns];
            }
        }

        private void GetFontInfo()
        {
            Graphics tmpGraphics = this.CreateGraphics();

            // get the offset that the moron Graphics.Drawstring method adds by default
            this.DrawStringOffset = this.GetDrawStringOffset(tmpGraphics, 0, 0, 'A');

            // get the size of the character using the same type of method
            System.Drawing.Point tmpPoint = this.GetCharSize(tmpGraphics);

            this.CharSize.Width = tmpPoint.X; // make a little breathing room
            this.CharSize.Height = tmpPoint.Y;

            ////Graphics g = this.CreateGraphics();
            ////SizeF size = g.MeasureString("_", this.Font);
            ////this.CharSize.Width = (int) size.Width;
            ////this.CharSize.Height = (int) size.Height;

            tmpGraphics.Dispose();
            this.UnderlinePos = this.CharSize.Height - 2;

            UpdateCaret();
            this.EraseBitmap = new Bitmap(this.CharSize.Width, this.CharSize.Height);
            this.EraseBuffer = Graphics.FromImage(this.EraseBitmap);
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

        private void OnClickFont(Object Sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            fontDialog.FixedPitchOnly = true;
            fontDialog.ShowEffects = false;
            fontDialog.Font = this.Font;

            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                // Change the font
                this.Font = fontDialog.Font;

                this.GetFontInfo();

                this.ClientSize = new Size(
                    Convert.ToInt32(this.CharSize.Width * this._cols + 2) + this.VertScrollBar.Width,
                    Convert.ToInt32(this.CharSize.Height * this._rows + 2));
            }
        }

        #endregion

        #region Private Classes

        private class uc_VertScrollBar : VScrollBar
        {
            public uc_VertScrollBar()
            {
                this.SetStyle(ControlStyles.Selectable, false);
                this.Maximum = 0;
            }
        }

        #endregion
    }
}