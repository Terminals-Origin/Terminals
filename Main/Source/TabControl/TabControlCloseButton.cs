using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TabControl
{
    internal class TabControlCloseButton
    {
        #region Fields

        private Rectangle crossRect = Rectangle.Empty;
        private bool isMouseOver = false;
        private ToolStripProfessionalRenderer renderer;

        #endregion

        #region Props

        public bool IsMouseOver
        {
            get { return isMouseOver; }
            set { isMouseOver = value; }
        }

        public Rectangle Rect
        {
            get { return crossRect; }
            set { crossRect = value; }
        }

        #endregion

        #region Ctor

        internal TabControlCloseButton(ToolStripProfessionalRenderer renderer)
        {
            this.renderer = renderer;
        }

        #endregion

        #region Methods

        public void DrawCross(Graphics g)
        {
            if (isMouseOver)
            {
                Color fill = renderer.ColorTable.ButtonSelectedHighlight;

                g.FillRectangle(new SolidBrush(fill), crossRect);

                Rectangle borderRect = crossRect;

                borderRect.Width--;
                borderRect.Height--;

                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }

            using (Pen pen = new Pen(Color.Black, 1f))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.DrawLine(pen, crossRect.Left + 3, crossRect.Top + 4,
                    crossRect.Right - 6, crossRect.Bottom - 4);
                g.DrawLine(pen, crossRect.Left + 4, crossRect.Top + 4,
                    crossRect.Right - 5, crossRect.Bottom - 4);

                g.DrawLine(pen, crossRect.Right - 6, crossRect.Top + 4,
                    crossRect.Left + 3, crossRect.Bottom - 4);
                g.DrawLine(pen, crossRect.Right - 5, crossRect.Top + 4,
                    crossRect.Left + 4, crossRect.Bottom - 4);
            }
        }

        #endregion
    }
}
