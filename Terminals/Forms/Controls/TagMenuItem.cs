using System;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Custom toolstrip menu item with lazy loading
    /// </summary>
    internal class TagMenuItem : ToolStripMenuItem
    {
        internal TagMenuItem()
        {
            this.DropDown.Items.Add(TagTreeNode.DUMMY_NODE);
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
            this.DropDown.Items.Add(TagTreeNode.DUMMY_NODE);
        }
    }
}
