using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal class FavoriteTreeNode : TreeNode
    {
        internal FavoriteTreeNode(IFavorite favorite)
        {
            this.UpdateByFavorite(favorite);
        }

        /// <summary>
        /// Gets or sets the corresponding connection favorite
        /// </summary>
        internal IFavorite Favorite { get; private set; }

        /// <summary>
        /// Returns text compare to method values selecting property to compare
        /// depending on Settings default sort property value
        /// </summary>
        /// <param name="target">not null favorite to compare with</param>
        /// <returns>result of CompareTo method</returns>
        internal int CompareByDefaultFavoriteSorting(IFavorite target)
        {
            return this.Favorite.CompareByDefaultSorting(target);
        }

        internal bool HasFavoriteIn(IEnumerable<IFavorite> target)
        {
            return target.Any(required => required.StoreIdEquals(this.Favorite));
        }

        internal void UpdateByFavorite(IFavorite favorite)
        {
            this.Name = favorite.Name;
            this.Text = favorite.Name;
            this.Favorite = favorite;
            this.Tag = favorite; // temporary solution, for backward compatibility only

            this.ImageKey = FavoriteIcons.GetTreeviewImageListKey(favorite.Protocol);
            this.SelectedImageKey = this.ImageKey;
            // possible performance hit on SQL persistence, when loading details for each created favorite and also in menu loader
            this.ToolTipText = favorite.GetToolTipText();
        }
    }
}
