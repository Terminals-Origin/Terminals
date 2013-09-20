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

        public FavoritesTreeView()
        {
            InitializeComponent();
        }

        internal GroupTreeNode FindSelectedGroupNode()
        {
            if (this.SelectedNode == null)
                return null;

            var groupNode = this.SelectedNode as GroupTreeNode; // because first level nodes are Groups
            if (groupNode != null)
                return groupNode;

            return this.SelectedNode.Parent as GroupTreeNode;
        }

        internal void RestoreSelectedFavorite(TreeNode groupNode, IFavorite favorite)
        {
            if (favorite == null)
                return;

            TreeNode nodeToRestore = this.FindNoteToRestore(groupNode, favorite);
            if (nodeToRestore != null)
                this.SelectedNode = nodeToRestore;
        }

        private TreeNode FindNoteToRestore(TreeNode groupNode, IFavorite favorite)
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

            return groupNode.Nodes.OfType<FavoriteTreeNode>()
                .FirstOrDefault(favoriteNode => favoriteNode.Favorite.StoreIdEquals(favorite));
        }
    }
}
