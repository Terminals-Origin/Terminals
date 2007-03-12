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
using System.Text;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Controls
{
    [ToolboxBitmap(typeof(TabbedGroups))]
    public class TabbedGroups : UserControl, ISupportInitialize, IMessageFilter
	{
	    public class DragProvider
	    {
            protected object _tag;
            
            public DragProvider()
            {
                _tag = null;
            }
            
            public DragProvider(object tag)
            {
                _tag = tag;
            }
            
            public object Tag
            {
                get { return _tag; }
                set { _tag = value; }
            }
	    }
	
	    public enum DisplayTabModes
	    {
            HideAll,
            ShowAll,
            ShowActiveLeaf,
            ShowMouseOver,
            ShowActiveAndMouseOver
	    }
	
	    public enum CompactFlags
	    {
	        RemoveEmptyTabLeaf = 1,
	        RemoveEmptyTabSequence = 2,
	        ReduceSingleEntries = 4,
	        ReduceSameDirection = 8,
	        All = 15
	    }
	
	    // Instance fields
	    protected int _numLeafs;
        protected int _defMinWidth;
        protected int _defMinHeight;
        protected string _closeMenuText;
        protected string _prominentMenuText;
        protected string _rebalanceMenuText;
        protected string _movePreviousMenuText;
        protected string _moveNextMenuText;
        protected string _newVerticalMenuText;
        protected string _newHorizontalMenuText;
        protected ImageList _imageList;
        protected bool _dirty;
        protected bool _autoCalculateDirty;
        protected bool _saveControls;
        protected bool _initializing;
        protected bool _atLeastOneLeaf;
        protected bool _autoCompact;
        protected bool _compacting;
        protected bool _resizeBarLock;
        protected int _resizeBarVector;
        protected Color _resizeBarColor;
        protected Shortcut _closeShortcut;
        protected Shortcut _prominentShortcut;
        protected Shortcut _rebalanceShortcut;
        protected Shortcut _movePreviousShortcut;
        protected Shortcut _moveNextShortcut;
        protected Shortcut _splitVerticalShortcut;
        protected Shortcut _splitHorizontalShortcut;
        protected Shortcut _nextTabShortcut;
        protected CompactFlags _compactOptions;
        protected DisplayTabModes _displayTabMode;
        protected TabGroupLeaf _prominentLeaf;
        protected TabGroupLeaf _activeLeaf;
        protected TabGroupSequence _root;
        protected VisualStyle _style;
	
	    // Delegates for events
	    public delegate void TabControlCreatedHandler(TabbedGroups tg, Controls.TabControl tc);
	    public delegate void PageCloseRequestHandler(TabbedGroups tg, TGCloseRequestEventArgs e);
        public delegate void PageContextMenuHandler(TabbedGroups tg, TGContextMenuEventArgs e);
        public delegate void GlobalSavingHandler(TabbedGroups tg, XmlTextWriter xmlOut);
        public delegate void GlobalLoadingHandler(TabbedGroups tg, XmlTextReader xmlIn);
        public delegate void PageSavingHandler(TabbedGroups tg, TGPageSavingEventArgs e);
        public delegate void PageLoadingHandler(TabbedGroups tg, TGPageLoadingEventArgs e);
        public delegate void ExternalDropHandler(TabbedGroups tg, TabGroupLeaf tgl, Controls.TabControl tc, DragProvider dp);
	
	    // Instance events
	    public event TabControlCreatedHandler TabControlCreated;
	    public event PageCloseRequestHandler PageCloseRequest;
        public event PageContextMenuHandler PageContextMenu;
        public event GlobalSavingHandler GlobalSaving;
        public event GlobalLoadingHandler GlobalLoading;
        public event PageSavingHandler PageSaving;
        public event PageLoadingHandler PageLoading;
        public event EventHandler ProminentLeafChanged;
        public event EventHandler ActiveLeafChanged;
        public event EventHandler DirtyChanged;
        public event ExternalDropHandler ExternalDrop;
	
        public TabbedGroups()
        {
            InternalConstruct(VisualStyle.IDE);
        }
            
        public TabbedGroups(VisualStyle style)
		{
		    InternalConstruct(style);
        }
        
        protected void InternalConstruct(VisualStyle style)
		{
            // Prevent flicker with double buffering and all painting inside WM_PAINT
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		
		    // We want to act as a drop target
		    this.AllowDrop = true;
		
		    // Remember parameters
		    _style = style;
		    
		    // Define initial state
		    _numLeafs = 0;
		    _compacting = false;
		    _initializing = false;
		    
            // Create the root sequence that always exists
            _root = new TabGroupSequence(this);
		    
            // Define default settings
		    ResetProminentLeaf();
		    ResetResizeBarVector();
		    ResetResizeBarColor();
		    ResetResizeBarLock();
		    ResetCompactOptions();
		    ResetDefaultGroupMinimumWidth();
            ResetDefaultGroupMinimumHeight();
            ResetActiveLeaf();
            ResetAutoCompact();
            ResetAtLeastOneLeaf();
            ResetCloseMenuText();
            ResetProminentMenuText();
            ResetRebalanceMenuText();
            ResetMovePreviousMenuText();
            ResetMoveNextMenuText();
            ResetNewVerticalMenuText();
            ResetNewHorizontalMenuText();
            ResetCloseShortcut();
            ResetProminentShortcut();
            ResetRebalanceShortcut();
            ResetMovePreviousShortcut();
            ResetMoveNextShortcut();
            ResetSplitVerticalShortcut();
            ResetSplitHorizontalShortcut();
            ResetImageList();
            ResetDisplayTabMode();
            ResetSaveControls();
            ResetAutoCalculateDirty();
            ResetDirty();
            
            // Add ourself to the application filtering list 
            // (to snoop for shortcut combinations)
            Application.AddMessageFilter(this);
            
        }
        
        [Category("TabbedGroups")]
        [DefaultValue(typeof(VisualStyle), "IDE")]
        public VisualStyle Style
        {
            get { return _style; }
            
            set
            {
                if (_style != value)
                {   
                    _style = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.StyleChanged);
                }
            }
        }

        public void ResetStyle()
        {
            Style = VisualStyle.IDE;
        }
        
        [Browsable(false)]
        public TabGroupSequence RootSequence
        {
            get { return _root; }
        }

        [Category("TabbedGroups")]
        [DefaultValue(-1)]
        public int ResizeBarVector
        {
            get { return _resizeBarVector; }
            
            set
            {
                if (_resizeBarVector != value)
                {
                    _resizeBarVector = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ResizeBarVectorChanged);
                }
            }
        }
        
        public void ResetResizeBarVector()
        {
            ResizeBarVector = -1;
        }
        
        [Category("TabbedGroups")]
        public Color ResizeBarColor
        {
            get { return _resizeBarColor; }
            
            set
            {
                if (!_resizeBarColor.Equals(value))
                {
                    _resizeBarColor = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ResizeBarColorChanged);
                }
            }
        }

        protected bool ShouldSerializeResizeBackColor()
        {
            return _resizeBarColor != base.BackColor;
        }
        
        public void ResetResizeBarColor()
        {
            ResizeBarColor = base.BackColor;
        }
        
        [Category("TabbedGroups")]
        [DefaultValue(false)]
        public bool ResizeBarLock
        {
            get { return _resizeBarLock; }
            set { _resizeBarLock = value; }
        }
        
        public void ResetResizeBarLock()
        {
            ResizeBarLock = false;
        }
        
        [Category("TabbedGroups")]
        public TabGroupLeaf ProminentLeaf
        {
            get { return _prominentLeaf; }
            
            set
            {
                if (_prominentLeaf != value)
                {
                    _prominentLeaf = value;

                    // Mark layout as dirty
                    if (_autoCalculateDirty)
                        _dirty = true;

                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ProminentChanged);
                    
                    OnProminentLeafChanged(EventArgs.Empty);
                }
            }
        }
        
        public void ResetProminentLeaf()
        {
            ProminentLeaf = null;
        }
        
        [Category("TabbedGroups")]
        [DefaultValue(typeof(CompactFlags), "All")]
        public CompactFlags CompactOptions
        {
            get { return _compactOptions; }
            set { _compactOptions = value; }
        }

        public void ResetCompactOptions()
        {
            CompactOptions = CompactFlags.All;
        }
        
        [Category("TabbedGroups")]
        [DefaultValue(4)]
        public int DefaultGroupMinimumWidth 
        {
            get { return _defMinWidth; }
            
            set
            {
                if (_defMinWidth != value)
                {
                    _defMinWidth = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MinimumSizeChanged);
                }
            }
        }        
        
        public void ResetDefaultGroupMinimumWidth()
        {
            DefaultGroupMinimumWidth = 4;
        }
        
        [Category("TabbedGroups")]
        [DefaultValue(4)]
        public int DefaultGroupMinimumHeight
        {
            get { return _defMinHeight; }
            
            set
            {
                if (_defMinHeight != value)
                {
                    _defMinHeight = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.MinimumSizeChanged);
                }
            }
        }        
        
        public void ResetDefaultGroupMinimumHeight()
        {
            DefaultGroupMinimumHeight = 4;
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("&Close")]
        public string CloseMenuText
        {
            get { return _closeMenuText; }
            set { _closeMenuText = value; }
        }
        
        public void ResetCloseMenuText()
        {
            CloseMenuText = "&Close";
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("Pro&minent")]
        public string ProminentMenuText
        {
            get { return _prominentMenuText; }
            set { _prominentMenuText = value; }
        }

        public void ResetProminentMenuText()
        {
            ProminentMenuText = "Pro&minent";
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("&Rebalance")]
        public string RebalanceMenuText
        {
            get { return _rebalanceMenuText; }
            set { _rebalanceMenuText = value; }
        }

        public void ResetRebalanceMenuText()
        {
            RebalanceMenuText = "&Rebalance";
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("Move to &Previous Tab Group")]
        public string MovePreviousMenuText
        {
            get { return _movePreviousMenuText; }
            set { _movePreviousMenuText = value; }
        }

        public void ResetMovePreviousMenuText()
        {
            MovePreviousMenuText = "Move to &Previous Tab Group";
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("Move to &Next Tab Group")]
        public string MoveNextMenuText
        {
            get { return _moveNextMenuText; }
            set { _moveNextMenuText = value; }
        }
        
        public void ResetMoveNextMenuText()
        {
            MoveNextMenuText = "Move to &Next Tab Group";
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("New &Vertical Tab Group")]
        public string NewVerticalMenuText
        {
            get { return _newVerticalMenuText; }
            set { _newVerticalMenuText = value; }
        }

        public void ResetNewVerticalMenuText()
        {
            NewVerticalMenuText = "New &Vertical Tab Group";
        }

        [Localizable(true)]
        [Category("Text String")]
        [DefaultValue("New &Horizontal Tab Group")]
        public string NewHorizontalMenuText
        {
            get { return _newHorizontalMenuText; }
            set { _newHorizontalMenuText = value; }
        }
        
        public void ResetNewHorizontalMenuText()
        {
            NewHorizontalMenuText = "New &Horizontal Tab Group";
        }

        [Category("Shortcuts")]
        public Shortcut CloseShortcut
        {
            get { return _closeShortcut; }
            set { _closeShortcut = value; }
        }

        protected bool ShouldSerializeCloseShortcut()
        {
            return !_closeShortcut.Equals(Shortcut.CtrlShiftC);
        }

        public void ResetCloseShortcut()
        {
            CloseShortcut = Shortcut.CtrlShiftC;
        }
        
        [Category("Shortcuts")]
        public Shortcut ProminentShortcut
        {
            get { return _prominentShortcut; }
            set { _prominentShortcut = value; }
        }

        protected bool ShouldSerializeProminentShortcut()
        {
            return !_prominentShortcut.Equals(Shortcut.CtrlShiftT);
        }

        public void ResetProminentShortcut()
        {
            ProminentShortcut = Shortcut.CtrlShiftT;  
        }
        
        [Category("Shortcuts")]
        public Shortcut RebalanceShortcut
        {
            get { return _rebalanceShortcut; }
            set { _rebalanceShortcut = value; }
        }

        protected bool ShouldSerializeRebalanceShortcut()
        {
            return !_rebalanceShortcut.Equals(Shortcut.CtrlShiftR);
        }

        public void ResetRebalanceShortcut()
        {
            RebalanceShortcut = Shortcut.CtrlShiftR;
        }

        [Category("Shortcuts")]
        public Shortcut MovePreviousShortcut
        {
            get { return _movePreviousShortcut; }
            set { _movePreviousShortcut = value; }
        }

        protected bool ShouldSerializeMovePreviousShortcut()
        {
            return !_movePreviousShortcut.Equals(Shortcut.CtrlShiftP);
        }

        public void ResetMovePreviousShortcut()
        {
            MovePreviousShortcut = Shortcut.CtrlShiftP;
        }
        
        [Category("Shortcuts")]
        public Shortcut MoveNextShortcut
        {
            get { return _moveNextShortcut; }
            set { _moveNextShortcut = value; }
        }

        protected bool ShouldSerializeMoveNextShortcut()
        {
            return !_moveNextShortcut.Equals(Shortcut.CtrlShiftN);
        }

        public void ResetMoveNextShortcut()
        {
            MoveNextShortcut = Shortcut.CtrlShiftN;
        }
        
        [Category("Shortcuts")]
        public Shortcut SplitVerticalShortcut
        {
            get { return _splitVerticalShortcut; }
            set { _splitVerticalShortcut = value; }
        }

        protected bool ShouldSerializeSplitVerticalShortcut()
        {
            return !_splitVerticalShortcut.Equals(Shortcut.CtrlShiftV);
        }

        public void ResetSplitVerticalShortcut()
        {
            SplitVerticalShortcut = Shortcut.CtrlShiftV;
        }
        
        [Category("Shortcuts")]
        public Shortcut SplitHorizontalShortcut
        {
            get { return _splitHorizontalShortcut; }
            set { _splitHorizontalShortcut = value; }
        }

        protected bool ShouldSerializeSplitHorizontalShortcut()
        {
            return !_splitHorizontalShortcut.Equals(Shortcut.CtrlShiftH);
        }

        public void ResetSplitHorizontalShortcut()
        {
            SplitHorizontalShortcut = Shortcut.CtrlShiftH;
        }
        
        [Category("TabbedGroups")]
        public ImageList ImageList
        {
            get { return _imageList; }
            
            set 
            { 
                if (_imageList != value)
                {
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ImageListChanging);

                    _imageList = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.ImageListChanged);
                }
            }
        }
        
        protected bool ShouldSerializeImageList()
        {
            return _imageList != null;
        }
        
        public void ResetImageList()
        {
            ImageList = null;
        }
        
        [Category("TabbedGroups")]
        [DefaultValue(typeof(DisplayTabModes), "ShowAll")]
        public DisplayTabModes DisplayTabMode
        {
            get { return _displayTabMode; }
            
            set
            {
                if (_displayTabMode != value)
                {
                    _displayTabMode = value;
                    
                    // Propogate to all children
                    Notify(TabGroupBase.NotifyCode.DisplayTabMode);
                }
            }
        }
        
        public void ResetDisplayTabMode()
        {
            DisplayTabMode = DisplayTabModes.ShowAll;
        }

        [Category("TabbedGroups")]
        [DefaultValue(true)]
        public bool SaveControls
        {
            get { return _saveControls; }
            set { _saveControls = value; }
        }
        
        public void ResetSaveControls()
        {
            SaveControls = true;
        }

        [Category("TabbedGroups")]
        public bool Dirty
        {
            get { return _dirty; }
            
            set 
            {
                if (_dirty != value)
                {
                    _dirty = value;
                    
                    OnDirtyChanged(EventArgs.Empty);
                }
            }
        }
        
        protected bool ShouldSerializeDirty()
        {
            return false;
        }
        
        public void ResetDirty()
        {
            Dirty = false;
        }

        [Category("TabbedGroups")]
        [DefaultValue(true)]
        public bool AutoCalculateDirty
        {
            get { return _autoCalculateDirty; }
            set { _autoCalculateDirty = value; }
        }
        
        public void ResetAutoCalculateDirty()
        {
            AutoCalculateDirty = true;
        }

        [Category("TabbedGroups")]
        public TabGroupLeaf ActiveLeaf
        {
            get { return _activeLeaf; }
            
            set
            {
                if (_activeLeaf != value)
                {
                    // Mark layout as dirty
                    if (_autoCalculateDirty)
                        _dirty = true;

                    // Remove selection highlight from old leaf
                    if (_activeLeaf != null)
                    {
                        // Get access to the contained tab control
                        TabControl tc = _activeLeaf.GroupControl as Controls.TabControl;
                        
                        // Remove bold text for the selected page
                        tc.BoldSelectedPage = false;
                        
                        _activeLeaf = null;
                    }

                    // Set selection highlight on new active leaf            
                    if (value != null)
                    {
                        // Get access to the contained tab control
                        TabControl tc = value.GroupControl as Controls.TabControl;
                        
                        // Remove bold text for the selected page
                        tc.BoldSelectedPage = true;
                        
                        _activeLeaf = value;
                    }

                    // Is the tab mode dependant on the active leaf value
                    if ((_displayTabMode == DisplayTabModes.ShowActiveLeaf) ||
                        (_displayTabMode == DisplayTabModes.ShowActiveAndMouseOver))
                    {
                        // Yes, better notify a change in value so it can be applied
                        Notify(TabGroupBase.NotifyCode.DisplayTabMode);
                    }
                        
                    OnActiveLeafChanged(EventArgs.Empty);
                }
            }
        }
        
        public void ResetActiveLeaf()
        {
            ActiveLeaf = null;
        }

        [Category("TabbedGroups")]
        public bool AtLeastOneLeaf
        {
            get { return _atLeastOneLeaf; }
            
            set
            {
                if (_atLeastOneLeaf != value)
                {
                    _atLeastOneLeaf = value;
                    
                    // Do always need at least one leaf?
                    if (_atLeastOneLeaf)
                    {
                        // Is there at least one?
                        if (_numLeafs == 0)
                        {
                            // No, create a default entry for the root sequence
                            _root.AddNewLeaf();

                            // Mark layout as dirty
                            if (_autoCalculateDirty)
                                _dirty = true;
                        }
                    }
                    else
                    {
                        // Are there some potential leaves not needed
                        if (_numLeafs > 0)
                        {
                            // Use compaction so only needed ones are retained
                            if (_autoCompact)
                                Compact();
                        }
                    }
                }
            }
        }
        
        public void ResetAtLeastOneLeaf()
        {
            AtLeastOneLeaf = true;
        }

        [Category("TabbedGroups")]
        [DefaultValue(true)]
        public bool AutoCompact
        {
            get { return _autoCompact; }
            set { _autoCompact = value; }
        }

        public void ResetAutoCompact()
        {
            _autoCompact = true;
        }

        public void Rebalance()
        {
            _root.Rebalance(true);
        }

        public void Rebalance(bool recurse)
        {
            _root.Rebalance(recurse);
        }
        
        public void Compact()
        {
            Compact(_compactOptions);
        }
        
        public void Compact(CompactFlags flags)
        {
            // When  entries are removed because of compacting this may cause the container object 
            // to start a compacting request. Prevent this recursion by using a simple varible.
            if (!_compacting)
            {
                // We never compact when loading/initializing the contents
                if (!_initializing)
                {
                    _compacting = true;
                    _root.Compact(flags);
                    _compacting = false;
                    
                    EnforceAtLeastOneLeaf();
                }
            }
        }
        
        public TabGroupLeaf FirstLeaf()
        {
            return RecursiveFindLeafInSequence(_root, true);
        }

        public TabGroupLeaf LastLeaf()
        {
            return RecursiveFindLeafInSequence(_root, false);
        }

        public TabGroupLeaf NextLeaf(TabGroupLeaf current)
        {
            // Get parent of the provided leaf
            TabGroupSequence tgs = current.Parent as TabGroupSequence;
            
            // Must have a valid parent sequence
            if (tgs != null)
                return RecursiveFindLeafInSequence(tgs, current, true);
            else
                return null;
        }
        
        public TabGroupLeaf PreviousLeaf(TabGroupLeaf current)
        {
            // Get parent of the provided leaf
            TabGroupSequence tgs = current.Parent as TabGroupSequence;
            
            // Must have a valid parent sequence
            if (tgs != null)
                return RecursiveFindLeafInSequence(tgs, current, false);
            else
                return null;
        }

        internal void MoveActiveToNearestFromLeaf(TabGroupBase oldLeaf)
        {
            // Must have a reference to begin movement
            if (oldLeaf != null)
            {
                // Find the parent sequence of leaf, remember that a 
                // leaf must be contained within a sequence instance
                TabGroupSequence tgs = oldLeaf.Parent as TabGroupSequence;
                
                // Must be valid, but had better check anyway
                if (tgs != null)
                {
                    // Move relative to given base in the sequence
                    MoveActiveInSequence(tgs, oldLeaf);
                }
            }
        }
        
        internal void MoveActiveToNearestFromSequence(TabGroupSequence tgs)
        {
            // Is active leaf being moved from root sequence
            if (_root == tgs)
            {
                // Then make nothing active
                ActiveLeaf = null;
            }
            else
            {
                // Find the parent sequence of given sequence
                TabGroupSequence tgsParent = tgs.Parent as TabGroupSequence;
            
                // Must be valid, but had better check anyway
                if (tgs != null)
                {
                    // Move relative to given base in the sequence
                    MoveActiveInSequence(tgsParent, tgs);
                }
            }
        }
        
        public virtual void OnTabControlCreated(Controls.TabControl tc)
        {
            // Remember how many leafs there are
            _numLeafs++;
        
            // Define default values
            tc.Appearance = Magic.Controls.TabControl.VisualAppearance.MultiDocument;
            tc.BoldSelectedPage = false;
            tc.IDEPixelBorder = true;
            tc.ImageList = _imageList;
            tc.Style = _style;

            // Apply the current display tab mode setting            
            switch(_displayTabMode)
            {
                case TabbedGroups.DisplayTabModes.ShowAll:
                    tc.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.ShowAlways;
                    break;
                case TabbedGroups.DisplayTabModes.HideAll:
                    tc.HideTabsMode = Magic.Controls.TabControl.HideTabsModes.HideAlways;
                    break;
            }
            
            // Has anyone registered for the event?
            if (TabControlCreated != null)
                TabControlCreated(this, tc);
        }
        
        public virtual void OnPageCloseRequested(TGCloseRequestEventArgs e)
        {
            // Has anyone registered for the event?
            if (PageCloseRequest != null)
                PageCloseRequest(this, e);
                
        }

        public virtual void OnPageContextMenu(TGContextMenuEventArgs e)
        {
            // Has anyone registered for the event?
            if (PageContextMenu != null)
                PageContextMenu(this, e);
        }

        public virtual void OnGlobalSaving(XmlTextWriter xmlOut)
        {
            // Has anyone registered for the event?
            if (GlobalSaving != null)
                GlobalSaving(this, xmlOut);
        }
        
        public virtual void OnGlobalLoading(XmlTextReader xmlIn)
        {
            // Has anyone registered for the event?
            if (GlobalLoading != null)
                GlobalLoading(this, xmlIn);
        }
        
        public virtual void OnPageSaving(TGPageSavingEventArgs e)
        {
            // Has anyone registered for the event?
            if (PageSaving != null)
                PageSaving(this, e);
        }
        
        public virtual void OnPageLoading(TGPageLoadingEventArgs e)
        {
            // Has anyone registered for the event?
            if (PageLoading != null)
                PageLoading(this, e);
        }

        public virtual void OnProminentLeafChanged(EventArgs e)
        {
            // Has anyone registered for the event?
            if (ProminentLeafChanged != null)
                ProminentLeafChanged(this, e);
        }
        
        public virtual void OnActiveLeafChanged(EventArgs e)
        {
            // Has anyone registered for the event?
            if (ActiveLeafChanged != null)
                ActiveLeafChanged(this, e);
        }
        
        public virtual void OnDirtyChanged(EventArgs e)
        {
            // Has anyone registered for the event?
            if (DirtyChanged != null)
                DirtyChanged(this, e);
        }

        public virtual void OnExternalDrop(TabGroupLeaf tgl, Controls.TabControl tc, DragProvider dp)
        {
            // Has anyone registered for the event?
            if (ExternalDrop != null)
                ExternalDrop(this, tgl, tc, dp);
        }

        public void BeginInit()
        {
            _initializing = true;
        }
        
        public void EndInit()
        {
            _initializing = false;
            
            // Inform the root sequence to reposition itself
            _root.Reposition();
        }
        
        public bool Initializing
        {
            get { return _initializing; }
        }

        public byte[] SaveConfigToArray()
        {
            return SaveConfigToArray(Encoding.Unicode);	
        }

        public byte[] SaveConfigToArray(Encoding encoding)
        {
            // Create a memory based stream
            MemoryStream ms = new MemoryStream();
			
            // Save into the file stream
            SaveConfigToStream(ms, encoding);

            // Must remember to close
            ms.Close();

            // Return an array of bytes that contain the streamed XML
            return ms.GetBuffer();
        }

        public void SaveConfigToFile(string filename)
        {
            SaveConfigToFile(filename, Encoding.Unicode);
        }

        public void SaveConfigToFile(string filename, Encoding encoding)
        {
            // Create/Overwrite existing file
            FileStream fs = new FileStream(filename, FileMode.Create);
			
            // Save into the file stream
            SaveConfigToStream(fs, encoding);		

            // Must remember to close
            fs.Close();
        }

        public void SaveConfigToStream(Stream stream, Encoding encoding)
        {
            XmlTextWriter xmlOut = new XmlTextWriter(stream, encoding); 

            // Use indenting for readability
            xmlOut.Formatting = Formatting.Indented;
			
            // Always begin file with identification and warning
            xmlOut.WriteStartDocument();
            xmlOut.WriteComment(" Magic, The User Interface library for .NET (www.dotnetmagic.com) ");
            xmlOut.WriteComment(" Modifying this generated file will probably render it invalid ");

            // Associate a version number with the root element so that future version of the code
            // will be able to be backwards compatible or at least recognise out of date versions
            xmlOut.WriteStartElement("TabbedGroups");
            xmlOut.WriteAttributeString("FormatVersion", "1");
            
            if (_activeLeaf != null)
                xmlOut.WriteAttributeString("ActiveLeaf", _activeLeaf.Unique.ToString());
            else
                xmlOut.WriteAttributeString("ActiveLeaf", "-1");

            // Give handlers chance to embed custom data
            xmlOut.WriteStartElement("CustomGlobalData");
            OnGlobalSaving(xmlOut);
            xmlOut.WriteEndElement();

            // Save the root sequence
            _root.SaveToXml(xmlOut);

            // Terminate the root element and document        
            xmlOut.WriteEndElement();
            xmlOut.WriteEndDocument();

            // This should flush all actions and close the file
            xmlOut.Close();			
            
            // Saved, so cannot be dirty any more
            if (_autoCalculateDirty)
                _dirty = false;
            
        }

        public void LoadConfigFromArray(byte[] buffer)
        {
            // Create a memory based stream
            MemoryStream ms = new MemoryStream(buffer);
			
            // Save into the file stream
            LoadConfigFromStream(ms);

            // Must remember to close
            ms.Close();
        }

        public void LoadConfigFromFile(string filename)
        {
            // Open existing file
            FileStream fs = new FileStream(filename, FileMode.Open);
			
            // Load from the file stream
            LoadConfigFromStream(fs);		

            // Must remember to close
            fs.Close();
        }

        public void LoadConfigFromStream(Stream stream)
        {
            XmlTextReader xmlIn = new XmlTextReader(stream); 

            // Ignore whitespace, not interested
            xmlIn.WhitespaceHandling = WhitespaceHandling.None;

            // Moves the reader to the root element.
            xmlIn.MoveToContent();

            // Double check this has the correct element name
            if (xmlIn.Name != "TabbedGroups")
                throw new ArgumentException("Root element must be 'TabbedGroups'");

            // Load the format version number
            string version = xmlIn.GetAttribute(0);
            string rawActiveLeaf = xmlIn.GetAttribute(1);

            // Convert format version from string to double
            int formatVersion = (int)Convert.ToDouble(version);
            int activeLeaf = Convert.ToInt32(rawActiveLeaf);
            
            // We can only load 1 upward version formats
            if (formatVersion < 1)
                throw new ArgumentException("Can only load Version 1 and upwards TabbedGroups Configuration files");

            try
            {
                // Prevent compacting and reposition of children
                BeginInit();
                
                // Remove all existing contents
                _root.Clear();
                
                // Read to custom data element
                if (!xmlIn.Read())
                    throw new ArgumentException("An element was expected but could not be read in");

                if (xmlIn.Name != "CustomGlobalData")
                    throw new ArgumentException("Expected 'CustomData' element was not found");

                bool finished = xmlIn.IsEmptyElement;

                // Give handlers chance to reload custom saved data
                OnGlobalLoading(xmlIn);

                // Read everything until we get the end of custom data marker
                while(!finished)
                {
                    // Check it has the expected name
                    if (xmlIn.NodeType == XmlNodeType.EndElement)
                        finished = (xmlIn.Name == "CustomGlobalData");

                    if (!finished)
                    {
                        if (!xmlIn.Read())
                            throw new ArgumentException("An element was expected but could not be read in");
                    }
                } 

                // Read the next well known lement
                if (!xmlIn.Read())
                    throw new ArgumentException("An element was expected but could not be read in");

                // Is it the expected element?
                if (xmlIn.Name != "Sequence")
                    throw new ArgumentException("Element 'Sequence' was expected but not found");
                
                // Reload the root sequence
                _root.LoadFromXml(xmlIn);

                // Move past the end element
                if (!xmlIn.Read())
                    throw new ArgumentException("Could not read in next expected node");

                // Check it has the expected name
                if (xmlIn.NodeType != XmlNodeType.EndElement)
                    throw new ArgumentException("EndElement expected but not found");
            }
            finally
            {
                TabGroupLeaf newActive = null;
            
                // Reset the active leaf correctly
                TabGroupLeaf current = FirstLeaf();
                
                while(current != null)
                {
                    // Default to the first leaf if we cannot find a match
                    if (newActive == null)
                        newActive = current;
                        
                    // Find an exact match?
                    if (current.Unique == activeLeaf)
                    {
                        newActive = current;
                        break;
                    }
                    
                    current = NextLeaf(current);
                }
                
                // Reinstate the active leaf indication
                if (newActive != null)
                    ActiveLeaf = newActive;
            
                // Allow normal operation
                EndInit();
            }
                        
            xmlIn.Close();			
            
            // Just loaded, so cannot be dirty
            if (_autoCalculateDirty)
                _dirty = false;
        }
        
        protected TabGroupLeaf RecursiveFindLeafInSequence(TabGroupSequence tgs, bool forwards)
        {
            int count = tgs.Count;
        
            for(int i=0; i<count; i++)
            {
                // Index depends on which direction we are processing
                int index = (forwards == true) ? i : (tgs.Count - i - 1);
                
                // Is this the needed leaf node?
                if (tgs[index].IsLeaf)
                    return tgs[index] as TabGroupLeaf;
                else
                {
                    // Need to make a recursive check inside group
                    TabGroupLeaf leaf = RecursiveFindLeafInSequence(tgs[index] as TabGroupSequence, forwards);

                    if (leaf != null)
                        return leaf;
                }
            }
            
            // Still no luck
            return null;
        }

        protected TabGroupLeaf RecursiveFindLeafInSequence(TabGroupSequence tgs, TabGroupBase tgb, bool forwards)
        {
            int count = tgs.Count;
            int index = tgs.IndexOf(tgb);
        
            // Are we look for entries after the provided one?
            if (forwards)
            {
                for(int i=index+1; i<count; i++)
                {
                    // Is this the needed leaf node?
                    if (tgs[i].IsLeaf)
                        return tgs[i] as TabGroupLeaf;
                    else
                    {
                        TabGroupLeaf leaf = RecursiveFindLeafInSequence(tgs[i] as TabGroupSequence, forwards);
                    
                        if (leaf != null)
                            return leaf;
                    }
                }
            }
            else
            {
                // Now try each entry before that given
                for(int i=index-1; i>=0; i--)
                {
                    // Is this the needed leaf node?
                    if (tgs[i].IsLeaf)
                        return tgs[i] as TabGroupLeaf;
                    else
                    {
                        TabGroupLeaf leaf = RecursiveFindLeafInSequence(tgs[i] as TabGroupSequence, forwards);
                    
                        if (leaf != null)
                            return leaf;
                    }
                }
            }
                        
            // Still no luck, try our own parent
            if (tgs.Parent != null)
                return RecursiveFindLeafInSequence(tgs.Parent as TabGroupSequence, tgs, forwards);
            else
                return null;
        }

        protected void MoveActiveInSequence(TabGroupSequence tgs, TabGroupBase child)
        {
            int count = tgs.Count;
            int index = tgs.IndexOf(child);
        
            // First try each entry after that given
            for(int i=index+1; i<count; i++)
            {
                // Is this the needed leaf node?
                if (tgs[i].IsLeaf)
                {
                    // Make it active, and finish
                    ActiveLeaf = tgs[i] as TabGroupLeaf;
                    return;  
                }
                else
                {
                    // Need to make a recursive check inside group
                    if (RecursiveActiveInSequence(tgs[i] as TabGroupSequence, true))
                        return;
                }
            }
            
            // Now try each entry before that given
            for(int i=index-1; i>=0; i--)
            {
                // Is this the needed leaf node?
                if (tgs[i].IsLeaf)
                {
                    // Make it active, and finish
                    ActiveLeaf = tgs[i] as TabGroupLeaf;
                    return;  
                }
                else
                {
                    // Need to make a recursive check inside group
                    if (RecursiveActiveInSequence(tgs[i] as TabGroupSequence, false))
                        return;
                }
            }
            
            // Still no luck, try our own parent
            if (tgs.Parent != null)
                MoveActiveInSequence(tgs.Parent as TabGroupSequence, tgs);
        }
        
        protected bool RecursiveActiveInSequence(TabGroupSequence tgs, bool forwards)
        {
            int count = tgs.Count;
        
            for(int i=0; i<count; i++)
            {
                // Index depends on which direction we are processing
                int index = (forwards == true) ? i : (tgs.Count - i - 1);
                
                // Is this the needed leaf node?
                if (tgs[index].IsLeaf)
                {
                    // Make it active, and finish
                    ActiveLeaf = tgs[index] as TabGroupLeaf;
                    return true;
                }
                else
                {
                    // Need to make a recursive check inside group
                    if (RecursiveActiveInSequence(tgs[index] as TabGroupSequence, forwards))
                        return true;
                }
            }
            
            // Still no luck
            return false;
        }
        
        protected void Notify(TabGroupBase.NotifyCode notifyCode)
        {
            // Propogate change notification only is we have a root sequence
            if (_root != null)
                _root.Notify(notifyCode);
        }
        
        internal void EnforceAtLeastOneLeaf()
        {
            // Should not add items during compacting operation
            if (!_compacting)
            {
                // Ensure we enfore policy of at least one leaf
                if (_atLeastOneLeaf)
                {
                    // Is there at least one?
                    if (_numLeafs == 0)
                    {
                        // No, create a default entry for the root sequence
                        _root.AddNewLeaf();
                        
                        // Update the active leaf
                        _activeLeaf = FirstLeaf();

                        // Mark layout as dirty
                        if (_autoCalculateDirty)
                            _dirty = true;
                    }
                }
            }
        }
        
        internal void GroupRemoved(TabGroupBase tgb)
        {
            // Decrease count of leafs entries for each leaf that exists
            // which in the hierarchy that is being removed
            
            if (tgb.IsLeaf)
                _numLeafs--;
            else
            {
                TabGroupSequence tgs = tgb as TabGroupSequence;
            
                // Recurse into processing each child item
                for(int i=0; i<tgs.Count; i++)
                    GroupRemoved(tgs[i]);
            }
            
            // Mark layout as dirty
            if (_autoCalculateDirty)
                _dirty = true;
        }
         
        public bool PreFilterMessage(ref Message msg)
        {
            Form parentForm = this.FindForm();

            // Only interested if the Form we are on is activate (i.e. contains focus)
            if ((parentForm != null) && (parentForm == Form.ActiveForm) && parentForm.ContainsFocus)
            {		
                switch(msg.Msg)
                {
                    case (int)Win32.Msgs.WM_KEYDOWN:
                        // Ignore keyboard input if the control is disabled
                        if (this.Enabled)
                        {
                            // Find up/down state of shift and control keys
                            ushort shiftKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_SHIFT);
                            ushort controlKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_CONTROL);

                            // Basic code we are looking for is the key pressed
                            int code = (int)msg.WParam;

                            // Is SHIFT pressed?
                            bool shiftPressed = (((int)shiftKey & 0x00008000) != 0);

                            // Is CONTROL pressed?
                            bool controlPressed = (((int)controlKey & 0x00008000) != 0);

                            // Was the TAB key pressed?
                            if ((code == (int)Win32.VirtualKeys.VK_TAB) && controlPressed)
                            {
                                if (shiftPressed)
                                    return SelectPreviousTab();
                                else
                                    return SelectNextTab();
                            }
                            else
                            {
                                // Plus the modifier for SHIFT...
                                if (shiftPressed)
                                    code += 0x00010000;

                                // Plus the modifier for CONTROL
                                if (controlPressed)
                                    code += 0x00020000;

                                // Construct shortcut from keystate and keychar
                                Shortcut sc = (Shortcut)(code);

                                // Search for a matching command
                                return TestShortcut(sc);
                            }
                        }
                        break;
                    case (int)Win32.Msgs.WM_SYSKEYDOWN:
                        // Ignore keyboard input if the control is disabled
                        if (this.Enabled)
                        {
                            if ((int)msg.WParam != (int)Win32.VirtualKeys.VK_MENU)
                            {
                                // Construct shortcut from ALT + keychar
                                Shortcut sc = (Shortcut)(0x00040000 + (int)msg.WParam);
		
                                // Search for a matching command
                                return TestShortcut(sc);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return false;
        }
        
        protected bool TestShortcut(Shortcut sc)
        {
            bool result = false;
        
            // Must have an active leaf for shortcuts to operate against
            if (_activeLeaf != null)
            {
                Controls.TabControl tc = _activeLeaf.GroupControl as Controls.TabControl;
            
                // Must have an active tab for these shortcuts to work against
                if (tc.SelectedTab != null)
                {
                    // Close selected page requested?
                    if (sc.Equals(_closeShortcut))
                    {
                        _activeLeaf.OnClose(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }

                    // Toggle the prominence state?
                    if (sc.Equals(_prominentShortcut))
                    {
                        _activeLeaf.OnToggleProminent(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }
                        
                    // Move page to the next group?
                    if (sc.Equals(_moveNextShortcut))
                    {
                        _activeLeaf.OnMoveNext(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }
                
                    // Move page to the previous group?
                    if (sc.Equals(_movePreviousShortcut))
                    {
                        _activeLeaf.OnMovePrevious(_activeLeaf, EventArgs.Empty);
                        result = true;
                    }
                
                    // Cannot split a group unless at least two entries exist                
                    if (tc.TabPages.Count > 1)
                    {
                        bool allowVert = false;
                        bool allowHorz = false;
                        
                        if (_root.Count <= 1)
                        {
                            allowVert = true;
                            allowHorz = true;
                        }
                        else
                        {
                            if (_root.Direction == Direction.Vertical)
                                allowVert = true;
                            else
                                allowHorz = true;
                        }
                    
                        // Create two vertical groups
                        if (allowHorz && sc.Equals(_splitVerticalShortcut))
                        {
                            _activeLeaf.NewHorizontalGroup(_activeLeaf, false);
                            result = true;
                        }

                        // Create two horizontal groups
                        if (allowVert && sc.Equals(_splitHorizontalShortcut))
                        {
                            _activeLeaf.NewVerticalGroup(_activeLeaf, false);
                            result = true;
                        }
                    }
                }
                
                // Request to rebalance all spacing
                if (sc.Equals(_rebalanceShortcut))
                {
                    _activeLeaf.OnRebalance(_activeLeaf, EventArgs.Empty);
                    result = true;
                }
            }

            return result;
        }
        
        protected bool SelectNextTab()
        {
            // If no active leaf...
            if (_activeLeaf == null)
                SelectFirstPage();
            else
            {
                bool selectFirst = false;
                TabGroupLeaf startLeaf = _activeLeaf;
                TabGroupLeaf thisLeaf = startLeaf;
                
                do
                {
                    // Access to the embedded tab control
                    Controls.TabControl tc = thisLeaf.GroupControl as Controls.TabControl;
                
                    // Does it have any pages?
                    if (tc.TabPages.Count > 0)
                    {
                        // Are we allowed to select the first page?
                        if (selectFirst)
                        {
                            // Do it and exit loop
                            tc.SelectedIndex = 0;
                            
                            // Must ensure this becomes the active leaf
                            if (thisLeaf != _activeLeaf)
                                ActiveLeaf = thisLeaf;
                                
                            break;
                        }
                        else
                        {
                            // Is there another page after the selected one?
                            if (tc.SelectedIndex < tc.TabPages.Count - 1)
                            {
                                // Select new page and exit loop
                                tc.SelectedIndex = tc.SelectedIndex + 1;
                                break;
                            }         
                        }           
                    }
                    
                    selectFirst = true;
                    
                    // Find the next leaf in sequence
                    thisLeaf = NextLeaf(thisLeaf);
                    
                    // No more leafs, wrap back to first
                    if (thisLeaf == null)
                        thisLeaf = FirstLeaf();

                    // Back at starting leaf?
                    if (thisLeaf == startLeaf)
                    {
                        // If it was not the first page that we started from
                        if (tc.SelectedIndex > 0)
                        {
                            // Then we have circles all the way around, select first page
                            tc.SelectedIndex = 0;
                        }
                    }

                } while(thisLeaf != startLeaf);
            }
            
            return true;
        }

        protected bool SelectPreviousTab()
        {
            // If no active leaf...
            if (_activeLeaf == null)
                SelectLastPage();
            else
            {
                bool selectLast = false;
                TabGroupLeaf startLeaf = _activeLeaf;
                TabGroupLeaf thisLeaf = startLeaf;
                
                do
                {
                    // Access to the embedded tab control
                    Controls.TabControl tc = thisLeaf.GroupControl as Controls.TabControl;
                
                    // Does it have any pages?
                    if (tc.TabPages.Count > 0)
                    {
                        // Are we allowed to select the last page?
                        if (selectLast)
                        {
                            // Do it and exit loop
                            tc.SelectedIndex = tc.TabPages.Count - 1;
                            
                            // Must ensure this becomes the active leaf
                            if (thisLeaf != _activeLeaf)
                                ActiveLeaf = thisLeaf;
                                
                            break;
                        }
                        else
                        {
                            // Is there another page before the selected one?
                            if (tc.SelectedIndex > 0)
                            {
                                // Select previous page and exit loop
                                tc.SelectedIndex = tc.SelectedIndex - 1;
                                break;
                            }         
                        }           
                    }
                    
                    selectLast = true;
                    
                    // Find the previous leaf in sequence
                    thisLeaf = PreviousLeaf(thisLeaf);
                    
                    // No more leafs, wrap back to first
                    if (thisLeaf == null)
                        thisLeaf = LastLeaf();

                    // Back at starting leaf?
                    if (thisLeaf == startLeaf)
                    {
                        // If it was not the first page that we started from
                        if (tc.SelectedIndex == 0)
                        {
                            // Then we have circles all the way around, select last page
                            tc.SelectedIndex = tc.TabPages.Count - 1;
                        }
                    }

                } while(thisLeaf != startLeaf);
            }
            
            return true;
        }

        protected void SelectFirstPage()
        {
            // Find the first leaf
            ActiveLeaf = FirstLeaf();
                    
            // Did we find a leaf?
            if (_activeLeaf != null)
            {
                // Is there a page that can be selected?
                if (_activeLeaf.TabPages.Count > 0)
                    _activeLeaf.TabPages[0].Selected = true;
            }
        }
        
        protected void SelectLastPage()
        {
            // Find the first leaf
            ActiveLeaf = LastLeaf();
                    
            // Did we find a leaf?
            if (_activeLeaf != null)
            {
                // Is there a page that can be selected?
                if (_activeLeaf.TabPages.Count > 0)
                    _activeLeaf.TabPages[_activeLeaf.TabPages.Count - 1].Selected = true;
            }
        }
    }
}
