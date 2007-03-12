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
using System.Windows.Forms;

namespace Crownwood.Magic.Common
{
    public class DebugHelper
    {
        public static void ListControls(Control.ControlCollection controls)
        {
			ListControls("Control Collection", controls, false);
		}

        public static void ListControls(string title, Control.ControlCollection controls)
        {
			ListControls(title, controls, false);
		}

        public static void ListControls(string title, Control.ControlCollection controls, bool fullName)
        {
			// Output title first
			Console.WriteLine("\n{0}", title);

			// Find number of controls in the collection
			int count = controls.Count;

			// Output details for each 
			for(int index=0; index<count; index++)
			{
				Control c = controls[index];

				string typeName;
				
				if (fullName)
					typeName = c.GetType().FullName;
				else
					typeName = c.GetType().Name;

				Console.WriteLine("{0} V:{1} T:{2} N:{3}", index, c.Visible, typeName, c.Name);
			}
        }
    }
}


