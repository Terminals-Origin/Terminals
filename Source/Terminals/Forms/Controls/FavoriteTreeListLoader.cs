using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Fills tree list with group and favorites.
    /// Handles Persistence updates.
    /// Handles expansion request on group tree nodes, which arent loaded yet (tree lazy loading).
    /// </summary>
    internal class FavoriteTreeListLoader
    {
        private readonly FavoritesTreeView treeList;

        private readonly IGroups persistedGroups;

        private readonly DataDispatcher dispatcher;

        private readonly IFavorites favorites;

        private TreeNodeCollection RootNodes
        {
            get { return this.treeList.Nodes; }
        }

        /// <summary>
        /// This prevents performance problems, when someone forgets to unregister.
        /// Returns true, if the associated treeview is already dead; otherwise false.
        /// </summary>
        private Boolean IsOrphan
        {
            get
            {
                return this.treeList.IsDisposed;
            }
        }

        internal FavoriteTreeListLoader(FavoritesTreeView treeListToFill, IPersistence persistence)
        {
            this.treeList = treeListToFill;
            this.treeList.AfterExpand += new TreeViewEventHandler(this.OnTreeViewExpand);

            this.persistedGroups = persistence.Groups;
            this.favorites = persistence.Favorites;
            this.dispatcher = persistence.Dispatcher;
            this.dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.OnGroupsCollectionChanged);
            this.dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(this.OnFavoritesCollectionChanged);
        }

        private void OnTreeViewExpand(object sender, TreeViewEventArgs e)
        {
            GroupTreeNode groupNode = e.Node as GroupTreeNode;
            if (groupNode != null)
            {
                this.LoadGroupNode(groupNode);
            }
        }

        /// <summary>
        /// Unregisters the Data dispatcher eventing.
        /// Call this to release the treeview, otherwise it will result in memory gap.
        /// </summary>
        internal void UnregisterEvents()
        {
            this.dispatcher.GroupsChanged -= new GroupsChangedEventHandler(this.OnGroupsCollectionChanged);
            this.dispatcher.FavoritesChanged -= new FavoritesChangedEventHandler(this.OnFavoritesCollectionChanged);
            this.treeList.AfterExpand -= new TreeViewEventHandler(this.OnTreeViewExpand);
        }

        private void OnFavoritesCollectionChanged(FavoritesChangedEventArgs args)
        {
            if (IsOrphan)
                this.UnregisterEvents();
            else
                this.PerformFavoritesUpdate(args);
        }

        private void PerformFavoritesUpdate(FavoritesChangedEventArgs args)
        {
            GroupTreeNode selectedGroup = this.treeList.FindSelectedGroupNode();
            IFavorite selectedFavorite = this.treeList.SelectedFavorite;
            var updater = new FavoritesLevelUpdate(this.RootNodes, args);
            updater.Run();
            this.treeList.RestoreSelectedFavorite(selectedGroup, selectedFavorite);
        }

        private void OnGroupsCollectionChanged(GroupsChangedArgs args)
        {
            if (IsOrphan)
            {
                this.UnregisterEvents();
            }
            else
            {
                var levelParams = new GroupsLevelUpdate(this.RootNodes, args);
                levelParams.Run();
            }
        }

        internal void LoadGroups()
        {
            if (persistedGroups == null) // because of designer
                return;

            // dont load everything, it is done by lazy loading after expand
            IOrderedEnumerable<IGroup> rootGroups = GetSortedRootGroups();
            AddChildGroupNodes(this.RootNodes, rootGroups);
            List<IFavorite> untaggedFavorite = GetUntaggedFavorites(this.favorites);
            AddFavoriteNodes(this.RootNodes, untaggedFavorite);
        }

        internal static List<IFavorite> GetUntaggedFavorites(IEnumerable<IFavorite> favorites)
        {
            IEnumerable<IFavorite> relevantFavorites = favorites.Where(candidate => candidate.Groups.Count == 0);
            return Favorites.OrderByDefaultSorting(relevantFavorites);
        }

        private IOrderedEnumerable<IGroup> GetSortedRootGroups()
        {
            return this.persistedGroups.Where(candidate => candidate.Parent == null)
                                  .OrderBy(group => group.Name);
        }

        private void LoadGroupNode(GroupTreeNode groupNode)
        {
            if (!groupNode.NotLoadedYet)
                return;

            groupNode.Nodes.Clear();
            this.AddGroupNodes(groupNode);
            AddFavoriteNodes(groupNode.Nodes, groupNode.Favorites);
        }

        private void AddGroupNodes(GroupTreeNode groupNode)
        {
            IEnumerable<IGroup> childGroups = this.GetChildGroups(groupNode.Group);
            AddChildGroupNodes(groupNode.Nodes, childGroups);
        }

        private IEnumerable<IGroup> GetChildGroups(IGroup current)
        {
            return this.persistedGroups.Where(candidate => candidate.Parent != null && candidate.Parent.StoreIdEquals(current))
                                  .OrderBy(group => group.Name);
        }

        private static void AddChildGroupNodes(TreeNodeCollection nodes, IEnumerable<IGroup> sortedGroups)
        {
            foreach (IGroup group in sortedGroups)
            {
                CreateAndAddGroupNode(nodes, group);
            }
        }

        /// <summary>
        /// Creates the and add Group node in tree list on proper position defined by index.
        /// This allowes the Group nodes to keep ordered by name.
        /// </summary>
        /// <param name="nodes">Not null collection of nodes, where to add new child.</param>
        /// <param name="group">The group to create.</param>
        /// <param name="index">The index on which node would be inserted.
        /// If negative number, than it is added to the end.</param>
        internal static void CreateAndAddGroupNode(TreeNodeCollection nodes, IGroup group, int index = -1)
        {
            var groupNode = new GroupTreeNode(group);
            InsertNodePreservingOrder(nodes, index, groupNode);
        }

        private static void AddFavoriteNodes(TreeNodeCollection nodes, List<IFavorite> favorites)
        {
            foreach (IFavorite favorite in favorites)
            {
                AddFavoriteNode(nodes, favorite);
            }
        }

        private static void AddFavoriteNode(TreeNodeCollection nodes, IFavorite favorite)
        {
            var favoriteTreeNode = new FavoriteTreeNode(favorite);
            nodes.Add(favoriteTreeNode);
        }

        /// <summary>
        /// Identify favorite index position in nodes collection by default sorting order.
        /// </summary>
        /// <param name="nodes">Not null nodes collection of FavoriteTreeNodes to search in.</param>
        /// <param name="favorite">Not null favorite to identify in nodes collection.</param>
        /// <returns>
        /// -1, if the Group should be added to the end of Group nodes, otherwise found index.
        /// </returns>
        internal static int FindFavoriteNodeInsertIndex(TreeNodeCollection nodes, IFavorite favorite)
        {
            for (int index = 0; index < nodes.Count; index++)
            {
                var comparedNode = nodes[index] as FavoriteTreeNode;
                if (comparedNode != null && comparedNode.CompareByDefaultFavoriteSorting(favorite) > 0)
                    return index;
            }

            return -1;
        }

        internal static void InsertNodePreservingOrder(TreeNodeCollection nodes, int index, TreeNode groupNode)
        {
            if (index < 0)
                nodes.Add(groupNode);
            else
                nodes.Insert(index, groupNode);
        }
    }
}
