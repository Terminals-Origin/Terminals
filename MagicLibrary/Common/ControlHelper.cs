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
    public class ControlHelper
	{
		public static void RemoveAll(Control control)
		{
			if ((control != null) && (control.Controls.Count > 0))
			{
                Button tempButton = null;
                Form parentForm = control.FindForm();
                
				if (parentForm != null)
				{
					// Create a hidden, temporary button
					tempButton = new Button();
					tempButton.Visible = false;

					// Add this temporary button to the parent form
					parentForm.Controls.Add(tempButton);

					// Must ensure that temp button is the active one
					parentForm.ActiveControl = tempButton;
                }

  				// Remove all entries from target
				control.Controls.Clear();

                if (parentForm != null)
                {
                    // Remove the temporary button
					tempButton.Dispose();
					parentForm.Controls.Remove(tempButton);
				}
			}
		}

		public static void Remove(Control.ControlCollection coll, Control item)
		{
			if ((coll != null) && (item != null))
			{
                Button tempButton = null;
                Form parentForm = item.FindForm();

				if (parentForm != null)
				{
					// Create a hidden, temporary button
					tempButton = new Button();
					tempButton.Visible = false;

					// Add this temporary button to the parent form
					parentForm.Controls.Add(tempButton);

					// Must ensure that temp button is the active one
					parentForm.ActiveControl = tempButton;
                }
                
				// Remove our target control
				coll.Remove(item);

                if (parentForm != null)
                {
                    // Remove the temporary button
					tempButton.Dispose();
					parentForm.Controls.Remove(tempButton);
				}
			}
		}

		public static void RemoveAt(Control.ControlCollection coll, int index)
		{
			if (coll != null)
			{
				if ((index >= 0) && (index < coll.Count))
				{
					Remove(coll, coll[index]);
				}
			}
		}
    
        public static void RemoveForm(Control source, Form form)
        {
            ContainerControl container = source.FindForm() as ContainerControl;
            
            if (container == null)
                container = source as ContainerControl;

            Button tempButton = new Button();
            tempButton.Visible = false;

            // Add this temporary button to the parent form
            container.Controls.Add(tempButton);

            // Must ensure that temp button is the active one
            container.ActiveControl = tempButton;

            // Remove Form parent
            form.Parent = null;
            
            // Remove the temporary button
            tempButton.Dispose();
            container.Controls.Remove(tempButton);
        }
    }
}