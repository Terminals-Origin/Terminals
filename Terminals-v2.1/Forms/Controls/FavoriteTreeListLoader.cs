using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Fills tree list with favorites
    /// </summary>
    internal class FavoriteTreeListLoader
    {
        private FavoritesTreeView treeList;

        /// <summary>
        /// gets or sets virtual tree node for favorites, which have no Group defined
        /// </summary>
        private GroupTreeNode unTaggedNode;

        private static IGroups PersistedGroups
        {
            get { return Persistance.Instance.Groups; }
        }

        private static DataDispatcher Dispatcher
        {
            get { return Persistance.Instance.Dispatcher; }
        }

        internal FavoriteTreeListLoader(FavoritesTreeView treeListToFill)
        {
            this.treeList = treeListToFill;

            Dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.OnGroupsCollectionChanged);
            Dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(this.OnFavoritesCollectionChanged);
        }

        /// <summary>
        /// Unregisters the Data dispatcher eventing.
        /// Call this to release the treeview, otherwise it will result in memory gap.
        /// </summary>
        internal void UnregisterEvents()
        {
            Dispatcher.GroupsChanged -= new GroupsChangedEventHandler(this.OnGroupsCollectionChanged);
            Dispatcher.FavoritesChanged -= new FavoritesChangedEventHandler(this.OnFavoritesCollectionChanged);
        }

        private void OnFavoritesCollectionChanged(FavoritesChangedEventArgs args)
        {
            if(IsOrphan())
              return;

            GroupTreeNode selectedGroup = this.treeList.FindSelectedGroupNode();
            IFavorite selectedFavorite = this.treeList.SelectedFavorite;
            RemoveFavorites(args.Removed);
            UpdateFavorites(args.Updated);
            AddNewFavorites(args.Added);
            this.treeList.RestoreSelectedFavorite(selectedGroup, selectedFavorite);
        }

        /// <summary>
        /// This prevents performance problems, when someone forgets to unregister.
        /// Returns true, if the associated treeview is already dead; otherwise false.
        /// </summary>
        private Boolean IsOrphan()
        {
            if (this.treeList.IsDisposed)
            {
                this.UnregisterEvents();
                return true;
            }

            return false;
        }

        private void RemoveFavorites(List<IFavorite> removedFavorites)
        {
            foreach (IFavorite favorite in removedFavorites)
            {
                RemoveFavoriteFromAllGroups(favorite);
                RemoveFavoriteFromGroupNode(this.unTaggedNode, favorite);  
            }
        }

        private void UpdateFavorites(List<IFavorite> updatedFavorites)
        {
            foreach (var favorite in updatedFavorites)
            {
                // remove and then insert instead of tree node update to keep default sorting
                RemoveFavoriteFromAllGroups(favorite);
                this.AddFavoriteToAllItsGroupNodes(favorite);
            }
        }

        private void RemoveFavoriteFromAllGroups(IFavorite favorite)
        {
            foreach (GroupTreeNode groupNode in this.treeList.Nodes)
            {
                RemoveFavoriteFromGroupNode(groupNode, favorite);  
            }
        }

        private void AddNewFavorites(List<IFavorite> addedFavorites)
        {
            foreach (Favorite favorite in addedFavorites)
            {
                this.AddFavoriteToAllItsGroupNodes(favorite);
            }
        }

        private void AddFavoriteToAllItsGroupNodes(IFavorite favorite)
        {
            foreach (IGroup group in favorite.Groups)
            {
                GroupTreeNode groupNode = this.treeList.Nodes[group.Name] as GroupTreeNode;
                AddNewFavoriteNodeToGroupNode(favorite, groupNode);
            }

            if (favorite.Groups.Count == 0)
            {
                AddNewFavoriteNodeToGroupNode(favorite, this.unTaggedNode);
            }
        }

        private static void RemoveFavoriteFromGroupNode(GroupTreeNode groupNode, IFavorite favorite)
        {
            if (groupNode != null && !groupNode.NotLoadedYet)
            {
                var favoriteNode = groupNode.Nodes.Cast<FavoriteTreeNode>()
                    .FirstOrDefault(candidate => candidate.Favorite.Equals(favorite));

                if(favoriteNode != null)
                    groupNode.Nodes.Remove(favoriteNode);
            }
        }

        private static void AddNewFavoriteNodeToGroupNode(IFavorite favorite, GroupTreeNode groupNode)
        {
            if (groupNode != null && !groupNode.NotLoadedYet) // add only to expanded nodes
            {
                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                int index = FindFavoriteNodeInsertIndex(groupNode.Nodes, favorite);
                InsertNodePreservingOrder(groupNode.Nodes, index, favoriteTreeNode);
            }
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
                if (comparedNode.CompareByDefaultFavoriteSorting(favorite) > 0)
                    return index;
            }

            return -1;  
        }

        private void OnGroupsCollectionChanged(GroupsChangedArgs args)
        {
            if (this.IsOrphan())
                return;

            this.RemoveUnusedGroupNodes(args.Removed);
            this.UpdateGroups(args.Updated);
            this.AddMissingGroupNodes(args.Added);
        }

        private void AddMissingGroupNodes(List<IGroup> newGroups)
        {
            foreach (IGroup newGroup in newGroups)
            {
                int index = FindGroupNodeInsertIndex(this.treeList.Nodes, newGroup);
                this.CreateAndAddGroupNode(newGroup, index);
            }
        }

        /// <summary>
        /// Finds the index for the node to insert in nodes collection
        /// and skips nodes before the startIndex.
        /// </summary>
        /// <param name="nodes">Not null nodes collection to search in.</param>
        /// <param name="newGroup">Not empty new Group to add.</param>
        /// <returns>
        /// -1, if the Group should be added to the end of Group nodes, otherwise found index.
        /// </returns>
        private static int FindGroupNodeInsertIndex(TreeNodeCollection nodes, IGroup newGroup)
        {
            // Skips first "Untagged" node to keep it first.
            for (int index = 1; index < nodes.Count; index++)
            {
                if (nodes[index].Text.CompareTo(newGroup.Name) > 0)
                    return index;
            }

            return -1;
        }

        private void UpdateGroups(List<IGroup> updatedGroups)
        {
            List<GroupTreeNode> affectedNodes = GetAffectedNodes(updatedGroups);
            foreach (var groupNode in affectedNodes)
            {
                groupNode.Text = groupNode.Group.Name;
                UpdateGroupNodeChilds(groupNode);
            }
        }

        private static void UpdateGroupNodeChilds(GroupTreeNode groupNode)
        {
            if (groupNode.IsExpanded)
            {
                ReLoadFavoriteNodes(groupNode);
            }
        }

        private void RemoveUnusedGroupNodes(List<IGroup> removedGroups)
        {
            List<GroupTreeNode> affectedNodes = GetAffectedNodes(removedGroups);
            foreach (GroupTreeNode groupNode in affectedNodes)
            {
                this.treeList.Nodes.Remove(groupNode);
            }
        }

        private List<GroupTreeNode> GetAffectedNodes(List<IGroup> requiredGroups)
        {
            return this.treeList.Nodes.Cast<GroupTreeNode>()
                .Where(candidate => requiredGroups.Contains(candidate.Group))
                .ToList();
        }

        /// <summary>
        /// Creates the and add Group node in tree list on proper position defined by index.
        /// This allowes the Group nodes to keep ordered by name.
        /// </summary>
        /// <param name="group">The group to create.</param>
        /// <param name="index">The index on which node would be inserted.
        /// If negative number, than it is added to the end.</param>
        /// <returns>Not null, newly creted node</returns>
        private GroupTreeNode CreateAndAddGroupNode(IGroup group, int index = -1)
        {
            GroupTreeNode groupNode = new GroupTreeNode(group);
            InsertNodePreservingOrder(this.treeList.Nodes, index, groupNode);
            return groupNode;
        }

        private static void InsertNodePreservingOrder(TreeNodeCollection nodes, int index, TreeNode groupNode)
        {
            if (index < 0)
                nodes.Add(groupNode);
            else
                nodes.Insert(index, groupNode);
        }

        internal void LoadGroups()
        {
            if (PersistedGroups == null) // because of designer
                return;

            IGroup untagedVirtualGroup = CreateUntagedVirtualGroup();
            this.unTaggedNode = this.CreateAndAddGroupNode(untagedVirtualGroup);

            foreach (IGroup group in PersistedGroups)
            {
                this.CreateAndAddGroupNode(group);
            }
        }

        internal static IGroup CreateUntagedVirtualGroup()
        {
            var untagedFavorites = Persistance.Instance.Favorites
                .Where(candidate => candidate.Groups.Count == 0)
                .ToList();

            var factory = Persistance.Instance.Factory;
            return factory.CreateGroup(Settings.UNTAGGED_NODENAME, untagedFavorites);
        }

        internal void LoadFavorites(GroupTreeNode groupNode)
        {
            if (groupNode.NotLoadedYet)
            {
                ReLoadFavoriteNodes(groupNode);
            }
        }

        private static void ReLoadFavoriteNodes(GroupTreeNode groupNode) 
        {
            groupNode.Nodes.Clear();
            AddFavoriteNodes(groupNode);
        }

        private static void AddFavoriteNodes(GroupTreeNode groupNode)
        {
            List<IFavorite> favorites = groupNode.Group.Favorites;
            foreach (var favorite in favorites)
            {
                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                groupNode.Nodes.Add(favoriteTreeNode);
            }
        }
    }
}
