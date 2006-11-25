using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace TabControl
{
    #region TabControlItemClosingEventArgs

    public class TabControlItemClosingEventArgs : EventArgs
    {
        public TabControlItemClosingEventArgs(TabControlItem item)
        {
            _item = item;
        }

        private bool _cancel = false;
        private TabControlItem _item;

        public TabControlItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

    }

    #endregion

    #region TabControlItemChangedEventArgs

    public class TabControlItemChangedEventArgs : EventArgs
    {
        TabControlItem itm;
        TabControlItemChangeTypes changeType;

        public TabControlItemChangedEventArgs(TabControlItem item, TabControlItemChangeTypes type)
        {
            changeType = type;
            itm = item;
        }

        public TabControlItemChangeTypes ChangeType
        {
            get { return changeType; }
        }

        public TabControlItem Item
        {
            get { return itm; }
        }
    }

    #endregion

    public delegate void TabControlItemChangedHandler(TabControlItemChangedEventArgs e);
    public delegate void TabControlItemClosingHandler(TabControlItemClosingEventArgs e);

}
