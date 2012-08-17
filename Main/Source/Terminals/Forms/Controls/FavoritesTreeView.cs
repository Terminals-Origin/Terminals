using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

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
        /// Because of designer, dont call in constructor
        /// </summary>
        internal void Load()
        {
            this.loader.LoadGroups();            
        }

        internal void UnregisterEvents()
        {
            this.loader.UnregisterEvents();
        }

        private void OnTreeViewExpand(object sender, TreeViewEventArgs e)
        {
            GroupTreeNode groupNode = e.Node as GroupTreeNode;
            if (groupNode != null)
            {
                loader.LoadFavorites(groupNode);
            }
        }

        internal GroupTreeNode FindSelectedGroupNode()
        {
            if (this.SelectedNode == null || this.SelectedNode.Parent == null)
                return null;

            var groupNode = this.SelectedNode.Parent as GroupTreeNode; // because first level nodes are Groups
            return groupNode;
        }

        internal void RestoreSelectedFavorite(TreeNode groupNode, IFavorite favorite)
        {
            if (groupNode != null || favorite == null)
                return;

            TreeNode favoriteNode = FindFavoriteNodeByName(groupNode, favorite);
            if (favoriteNode == null) // tag node was removed, try find another one
                groupNode = this.FindFirstGroupNodeContainingFavorite(favorite);

            this.SelectedNode = FindFavoriteNodeByName(groupNode, favorite);
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

            return groupNode.Nodes.Cast<FavoriteTreeNode>()
                .FirstOrDefault(favoriteNode => favoriteNode.Favorite.StoreIdEquals(favorite));
        }
    }
}
