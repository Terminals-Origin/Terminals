using System;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Event arguments fired by search control to inform, which text should be searched
    /// </summary>
    internal class SearchEventArgs : EventArgs
    {
        internal SearchEventArgs(string searchText)
        {
            this.SearchText = searchText;
        }

        internal string SearchText { get; private set; }
    }
}
