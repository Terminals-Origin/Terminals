using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Fills tree list with favorites
    /// </summary>
    internal class FavoriteTreeListLoader
    {
        private TreeView treeList;

        /// <summary>
        /// gets or sets virtual tree node for favorites, which have no tag defined
        /// </summary>
        private TagTreeNode unTaggedNode;

        internal FavoriteTreeListLoader(TreeView treeListToFill)
        {
            this.treeList = treeListToFill;
            DataDispatcher.Instance.TagsChanged += new TagsChangedEventHandler(this.OnTagsCollectionChanged);
            DataDispatcher.Instance.FavoritesChanged += new FavoritesChangedEventHandler(this.OnFavoritesCollectionChanged);
        }

        /// <summary>
        /// Unregisters the Data dispatcher eventing.
        /// Call this to release the treeview, otherwise it will result in memory gap.
        /// </summary>
        internal void UnregisterEvents()
        {
            DataDispatcher.Instance.TagsChanged -= new TagsChangedEventHandler(this.OnTagsCollectionChanged);
            DataDispatcher.Instance.FavoritesChanged -= new FavoritesChangedEventHandler(this.OnFavoritesCollectionChanged);
        }

        private void OnFavoritesCollectionChanged(FavoritesChangedEventArgs args)
        {
            if(IsOrphan())
              return;

            RemoveFavorites(args.Removed);
            UpdateFavorites(args.Updated);
            AddNewFavorites(args.Added);
        }

        /// <summary>
        /// This prevents performance problems, when someone forgets to unregister.
        /// Returns true, if the associated treeview is already dead; otherwise false.
        /// </summary>
        private Boolean IsOrphan()
        {
            if (this.treeList.IsDisposed)
            {
                this.UnregisterEvents();
                return true;
            }

            return false;
        }

        private void RemoveFavorites(List<FavoriteConfigurationElement> removedFavorites)
        {
            foreach (FavoriteConfigurationElement favorite in removedFavorites)
            {
                foreach (String tag in favorite.TagList)
                {
                    TagTreeNode tagNode = this.treeList.Nodes[tag] as TagTreeNode;
                    RemoveFavoriteFromTagNode(tagNode, favorite.Name);
                }

                RemoveFavoriteFromTagNode(this.unTaggedNode, favorite.Name);  
            }
        }

        private void UpdateFavorites(Dictionary<String, FavoriteConfigurationElement> updatedFavorites)
        {
            foreach (var updateArg in updatedFavorites)
            {
                foreach (TagTreeNode tagNode in this.treeList.Nodes)
                {
                    RemoveFavoriteFromTagNode(tagNode, updateArg.Key);  
                }

                this.AddFavoriteToAllItsTagNodes(updateArg.Value);
            }
        }

        private void AddNewFavorites(List<FavoriteConfigurationElement> addedFavorites)
        {
            foreach (FavoriteConfigurationElement favorite in addedFavorites)
            {
                this.AddFavoriteToAllItsTagNodes(favorite);
            }
        }

        private void AddFavoriteToAllItsTagNodes(FavoriteConfigurationElement favorite)
        {
            foreach (String tag in favorite.TagList)
            {
                TagTreeNode tagNode = this.treeList.Nodes[tag] as TagTreeNode;
                AddNewFavoriteNodeToTagNode(favorite, tagNode);
            }

            if (String.IsNullOrEmpty(favorite.Tags))
            {
                AddNewFavoriteNodeToTagNode(favorite, this.unTaggedNode);
            }
        }

        private static void RemoveFavoriteFromTagNode(TagTreeNode tagNode, String favoriteName)
        {
            if (tagNode != null && !tagNode.NotLoadedYet)
                tagNode.Nodes.RemoveByKey(favoriteName);
        }

        private static void AddNewFavoriteNodeToTagNode(FavoriteConfigurationElement favorite, TagTreeNode tagNode)
        {
            if (tagNode != null && !tagNode.NotLoadedYet) // add only to expanded nodes
            {
                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                int index = FindFavoriteNodeInsertIndex(tagNode.Nodes, favorite);
                InsertNodePreservingOrder(tagNode.Nodes, index, favoriteTreeNode);
            }
        }

        /// <summary>
        /// Identify favorite index position in nodes collection by default sorting order.
        /// </summary>
        /// <param name="nodes">Not null nodes collection of FavoriteTreeNodes to search in.</param>
        /// <param name="favorite">Not null favorite to identify in nodes collection.</param>
        /// <returns>
        /// -1, if the tag should be added to the end of tag nodes, otherwise found index.
        /// </returns>
        internal static int FindFavoriteNodeInsertIndex(TreeNodeCollection nodes, FavoriteConfigurationElement favorite)
        {
            for (int index = 0; index < nodes.Count; index++)
            {
                var comparedNode = nodes[index] as FavoriteTreeNode;
                if (comparedNode.CompareByDefaultFavoriteSorting(favorite) > 0)
                    return index;
            }

            return -1;  
        }

        private void OnTagsCollectionChanged(TagsChangedArgs args)
        {
            if (IsOrphan())
                return;

            RemoveUnusedTagNodes(args.Removed);
            AddMissingTagNodes(args.Added);
        }

        private void AddMissingTagNodes(List<String> newTags)
        {
            foreach (String newTag in newTags)
            {
                int index = FindTagNodeInsertIndex(this.treeList.Nodes, newTag);
                this.CreateAndAddTagNode(newTag, index);
            }
        }

        /// <summary>
        /// Finds the index for the node to insert in nodes collection
        /// and skips nodes before the startIndex.
        /// </summary>
        /// <param name="nodes">Not null nodes collection to search in.</param>
        /// <param name="newTag">Not empty new tag to add.</param>
        /// <returns>
        /// -1, if the tag should be added to the end of tag nodes, otherwise found index.
        /// </returns>
        private static int FindTagNodeInsertIndex(TreeNodeCollection nodes, string newTag)
        {
            // Skips first "Untagged" node to keep it first.
            for (int index = 1; index < nodes.Count; index++)
            {
                if (nodes[index].Text.CompareTo(newTag) > 0)
                    return index;
            }

            return -1;
        }

        private void RemoveUnusedTagNodes(List<String> removedTags)
        {
            foreach (String obsoletTag in removedTags)
            {
                this.treeList.Nodes.RemoveByKey(obsoletTag);
            }
        }

        /// <summary>
        /// Creates the and add tag node in tree list on proper position defined by index.
        /// This allowes the tag nodes to keep ordered by name.
        /// </summary>
        /// <param name="tag">The tag name to create.</param>
        /// <param name="index">The index on which node would be inserted.
        /// If negative number, than it is added to the end.</param>
        /// <returns>Not null, newly creted node</returns>
        private TagTreeNode CreateAndAddTagNode(String tag, int index = -1)
        {
            TagTreeNode tagNode = new TagTreeNode(tag);
            InsertNodePreservingOrder(this.treeList.Nodes, index, tagNode);
            return tagNode;
        }

        private static void InsertNodePreservingOrder(TreeNodeCollection nodes, int index, TreeNode tagNode)
        {
            if (index < 0)
                nodes.Add(tagNode);
            else
                nodes.Insert(index, tagNode);
        }

        internal void LoadTags()
        {
            this.unTaggedNode = this.CreateAndAddTagNode(Settings.UNTAGGED_NODENAME);

            if(Settings.Tags == null) // because of designer
                return;
            
            foreach (string tagName in Settings.Tags)
            {
                this.CreateAndAddTagNode(tagName);
            }
        }

        internal void LoadFavorites(TagTreeNode tagNode)
        {
            if (tagNode.NotLoadedYet)
            {
                tagNode.Nodes.Clear();
                AddFavoriteNodes(tagNode);
            }
        }

        private static void AddFavoriteNodes(TagTreeNode tagNode)
        {
            List<FavoriteConfigurationElement> tagFavorites = Settings.GetSortedFavoritesByTag(tagNode.Text);

            foreach (var favorite in tagFavorites)
            {
                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                tagNode.Nodes.Add(favoriteTreeNode);
            }
        }
    }
}
