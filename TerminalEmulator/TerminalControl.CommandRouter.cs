using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        private void CommandRouter(Object Sender, ParserEventArgs e)
        {
            switch (e.Action)
            {
                case Actions.Print:
                    this.PrintChar(e.CurChar);
                    ////Console.Write ("{0}", e.CurChar);
                    break;

                case Actions.Execute:
                    this.ExecuteChar(e.CurChar);
                    break;

                case Actions.Dispatch:
                    break;

                default:
                    break;
            }

            Int32 Param = 0;
            Int32 Inc = 1; // increment

            switch (e.CurSequence)
            {
                case "":
                    break;

                case "\x1b" + "7": // DECSC Save Cursor position and attributes
                    this.SavedCarets.Add(new uc_CaretAttribs(
                        this.Caret.Pos,
                        this.G0.Set,
                        this.G1.Set,
                        this.G2.Set,
                        this.G3.Set,
                        this.CharAttribs));

                    break;

                case "\x1b" + "8": // DECRC Restore Cursor position and attributes
                    this.Caret.Pos = ((uc_CaretAttribs)this.SavedCarets[this.SavedCarets.Count - 1]).Pos;
                    this.CharAttribs = ((uc_CaretAttribs)this.SavedCarets[this.SavedCarets.Count - 1]).Attribs;

                    this.G0.Set = ((uc_CaretAttribs)this.SavedCarets[this.SavedCarets.Count - 1]).G0Set;
                    this.G1.Set = ((uc_CaretAttribs)this.SavedCarets[this.SavedCarets.Count - 1]).G1Set;
                    this.G2.Set = ((uc_CaretAttribs)this.SavedCarets[this.SavedCarets.Count - 1]).G2Set;
                    this.G3.Set = ((uc_CaretAttribs)this.SavedCarets[this.SavedCarets.Count - 1]).G3Set;

                    this.SavedCarets.RemoveAt(this.SavedCarets.Count - 1);

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
                    e.CurParams.Elements.Add("1");
                    e.CurParams.Elements.Add(this._rows.ToString());
                    this.SetScrollRegion(e.CurParams);

                    // put E's on the entire screen
                    for (int y = 0; y < this._rows; y++)
                    {
                        this.CaretToAbs(y, 0);
                        for (int x = 0; x < this._cols; x++)
                        {
                            this.PrintChar('E');
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

                    if (e.CurParams.Count() > 0)
                    {
                        Inc = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (Inc == 0)
                        Inc = 1;

                    this.CaretToAbs(this.Caret.Pos.Y + Inc, this.Caret.Pos.X);
                    break;

                case "\x1b[A": // CUU

                    if (e.CurParams.Count() > 0)
                    {
                        Inc = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (Inc == 0)
                        Inc = 1;

                    this.CaretToAbs(this.Caret.Pos.Y - Inc, this.Caret.Pos.X);
                    break;

                case "\x1b[C": // CUF

                    if (e.CurParams.Count() > 0)
                    {
                        Inc = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (Inc == 0)
                        Inc = 1;

                    this.CaretToAbs(this.Caret.Pos.Y, this.Caret.Pos.X + Inc);
                    break;

                case "\x1b[D": // CUB

                    if (e.CurParams.Count() > 0)
                    {
                        Inc = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (Inc == 0)
                        Inc = 1;

                    this.CaretToAbs(this.Caret.Pos.Y, this.Caret.Pos.X - Inc);
                    break;

                case "\x1b[H": // CUP
                case "\x1b[f": // HVP

                    Int32 X = 0;
                    Int32 Y = 0;

                    if (e.CurParams.Count() > 0)
                    {
                        Y = Convert.ToInt32(e.CurParams.Elements[0]) - 1;
                    }

                    if (e.CurParams.Count() > 1)
                    {
                        X = Convert.ToInt32(e.CurParams.Elements[1]) - 1;
                    }

                    this.CaretToRel(Y, X);
                    break;

                case "\x1b[J":

                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.ClearDown(Param);
                    break;

                case "\x1b[K":

                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.ClearRight(Param);
                    break;

                case "\x1b[L": // INSERT LINE
                    this.InsertLine(e.CurParams);
                    break;

                case "\x1b[M": // DELETE LINE
                    this.DeleteLine(e.CurParams);
                    break;

                case "\x1bN": // SS2 Single Shift (G2 -> GL)
                    this.CharAttribs.GS = this.G2;
                    break;

                case "\x1bO": // SS3 Single Shift (G3 -> GL)
                    this.CharAttribs.GS = this.G3;
                    // Console.WriteLine ("SS3: GS = {0}", this.CharAttribs.GS);
                    break;

                case "\x1b[m":
                    this.SetCharAttribs(e.CurParams);
                    break;

                case "\x1b[?h":
                    this.SetqmhMode(e.CurParams);
                    break;

                case "\x1b[?l":
                    this.SetqmlMode(e.CurParams);
                    break;

                case "\x1b[c": // DA Device Attributes
                    //                    this.DispatchMessage (this, "\x1b[?64;1;2;6;7;8;9c");
                    this.DispatchMessage(this, "\x1b[?6c");
                    break;

                case "\x1b[g":
                    this.ClearTabs(e.CurParams);
                    break;

                case "\x1b[h":
                    this.SethMode(e.CurParams);
                    break;

                case "\x1b[l":
                    this.SetlMode(e.CurParams);
                    break;

                case "\x1b[r": // DECSTBM Set Top and Bottom Margins
                    this.SetScrollRegion(e.CurParams);
                    break;

                case "\x1b[t": // DECSLPP Set Lines Per Page
                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (Param > 0)
                        this.SetSize(Param, this._cols);
                    break;

                case "\x1b" + "D": // IND

                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.Index(Param);
                    break;

                case "\x1b" + "E": // NEL
                    this.LineFeed();
                    this.CarriageReturn();
                    break;

                case "\x1bH": // HTS
                    this.TabSet();
                    break;

                case "\x1bM": // RI
                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.ReverseIndex(Param);
                    break;

                default:
                    // Console.Write ("unsupported VT sequence {0} happened\n", e.CurSequence);
                    break;
            }

            if (e.CurSequence.StartsWith("\x1b("))
            {
                this.SelectCharSet(ref this.G0.Set, e.CurSequence.Substring(2));
            }
            else if (e.CurSequence.StartsWith("\x1b-") ||
                e.CurSequence.StartsWith("\x1b)"))
            {
                this.SelectCharSet(ref this.G1.Set, e.CurSequence.Substring(2));
            }
            else if (e.CurSequence.StartsWith("\x1b.") ||
                e.CurSequence.StartsWith("\x1b*"))
            {
                this.SelectCharSet(ref this.G2.Set, e.CurSequence.Substring(2));
            }
            else if (e.CurSequence.StartsWith("\x1b/") ||
                e.CurSequence.StartsWith("\x1b+"))
            {
                this.SelectCharSet(ref this.G3.Set, e.CurSequence.Substring(2));
            }
        }

        private void SelectCharSet(ref uc_Chars.Sets CurTarget, String CurString)
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

        private void SetqmhMode(uc_Params CurParams) // set mode for ESC?h command
        {
            Int32 OptInt = 0;

            foreach (String CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = Convert.ToInt32(CurOption);
                }
                catch (Exception CurException)
                {
                    // Console.WriteLine (CurException.Message);
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
                        this.SetSize(this._rows, 132);
                        break;

                    case 5: // Light Background Mode
                        this.Modes.Flags = this.Modes.Flags | uc_Mode.LightBackground;
                        this.RefreshEvent();
                        break;

                    case 6: // Origin Mode Relative
                        this.Modes.Flags = this.Modes.Flags | uc_Mode.OriginRelative;
                        this.CaretToRel(0, 0);
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

        private void SetqmlMode(uc_Params CurParams) // set mode for ESC?l command
        {
            Int32 OptInt = 0;

            foreach (String CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = Convert.ToInt32(CurOption);
                }
                catch (Exception CurException)
                {
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
                        this.SetSize(this._rows, 80);
                        break;

                    case 5: // Dark Background Mode
                        this.Modes.Flags = this.Modes.Flags & ~uc_Mode.LightBackground;
                        this.RefreshEvent();
                        break;

                    case 6: // Origin Mode Absolute
                        this.Modes.Flags = this.Modes.Flags & ~uc_Mode.OriginRelative;
                        this.CaretToAbs(0, 0);
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

        private void SethMode(uc_Params CurParams) // set mode for ESC?h command
        {
            Int32 OptInt = 0;

            foreach (String CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = Convert.ToInt32(CurOption);
                }
                catch (Exception CurException)
                {
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

        private void SetlMode(uc_Params CurParams) // set mode for ESC?l command
        {
            Int32 OptInt = 0;

            foreach (String CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = Convert.ToInt32(CurOption);
                }
                catch (Exception CurException)
                {
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

        private void SetScrollRegion(uc_Params CurParams)
        {
            if (CurParams.Count() > 0)
            {
                this.TopMargin = Convert.ToInt32(CurParams.Elements[0]) - 1;
            }

            if (CurParams.Count() > 1)
            {
                this.BottomMargin = Convert.ToInt32(CurParams.Elements[1]) - 1;
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

        private void ClearCharAttribs()
        {
            this.CharAttribs.IsBold = false;
            this.CharAttribs.IsDim = false;
            this.CharAttribs.IsUnderscored = false;
            this.CharAttribs.IsBlinking = false;
            this.CharAttribs.IsInverse = false;
            this.CharAttribs.IsPrimaryFont = false;
            this.CharAttribs.IsAlternateFont = false;
            this.CharAttribs.UseAltBGColor = false;
            this.CharAttribs.UseAltColor = false;
            this.CharAttribs.AltColor = Color.White;
            this.CharAttribs.AltBGColor = Color.Black;
        }

        private void SetCharAttribs(uc_Params CurParams)
        {
            if (CurParams.Count() < 1)
            {
                this.ClearCharAttribs();
                return;
            }

            for (int i = 0; i < CurParams.Count(); i++)
            {
                switch (Convert.ToInt32(CurParams.Elements[i]))
                {
                    case 0:
                        this.ClearCharAttribs();
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
                        this.CharAttribs.AltColor = Color.Black;
                        break;

                    case 31:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.Red;
                        break;

                    case 32:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.Green;
                        break;

                    case 33:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.Yellow;
                        break;

                    case 34:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.Blue;
                        break;

                    case 35:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.Magenta;
                        break;

                    case 36:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.Cyan;
                        break;

                    case 37:
                        this.CharAttribs.UseAltColor = true;
                        this.CharAttribs.AltColor = Color.White;
                        break;

                    case 40:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Black;
                        break;

                    case 41:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Red;
                        break;

                    case 42:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Green;
                        break;

                    case 43:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Yellow;
                        break;

                    case 44:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Blue;
                        break;

                    case 45:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Magenta;
                        break;

                    case 46:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.Cyan;
                        break;

                    case 47:
                        this.CharAttribs.UseAltBGColor = true;
                        this.CharAttribs.AltBGColor = Color.White;
                        break;

                    default:
                        break;
                }
            }
        }

        private void ExecuteChar(Char CurChar)
        {
            switch (CurChar)
            {
                case '\x05': // ENQ request for the answerback message
                    this.DispatchMessage(this, TerminalType);
                    break;

                case '\x07': // BEL ring my bell
                    // this.BELL;
                    break;

                case '\x08': // BS back space
                    this.CaretLeft();
                    break;

                case '\x09': // HT Horizontal Tab
                    this.Tab();
                    break;

                case '\x0A': // LF  LineFeed
                case '\x0B': // VT  VerticalTab
                case '\x0C': // FF  FormFeed
                case '\x84': // IND Index
                    this.LineFeed();
                    break;

                case '\x0D': // CR CarriageReturn
                    this.CarriageReturn();
                    break;

                case '\x0E': // SO maps G1 into GL
                    this.CharAttribs.GL = this.G1;
                    break;

                case '\x0F': // SI maps G0 into GL
                    this.CharAttribs.GL = this.G0;
                    break;

                case '\x11': // DC1/XON continue sending characters
                    this.XOFF = false;
                    this.DispatchMessage(this, String.Empty);
                    break;

                case '\x13': // DC3/XOFF stop sending characters
                    this.XOFF = true;
                    break;

                case '\x85': // NEL Next line (same as line feed and carriage return)
                    this.LineFeed();
                    this.CaretToAbs(this.Caret.Pos.Y, 0);
                    break;

                case '\x88': // HTS Horizontal tab set 
                    this.TabSet();
                    break;

                case '\x8D': // RI Reverse Index 
                    this.ReverseLineFeed();
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
    }
}
