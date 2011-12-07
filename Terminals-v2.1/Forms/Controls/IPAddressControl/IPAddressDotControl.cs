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
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    internal class IPAddressDotControl : System.Windows.Forms.Control
    {
        #region Fields

        private bool _backColorChanged;
        private bool _readOnly;

        private StringFormat _stringFormat;
        private SizeF _sizeText;

        #endregion

        #region Constructors

        public IPAddressDotControl()
        {
            Text = IPAddressControl.FieldSeparator;

            _stringFormat = StringFormat.GenericTypographic;
            _stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            BackColor = SystemColors.Window;
            Size = MinimumSize;
            TabStop = false;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            SetStyle(ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.FixedWidth, true);
        }

        #endregion // Constructors

        #region Public Properties

        public override Size MinimumSize
        {
            get
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                {
                    _sizeText = g.MeasureString(Text, Font, -1, _stringFormat);
                }

                // MeasureString() cuts off the bottom pixel for descenders no matter
                // which StringFormatFlags are chosen.  This doesn't matter for '.' but
                // it's here in case someone wants to modify the text.
                //
                _sizeText.Height += 1F;

                return _sizeText.ToSize();
            }
        }

        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                Invalidate();
            }
        }

        #endregion // Public Properties

        #region Public Methods

        public override string ToString()
        {
            return Text;
        }

        #endregion // Public Methods

        #region Protected Methods

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Size = MinimumSize;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color backColor = BackColor;

            if (!_backColorChanged)
            {
                if (!Enabled || ReadOnly)
                    backColor = SystemColors.Control;
            }

            Color textColor = ForeColor;

            if (!Enabled)
                textColor = SystemColors.GrayText;
            else if (ReadOnly)
            {
                if (!_backColorChanged)
                    textColor = SystemColors.WindowText;
            }

            using (SolidBrush backgroundBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
            }

            using (SolidBrush foreBrush = new SolidBrush(textColor))
            {
                float x = (float) ClientRectangle.Width/2F - _sizeText.Width/2F;
                e.Graphics.DrawString(Text, Font, foreBrush,
                                      new RectangleF(x, 0F, _sizeText.Width, _sizeText.Height), _stringFormat);
            }
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            BackColor = Parent.BackColor;
            _backColorChanged = true;
        }

        protected override void OnParentForeColorChanged(EventArgs e)
        {
            base.OnParentForeColorChanged(e);
            ForeColor = Parent.ForeColor;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            base.Size = MinimumSize;
        }

        #endregion // Protected Methods
    }
}
