using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace TabControl
{
    [Designer(typeof(TabControlItemDesigner))]
    [ToolboxItem(false)]
    [DefaultProperty("Title")]
    [DefaultEvent("Changed")]
    public class TabControlItem : Panel
    {
        #region Fields

        //private DrawItemState drawState = DrawItemState.None;
        private RectangleF stripRect = Rectangle.Empty;
        private bool canClose = true;
        private bool selected = false;
        private bool visible = true;
        private bool isDrawn = false;
        private string title = string.Empty;

        public event EventHandler Changed;

        #endregion

        #region Props

        [DefaultValue(true)]
        public new bool Visible
        {
            get { return visible; }
            set
            {
                if (visible == value)
                    return;

                visible = value;
                OnChanged();
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDrawn
        {
            get { return isDrawn; }
            set
            {
                if (isDrawn == value)
                    return;

                isDrawn = value;
            }
        }

        [DefaultValue(true)]
        public bool CanClose
        {
            get { return canClose; }
            set { canClose = value; }
        }

        internal RectangleF StripRect
        {
            get { return stripRect; }
            set { stripRect = value; }
        }

        [DefaultValue("Name")]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title == value)
                    return;

                title = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Gets and sets a value indicating if the page is selected.
        /// </summary>
        [DefaultValue(false)]
        [Browsable(false)]
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected == value)
                    return;

                selected = value;
            }
        }

        #endregion

        #region Ctor

        public TabControlItem() : this(string.Empty, null)
        {
        }

        public TabControlItem(Control displayControl) : this(string.Empty, displayControl)
        {
        }

        public TabControlItem(string caption, Control displayControl) 
        {
            //base.Dock = DockStyle.Fill;
            //this.Size = new Size(100, 100);
            this.selected = false;
            this.Visible = true;
            this.BorderStyle = BorderStyle.None;

            UpdateText(caption, displayControl);

            //Add to controls
            this.Controls.Add(displayControl);
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeIsDrawn()
        {
            return false;
        }

        public bool ShouldSerializeDock()
        {
            return false;
        }

        public bool ShouldSerializeVisible()
        {
            return true;
        }

        #endregion

        #region Methods

        private void UpdateText(string caption, Control displayControl)
        {
            if (displayControl != null && displayControl is ICaptionSupport)
            {
                ICaptionSupport capControl = displayControl as ICaptionSupport;
                this.Title = capControl.Caption;
            }
            else if (caption.Length <= 0 && displayControl != null)
            {
                this.Title = displayControl.Text;
            }
            else if (caption != null)
            {
                this.Title = caption;
            }
            else
            {
                this.Title = string.Empty;
            }
        }

        /// <summary>
        /// Return a string representation of page.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Caption;
        }

        public void Assign(TabControlItem item)
        {
            this.Visible = item.Visible;
            this.Text = item.Text;
            this.CanClose = item.CanClose;
            this.Tag = item.Tag;
        }

        protected internal virtual void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        #endregion

        #region ICaptionSupport Members

        [Browsable(false)]
        public string Caption
        {
            get { return Text; }
        }

        #endregion

    }
}
