using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        #region Private Classes
        
        private class uc_TabStops
        {
            public Boolean[] Columns;

            public uc_TabStops()
            {
                this.Columns = new Boolean[256];

                this.Columns[8] = true;
                this.Columns[16] = true;
                this.Columns[24] = true;
                this.Columns[32] = true;
                this.Columns[40] = true;
                this.Columns[48] = true;
                this.Columns[56] = true;
                this.Columns[64] = true;
                this.Columns[72] = true;
                this.Columns[80] = true;
                this.Columns[88] = true;
                this.Columns[96] = true;
                this.Columns[104] = true;
                this.Columns[112] = true;
                this.Columns[120] = true;
                this.Columns[128] = true;
            }
        }

        private class uc_CaretAttribs
        {
            public Point Pos;
            public uc_Chars.Sets G0Set;
            public uc_Chars.Sets G1Set;
            public uc_Chars.Sets G2Set;
            public uc_Chars.Sets G3Set;
            public CharAttribStruct Attribs;

            public uc_CaretAttribs(
                Point p1,
                uc_Chars.Sets p2,
                uc_Chars.Sets p3,
                uc_Chars.Sets p4,
                uc_Chars.Sets p5,
                CharAttribStruct p6)
            {
                this.Pos = p1;
                this.G0Set = p2;
                this.G1Set = p3;
                this.G2Set = p4;
                this.G3Set = p5;
                this.Attribs = p6;
            }
        }

        private class uc_Caret
        {
            public Point Pos;
            public Color Color = Color.FromArgb(255, 181, 106);
            public Bitmap Bitmap = null;
            public Graphics Buffer = null;
            public Boolean IsOff = false;
            public Boolean EOL = false;

            public uc_Caret()
            {
                this.Pos = new Point(0, 0);
            }
        }

        #endregion

        #region Private Methods
        
        private void CarriageReturn()
        {
            this.CaretToAbs(this.Caret.Pos.Y, 0);
        }

        private void Tab()
        {
            for (Int32 i = 0; i < this.TabStops.Columns.Length; i++)
            {
                if (i > this.Caret.Pos.X && this.TabStops.Columns[i] == true)
                {
                    this.CaretToAbs(this.Caret.Pos.Y, i);
                    return;
                }
            }

            this.CaretToAbs(this.Caret.Pos.Y, this._cols - 1);
            return;
        }

        private void TabSet()
        {
            this.TabStops.Columns[this.Caret.Pos.X] = true;
        }

        private void ClearTabs(uc_Params CurParams) // TBC 
        {
            Int32 Param = 0;

            if (CurParams.Count() > 0)
            {
                Param = Convert.ToInt32(CurParams.Elements[0]);
            }

            switch (Param)
            {
                case 0: // Current Position
                    this.TabStops.Columns[this.Caret.Pos.X] = false;
                    break;

                case 3: // All Tabs
                    for (Int32 i = 0; i < this.TabStops.Columns.Length; i++)
                    {
                        this.TabStops.Columns[i] = false;
                    }

                    break;

                default:
                    break;
            }
        }

        private void ReverseLineFeed()
        {
            // if we're at the top of the scroll region (top margin)
            if (this.Caret.Pos.Y == this.TopMargin)
            {
                // we need to add a new line at the top of the screen margin
                // so shift all the rows in the scroll region down in the array and
                // insert a new row at the top
                for (Int32 i = this.BottomMargin; i > this.TopMargin; i--)
                {
                    this.CharGrid[i] = this.CharGrid[i - 1];
                    this.AttribGrid[i] = this.AttribGrid[i - 1];
                }

                this.CharGrid[this.TopMargin] = new Char[this._cols];
                this.AttribGrid[this.TopMargin] = new CharAttribStruct[this._cols];
            }

            this.CaretUp();
        }

        private void InsertLine(uc_Params CurParams)
        {
            // if we're not in the scroll region then bail
            if (this.Caret.Pos.Y < this.TopMargin || this.Caret.Pos.Y > this.BottomMargin)
            {
                return;
            }

            Int32 NbrOff = 1;
            if (CurParams.Count() > 0)
            {
                NbrOff = Convert.ToInt32(CurParams.Elements[0]);
            }

            while (NbrOff > 0)
            {
                // Shift all the rows from the current row to the bottom margin down one place
                for (Int32 i = this.BottomMargin; i > this.Caret.Pos.Y; i--)
                {
                    this.CharGrid[i] = this.CharGrid[i - 1];
                    this.AttribGrid[i] = this.AttribGrid[i - 1];
                }

                this.CharGrid[this.Caret.Pos.Y] = new Char[this._cols];
                this.AttribGrid[this.Caret.Pos.Y] = new CharAttribStruct[this._cols];

                NbrOff--;
            }
        }

        private void DeleteLine(uc_Params CurParams)
        {
            // if we're not in the scroll region then bail
            if (this.Caret.Pos.Y < this.TopMargin || this.Caret.Pos.Y > this.BottomMargin)
            {
                return;
            }

            Int32 NbrOff = 1;
            if (CurParams.Count() > 0)
            {
                NbrOff = Convert.ToInt32(CurParams.Elements[0]);
            }

            while (NbrOff > 0)
            {
                // Shift all the rows from below the current row to the bottom margin up one place
                for (Int32 i = this.Caret.Pos.Y; i < this.BottomMargin; i++)
                {
                    this.CharGrid[i] = this.CharGrid[i + 1];
                    this.AttribGrid[i] = this.AttribGrid[i + 1];
                }

                this.CharGrid[this.BottomMargin] = new Char[this._cols];
                this.AttribGrid[this.BottomMargin] = new CharAttribStruct[this._cols];

                NbrOff--;
            }
        }

        private void LineFeed()
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

            String s = String.Empty;
            for (Int32 x = 0; x < this._cols; x++)
            {
                Char CurChar = this.CharGrid[this.Caret.Pos.Y][x];

                if (CurChar == '\0')
                {
                    continue;
                }

                s = s + Convert.ToString(CurChar);
            }

            this.ScrollbackBuffer.Add(s);
            ////Console.WriteLine("there are " + Convert.ToString(this.ScrollbackBuffer.Count) + " lines in the scrollback buffer");
            ////Console.WriteLine(s);

            if (this.Caret.Pos.Y == this.BottomMargin || this.Caret.Pos.Y == this._rows - 1)
            {
                // we need to add a new line so shift all the rows up in the array and
                // insert a new row at the bottom
                Int32 i;
                for (i = this.TopMargin; i < this.BottomMargin; i++)
                {
                    this.CharGrid[i] = this.CharGrid[i + 1];
                    this.AttribGrid[i] = this.AttribGrid[i + 1];
                }

                this.CharGrid[i] = new Char[this._cols];
                this.AttribGrid[i] = new CharAttribStruct[this._cols];
            }

            this.CaretDown();
        }

        private void Index(Int32 Param)
        {
            if (Param == 0)
                Param = 1;

            for (Int32 i = 0; i < Param; i++)
            {
                this.LineFeed();
            }
        }

        private void ReverseIndex(Int32 Param)
        {
            if (Param == 0)
                Param = 1;

            for (Int32 i = 0; i < Param; i++)
            {
                this.ReverseLineFeed();
            }
        }

        private void CaretOff()
        {
            if (this.Caret.IsOff == true)
            {
                return;
            }

            this.Caret.IsOff = true;
        }

        private void CaretOn()
        {
            if (this.Caret.IsOff == false)
            {
                return;
            }

            this.Caret.IsOff = false;
        }

        private void ShowCaret(Graphics CurGraphics)
        {
            Int32 X = this.Caret.Pos.X;
            Int32 Y = this.Caret.Pos.Y;

            if (this.Caret.IsOff == true)
            {
                return;
            }

            // paint a rectangle over the cursor position
            CurGraphics.DrawImageUnscaled(this.Caret.Bitmap, X * (Int32)this.CharSize.Width, Y * (Int32)this.CharSize.Height);

            // if we don't have a char to redraw then leave
            if (this.CharGrid[Y][X] == '\0')
            {
                return;
            }

            CharAttribStruct CurAttribs = new CharAttribStruct();
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
            CurAttribs.IsDECSG = this.AttribGrid[Y][X].IsDECSG;

            // redispay the current char in the background colour
            this.ShowChar(
                CurGraphics, 
                this.CharGrid[Y][X], 
                Caret.Pos.Y * this.CharSize.Height,
                Caret.Pos.X * this.CharSize.Width,
                CurAttribs);
        }

        private void CaretUp()
        {
            this.Caret.EOL = false;

            if ((this.Caret.Pos.Y > 0 && (this.Modes.Flags & uc_Mode.OriginRelative) == 0) ||
                (this.Caret.Pos.Y > this.TopMargin && (this.Modes.Flags & uc_Mode.OriginRelative) > 0))
            {
                this.Caret.Pos.Y -= 1;
            }
        }

        private void CaretDown()
        {
            this.Caret.EOL = false;

            if ((this.Caret.Pos.Y < this._rows - 1 && (this.Modes.Flags & uc_Mode.OriginRelative) == 0) ||
                (this.Caret.Pos.Y < this.BottomMargin && (this.Modes.Flags & uc_Mode.OriginRelative) > 0))
            {
                this.Caret.Pos.Y += 1;
            }
        }

        private void CaretLeft()
        {
            this.Caret.EOL = false;

            if (this.Caret.Pos.X > 0)
            {
                this.Caret.Pos.X -= 1;
            }
        }

        private void CaretRight()
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

        private void CaretToRel(Int32 Y, Int32 X)
        {
            this.Caret.EOL = false;

             // This code is used when we get a cursor position command from
             // the host. Even if we're not in relative mode we use this as this will
             // sort that out for us. The ToAbs code is used internally by this prog 
             // but is smart enough to stay within the margins if the originrelative 
             // flagis set.
            if ((this.Modes.Flags & uc_Mode.OriginRelative) == 0)
            {
                this.CaretToAbs(Y, X);
                return;
            }

            // the origin mode is relative so add the top and left margin
            // to Y and X respectively
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

        private void CaretToAbs(Int32 Y, Int32 X)
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

        #endregion
    }
}
