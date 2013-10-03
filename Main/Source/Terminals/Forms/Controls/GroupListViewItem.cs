using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Represents concreate ListViewItem to present groups
    /// </summary>
    internal class GroupListViewItem : ListViewItem
    {
        internal GroupListViewItem(IGroup group)
            : base(group.Name)
        {
            this.FavoritesGroup = group;
        }

        // named because of conflicting property in base class
        internal IGroup FavoritesGroup { get; private set; }
    }
}
