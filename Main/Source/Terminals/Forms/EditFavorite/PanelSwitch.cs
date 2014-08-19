using System;

namespace Terminals.Forms.EditFavorite
{
    internal class PanelSwitch
    {
        internal string Title { get; private set; }

        internal Action ShowPanel { get; private set; }

        internal PanelSwitch(string title, Action showPanel)
        {
            this.Title = title;
            this.ShowPanel = showPanel;
        }
    }
}