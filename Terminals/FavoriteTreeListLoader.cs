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
        SortedDictionary<string, TreeNode> sortedTags = new SortedDictionary<string, TreeNode>();
        private const String UNTAGGED_NODENAME = "Untagged";

        internal FavoriteTreeListLoader(TreeView treeListToFill)
        {
            this.treeList = treeListToFill;
        }

        internal void Load()
        {
            sortedTags.Clear();
            treeList.Nodes.Clear();
            SortedDictionary<string, FavoriteConfigurationElement> favorites =
                Settings.GetSortedFavorites(Settings.DefaultSortProperty);

            if (favorites != null)
            {
                foreach (KeyValuePair<string, FavoriteConfigurationElement> favoriteKeyPair in favorites)
                {
                    AddFavoriteTreeNode(favoriteKeyPair);
                }
            }
            treeList.Sort();
        }

        private void AddFavoriteTreeNode(KeyValuePair<string, FavoriteConfigurationElement> favoriteKeyPair)
        {
            FavoriteConfigurationElement favorite = favoriteKeyPair.Value;
            if (favorite.TagList.Count > 0)
            {
                foreach (string tag in favorite.TagList)
                {
                    CreateNodeInSortedTagsByTag(tag, favorite);
                }
            }
            else
            {
                CreateNodeInSortedTagsByTag(UNTAGGED_NODENAME, favorite);
            }
        }

        private void CreateNodeInSortedTagsByTag(string tag, FavoriteConfigurationElement favorite)
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
        private void EnsureTagNode(SortedDictionary<string, TreeNode> sortedTags, string tag)
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
