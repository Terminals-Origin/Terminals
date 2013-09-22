using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// One level of favorites updated by persistence arguments
    /// </summary>
    internal class FavoritesLevelUpdate
    {
        private readonly TreeNodeCollection nodes;

        private readonly FavoritesChangedEventArgs changes;

        internal FavoritesLevelUpdate(TreeNodeCollection nodes, FavoritesChangedEventArgs changes)
        {
            this.nodes = nodes;
            this.changes = changes;
        }

        internal void Run()
        {
            AddNewFavorites(this.nodes, this.changes.Added);
            UpdateFavorites(this.nodes, this.changes.Updated);
            RemoveFavorites(this.nodes, this.changes.Removed);
        }

        private static void RemoveFavorites(TreeNodeCollection rootNodes, List<IFavorite> removedFavorites)
        {
            foreach (IFavorite favorite in removedFavorites)
            {
                RemoveFavoriteFromAllGroups(rootNodes, favorite);
            }
        }

        private static void RemoveFavoriteFromAllGroups(TreeNodeCollection rootNodes, IFavorite favorite)
        {
            foreach (GroupTreeNode groupNode in rootNodes.OfType<GroupTreeNode>())
            {
                RemoveFavoriteFromGroupNode(groupNode, favorite);
            }
        }

        private static void RemoveFavoriteFromGroupNode(GroupTreeNode groupNode, IFavorite favorite)
        {
            if (groupNode != null && !groupNode.NotLoadedYet)
            {
                var favoriteNode = groupNode.Nodes.OfType<FavoriteTreeNode>()
                    .FirstOrDefault(candidate => candidate.Favorite.StoreIdEquals(favorite));

                if (favoriteNode != null)
                    groupNode.Nodes.Remove(favoriteNode);
            }
        }

        private static void UpdateFavorites(TreeNodeCollection rootNodes, List<IFavorite> updatedFavorites)
        {
            foreach (var favorite in updatedFavorites)
            {
                // remove and then insert instead of tree node update to keep default sorting
                RemoveFavoriteFromAllGroups(rootNodes, favorite);
                AddFavoriteToAllItsGroupNodes(rootNodes, favorite);
            }
        }

        private static void AddFavoriteToAllItsGroupNodes(TreeNodeCollection rootNodes, IFavorite favorite)
        {
            //todo missing recursion
            foreach (IGroup group in favorite.Groups)
            {
                var groupNode = rootNodes[group.Name] as GroupTreeNode;
                AddNewFavoriteNodeToGroupNode(favorite, groupNode);
            }

            if (favorite.Groups.Count == 0)
            {
                // todo add to the root favorites
            }
        }

        private static void AddNewFavoriteNodeToGroupNode(IFavorite favorite, GroupTreeNode groupNode)
        {
            if (groupNode != null && !groupNode.NotLoadedYet) // add only to expanded nodes
            {
                bool alreadyThere = groupNode.ContainsFavoriteNode(favorite);
                if (alreadyThere)
                    return;

                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                int index = FavoriteTreeListLoader.FindFavoriteNodeInsertIndex(groupNode.Nodes, favorite);
                FavoriteTreeListLoader.InsertNodePreservingOrder(groupNode.Nodes, index, favoriteTreeNode);
            }
        }

        private static void AddNewFavorites(TreeNodeCollection rootNodes, List<IFavorite> addedFavorites)
        {
            foreach (IFavorite favorite in addedFavorites)
            {
                AddFavoriteToAllItsGroupNodes(rootNodes, favorite);
            }
        }
    }
}
