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
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
    public class Redocker
    {
        // Instance fields
        protected bool _tracking;

        public Redocker()
        {
            // Default the state
            _tracking = false;
        }

        public bool Tracking
        {
            get { return _tracking; }
        }

        public virtual void EnterTrackingMode()
        {
            if (!_tracking)
                _tracking = true;
        }

        public virtual bool ExitTrackingMode(MouseEventArgs e)
        {
            if (_tracking)
                _tracking = false;

            return false;
        }

        public virtual void QuitTrackingMode(MouseEventArgs e)
        {
            if (_tracking)
                _tracking = false;
        }

        public virtual void OnMouseMove(MouseEventArgs e) {}

        public virtual bool OnMouseUp(MouseEventArgs e)
        {
            if (_tracking)
            {
                if (e.Button == MouseButtons.Left)
                    return ExitTrackingMode(e);
                else
                    QuitTrackingMode(e);
            }

            return false;
        }
    }
}
