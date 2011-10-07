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
            if (tagNode != null && !tagNode.IsEmpty)
                tagNode.Nodes.RemoveByKey(favoriteName);
        }

        private static void AddNewFavoriteNodeToTagNode(FavoriteConfigurationElement favorite, TagTreeNode tagNode)
        {
            if (tagNode != null && !tagNode.IsEmpty) // add only to expanded nodes
            {
                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                int index = FindNodeInsertIndex(tagNode.Nodes, favorite.Name);
                InsertNodePreservingOrder(tagNode.Nodes, index, favoriteTreeNode);
            }
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
                // Skips first Untagged node to keep it first.
                int index = FindNodeInsertIndex(this.treeList.Nodes, newTag, 1);
                this.CreateAndAddTagNode(newTag, index);
            }
        }

        /// <summary>
        /// Finds the index for the node to insert in nodes collection
        /// and skips nodes before the startIndex.
        /// </summary>
        /// <param name="nodes">The nodes collection to search in.</param>
        /// <param name="newTag">The new tag to add.</param>
        /// <param name="startIndex">The search start index.</param>
        /// <returns>
        /// -1, if the tag should be added to the end of tag nodes,
        /// otherwise found index.
        /// </returns>
        private static int FindNodeInsertIndex(TreeNodeCollection nodes, string newTag, int startIndex = 0)
        {
            for (int index = startIndex; index < nodes.Count; index++)
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
            if (tagNode.IsEmpty)
            {
                tagNode.Nodes.Clear();
                AddFavoriteNodes(tagNode);
            }
        }

        private static void AddFavoriteNodes(TagTreeNode tagNode)
        {
            List<FavoriteConfigurationElement> tagFavorites = Settings.GetFavoritesByTag(tagNode.Text);
            foreach (var favorite in tagFavorites)
            {
                var favoriteTreeNode = new FavoriteTreeNode(favorite);
                tagNode.Nodes.Add(favoriteTreeNode);
            }
        }
    }
}
