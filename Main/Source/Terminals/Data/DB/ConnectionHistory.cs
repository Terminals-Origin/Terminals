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
        private Database dataBase;
        public event HistoryRecorded OnHistoryRecorded;

        internal ConnectionHistory(Database dataBase)
        {
            this.dataBase = dataBase;
        }

        public SortableList<IFavorite> GetDateItems(string historyDateKey)
        {
            HistoryInterval interval = HistoryIntervals.GetIntervalByName(historyDateKey);
            var favoriteIds = this.dataBase.GetFavoritesHistoryByDate(interval.From, interval.To);
            IQueryable<Favorite> favorites = this.dataBase.Favorites.Where(favorite => favoriteIds.Contains(favorite.Id));
            return Data.Favorites.OrderByDefaultSorting(favorites);
        }

        public void RecordHistoryItem(IFavorite favorite)
        {
            var historyTarget = favorite as Favorite;
            if (historyTarget == null || historyTarget.EntityState == EntityState.Detached)
                return;

            string userSid = WindowsUserIdentifiers.GetCurrentUserSid();
            int favoriteId = ((Favorite)favorite).Id;
            this.dataBase.InsertHistory(favoriteId, DateTime.Now, userSid);
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
