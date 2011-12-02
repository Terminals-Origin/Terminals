using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Treeview in main window to present favorites organized by Tags
    /// </summary>
    internal partial class FavoritesTreeView : TreeView
    {
        private FavoriteTreeListLoader loader;

        public FavoritesTreeView()
        {
            InitializeComponent();

            loader = new FavoriteTreeListLoader(this);
        }

        internal FavoriteConfigurationElement SelectedFavorite
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
        /// Because of designer, dont call in constructor
        /// </summary>
        internal void Load()
        {
            this.loader.LoadTags();            
        }

        internal void UnregisterEvents()
        {
            this.loader.UnregisterEvents();
        }

        private void OnTreeViewExpand(object sender, TreeViewEventArgs e)
        {
            TagTreeNode tagNode = e.Node as TagTreeNode;
            if (tagNode != null)
            {
                loader.LoadFavorites(tagNode);
            }
        }

        internal string FindSelectedTagNodeName()
        {
            if (this.SelectedNode == null || this.SelectedNode.Parent == null)
                return string.Empty;

            var tagNode = this.SelectedNode.Parent;
            return tagNode.Text;
        }

        internal string GetSelectedFavoriteNodeName()
        {
            FavoriteConfigurationElement selectedFavorite = this.SelectedFavorite;
            if (selectedFavorite != null)
                return selectedFavorite.Name;

            return string.Empty;
        }
        
        internal void RestoreSelectedFavorite(string tagNodeName, string favoriteName)
        {
            if (string.IsNullOrEmpty(tagNodeName) || string.IsNullOrEmpty(favoriteName))
                return;

            TreeNode tagNode = GetTagNodeByName(tagNodeName);
            TreeNode favoriteNode = FindFavoriteNodeByName(tagNode, favoriteName);
            if (tagNode == null || favoriteNode == null) // tag node was removed, try find another one
                tagNode = FindFirstTagNodeContainingFavorite(favoriteName);

            this.SelectedNode = FindFavoriteNodeByName(tagNode, favoriteName);
        }

        private TreeNode GetTagNodeByName(string tagName)
        {
            foreach (TreeNode tagNode in this.Nodes)
            {
                if (tagNode.Name == tagName)
                {
                    return tagNode;
                }
            }

            return null;
        }

        private TreeNode FindFirstTagNodeContainingFavorite(string favoriteName)
        {
            foreach (TreeNode tagNode in this.Nodes)
            {
                var favoriteNode = FindFavoriteNodeByName(tagNode, favoriteName);
                if (favoriteNode != null)
                    return tagNode;
            }

            return null;
        }

        private static FavoriteTreeNode FindFavoriteNodeByName(TreeNode tagNode, string favoriteName)
        {
            if (tagNode == null)
                return null;

            foreach (FavoriteTreeNode favoriteNode in tagNode.Nodes)
            {
                if (favoriteNode.Favorite.Name.Equals(favoriteName))
                {
                    return favoriteNode;
                }
            }

            return null;
        }
    }
}
