using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Integration;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Treeview in main window to present favorites organized by Tags
    /// </summary>
    internal partial class FavoritesTreeView : TreeView
    {
        internal IFavorite SelectedFavorite
        {
            get
            {
                var selectedFavoriteNode = this.SelectedNode as FavoriteTreeNode;
                if (selectedFavoriteNode != null)
                    return selectedFavoriteNode.Favorite;

                return null;
            }
        }

        /// <summary>
        /// Gets currently selected tree node in case it is a group node, otherwise null.
        /// </summary>
        internal GroupTreeNode SelectedGroupNode
        {
            get
            {
                return this.SelectedNode as GroupTreeNode;
            }
        }

        /// <summary>
        /// Gets never null collection of favorites in selected group.
        /// If no group node is selected, returns empty collection.
        /// </summary>
        internal List<IFavorite> SelectedGroupFavorites
        {
            get
            {
                var groupNode = this.SelectedGroupNode;
                if (groupNode == null)
                    return new List<IFavorite>();

                return groupNode.Favorites;
            }
        }

        public FavoritesTreeView()
        {
            InitializeComponent();
        }

        internal GroupTreeNode FindSelectedGroupNode()
        {
            if (this.SelectedNode == null)
                return null;

            // only leaf nodes arent group nodes
            var groupNode = this.SelectedNode as GroupTreeNode;
            if (groupNode != null)
                return groupNode;

            return this.SelectedNode.Parent as GroupTreeNode;
        }

        internal void RestoreSelectedFavorite(TreeNode groupNode, IFavorite favorite)
        {
            if (favorite == null)
                return;

            TreeNode nodeToRestore = this.FindNodeToRestore(groupNode, favorite);
            if (nodeToRestore != null)
                this.SelectedNode = nodeToRestore;
        }

        private TreeNode FindNodeToRestore(TreeNode groupNode, IFavorite favorite)
        {
            TreeNode favoriteNode = FindFavoriteNodeByName(groupNode, favorite);
            if (favoriteNode == null) // group node was removed, try find another one
                groupNode = this.FindFirstGroupNodeContainingFavorite(favorite);

            return FindFavoriteNodeByName(groupNode, favorite);
        }

        private TreeNode FindFirstGroupNodeContainingFavorite(IFavorite favorite)
        {
            foreach (TreeNode groupNode in this.Nodes)
            {
                var favoriteNode = FindFavoriteNodeByName(groupNode, favorite);
                if (favoriteNode != null)
                    return groupNode;
            }

            return null;
        }

        private static FavoriteTreeNode FindFavoriteNodeByName(TreeNode groupNode, IFavorite favorite)
        {
            if (groupNode == null)
                return null;

            var nodes = new TreeListNodes(groupNode.Nodes);
            return nodes.FavoriteNodes.FirstOrDefault(favoriteNode => favoriteNode.Favorite.StoreIdEquals(favorite));
        }

        private void FavsTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }

        private void FavsTree_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (files != null)
            {
                List<FavoriteConfigurationElement> favoritesToImport = Integrations.Importers.ImportFavorites(files);
                var managedImport = new ImportWithDialogs(this.FindForm());
                managedImport.Import(favoritesToImport);
            }
        }
    }
}
