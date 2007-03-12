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
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Collections;
using Crownwood.Magic.Menus;

namespace Crownwood.Magic.Collections
{
    public class MenuCommandCollection : CollectionWithEvents
    {
        // Instance fields
        protected string _extraText;
        protected Font _extraFont;
        protected Color _extraTextColor;
        protected Brush _extraTextBrush;
        protected Color _extraBackColor;
        protected Brush _extraBackBrush;
        protected bool _showInfrequent;

        public MenuCommandCollection()
        {
            // Define defaults for internal state
            _extraText = "";
            _extraFont = SystemInformation.MenuFont;
            _extraTextColor = SystemColors.ActiveCaptionText;
            _extraTextBrush = null;
            _extraBackColor = SystemColors.ActiveCaption;
            _extraBackBrush = null;
            _showInfrequent = false;
        }

        public MenuCommand Add(MenuCommand value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

        public void AddRange(MenuCommand[] values)
        {
            // Use existing method to add each array entry
            foreach(MenuCommand page in values)
                Add(page);
        }

        public void Remove(MenuCommand value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public void Insert(int index, MenuCommand value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(MenuCommand value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

        public MenuCommand this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as MenuCommand); }
        }

        public MenuCommand this[string text]
        {
            get 
            {
                // Search for a MenuCommand with a matching title
                foreach(MenuCommand mc in base.List)
                    if (mc.Text == text)
                        return mc;

                return null;
            }
        }

        public int IndexOf(MenuCommand value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

        public bool VisibleItems()
        {
            foreach(MenuCommand mc in base.List)
            {
                // Is the item visible?
                if (mc.Visible)
                {
                    // And its not a separator...
                    if (mc.Text != "-")
                    {
                        // Then should return 'true' except when we are a sub menu item ourself
                        // in which case there might not be any visible children which means that
                        // this item would not be visible either.
                        if ((mc.MenuCommands.Count > 0) && (!mc.MenuCommands.VisibleItems()))
                            continue;

                        return true;
                    }
                }
            }

            return false;
        }

        public string ExtraText
        {
            get { return _extraText; }
            set { _extraText = value; }
        }

        public Font ExtraFont
        {
            get { return _extraFont; }
            set { _extraFont = value; }
        }

        public Color ExtraTextColor
        {
            get { return _extraTextColor; }
            set { _extraTextColor = value; }
        }

        public Brush ExtraTextBrush
        {
            get { return _extraTextBrush; }
            set { _extraTextBrush = value; }
        }

        public Color ExtraBackColor
        {
            get { return _extraBackColor; }
            set { _extraBackColor = value; }
        }

        public Brush ExtraBackBrush
        {
            get { return _extraBackBrush; }
            set { _extraBackBrush = value; }
        }

        public bool ShowInfrequent
        {
            get { return _showInfrequent; }
            set { _showInfrequent = value; }
        }
    }
}
