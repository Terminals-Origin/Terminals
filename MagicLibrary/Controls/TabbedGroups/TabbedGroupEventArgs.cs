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
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Controls
{
    public class TGCloseRequestEventArgs
	{
	    protected TabGroupLeaf _tgl;
	    protected Controls.TabControl _tc;
	    protected Controls.TabPage _tp;
	    protected bool _cancel;
	
		public TGCloseRequestEventArgs(TabGroupLeaf tgl, Controls.TabControl tc, Controls.TabPage tp)
		{
		    // Definie initial state
		    _tgl = tgl;
		    _tc = tc;
		    _tp = tp;
		    _cancel = false;
		}
		
		public TabGroupLeaf Leaf
		{
		    get { return _tgl; }
		}
    
        public Controls.TabControl TabControl
        {
            get { return _tc; }
        }

        public Controls.TabPage TabPage
        {
            get { return _tp; }
        }
        
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
    }

    public class TGContextMenuEventArgs : TGCloseRequestEventArgs
    {
        protected PopupMenu _contextMenu;
	
        public TGContextMenuEventArgs(TabGroupLeaf tgl, Controls.TabControl tc, 
                                      Controls.TabPage tp, PopupMenu contextMenu)
            : base(tgl, tc, tp)
        {
            // Definie initial state
            _contextMenu = contextMenu;
        }
		
        public PopupMenu ContextMenu
        {
            get { return _contextMenu; }
        }    
    }
    
    public class TGPageLoadingEventArgs
    {
        protected Controls.TabPage _tp;
        protected XmlTextReader _xmlIn;
        protected bool _cancel;
        
        public TGPageLoadingEventArgs(Controls.TabPage tp, XmlTextReader xmlIn)
        {
            // Definie initial state
            _tp = tp;
            _xmlIn = xmlIn;
            _cancel = false;
        }
        
        public Controls.TabPage TabPage
        {
            get { return _tp; }
        }
        
        public XmlTextReader XmlIn
        {
            get { return _xmlIn; }
        }
        
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
    }    

    public class TGPageSavingEventArgs
    {
        protected Controls.TabPage _tp;
        protected XmlTextWriter _xmlOut;
        
        public TGPageSavingEventArgs(Controls.TabPage tp, XmlTextWriter xmlOut)
        {
            // Definie initial state
            _tp = tp;
            _xmlOut = xmlOut;
        }
        
        public Controls.TabPage TabPage
        {
            get { return _tp; }
        }
        
        public XmlTextWriter XmlOut
        {
            get { return _xmlOut; }
        }
    }    
}
