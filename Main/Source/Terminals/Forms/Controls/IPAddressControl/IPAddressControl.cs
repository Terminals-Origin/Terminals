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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    [DesignerAttribute(typeof (IPAddressControlDesigner))]
    internal class IPAddressControl : ContainerControl
    {
        #region Fields

        private bool _autoHeight = true;
        private bool _backColorChanged;
        private BorderStyle _borderStyle = BorderStyle.Fixed3D;
        private IPAddressDotControl[] ipAddressDotControls = new IPAddressDotControl[FieldCount - 1];
        private IPAddressFieldControl[] ipAddressFieldControls = new IPAddressFieldControl[FieldCount];
        private bool _focused;
        private bool _hasMouse;
        private bool _readOnly;

        private Size Fixed3DOffset = new Size(3, 3);
        private Size FixedSingleOffset = new Size(2, 2);

        private TextBox _referenceTextBox = new TextBox();

        #endregion

        #region Constructors

        public IPAddressControl()
        {
            BackColor = SystemColors.Window;

            ResetBackColorChanged();

            for (int index = 0; index < ipAddressFieldControls.Length; ++index)
            {
                ipAddressFieldControls[index] = new IPAddressFieldControl();

                ipAddressFieldControls[index].CreateControl();

                ipAddressFieldControls[index].FieldIndex = index;
                ipAddressFieldControls[index].Name = "FieldControl" + index.ToString(CultureInfo.InvariantCulture);
                ipAddressFieldControls[index].Parent = this;

                ipAddressFieldControls[index].CedeFocusEvent += new EventHandler<CedeFocusEventArgs>(OnCedeFocus);
                ipAddressFieldControls[index].Click += new EventHandler(OnSubControlClicked);
                ipAddressFieldControls[index].DoubleClick += new EventHandler(OnSubControlDoubleClicked);
                ipAddressFieldControls[index].GotFocus += new EventHandler(OnFieldGotFocus);
                ipAddressFieldControls[index].KeyDown += new KeyEventHandler(OnFieldKeyDown);
                ipAddressFieldControls[index].KeyPress += new KeyPressEventHandler(OnFieldKeyPressed);
                ipAddressFieldControls[index].KeyUp += new KeyEventHandler(OnFieldKeyUp);
                ipAddressFieldControls[index].LostFocus += new EventHandler(OnFieldLostFocus);
                ipAddressFieldControls[index].MouseClick += new MouseEventHandler(OnSubControlMouseClicked);
                ipAddressFieldControls[index].MouseDoubleClick += new MouseEventHandler(OnSubControlMouseDoubleClicked);
                ipAddressFieldControls[index].MouseEnter += new EventHandler(OnSubControlMouseEntered);
                ipAddressFieldControls[index].MouseHover += new EventHandler(OnSubControlMouseHovered);
                ipAddressFieldControls[index].MouseLeave += new EventHandler(OnSubControlMouseLeft);
                ipAddressFieldControls[index].MouseMove += new MouseEventHandler(OnSubControlMouseMoved);
                ipAddressFieldControls[index].PreviewKeyDown += new PreviewKeyDownEventHandler(OnFieldPreviewKeyDown);
                ipAddressFieldControls[index].TextChangedEvent += new EventHandler<TextChangedEventArgs>(OnFieldTextChanged);

                Controls.Add(ipAddressFieldControls[index]);

                if (index < (FieldCount - 1))
                {
                    ipAddressDotControls[index] = new IPAddressDotControl();

                    ipAddressDotControls[index].CreateControl();

                    ipAddressDotControls[index].Name = "DotControl" + index.ToString(CultureInfo.InvariantCulture);
                    ipAddressDotControls[index].Parent = this;

                    ipAddressDotControls[index].Click += new EventHandler(OnSubControlClicked);
                    ipAddressDotControls[index].DoubleClick += new EventHandler(OnSubControlDoubleClicked);
                    ipAddressDotControls[index].MouseClick += new MouseEventHandler(OnSubControlMouseClicked);
                    ipAddressDotControls[index].MouseDoubleClick += new MouseEventHandler(OnSubControlMouseDoubleClicked);
                    ipAddressDotControls[index].MouseEnter += new EventHandler(OnSubControlMouseEntered);
                    ipAddressDotControls[index].MouseHover += new EventHandler(OnSubControlMouseHovered);
                    ipAddressDotControls[index].MouseLeave += new EventHandler(OnSubControlMouseLeft);
                    ipAddressDotControls[index].MouseMove += new MouseEventHandler(OnSubControlMouseMoved);

                    Controls.Add(ipAddressDotControls[index]);
                }
            }

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.FixedWidth, true);
            SetStyle(ControlStyles.FixedHeight, true);

            _referenceTextBox.AutoSize = true;

            Cursor = Cursors.IBeam;

            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;

            Size = MinimumSize;

            DragEnter += new DragEventHandler(IPAddressControl_DragEnter);
            DragDrop += new DragEventHandler(IPAddressControl_DragDrop);
        }

        #endregion // Constructors

        #region Public Constants

        public const int FieldCount = 4;

        public const string FieldMeasureText = "333";

        public const string FieldSeparator = ".";

        #endregion // Public Constants

        #region Public Events

        public event EventHandler<FieldChangedEventArgs> FieldChangedEvent;

        #endregion //Public Events

        #region Public Properties

        [Browsable(true)]
        public bool AllowInternalTab
        {
            get
            {
                foreach (IPAddressFieldControl fc in ipAddressFieldControls)
                {
                    return fc.TabStop;
                }

                return false;
            }
            set
            {
                foreach (IPAddressFieldControl fc in ipAddressFieldControls)
                {
                    fc.TabStop = value;
                }
            }
        }

        [Browsable(true)]
        public bool AnyBlank
        {
            get
            {
                foreach (IPAddressFieldControl fc in ipAddressFieldControls)
                {
                    if (fc.Blank)
                        return true;
                }

                return false;
            }
        }

        [Browsable(true)]
        public bool AutoHeight
        {
            get
            {
                return _autoHeight;
            }
            set
            {
                _autoHeight = value;

                if (_autoHeight)
                    AdjustSize();
            }
        }

        [Browsable(false)]
        public int Baseline
        {
            get
            {
                Native.TEXTMETRIC textMetric = GetTextMetrics(Handle, Font);

                int offset = textMetric.tmAscent + 1;

                switch (BorderStyle)
                {
                    case BorderStyle.Fixed3D:
                        offset += Fixed3DOffset.Height;
                        break;
                    case BorderStyle.FixedSingle:
                        offset += FixedSingleOffset.Height;
                        break;
                }

                return offset;
            }
        }

        [Browsable(true)]
        public bool Blank
        {
            get
            {
                foreach (IPAddressFieldControl fc in ipAddressFieldControls)
                {
                    if (!fc.Blank)
                        return false;
                }

                return true;
            }
        }

        [Browsable(true)]
        public BorderStyle BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                _borderStyle = value;
                AdjustSize();
                Invalidate();
            }
        }

        [Browsable(false)]
        public override bool Focused
        {
            get
            {
                foreach (IPAddressFieldControl fc in ipAddressFieldControls)
                {
                    if (fc.Focused)
                        return true;
                }

                return false;
            }
        }

        [Browsable(true)]
        public override Size MinimumSize
        {
            get
            {
                return CalculateMinimumSize();
            }
        }

        [Browsable(true)]
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;

                foreach (IPAddressFieldControl fc in ipAddressFieldControls)
                {
                    fc.ReadOnly = _readOnly;
                }

                foreach (IPAddressDotControl dc in ipAddressDotControls)
                {
                    dc.ReadOnly = _readOnly;
                }

                Invalidate();
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                ;

                for (int index = 0; index < ipAddressFieldControls.Length; ++index)
                {
                    sb.Append(ipAddressFieldControls[index].Text);

                    if (index < ipAddressDotControls.Length)
                        sb.Append(ipAddressDotControls[index].Text);
                }

                return sb.ToString();
            }
            set
            {
                Parse(value);
            }
        }

        #endregion // Public Properties

        #region Public Methods

        public void Clear()
        {
            foreach (IPAddressFieldControl fc in ipAddressFieldControls)
            {
                fc.Clear();
            }
        }

        public byte[] GetAddressBytes()
        {
            byte[] bytes = new byte[FieldCount];

            for (int index = 0; index < FieldCount; ++index)
            {
                bytes[index] = ipAddressFieldControls[index].Value;
            }

            return bytes;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720", Justification = "Prefer to use bytes as a variable name.")]
        public void SetAddressBytes(byte[] bytes)
        {
            Clear();

            if (bytes == null)
                return;

            int length = Math.Min(FieldCount, bytes.Length);

            for (int i = 0; i < length; ++i)
            {
                ipAddressFieldControls[i].Text = bytes[i].ToString(CultureInfo.InvariantCulture);
            }
        }

        public void SetFieldFocus(int fieldIndex)
        {
            if ((fieldIndex >= 0) && (fieldIndex < FieldCount))
                ipAddressFieldControls[fieldIndex].TakeFocus(IPAddressControlDirection.Forward, IPAddressControlSelection.All);
        }

        public void SetFieldRange(int fieldIndex, byte rangeLower, byte rangeUpper)
        {
            if ((fieldIndex >= 0) && (fieldIndex < FieldCount))
            {
                ipAddressFieldControls[fieldIndex].RangeLower = rangeLower;
                ipAddressFieldControls[fieldIndex].RangeUpper = rangeUpper;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < FieldCount; ++index)
            {
                sb.Append(ipAddressFieldControls[index].ToString());

                if (index < ipAddressDotControls.Length)
                    sb.Append(ipAddressDotControls[index].ToString());
            }

            return sb.ToString();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _backColorChanged = true;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            AdjustSize();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _focused = true;
            ipAddressFieldControls[0].TakeFocus(IPAddressControlDirection.Forward, IPAddressControlSelection.All);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!Focused)
            {
                _focused = false;
                base.OnLostFocus(e);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!_hasMouse)
            {
                _hasMouse = true;
                base.OnMouseEnter(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!HasMouse)
            {
                base.OnMouseLeave(e);
                _hasMouse = false;
            }
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

            using (SolidBrush backgroundBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
            }

            Rectangle rectBorder = new Rectangle(ClientRectangle.Left, ClientRectangle.Top,
                                                 ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            switch (BorderStyle)
            {
                case BorderStyle.Fixed3D:

                    if (Application.RenderWithVisualStyles)
                        ControlPaint.DrawVisualStyleBorder(e.Graphics, rectBorder);
                    else
                        ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
                    break;

                case BorderStyle.FixedSingle:

                    ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                            SystemColors.WindowFrame, ButtonBorderStyle.Solid);
                    break;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustSize();
        }

        #endregion // Protected Methods

        #region Private Properties

        private bool HasMouse
        {
            get
            {
                return DisplayRectangle.Contains(PointToClient(MousePosition));
            }
        }

        #endregion  // Private Properties

        #region Private Methods

        private void AdjustSize()
        {
            Size newSize = MinimumSize;

            if (Width > newSize.Width)
                newSize.Width = Width;

            if (Height > newSize.Height)
                newSize.Height = Height;

            if (AutoHeight)
                Size = new Size(newSize.Width, MinimumSize.Height);
            else
                Size = newSize;

            LayoutControls();
        }

        private Size CalculateMinimumSize()
        {
            Size minimumSize = new Size(0, 0);

            foreach (IPAddressFieldControl fc in ipAddressFieldControls)
            {
                minimumSize.Width += fc.Width;
                minimumSize.Height = Math.Max(minimumSize.Height, fc.Height);
            }

            foreach (IPAddressDotControl dc in ipAddressDotControls)
            {
                minimumSize.Width += dc.Width;
                minimumSize.Height = Math.Max(minimumSize.Height, dc.Height);
            }

            switch (BorderStyle)
            {
                case BorderStyle.Fixed3D:
                    minimumSize.Width += 6;
                    minimumSize.Height += (GetSuggestedHeight() - minimumSize.Height);
                    break;
                case BorderStyle.FixedSingle:
                    minimumSize.Width += 4;
                    minimumSize.Height += (GetSuggestedHeight() - minimumSize.Height);
                    break;
            }

            return minimumSize;
        }

        private int GetSuggestedHeight()
        {
            _referenceTextBox.BorderStyle = BorderStyle;
            _referenceTextBox.Font = Font;
            return _referenceTextBox.Height;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806", Justification = "What should be done if ReleaseDC() doesn't work?")]
        private static Native.TEXTMETRIC GetTextMetrics(IntPtr hwnd, Font font)
        {
            IntPtr hdc = Native.Methods.GetWindowDC(hwnd);

            Native.TEXTMETRIC textMetric;
            IntPtr hFont = font.ToHfont();

            try
            {
                IntPtr hFontPrevious = Native.Methods.SelectObject(hdc, hFont);
                Native.Methods.GetTextMetrics(hdc, out textMetric);
                Native.Methods.SelectObject(hdc, hFontPrevious);
            }
            finally
            {
                Native.Methods.ReleaseDC(hwnd, hdc);
                Native.Methods.DeleteObject(hFont);
            }

            return textMetric;
        }

        private void IPAddressControl_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Text = e.Data.GetData(DataFormats.Text).ToString();
        }

        private void IPAddressControl_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void LayoutControls()
        {
            SuspendLayout();

            int difference = Width - MinimumSize.Width;

            Debug.Assert(difference >= 0);

            int numOffsets = ipAddressFieldControls.Length + ipAddressDotControls.Length + 1;

            int div = difference/(numOffsets);
            int mod = difference%(numOffsets);

            int[] offsets = new int[numOffsets];

            for (int index = 0; index < numOffsets; ++index)
            {
                offsets[index] = div;

                if (index < mod)
                    ++offsets[index];
            }

            int x = 0;
            int y = 0;

            switch (BorderStyle)
            {
                case BorderStyle.Fixed3D:
                    x = Fixed3DOffset.Width;
                    y = Fixed3DOffset.Height;
                    break;
                case BorderStyle.FixedSingle:
                    x = FixedSingleOffset.Width;
                    y = FixedSingleOffset.Height;
                    break;
            }

            int offsetIndex = 0;

            x += offsets[offsetIndex++];

            for (int i = 0; i < ipAddressFieldControls.Length; ++i)
            {
                ipAddressFieldControls[i].Location = new Point(x, y);

                x += ipAddressFieldControls[i].Width;

                if (i < ipAddressDotControls.Length)
                {
                    x += offsets[offsetIndex++];
                    ipAddressDotControls[i].Location = new Point(x, y);
                    x += ipAddressDotControls[i].Width;
                    x += offsets[offsetIndex++];
                }
            }

            ResumeLayout(false);
        }

        private void OnCedeFocus(Object sender, CedeFocusEventArgs e)
        {
            switch (e.IPAddressControlAction)
            {
                case IPAddressControlAction.Home:

                    ipAddressFieldControls[0].TakeFocus(IPAddressControlAction.Home);
                    return;

                case IPAddressControlAction.End:

                    ipAddressFieldControls[FieldCount - 1].TakeFocus(IPAddressControlAction.End);
                    return;

                case IPAddressControlAction.Trim:

                    if (e.FieldIndex == 0)
                        return;

                    ipAddressFieldControls[e.FieldIndex - 1].TakeFocus(IPAddressControlAction.Trim);
                    return;
            }

            if ((e.IPAddressControlDirection == IPAddressControlDirection.Reverse && e.FieldIndex == 0) ||
                (e.IPAddressControlDirection == IPAddressControlDirection.Forward && e.FieldIndex == (FieldCount - 1)))
                return;

            int fieldIndex = e.FieldIndex;

            if (e.IPAddressControlDirection == IPAddressControlDirection.Forward)
                ++fieldIndex;
            else
                --fieldIndex;

            ipAddressFieldControls[fieldIndex].TakeFocus(e.IPAddressControlDirection, e.IPAddressControlSelection);
        }

        private void OnFieldGotFocus(Object sender, EventArgs e)
        {
            if (!_focused)
            {
                _focused = true;
                base.OnGotFocus(EventArgs.Empty);
            }
        }

        private void OnFieldKeyDown(Object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void OnFieldKeyPressed(Object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void OnFieldPreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            OnPreviewKeyDown(e);
        }

        private void OnFieldKeyUp(Object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        private void OnFieldLostFocus(Object sender, EventArgs e)
        {
            if (!Focused)
            {
                _focused = false;
                base.OnLostFocus(EventArgs.Empty);
            }
        }

        private void OnFieldTextChanged(Object sender, TextChangedEventArgs e)
        {
            if (null != FieldChangedEvent)
            {
                FieldChangedEventArgs args = new FieldChangedEventArgs();
                args.FieldIndex = e.FieldIndex;
                args.Text = e.Text;
                FieldChangedEvent(this, args);
            }

            OnTextChanged(EventArgs.Empty);
        }

        private void OnSubControlClicked(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void OnSubControlDoubleClicked(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }

        private void OnSubControlMouseClicked(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }

        private void OnSubControlMouseDoubleClicked(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        private void OnSubControlMouseEntered(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        private void OnSubControlMouseHovered(object sender, EventArgs e)
        {
            OnMouseHover(e);
        }

        private void OnSubControlMouseLeft(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        private void OnSubControlMouseMoved(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void Parse(String text)
        {
            Clear();

            if (null == text)
                return;

            int textIndex = 0;

            int index = 0;

            for (index = 0; index < ipAddressDotControls.Length; ++index)
            {
                int findIndex = text.IndexOf(ipAddressDotControls[index].Text, textIndex, StringComparison.Ordinal);

                if (findIndex >= 0)
                {
                    ipAddressFieldControls[index].Text = text.Substring(textIndex, findIndex - textIndex);
                    textIndex = findIndex + ipAddressDotControls[index].Text.Length;
                }
                else
                    break;
            }

            ipAddressFieldControls[index].Text = text.Substring(textIndex);
        }

        // a hack to remove an FxCop warning
        private void ResetBackColorChanged()
        {
            _backColorChanged = false;
        }

        #endregion Private Methods
    }
}