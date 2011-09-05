using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    internal class FavoriteTreeNode : TreeNode
    {
        internal FavoriteTreeNode(FavoriteConfigurationElement favorite)
            : base(favorite.Name)
        {
            this.Name = favorite.Name;
            this.Favorite = favorite;
            this.Tag = favorite; // temporar solution, for backwarad compatibility only

            this.ImageIndex = FavoriteIcons.GetTreeviewImageListIndex(favorite);
            this.SelectedImageIndex = this.ImageIndex;
            this.ToolTipText = favorite.GetToolTipText();
        }

        /// <summary>
        /// Gets or sets the corresponding connection favorite
        /// </summary>
        internal FavoriteConfigurationElement Favorite { get; private set; }

    }
}
