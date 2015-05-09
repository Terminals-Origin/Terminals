using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    internal class TabControlFilter
    {
        internal bool HasSelected
        {
            get
            {
                return this.tabControl.SelectedItem != null;
            }
        }

        internal IFavorite SelectedFavorite
        {
            get
            {
                if (this.HasSelected)
                    return this.Selected.Favorite;

                return null;
            }
        }

        internal IConnection CurrentConnection
        {
            get
            {
                if (this.HasSelected)
                    return this.Selected.Connection;

                return null;
            }
        }

        internal TerminalTabControlItem Selected
        {
            get
            {
                return this.tabControl.SelectedItem as TerminalTabControlItem;
            }
        }

        private readonly TabControl.TabControl tabControl;

        public TabControlFilter(TabControl.TabControl tabControl)
        {
            this.tabControl = tabControl;
        }
    }
}