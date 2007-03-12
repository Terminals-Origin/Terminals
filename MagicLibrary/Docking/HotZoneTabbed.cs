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
using Crownwood.Magic.Common;

namespace Crownwood.Magic.Docking
{
    public class HotZoneTabbed : HotZone
    {
        // Class constants
        protected static int _tabPageLeft = 9;
        protected static int _tabPageHeight = 25;
        protected static int _tabPageWidth= 45;

        // Instance fields
        protected bool _itself;
        protected Rectangle _tabRect;
        protected Rectangle _tabRectTL;
        protected Rectangle _tabRectTR;
        protected WindowContentTabbed _wct;

        public HotZoneTabbed(Rectangle hotArea, Rectangle newSize, WindowContentTabbed wct, bool itself)
            : base(hotArea, newSize)
        {
            // Remember state
            _wct = wct;
            _itself = itself;

            // Instead of a single rectangle for the dragging indicator we want to provide
            // two rectangles. One for the main area and another to show a tab extending
            // below it. This ensures the user can tell that it will be added as a new tab
            // page of the control.

            int tabHeight = _tabPageHeight;

            // Make sure the tab rectangle does not extend past end of control
            if (newSize.Height < (tabHeight + _dragWidth))
                tabHeight = newSize.Height -  _dragWidth * 3;

            // Create the tab page extension
            _tabRect = new Rectangle(newSize.X + _tabPageLeft,
                                     newSize.Bottom - tabHeight,
                                     _tabPageWidth,
                                     tabHeight);

            // Make sure tab rectangle does not draw off right side of control
            if (_tabRect.Right > newSize.Right)
                _tabRect.Width -= _tabRect.Right - newSize.Right;

            // We want the intersection between the top left and top right corners to be displayed
            _tabRectTL = new Rectangle(_tabRect.X, _tabRect.Y, _dragWidth, _dragWidth);
            _tabRectTR = new Rectangle(_tabRect.Right - _dragWidth, _tabRect.Y, _dragWidth, _dragWidth);

            // Reduce the main area by the height of the above item
            _newSize.Height -= tabHeight - _dragWidth;
        }

        public override bool ApplyChange(Point screenPos, Redocker parent)
        {
            // If docking back to itself then refuse to apply the change, this will cause the
            // WindowContentTabbed object to put back the content which is the desired effect
            if (_itself)
                return false;

            // We are only called from the RedockerContent class
            RedockerContent redock = parent as RedockerContent;

            DockingManager dockingManager = redock.DockingManager;

			bool becomeFloating = (_wct.ParentZone.State == State.Floating);

            // Reduce flicker during transition
            dockingManager.Container.SuspendLayout();

            // Manageing Zones should remove display AutoHide windows
            dockingManager.RemoveShowingAutoHideWindows();

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.RawContent:
					{
						// Perform State specific Restore actions
						if (becomeFloating)
							redock.Content.ContentBecomesFloating();

						_wct.Contents.Add(redock.Content);
					}
                    break;
                case RedockerContent.Source.WindowContent:
					{
						// Perform State specific Restore actions
						if (becomeFloating)
						{
							foreach(Content c in redock.WindowContent.Contents)
								c.ContentBecomesFloating();
						}
                        else
                        {
                            // If the source is leaving the Floating state then need to record Restore positions
                            if (redock.WindowContent.State == State.Floating)
                            {
                                foreach(Content c in redock.WindowContent.Contents)
                                    c.ContentLeavesFloating();
                            }
                        }

						int count = redock.WindowContent.Contents.Count;

						for(int index=0; index<count; index++)
						{
							Content c = redock.WindowContent.Contents[0];

							// Remove Content from previous WindowContent
							redock.WindowContent.Contents.RemoveAt(0);

							// Add into new WindowContent
							_wct.Contents.Add(c);
						}
					}
                    break;
                case RedockerContent.Source.ContentInsideWindow:
					{
						// Perform State specific Restore actions
						if (becomeFloating)
							redock.Content.ContentBecomesFloating();
                        else
                        {
                            // If the source is leaving the Floating state then need to record Restore position
                            if (redock.Content.ParentWindowContent.State == State.Floating)
                                redock.Content.ContentLeavesFloating();
                        }

						// Remove Content from existing WindowContent
						if (redock.Content.ParentWindowContent != null)
							redock.Content.ParentWindowContent.Contents.Remove(redock.Content);

						_wct.Contents.Add(redock.Content);
					}
                    break;
                case RedockerContent.Source.FloatingForm:
					{
						// Perform State specific Restore actions
						if (!becomeFloating)
						{
							// Make every Content object in the Floating Zone 
							// record its current state as the Floating state 
							redock.FloatingForm.ExitFloating();
						}

						int wCount = redock.FloatingForm.Zone.Windows.Count;
                    
						for(int wIndex=0; wIndex<wCount; wIndex++)
						{
							WindowContent wc = redock.FloatingForm.Zone.Windows[0] as WindowContent;

							if (wc != null)
							{
								int cCount = wc.Contents.Count;
                            
								for(int cIndex=0; cIndex<cCount; cIndex++)
								{
									// Get reference to first content in collection
									Content c = wc.Contents[0];

									// Remove from old WindowContent
									wc.Contents.RemoveAt(0);

									// Add into new WindowContentTabbed
									_wct.Contents.Add(c);
								}
							}
						}
					}
                    break;
            }

			dockingManager.UpdateInsideFill();

            // Reduce flicker during transition
            dockingManager.Container.ResumeLayout();

            return true;
        }

        public override void DrawReversible(Rectangle rect)
        {
            DrawHelper.DrawDragRectangles(new Rectangle[]{rect, _tabRect, _tabRectTL, _tabRectTR}, _dragWidth);
        }
    }
}