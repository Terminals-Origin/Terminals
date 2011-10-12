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

        /// <summary>
        /// Returns text compareto method values selecting property to compare
        /// depending on Settings default sort property value
        /// </summary>
        /// <param name="target">not null favorite to compare with</param>
        /// <returns>result of CompareTo method</returns>
        internal int CompareByDefaultFavoriteSorting(FavoriteConfigurationElement target)
        {
            return this.Favorite.CompareByDefaultSorting(target);
        }
    }
}
