using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Process tree view updates for group tree nodes on one level of the tree
    /// </summary>
    internal class GroupsLevelUpdate : TreeNodesLevelUpdate<GroupsChangedArgs, GroupTreeNode>
    {
        protected override bool RemoveCurrent
        {
            get
            {
                return this.CurrentNode.HasGroupIn(this.Changes.Removed);
            }
        }

        private IEnumerable<IGroup> GroupsToAdd
        {
            get
            {
                List<IGroup> toAdd = this.Changes.Added.Where(this.IsThisLevelGroup)
                                                       .ToList();

                IEnumerable<IGroup> moveCandidates = this.MovedGroups();
                toAdd.AddRange(moveCandidates);
                // if persistence sends added and updated at once, than we have to filter the already added
                return toAdd.Distinct();
            }
        }

        /// <summary>
        /// Create new not root level container
        /// </summary>
        private GroupsLevelUpdate(FavoriteIcons favoriteIcons, TreeNodeCollection nodes, GroupsChangedArgs changes,
            GroupTreeNode parent, ToolTipBuilder toolTipBuilder)
            : base(favoriteIcons, nodes, changes, toolTipBuilder, parent)
        {
        }

        /// <summary>
        /// Create root level container. Parent is not defined. This is an entry point of the update.
        /// </summary>
        internal GroupsLevelUpdate(FavoriteIcons favoriteIcons, TreeNodeCollection nodes, GroupsChangedArgs changes,
            ToolTipBuilder toolTipBuilder)
            : base(favoriteIcons, nodes, changes, toolTipBuilder)
        {
        }

        /// <summary>
        /// Selects updated groups, which do not have their node on this level yet
        /// </summary>
        private IEnumerable<IGroup> MovedGroups()
        {
            return this.Changes.Updated.Where(candidate => this.IsThisLevelGroup(candidate) &&
                                                          !this.NodeAlreadyPresent(candidate));
        }

        private bool NodeAlreadyPresent(IGroup candidate)
        {
            return this.GroupNodes.Any(node => node.Group.StoreIdEquals(candidate));
        }

        private bool IsThisLevelGroup(IGroup candidate)
        {
            return this.CandidateAndParentAreRoot(candidate) ||
                   this.CandidateParentAndParentEquals(candidate);
        }

        private bool CandidateAndParentAreRoot(IGroup candidate)
        {
            return this.Parent == null && candidate.Parent == null;
        }

        private bool CandidateParentAndParentEquals(IGroup candidate)
        {
            // because we start from the root nodes, the parent is already updated
            return this.Parent != null &&
                   candidate.Parent != null &&
                   candidate.Parent.StoreIdEquals(this.Parent.Group);
        }

        internal void Run()
        {
            this.ProcessNodes();
            // now it is effective to add the rest
            this.AddMissingGroupNodes();
        }

        private void AddMissingGroupNodes()
        {
            foreach (IGroup newGroup in this.GroupsToAdd)
            {
                int index = this.Nodes.FindGroupNodeInsertIndex(newGroup);
                this.Nodes.InsertGroupNode(newGroup, index);
            }
        }

        /// <summary>
        /// Always perform recursive update, because udated group may also live deeper in tree structure
        /// </summary>
        protected override void UpdateCurrent()
        {
            this.UpdateContent();
            
            // the groupNode has survived the update
            if (!this.CurrentNode.IsOrphan)
                this.UpdateGroupNodeChilds();
        }

        /// <summary>
        /// Perform move or Rename of current node.
        /// </summary>
        private void UpdateContent()
        {
            IGroup updatedGroup = this.SelectUpdatedGroup();
            if (updatedGroup == null)
                return;

            // move or the rename may result in changing of the tree node position => remove always
            this.CurrentNode.Remove();

            // if the parent has changed, the tree node should appear on another level
            if (this.IsThisLevelGroup(updatedGroup))
              this.UpdateGroupByRename(updatedGroup);
        }

        private IGroup SelectUpdatedGroup()
        {
            IGroup currentGroup = this.CurrentNode.Group;
            return this.Changes.Updated.FirstOrDefault(candidate => candidate.StoreIdEquals(currentGroup));
        }

        private void UpdateGroupByRename(IGroup updatedGroup)
        {
            int index = this.Nodes.FindGroupNodeInsertIndex(updatedGroup);
            this.Nodes.InsertNodePreservingOrder(index, this.CurrentNode);
            // dont apply the name before we fix the position
            this.CurrentNode.UpdateByGroup(updatedGroup);
        }

        private void UpdateGroupNodeChilds()
        {
            // take only expanded nodes, for better performance and to protect the lazy loading
            if (this.CurrentNode.NotLoadedYet)
                return;

            var nextLevelUpdate = new GroupsLevelUpdate(this.FavoriteIcons, this.CurrentNode.Nodes, this.Changes, this.CurrentNode, this.ToolTipBuilder);
            nextLevelUpdate.Run();
        }
    }
}