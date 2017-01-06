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
                // select unique favorites, because add new sends at once added and update
                return toAdd.Distinct();
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
        private FavoritesLevelUpdate(FavoriteIcons favoriteIcons, TreeNodeCollection nodes, FavoritesChangedEventArgs changes,
            GroupTreeNode parent, ToolTipBuilder toolTipBuilder)
            : base(favoriteIcons, nodes, changes, toolTipBuilder, parent)
        {
        }

        /// <summary>
        /// Create root level container. Parent is not defined. This is an entry point of the update.
        /// </summary>
        internal FavoritesLevelUpdate(FavoriteIcons favoriteIcons, TreeNodeCollection nodes, FavoritesChangedEventArgs changes,
            ToolTipBuilder toolTipBuilder)
            : base(favoriteIcons, nodes, changes, toolTipBuilder)
        {
        }

        private IEnumerable<IFavorite> MovedFavorites()
        {
            return this.Changes.Updated.Where(candidate => this.IsThisLevelFavorite(candidate) &&
                                                          !this.NodeAlreadyPresent(candidate));
        }

        private bool NodeAlreadyPresent(IFavorite candidate)
        {
            return this.Nodes.ContainsFavoriteNode(candidate);
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
            this.Nodes.InsertFavorites(this.FavoritesToAdd);
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
            int index = this.Nodes.FindFavoriteNodeInsertIndex(updatedFavorite);
            this.Nodes.InsertNodePreservingOrder(index, this.CurrentNode);

            // dont apply the name before we fix the position
            string toolTip = this.ToolTipBuilder.BuildTooTip(updatedFavorite);
            this.CurrentNode.UpdateByFavorite(updatedFavorite, toolTip);
        }

        private IFavorite SelectUpdatedFavorite()
        {
            IFavorite favorite = this.CurrentNode.Favorite;
            return this.Changes.Updated.FirstOrDefault(candidate => candidate.StoreIdEquals(favorite));
        }

        private void UpdateGroupNodeChilds()
        {
            // take only expanded nodes, for better performance and to protect the lazy loading
            foreach (GroupTreeNode groupNode in this.LoadedGroupNodes)
            {
                var levelUpdate = new FavoritesLevelUpdate(this.FavoriteIcons, groupNode.Nodes, this.Changes, groupNode, this.ToolTipBuilder);
                levelUpdate.Run();
            }
        }
    }
}
