using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Integrated favorites search box and results panel
    /// </summary>
    internal partial class SearchPanel : UserControl
    {
        internal List<IFavorite> SelectedFavorites
        {
            get { return this.searchResultsPanel.SelectedFavorites; }
        }

        /// <summary>
        /// Ocures, when selection state of an item in results list changes.
        /// This only forwards the event to the wrapped control.
        /// </summary>
        public event ListViewItemSelectionChangedEventHandler ResultsSelectionChanged
        {
            add { this.searchResultsPanel.ResultsSelectionChanged += value; }
            remove { this.searchResultsPanel.ResultsSelectionChanged -= value; }
        }

        internal SearchPanel()
        {
            InitializeComponent();
        }

        internal void LoadEvents(IPersistence persistence, FavoriteIcons favoriteIcons)
        {
            this.searchBox.LoadEvents(persistence);
            this.searchResultsPanel.LoadEvents(persistence, favoriteIcons);
        }

        internal void UnLoadEvents()
        {
            this.searchBox.UnloadEvents();
        }

        private void FavoritesSearchFound(object sender, FavoritesFoundEventArgs args)
        {
            this.searchResultsPanel.LoadFromFavorites(args.Favorites);
        }

        private void SearchBox_Canceled(object sender, EventArgs e)
        {
            // this will be reported also in a case the search wasnt peformed yet
            // and we should show all favorites
            this.searchResultsPanel.LoadAll();
        }
    }
}
