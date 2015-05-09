using System.Collections.Generic;
using System.Linq;
using TabControl;
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

        internal TabControlItem FindAttachedTab(IFavorite updated)
        {
            return this.tabControl.Items.Cast<TerminalTabControlItem>()
                .FirstOrDefault(tab => tab.Favorite != null && tab.Favorite.StoreIdEquals(updated));
        }

        internal IEnumerable<IFavorite> SelectTabsWithFavorite()
        {
            return this.tabControl.Items.OfType<TerminalTabControlItem>()
                .Where(ti => ti.Favorite != null)
                .Select(ti => ti.Favorite);
        }

        internal IFavorite FindFavoriteByTabTitle(string tabTitle)
        {
            var tab = this.tabControl.Items.OfType<TerminalTabControlItem>()
                .FirstOrDefault(candidate => candidate.Title == tabTitle);

            if (tab != null)
                return tab.Favorite;

            return null;
        }

        internal TerminalTabControlItem FindCaptureManagerTab()
        {
            string captureManagerTitle = Program.Resources.GetString("CaptureManager");

            return this.tabControl.Items
                .OfType<TerminalTabControlItem>()
                .FirstOrDefault(ti => ti.Title == captureManagerTitle);
        }

        internal TerminalTabControlItem FindTabToClose(Connection connection)
        {
            return this.tabControl.Items
                .OfType<TerminalTabControlItem>()
                .FirstOrDefault(tab => tab.Connection == connection);
        }
    }
}