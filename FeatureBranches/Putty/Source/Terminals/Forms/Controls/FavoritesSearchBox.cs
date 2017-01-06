using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class FavoritesSearchBox : UserControl
    {
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Informs, that user requests new search by changing the text to search or press enter key,
        /// or click on search button. Event is delayed by 250 ms, but fired in GUI thread.
        /// If user doesnt cancel, returns found favorites.
        /// </summary>
        public event EventHandler<FavoritesFoundEventArgs> Found;

        /// <summary>
        /// User requests to cancel currently running search by click on cancel button or by press of Escape key.
        /// </summary>
        public event EventHandler Canceled;

        private bool alreadySearched;

        private IPersistence persistence;

        private readonly Settings settings = Settings.Instance;

        public FavoritesSearchBox()
        {
            InitializeComponent();
        }

        internal void LoadEvents(IPersistence persistence)
        {
            this.persistence = persistence;
            this.searchTextBox.SearchedTexts = settings.SavedSearches;
            this.persistence.Dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(PersistenceFavoritesChanged);
        }

        internal void UnloadEvents()
        {
            settings.SavedSearches = this.searchTextBox.SearchedTexts;
            // if the panel is not visible, the loading never happens, and the persistence
            // doesnt have to be assigned
            if (this.persistence != null)
                this.persistence.Dispatcher.FavoritesChanged -= new FavoritesChangedEventHandler(PersistenceFavoritesChanged);
        }

        /// <summary>
        /// we dont want to change the search user control state,
        /// we only want to kick off new search of favorites.
        /// Using this as an persistence event handler allowes to keep the results list up to date with the cache
        /// </summary>
        private void PersistenceFavoritesChanged(FavoritesChangedEventArgs args)
        {
            if (this.alreadySearched)
                this.RefreshSearch();
            else
                this.FireCanceled();
        }

        private void RefreshSearch()
        {
            string searchText = this.searchTextBox.SearchText;
            if (string.IsNullOrEmpty(searchText))
                this.Cancel();
            else
                this.StartSearch(searchText);
        }

        private void SearchTextBoxStart(object sender, SearchEventArgs e)
        {
            this.alreadySearched = true;
            this.StartSearch(e.SearchText);
        }

        private void StartSearch(string searchText)
        {
            this.Cancel(); // may be previous search
            this.cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = this.cancellationTokenSource.Token;
            var criteria = new FavoritesSearch(this.persistence.Favorites, token, searchText);
            Task<List<IFavorite>> searchTask = criteria.FindAsync();
            searchTask.ContinueWith(this.FinishSearch, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FinishSearch(Task<List<IFavorite>> searchTask)
        {
            this.ReleaseCancelationTokenSource();
            this.AssignDataSource(searchTask);
        }

        private void ReleaseCancelationTokenSource()
        {
            var backup = this.cancellationTokenSource;
            this.cancellationTokenSource = null;
            if (backup != null)
                backup.Dispose();
        }

        private void AssignDataSource(Task<List<IFavorite>> searchTask)
        {
            if (searchTask.IsCanceled)
                this.FireFound(new List<IFavorite>());
            else
                this.FireFound(searchTask.Result);
        }

        private void SearchTextBoxCancel(object sender, EventArgs e)
        {
            this.Cancel();
            this.FireCanceled();
        }

        private void FireCanceled()
        {
            if (this.Canceled != null)
                this.Canceled(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();
        }

        private void FireFound(List<IFavorite> found)
        {
            if (this.Found != null)
                this.Found(this, new FavoritesFoundEventArgs(found));
        }
    }
}
