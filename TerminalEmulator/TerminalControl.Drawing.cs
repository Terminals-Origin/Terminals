using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        private void PrintChar(System.Char CurChar)
        {
            if (this.Caret.EOL == true)
            {
                if ((this.Modes.Flags & uc_Mode.AutoWrap) == uc_Mode.AutoWrap)
                {
                    this.LineFeed();
                    this.CarriageReturn();
                    this.Caret.EOL = false;
                }
            }

            Int32 X = this.Caret.Pos.X;
            Int32 Y = this.Caret.Pos.Y;

            this.AttribGrid[Y][X] = this.CharAttribs;

            if (this.CharAttribs.GS != null)
            {
                CurChar = uc_Chars.Get(CurChar, this.AttribGrid[Y][X].GS.Set, this.AttribGrid[Y][X].GR.Set);

                if (this.CharAttribs.GS.Set == uc_Chars.Sets.DECSG)
                    this.AttribGrid[Y][X].IsDECSG = true;

                this.CharAttribs.GS = null;
            }
            else
            {
                CurChar = uc_Chars.Get(CurChar, this.AttribGrid[Y][X].GL.Set, this.AttribGrid[Y][X].GR.Set);

                if (this.CharAttribs.GL.Set == uc_Chars.Sets.DECSG) this.AttribGrid[Y][X].IsDECSG = true;
            }

            this.CharGrid[Y][X] = CurChar;
            this.CaretRight();
        }

        private Point GetDrawStringOffset(Graphics CurGraphics, Int32 X, Int32 Y, Char CurChar)
        {
            // DrawString doesn't actually print where you tell it to but instead consistently prints
            // with an offset. This is annoying when the other draw commands do not print with an offset
            // this method returns a point defining the offset so we can take it off the printstring command.

            CharacterRange[] characterRanges = { new CharacterRange(0, 1) };
            RectangleF layoutRect = new RectangleF(X, Y, 100, 100);
            StringFormat stringFormat = new StringFormat();
            stringFormat.SetMeasurableCharacterRanges(characterRanges);
            Region[] stringRegions = new Region[1];

            stringRegions = CurGraphics.MeasureCharacterRanges(
                CurChar.ToString(),
                this.Font,
                layoutRect,
                stringFormat);

            RectangleF measureRect1 = stringRegions[0].GetBounds(CurGraphics);
            return new Point((Int32)(measureRect1.X + 0.5), (Int32)(measureRect1.Y + 0.5));
        }

        private Point GetCharSize(Graphics CurGraphics)
        {
            // DrawString doesn't actually print where you tell it to but instead consistently prints
            // with an offset. This is annoying when the other draw commands do not print with an offset
            // this method returns a point defining the offset so we can take it off the printstring command.

            CharacterRange[] characterRanges = { new CharacterRange(0, 1) };
            RectangleF layoutRect = new RectangleF(0, 0, 100, 100);
            StringFormat stringFormat = new StringFormat();
            stringFormat.SetMeasurableCharacterRanges(characterRanges);
            Region[] stringRegions = new Region[1];

            stringRegions = CurGraphics.MeasureCharacterRanges(
                "A",
                this.Font,
                layoutRect,
                stringFormat);

            RectangleF measureRect1 = stringRegions[0].GetBounds(CurGraphics);
            return new Point((Int32)(measureRect1.Width + 0.5), (Int32)(measureRect1.Height + 0.5));
        }

        private void AssignColors(CharAttribStruct CurAttribs, ref Color CurFGColor, ref Color CurBGColor)
        {
            CurFGColor = this.ForeColor;
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
                Color TmpColor = CurBGColor;

                CurBGColor = CurFGColor;
                CurFGColor = TmpColor;
            }

            // If light background is on and we're not using alt colors
            // reverse the colors
            if ((this.Modes.Flags & uc_Mode.LightBackground) > 0 &&
                CurAttribs.UseAltColor == false && CurAttribs.UseAltBGColor == false)
            {
                Color TmpColor = CurBGColor;
                CurBGColor = CurFGColor;
                CurFGColor = TmpColor;
            }
        }

        private void ShowChar(Graphics CurGraphics, Char CurChar, Int32 Y, Int32 X, CharAttribStruct CurAttribs)
        {
            if (CurChar == '\0')
            {
                return;
            }

            Color CurFGColor = Color.White;
            Color CurBGColor = Color.Black;

            this.AssignColors(CurAttribs, ref CurFGColor, ref CurBGColor);

            if ((CurBGColor != this.BackColor && (this.Modes.Flags & uc_Mode.LightBackground) == 0) ||
                (CurBGColor != this.ForeColor && (this.Modes.Flags & uc_Mode.LightBackground) > 0))
            {
                // Erase the current Character underneath the cursor postion
                this.EraseBuffer.Clear(CurBGColor);

                // paint a rectangle over the cursor position in the character's BGColor
                CurGraphics.DrawImageUnscaled(this.EraseBitmap, X, Y);
            }

            if (CurAttribs.IsUnderscored)
            {
                CurGraphics.DrawLine(new Pen(CurFGColor, 1), X, Y + this.UnderlinePos, X + this.CharSize.Width, Y + this.UnderlinePos);
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
                this.ShowSpecialChar(CurGraphics, CurChar, Y, X, CurFGColor, CurBGColor);
                return;
            }

            CurGraphics.DrawString(
                CurChar.ToString(),
                this.Font,
                new SolidBrush(CurFGColor),
                X - this.DrawStringOffset.X,
                Y - this.DrawStringOffset.Y);
        }

        private void ShowSpecialChar(Graphics CurGraphics, Char CurChar, Int32 Y, Int32 X, Color CurFGColor, Color CurBGColor)
        {
            if (CurChar == '\0')
            {
                return;
            }

            switch (CurChar)
            {
                case '`': // diamond
                    System.Drawing.Point[] CurPoints = new System.Drawing.Point[4];

                    CurPoints[0] = new Point(X + this.CharSize.Width / 2, Y + this.CharSize.Height / 6);
                    CurPoints[1] = new Point(X + 5 * this.CharSize.Width / 6, Y + this.CharSize.Height / 2);
                    CurPoints[2] = new Point(X + this.CharSize.Width / 2, Y + 5 * this.CharSize.Height / 6);
                    CurPoints[3] = new Point(X + this.CharSize.Width / 6, Y + this.CharSize.Height / 2);

                    CurGraphics.FillPolygon(new SolidBrush(CurFGColor), CurPoints);
                    break;

                case 'l': // top left bracket
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2 - 1, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);
                    break;

                case 'q': // horizontal line
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);
                    break;

                case 'w': // top tee-piece
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);
                    break;

                case 'k': // top right bracket
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);
                    break;

                case 'x': // vertical line
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);
                    break;

                case 't': // left hand tee-piece
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);
                    break;

                case 'n': // cross piece
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);
                    break;

                case 'u': // right hand tee-piece
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height);
                    break;

                case 'm': // bottom left bracket
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2);
                    break;

                case 'v': // bottom tee-piece
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2);
                    break;

                case 'j': // bottom right bracket
                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X, Y + this.CharSize.Height / 2,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2);

                    CurGraphics.DrawLine(new Pen(CurFGColor, 1),
                        X + this.CharSize.Width / 2, Y,
                        X + this.CharSize.Width / 2, Y + this.CharSize.Height / 2);
                    break;

                default:
                    break;
            }
        }

        private void WipeScreen(Graphics CurGraphics)
        {
            // clear the screen buffer area
            if ((this.Modes.Flags & uc_Mode.LightBackground) > 0)
            {
                CurGraphics.Clear(this.ForeColor);
            }
            else
            {
                CurGraphics.Clear(this.BackColor);
            }
        }

        private void ClearDown(Int32 Param)
        {
            Int32 caretCharIndex = this.Caret.Pos.X;
            Int32 caretRowIndex = this.Caret.Pos.Y;

            switch (Param)
            {
                case 0: // from cursor to bottom inclusive
                    int charCount = this.CharGrid[caretRowIndex].Length - caretCharIndex;
                    ClearRowChars(caretRowIndex, caretCharIndex, charCount);
                    ClearRows(caretRowIndex + 1, this._rows);
                    break;

                case 1: // from top to cursor inclusive
                    ClearRowChars(caretRowIndex, 0, caretCharIndex + 1);
                    ClearRows(0, caretRowIndex);
                    break;

                case 2: // entire screen
                    ClearRows(0, this._rows);
                    break;
            }
        }

        private void ClearRows(int startRowIndex, int endRowIndex)
        {
            try
            {
                for (Int32 rowIndex = startRowIndex; rowIndex < endRowIndex; rowIndex++)
                {
                    ClearRow(rowIndex);
                    //int scrollBufferRow = this._rows + rowIndex;
                    if (this.ScrollbackBuffer.Count > startRowIndex)
                    {
                        this.ScrollbackBuffer.Characters[startRowIndex] = "";
                        this.ScrollbackBuffer.Attributes[startRowIndex] = new CharAttribStruct[0];
                    }
                }
            }
            catch(Exception exc)
            { 
            }
        }

        private void ClearRow(int rowIndex)
        {
            ClearRowChars(rowIndex, 0, this.CharGrid[rowIndex].Length);
        }

        private void ClearRowChars(int rowIndex, int startCharIndex, int charsCount)
        {
            Array.Clear(this.CharGrid[rowIndex], startCharIndex, charsCount);
            Array.Clear(this.AttribGrid[rowIndex], startCharIndex, charsCount);
        }

        private void ClearRight(Int32 Param)
        {
            Int32 caretCharIndex = this.Caret.Pos.X;
            Int32 caretRowIndex = this.Caret.Pos.Y;

            switch (Param)
            {
                case 0: // from cursor to end of line inclusive
                    int charsCount = this.CharGrid[caretRowIndex].Length - caretCharIndex;
                    ClearRowChars(caretRowIndex, caretCharIndex, charsCount);
                    break;

                case 1: // from beginning to cursor inclusive
                    ClearRowChars(caretRowIndex, 0, caretCharIndex + 1);
                    break;

                case 2: // entire line
                    ClearRowChars(caretRowIndex, 0, this.CharGrid[caretRowIndex].Length);
                    break;

                default:
                    break;
            }
        }

        private void ShowBuffer()
        {
            this.Invalidate();
        }

        private void Redraw(Graphics CurGraphics)
        {
            Point CurPoint;
            Char CurChar;

            // refresh the screen
            for (Int32 Y = 0; Y < this._rows; Y++)
            {
                for (Int32 X = 0; X < this._cols; X++)
                {
                    CurChar = this.CharGrid[Y][X];
                    if (CurChar == '\0')
                    {
                        continue;
                    }

                    CurPoint = new Point(X * this.CharSize.Width, Y * this.CharSize.Height);
                    this.ShowChar(CurGraphics, CurChar, CurPoint.Y, CurPoint.X, this.AttribGrid[Y][X]);
                }
            }
        }

        private struct CharAttribStruct
        {
            public Boolean IsBold;
            public Boolean IsDim;
            public Boolean IsUnderscored;
            public Boolean IsBlinking;
            public Boolean IsInverse;
            public Boolean IsPrimaryFont;
            public Boolean IsAlternateFont;
            public Boolean UseAltColor;
            public Color AltColor;
            public Boolean UseAltBGColor;
            public Color AltBGColor;
            public uc_Chars GL;
            public uc_Chars GR;
            public uc_Chars GS;
            public Boolean IsDECSG;

            public CharAttribStruct(
                Boolean p1,
                Boolean p2,
                Boolean p3,
                Boolean p4,
                Boolean p5,
                Boolean p6,
                Boolean p7,
                Boolean p12,
                Color p13,
                Boolean p14,
                Color p15,
                uc_Chars p16,
                uc_Chars p17,
                uc_Chars p18,
                Boolean p19)
            {
                this.IsBold = p1;
                this.IsDim = p2;
                this.IsUnderscored = p3;
                this.IsBlinking = p4;
                this.IsInverse = p5;
                this.IsPrimaryFont = p6;
                this.IsAlternateFont = p7;
                this.UseAltColor = p12;
                this.AltColor = p13;
                this.UseAltBGColor = p14;
                this.AltBGColor = p15;
                this.GL = p16;
                this.GR = p17;
                this.GS = p18;
                this.IsDECSG = p19;
            }
        }
    }
}
