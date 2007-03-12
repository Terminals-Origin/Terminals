// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Controls
{
	public class TabGroupLeaf : TabGroupBase
	{
	    // Class constants
	    protected const int _imageWidth = 16;
        protected const int _imageHeight = 16;
        protected const int _imageHorzSplit = 0;
        protected const int _imageVertSplit = 1;
	
        // Class state
        protected static ImageList _internalImages;
        
        // Instance fields
	    protected MenuCommand _mcClose;
        protected MenuCommand _mcSep1;
        protected MenuCommand _mcProm;
        protected MenuCommand _mcReba;
        protected MenuCommand _mcSep2;
        protected MenuCommand _mcPrev;
        protected MenuCommand _mcNext;
        protected MenuCommand _mcVert;
        protected MenuCommand _mcHorz;
        protected Cursor _savedCursor;
        protected bool _dragEntered;
        protected TargetManager _targetManager;
        protected Controls.TabControl _tabControl;

        static TabGroupLeaf()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _internalImages = ResourceHelper.LoadBitmapStrip(Type.GetType("Crownwood.Magic.Controls.TabbedGroups"),
                                                             "Crownwood.Magic.Resources.ImagesTabbedGroups.bmp",
                                                             new Size(_imageWidth, _imageHeight),
                                                             new Point(0,0));
        }
	
		public TabGroupLeaf(TabbedGroups tabbedGroups, TabGroupBase parent)
		    : base(tabbedGroups, parent)
		{
		    // Create our managed tab control instance
		    _tabControl = new Controls.TabControl();
		    
		    // We need page drag to begin when mouse dragged a small distance
		    _tabControl.DragFromControl = false;
		    
		    // We need to monitor attempts to drag into the tab control
            _dragEntered = false;
            _tabControl.AllowDrop = true;
            _tabControl.DragDrop += new DragEventHandler(OnControlDragDrop);
            _tabControl.DragEnter += new DragEventHandler(OnControlDragEnter);
            _tabControl.DragLeave += new EventHandler(OnControlDragLeave);
		    
		    // Need notification when page drag begins
            _tabControl.PageDragStart += new MouseEventHandler(OnPageDragStart);
            _tabControl.PageDragMove += new MouseEventHandler(OnPageDragMove);
            _tabControl.PageDragEnd += new MouseEventHandler(OnPageDragEnd);
            _tabControl.PageDragQuit += new MouseEventHandler(OnPageDragQuit);
		    
		    // Hook into tab page collection events
            _tabControl.TabPages.Cleared += new CollectionClear(OnTabPagesCleared);
            _tabControl.TabPages.Inserted += new CollectionChange(OnTabPagesInserted);
            _tabControl.TabPages.Removed += new CollectionChange(OnTabPagesRemoved);
            
            // Hook into page level events
            _tabControl.GotFocus += new EventHandler(OnGainedFocus);
            _tabControl.PageGotFocus += new EventHandler(OnGainedFocus);
            _tabControl.ClosePressed += new EventHandler(OnClose);            
            
            // Manager only created at start of drag operation
            _targetManager = null;
            
            DefinePopupMenuForControl(_tabControl);

            // Setup the correct 'HideTabsMode' for the control
            Notify(TabGroupBase.NotifyCode.DisplayTabMode);

		    // Define the default setup of TabControl and allow developer to customize
		    _tabbedGroups.OnTabControlCreated(_tabControl);
		}

        protected void DefinePopupMenuForControl(Controls.TabControl tabControl)
        {
            PopupMenu pm = new PopupMenu();
            
            // Add all the standard menus we manage
            _mcClose = new MenuCommand("", new EventHandler(OnClose));
            _mcSep1 = new MenuCommand("-");
            _mcProm = new MenuCommand("", new EventHandler(OnToggleProminent));
            _mcReba = new MenuCommand("", new EventHandler(OnRebalance));
            _mcSep2 = new MenuCommand("-");
            _mcHorz = new MenuCommand("", _internalImages, _imageHorzSplit, new EventHandler(OnNewVertical));
            _mcVert = new MenuCommand("", _internalImages, _imageVertSplit, new EventHandler(OnNewHorizontal));
            _mcNext = new MenuCommand("", new EventHandler(OnMoveNext));
            _mcPrev = new MenuCommand("", new EventHandler(OnMovePrevious));

            // Prominent is a radio checked item
            _mcProm.RadioCheck = true;

            // Use the provided context menu
            tabControl.ContextPopupMenu = pm;

            // Update command states when shown
            tabControl.PopupMenuDisplay += new CancelEventHandler(OnPopupMenuDisplay);
        }
    
        public TabPageCollection TabPages
        {
            get { return _tabControl.TabPages; }
        }
    
        public override void Notify(NotifyCode code)
        {
            switch(code)
            {
                case NotifyCode.ImageListChanging:
                    // Are we using the group level imagelist?
                    if (_tabbedGroups.ImageList == _tabControl.ImageList)
                    {   
                        // Then remove its use
                        _tabControl.ImageList = null;
                    }
                    break;
                case NotifyCode.ImageListChanged:
                    // If no imagelist defined
                    if (_tabControl.ImageList == null)
                    {   
                        // Then use the group level one
                        _tabControl.ImageList = _tabbedGroups.ImageList;
                    }
                    break;
                case NotifyCode.StyleChanged:
                    // Update tab control with new style
                    _tabControl.Style = _tabbedGroups.Style;
                    break;
                case NotifyCode.DisplayTabMode:
                    // Apply the latest mode
                    switch(_tabbedGroups.DisplayTabMode)
                    {
                        case Crownwood.Magic.Controls.TabbedGroups.DisplayTabModes.ShowAll:
                            _tabControl.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.ShowAlways;
                            break;
                        case Crownwood.Magic.Controls.TabbedGroups.DisplayTabModes.HideAll:
                            _tabControl.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.HideAlways;
                            break;
                        case Crownwood.Magic.Controls.TabbedGroups.DisplayTabModes.ShowActiveLeaf:
                            _tabControl.HideTabsMode = (_tabbedGroups.ActiveLeaf == this ? Magic.Controls.TabControl.HideTabsModes.ShowAlways :
                                                                                           Magic.Controls.TabControl.HideTabsModes.HideAlways);
                            break;
                        case Crownwood.Magic.Controls.TabbedGroups.DisplayTabModes.ShowMouseOver:
                            _tabControl.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.HideWithoutMouse;
                            break;
                        case Crownwood.Magic.Controls.TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver:
                            _tabControl.HideTabsMode = (_tabbedGroups.ActiveLeaf == this ? Magic.Controls.TabControl.HideTabsModes.ShowAlways :
                                                                                           Magic.Controls.TabControl.HideTabsModes.HideWithoutMouse);
                            break;
                    }
                    break;
            }
        }

        public override int Count               { get { return _tabControl.TabPages.Count; } }
        public override bool IsLeaf             { get { return true; } }
        public override bool IsSequence         { get { return false; } }
        public override Control GroupControl    { get { return _tabControl; } }
        
        public override bool ContainsProminent(bool recurse)
        {
            // Cache the currently selected prominent group
            TabGroupLeaf prominent = _tabbedGroups.ProminentLeaf;

            // Valid value to test against?            
            if (prominent != null)
                return (this == prominent);
            else
                return false;
        }

        public override void SaveToXml(XmlTextWriter xmlOut)
        {
            // Output standard values appropriate for all Sequence instances
            xmlOut.WriteStartElement("Leaf");
            xmlOut.WriteAttributeString("Count", Count.ToString());
            xmlOut.WriteAttributeString("Unique", _unique.ToString());
            xmlOut.WriteAttributeString("Space", _space.ToString());

            // Output each tab page
            foreach(Controls.TabPage tp in _tabControl.TabPages)
            {
                string controlType = "null";
                
                if (tp.Control != null)
                    controlType = tp.Control.GetType().AssemblyQualifiedName;

                xmlOut.WriteStartElement("Page");
                xmlOut.WriteAttributeString("Title", tp.Title);
                xmlOut.WriteAttributeString("ImageList", (tp.ImageList != null).ToString());
                xmlOut.WriteAttributeString("ImageIndex", tp.ImageIndex.ToString());
                xmlOut.WriteAttributeString("Selected", tp.Selected.ToString());
                xmlOut.WriteAttributeString("Control", controlType);

                // Give handlers chance to reconstruct the page
                xmlOut.WriteStartElement("CustomPageData");
                _tabbedGroups.OnPageSaving(new TGPageSavingEventArgs(tp, xmlOut));
                xmlOut.WriteEndElement();

                xmlOut.WriteEndElement();
            }
                
            xmlOut.WriteEndElement();
        }

        public override void LoadFromXml(XmlTextReader xmlIn)
        {
            // Grab the expected attributes
            string rawCount = xmlIn.GetAttribute(0);
            string rawUnique = xmlIn.GetAttribute(1);
            string rawSpace = xmlIn.GetAttribute(2);

            // Convert to correct types
            int count = Convert.ToInt32(rawCount);
            int unique = Convert.ToInt32(rawUnique);
            Decimal space = Convert.ToDecimal(rawSpace);
            
            // Update myself with new values
            _unique = unique;
            _space = space;
            
            // Load each of the children
            for(int i=0; i<count; i++)
            {
                // Read to the first page element or 
                if (!xmlIn.Read())
                    throw new ArgumentException("An element was expected but could not be read in");

                // Must always be a page instance
                if (xmlIn.Name == "Page")
                {
                    Controls.TabPage tp = new Controls.TabPage();

                    // Grab the expected attributes
                    string title = xmlIn.GetAttribute(0);
                    string rawImageList = xmlIn.GetAttribute(1);
                    string rawImageIndex = xmlIn.GetAttribute(2);
                    string rawSelected = xmlIn.GetAttribute(3);
                    string controlType = xmlIn.GetAttribute(4);

                    // Convert to correct types
                    bool imageList = Convert.ToBoolean(rawImageList);
                    int imageIndex = Convert.ToInt32(rawImageIndex);
                    bool selected = Convert.ToBoolean(rawSelected);

                    // Setup new page instance
                    tp.Title = title;
                    tp.ImageIndex = imageIndex;
                    tp.Selected = selected;
                    
                    if (imageList)
                        tp.ImageList = _tabbedGroups.ImageList;
                    
                    // Is there a type description of required control?
                    if (controlType != "null")
                    {
                        try
                        {
                            // Get type description, if possible
                            Type t = Type.GetType(controlType);
                            
                            // Was a valid, known type?
                            if (t != null)
                            {
                                // Get the assembly this type is defined inside
                                Assembly a = t.Assembly;
                                
                                if (a != null)
                                {
                                    // Create a new instance form the assemnly
                                    object newObj = a.CreateInstance(t.FullName);
                                    
                                    Control newControl = newObj as Control;
                                    
                                    // Must derive from Control to be of use to us
                                    if (newControl != null)
                                        tp.Control = newControl;
                                }
                            }
                        }
                        catch
                        {
                            // We ignore failure to recreate given control type
                        }
                    }
                    
                    // Read to the custom data element
                    if (!xmlIn.Read())
                        throw new ArgumentException("An element was expected but could not be read in");

                    if (xmlIn.Name != "CustomPageData")
                        throw new ArgumentException("Expected 'CustomPageData' element was not found");

                    bool finished = xmlIn.IsEmptyElement;

                    TGPageLoadingEventArgs e = new TGPageLoadingEventArgs(tp, xmlIn);

                    // Give handlers chance to reconstruct per-page information
                    _tabbedGroups.OnPageLoading(e);
                    
                    // Add into the control unless handler cancelled add operation
                    if (!e.Cancel)
                        _tabControl.TabPages.Add(tp);
                    
                    // Read everything until we get the end of custom data marker
                    while(!finished)
                    {
                        // Check it has the expected name
                        if (xmlIn.NodeType == XmlNodeType.EndElement)
                            finished = (xmlIn.Name == "CustomPageData");

                        if (!finished)
                        {
                            if (!xmlIn.Read())
                                throw new ArgumentException("An element was expected but could not be read in");
                        }
                    } 

                    // Read past the end of page element                    
                    if (!xmlIn.Read())
                        throw new ArgumentException("An element was expected but could not be read in");

                    // Check it has the expected name
                    if (xmlIn.NodeType != XmlNodeType.EndElement)
                        throw new ArgumentException("End of 'page' element expected but missing");
                    
                }
                else
                    throw new ArgumentException("Unknown element was encountered");
            }
        }

        protected void OnGainedFocus(object sender, EventArgs e)
        {
            // This tab control has the focus, make it the active leaf
            _tabbedGroups.ActiveLeaf = this;
        }

        protected void OnTabPagesCleared()
        {
            // All pages removed, do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }

        protected void OnTabPagesInserted(int index, object value)
        {
            // If there is no currently active leaf then make it us
            if (_tabbedGroups.ActiveLeaf == null)
                _tabbedGroups.ActiveLeaf = this;

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }

        protected void OnTabPagesRemoved(int index, object value)
        {
            if (_tabControl.TabPages.Count == 0)
            {
                // All pages removed, do we need to compact?
                if (_tabbedGroups.AutoCompact)
                    _tabbedGroups.Compact();
            }

            // Mark layout as dirty
            if (_tabbedGroups.AutoCalculateDirty)
                _tabbedGroups.Dirty = true;
        }
        
        protected void OnPopupMenuDisplay(object sender, CancelEventArgs e)
        {
            // Remove all existing menu items
            _tabControl.ContextPopupMenu.MenuCommands.Clear();
            
            // Add our standard set of menus
            _tabControl.ContextPopupMenu.MenuCommands.AddRange(new MenuCommand[]{_mcClose, _mcSep1, 
                                                                                 _mcProm, _mcReba, 
                                                                                 _mcSep2, _mcHorz, 
                                                                                 _mcVert, _mcNext, 
                                                                                 _mcPrev});
        
            // Are any pages selected
            bool valid = (_tabControl.SelectedIndex != -1);
        
            // Define the latest text string
            _mcClose.Text = _tabbedGroups.CloseMenuText;
            _mcProm.Text = _tabbedGroups.ProminentMenuText;
            _mcReba.Text = _tabbedGroups.RebalanceMenuText;
            _mcPrev.Text = _tabbedGroups.MovePreviousMenuText;
            _mcNext.Text = _tabbedGroups.MoveNextMenuText;
            _mcVert.Text = _tabbedGroups.NewVerticalMenuText;
            _mcHorz.Text = _tabbedGroups.NewHorizontalMenuText;
            
            // Only need to close option if the tab has close defined
            _mcClose.Visible = _tabControl.ShowClose && valid;
            _mcSep1.Visible = _tabControl.ShowClose && valid;
            
            // Update the radio button for prominent
            _mcProm.Checked = (_tabbedGroups.ProminentLeaf == this);
            
            // Can only create new group if at least two pages exist
            bool split = valid && (_tabControl.TabPages.Count > 1);

            bool vertSplit = split;
            bool horzSplit = split;
            
            TabGroupSequence tgs = _parent as TabGroupSequence;

            // If we are not the only leaf, then can only split in 
            // the same direction as the group we are in
            if (tgs.Count > 1)
            {
                if (tgs.Direction == Direction.Vertical)
                   vertSplit = false;
                else
                   horzSplit = false;
            }
            
            _mcVert.Visible = vertSplit;
            _mcHorz.Visible = horzSplit;

            // Can only how movement if group exists in that direction
            _mcNext.Visible = valid && (_tabbedGroups.NextLeaf(this) != null);
            _mcPrev.Visible = valid && (_tabbedGroups.PreviousLeaf(this) != null);

            TGContextMenuEventArgs tge = new TGContextMenuEventArgs(this, 
                                                                    _tabControl, 
                                                                    _tabControl.SelectedTab,
                                                                    _tabControl.ContextPopupMenu);
        
            // Generate event so handlers can add/remove/cancel menu
            _tabbedGroups.OnPageContextMenu(tge);
            
            // Pass back cancel value
            e.Cancel = tge.Cancel;
        }
        
        internal void OnClose(object sender, EventArgs e)
        {
            TGCloseRequestEventArgs tge = new TGCloseRequestEventArgs(this, _tabControl, _tabControl.SelectedTab);
        
            // Generate event so handlers can perform appropriate action
            _tabbedGroups.OnPageCloseRequested(tge);
            
            // Still want to close the page?
            if (!tge.Cancel)
                _tabControl.TabPages.Remove(_tabControl.SelectedTab);
        }
        
        internal void OnToggleProminent(object sender, EventArgs e)
        {
            // Toggel the prominent mode
            if (_tabbedGroups.ProminentLeaf == this)
                _tabbedGroups.ProminentLeaf = null;
            else
                _tabbedGroups.ProminentLeaf = this;
        }

        internal void OnRebalance(object sender, EventArgs e)
        {
            _tabbedGroups.Rebalance();
        }
            
        internal void OnMovePrevious(object sender, EventArgs e)
        {
            // Find the previous leaf node
            TabGroupLeaf prev = _tabbedGroups.PreviousLeaf(this);
            
            // Must always be valid!
            if (prev != null)           
                MovePageToLeaf(prev);
        }

        internal void OnMoveNext(object sender, EventArgs e)
        {
            // Find the previous leaf node
            TabGroupLeaf next = _tabbedGroups.NextLeaf(this);
            
            // Must always be valid!
            if (next != null)           
                MovePageToLeaf(next);
        }

        internal void OnNewVertical(object sender, EventArgs e)
        {
            NewVerticalGroup(this, false);
        }

        protected void OnNewHorizontal(object sender, EventArgs e)
        {
            NewHorizontalGroup(this, false);    
        }

        internal void NewVerticalGroup(TabGroupLeaf sourceLeaf, bool before)
        {
            TabGroupSequence tgs = this.Parent as TabGroupSequence;
        
            // We must have a parent sequence!
            if (tgs != null)
            {
                tgs.Direction = Direction.Vertical;
                AddGroupToSequence(tgs, sourceLeaf, before);
            }
        }
        
        internal void NewHorizontalGroup(TabGroupLeaf sourceLeaf, bool before)
        {
            TabGroupSequence tgs = this.Parent as TabGroupSequence;
        
            // We must have a parent sequence!
            if (tgs != null)
            {
                tgs.Direction = Direction.Horizontal;
                AddGroupToSequence(tgs, sourceLeaf, before);
            }
        }
        
        internal void MovePageToLeaf(TabGroupLeaf leaf)
        {
            // Remember original auto compact mode
            bool autoCompact = _tabbedGroups.AutoCompact;

            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;

            // Get the requested tab page to be moved to new leaf
            TabPage tp = _tabControl.SelectedTab;

            // Remove page from ourself
            _tabControl.TabPages.Remove(tp);
            
            // Add into the new leaf
            leaf.TabPages.Add(tp);

            // Make new leaf the active one
            _tabbedGroups.ActiveLeaf = leaf;
                
            TabControl tc = leaf.GroupControl as Controls.TabControl;
                
            // Select the newly added page
            tc.SelectedTab = tp;

            // Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;
            
            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();
        }

        protected void AddGroupToSequence(TabGroupSequence tgs, TabGroupLeaf sourceLeaf, bool before)
        {
            // Remember original auto compact mode
            bool autoCompact = _tabbedGroups.AutoCompact;

            // Turn mode off as it interferes with reorganisation
            _tabbedGroups.AutoCompact = false;

            // Find our index into parent collection
            int pos = tgs.IndexOf(this);
                
            TabGroupLeaf newGroup = null;

            // New group inserted before existing one?                
            if (before)
                newGroup = tgs.InsertNewLeaf(pos);
            else
            {
                // No, are we at the end of the collection?
                if (pos == (tgs.Count - 1))
                    newGroup = tgs.AddNewLeaf();
                else
                    newGroup = tgs.InsertNewLeaf(pos + 1);
            }
                     
            // Get tab control for source leaf
            Controls.TabControl tc = sourceLeaf.GroupControl as Controls.TabControl;
                        
            TabPage tp = tc.SelectedTab;

            // Remove page from ourself
            tc.TabPages.Remove(tp);
                    
            // Add into the new leaf
            newGroup.TabPages.Add(tp);

            // Reset compacting mode as we have updated the structure
            _tabbedGroups.AutoCompact = autoCompact;

            // Do we need to compact?
            if (_tabbedGroups.AutoCompact)
                _tabbedGroups.Compact();
        }
        
        protected void OnPageDragStart(object sender, MouseEventArgs e)
        {
            // Save the current cursor value
            _savedCursor = _tabControl.Cursor;
            
            // Manager will create hot zones and draw dragging rectangle
            _targetManager = new TargetManager(_tabbedGroups, this, _tabControl);
        }
 
        protected void OnPageDragMove(object sender, MouseEventArgs e)
        {
            // Convert from Control coordinates to screen coordinates
            Point mousePos = _tabControl.PointToScreen(new Point(e.X, e.Y));

            // Let manager decide on drawing rectangles and setting cursor
            _targetManager.MouseMove(mousePos);
        }

        protected void OnPageDragEnd(object sender, MouseEventArgs e)
        {
            // Give manager chance to action request and cleanup
            _targetManager.Exit();

            // No longer need the manager
            _targetManager = null;
            
            // Restore the original cursor
            _tabControl.Cursor = _savedCursor;
        }

        protected void OnPageDragQuit(object sender, MouseEventArgs e)
        {
            // Give manager chance to cleanup
            _targetManager.Quit();
        
            // No longer need the manager
            _targetManager = null;

            // Restore the original cursor
            _tabControl.Cursor = _savedCursor;
        }

        
        protected void OnControlDragEnter(object sender, DragEventArgs drgevent)
        {
            _dragEntered = ValidFormat(drgevent);
        
            // Do we allow the drag to occur?
            if (_dragEntered)
            {
                // Must draw a drag indicator
                DrawDragIndicator();
                
                // Update the allowed effects
                drgevent.Effect = DragDropEffects.Copy;
            }
        }

        protected void OnControlDragDrop(object sender, DragEventArgs drgevent)
        {
            // Do we allow the drop to occur?
            if (_dragEntered)
            {
                // Must remove the drag indicator
                DrawDragIndicator();
                
                // Generate an event so caller can perform required action
                _tabbedGroups.OnExternalDrop(this, _tabControl, GetDragProvider(drgevent));
            }

            _dragEntered = false;
        }

        protected void OnControlDragLeave(object sender, EventArgs e)
        {
            // Do we need to remove the drag indicator?
            if (_dragEntered)
                DrawDragIndicator();
                
            _dragEntered = false;
        }
        
        protected bool ValidFormat(DragEventArgs e)
        {
            return e.Data.GetDataPresent(typeof(TabbedGroups.DragProvider));
        }
        
        protected TabbedGroups.DragProvider GetDragProvider(DragEventArgs e)
        {
            return (TabbedGroups.DragProvider)e.Data.GetData(typeof(TabbedGroups.DragProvider));
        }
        
        protected void DrawDragIndicator()
        {
            // Create client rectangle
            Rectangle clientRect = new Rectangle(new Point(0,0), _tabControl.ClientSize);
            
            // Draw drag indicator around whole control
            TargetManager.DrawDragRectangle(_tabControl.RectangleToScreen(clientRect));
        }
    }
}
