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
                return toAdd;
            }
        }

        /// <summary>
        /// Create new not root level container
        /// </summary>
        private GroupsLevelUpdate(TreeNodeCollection nodes, GroupsChangedArgs changes, GroupTreeNode parent)
            : base(nodes, changes, parent)
        {
        }

        /// <summary>
        /// Create root level container. Parent is not defined. This is an entry point of the update.
        /// </summary>
        internal GroupsLevelUpdate(TreeNodeCollection nodes, GroupsChangedArgs changes)
            : base(nodes, changes)
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
            return this.CurrentNodes.Any(node => node.Group.StoreIdEquals(candidate));
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
                int index = this.FindGroupNodeInsertIndex(newGroup);
                FavoriteTreeListLoader.CreateAndAddGroupNode(this.Nodes, newGroup, index);
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
            int index = this.FindGroupNodeInsertIndex(updatedGroup);
            FavoriteTreeListLoader.InsertNodePreservingOrder(this.Nodes, index, this.CurrentNode);
            
            // dont apply the name before we fix the position
            this.CurrentNode.UpdateByGroup(updatedGroup);
        }

        /// <summary>
        /// Finds the index for the node to insert in nodes collection
        /// and skips nodes before the startIndex.
        /// </summary>
        /// <param name="newGroup">Not empty new Group to add.</param>
        /// <returns>
        /// -1, if the Group should be added to the end of Group nodes, otherwise found index.
        /// </returns>
        private int FindGroupNodeInsertIndex(IGroup newGroup)
        {
            // take all, we have to find place, where favorite Nodes start
            foreach (TreeNode treeNode in this.Nodes)
            {
                // reached end of group nodes, all following are Favorite nodes
                // or search index between group nodes
                if (treeNode is FavoriteTreeNode || SortNewBeforeSelected(newGroup, treeNode))
                    return treeNode.Index;
            }

            return -1;
        }

        private static bool SortNewBeforeSelected(IGroup newGroup, TreeNode candidate)
        {
            return candidate.Text.CompareTo(newGroup.Name) > 0;
        }

        private void UpdateGroupNodeChilds()
        {
            // take only expanded nodes, for better performance and to protect the lazy loading
            if (this.CurrentNode.NotLoadedYet)
                return;

            var nextLevelUpdate = new GroupsLevelUpdate(this.CurrentNode.Nodes, this.Changes, this.CurrentNode);
            nextLevelUpdate.Run();
        }
    }
}