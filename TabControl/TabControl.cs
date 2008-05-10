using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Collections;

namespace TabControl
{
    [Designer(typeof(TabControlDesigner))]
    [DefaultEvent("TabControlItemSelectionChanged")]
    [DefaultProperty("Items")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TabControl))]
    public class TabControl : BaseStyledPanel, ISupportInitialize
    {
       

        #region Static Fields

        internal static int PreferredWidth = 350;
        internal static int PreferredHeight = 200;

        #endregion

        #region Constants

        private const int DEF_HEADER_HEIGHT = 19;
        //private int DEF_GLYPH_INDENT = 10;
        private int DEF_START_POS = 10;
        private const int DEF_GLYPH_WIDTH = 40;

        #endregion

        #region Fields

        private Rectangle stripButtonRect = Rectangle.Empty;
        private TabControlItem selectedItem = null;
        private ContextMenuStrip menu = null;
        private TabControlMenuGlyph menuGlyph = null;
        private TabControlCloseButton closeButton = null;
        private TabControlItemCollection items;
        private StringFormat sf = null;

        private bool alwaysShowClose = true;
        private bool isIniting = false;
        private bool alwaysShowMenuGlyph = true;
        private bool menuOpen = false;
        private bool showTabs = true;
        private bool showBorder = true;

        public event TabControlItemClosingHandler TabControlItemClosing;
        public event TabControlItemChangedHandler TabControlItemSelectionChanged;
        public event TabControlMouseOnTitleHandler TabControlMouseOnTitle;
        public event TabControlMouseLeftTitleHandler TabControlMouseLeftTitle;
        public event TabControlMouseEnteredTitleHandler TabControlMouseEnteredTitle;
        public event HandledEventHandler MenuItemsLoading;
        public event EventHandler MenuItemsLoaded;
        public event EventHandler TabControlItemClosed;

        #endregion

        #region Methods

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            UpdateLayout();
            Invalidate();
        }

        private bool AllowDraw(TabControlItem item)
        {
            bool result = true;

            if (RightToLeft == RightToLeft.No)
            {
                if (item.StripRect.Right >= stripButtonRect.Width)
                    result = false;
            }
            else
            {
                if (item.StripRect.Left <= stripButtonRect.Left)
                    return false;
            }

            return result;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SetDefaultSelected();
            if (showBorder)
            {
                Rectangle borderRc = base.ClientRectangle;
                borderRc.Width--;
                borderRc.Height--;
                e.Graphics.DrawRectangle(SystemPens.ControlDark, borderRc);
            }

            if (RightToLeft == RightToLeft.No)
            {
                DEF_START_POS = 10;
            }
            else
            {
                DEF_START_POS = stripButtonRect.Right;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF selectedButton = Rectangle.Empty;


            #region Draw Pages

            if (showTabs)
            {
                TabControlItem selectedTabItem = this.SelectedItem;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    TabControlItem currentItem = Items[i];
                    if (!currentItem.Visible && !DesignMode)
                        continue;

                    OnCalcTabPage(e.Graphics, currentItem);
                    currentItem.IsDrawn = false;

                    if (currentItem == selectedTabItem) //delay drawing active item to the end
                        continue;

                    if (!AllowDraw(currentItem))
                        continue;

                    OnDrawTabPage(e.Graphics, currentItem);
                }
                if (selectedTabItem != null && AllowDraw(selectedTabItem))
                {
                    try {
                        OnDrawTabPage(e.Graphics, selectedTabItem);
                    } catch(Exception exc) {
                        //black hole this for now
                    }
                }
            }

            #endregion

            #region Draw UnderPage Line

            if (showTabs && showBorder)
            {
                using (Pen pen = new Pen(ToolStripRenderer.ColorTable.MenuStripGradientBegin))
                {
                    if (RightToLeft == RightToLeft.No)
                    {
                        if (Items.DrawnCount == 0 || Items.VisibleCount == 0)
                        {
                            e.Graphics.DrawLine(pen, new Point(0, DEF_HEADER_HEIGHT), new Point(ClientRectangle.Width, DEF_HEADER_HEIGHT));
                        }
                        else if (SelectedItem != null && SelectedItem.IsDrawn)
                        {
                            Point end = new Point((int)SelectedItem.StripRect.Left - 9, DEF_HEADER_HEIGHT);
                            e.Graphics.DrawLine(pen, new Point(0, DEF_HEADER_HEIGHT), end);
                            end.X += (int)SelectedItem.StripRect.Width + 10;
                            e.Graphics.DrawLine(pen, end, new Point(ClientRectangle.Width, DEF_HEADER_HEIGHT));
                        }
                    }
                    else
                    {
                        if (Items.DrawnCount == 0 || Items.VisibleCount == 0)
                        {
                            e.Graphics.DrawLine(SystemPens.ControlDark, new Point(0, DEF_HEADER_HEIGHT), new Point(ClientRectangle.Width, DEF_HEADER_HEIGHT));
                        }
                        else if (SelectedItem != null && SelectedItem.IsDrawn)
                        {
                            Point end = new Point((int)SelectedItem.StripRect.Left, DEF_HEADER_HEIGHT);
                            e.Graphics.DrawLine(pen, new Point(0, DEF_HEADER_HEIGHT), end);
                            end.X += (int)SelectedItem.StripRect.Width + 20;
                            e.Graphics.DrawLine(pen, end, new Point(ClientRectangle.Width, DEF_HEADER_HEIGHT));
                        }
                    }
                }
            }

            #endregion

            #region Draw Menu and Close Glyphs

            if (showTabs)
            {
                if ((AlwaysShowMenuGlyph && Items.VisibleCount > 0) || Items.DrawnCount > Items.VisibleCount)
                    menuGlyph.DrawGlyph(e.Graphics);

                if (AlwaysShowClose || (SelectedItem != null && SelectedItem.CanClose))
                    closeButton.DrawCross(e.Graphics);
            }
            #endregion
        }

        public void AddTab(TabControlItem tabItem)
        {
            Items.Add(tabItem);
            tabItem.Dock = DockStyle.Fill;
        }

        public void RemoveTab(TabControlItem tabItem)
        {
            int tabIndex = Items.IndexOf(tabItem);
            bool wasSelected = tabItem.Selected;

            if (tabIndex >= 0)
            {
                UnSelectItem(tabItem);
                Items.Remove(tabItem);
            }

            if (wasSelected)
            {
                if (Items.Count > 0)
                {
                    if (RightToLeft == RightToLeft.No)
                    {
                        if (Items[tabIndex - 1] != null)
                        {
                            SelectedItem = Items[tabIndex - 1];
                        }
                        else
                        {
                            SelectedItem = Items.FirstVisible;
                        }
                    }
                    else
                    {
                        if (Items[tabIndex + 1] != null)
                        {
                            SelectedItem = Items[tabIndex + 1];
                        }
                        else
                        {
                            SelectedItem = Items.LastVisible;
                        }
                    }
                }
                else
                    SelectedItem = null;
            }
        }
        public void ForceCloseTab(TabControlItem tabItem)
        {
            RemoveTab(tabItem);
            OnTabControlItemClosed(EventArgs.Empty);
        }
        public void CloseTab(TabControlItem tabItem)
        {
            if (tabItem != null)
            {
                TabControlItemClosingEventArgs args = new TabControlItemClosingEventArgs(SelectedItem);
                OnTabControlItemClosing(args);
                if (!args.Cancel && SelectedItem.CanClose)
                {
                    RemoveTab(SelectedItem);
                    OnTabControlItemClosed(EventArgs.Empty);
                }
            }
        }
        protected override void OnClick(EventArgs e)
        {
            //If its the middle button, close the tab
            MouseEventArgs mouse = (e as MouseEventArgs);
            if(mouse != null)
            {
                if(mouse.Button == MouseButtons.Middle)
                {
                    TabControlItem item = GetTabItemByPoint(PointToClient(Cursor.Position));
                    if(item!=null) CloseTab(item);
                }
            }
            base.OnClick(e);
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            TabControlItem item = GetTabItemByPoint(PointToClient(Cursor.Position));
            if (item != null)
            {
                CloseTab(item);
            }
        }

        private bool showToolTipOnTitle;

        public bool ShowToolTipOnTitle
        {
            get { return showToolTipOnTitle; }
            set { showToolTipOnTitle = value; }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            /*if (showToolTipOnTitle)
            {
                TabControlItem item = GetTabItemByPoint(PointToClient(Cursor.Position));
                if ((item != null) && (item != selectedItem))
                {
                    ToolTip toolTip = new ToolTip();
                    toolTip.ToolTipTitle = item.Title;
                    toolTip.ToolTipIcon = ToolTipIcon.Info;
                    toolTip.UseFading = true;
                    toolTip.UseAnimation = true;
                    toolTip.IsBalloon = false;
                    toolTip.Show(item.ToolTipText, item, PointToClient(Cursor.Position), 5000);
                }
            }*/
        }
        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if(downItem != null) {
                TabControlItem upItem = GetTabItemByPoint(e.Location);
                if(upItem != downItem) {
                    //perform swap
                    int downIndex = this.items.IndexOf(downItem);
                    int newIndex = downIndex;
                    if(upItem == null) {
                        newIndex = this.items.Count;
                    } else {
                        if(upItem.Left > downItem.Left) {
                            newIndex = downIndex - 1;
                        } else {
                            newIndex = downIndex + 1;
                        }
                    }
                    if(newIndex>this.Items.Count-1) newIndex = this.Items.Count-1;
                    if(newIndex < 0) newIndex = 0;
                    this.items.Remove(downItem);
                    this.items.Insert(newIndex, downItem);

                }
            }
            downItem = null;
        }
        TabControlItem downItem = null;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if(e.Button != MouseButtons.Left) {
                return;
            }
            downItem = GetTabItemByPoint(e.Location);
            if (menuGlyph.Rect.Contains(e.Location))
            {
                HandledEventArgs args = new HandledEventArgs(false);
                OnMenuItemsLoading(args);
                if (!args.Handled)
                    OnMenuItemsLoad(EventArgs.Empty);

                OnMenuShow();
            }
            else if (closeButton.Rect.Contains(e.Location))
            {
                if (SelectedItem != null)
                {
                    TabControlItemClosingEventArgs args = new TabControlItemClosingEventArgs(SelectedItem);
                    OnTabControlItemClosing(args);
                    if (SelectedItem!=null && !args.Cancel && SelectedItem.CanClose)
                    {
                        RemoveTab(SelectedItem);
                        OnTabControlItemClosed(EventArgs.Empty);
                    }
                }
            }
            else
            {
                TabControlItem item = GetTabItemByPoint(e.Location);
                if (item != null)
                    SelectedItem = item;
            }

            Invalidate();
        }

        public TabControlItem GetTabItemByPoint(Point pt)
        {
            TabControlItem item = null;

            for (int i = 0; i < Items.Count; i++)
            {
                TabControlItem current = Items[i];
                if (current.StripRect.Contains(pt))
                {
                    item = current;
                }
            }

            return item;
        }

        private void ShowTooltip(string text)
        {
            //// Remember starting mouse position
            //tooltipRect = new Rectangle(Control.MousePosition, Size.Empty);

            //// Make twice as big as the double click region
            //tooltipRect.Inflate(SystemInformation.DoubleClickSize.Width * 2, SystemInformation.DoubleClickSize.Height * 2);

            //// Move left and up to center at mouse point
            //tooltipRect.Offset(-SystemInformation.DoubleClickSize.Width, -SystemInformation.DoubleClickSize.Height);

            //// Create a tooltip control
            //toolTip = new PopupTooltipSingle();

            //// Define string for display
            //toolTip.ToolText = text;

            //// Setup its position
            //_toolTip.ShowWithoutFocus(new Point(Control.MousePosition.X, Control.MousePosition.Y + 24));
        }

        private bool mouseWasOnTitle;
        private bool mouseEnteredTitle;

        protected internal virtual void OnTabControlMouseOnTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (TabControlMouseOnTitle != null)
            {
                TabControlMouseOnTitle(e);
            }
        }

        protected internal virtual void OnTabControlMouseEnteredTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (TabControlMouseEnteredTitle != null)
            {
                TabControlMouseEnteredTitle(e);
            }
        }

        protected internal virtual void OnTabControlMouseLeftTitle(TabControlMouseOnTitleEventArgs e)
        {
            if (TabControlMouseLeftTitle != null)
            {
                TabControlMouseLeftTitle(e);
            }
        }

        protected internal virtual void OnTabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            if (TabControlItemClosing != null)
                TabControlItemClosing(e);
        }

        protected internal virtual void OnTabControlItemClosed(EventArgs e)
        {
            if (TabControlItemClosed != null)
                TabControlItemClosed(this, e);
        }

        private void SetDefaultSelected()
        {
            if (selectedItem == null && Items.Count > 0)
                SelectedItem = Items[0];

            for (int i = 0; i < this.Items.Count; i++)
            {
                TabControlItem itm = Items[i];
                itm.Dock = DockStyle.Fill;
            }
        }

        private void UnSelectAll()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                TabControlItem item = this.Items[i];
                UnSelectItem(item);
            }
        }

        internal void UnDrawAll()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].IsDrawn = false;
            }
        }

        internal void SelectItem(TabControlItem tabItem)
        {
            tabItem.Dock = DockStyle.Fill;
            tabItem.Visible = true;
            tabItem.Selected = true;
        }

        internal void UnSelectItem(TabControlItem tabItem)
        {
            //tabItem.Visible = false;
            tabItem.Selected = false;
        }

        protected virtual void OnMenuItemsLoading(HandledEventArgs e)
        {
            if (MenuItemsLoading != null)
                MenuItemsLoading(this, e);
        }

        protected virtual void OnMenuShow()
        {
            if (menu.Visible == false && menu.Items.Count > 0)
            {
                if (RightToLeft == RightToLeft.No)
                {
                    menu.Show(this, new Point(menuGlyph.Rect.Left, menuGlyph.Rect.Bottom + 2));
                }
                else
                {
                    menu.Show(this, new Point(menuGlyph.Rect.Right, menuGlyph.Rect.Bottom + 2));
                }

                menuOpen = true;
            }
        }

        protected virtual void OnMenuItemsLoaded(EventArgs e)
        {
            if (MenuItemsLoaded != null)
                MenuItemsLoaded(this, e);
        }

        protected virtual void OnMenuItemsLoad(EventArgs e)
        {
            menu.RightToLeft = this.RightToLeft;
            menu.Items.Clear();

            for (int i = 0; i < Items.Count; i++)
            {
                TabControlItem item = this.Items[i];
                if (!item.Visible)
                    continue;

                ToolStripMenuItem tItem = new ToolStripMenuItem(item.Title);
                tItem.Tag = item;
                menu.Items.Add(tItem);
            }

            OnMenuItemsLoaded(EventArgs.Empty);
        }

        private void OnMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TabControlItem clickedItem = (TabControlItem)e.ClickedItem.Tag;
            if(clickedItem != null) {
                SelectedItem = clickedItem;
            }
        }

        private void OnMenuVisibleChanged(object sender, EventArgs e)
        {
            if (menu.Visible == false)
            {
                menuOpen = false;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (menuGlyph.Rect.Contains(e.Location))
            {
                menuGlyph.IsMouseOver = true;
                this.Invalidate(menuGlyph.Rect);
            }
            else
            {
                if (menuGlyph.IsMouseOver != false && !menuOpen)
                {
                    menuGlyph.IsMouseOver = false;
                    this.Invalidate(menuGlyph.Rect);
                }
            }

            if (closeButton.Rect.Contains(e.Location))
            {
                closeButton.IsMouseOver = true;
                this.Invalidate(closeButton.Rect);
            }
            else
            {
                if (closeButton.IsMouseOver != false)
                {
                    closeButton.IsMouseOver = false;
                    this.Invalidate(closeButton.Rect);
                }
            }

            TabControlItem item = GetTabItemByPoint(e.Location);
            if (item != null)
            {
                bool inTitle = (((item.StripRect.X + item.StripRect.Width - 1) > e.Location.X) &&
                    ((item.StripRect.Y + item.StripRect.Height - 1) > e.Location.Y));

                TabControlMouseOnTitleEventArgs args = new TabControlMouseOnTitleEventArgs(item, e.Location);
                if (inTitle)
                {
                    //mouseWasOnTitle = true;
                    OnTabControlMouseOnTitle(args);
                    if (!mouseEnteredTitle)
                    {
                        mouseEnteredTitle = true;
                        OnTabControlMouseEnteredTitle(args);
                    }
                }
                else if (mouseEnteredTitle)// if (mouseWasOnTitle)
                {
                    //mouseWasOnTitle = false;
                    mouseEnteredTitle = false;
                    OnTabControlMouseLeftTitle(args);
                }
            }
            /*else
            {
                if (mouseWasOnTitle)
                {
                    mouseWasOnTitle = false;
                    mouseEnteredTitle = false;
                    OnTabControlMouseLeftTitle(new EventArgs());
                }
            }*/
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            menuGlyph.IsMouseOver = false;
            this.Invalidate(menuGlyph.Rect);

            closeButton.IsMouseOver = false;
            this.Invalidate(closeButton.Rect);
        }

        private void OnCalcTabPage(Graphics g, TabControlItem currentItem)
        {
            Font currentFont = this.Font;
            if (currentItem == SelectedItem)
                currentFont = new Font(this.Font, FontStyle.Bold);

            SizeF textSize = g.MeasureString(currentItem.Title, currentFont, new SizeF(200, 10), sf);
            textSize.Width += 20;

            if (RightToLeft == RightToLeft.No)
            {
                RectangleF buttonRect = new RectangleF(DEF_START_POS, 3, textSize.Width, 17);
                currentItem.StripRect = buttonRect;
                DEF_START_POS += (int)textSize.Width;
            }
            else
            {
                RectangleF buttonRect = new RectangleF(DEF_START_POS - textSize.Width + 1, 3, textSize.Width - 1, 17);
                currentItem.StripRect = buttonRect;
                DEF_START_POS -= (int)textSize.Width;
            }
        }

        private void OnDrawTabPage(Graphics g, TabControlItem currentItem)
        {
            bool isFirstTab = Items.IndexOf(currentItem) == 0;
            Font currentFont = this.Font;

            if (currentItem == SelectedItem)
                currentFont = new Font(this.Font, FontStyle.Bold);

            SizeF textSize = g.MeasureString(currentItem.Title, currentFont, new SizeF(200, 10), sf);
            textSize.Width += 20;
            RectangleF buttonRect = currentItem.StripRect;

            GraphicsPath path = new GraphicsPath();
            LinearGradientBrush brush = null;
            int mtop = 3;

            #region Draw Not Right-To-Left Tab

            if (RightToLeft == RightToLeft.No)
            {
                if (currentItem == SelectedItem || isFirstTab)
                {
                    path.AddLine(buttonRect.Left - 10, buttonRect.Bottom - 1, buttonRect.Left + (buttonRect.Height / 2) - 4, mtop + 4);
                }
                else
                {
                    path.AddLine(buttonRect.Left, buttonRect.Bottom - 1, buttonRect.Left, buttonRect.Bottom - (buttonRect.Height / 2) - 2);
                    path.AddLine(buttonRect.Left, buttonRect.Bottom - (buttonRect.Height / 2) - 3, buttonRect.Left + (buttonRect.Height / 2) - 4, mtop + 3);
                }

                path.AddLine(buttonRect.Left + (buttonRect.Height / 2) + 2, mtop, buttonRect.Right - 3, mtop);
                path.AddLine(buttonRect.Right, mtop + 2, buttonRect.Right, buttonRect.Bottom - 1);
                path.AddLine(buttonRect.Right - 4, buttonRect.Bottom - 1, buttonRect.Left, buttonRect.Bottom - 1);
                path.CloseFigure();
                try {
                    if(currentItem == SelectedItem) {
                        brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Window, LinearGradientMode.Vertical);
                    } else {
                        brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical);
                    }
                }catch(Exception ex) {
                    
                }
                g.FillPath(brush, path);
                Pen pen = SystemPens.ControlDark;
                if (currentItem == SelectedItem)
                {
                    pen = new Pen(ToolStripRenderer.ColorTable.MenuStripGradientBegin);
                }
                g.DrawPath(pen, path);
                if (currentItem == SelectedItem)
                {
                    pen.Dispose();
                }

                if (currentItem == SelectedItem)
                {
                    g.DrawLine(new Pen(brush), buttonRect.Left - 9, buttonRect.Height + 2, buttonRect.Left + buttonRect.Width - 1, buttonRect.Height + 2);
                }

                PointF textLoc = new PointF(buttonRect.Left + buttonRect.Height - 4, buttonRect.Top + (buttonRect.Height / 2) - (textSize.Height / 2) - 3);
                RectangleF textRect = buttonRect;
                textRect.Location = textLoc;
                textRect.Width = (float)buttonRect.Width - (textRect.Left - buttonRect.Left) - 4;
                textRect.Height = textSize.Height + currentFont.Size / 2;

                if (currentItem == SelectedItem)
                {
                    //textRect.Y -= 2;
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(this.ForeColor), textRect, sf);
                }
                else
                {
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(this.ForeColor), textRect, sf);
                }
            }

            #endregion

            #region Draw Right-To-Left Tab

            if (RightToLeft == RightToLeft.Yes)
            {
                if (currentItem == SelectedItem || isFirstTab)
                {
                    path.AddLine(buttonRect.Right + 10, buttonRect.Bottom - 1, buttonRect.Right - (buttonRect.Height / 2) + 4, mtop + 4);
                }
                else
                {
                    path.AddLine(buttonRect.Right, buttonRect.Bottom - 1, buttonRect.Right, buttonRect.Bottom - (buttonRect.Height / 2) - 2);
                    path.AddLine(buttonRect.Right, buttonRect.Bottom - (buttonRect.Height / 2) - 3, buttonRect.Right - (buttonRect.Height / 2) + 4, mtop + 3);
                }

                path.AddLine(buttonRect.Right - (buttonRect.Height / 2) - 2, mtop, buttonRect.Left + 3, mtop);
                path.AddLine(buttonRect.Left, mtop + 2, buttonRect.Left, buttonRect.Bottom - 1);
                path.AddLine(buttonRect.Left + 4, buttonRect.Bottom - 1, buttonRect.Right, buttonRect.Bottom - 1);
                path.CloseFigure();

                if (currentItem == SelectedItem)
                {
                    brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Window, LinearGradientMode.Vertical);
                }
                else
                {
                    brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical);
                }

                g.FillPath(brush, path);
                g.DrawPath(SystemPens.ControlDark, path);

                if (currentItem == SelectedItem)
                {
                    g.DrawLine(new Pen(brush), buttonRect.Right + 9, buttonRect.Height + 2, buttonRect.Right - buttonRect.Width + 1, buttonRect.Height + 2);
                }

                PointF textLoc = new PointF(buttonRect.Left + 2, buttonRect.Top + (buttonRect.Height / 2) - (textSize.Height / 2) - 2);
                RectangleF textRect = buttonRect;
                textRect.Location = textLoc;
                textRect.Width = (float)buttonRect.Width - (textRect.Left - buttonRect.Left) - 10;
                textRect.Height = textSize.Height + currentFont.Size / 2;

                if (currentItem == SelectedItem)
                {
                    textRect.Y -= 1;
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(this.ForeColor), textRect, sf);
                }
                else
                {
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(this.ForeColor), textRect, sf);
                }

                //g.FillRectangle(Brushes.Red, textRect);
            }

            #endregion

            currentItem.IsDrawn = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (isIniting)
                return;

            UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (RightToLeft == RightToLeft.No)
            {
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags |= StringFormatFlags.NoWrap;
                sf.FormatFlags &= StringFormatFlags.DirectionRightToLeft;

                stripButtonRect = new Rectangle(0, 0, this.ClientSize.Width - DEF_GLYPH_WIDTH - 2, 10);
                menuGlyph.Rect = new Rectangle(this.ClientSize.Width - DEF_GLYPH_WIDTH, 2, 16, 16);
                closeButton.Rect = new Rectangle(this.ClientSize.Width - 20, 2, 16, 15);
            }
            else
            {
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags |= StringFormatFlags.NoWrap;
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

                stripButtonRect = new Rectangle(DEF_GLYPH_WIDTH + 2, 0, this.ClientSize.Width - DEF_GLYPH_WIDTH - 15, 10);
                menuGlyph.Rect = new Rectangle(20 + 4, 2, 16, 16);//this.ClientSize.Width - 20, 2, 16, 16);
                closeButton.Rect = new Rectangle(4, 2, 16, 15);//ClientSize.Width - DEF_GLYPH_WIDTH, 2, 16, 16);
            }

            int borderWidth = (showBorder ? 1 : 0);
            int headerWidth = (showTabs ? DEF_HEADER_HEIGHT + 1 : 1);

            DockPadding.Top = headerWidth;
            DockPadding.Bottom = borderWidth;
            DockPadding.Right = borderWidth;
            DockPadding.Left = borderWidth;
        }

        protected virtual void OnTabControlItemChanged(TabControlItemChangedEventArgs e)
        {
            if (TabControlItemSelectionChanged != null)
                TabControlItemSelectionChanged(e);
        }

        private void OnCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            TabControlItem itm = (TabControlItem)e.Element;

            if (e.Action == CollectionChangeAction.Add)
            {
                Controls.Add(itm);
                OnTabControlItemChanged(new TabControlItemChangedEventArgs(itm, TabControlItemChangeTypes.Added));
            }
            else if (e.Action == CollectionChangeAction.Remove)
            {
                Controls.Remove(itm);
                OnTabControlItemChanged(new TabControlItemChangedEventArgs(itm, TabControlItemChangeTypes.Removed));
            }
            else
            {
                OnTabControlItemChanged(new TabControlItemChangedEventArgs(itm, TabControlItemChangeTypes.Changed));
            }

            UpdateLayout();
            Invalidate();
        }

        #endregion

        #region Ctor

        public TabControl()
        {
            BeginInit();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.Selectable, true);

            items = new TabControlItemCollection(this);
            items.CollectionChanged += new CollectionChangeEventHandler(OnCollectionChanged);
            base.Size = new Size(350, 200);

            menu = new ContextMenuStrip();
            menu.Renderer = ToolStripRenderer;
            menu.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuItemClicked);
            menu.VisibleChanged += new EventHandler(OnMenuVisibleChanged);

            menuGlyph = new TabControlMenuGlyph(ToolStripRenderer);
            closeButton = new TabControlCloseButton(ToolStripRenderer);
            Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
            sf = new StringFormat();

            EndInit();

            UpdateLayout();
        }

        #endregion

        #region Props

        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.All)]
        public TabControlItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem == value)
                    return;

                if (value == null && Items.Count > 0)
                {
                    TabControlItem itm = Items[0];
                    if (itm.Visible)
                    {
                        selectedItem = itm;
                        selectedItem.Selected = true;
                        selectedItem.Dock = DockStyle.Fill;
                    }
                }
                else
                {
                    selectedItem = value;
                }

                foreach (TabControlItem itm in Items)
                {
                    if (itm == selectedItem)
                    {
                        SelectItem(itm);
                        itm.Dock = DockStyle.Fill;
                        itm.Show();
                    }
                    else
                    {
                        UnSelectItem(itm);
                        itm.Hide();
                    }
                }

                if (selectedItem != null)
                    SelectItem(selectedItem);
                Invalidate();

                /*if (selectedItem != null && !selectedItem.IsDrawn)
                {
                    Items.MoveTo(0, selectedItem);
                    Invalidate();
                }*/

                OnTabControlItemChanged(new TabControlItemChangedEventArgs(selectedItem, TabControlItemChangeTypes.SelectionChanged));
            }
        }

        [DefaultValue(typeof(Size), "350,200")]
        public new Size Size
        {
            get { return base.Size; }
            set
            {
                if (base.Size == value)
                    return;

                base.Size = value;
                UpdateLayout();
            }
        }

        [DefaultValue(true)]
        public bool AlwaysShowMenuGlyph
        {
            get { return alwaysShowMenuGlyph; }
            set
            {
                if (alwaysShowMenuGlyph == value)
                    return;

                alwaysShowMenuGlyph = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public bool AlwaysShowClose
        {
            get { return alwaysShowClose; }
            set
            {
                if (alwaysShowClose == value)
                    return;

                alwaysShowClose = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public bool ShowTabs
        {
            get
            {
                return showTabs;
            }
            set
            {
                if (showTabs != value)
                {
                    showTabs = value;
                    Invalidate();
                    UpdateLayout();
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowBorder
        {
            get
            {
                return showBorder;
            }
            set
            {
                if (showBorder != value)
                {
                    showBorder = value;
                    Invalidate();
                    UpdateLayout();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabControlItemCollection Items
        {
            get { return items; }
        }

        /// <summary>
        /// DesignerSerializationVisibility
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Control.ControlCollection Controls
        {
            get { return base.Controls; }
        }

        [Browsable(false)]
        public ContextMenuStrip Menu
        {
            get
            {
                return menu;
            }
        }

        [Browsable(false)]
        public bool MenuOpen
        {
            get
            {
                return menuOpen;
            }
        }

        #endregion

        #region ShouldSerialize

        public bool ShouldSerializeSelectedItem()
        {
            return true;
        }

        public bool ShouldSerializeItems()
        {
            return items.Count > 0;
        }

        public bool ShouldSerializeFont()
        {
            return Font.Name != "Tahoma" && Font.Size != 8.25f && Font.Style != FontStyle.Regular;
        }

        public new void ResetFont()
        {
            Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
        }

        #endregion

        #region ISupportInitialize Members

        public void BeginInit()
        {
            isIniting = true;
        }

        public void EndInit()
        {
            isIniting = false;
        }

        #endregion

    }
}
