using System;
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

        internal IGroup Group { get; private set; }

        internal GroupMenuItem(IGroup group)
        {
            this.Group = group;
            this.Text = group.Name;
            this.Tag = TAG;
            this.DropDown.Items.Add(GroupTreeNode.DUMMY_NODE);
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

        internal void ClearDropDownsToEmpty()
        {
            this.DropDown.Items.Clear();
            this.DropDown.Items.Add(GroupTreeNode.DUMMY_NODE);
        }
    }
}
