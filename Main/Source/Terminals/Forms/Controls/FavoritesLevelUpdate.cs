using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// One level of favorites tree view nodes updated by persistence arguments
    /// </summary>
    internal class FavoritesLevelUpdate : TreeNodesLevelUpdate<FavoritesChangedEventArgs, FavoriteTreeNode>
    {
        protected override bool RemoveCurrent
        {
            get
            {
                return this.CurrentNode.HasFavoriteIn(this.Changes.Removed);
            }
        }

        private IEnumerable<IFavorite> FavoritesToAdd
        {
            get
            {
                List<IFavorite> toAdd = this.Changes.Added.Where(this.IsThisLevelFavorite)
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
                return this.GroupNodes.Where(groupNode => !groupNode.NotLoadedYet);
            }
        }

        /// <summary>
        /// Create new not root level container
        /// </summary>
        private FavoritesLevelUpdate(TreeNodeCollection nodes, FavoritesChangedEventArgs changes, GroupTreeNode parent)
            : base(nodes, changes, parent)
        {
        }

        /// <summary>
        /// Create root level container. Parent is not defined. This is an entry point of the update.
        /// </summary>
        internal FavoritesLevelUpdate(TreeNodeCollection nodes, FavoritesChangedEventArgs changes)
            : base(nodes, changes)
        {
        }

        private IEnumerable<IFavorite> MovedFavorites()
        {
            return this.Changes.Updated.Where(candidate => this.IsThisLevelFavorite(candidate) &&
                                                          !this.NodeAlreadyPresent(candidate));
        }

        private bool NodeAlreadyPresent(IFavorite candidate)
        {
            return this.CurrentNodes.Any(node => node.Favorite.StoreIdEquals(candidate));
        }

        private bool IsThisLevelFavorite(IFavorite candidate)
        {
            return this.CandidateAndParentAreRoot(candidate) ||
                   this.CandidateParentAndParentEquals(candidate);
        }

        private bool CandidateAndParentAreRoot(IFavorite candidate)
        {
            return this.Parent == null && !candidate.Groups.Any();
        }

        private bool CandidateParentAndParentEquals(IFavorite candidate)
        {
            // because we start from the root nodes, the parent is already updated
            return this.Parent != null &&
                   candidate.Groups.Any(group => group.StoreIdEquals(this.Parent.Group));
        }

        internal void Run()
        {
            this.ProcessNodes();
            // now it is effective to add the rest
            this.AddMissingNodes();
            this.UpdateGroupNodeChilds();
        }

        protected override void UpdateCurrent()
        {
            IFavorite updatedFavorite = this.SelectUpdatedFavorite();
            if (updatedFavorite == null)
                return;

            // move or the rename may result in changing of the tree node position => remove always
            this.CurrentNode.Remove();

            // if the parent has changed, the tree node should appear on another level
            if (this.IsThisLevelFavorite(updatedFavorite))
                this.UpdateFavoriteByRename(updatedFavorite);
        }

        private void UpdateFavoriteByRename(IFavorite updatedFavorite)
        {
            int index = FindFavoriteNodeInsertIndex(this.Nodes, updatedFavorite);
            FavoriteTreeListLoader.InsertNodePreservingOrder(this.Nodes, index, this.CurrentNode);

            // dont apply the name before we fix the position
            this.CurrentNode.UpdateByFavorite(updatedFavorite);
        }

        private IFavorite SelectUpdatedFavorite()
        {
            IFavorite favorite = this.CurrentNode.Favorite;
            return this.Changes.Updated.FirstOrDefault(candidate => candidate.StoreIdEquals(favorite));
        }

        private void AddMissingNodes()
        {
            foreach (IFavorite favorite in this.FavoritesToAdd)
            {
                int index = FindFavoriteNodeInsertIndex(this.Nodes, favorite);
                FavoriteTreeListLoader.CreateAndAddFavoriteNode(this.Nodes, favorite, index);
            }
        }

        private void UpdateGroupNodeChilds()
        {
            // take only expanded nodes, for better performance and to protect the lazy loading
            foreach (GroupTreeNode groupNode in this.LoadedGroupNodes)
            {
                var levelUpdate = new FavoritesLevelUpdate(groupNode.Nodes, this.Changes, groupNode);
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
