using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Encapsulates customized logic for work with FavoriteTreeNodes and GroupTreeNodes.
    /// </summary>
    internal class TreeListNodes
    {
        private readonly TreeNodeCollection nodes;

        internal IEnumerable<FavoriteTreeNode> FavoriteNodes
        {
            get
            {
                return this.nodes.OfType<FavoriteTreeNode>()
                                 .ToList();
            }
        }

        internal IEnumerable<GroupTreeNode> GroupNodes
        {
            get
            {
                return this.nodes.OfType<GroupTreeNode>()
                                 .ToList();
            }
        }

        public TreeListNodes(TreeNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        /// <summary>
        /// Gets not null collection of favorites obtained using recursive search from all favorite nodes.
        /// </summary>
        internal List<IFavorite> FindAllCheckedFavorites()
        {
            var favorites = new List<IFavorite>();
            this.FindAllCheckedFavorites(favorites);
            return favorites;
        }

        private void FindAllCheckedFavorites(List<IFavorite> favorites)
        {
            var candidates = this.FindCheckedFavorites();
            favorites.AddRange(candidates);

            foreach (GroupTreeNode groupNode in this.GroupNodes)
            {
                // dont expect only Favorite nodes, because of group nodes on the same level
                groupNode.ExpandCheckedGroupNode();
                var childNodes = new TreeListNodes(groupNode.Nodes);
                childNodes.FindAllCheckedFavorites(favorites);
            }
        }

        private IEnumerable<IFavorite> FindCheckedFavorites()
        {
            return this.FavoriteNodes.Where(node => node.Checked)
                                     .Select(node => node.Favorite);
        }

        internal void CheckChildNodesRecursive(bool checkState)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = checkState;
                var childNodes = new TreeListNodes(node.Nodes);
                childNodes.CheckChildNodesRecursive(checkState);
            }
        }

        internal bool ContainsFavoriteNode(IFavorite favorite)
        {
            return this.FavoriteNodes.Any(node => node.Favorite.StoreIdEquals(favorite));
        }
    }
}
