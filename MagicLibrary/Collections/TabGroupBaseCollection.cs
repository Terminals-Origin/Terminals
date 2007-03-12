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
using System.Xml;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Collections
{
    public class TabGroupBaseCollection : CollectionWithEvents
    {
        public TabGroupBase Add(TabGroupBase value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

        public void AddRange(TabGroupBase[] values)
        {
            // Use existing method to add each array entry
            foreach(TabGroupBase item in values)
                Add(item);
        }

        public void Remove(TabGroupBase value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public void Insert(int index, TabGroupBase value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(TabGroupBase value)
        {
			// Value comparison
			foreach(String s in base.List)
				if (value.Equals(s))
					return true;

			return false;
        }

        public bool Contains(TabGroupBaseCollection values)
        {
			foreach(TabGroupBase c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

        public TabGroupBase this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as TabGroupBase); }
            set { base.List[index] = value; }
        }

        public int IndexOf(TabGroupBase value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
