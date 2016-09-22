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

        private readonly ToolTipBuilder toolTipBuilder;

        internal IEnumerable<FavoriteTreeNode> FavoriteNodes
        {
            get
            {
                return FilterFavoriteNodes(this.nodes);
            }
        }

        internal IEnumerable<GroupTreeNode> GroupNodes
        {
            get
            {
                return FilterGroupNodes(this.nodes);
            }
        }

        public TreeListNodes(TreeNodeCollection nodes, ToolTipBuilder toolTipBuilder)
        {
            this.nodes = nodes;
            this.toolTipBuilder = toolTipBuilder;
        }

        private static List<GroupTreeNode> FilterGroupNodes(TreeNodeCollection nodes)
        {
            return nodes.OfType<GroupTreeNode>()
                .ToList();
        }

        internal static List<FavoriteTreeNode> FilterFavoriteNodes(TreeNodeCollection nodes)
        {
            return nodes.OfType<FavoriteTreeNode>()
                .ToList();
        }

        /// <summary>
        /// Gets not null collection of favorites obtained using recursive search from all favorite nodes.
        /// </summary>
        internal static List<IFavorite> FindAllCheckedFavorites(TreeNodeCollection nodes)
        {
            var favorites = new List<IFavorite>();
            FindAllCheckedFavorites(nodes, favorites);
            return favorites;
        }

        private static void FindAllCheckedFavorites(TreeNodeCollection nodes, List<IFavorite> favorites)
        {
            var candidates = FindCheckedFavorites(nodes);
            favorites.AddRange(candidates);

            foreach (GroupTreeNode groupNode in FilterGroupNodes(nodes))
            {
                // dont expect only Favorite nodes, because of group nodes on the same level
                groupNode.ExpandCheckedGroupNode();
                FindAllCheckedFavorites(groupNode.Nodes, favorites);
            }
        }

        private static IEnumerable<IFavorite> FindCheckedFavorites(TreeNodeCollection nodes)
        {
            return FilterFavoriteNodes(nodes).Where(node => node.Checked)
                                     .Select(node => node.Favorite);
        }

        internal static void CheckChildNodesRecursive(TreeNodeCollection nodes, bool checkState)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = checkState;
                CheckChildNodesRecursive(node.Nodes, checkState);
            }
        }

        internal bool ContainsFavoriteNode(IFavorite favorite)
        {
            return ContainsFavoriteNode(this.nodes, favorite);
        }

        internal static bool ContainsFavoriteNode(TreeNodeCollection nodes, IFavorite favorite)
        {
            return FilterFavoriteNodes(nodes).Any(node => node.Favorite.StoreIdEquals(favorite));
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
            string toolTip = this.toolTipBuilder.BuildTooTip(favorite);
            var favoriteTreeNode = new FavoriteTreeNode(favorite, toolTip);
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
