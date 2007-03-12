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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Controls
{
    public class WizardControlDesigner :  System.Windows.Forms.Design.ParentControlDesigner
    {
        private ISelectionService _selectionService = null;

        public override ICollection AssociatedComponents
        {
            get 
            {
                if (base.Control is Crownwood.Magic.Controls.WizardControl)
                    return ((Crownwood.Magic.Controls.WizardControl)base.Control).WizardPages;
                else
                    return base.AssociatedComponents;
            }
        }

        protected override bool DrawGrid
        {
            get { return false; }
        }

        public ISelectionService SelectionService
        {
            get
            {
                // Is this the first time the accessor has been called?
                if (_selectionService == null)
                {
                    // Then grab and cache the required interface
                    _selectionService = (ISelectionService)GetService(typeof(ISelectionService));
                }

                return _selectionService;
            }
        }

        protected override void WndProc(ref Message msg)
        {
            // Test for the left mouse down windows message
            if (msg.Msg == (int)Win32.Msgs.WM_LBUTTONDOWN)
            {
                Crownwood.Magic.Controls.WizardControl wizardControl = this.SelectionService.PrimarySelection as Crownwood.Magic.Controls.WizardControl;

                // Check we have a valid object reference
                if (wizardControl != null)
                {
                    Crownwood.Magic.Controls.TabControl tabControl = wizardControl.TabControl;

                    // Check we have a valid object reference
                    if (tabControl != null)
                    {
                        // Extract the mouse position
                        int xPos = (short)((uint)msg.LParam & 0x0000FFFFU);
                        int yPos = (short)(((uint)msg.LParam & 0xFFFF0000U) >> 16);

                        Point screenCoord = wizardControl.PointToScreen(new Point(xPos, yPos));
                        Point clientCoord = tabControl.PointToClient(screenCoord);

                        // Ask the TabControl to change tabs according to mouse message
                        tabControl.ExternalMouseTest(msg.HWnd, clientCoord);
                    }
                }
            }
            else
            {
                if (msg.Msg == (int)Win32.Msgs.WM_LBUTTONDBLCLK)
                {
                    Crownwood.Magic.Controls.WizardControl wizardControl = this.SelectionService.PrimarySelection as Crownwood.Magic.Controls.WizardControl;
                    
                    // Check we have a valid object reference
                    if (wizardControl != null)
                    {
                        Crownwood.Magic.Controls.TabControl tabControl = wizardControl.TabControl;

                        // Check we have a valid object reference
                        if (tabControl != null)
                        {
                            // Extract the mouse position
                            int xPos = (short)((uint)msg.LParam & 0x0000FFFFU);
                            int yPos = (short)(((uint)msg.LParam & 0xFFFF0000U) >> 16);

                            Point screenCoord = wizardControl.PointToScreen(new Point(xPos, yPos));
                            Point clientCoord = tabControl.PointToClient(screenCoord);

                            // Ask the TabControl to process a double click over an arrow as a simple
                            // click of the arrow button. In which case we return immediately to prevent
                            // the base class from using the double to generate the default event
                            if (tabControl.WantDoubleClick(msg.HWnd, clientCoord))
                                return;
                        }
                    }
                }
            }

            base.WndProc(ref msg);
        }
    }
}
