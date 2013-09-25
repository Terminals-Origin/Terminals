using System;
using System.Collections.Generic;
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
        internal const string DUMMY_NODE = "Dummy";

        /// <summary>
        /// Gets associated data objects, which should be shown in this group
        /// </summary>
        internal List<IFavorite> Favorites
        {
            get
            {
                return Data.Favorites.OrderByDefaultSorting(this.Group.Favorites);
            }
        }

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

        internal IGroup Group { get; private set; }

        internal bool IsOrphan
        {
            get { return this.TreeView == null; }
        }

        internal GroupTreeNode(IGroup group, string imageKey)
            : this(group)
        {
            this.ImageKey = imageKey;
            this.SelectedImageKey = imageKey;
        }

        internal GroupTreeNode(IGroup group)
            : base(group.Name, 0, 1)
        {
            this.Group = group;
            this.Nodes.Add(String.Empty, DUMMY_NODE);
            this.Name = group.Name;
        }

        internal bool ContainsFavoriteNode(IFavorite favorite)
        {
            return this.Nodes.OfType<FavoriteTreeNode>()
                .Any(treeNode => treeNode.Favorite.StoreIdEquals(favorite));
        }

        internal void UpdateByGroup(IGroup group)
        {
            this.Group = group;
            this.Text = this.Group.Name;
        }

        internal bool HasGroupIn(IEnumerable<IGroup> requiredGroups)
        {
            return requiredGroups.Any(required => required.StoreIdEquals(this.Group));
        }
    }
}
