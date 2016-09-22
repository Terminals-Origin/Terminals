using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

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

        private readonly TreeNodeCollection nodes;

        protected ToolTipBuilder ToolTipBuilder { get; private set; }

        protected TreeListNodes Nodes { get; private set; }

        protected abstract bool RemoveCurrent { get; }

        /// <summary>
        /// Gets group nodes on this level. They always precede favorite nodes.
        /// </summary>
        protected IEnumerable<GroupTreeNode> GroupNodes
        {
            get
            {
                return this.Nodes.GroupNodes;
            }
        }
        
        /// <summary>
        /// Create new not root level container if parent is defined, otherwise
        /// create root level container. Than this is an entry point of the update.
        /// </summary>
        protected TreeNodesLevelUpdate(TreeNodeCollection nodes, TChanges changes, ToolTipBuilder toolTipBuilder, GroupTreeNode parent = null)
        {
            this.nodes = nodes;
            this.ToolTipBuilder = toolTipBuilder;
            this.Nodes = new TreeListNodes(nodes, toolTipBuilder);
            this.Changes = changes;
            this.Parent = parent;
        }

        protected void ProcessNodes()
        {
            // because making changes in the collection during procesing each node, call ToList to evaluate the collection
            foreach (TNode toProcess in this.nodes.OfType<TNode>().ToList())
            {
                this.CurrentNode = toProcess;
                this.ProcessCurrentNode();
            }
        }

        private void ProcessCurrentNode()
        {
            if (this.RemoveCurrent)
                this.nodes.Remove(this.CurrentNode);
            else
                this.UpdateCurrent();
        }

        protected abstract void UpdateCurrent();
    }
}