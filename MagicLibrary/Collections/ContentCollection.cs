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
    public class ContentCollection : CollectionWithEvents
    {
        public Content Add(Content value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

        public void AddRange(Content[] values)
        {
            // Use existing method to add each array entry
            foreach(Content page in values)
                Add(page);
        }

        public void Remove(Content value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public void Insert(int index, Content value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(Content value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

        public bool Contains(ContentCollection values)
        {
			foreach(Content c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

		public bool Contains(String value)
		{
			foreach(Content c in base.List)
				if (c.Title.Equals(value))
					return true;
					
			return false;			
		}

		public bool Contains(StringCollection values)
		{
			foreach(String s in values)
				if (Contains(s))
					return true;

			return false;
		}

        public Content this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as Content); }
        }

        public Content this[string title]
        {
            get 
            {
                // Search for a Content with a matching title
                foreach(Content c in base.List)
                    if (c.Title == title)
                        return c;

                return null;
            }
        }

        public int IndexOf(Content value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		public ContentCollection Copy()
		{
			ContentCollection clone = new ContentCollection();

			// Copy each reference across
            foreach(Content c in base.List)
				clone.Add(c);

			return clone;
		}
    }
}
