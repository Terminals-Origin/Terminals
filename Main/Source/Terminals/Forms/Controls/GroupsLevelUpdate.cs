using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Process tree view updates for group tree nodes on one level of the tree
    /// </summary>
    internal class GroupsLevelUpdate
    {
        private readonly GroupTreeNode parent;

        private readonly GroupsChangedArgs changes;

        private readonly TreeNodeCollection nodes;

        /// <summary>
        /// Gets group nodes on this level. They always precede favorite nodes.
        /// </summary>
        private List<GroupTreeNode> GroupNodes
        {
            get { return this.nodes.OfType<GroupTreeNode>().ToList(); }
        }

        /// <summary>
        /// Currently processed tree node.
        /// </summary>
        private GroupTreeNode currentNode;
        
        private bool RemoveCurrent
        {
            get
            {
                return this.currentNode.HasGroupIn(this.changes.Removed);
            }
        }

        private IEnumerable<IGroup> GroupsToAdd
        {
            get
            {
                List<IGroup> toAdd = this.changes.Added.Where(this.IsThisLevelGroup)
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
            : this(nodes, changes)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Create root level container. Parent is not defined. This is an entry point of the update.
        /// </summary>
        internal GroupsLevelUpdate(TreeNodeCollection nodes, GroupsChangedArgs changes)
        {
            this.nodes = nodes;
            this.changes = changes;
        }

        /// <summary>
        /// Selects updated groups, which do not have their node on this level yet
        /// </summary>
        private IEnumerable<IGroup> MovedGroups()
        {
            return this.changes.Updated.Where(candidate => this.IsThisLevelGroup(candidate) &&
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
            return this.parent == null && candidate.Parent == null;
        }

        private bool CandidateParentAndParentEquals(IGroup candidate)
        {
            // because we start from the root nodes, the parent is already updated
            return this.parent != null &&
                   candidate.Parent != null &&
                   candidate.Parent.StoreIdEquals(this.parent.Group);
        }

        internal void Run()
        {
            foreach (GroupTreeNode groupNode in this.GroupNodes)
            {
                this.currentNode = groupNode;
                this.ProcessCurrentNode();
            }

            // now it is effective to add the rest
            this.AddMissingGroupNodes();
        }

        private void ProcessCurrentNode()
        {
            if (this.RemoveCurrent)
                this.nodes.Remove(this.currentNode);
            else
                this.UpdateGroup();
        }

        private void AddMissingGroupNodes()
        {
            foreach (IGroup newGroup in this.GroupsToAdd)
            {
                int index = this.FindGroupNodeInsertIndex(newGroup);
                FavoriteTreeListLoader.CreateAndAddGroupNode(this.nodes, newGroup, index);
            }
        }

        /// <summary>
        /// Always perform recursive update, because udated group may also live deeper in tree structure
        /// </summary>
        private void UpdateGroup()
        {
            this.UpdateContent();
            
            // the groupNode has survived the update
            if (!this.currentNode.IsOrphan)
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
            this.currentNode.Remove();

            // if the parent has changed, the tree node should appear on another level
            if (this.IsThisLevelGroup(updatedGroup))
              this.UpdateGroupByRename(updatedGroup);
        }

        private IGroup SelectUpdatedGroup()
        {
            IGroup currentGroup = this.currentNode.Group;
            return this.changes.Updated.FirstOrDefault(candidate => candidate.StoreIdEquals(currentGroup));
        }

        private void UpdateGroupByRename(IGroup updatedGroup)
        {
            int index = this.FindGroupNodeInsertIndex(updatedGroup);
            FavoriteTreeListLoader.InsertNodePreservingOrder(this.nodes, index, this.currentNode);
            
            // dont apply the name before we fix the position
            this.currentNode.UpdateByGroup(updatedGroup);
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
            foreach (TreeNode treeNode in this.nodes)
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
            if (this.currentNode.NotLoadedYet)
                return;

            var nextLevelUpdate = new GroupsLevelUpdate(this.currentNode.Nodes, this.changes, this.currentNode);
            nextLevelUpdate.Run();
        }
    }
}