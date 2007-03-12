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
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    public class RedockerContent : Redocker
    {
        public enum Source
        {
            RawContent,
            ContentInsideWindow,
            WindowContent,
            FloatingForm
        }

        // Class constants
        protected static int _hotVectorFromEdge = 2;
        protected static int _hotVectorBeforeControl = 5;

        // Instance fields
        protected ContainerControl _container;
        protected Source _source;
        protected Content _content;
        protected HotZone _currentHotZone;
        protected Control _callingControl;
        protected FloatingForm _floatingForm;
        protected HotZoneCollection _hotZones;
        protected WindowContent _windowContent;
        protected DockingManager _dockingManager;
        protected Rectangle _insideRect;
        protected Rectangle _outsideRect;
        protected Point _offset;

        public RedockerContent(Control callingControl, Content c, Point offset)
        {
            InternalConstruct(callingControl, Source.RawContent, c, null, null, c.DockingManager, offset);
        }

        public RedockerContent(Control callingControl, Content c, WindowContent wc, Point offset)
        {
            InternalConstruct(callingControl, Source.ContentInsideWindow, c, wc, null, c.DockingManager, offset);
        }

        public RedockerContent(Control callingControl, WindowContent wc, Point offset)
        {
            InternalConstruct(callingControl, Source.WindowContent, null, wc, null, wc.DockingManager, offset);
        }

        public RedockerContent(FloatingForm ff, Point offset)
        {
            InternalConstruct(ff, Source.FloatingForm, null, null, ff, ff.DockingManager, offset);
        }

        protected void InternalConstruct(Control callingControl, 
                                         Source source, 
                                         Content c, 
                                         WindowContent wc, 
                                         FloatingForm ff,
                                         DockingManager dm,
                                         Point offset)
        {
            // Store the starting state
            _callingControl = callingControl;
            _source = source;
            _content = c;
            _windowContent = wc;
            _dockingManager = dm;
            _container = _dockingManager.Container;
            _floatingForm = ff;
            _hotZones = null;
            _currentHotZone = null;
            _insideRect = new Rectangle();
            _outsideRect = new Rectangle();
            _offset = offset;

            // Begin tracking straight away
            EnterTrackingMode();
        }

        public override void EnterTrackingMode()
        {
            // Have we entered tracking mode?
            if (!_tracking)
            {
                base.EnterTrackingMode();

                // Source must provide a valid manager instance
                if (_dockingManager == null)
                    throw new ArgumentNullException("DockingManager");

                // Generate the hot spots that represent actions
                GenerateHotZones();
            }
        }

        public override bool ExitTrackingMode(MouseEventArgs e)
        {
            // Have we exited tracking mode?
            if (_tracking)
            {
                base.ExitTrackingMode(e);
	
                // Is there a current HotZone active?
                if (_currentHotZone != null)
                {
					// Convert from Control coordinates to screen coordinates
					Point mousePos = _callingControl.PointToScreen(new Point(e.X, e.Y));
					
					// Let the zone apply whatever change it represents
					return _currentHotZone.ApplyChange(mousePos, this);
                }
            }

            return false;
        }

        public override void QuitTrackingMode(MouseEventArgs e)
        {
            // Have we quit tracking mode?
            if (_tracking)
            {
                if (_callingControl.Handle != IntPtr.Zero)
                {
                    // Remove any visible tracking indicator
                    if (_currentHotZone != null)
                        _currentHotZone.RemoveIndicator(new Point(0,0));
                }
                                    
                base.QuitTrackingMode(e);
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
			if (_callingControl.Handle != IntPtr.Zero)
			{
				// Convert from Control coordinates to screen coordinates
				Point mousePos = _callingControl.PointToScreen(new Point(e.X, e.Y));

				// Find HotZone this position is inside
				HotZone hz = _hotZones.Contains(mousePos);

				if (hz != _currentHotZone)
				{
					if (_currentHotZone != null)
						_currentHotZone.RemoveIndicator(mousePos);

					_currentHotZone = hz;

					if (_currentHotZone != null)
						_currentHotZone.DrawIndicator(mousePos);
				}
				else
				{
					if (_currentHotZone != null)
						_currentHotZone.UpdateForMousePosition(mousePos, this);
				}
			}

            base.OnMouseMove(e);
        }

        public override bool OnMouseUp(MouseEventArgs e)
        {
			if (_callingControl.Handle != IntPtr.Zero)
			{
				if (_currentHotZone != null)
					_currentHotZone.RemoveIndicator(_callingControl.PointToScreen(new Point(e.X, e.Y)));
			}

            return base.OnMouseUp(e);
        }

        public Source DockingSource
        {
            get { return _source; }
        }

        public Content Content
        {
            get { return _content; }
        }

        public WindowContent WindowContent
        {
            get { return _windowContent; }
        }

        public DockingManager DockingManager
        {
            get { return _dockingManager; }
        }

        public FloatingForm FloatingForm
        {
            get { return _floatingForm; }
        }

        protected void GenerateHotZones()
        {
            // Need the client rectangle for the whole Form
            Rectangle formClient = _container.RectangleToScreen(_container.ClientRectangle);

            // Create a fresh collection for HotZones
            _hotZones = new HotZoneCollection();

            // We do not allow docking in front of the outer index entry
            int outerIndex = FindOuterIndex();

            // Create lists of Controls which are docked against each edge	
            ArrayList topList = new ArrayList();
            ArrayList leftList = new ArrayList();
            ArrayList rightList = new ArrayList();
            ArrayList bottomList = new ArrayList();
            ArrayList fillList = new ArrayList();

            PreProcessControlsCollection(topList, leftList, rightList, bottomList, fillList, outerIndex);

            int vectorH;
            int vectorV;

            // Find the vectors required for calculating new sizes
            VectorDependsOnSourceAndState(out vectorH, out vectorV);

            GenerateHotZonesForTop(topList, formClient, vectorV, outerIndex);
            GenerateHotZonesForLeft(leftList, formClient, vectorH, outerIndex);
            GenerateHotZonesForRight(rightList, formClient, vectorH, outerIndex);
            GenerateHotZonesForBottom(bottomList, formClient, vectorV, outerIndex);
            GenerateHotZonesForFill(fillList, outerIndex);
            GenerateHotZonesForFloating(SizeDependsOnSource());
        }

        protected void PreProcessControlsCollection(ArrayList topList, ArrayList leftList, ArrayList rightList, ArrayList bottomList, ArrayList fillList, int outerIndex)
        {
            // Find space left after all docking windows has been positioned
            _insideRect = _container.ClientRectangle; 
            _outsideRect = _insideRect; 

            // We want lists of docked controls grouped by docking style
            foreach(Control item in _container.Controls)
            {
                bool ignoreType = (item is AutoHidePanel);

                int controlIndex = _container.Controls.IndexOf(item);

                bool outer = (controlIndex >= outerIndex);

                if (item.Visible)
                {
                    if (item.Dock == DockStyle.Top)
                    {
                        topList.Insert(0, item);

                        if (_insideRect.Y < item.Bottom)
                        {
                            _insideRect.Height -= item.Bottom - _insideRect.Y;
                            _insideRect.Y = item.Bottom;
                        }

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.Y < item.Bottom)
                            {
                                _outsideRect.Height -= item.Bottom - _outsideRect.Y;
                                _outsideRect.Y = item.Bottom;
                            }
                        }
                    }

                    if (item.Dock == DockStyle.Left)
                    {
                        leftList.Insert(0, item);

                        if (_insideRect.X < item.Right)
                        {
                            _insideRect.Width -= item.Right - _insideRect.X;
                            _insideRect.X = item.Right;
                        }

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.X < item.Right)
                            {
                                _outsideRect.Width -= item.Right - _outsideRect.X;
                                _outsideRect.X = item.Right;
                            }
                        }
                    }

                    if (item.Dock == DockStyle.Bottom)
                    {
                        bottomList.Insert(0, item);

                        if (_insideRect.Bottom > item.Top)
                            _insideRect.Height -= _insideRect.Bottom - item.Top;

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.Bottom > item.Top)
                                _outsideRect.Height -= _outsideRect.Bottom - item.Top;
                        }
                    }

                    if (item.Dock == DockStyle.Right)
                    {
                        rightList.Insert(0, item);

                        if (_insideRect.Right > item.Left)
                            _insideRect.Width -= _insideRect.Right - item.Left;

                        if (outer || ignoreType)
                        {
                            if (_outsideRect.Right > item.Left)
                                _outsideRect.Width -= _outsideRect.Right - item.Left;
                        }
                    }

                    if (item.Dock == DockStyle.Fill)
						fillList.Insert(0, item);
                }
            }

            // Convert to screen coordinates
            _insideRect = _container.RectangleToScreen(_insideRect);
            _outsideRect = _container.RectangleToScreen(_outsideRect);
        }

        protected int FindOuterIndex()
        {
            // We do not allow docking in front of the outer index entry
            int outerIndex = _container.Controls.Count;
			
            Control outerControl = _dockingManager.OuterControl;

            // If an outer control has been specified then use this as the limit
            if (outerControl != null)
                outerIndex = _container.Controls.IndexOf(outerControl);

            return outerIndex;
        }

        protected void GenerateHotZonesForLeft(ArrayList leftList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in leftList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = _container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.Width = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.X, hotArea.Y, vector, hotArea.Height);
					
					hotArea.X += _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockLeft, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.Width = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.X, fullArea.Y, vector, fullArea.Height);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockLeft, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!_dockingManager.InsideFill)
			{
				// Create the HotArea at the left side of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.X, _insideRect.Y, _hotVectorBeforeControl, _insideRect.Height);

				// Create the rectangle for tgqhe insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.X, innerHotArea.Y, vector, innerHotArea.Height);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockLeft, true));
			}
        }

        protected void GenerateHotZonesForRight(ArrayList rightList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in rightList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = _container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.X = hotArea.Right - _hotVectorBeforeControl;
                    hotArea.Width = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.Right - vector, hotArea.Y, vector, hotArea.Height);
					
					hotArea.X -= _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockRight, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.X = fullArea.Right - _hotVectorFromEdge;
            fullArea.Width = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.Right - vector, fullArea.Y, vector, fullArea.Height);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockRight, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!_dockingManager.InsideFill)
			{
				// Create the HotArea at the right side of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.Right - _hotVectorBeforeControl, _insideRect.Y, _hotVectorBeforeControl, _insideRect.Height);

				// Create the rectangle for the insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.Right - vector, innerHotArea.Y, vector, innerHotArea.Height);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockRight, true));
			}
        }

        protected void GenerateHotZonesForTop(ArrayList topList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in topList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = _container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.Height = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.X, hotArea.Y, hotArea.Width, vector);
					
					hotArea.Y += _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockTop, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.Height = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.X, fullArea.Y, fullArea.Width, vector);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockTop, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!_dockingManager.InsideFill)
			{
				// Create the HotArea at the left side of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.X, _insideRect.Y, _insideRect.Width, _hotVectorBeforeControl);

				// Create the rectangle for the insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.X, innerHotArea.Y, innerHotArea.Width, vector);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockTop, true));
			}
        }

        protected void GenerateHotZonesForBottom(ArrayList bottomList, Rectangle formClient, int vector, int outerIndex)
        {
            foreach(Control c in bottomList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = _container.Controls.IndexOf(c);

                if (!ignoreType && (controlIndex < outerIndex))
                {
                    // Grab the screen rectangle of the Control being processed
                    Rectangle hotArea = c.RectangleToScreen(c.ClientRectangle);

                    // Create the rectangle for the hot area
                    hotArea.Y = hotArea.Bottom - _hotVectorBeforeControl;
                    hotArea.Height = _hotVectorBeforeControl;

                    // Create the rectangle for the insertion indicator
                    Rectangle newSize = new Rectangle(hotArea.X, hotArea.Bottom - vector, hotArea.Width, vector);
					
					hotArea.Y -= _hotVectorFromEdge;

                    // Create the new HotZone used to reposition a docking content/windowcontent
                    _hotZones.Add(new HotZoneReposition(hotArea, newSize, State.DockBottom, controlIndex));

                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }

            // Grab the screen rectangle of the Control being processed
            Rectangle fullArea = _outsideRect;

            // Create the rectangle for the hot area
            fullArea.Y = fullArea.Bottom - _hotVectorFromEdge;
            fullArea.Height = _hotVectorFromEdge;

            // Create the rectangle for the insertion indicator
            Rectangle fillSize = new Rectangle(fullArea.X, fullArea.Bottom - vector, fullArea.Width, vector);

            _hotZones.Add(new HotZoneReposition(fullArea, fillSize, State.DockBottom, false));

			// If performing our own InsideFill then do not dock at inner positions
			if (!_dockingManager.InsideFill)
			{
				// Create the HotArea at the bottom of the inner rectangle
				Rectangle innerHotArea = new Rectangle(_insideRect.X, _insideRect.Bottom - _hotVectorBeforeControl, _insideRect.Width, _hotVectorBeforeControl);

				// Create the rectangle for the insertion indicator
				Rectangle innerNewSize = new Rectangle(innerHotArea.X, innerHotArea.Bottom - vector, innerHotArea.Width, vector);

				// Create a HotZone for docking to the Left at the innermost position
				_hotZones.Add(new HotZoneReposition(innerHotArea, innerNewSize, State.DockBottom, true));
			}
        }

        protected void GenerateHotZonesForFill(ArrayList fillList, int outerIndex)
        {
            foreach(Control c in fillList)
            {	
                bool ignoreType = (c is AutoHidePanel);
            
                int controlIndex = _container.Controls.IndexOf(c);

                if (controlIndex < outerIndex)
                {
                    IHotZoneSource ag = c as IHotZoneSource;

                    // Does this control expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }
        }

        protected void GenerateHotZonesForFloating(Size sourceSize)
        {
            ContainerControl main = _dockingManager.Container;
            
            foreach(Form f in main.FindForm().OwnedForms)
            {
                // Cannot redock entire Floatin form onto itself
                if (f != _floatingForm)
                {
                    IHotZoneSource ag = f as IHotZoneSource;

                    // Does this Form expose an interface for its own HotZones?
                    if (ag != null)
                        ag.AddHotZones(this, _hotZones);
                }
            }
             
            // Applies to the entire desktop area
            Rectangle hotArea = new Rectangle(0, 0, 
                                              SystemInformation.MaxWindowTrackSize.Width,
                                              SystemInformation.MaxWindowTrackSize.Height);

            // Position is determined by HotZone dynamically but the size is defined now
            Rectangle newSize = new Rectangle(0, 0, sourceSize.Width, sourceSize.Height);

            // Generate a catch all HotZone for floating a Content
            _hotZones.Add(new HotZoneFloating(hotArea, newSize, _offset, this)); 
        }

        protected void VectorDependsOnSourceAndState(out int vectorH, out int vectorV)
        {
            Size sourceSize = SizeDependsOnSource();

            switch(_source)
            {
                case Source.FloatingForm:
					// Make sure the vector is the smaller of the two dimensions
					if (sourceSize.Width > sourceSize.Height)
						sourceSize.Width = sourceSize.Height;

					if (sourceSize.Height > sourceSize.Width)
						sourceSize.Height = sourceSize.Width;

					// Do not let the new vector extend beyond halfway
					if (sourceSize.Width > (_container.Width / 2))
						sourceSize.Width = _container.Width / 2;

					if (sourceSize.Height > (_container.Height / 2))
						sourceSize.Height = _container.Height / 2;
					break;
                case Source.WindowContent:
                case Source.ContentInsideWindow:
                switch(_windowContent.State)
                {
                    case State.DockLeft:
                    case State.DockRight:
                        vectorH = sourceSize.Width;
                        vectorV = vectorH;
                        return;
                    case State.DockTop:
                    case State.DockBottom:
                        vectorH = sourceSize.Height;
                        vectorV = vectorH;
                        return;
                }
                break;
            }

            vectorV = sourceSize.Height;
            vectorH = sourceSize.Width;
        }

        protected Size SizeDependsOnSource()
        {
            switch(_source)
            {
                case Source.WindowContent:
                    return _windowContent.Size;
                case Source.FloatingForm:
                    return _floatingForm.Size;
                case Source.RawContent:
                case Source.ContentInsideWindow:
                default:
                    return _content.DisplaySize;
            }
        }
    }
}
