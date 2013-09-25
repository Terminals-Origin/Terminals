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
            foreach (TreeNode node in this.nodes)
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

        internal void InsertGroupNodes(IEnumerable<IGroup> groups)
        {
            foreach (IGroup group in groups)
            {
                this.InsertGroupNode(group);
            }
        }

        /// <summary>
        /// Creates the and add Group node in tree list on proper position defined by index.
        /// This allowes the Group nodes to keep ordered by name.
        /// </summary>
        /// <param name="group">The group to create.</param>
        /// <param name="index">The index on which node would be inserted.
        /// If negative number, than it is added to the end.</param>
        internal void InsertGroupNode(IGroup group, int index = -1)
        {
            var groupNode = new GroupTreeNode(group);
            this.InsertNodePreservingOrder(index, groupNode);
        }

        internal void InsertFavorites(IEnumerable<IFavorite> favorites)
        {
            foreach (IFavorite favorite in favorites)
            {
                this.InsertFavorite(favorite);
            }
        }

        internal void InsertFavorite(IFavorite favorite)
        {
            int insertIndex = this.FindFavoriteNodeInsertIndex(favorite);
            this.AddFavoriteNode(favorite, insertIndex);
        }

        internal void AddFavoriteNodes(IEnumerable<IFavorite> favorites)
        {
            foreach (IFavorite favorite in favorites)
            {
                this.AddFavoriteNode(favorite);
            }
        }

        private void AddFavoriteNode(IFavorite favorite, int index = -1)
        {
            var favoriteTreeNode = new FavoriteTreeNode(favorite);
            this.InsertNodePreservingOrder(index, favoriteTreeNode);
        }

        internal void InsertNodePreservingOrder(int index, TreeNode node)
        {
            if (index < 0)
                this.nodes.Add(node);
            else
                this.nodes.Insert(index, node);
        }

        /// <summary>
        /// Finds the index for the node to insert in nodes collection
        /// and skips nodes before the startIndex.
        /// </summary>
        /// <param name="group">Not empty new Group to add.</param>
        /// <returns>
        /// -1, if the Group should be added to the end of Group nodes, otherwise found index.
        /// </returns>
        internal int FindGroupNodeInsertIndex(IGroup group)
        {
            // take all, we have to find place, where favorite Nodes start
            foreach (TreeNode treeNode in this.nodes)
            {
                // reached end of group nodes, all following are Favorite nodes
                // or search index between group nodes
                if (treeNode is FavoriteTreeNode || SortNewBeforeSelected(group, treeNode))
                    return treeNode.Index;
            }

            return -1;
        }

        private static bool SortNewBeforeSelected(IGroup group, TreeNode candidate)
        {
            return candidate.Text.CompareTo(group.Name) > 0;
        }

        /// <summary>
        /// Identify favorite index position in nodes collection by default sorting order.
        /// </summary>
        /// <param name="favorite">Not null favorite to identify in nodes collection.</param>
        /// <returns>
        /// -1, if the Group should be added to the end of Group nodes, otherwise found index.
        /// </returns>
        internal int FindFavoriteNodeInsertIndex(IFavorite favorite)
        {
            for (int index = 0; index < nodes.Count; index++)
            {
                var comparedNode = nodes[index] as FavoriteTreeNode;
                if (comparedNode != null && comparedNode.CompareByDefaultFavoriteSorting(favorite) > 0)
                    return comparedNode.Index;
            }

            return -1;
        }
    }
}
