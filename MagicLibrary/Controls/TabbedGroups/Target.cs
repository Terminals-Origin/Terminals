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
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Controls
{
	public class Target
	{
	    public enum TargetActions
	    {
	        Transfer,
	        GroupLeft,
	        GroupRight,
	        GroupTop,
	        GroupBottom
	    }
	    
	    // Instance fields
	    protected Rectangle _hotRect;
        protected Rectangle _drawRect;
        protected TabGroupLeaf _leaf;
        protected TargetActions _action;

        public Target(Rectangle hotRect, Rectangle drawRect, TabGroupLeaf leaf, TargetActions action)
        {
            // Define state
            _hotRect = hotRect;
            _drawRect = drawRect;
            _leaf = leaf;
            _action = action;
        }

        public Rectangle HotRect
        {
            get { return _hotRect; }
        }	    
        
        public Rectangle DrawRect
        {
            get { return _drawRect; }
        }	    

        public TabGroupLeaf Leaf
        {
            get { return _leaf; }
        }

        public TargetActions Action
        {
            get { return _action; }
        }
	}
	
	public class TargetManager
	{
	    // Class fields
	    protected const int _rectWidth = 4;
        protected static Cursor _validCursor;
        protected static Cursor _invalidCursor;
	
	    // Instance fields
        protected TabbedGroups _host;
        protected TabGroupLeaf _leaf;
	    protected Controls.TabControl _source;
        protected Target _lastTarget;
        protected TargetCollection _targets;
	    
	    static TargetManager()
	    {
            _validCursor = ResourceHelper.LoadCursor(Type.GetType("Crownwood.Magic.Controls.TabbedGroups"),
                                                                  "Crownwood.Magic.Resources.TabbedValid.cur");

            _invalidCursor = ResourceHelper.LoadCursor(Type.GetType("Crownwood.Magic.Controls.TabbedGroups"),
                                                                    "Crownwood.Magic.Resources.TabbedInvalid.cur");
        }
	    
	    public TargetManager(TabbedGroups host, TabGroupLeaf leaf, Controls.TabControl source)
	    {
	        // Define state
	        _host = host;
	        _leaf = leaf;
	        _source = source;
	        _lastTarget = null;
	    
	        // Create collection to hold generated targets
	        _targets = new TargetCollection();
	        
	        // Process each potential leaf in turn
	        TabGroupLeaf tgl = host.FirstLeaf();
	        
	        while(tgl != null)
	        {
	            // Create all possible targets for this leaf
	            CreateTargets(tgl);
	        
	            // Enumerate all leafs
	            tgl = host.NextLeaf(tgl);
	        }
	    }
	    
	    protected void CreateTargets(TabGroupLeaf leaf)
	    {
	        // Grab the underlying tab control
	        Controls.TabControl tc = leaf.GroupControl as Controls.TabControl;

            // Get the total size of the tab control itself in screen coordinates
            Rectangle totalSize = tc.RectangleToScreen(tc.ClientRectangle);

            // We do not allow a page to be transfered to its own leaf!
            if (leaf != _leaf)
            {
                Rectangle tabsSize = tc.RectangleToScreen(tc.TabsAreaRect);

                // Give priority to the tabs area being used to transfer page
                _targets.Add(new Target(tabsSize, totalSize, leaf, Target.TargetActions.Transfer));
            }
	        
            // Can only create new groups if moving relative to a new group 
	        // or we have more than one page in the originating group
	        if ((leaf != _leaf) || ((leaf == _leaf) && _leaf.TabPages.Count > 1))
	        {
	            int horzThird = totalSize.Width / 3;
	            int vertThird = totalSize.Height / 3;
	        
                // Create the four spacing rectangle
                Rectangle leftRect = new Rectangle(totalSize.X, totalSize.Y, horzThird, totalSize.Height);
                Rectangle rightRect = new Rectangle(totalSize.Right - horzThird, totalSize.Y, horzThird, totalSize.Height);
                Rectangle topRect = new Rectangle(totalSize.X, totalSize.Y, totalSize.Width, vertThird);
                Rectangle bottomRect = new Rectangle(totalSize.X, totalSize.Bottom - vertThird, totalSize.Width, vertThird);

                TabGroupSequence tgs = _leaf.Parent as TabGroupSequence;

                // Can only create new groups in same direction, unless this is the only leaf
                if (tgs.Count <= 1)
                {
                    // Add each new target
                    _targets.Add(new Target(leftRect, leftRect, leaf, Target.TargetActions.GroupLeft));
                    _targets.Add(new Target(rightRect, rightRect, leaf, Target.TargetActions.GroupRight));
                    _targets.Add(new Target(topRect, topRect, leaf, Target.TargetActions.GroupTop));
                    _targets.Add(new Target(bottomRect, bottomRect, leaf, Target.TargetActions.GroupBottom));
                }
                else
                {
                    if (tgs.Direction == Direction.Vertical)
                    {
                        _targets.Add(new Target(topRect, topRect, leaf, Target.TargetActions.GroupTop));
                        _targets.Add(new Target(bottomRect, bottomRect, leaf, Target.TargetActions.GroupBottom));
                    }
                    else
                    {
                        _targets.Add(new Target(leftRect, leftRect, leaf, Target.TargetActions.GroupLeft));
                        _targets.Add(new Target(rightRect, rightRect, leaf, Target.TargetActions.GroupRight));
                    }
                }
            }
	        
            // We do not allow a page to be transfered to its own leaf!
            if (leaf != _leaf)
            {
                // Any remaining space is used to 
                _targets.Add(new Target(totalSize, totalSize, leaf, Target.TargetActions.Transfer));
            }
        }
	    
	    public void MouseMove(Point mousePos)
	    {
	        // Find the Target the mouse is currently over (if any)
	        Target t = _targets.Contains(mousePos);
	    
	        // Set appropriate cursor
	        if (t != null)
                _source.Cursor = _validCursor;
            else
                _source.Cursor = _invalidCursor;
                
            if (t != _lastTarget)
            {
                // Remove the old indicator
                if (_lastTarget != null)
                    DrawHelper.DrawDragRectangle(_lastTarget.DrawRect, _rectWidth);
                
                // Draw the new indicator
                if (t != null)
                    DrawHelper.DrawDragRectangle(t.DrawRect, _rectWidth);
                
                // Remember for next time around
                _lastTarget = t;
            }
        }
        
        public void Exit()
        {
            // Remove any showing indicator
            Quit();

            if (_lastTarget != null)
            {
                // Perform action specific operation
                switch(_lastTarget.Action)
                {
                    case Target.TargetActions.Transfer:
                        // Transfer selecte page from source to destination
                        _leaf.MovePageToLeaf(_lastTarget.Leaf);
                        break;
                    case Target.TargetActions.GroupLeft:                        
                        _lastTarget.Leaf.NewHorizontalGroup(_leaf, true);
                        break;
                    case Target.TargetActions.GroupRight:
                        _lastTarget.Leaf.NewHorizontalGroup(_leaf, false);
                        break;
                    case Target.TargetActions.GroupTop:
                        _lastTarget.Leaf.NewVerticalGroup(_leaf, true);
                        break;
                    case Target.TargetActions.GroupBottom:
                        _lastTarget.Leaf.NewVerticalGroup(_leaf, false);
                        break;
                }
            }
        }
            
        public void Quit()
        {
            // Remove drawing of any indicator
            if (_lastTarget != null)
                DrawHelper.DrawDragRectangle(_lastTarget.DrawRect, _rectWidth);
        }
        
        public static void DrawDragRectangle(Rectangle rect)
        {
            DrawHelper.DrawDragRectangle(rect, _rectWidth);
        }
	}
}
