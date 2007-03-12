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
using Crownwood.Magic.Win32;
using Crownwood.Magic.Common;

namespace Crownwood.Magic.Docking
{
    public class HotZoneSequence : HotZone
    {
        // Instance fields
        protected int _index;
        protected ZoneSequence _zs;

        public HotZoneSequence(Rectangle hotArea, Rectangle newSize, ZoneSequence zs, int index)
            : base(hotArea, newSize)
        {
            _index = index;
            _zs = zs;
        }

        public override bool ApplyChange(Point screenPos, Redocker parent)
        {
            // We are only called from the RedockerContent class
            RedockerContent redock = parent as RedockerContent;

            DockingManager dockingManager = redock.DockingManager;

			bool becomeFloating = (_zs.State == State.Floating);

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

                        // Create a new Window to host Content
                        Window w = dockingManager.CreateWindowForContent(redock.Content);

                        // Add into Zone
                        _zs.Windows.Insert(_index, w);
                    }
                    break;
                case RedockerContent.Source.WindowContent:
                    {
                        // Is the destination Zone in the Floating state?
						if (becomeFloating)
						{
							foreach(Content c in redock.WindowContent.Contents)
								c.ContentBecomesFloating();
						}
						else
						{
						    if (redock.WindowContent.State == State.Floating)
						    {
                                foreach(Content c in redock.WindowContent.Contents)
                                    c.ContentLeavesFloating();
                            }
						}

                        // Check if the WindowContent source is in same Zone
                        if (redock.WindowContent.ParentZone == _zs)
                        {
                            // Find current position of source WindowContent
                            int currPos = _zs.Windows.IndexOf(redock.WindowContent);
                            
                            // If current window is before the new position then the current 
                            // window will disappear before the new one is inserted,so need to 
                            // adjust down the new insertion point
                            if (currPos < _index)
                                _index--;
                        }

                        // Create a new Window to host Content
                        WindowContent wc = dockingManager.CreateWindowForContent(null) as WindowContent;

                        // Transfer content across
                        int count = redock.WindowContent.Contents.Count;
                        
                        for(int index=0; index<count; index++)
                        {
                            Content c = redock.WindowContent.Contents[0];

                            // Remove from existing location                            
                            redock.WindowContent.Contents.RemoveAt(0);

                            // Add into new WindowContent host
                            wc.Contents.Add(c);  
                        }

                        // Add into host into Zone
                        _zs.Windows.Insert(_index, wc);
                    }
                    break;
                case RedockerContent.Source.ContentInsideWindow:
                    {
						// Perform State specific Restore actions
						if (becomeFloating)
							redock.Content.ContentBecomesFloating();
						else
						{
						    if (redock.Content.ParentWindowContent.State == State.Floating)
                                redock.Content.ContentLeavesFloating();
                        }

                        // Remove Content from existing WindowContent
                        if (redock.Content.ParentWindowContent != null)
                        {
                            // Will removing the Content cause the WindowContent to die?
                            if (redock.Content.ParentWindowContent.Contents.Count == 1)
                            {
                                // Check if the WindowContent source is in same Zone
                                if (redock.Content.ParentWindowContent.ParentZone == _zs)
                                {
                                    // Find current position of source WindowContent
                                    int currPos = _zs.Windows.IndexOf(redock.Content.ParentWindowContent);
                            
                                    // If current window is before the new position then the current 
                                    // window will disappear before the new one is inserted,so need to 
                                    // adjust down the new insertion point
                                    if (currPos < _index)
                                        _index--;
                                }
                            }

                            redock.Content.ParentWindowContent.Contents.Remove(redock.Content);
                        }
    				
                        // Create a new Window to host Content
                        Window w = dockingManager.CreateWindowForContent(redock.Content);

                        // Add into Zone
                        _zs.Windows.Insert(_index, w);
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

                        int count = redock.FloatingForm.Zone.Windows.Count;
                        
                        for(int index=count-1; index>=0; index--)
                        {
                            // Remember the Window reference
                            Window w = redock.FloatingForm.Zone.Windows[index];
                        
                            // Remove from floating collection
                            redock.FloatingForm.Zone.Windows.RemoveAt(index);

                            // Add into new ZoneSequence destination
                            _zs.Windows.Insert(_index, w);
                        }
                    }
                    break;
            }

			dockingManager.UpdateInsideFill();

            // Reduce flicker during transition
            dockingManager.Container.ResumeLayout();

            return true;
        }
    }
}