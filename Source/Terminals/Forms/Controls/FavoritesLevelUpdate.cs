using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// One level of favorites tree view nodes updated by persistence arguments
    /// </summary>
    internal class FavoritesLevelUpdate
    {
        private readonly GroupTreeNode parent;
        
        private readonly TreeNodeCollection nodes;

        private readonly FavoritesChangedEventArgs changes;

        private IEnumerable<FavoriteTreeNode> FavoriteNodes
        {
            get 
            {
                return this.nodes.OfType<FavoriteTreeNode>()
                                 .ToList();
            }
        }

        private FavoriteTreeNode currentNode;

        private bool RemoveCurrent
        {
            get
            {
                return this.currentNode.HasFavoriteIn(this.changes.Removed);
            }
        }

        private IEnumerable<IFavorite> FavoritesToAdd
        {
            get
            {
                List<IFavorite> toAdd = this.changes.Added.Where(this.IsThisLevelFavorite)
                                         .ToList();

                IEnumerable<IFavorite> moved = this.MovedFavorites();
                toAdd.AddRange(moved);
                return toAdd;
            }
        }

        private IEnumerable<GroupTreeNode> LoadedGroupNodes
        {
            get
            {
                return this.nodes.OfType<GroupTreeNode>()
                                 .Where(groupNode => !groupNode.NotLoadedYet);
            }
        }

        /// <summary>
        /// Create new not root level container
        /// </summary>
        private FavoritesLevelUpdate(TreeNodeCollection nodes, FavoritesChangedEventArgs changes, GroupTreeNode parent)
            : this(nodes, changes)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Create root level container. Parent is not defined. This is an entry point of the update.
        /// </summary>
        internal FavoritesLevelUpdate(TreeNodeCollection nodes, FavoritesChangedEventArgs changes)
        {
            this.nodes = nodes;
            this.changes = changes;
        }

        private IEnumerable<IFavorite> MovedFavorites()
        {
            return this.changes.Updated.Where(candidate => this.IsThisLevelFavorite(candidate) &&
                                                          !this.NodeAlreadyPresent(candidate));
        }

        private bool NodeAlreadyPresent(IFavorite candidate)
        {
            return this.FavoriteNodes.Any(node => node.Favorite.StoreIdEquals(candidate));
        }

        private bool IsThisLevelFavorite(IFavorite candidate)
        {
            return this.CandidateAndParentAreRoot(candidate) ||
                   this.CandidateParentAndParentEquals(candidate);
        }

        private bool CandidateAndParentAreRoot(IFavorite candidate)
        {
            return this.parent == null && !candidate.Groups.Any();
        }

        private bool CandidateParentAndParentEquals(IFavorite candidate)
        {
            // because we start from the root nodes, the parent is already updated
            return this.parent != null &&
                   candidate.Groups.Any(group => group.StoreIdEquals(this.parent.Group));
        }

        internal void Run()
        {
            foreach (FavoriteTreeNode favoriteNode in this.FavoriteNodes)
            {
                this.currentNode = favoriteNode;
                this.ProcessCurrentNode();
            }

            // now it is effective to add the rest
            this.AddMissingNodes();
            this.UpdateGroupNodeChilds();
        }

        private void ProcessCurrentNode()
        {
            if (this.RemoveCurrent)
                this.nodes.Remove(this.currentNode);
            else
                this.UpdateFavorite();
        }

        private void UpdateFavorite()
        {
            IFavorite updatedFavorite = this.SelectUpdatedFavorite();
            if (updatedFavorite == null)
                return;

            // move or the rename may result in changing of the tree node position => remove always
            this.currentNode.Remove();

            // if the parent has changed, the tree node should appear on another level
            if (this.IsThisLevelFavorite(updatedFavorite))
                this.UpdateFavoriteByRename(updatedFavorite);
        }

        private void UpdateFavoriteByRename(IFavorite updatedFavorite)
        {
            int index = FindFavoriteNodeInsertIndex(this.nodes, updatedFavorite);
            FavoriteTreeListLoader.InsertNodePreservingOrder(this.nodes, index, this.currentNode);

            // dont apply the name before we fix the position
            this.currentNode.UpdateByFavorite(updatedFavorite);
        }

        private IFavorite SelectUpdatedFavorite()
        {
            IFavorite favorite = this.currentNode.Favorite;
            return this.changes.Updated.FirstOrDefault(candidate => candidate.StoreIdEquals(favorite));
        }

        private void AddMissingNodes()
        {
            foreach (IFavorite favorite in this.FavoritesToAdd)
            {
                int index = FindFavoriteNodeInsertIndex(this.nodes, favorite);
                FavoriteTreeListLoader.CreateAndAddFavoriteNode(this.nodes, favorite, index);
            }
        }

        private void UpdateGroupNodeChilds()
        {
            // take only expanded nodes, for better performance and to protect the lazy loading
            foreach (GroupTreeNode groupNode in this.LoadedGroupNodes)
            {
                var levelUpdate = new FavoritesLevelUpdate(groupNode.Nodes, this.changes, groupNode);
                levelUpdate.Run();
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
                if (comparedNode != null && comparedNode.CompareByDefaultFavoriteSorting(favorite) > 0)
                    return comparedNode.Index;
            }

            return -1;
        }
    }
}
