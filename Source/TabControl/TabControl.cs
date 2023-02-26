using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace TabControl
{
    /// <summary>
    ///  http://www.codeproject.com/Articles/13902/TabStrips-A-TabControl-in-the-Visual-Studio-2005-w
    /// </summary>
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

        private TabControlItem tabAtMouseDown = null;
        private bool showToolTipOnTitle;
        private bool mouseEnteredTitle;

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
        public event TabControlItemClosedHandler TabControlItemClosed;
        public event TabControlItemChangedHandler TabControlItemDetach;

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

                // todo bug 32353 - first calculate all, then identify ragne of tabs to draw, and finally paint them 
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
                    } catch(Exception) {
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
            this.OnTabControlItemClosed(tabItem);
        }

        public void CloseTab(TabControlItem tabItem)
        {
            if (tabItem != null)
            {
                this.SelectedItem = tabItem;
                TabControlItemClosingEventArgs args = new TabControlItemClosingEventArgs(SelectedItem);
                OnTabControlItemClosing(args);
                if (SelectedItem != null && !args.Cancel && SelectedItem.CanClose)
                {
                    RemoveTab(SelectedItem);
                    this.OnTabControlItemClosed(tabItem);
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.IsMouseModdleClick(e))
                this.CloseTabAtCurrentCursor();

            base.OnClick(e);
        }

        private bool IsMouseModdleClick(EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;
            return mouse != null && mouse.Button == MouseButtons.Middle;
        }

        private void CloseTabAtCurrentCursor()
        {
            TabControlItem selectedTab = this.GetTabItemByPoint(this.PointToClient(Cursor.Position));
            this.CloseTab(selectedTab);
        }

        public bool ShowToolTipOnTitle
        {
            get { return showToolTipOnTitle; }
            set { showToolTipOnTitle = value; }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                this.HandleTablItemMouseUpActions(e);
                bool handled = this.HandleMenuGlipMouseUp(e);
                handled |= this.HandleCloseButtonMouseUp(e);
                handled |= this.HandleTabDetach(e);

                if (!handled)
                    base.OnMouseUp(e);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
            finally
            {
                this.tabAtMouseDown = null;
                this.mouseDownAtMenuGliph = false;
                this.mouseDownAtCloseGliph = false;
                this.movePreview.Hide();
            }
        }

        private bool HandleTabDetach(MouseEventArgs e)
        {
            bool outside = this.IsMouseOutsideHeader(e.Location);
            if (outside && this.tabAtMouseDown != null)
            {
                this.FireTabItemDetach();
                return true;
            }

            return false;
        }

        private void FireTabItemDetach()
        {
            if (this.TabControlItemDetach != null)
            {
                var args = new TabControlItemChangedEventArgs(this.tabAtMouseDown, TabControlItemChangeTypes.Changed);
                this.TabControlItemDetach(args);
            }
        }

        private const int MOVE_TOLERANCE = 5;

        private bool IsMouseOutsideHeader(Point location)
        {
            bool outsideY = location.Y < -MOVE_TOLERANCE || (DEF_HEADER_HEIGHT + MOVE_TOLERANCE) < location.Y;
            bool outsideX = location.X < -MOVE_TOLERANCE || (this.Width + MOVE_TOLERANCE) < location.X;
            return outsideY || outsideX;
        }

        private bool HandleCloseButtonMouseUp(MouseEventArgs e)
        {
            if (this.mouseDownAtCloseGliph && this.MouseIsOnCloseButton(e))
            {
                this.CloseTab(this.SelectedItem);
                return true;
            }

            return false;
        }

        private bool HandleMenuGlipMouseUp(MouseEventArgs e)
        {
            if (this.mouseDownAtMenuGliph && this.MouseIsOnMenuGliph(e))
            {
                this.ShowTabsMenu();
                return true;
            }

            return false;
        }

        private void HandleTablItemMouseUpActions(MouseEventArgs e)
        {
            if (this.tabAtMouseDown != null)
            {
                TabControlItem upItem = this.GetTabItemByPoint(e.Location);
                if (upItem != null && upItem == this.tabAtMouseDown)
                    this.SelectedItem = upItem;
                else
                    this.SwapTabItems(e.X, upItem);
            }
        }

        private void SwapTabItems(int mouseX, TabControlItem upItem)
        {
            int downIndex = this.items.IndexOf(this.tabAtMouseDown);
            int newIndex = this.items.IndexOf(upItem);

            int upCentre = 48 + newIndex * 87;
            if (downIndex < newIndex)
            {
                newIndex--;
                upCentre += 10;
            }

            if (mouseX >= upCentre)
            {
                newIndex++;
            }

            if (newIndex > this.Items.Count - 1)
            {
                newIndex = this.Items.Count - 1;
            }
            if (newIndex <= 0)
            {
                newIndex = 0;
            }
            this.items.Remove(this.tabAtMouseDown);
            this.items.Insert(newIndex, this.tabAtMouseDown);
        }

        private bool mouseDownAtMenuGliph = false;
        private bool mouseDownAtCloseGliph = false;
        private Point mouseDownPoint;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mouseDownPoint = e.Location;
                this.tabAtMouseDown = GetTabItemByPoint(this.mouseDownPoint);

                if (this.MouseIsOnMenuGliph(e)) // Show Tabs menu
                    mouseDownAtMenuGliph = true;

                if (this.MouseIsOnCloseButton(e)) // close by click on close button
                    mouseDownAtCloseGliph = true;
            }
            
            if(!mouseDownAtCloseGliph && !mouseDownAtMenuGliph && this.tabAtMouseDown == null)
                base.OnMouseDown(e); // custom handling

            Invalidate();
        }

        private bool MouseIsOnMenuGliph(MouseEventArgs e)
        {
            return menuGlyph.Rect.Contains(e.Location);
        }

        private bool MouseIsOnCloseButton(MouseEventArgs e)
        {
            return closeButton.Rect.Contains(e.Location);
        }

        private void ShowTabsMenu()
        {
            HandledEventArgs args = new HandledEventArgs(false);
            this.OnMenuItemsLoading(args);
            if (!args.Handled)
            {
                this.OnMenuItemsLoad(EventArgs.Empty);
            }

            this.OnMenuShow();
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

        protected internal virtual void OnTabControlItemClosed(TabControlItem tabItem)
        {
            if (TabControlItemClosed != null)
            {
                var args = new TabControlItemClosedEventArgs() { Item = tabItem };
                TabControlItemClosed(this, args);
            }
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
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
            int nr = Items.Count;
            for (int i = 0; i < nr; i++)
            {
                TabControlItem item = this.Items[i];
                if (!item.Visible)
                    continue;

                ToolStripMenuItem tItem = new ToolStripMenuItem(item.Title);
                tItem.Tag = item;
                if (item.Selected)
                    tItem.Select();
                list.Add(tItem);
            }
            list.Sort(CompareSortText);
            menu.Items.AddRange(list.ToArray());
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

            this.HandleDrawMenuGliph(e);
            this.HandleDrawCloseButton(e);
            this.HandleMouseInTitle(e);
            this.HandlePreviewMove(e);
        }

        private TabPreview movePreview;

        private void HandlePreviewMove(MouseEventArgs e)
        {
            if (this.tabAtMouseDown != null)
            {
                this.UpdateMovePreviewLocation(e);
                this.ShowTabPreview(e);
            }
        }

        private void UpdateMovePreviewLocation(MouseEventArgs e)
        {
            Point newLocation = this.PointToScreen(e.Location);
            newLocation.X -= this.mouseDownPoint.X;
            newLocation.Y -= this.mouseDownPoint.Y;
            this.movePreview.Location = newLocation;
        }

        private void ShowTabPreview(MouseEventArgs e)
        {
            bool movedOutOfTolerance = this.MovedOutOfTolerance(e);
            if (movedOutOfTolerance)
            {
                bool toDetach = IsMouseOutsideHeader(e.Location);
                this.movePreview.UpdateDetachState(toDetach);
                this.movePreview.Show(this.tabAtMouseDown);
            }
        }

        private bool MovedOutOfTolerance(MouseEventArgs e)
        {
            int xDelta = Math.Abs(this.mouseDownPoint.X - e.Location.X);
            int yDelta = Math.Abs(this.mouseDownPoint.Y - e.Location.Y);
            bool movedOutOfTolerance = xDelta > MOVE_TOLERANCE || yDelta > MOVE_TOLERANCE;
            return movedOutOfTolerance;
        }

        private void HandleMouseInTitle(MouseEventArgs e)
        {
            TabControlItem item = this.GetTabItemByPoint(e.Location);
            if (item != null)
            {
                var inTitle = item.LocationIsInTitle(e.Location);
                TabControlMouseOnTitleEventArgs args = new TabControlMouseOnTitleEventArgs(item, e.Location);
                if (inTitle)
                {
                    //mouseWasOnTitle = true;
                    this.OnTabControlMouseOnTitle(args);
                    if (!this.mouseEnteredTitle)
                    {
                        this.mouseEnteredTitle = true;
                        this.OnTabControlMouseEnteredTitle(args);
                    }
                }
                else if (this.mouseEnteredTitle) // if (mouseWasOnTitle)
                {
                    //mouseWasOnTitle = false;
                    this.mouseEnteredTitle = false;
                    this.OnTabControlMouseLeftTitle(args);
                }
            }
        }

        private void HandleDrawMenuGliph(MouseEventArgs e)
        {
            if (this.MouseIsOnMenuGliph(e))
            {
                this.menuGlyph.IsMouseOver = true;
                this.Invalidate(this.menuGlyph.Rect);
            }
            else
            {
                if (this.menuGlyph.IsMouseOver && !this.menuOpen)
                {
                    this.menuGlyph.IsMouseOver = false;
                    this.Invalidate(this.menuGlyph.Rect);
                }
            }
        }

        private void HandleDrawCloseButton(MouseEventArgs e)
        {
            if (this.MouseIsOnCloseButton(e))
            {
                this.closeButton.IsMouseOver = true;
                this.Invalidate(this.closeButton.Rect);
            }
            else
            {
                if (this.closeButton.IsMouseOver)
                {
                    this.closeButton.IsMouseOver = false;
                    this.Invalidate(this.closeButton.Rect);
                }
            }
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

        internal void OnDrawTabPage(Graphics g, TabControlItem currentItem)
        {
            bool isFirstTab = Items.IndexOf(currentItem) == 0;
            Font currentFont = Font;

           if (currentItem == SelectedItem)
                currentFont = new Font(Font, FontStyle.Bold);

            SizeF textSize = g.MeasureString(currentItem.Title, currentFont, new SizeF(200, 10), sf);
            textSize.Width += 20;
            RectangleF buttonRect = currentItem.StripRect;

            GraphicsPath path = new GraphicsPath();
            LinearGradientBrush brush = null;
            int mtop = 2;

            #region Draw Not Right-To-Left Tab

            if (RightToLeft == RightToLeft.No)
            {
                if (currentItem == SelectedItem || isFirstTab)
                {
                    path.AddLine(buttonRect.Left - 10, buttonRect.Bottom - 0, buttonRect.Left + (buttonRect.Height / 2) - 4, mtop + 0);
                }
                else
                {
                    path.AddLine(buttonRect.Left, buttonRect.Bottom - 1, buttonRect.Left, buttonRect.Bottom - (buttonRect.Height / 2) - 2);
                    path.AddLine(buttonRect.Left, buttonRect.Bottom - (buttonRect.Height / 2) - 3, buttonRect.Left + (buttonRect.Height / 2) - 4, mtop + 3);
                }

                path.AddLine(buttonRect.Left + (buttonRect.Height / 2) + 2, mtop, buttonRect.Right - 3, mtop);
                path.AddLine(buttonRect.Right, mtop + 2, buttonRect.Right, buttonRect.Bottom - 1);
                path.AddLine(buttonRect.Right - 4, buttonRect.Bottom - 0, buttonRect.Left, buttonRect.Bottom - 0);
                path.CloseFigure();
                try {
                    if(currentItem == SelectedItem) {
                        brush = new LinearGradientBrush(buttonRect, SystemColors.GrayText, SystemColors.Window, LinearGradientMode.Vertical);
                    } else {
                        brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical);
                    }
                }catch(Exception) {
                    
                }
                g.FillPath(brush, path);
                Pen pen = SystemPens.ControlDark;
                if (currentItem == SelectedItem)
                {
                    pen = new Pen(ToolStripRenderer.ColorTable.MenuStripGradientBegin);
                    g.DrawPath(pen, path);
                }
                g.DrawPath(SystemPens.ControlDark, path);
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
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), textRect, sf);
                }
                else
                {
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), textRect, sf);
                }
            }

            #endregion

            #region Draw Right-To-Left Tab

            if (RightToLeft == RightToLeft.Yes)
            {
                if (currentItem == SelectedItem || isFirstTab)
                {
                    path.AddLine(buttonRect.Right + 10, buttonRect.Bottom - 0, buttonRect.Right - (buttonRect.Height / 2) + 4, mtop + 0);
                }
                else
                {
                    path.AddLine(buttonRect.Right, buttonRect.Bottom - 1, buttonRect.Right, buttonRect.Bottom - (buttonRect.Height / 2) - 2);
                    path.AddLine(buttonRect.Right, buttonRect.Bottom - (buttonRect.Height / 2) - 3, buttonRect.Right - (buttonRect.Height / 2) + 4, mtop + 3);
                }

                path.AddLine(buttonRect.Right - (buttonRect.Height / 2) - 2, mtop, buttonRect.Left + 3, mtop);
                path.AddLine(buttonRect.Left, mtop + 2, buttonRect.Left, buttonRect.Bottom - 1);
                path.AddLine(buttonRect.Left + 4, buttonRect.Bottom - 0, buttonRect.Right, buttonRect.Bottom - 0);
                path.CloseFigure();

                if (currentItem == SelectedItem)
                {
                    brush = new LinearGradientBrush(buttonRect, SystemColors.GrayText, SystemColors.Window, LinearGradientMode.Vertical);
                }
                else
                {
                    brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical);
                }

                g.FillPath(brush, path);
                Pen pen = SystemPens.ControlDark;
                if (currentItem == SelectedItem)
                {
                    pen = new Pen(ToolStripRenderer.ColorTable.MenuStripGradientBegin);
                    g.DrawPath(pen, path);
                }
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
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), textRect, sf);
                }
                else
                {
                    g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), textRect, sf);
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
            this.movePreview = new TabPreview(this);

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

        private static int CompareSortText(ToolStripMenuItem x, ToolStripMenuItem y)
        {
            return x.Text.CompareTo(y.Text);
        }

    }
}
