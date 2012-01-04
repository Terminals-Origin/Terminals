using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Tree node for tags, this simulates lazy loading using dummy node,
    /// until first expansion, where favorite nodes should replace the dummy node
    /// </summary>
    internal class GroupTreeNode : TreeNode
    {
        internal GroupTreeNode(IGroup group, string imageKey)
            : this(group)
        {
            this.ImageKey = imageKey;
            this.SelectedImageKey = imageKey;
        }

        internal GroupTreeNode(IGroup group)
            : base(group.Name, 0, 1)
        {
            this.Nodes.Add(String.Empty, DUMMY_NODE);
            this.Name = group.Name;
            this.Group = group;
        }

        internal const string DUMMY_NODE = "Dummy";
        internal IGroup Group { get; private set; }

        /// <summary>
        /// Gets the value indicating lazy loading not performed yet,
        /// e.g. node contains only dummy node and contains no favorite nodes
        /// </summary>
        internal Boolean NotLoadedYet
        {
            get 
            {
                return this.Nodes.Count == 1 &&
                String.IsNullOrEmpty(this.Nodes[0].Name);
            }
        }

        internal bool ContainsFavoriteNode(string favoriteName)
        {
            return this.Nodes.Cast<FavoriteTreeNode>()
                .Any(treeNode => treeNode.Favorite.Name == favoriteName);
        }
    }
}
