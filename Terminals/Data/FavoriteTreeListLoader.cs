using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals
{
    /// <summary>
    /// Fills tree list with favorites
    /// </summary>
    internal class FavoriteTreeListLoader
    {
        private TreeView treeList;
        SortedDictionary<String, TreeNode> sortedTags = new SortedDictionary<String, TreeNode>();
        private const String UNTAGGED_NODENAME = "Untagged";

        internal FavoriteTreeListLoader(TreeView treeListToFill)
        {
            this.treeList = treeListToFill;
        }

        internal void Load()
        {
            List<string> nodeTextList = BackUpExpandedNodes();
            sortedTags.Clear();
            treeList.Nodes.Clear();
            CreateTreeNodes();
            treeList.Sort();
            RestoreExpandedNodes(nodeTextList);
        }

        private void CreateTreeNodes()
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();

            if (favorites != null)
            {
                foreach (FavoriteConfigurationElement favorite in favorites)
                {
                    this.AddFavoriteTreeNode(favorite);
                }
            }
        }

        private List<string> BackUpExpandedNodes()
        {
            List<String> nodeTextList = new List<String>();
            foreach (TreeNode node in this.treeList.Nodes)
            {
                if (node.IsExpanded)
                    nodeTextList.Add(node.Text);
            }
            return nodeTextList;
        }

        private void RestoreExpandedNodes(List<String> nodeTextList)
        {
            foreach (TreeNode node in this.treeList.Nodes)
            {
                if (nodeTextList.Contains(node.Text))
                    node.Expand();
            }
        }
 
        private void AddFavoriteTreeNode(FavoriteConfigurationElement favorite)
        {
            if (favorite.TagList.Count > 0)
            {
                foreach (String tag in favorite.TagList)
                {
                    CreateNodeInSortedTagsByTag(tag, favorite);
                }
            }
            else
            {
                CreateNodeInSortedTagsByTag(UNTAGGED_NODENAME, favorite);
            }
        }

        private void CreateNodeInSortedTagsByTag(String tag, FavoriteConfigurationElement favorite)
        {
            EnsureTagNode(sortedTags, tag);
            TreeNode favNode = new TreeNode(favorite.Name);
            favNode.Tag = favorite;

            if (!sortedTags[tag].Nodes.Contains(favNode))
                sortedTags[tag].Nodes.Add(favNode);
        }

        /// <summary>
        /// Create tree node for Tag, if isn't already in sortedTags
        /// </summary>
        private void EnsureTagNode(SortedDictionary<String, TreeNode> sortedTags, String tag)
        {
            if (!sortedTags.ContainsKey(tag))
            {
                TreeNode tagNode = new TreeNode(tag);
                treeList.Nodes.Add(tagNode);
                sortedTags.Add(tag, tagNode);
            }
        }
    }
}
