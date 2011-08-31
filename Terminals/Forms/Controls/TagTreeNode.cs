using System;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Tree node for tags, this simulates lazy loading using dummy node,
    /// until first expansion, where favorite nodes should replace the dummy node
    /// </summary>
    internal class TagTreeNode : TreeNode
    {
        internal TagTreeNode()
        {
            Initialize();
        }

        internal TagTreeNode(String tagName) : base(tagName)
        {
            Initialize();
            this.Name = tagName;
        }

        private void Initialize()
        {
            this.Nodes.Add(DUMMY_NODE, DUMMY_NODE);
            this.SelectedImageIndex = 1;
        }

        private const String DUMMY_NODE = "Dummy";

        /// <summary>
        /// Gets the value indicating, that this node contains only dummy node
        /// and contains no favorite nodes
        /// </summary>
        internal Boolean IsEmpty
        {
            get { return this.Nodes.Count == 1 && this.Nodes[0].Name == DUMMY_NODE; }
        }
    }
}
