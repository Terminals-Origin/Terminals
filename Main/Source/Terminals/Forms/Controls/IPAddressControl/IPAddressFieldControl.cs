// Copyright (c) 2007 Michael Chapman

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    internal class IPAddressFieldControl : TextBox
    {
        #region Fields

        private int _fieldIndex = -1;
        private byte _rangeLower; // = MinimumValue;  // this is removed for FxCop approval
        private byte _rangeUpper = MaximumValue;

        private TextFormatFlags _textFormatFlags = TextFormatFlags.HorizontalCenter |
                                                   TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

        #endregion

        #region Constructors

        public IPAddressFieldControl()
        {
            BorderStyle = BorderStyle.None;
            MaxLength = 3;
            Size = MinimumSize;
            TabStop = false;
            TextAlign = HorizontalAlignment.Center;
        }

        #endregion

        #region Public Constants

        public const byte MinimumValue = 0;
        public const byte MaximumValue = 255;

        #endregion // Public Constants

        #region Public Events

        public event EventHandler<CedeFocusEventArgs> CedeFocusEvent;
        public event EventHandler<TextChangedEventArgs> TextChangedEvent;

        #endregion // Public Events

        #region Public Properties

        public bool Blank
        {
            get
            {
                return (TextLength == 0);
            }
        }

        public int FieldIndex
        {
            get
            {
                return _fieldIndex;
            }
            set
            {
                _fieldIndex = value;
            }
        }

        public override Size MinimumSize
        {
            get
            {
                Graphics g = Graphics.FromHwnd(Handle);

                Size minimumSize = TextRenderer.MeasureText(g,
                                                            IPAddressControl.FieldMeasureText, Font, Size,
                                                            _textFormatFlags);

                g.Dispose();

                return minimumSize;
            }
        }

        public byte RangeLower
        {
            get
            {
                return _rangeLower;
            }
            set
            {
                if (value < MinimumValue)
                    _rangeLower = MinimumValue;
                else if (value > _rangeUpper)
                    _rangeLower = _rangeUpper;
                else
                    _rangeLower = value;

                if (Value < _rangeLower)
                    Text = _rangeLower.ToString(CultureInfo.InvariantCulture);
            }
        }

        public byte RangeUpper
        {
            get
            {
                return _rangeUpper;
            }
            set
            {
                if (value < _rangeLower)
                    _rangeUpper = _rangeLower;
                else if (value > MaximumValue)
                    _rangeUpper = MaximumValue;
                else
                    _rangeUpper = value;

                if (Value > _rangeUpper)
                    Text = _rangeUpper.ToString(CultureInfo.InvariantCulture);
            }
        }

        public byte Value
        {
            get
            {
                byte result;

                if (!Byte.TryParse(Text, out result))
                    result = RangeLower;

                return result;
            }
        }

        #endregion // Public Properties

        #region Public Methods

        public void TakeFocus(IPAddressControlAction ipAddressControlAction)
        {
            Focus();

            switch (ipAddressControlAction)
            {
                case IPAddressControlAction.Trim:

                    if (TextLength > 0)
                    {
                        int newLength = TextLength - 1;
                        base.Text = Text.Substring(0, newLength);
                    }

                    SelectionStart = TextLength;

                    return;

                case IPAddressControlAction.Home:

                    SelectionStart = 0;
                    SelectionLength = 0;

                    return;

                case IPAddressControlAction.End:

                    SelectionStart = TextLength;

                    return;
            }
        }

        public void TakeFocus(IPAddressControlDirection ipAddressControlDirection, IPAddressControlSelection ipAddressControlSelection)
        {
            Focus();

            if (ipAddressControlSelection == IPAddressControlSelection.All)
            {
                SelectionStart = 0;
                SelectionLength = TextLength;
            }
            else
                SelectionStart = (ipAddressControlDirection == IPAddressControlDirection.Forward) ? 0 : TextLength;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion // Public Methods

        #region Protected Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Home:
                    SendCedeFocusEvent(IPAddressControlAction.Home);
                    return;

                case Keys.End:
                    SendCedeFocusEvent(IPAddressControlAction.End);
                    return;
            }

            if (IsCedeFocusKey(e))
            {
                SendCedeFocusEvent(IPAddressControlDirection.Forward, IPAddressControlSelection.All);
                e.SuppressKeyPress = true;
                return;
            }
            else if (IsForwardKey(e))
            {
                if (e.Control)
                {
                    SendCedeFocusEvent(IPAddressControlDirection.Forward, IPAddressControlSelection.All);
                    return;
                }
                else if (SelectionLength == 0 && SelectionStart == TextLength)
                {
                    SendCedeFocusEvent(IPAddressControlDirection.Forward, IPAddressControlSelection.None);
                    return;
                }
            }
            else if (IsReverseKey(e))
            {
                if (e.Control)
                {
                    SendCedeFocusEvent(IPAddressControlDirection.Reverse, IPAddressControlSelection.All);
                    return;
                }
                else if (SelectionLength == 0 && SelectionStart == 0)
                {
                    SendCedeFocusEvent(IPAddressControlDirection.Reverse, IPAddressControlSelection.None);
                    return;
                }
            }
            else if (IsBackspaceKey(e))
                HandleBackspaceKey(e);
            else if (!IsNumericKey(e) &&
                     !IsEditKey(e) &&
                     !IsEnterKey(e))
                e.SuppressKeyPress = true;
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            BackColor = Parent.BackColor;
        }

        protected override void OnParentForeColorChanged(EventArgs e)
        {
            base.OnParentForeColorChanged(e);
            ForeColor = Parent.ForeColor;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Size = MinimumSize;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (!Blank)
            {
                int value;
                if (!Int32.TryParse(Text, out value))
                    base.Text = String.Empty;
                else
                {
                    if (value > RangeUpper)
                    {
                        base.Text = RangeUpper.ToString(CultureInfo.InvariantCulture);
                        SelectionStart = 0;
                    }
                    else if ((TextLength == MaxLength) && (value < RangeLower))
                    {
                        base.Text = RangeLower.ToString(CultureInfo.InvariantCulture);
                        SelectionStart = 0;
                    }
                    else
                    {
                        int originalLength = TextLength;
                        int newSelectionStart = SelectionStart;

                        base.Text = value.ToString(CultureInfo.InvariantCulture);

                        if (TextLength < originalLength)
                        {
                            newSelectionStart -= (originalLength - TextLength);
                            SelectionStart = Math.Max(0, newSelectionStart);
                        }
                    }
                }
            }

            if (null != TextChangedEvent)
            {
                TextChangedEventArgs args = new TextChangedEventArgs();
                args.FieldIndex = FieldIndex;
                args.Text = Text;
                TextChangedEvent(this, args);
            }

            if (TextLength == MaxLength && Focused && SelectionStart == TextLength)
                SendCedeFocusEvent(IPAddressControlDirection.Forward, IPAddressControlSelection.All);
        }

        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            base.OnValidating(e);

            if (!Blank)
            {
                if (Value < RangeLower)
                    Text = RangeLower.ToString(CultureInfo.InvariantCulture);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x007b: // WM_CONTEXTMENU
                    return;
            }

            base.WndProc(ref m);
        }

        #endregion // Protected Methods

        #region Private Methods

        private void HandleBackspaceKey(KeyEventArgs e)
        {
            if (!ReadOnly && (TextLength == 0 || (SelectionStart == 0 && SelectionLength == 0)))
            {
                SendCedeFocusEvent(IPAddressControlAction.Trim);
                e.SuppressKeyPress = true;
            }
        }

        private static bool IsBackspaceKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                return true;

            return false;
        }

        private bool IsCedeFocusKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemPeriod ||
                e.KeyCode == Keys.Decimal ||
                e.KeyCode == Keys.Space)
            {
                if (TextLength != 0 && SelectionLength == 0 && SelectionStart != 0)
                    return true;
            }

            return false;
        }

        private static bool IsEditKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back ||
                e.KeyCode == Keys.Delete)
                return true;
            else if (e.Modifiers == Keys.Control &&
                     (e.KeyCode == Keys.C ||
                      e.KeyCode == Keys.V ||
                      e.KeyCode == Keys.X))
                return true;

            return false;
        }

        private static bool IsEnterKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Return)
                return true;

            return false;
        }

        private static bool IsForwardKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Down)
                return true;

            return false;
        }

        private static bool IsNumericKey(KeyEventArgs e)
        {
            if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
            {
                if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
                    return false;
            }

            return true;
        }

        private static bool IsReverseKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Up)
                return true;

            return false;
        }

        private void SendCedeFocusEvent(IPAddressControlAction ipAddressControlAction)
        {
            if (null != CedeFocusEvent)
            {
                CedeFocusEventArgs args = new CedeFocusEventArgs();
                args.FieldIndex = FieldIndex;
                args.IPAddressControlAction = ipAddressControlAction;
                CedeFocusEvent(this, args);
            }
        }

        private void SendCedeFocusEvent(IPAddressControlDirection ipAddressControlDirection, IPAddressControlSelection ipAddressControlSelection)
        {
            if (null != CedeFocusEvent)
            {
                CedeFocusEventArgs args = new CedeFocusEventArgs();
                args.FieldIndex = FieldIndex;
                args.IPAddressControlAction = IPAddressControlAction.None;
                args.IPAddressControlDirection = ipAddressControlDirection;
                args.IPAddressControlSelection = ipAddressControlSelection;
                CedeFocusEvent(this, args);
            }
        }

        #endregion // Private Methods
    }

    internal enum IPAddressControlDirection
    {
        Forward,
        Reverse
    }

    internal enum IPAddressControlSelection
    {
        None,
        All
    }

    internal enum IPAddressControlAction
    {
        None,
        Trim,
        Home,
        End
    }

    internal class CedeFocusEventArgs : EventArgs
    {
        public Int32 FieldIndex { get; set; }

        public IPAddressControlAction IPAddressControlAction { get; set; }

        public IPAddressControlDirection IPAddressControlDirection { get; set; }

        public IPAddressControlSelection IPAddressControlSelection { get; set; }
    }

    internal class TextChangedEventArgs : EventArgs
    {
        public Int32 FieldIndex { get; set; }

        public String Text { get; set; }
    }
}
