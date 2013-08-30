using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class SearchPanel : UserControl
    {
        private CancellationTokenSource cancellationTokenSource;

        public SearchPanel()
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
                return;

            this.ClearResults();
            ListViewItem[] transformed = searchTask.Result.Select(FavoriteToListViewItem).ToArray();
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

        private void SearchTextBoxCancel(object sender, EventArgs e)
        {
            this.Cancel();
            this.ClearResults();
        }

        private void Cancel()
        {
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();
        }

        private void ClearResults()
        {
            this.resultsListView.Items.Clear();
        }
    }
}
