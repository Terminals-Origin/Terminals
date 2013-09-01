using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class SearchPanel : UserControl
    {
        /// <summary>
        /// Gets or sets the context menu, which will be shown, when selected item is clicked
        /// </summary>
        public ContextMenuStrip ResultsContextMenu { private get; set; }

        /// <summary>
        /// Gets first selected favorite in results list view. Null if nothing is selected.
        /// </summary>
        internal IFavorite SelectedFavorite
        {
            get { return this.SelectedFavorites.FirstOrDefault(); }
        }

        internal List<IFavorite> SelectedFavorites
        {
            get
            {
                return this.resultsListView.SelectedItems.Cast<ListViewItem>()
                    .Select(item => item.Tag as IFavorite)
                    .ToList();
            }
        }

        /// <summary>
        /// Ocures, when result list id double clicked. This only forwards the event to the wrapped control.
        /// </summary>
        public event EventHandler ResultListDoubleClick
        {
            add { this.resultsListView.DoubleClick += value; }
            remove { this.resultsListView.DoubleClick -= value; }
        }

        /// <summary>
        /// Ocures, when a key is released and while the results list has a Focus.
        /// This only forwards the event to the wrapped control.
        /// </summary>
        public event KeyEventHandler ResultListKeyUp
        {
            add { this.resultsListView.KeyUp += value; }
            remove { this.resultsListView.KeyUp -= value; }
        }

        /// <summary>
        /// Ocures, when selection state of an item in results list changes.
        /// This only forwards the event to the wrapped control.
        /// </summary>
        public event ListViewItemSelectionChangedEventHandler ResultsSelectionChanged
        {
            add { this.resultsListView.ItemSelectionChanged += value; }
            remove { this.resultsListView.ItemSelectionChanged -= value; }
        }

        public SearchPanel()
        {
            InitializeComponent();
        }

        internal void LoadEvents()
        {
            this.searchTextBox.LoadEvents();
            this.LoadAll();
        }

        internal void UnLoadEvents()
        {
            this.searchTextBox.UnloadEvents();
        }

        private void FavoritesSearchFound(object sender, FavoritesFoundEventArgs args)
        {
            this.resultsListView.Items.Clear();
            this.LoadFromFavorites(args.Favorites);
        }

        private void LoadFromFavorites(List<IFavorite> favorites)
        {
            ListViewItem[] transformed = favorites.Select(FavoriteToListViewItem).ToArray();
            this.resultsListView.Items.AddRange(transformed);
        }

        private static ListViewItem FavoriteToListViewItem(IFavorite favorite)
        {
            var item = new ListViewItem();
            item.Tag = favorite;
            item.Text = favorite.Name;
            item.ToolTipText = favorite.GetToolTipText();
            item.ImageKey = FavoriteIcons.GetTreeviewImageListKey(favorite);
            return item;
        }

        private void ResultsListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            this.resultsListView.SelectedItems.Clear();
            this.ShowContextMenu(e);
        }

        private void ShowContextMenu(MouseEventArgs e)
        {
            ListViewItem clickedItem = this.resultsListView.GetItemAt(e.X, e.Y);

            if (this.ResultsContextMenu == null || clickedItem == null)
                return;

            clickedItem.Selected = true;
            this.ResultsContextMenu.Show(this.resultsListView, new Point(e.X, e.Y));
        }

        private void SearchTextBox_Canceled(object sender, EventArgs e)
        {
            this.LoadAll();
        }

        /// <summary>
        /// Simulation of all favorites loaded by default
        /// </summary>
        private void LoadAll()
        {
            SortableList<IFavorite> favorites = Persistence.Instance.Favorites.ToListOrderedByDefaultSorting();
            this.LoadFromFavorites(favorites);
        }
    }
}
