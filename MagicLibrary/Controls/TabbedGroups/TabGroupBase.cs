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
using System.Drawing;
using System.Windows.Forms;

namespace Crownwood.Magic.Controls
{
	public abstract class TabGroupBase
	{
	    public enum NotifyCode
	    {
	        StyleChanged,
            ProminentChanged,
            MinimumSizeChanged,
            ResizeBarVectorChanged,
	        ResizeBarColorChanged,
	        DisplayTabMode,
	        ImageListChanging,
	        ImageListChanged
	    }

        // Class fields
        protected static int _count = 0;
	    
	    // Instance fields
	    protected int _unique;
	    protected object _tag;
        protected Size _minSize;
        protected Decimal _space;
        protected TabGroupBase _parent;
        protected TabbedGroups _tabbedGroups;
	
        public TabGroupBase(TabbedGroups tabbedGroups)
        {
            InternalConstruct(tabbedGroups, null);
        }
        
        public TabGroupBase(TabbedGroups tabbedGroups, TabGroupBase parent)
		{
		    InternalConstruct(tabbedGroups, parent);
		}
		
		protected void InternalConstruct(TabbedGroups tabbedGroups, TabGroupBase parent)
		{
		    // Assign initial values
		    _tabbedGroups = tabbedGroups;
		    _parent = parent;
		    _unique = _count++;
		    
		    // Defaults
		    _tag = null;
		    _space = 100m;
		    _minSize = new Size(_tabbedGroups.DefaultGroupMinimumWidth,
		                        _tabbedGroups.DefaultGroupMinimumHeight);
		}

        public Decimal Space
        {
            get 
            {
                TabGroupLeaf prominent = _tabbedGroups.ProminentLeaf;
                
                // Are we in prominent mode?
                if (prominent != null)
                {
                    // If we are a child of the root sequence
                    if (_parent.Parent == null)
                    {
                        // Then our space is determined by the containment of the prominent leaf
                        if (this.ContainsProminent(true))
                            return 100m;
                        else
                            return 0m;
                    }
                    else
                    {
                        // Else, if we are inside a sequence that contains prominent leaf
                        if (_parent.ContainsProminent(true))
                        {
                            // Then we need to decide on all or nothing allocation
                            if (this.ContainsProminent(true))
                                return 100m;
                            else
                                return 0m;
                        }
                        else
                        {
                            // Otherwise, we will already be shrunk
                            return _space;                        
                        }
                    }
                }
                else
                    return _space; 
            }
            
            set { _space = value; }
        }

        internal Decimal RealSpace
        {
            get { return _space; }
            set { _space = value; }
        }

        public Size MinimumSize
        {
            get { return _minSize; }
            
            set
            {
                if (!_minSize.Equals(value))
                {
                    _minSize = value;
                    
                    // Inform parent it might need to resize its children
                    if (_parent != null)
                        _parent.Notify(NotifyCode.MinimumSizeChanged);
                }
            }
        }

        public TabGroupBase Parent 
        {
            get { return _parent; }
        }

        internal void SetParent(TabGroupBase tgb)
        {
            _parent = tgb;
        }
        
        public TabbedGroups TabbedGroups 
        {
            get { return _tabbedGroups; }
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        
        public int Unique
        {
            get { return _unique; }
        }
        
        // Common Properties not implemented
        public abstract int Count               { get; }
        public abstract bool IsLeaf             { get; }
        public abstract bool IsSequence         { get; }
        public abstract Control GroupControl    { get; }
        
        // Common methods not implemented
        public abstract void Notify(NotifyCode code); 
        public abstract bool ContainsProminent(bool recurse);
        public abstract void SaveToXml(XmlTextWriter xmlOut);
        public abstract void LoadFromXml(XmlTextReader xmlIn);
    }
}
