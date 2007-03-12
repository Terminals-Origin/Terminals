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
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    public class ZoneHelper
    {
		public static ContentCollection Contents(Zone z)
		{
			// Container for returned group of found Content objects
			ContentCollection cc = new ContentCollection();

			// Process each Window in the Zone
			foreach(Window w in z.Windows)
			{
				WindowContent wc = w as WindowContent;

				// Is the Zone a Content derived variation?
				if (wc != null)
				{
					// Add each Content into the collection
					foreach(Content c in wc.Contents)
						cc.Add(c);
				}
			}

			return cc;
		}

		public static StringCollection ContentNames(Zone z)
		{
			// Container for returned group of found String objects
			StringCollection sc = new StringCollection();

			// Process each Window in the Zone
			foreach(Window w in z.Windows)
			{
				WindowContent wc = w as WindowContent;

				// Is the Zone a Content derived variation?
				if (wc != null)
				{
					// Add each Content into the collection
					foreach(Content c in wc.Contents)
						sc.Add(c.Title);
				}
			}

			return sc;
		}

		public static StringCollection ContentNamesInPriority(Zone z, Content c)
		{
			// Container for returned group of found Content objects
			StringCollection sc = new StringCollection();

			// Process each Window in the Zone
			foreach(Window w in z.Windows)
			{
				WindowContent wc = w as WindowContent;

				// Is the Zone a Content derived variation?
				if (wc != null)
				{
					// Does this contain the interesting Content?
					if (wc.Contents.Contains(c))
					{
						// All Content of this Window are given priority and 
						// added into the start of the collection
						foreach(Content content in wc.Contents)
							sc.Insert(0, content.Title);
					}
					else
					{
						// Lower priority Window and so contents are always
						// added to the end of the collection
						foreach(Content content in wc.Contents)
							sc.Add(content.Title);
					}
				}
			}

			return sc;
		}
    }
}