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
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Collections
{
    public class WindowCollection : CollectionWithEvents
    {
        public Window Add(Window value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

        public void AddRange(Window[] values)
        {
            // Use existing method to add each array entry
            foreach(Window page in values)
                Add(page);
        }

        public void Remove(Window value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public void Insert(int index, Window value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(Window value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

        public Window this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as Window); }
        }

        public int IndexOf(Window value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }
    }
}
