using System;
using System.Data;
using System.Linq;
using Terminals.History;
using Terminals.Network;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQl implementation of connections history.
    /// This doesnt cache the history table in entity framework, because of performance.
    /// </summary>
    internal class ConnectionHistory : IConnectionHistory
    {
        public event HistoryRecorded OnHistoryRecorded;

        public SortableList<IFavorite> GetDateItems(string historyDateKey)
        {
            using (var database = Database.CreateInstance())
            {
                HistoryInterval interval = HistoryIntervals.GetIntervalByName(historyDateKey);
                var favoriteIds = database.GetFavoritesHistoryByDate(interval.From, interval.To);
                IQueryable<Favorite> favorites =
                    database.Favorites.Where(favorite => favoriteIds.Contains(favorite.Id));
                return Data.Favorites.OrderByDefaultSorting(favorites);
            }
        }

        public void RecordHistoryItem(IFavorite favorite)
        {
            var historyTarget = favorite as Favorite;
            if (historyTarget == null)
                return;

            string userSid = WindowsUserIdentifiers.GetCurrentUserSid();

            using (var database = Database.CreateInstance())
            {
                database.InsertHistory(historyTarget.Id, DateTime.Now, userSid);
            }

            this.FireOnHistoryRecorded(favorite);
        }

        private void FireOnHistoryRecorded(IFavorite favorite)
        {
            if (this.OnHistoryRecorded != null)
            {
                var args = new HistoryRecordedEventArgs(favorite);
                this.OnHistoryRecorded(args);
            }
        }
    }
}
