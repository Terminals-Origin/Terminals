using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Treview in main window to present favorites organized by Tags
    /// </summary>
    internal partial class FavoritesTreeView : TreeView
    {
        public FavoritesTreeView()
        {
            InitializeComponent();

            loader = new FavoriteTreeListLoader(this);
        }

        private FavoriteTreeListLoader loader;

        internal FavoriteConfigurationElement SelectedFavorite
        {
            get
            {
                if (this.SelectedNode != null && this.SelectedNode is FavoriteTreeNode)
                    return (this.SelectedNode as FavoriteTreeNode).Favorite;

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
    }
}
