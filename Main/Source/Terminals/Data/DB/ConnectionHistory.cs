using System;
using System.Collections.Generic;
using System.Linq;
using Terminals.History;
using Terminals.Network;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQl implementation of connections history.
    /// This doesn't cache the history table in entity framework, because of performance.
    /// </summary>
    internal class ConnectionHistory : IConnectionHistory
    {
        /// <summary>
        /// Access the cached items, instead of retrieving them from database
        /// </summary>
        private readonly Favorites favorites;

        /// <summary>
        /// Cache older items than today only, because the history cant change.
        /// The reason is to don't reload these items from database.
        /// </summary>
        private readonly Dictionary<string, SortableList<IFavorite>> cache =
            new Dictionary<string, SortableList<IFavorite>>();

        public event HistoryRecorded OnHistoryRecorded;

        internal ConnectionHistory(Favorites favorites)
        {
            this.favorites = favorites;
        }

        public SortableList<IFavorite> GetDateItems(string historyDateKey)
        {
            // cache older groups only
            if (historyDateKey == HistoryIntervals.TODAY)
                return this.LodFromDatabaseByDate(historyDateKey);

            return this.LoadFromCache(historyDateKey);
        }

        private SortableList<IFavorite> LoadFromCache(string historyDateKey)
        {
            if (!this.cache.ContainsKey(historyDateKey))
            {
                SortableList<IFavorite> loaded = this.LodFromDatabaseByDate(historyDateKey);
                this.cache.Add(historyDateKey, loaded);
            }

            return this.cache[historyDateKey];
        }

        private SortableList<IFavorite> LodFromDatabaseByDate(string historyDateKey)
        {
            using (var database = Database.CreateInstance())
            {
                HistoryInterval interval = HistoryIntervals.GetIntervalByName(historyDateKey);
                var favoriteIds = database.GetFavoritesHistoryByDate(interval.From, interval.To).ToList();
                IEnumerable<DbFavorite> intervalFavorites =
                    this.favorites.Cast<DbFavorite>().Where(favorite => favoriteIds.Contains(favorite.Id));
                return Data.Favorites.OrderByDefaultSorting(intervalFavorites);
            }
        }

        public void RecordHistoryItem(IFavorite favorite)
        {
            var historyTarget = favorite as DbFavorite;
            if (historyTarget == null)
                return;

            // here we don't cache today's items, we always load the current state from database
            AddToDatabase(historyTarget);
            this.FireOnHistoryRecorded(favorite);
        }

        private static void AddToDatabase(DbFavorite historyTarget)
        {
            using (var database = Database.CreateInstance())
            {
                string userSid = WindowsUserIdentifiers.GetCurrentUserSid();
                database.InsertHistory(historyTarget.Id, DateTime.Now, userSid);
            }
        }

        private void FireOnHistoryRecorded(IFavorite favorite)
        {
            if (this.OnHistoryRecorded != null)
            {
                var args = new HistoryRecordedEventArgs(favorite);
                this.OnHistoryRecorded(args);
            }
        }

        public override string ToString()
        {
            return string.Format("ConnectionHistory:Cached={0}", this.cache.Count());
        }
    }
}
