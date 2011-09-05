using System;
using System.Drawing;
using System.Text;
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
