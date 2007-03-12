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
using System.ComponentModel;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    [ToolboxItem(false)]
    public class Zone : Panel
    {
        // Instance fields
        protected State _state;
        protected bool _autoDispose;
        protected DockingManager _manager;
        protected WindowCollection _windows;

        public Zone(DockingManager manager)
        {
            InternalConstruct(manager, State.DockLeft);
        }

        public Zone(DockingManager manager, State state)
        {
            InternalConstruct(manager, state);
        }

        protected void InternalConstruct(DockingManager manager, State state)
        {
            // Must provide a valid manager instance
            if (manager == null)
                throw new ArgumentNullException("DockingManager");

            // Remember initial state
            _state = state;
            _manager = manager;
            _autoDispose = true;

            // Get correct starting state from manager
            this.BackColor = _manager.BackColor;
            this.ForeColor = _manager.InactiveTextColor;

            // Create collection of windows
            _windows = new WindowCollection();

            // We want notification when contents are added/removed/cleared
            _windows.Clearing += new CollectionClear(OnWindowsClearing);
            _windows.Inserted += new CollectionChange(OnWindowInserted);
            _windows.Removing += new CollectionChange(OnWindowRemoving);
            _windows.Removed += new CollectionChange(OnWindowRemoved);
        }

        public virtual State State
        {
            get { return _state; }
            set { _state = value; }
        }

        public bool AutoDispose
        {
            get { return _autoDispose; }
            set { _autoDispose = value; }
        }

        public DockingManager DockingManager
        {
            get { return _manager; }
        }

        public WindowCollection Windows
        {
            get { return _windows; }

            set
            {
                _windows.Clear();
                _windows = value;
            }
        }

		public virtual Restore RecordRestore(Window w, object child, Restore childRestore) { return null; }
		public virtual int MinimumWidth { get { return 0; } }
		public virtual int MinimumHeight { get { return 0; } }

        public virtual void PropogateNameValue(PropogateName name, object value)
        {
            if (name == PropogateName.BackColor)
            {
                this.BackColor = (Color)value;
                Invalidate();
            }

            // Pass onto each of our child Windows
            foreach(Window w in _windows)
                w.PropogateNameValue(name, value);
        }

        protected virtual void OnWindowsClearing()
        {
            foreach(Control c in Controls)
            {
                Window w = c as Window;

                // Remove back pointers from Windows to this Zone
                if (w != null)
                    w.ParentZone = null;
            }

            // Should we kill ourself?
            if (this.AutoDispose)
            {
                // Remove notification when contents are added/removed/cleared
                _windows.Clearing -= new CollectionClear(OnWindowsClearing);
                _windows.Inserted -= new CollectionChange(OnWindowInserted);
                _windows.Removing -= new CollectionChange(OnWindowRemoving);
                _windows.Removed -= new CollectionChange(OnWindowRemoved);

                this.Dispose();
            }
        }

        protected virtual void OnWindowInserted(int index, object value)
        {
            Window w = value as Window;

            // Define back pointer from Window to its new Zone
            w.ParentZone = this;

            // Define the State the Window should adopt
            w.State = _state;
        }

        protected virtual void OnWindowRemoving(int index, object value)
        {
            Window w = value as Window;

            // Remove back pointer from Window to this Zone
            w.ParentZone = null;
        }

        protected virtual void OnWindowRemoved(int index, object value)
        {
            // Removed the last entry?
            if (_windows.Count == 0)
            {
                // Should we kill ourself?
                if (this.AutoDispose)
                    this.Dispose();
            }
        }
    }
}
