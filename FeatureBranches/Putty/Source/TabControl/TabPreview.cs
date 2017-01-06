using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TabControl
{
    /// <summary>
    /// Form, which shows present selected tab when moving using drag and drop
    /// </summary>
    internal partial class TabPreview : Form
    {
        private TabControl tabControl;
        private TabControlItem selectedTabItem;

        public TabPreview()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.Selectable, true);
        }

        internal TabPreview(TabControl tabControl)
            : this()
        {
            this.tabControl = tabControl;
        }

        internal void UpdateDetachState(bool toDetach)
        {
            this.imageDetach.Visible = toDetach;
        }

        internal void Show(TabControlItem tabControlItem)
        {
            this.selectedTabItem = tabControlItem;
            this.UpdateSize(tabControlItem);

            if (!this.Visible)
            {
                this.Show(); // this grabs the fucus, which we have to return
                Form parentForm = this.tabControl.FindForm();
                if (parentForm != null)
                    parentForm.Focus();
            }
        }

        private void UpdateSize(TabControlItem tabControlItem)
        {
            RectangleF tabRectangle = tabControlItem.StripRect;
            // two pixels to add right mergin
            // tab doesnt have to be first, so include all on left side
            int imageDistance = this.imageDetach.Width + 4;
            this.Width = (int)(tabRectangle.Width + tabRectangle.Left) + imageDistance;
            this.Height = (int)(tabRectangle.Height + tabRectangle.Top);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.tabControl != null && this.selectedTabItem != null) // because of designer
            {
                PaintPreview(e);
            }
        }

        private void PaintPreview(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.tabControl.OnDrawTabPage(e.Graphics, this.selectedTabItem);
        }
    }
}
