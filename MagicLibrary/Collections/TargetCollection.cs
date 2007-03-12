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
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Collections
{
    public class TargetCollection : CollectionWithEvents
    {
        public Target Add(Target value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

        public void AddRange(Target[] values)
        {
            // Use existing method to add each array entry
            foreach(Target page in values)
                Add(page);
        }

        public void Remove(Target value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public void Insert(int index, Target value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(Target value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

        public Target this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as Target); }
        }

        public int IndexOf(Target value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

        public Target Contains(Point pt)
        {
            foreach(Target t in base.List)
            {
                if (t.HotRect.Contains(pt))
                    return t;
            }

            return null;
        }
    }
}
