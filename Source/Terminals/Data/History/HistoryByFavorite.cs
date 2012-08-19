using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    /// <summary>
    /// Collection of favorites history touches grouped by favorite unique identifier
    /// </summary>
    public class HistoryByFavorite : SerializableSortedDictionary<Guid , List<HistoryItem>>
    {
        /// <summary>
        /// Internal store to resolve favorites from.
        /// </summary>
        internal Favorites Favorites { get; set; }

        internal SerializableDictionary<string, SortableList<IHistoryItem>> GroupByDate()
        {
            SerializableDictionary<string, SortableList<IHistoryItem>> groupedByDate = InitializeGroups();
            if (this.Favorites == null)
                return groupedByDate;

            this.GroupFavoriteKeysByDate(groupedByDate);
            return groupedByDate;
        }

        private void GroupFavoriteKeysByDate(SerializableDictionary<string, SortableList<IHistoryItem>> groupedByDate)
        {
            foreach (Guid favoriteId in this.Keys) // key is the favorite unique identifier
            {
                IFavorite favorite = this.Favorites[favoriteId];
                foreach (IHistoryItem item in this[favoriteId]) // each history item per favorite
                {
                    item.Favorite = favorite; // assign navigation property value
                    this.AddItemToGroup(groupedByDate, item);
                }
            }
        }

        private void AddItemToGroup(SerializableDictionary<string, SortableList<IHistoryItem>> groupedByDate, IHistoryItem item)
        {
            string intervalName = HistoryIntervals.GetDateIntervalName(item.Date);
            SortableList<IHistoryItem> timeIntervalItems = GetTimeIntervalItems(intervalName, groupedByDate);
            if (!timeIntervalItems.Contains(item)) // add each item only once
                timeIntervalItems.Add(item);
        }

        private SerializableDictionary<string, SortableList<IHistoryItem>> InitializeGroups()
        {
            var groupedByDate = new SerializableDictionary<string, SortableList<IHistoryItem>>();
            groupedByDate.Add(HistoryIntervals.TODAY, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.YESTERDAY, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.WEEK, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.TWOWEEKS, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.MONTH, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.OVERONEMONTH, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.HALFYEAR, new SortableList<IHistoryItem>());
            groupedByDate.Add(HistoryIntervals.YEAR, new SortableList<IHistoryItem>());
            return groupedByDate;
        }

        /// <summary>
        /// this will contain each name in each bin of grouped by time.
        /// Returns not null list of items.
        /// </summary>
        private SortableList<IHistoryItem> GetTimeIntervalItems(string timeKey,
            SerializableDictionary<string, SortableList<IHistoryItem>> groupedByDate)
        {
            if (!groupedByDate.ContainsKey(timeKey))
            {
                var timeIntervalItems = new SortableList<IHistoryItem>();
                groupedByDate.Add(timeKey, timeIntervalItems);
            }

            return groupedByDate[timeKey];
        }
    }
}
