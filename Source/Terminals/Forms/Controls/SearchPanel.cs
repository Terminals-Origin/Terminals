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
        internal string[] SaveSearches
        {
            get { return this.searchTextBox.SaveSearches; }
            set { this.searchTextBox.SaveSearches = value; }
        }

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

        public List<IFavorite> SelectedFavorites
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

        internal void RegisterUpdateEvent()
        {
            this.searchTextBox.RegisterUpdateEvent();
        }

        internal void UnRegisterUpdateEvent()
        {
            this.searchTextBox.UnRegisterUpdateEvent();
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
    }
}
