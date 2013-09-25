using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Template container for shared logig for tree nodes update for different type of nodes.
    /// </summary>
    /// <typeparam name="TChanges">Container based on type of changes in persistence</typeparam>
    /// <typeparam name="TNode">Type of tree node to process</typeparam>
    internal abstract class TreeNodesLevelUpdate<TChanges, TNode>
        where TNode : TreeNode
    {
        protected TChanges Changes { get; private set; }

        /// <summary>
        /// Gets not null currently processed tree node.
        /// </summary>
        protected TNode CurrentNode { get; private set; }
        
        protected GroupTreeNode Parent { get; private set; }

        protected TreeNodeCollection Nodes { get; private set; }

        protected abstract bool RemoveCurrent { get; }

        /// <summary>
        /// Gets group nodes on this level. They always precede favorite nodes.
        /// </summary>
        protected IEnumerable<GroupTreeNode> GroupNodes
        {
            get
            {
                var nodes = new TreeListNodes(this.Nodes);
                return nodes.GroupNodes;
            }
        }
        
        /// <summary>
        /// Create new not root level container if parent is defined, otherwise
        /// create root level container. Than this is an entry point of the update.
        /// </summary>
        protected TreeNodesLevelUpdate(TreeNodeCollection nodes, TChanges changes, GroupTreeNode parent = null)
        {
            this.Nodes = nodes;
            this.Changes = changes;
            this.Parent = parent;
        }

        protected void ProcessNodes()
        {
            foreach (TNode toProcess in this.Nodes.OfType<TNode>())
            {
                this.CurrentNode = toProcess;
                this.ProcessCurrentNode();
            }
        }

        private void ProcessCurrentNode()
        {
            if (this.RemoveCurrent)
                this.Nodes.Remove(this.CurrentNode);
            else
                this.UpdateCurrent();
        }

        protected abstract void UpdateCurrent();
    }
}