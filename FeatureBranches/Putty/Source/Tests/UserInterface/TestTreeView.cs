using System.Windows.Forms;
using Terminals.Forms.Controls;

namespace Tests.UserInterface
{
    /// <summary>
    /// Wrapped tree view to be able test UI from unit test
    /// </summary>
    internal class TestTreeView : FavoritesTreeView
    {
        /// <summary>
        /// Simulate expanding of the tree node from UI
        /// </summary>
        internal void ExpandAllTreeNodes()
        {
            this.ExpandNodes(this.Nodes);
        }

        private void ExpandNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                base.OnAfterExpand(new TreeViewEventArgs(node));
                this.ExpandNodes(node.Nodes);
            }
        }
    }
}