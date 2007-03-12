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
    public class HotZone
    {
        // Class constants
        protected static int _dragWidth = 4;
		
        // Instance fields
        protected Rectangle _hotArea;
        protected Rectangle _newSize;

        public HotZone(Rectangle hotArea, Rectangle newSize)
        {
            // Store initial state
            _hotArea = hotArea;
            _newSize = newSize;
        }

        public Rectangle HotArea
        {
            get { return _hotArea; }
        }

        public Rectangle NewSize
        {
            get { return _newSize; }
        }

        public virtual bool ApplyChange(Point screenPos, Redocker parent) { return false; }
        public virtual void UpdateForMousePosition(Point screenPos, Redocker parent) {}

        public virtual void DrawIndicator(Point mousePos) 
        {
            DrawReversible(_newSize);
        }
		
        public virtual void RemoveIndicator(Point mousePos) 
        {
            DrawReversible(_newSize);
        }

        public virtual void DrawReversible(Rectangle rect)
        {
            DrawHelper.DrawDragRectangle(rect, _dragWidth);
        }
    }
}