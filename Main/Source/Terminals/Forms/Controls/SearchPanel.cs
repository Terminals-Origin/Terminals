using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class SearchPanel : UserControl
    {
        internal string[] SaveSearches
        {
            get { return this.searchTextBox.SaveSearches; }
            set { this.searchTextBox.SaveSearches = value; }
        }

        public SearchPanel()
        {
            InitializeComponent();
        }

        private void FavoritesSearchFound(object sender, FavoritesFoundEventArgs args)
        {
            this.resultsListView.Items.Clear();
            ListViewItem[] transformed = args.Favorites.Select(FavoriteToListViewItem).ToArray();
            this.resultsListView.Items.AddRange(transformed);
        }

        private static ListViewItem FavoriteToListViewItem(IFavorite favorite)
        {
            var item = new ListViewItem();
            item.Text = favorite.Name;
            item.ToolTipText = favorite.GetToolTipText();
            item.ImageKey = FavoriteIcons.GetTreeviewImageListKey(favorite);
            return item;
        }
    }
}
