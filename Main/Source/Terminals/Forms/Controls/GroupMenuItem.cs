using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Custom toolstrip menu item with lazy loading
    /// </summary>
    internal class GroupMenuItem : ToolStripMenuItem
    {
        /// <summary>
        /// Stored in context menu Tag to identify virtual context menu groups by tag
        /// </summary>
        internal const String TAG = "tag";

        /// <summary>
        /// Gets the associated favorites group unique identifier
        /// </summary>
        internal IGroup Group { get; private set; }

        internal virtual List<IFavorite> Favorites
        {
            get
            {
                return this.Group.Favorites;
            }
        }

        /// <summary>
        /// Gets the value indicating, that this node contains only dummy node
        /// and contains no favorite nodes
        /// </summary>
        internal Boolean IsEmpty
        {
            get
            {
                return this.DropDown.Items.Count == 1 &&
                String.IsNullOrEmpty(this.DropDown.Items[0].Name);
            }
        }

        internal GroupMenuItem(IGroup group, bool createDummyItem = true)
            : this(group.Name, createDummyItem)
        {
            this.Group = group;
        }

        protected GroupMenuItem(string groupName, bool createDummyItem = true)
        {
            this.Text = groupName;
            this.Tag = TAG;

            if (createDummyItem)
                this.DropDown.Items.Add(GroupTreeNode.DUMMY_NODE);
        }

        internal void ClearDropDownsToEmpty()
        {
            this.DropDown.Items.Clear();
            this.DropDown.Items.Add(GroupTreeNode.DUMMY_NODE);
        }
    }
}
