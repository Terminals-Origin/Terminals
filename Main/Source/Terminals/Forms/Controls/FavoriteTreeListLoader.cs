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

            var nodes = new TreeListNodes(this.RootNodes);
            // dont load everything, it is done by lazy loading after expand
            IOrderedEnumerable<IGroup> rootGroups = GetSortedRootGroups();
            nodes.AddChildGroupNodes(rootGroups);
            List<IFavorite> untaggedFavorites = GetUntaggedFavorites(this.favorites);
            nodes.AddFavoriteNodes(untaggedFavorites);
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
            var nodes = new TreeListNodes(groupNode.Nodes);
            nodes.AddFavoriteNodes(groupNode.Favorites);
        }

        private void AddGroupNodes(GroupTreeNode groupNode)
        {
            IEnumerable<IGroup> childGroups = this.GetChildGroups(groupNode.Group);
            var nodes = new TreeListNodes(groupNode.Nodes);
            nodes.AddChildGroupNodes(childGroups);
        }

        private IEnumerable<IGroup> GetChildGroups(IGroup current)
        {
            return this.persistedGroups.Where(candidate => candidate.Parent != null && candidate.Parent.StoreIdEquals(current))
                                  .OrderBy(group => group.Name);
        }
    }
}
