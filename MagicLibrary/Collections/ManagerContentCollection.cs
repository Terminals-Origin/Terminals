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
using System.Windows.Forms;
using System.Runtime.Serialization;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Collections
{
    public class ManagerContentCollection : CollectionWithEvents
    {
        // Instance fields
        protected DockingManager _manager;

        public ManagerContentCollection(DockingManager manager)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Default the state
            _manager = manager;
        }
		
  		public Content Add()
        {
            Content c = new Content(_manager);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

		// Should only ever be used by Serialization
  		public Content Add(Content c)
        {
			// Assign correct docking manager to object
			c.DockingManager = _manager;

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

        public Content Add(Control control)
        {
            Content c = new Content(_manager, control);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

        public Content Add(Control control, string title)
        {
            Content c = new Content(_manager, control, title);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

        public Content Add(Control control, string title, ImageList imageList, int imageIndex)
        {
            Content c = new Content(_manager, control, title, imageList, imageIndex);

            // Use base class to process actual collection operation
            base.List.Add(c as object);

            return c;
        }

        public void Remove(Content value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public bool Contains(Content value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
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

		public int SetIndex(int newIndex, Content value)
		{
			base.List.Remove(value);
			base.List.Insert(newIndex, value);

			return newIndex;
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
