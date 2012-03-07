using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TabControl
{
    public class TabControlItemCollection : CollectionWithEvents
    {
        #region Fields

        [Browsable(false)]
        public event CollectionChangeEventHandler CollectionChanged;

        private TabControl owner = null;
        private int lockUpdate;

        #endregion

        #region Ctor

        public TabControlItemCollection(TabControl owner)
        {
            this.owner = owner;
            this.lockUpdate = 0;
        }

        #endregion

        #region Props

        public TabControlItem this[int index]
        {
            get
            {
                if (index < 0 || List.Count - 1 < index)
                    return null;

                return (TabControlItem)List[index];
            }
            set
            {
                List[index] = (TabControlItem)value;
            }
        }

        [Browsable(false)]
        public virtual int DrawnCount
        {
            get
            {
                int count = Count, res = 0;
                if (count == 0) return 0;
                for (int n = 0; n < count; n++)
                {
                    if (this[n].IsDrawn) res++;
                }
                return res;
            }
        }

        public virtual TabControlItem LastVisible
        {
            get
            {
                for (int n = Count - 1; n > 0; n--)
                {
                    if (this[n].Visible)
                        return this[n];
                }

                return null;
            }
        }

        public virtual TabControlItem FirstVisible
        {
            get
            {
                for (int n = 0; n < Count; n++)
                {
                    if (this[n].Visible)
                        return this[n];
                }

                return null;
            }
        }

        [Browsable(false)]
        public virtual int VisibleCount
        {
            get
            {
                int count = Count, res = 0;
                if (count == 0) return 0;
                for (int n = 0; n < count; n++)
                {
                    if (this[n].Visible) res++;
                }
                return res;
            }
        }

        #endregion

        #region Methods

        protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        protected virtual void BeginUpdate()
        {
            lockUpdate++;
        }

        protected virtual void EndUpdate()
        {
            if (--lockUpdate == 0)
                OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
        }

        public virtual void AddRange(TabControlItem[] items)
        {
            BeginUpdate();
            try
            {
                foreach (TabControlItem item in items)
                {
                    List.Add(item);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        public virtual void Assign(TabControlItemCollection collection)
        {
            BeginUpdate();
            try
            {
                Clear();
                for (int n = 0; n < collection.Count; n++)
                {
                    TabControlItem item = collection[n];
                    TabControlItem newItem = new TabControlItem();
                    newItem.Assign(item);
                    Add(newItem);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        public virtual int Add(TabControlItem item)
        {
            int res = IndexOf(item);
            if (res == -1) res = List.Add(item);
            return res;
        }

        public virtual void Remove(TabControlItem item)
        {
            if (List.Contains(item))
                List.Remove(item);
        }

        public virtual TabControlItem MoveTo(int newIndex, TabControlItem item)
        {
            int currentIndex = List.IndexOf(item);
            if (currentIndex >= 0)
            {
                RemoveAt(currentIndex);
                Insert(0, item);

                return item;
            }

            return null;
        }

        public virtual int IndexOf(TabControlItem item)
        {
            return List.IndexOf(item);
        }

        public virtual bool Contains(TabControlItem item)
        {
            return List.Contains(item);
        }

        public virtual void Insert(int index, TabControlItem item)
        {
            if (Contains(item)) return;
            List.Insert(index, item);
        }

        protected override void OnInsertComplete(int index, object item)
        {
            TabControlItem itm = item as TabControlItem;
            itm.Changed += new EventHandler(OnItem_Changed);
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        protected override void OnRemove(int index, object item)
        {
            base.OnRemove(index, item);
            TabControlItem itm = item as TabControlItem;
            itm.Changed -= new EventHandler(OnItem_Changed);
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
        }

        protected override void OnClear()
        {
            if (Count == 0) return;
            BeginUpdate();
            try
            {
                for (int n = Count - 1; n >= 0; n--)
                {
                    RemoveAt(n);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        protected virtual void OnItem_Changed(object sender, EventArgs e)
        {
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, sender));
        }

        #endregion
    }
}
