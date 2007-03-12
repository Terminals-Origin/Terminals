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
    public class HotZoneFloating : HotZone
    {
        // Instance fields
        protected Point _offset;
        protected Point _drawPos;
        protected Rectangle _drawRect;
        protected RedockerContent _redocker;

        public HotZoneFloating(Rectangle hotArea, Rectangle newSize, Point offset, RedockerContent redocker)
            : base(hotArea, newSize)
        {
            // Store initial state
            _offset = offset;
            _redocker = redocker;

            Size floatSize = CalculateFloatingSize();
            float widthPercentage = (float)floatSize.Width / (float)_newSize.Width;
            float heightPercentage = (float)floatSize.Height / (float)_newSize.Height;

            _newSize.Width = floatSize.Width;
            _newSize.Height = floatSize.Height + SystemInformation.ToolWindowCaptionHeight;
            
            _offset.X = (int)((float) _offset.X * widthPercentage);
            _offset.Y = (int)((float) _offset.Y * heightPercentage);

			// We do not want the indicator to be too far away from the cursor, so limit check the offset
			if (_offset.X > newSize.Width)
				_offset.X = newSize.Width;

			if (_offset.Y > newSize.Height)
				_offset.Y = newSize.Height;
        }

        public override bool ApplyChange(Point screenPos, Redocker parent)
        {
            // Should always be the appropriate type
            RedockerContent redock = parent as RedockerContent;
        
            DockingManager dockingManager = redock.DockingManager;

            Zone newZone = null;
            
            // Manageing Zones should remove display AutoHide windows
            dockingManager.RemoveShowingAutoHideWindows();

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.RawContent:
                    {
                        // Perform State specific Restore actions
                        redock.Content.ContentBecomesFloating();

                        // Create a new Window to host Content
                        Window w = dockingManager.CreateWindowForContent(redock.Content);

                        // We need to create a Zone for containing the transfered content
                        newZone = dockingManager.CreateZoneForContent(State.Floating);
                        
                        // Add into Zone
                        newZone.Windows.Add(w);
                    }
                    break;
                case RedockerContent.Source.WindowContent:
                    // Perform State specific Restore actions
                    foreach(Content c in redock.WindowContent.Contents)
                        c.ContentBecomesFloating();

                    // Remove WindowContent from old Zone
                    if (redock.WindowContent.ParentZone != null)
                        redock.WindowContent.ParentZone.Windows.Remove(redock.WindowContent);

                    // We need to create a Zone for containing the transfered content
                    newZone = dockingManager.CreateZoneForContent(State.Floating);
                    
                    // Add into new Zone
                    newZone.Windows.Add(redock.WindowContent);
                    break;
                case RedockerContent.Source.ContentInsideWindow:
                    {
                        // Perform State specific Restore actions
                        redock.Content.ContentBecomesFloating();

                        // Remove Content from existing WindowContent
                        if (redock.Content.ParentWindowContent != null)
                            redock.Content.ParentWindowContent.Contents.Remove(redock.Content);
    				
                        // Create a new Window to host Content
                        Window w = dockingManager.CreateWindowForContent(redock.Content);

                        // We need to create a Zone for containing the transfered content
                        newZone = dockingManager.CreateZoneForContent(State.Floating);
                        
                        // Add into Zone
                        newZone.Windows.Add(w);
                    }
                    break;
                case RedockerContent.Source.FloatingForm:
                    redock.FloatingForm.Location = new Point(screenPos.X - _offset.X,
                                                             screenPos.Y - _offset.Y);

                    return false;
            }
        
			dockingManager.UpdateInsideFill();

            // Create a new floating form
            FloatingForm floating = new FloatingForm(redock.DockingManager, newZone,
                                                     new ContextHandler(dockingManager.OnShowContextMenu));

            // Find screen location/size            
            _drawRect = new Rectangle(screenPos.X, screenPos.Y, _newSize.Width, _newSize.Height);

            // Adjust for mouse starting position relative to source control
            _drawRect.X -= _offset.X;
            _drawRect.Y -= _offset.Y;

            // Define its location/size
            floating.Location = new Point(_drawRect.Left, _drawRect.Top);
            floating.Size = new Size(_drawRect.Width, _drawRect.Height);

            // Show it!
            floating.Show();

            return true;
        }

        public override void UpdateForMousePosition(Point screenPos, Redocker parent)
        {
            // Remember the current mouse pos
            Point newPos = screenPos;

            // Calculate the new drawing rectangle
            Rectangle newRect = new Rectangle(newPos.X, newPos.Y, _newSize.Width, _newSize.Height);

            // Adjust for mouse starting position relative to source control
            newRect.X -= _offset.X;
            newRect.Y -= _offset.Y;

			// Draw both the old rectangle and the new one, that will remove flicker as the
			// draw method will only actually draw areas that differ between the two rectangles
            DrawHelper.DrawDragRectangles(new Rectangle[]{_drawRect,newRect}, _dragWidth);

			// Remember new values
			_drawPos = newPos;
			_drawRect = newRect;
        }

        public override void DrawIndicator(Point mousePos) 
        {
            // Remember the current mouse pos
            _drawPos = mousePos;

            // Calculate the new drawing rectangle
            _drawRect = new Rectangle(_drawPos.X, _drawPos.Y, _newSize.Width, _newSize.Height);

            // Adjust for mouse starting position relative to source control
            _drawRect.X -= _offset.X;
            _drawRect.Y -= _offset.Y;

            DrawReversible(_drawRect);
        }
		
        public override void RemoveIndicator(Point mousePos) 
        {			
            DrawReversible(_drawRect);
        }
        
        protected Size CalculateFloatingSize()
        {
            Size floatingSize = new Size(0,0);

            // Get specific redocker type
            RedockerContent redock = _redocker as RedockerContent;

            switch(redock.DockingSource)
            {
                case RedockerContent.Source.RawContent:
                    // Whole Form is size requested by single Content
                    floatingSize = redock.Content.FloatingSize;
                    break;
                case RedockerContent.Source.WindowContent:
                    // Find the largest requested floating size
                    foreach(Content c in redock.WindowContent.Contents)
                    {
                        if (c.FloatingSize.Width > floatingSize.Width)
                            floatingSize.Width = c.FloatingSize.Width;
                    
                        if (c.FloatingSize.Height > floatingSize.Height)
                            floatingSize.Height = c.FloatingSize.Height;
                    }

                    // Apply same size to all Content objects
                    foreach(Content c in redock.WindowContent.Contents)
                        c.FloatingSize = floatingSize;
                    break;
                case RedockerContent.Source.ContentInsideWindow:
                    // Whole Form is size requested by single Content
                    floatingSize = redock.Content.FloatingSize;
                    break;
                case RedockerContent.Source.FloatingForm:
                    // Use the requested size
                    floatingSize.Width = _newSize.Width;
                    floatingSize.Height = _newSize.Height;
                    break;
            }

            return floatingSize;
        }
    }
}
