﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class FavoritesSearchBox : UserControl
    {
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Informs, that user requests new search by changing the text to search or press enter key,
        /// or click on search button. Event is delayed by 250 ms, but fired in GUI thread.
        /// If user cancels the search empty list of favorites is found; otherwise found favorites.
        /// </summary>
        public event EventHandler<FavoritesFoundEventArgs> Found;

        internal string[] SaveSearches
        {
            get { return this.searchTextBox.SearchedTexts; }
            set { this.searchTextBox.SearchedTexts = value; }
        }

        public FavoritesSearchBox()
        {
            InitializeComponent();
        }

        private void SearchTextBoxStart(object sender, SearchEventArgs e)
        {
            this.Cancel(); // may be previous search
            this.cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = this.cancellationTokenSource.Token;
            var criteria = new FavoritesSearch(token, e.SearchText);
            Task<List<IFavorite>> searchTask = criteria.FindAsync();
            searchTask.ContinueWith(FinishSearch, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FinishSearch(Task<List<IFavorite>> searchTask)
        {
            this.cancellationTokenSource = null;
            this.AssignDataSource(searchTask);
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
            this.FireFound(new List<IFavorite>());
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
