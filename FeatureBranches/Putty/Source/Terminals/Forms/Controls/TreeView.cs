using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Treeview control derived from winforms tree view and allowes load or save expanded state.
    /// </summary>
    internal partial class TreeView : System.Windows.Forms.TreeView
    {
        private const string JOIN_SEPARATOR = "%%";
        
        protected TreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets collection of expanded tree node names as one text.
        /// This allowes load or save the expanded state.
        /// </summary>
        internal string ExpandedNodes
        {
            get { return this.GetExpandedFavoriteNodes(); }
            set { this.ExpandTreeView(value); }
        }

        private string GetExpandedFavoriteNodes()
        {
            List<string> expandedNodes = new List<string>();
            foreach (TreeNode treeNode in this.Nodes)
            {
                if (treeNode.IsExpanded)
                    expandedNodes.Add(treeNode.Text);
            }
            return string.Join(JOIN_SEPARATOR, expandedNodes.ToArray());
        }

        private void ExpandTreeView(string savedNodesToExpand)
        {
            var nodesToExpand = new List<string>();
            if (!string.IsNullOrEmpty(savedNodesToExpand))
                nodesToExpand.AddRange(Regex.Split(savedNodesToExpand, JOIN_SEPARATOR));

            if (nodesToExpand.Count > 0)
            {
                foreach (TreeNode treeNode in this.Nodes)
                {
                    if (nodesToExpand.Contains(treeNode.Text))
                        treeNode.Expand();
                }
            }
        }
    }
}
