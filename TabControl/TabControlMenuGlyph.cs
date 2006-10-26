using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TabControl
{
    internal class TabControlMenuGlyph
    {
        #region Fields

        private Rectangle glyphRect = Rectangle.Empty;
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
            get { return glyphRect; }
            set { glyphRect = value; }
        }

        #endregion

        #region Ctor

        internal TabControlMenuGlyph(ToolStripProfessionalRenderer renderer)
        {
            this.renderer = renderer;
        }

        #endregion

        #region Methods

        public void DrawGlyph(Graphics g)
        {
            if (isMouseOver)
            {
                Color fill = renderer.ColorTable.ButtonSelectedHighlight; //Color.FromArgb(35, SystemColors.Highlight);
                g.FillRectangle(new SolidBrush(fill), glyphRect);
                Rectangle borderRect = glyphRect;

                borderRect.Width--;
                borderRect.Height--;

                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }

            SmoothingMode bak = g.SmoothingMode;

            g.SmoothingMode = SmoothingMode.Default;

            using (Pen pen = new Pen(Color.Black))
            {
                pen.Width = 2;

                g.DrawLine(pen, new Point(glyphRect.Left + (glyphRect.Width / 3) - 2, glyphRect.Height / 2 - 1),
                    new Point(glyphRect.Right - (glyphRect.Width / 3), glyphRect.Height / 2 - 1));
            }

            g.FillPolygon(Brushes.Black, new Point[]{
                new Point(glyphRect.Left + (glyphRect.Width / 3)-2, glyphRect.Height / 2+2),
                new Point(glyphRect.Right - (glyphRect.Width / 3), glyphRect.Height / 2+2),
                new Point(glyphRect.Left + glyphRect.Width / 2-1,glyphRect.Bottom-4)});

            g.SmoothingMode = bak;
        }

        #endregion
    }
}
