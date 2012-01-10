using System;
using System.Collections.Generic;
using Terminals.Data;

namespace Terminals.History
{
    /// <summary>
    /// Collection of favorites history touches grouped by favorite unique identifier
    /// </summary>
    public class HistoryByFavorite : SerializableSortedDictionary<Guid , List<HistoryItem>>
    {
        internal const string TODAY = "Today";
        internal const string YESTERDAY = "Yesterday";
        internal const string WEEK = "Less than 1 Week";
        internal const string TWOWEEKS = "Less than 2 Weeks";
        internal const string MONTH = "Less than 1 Month";
        internal const string OVERONEMONTH = "Over 1 Month";
        internal const string HALFYEAR = "Over 6 Months";
        internal const string YEAR = "Over 1 Year";

        internal SerializableDictionary<string, SortableList<IHistoryItem>> GroupByDate()
        {
            SerializableDictionary<string, SortableList<IHistoryItem>> groupedByDate = InitializeGroups();
            IFavorites favorites = Persistance.Instance.Favorites;

            foreach (Guid favoriteId in this.Keys)  //key is the favorite unique identifier
            {
                IFavorite favorite = favorites[favoriteId];
                foreach (IHistoryItem item in this[favoriteId])  //each history item per favorite
                {
                    item.Favorite = favorite; // set navigation property
                    SortableList<IHistoryItem> timeIntervalItems = GetTimeIntervalItems(item.DateGroup, groupedByDate);
                    if (!timeIntervalItems.Contains(item)) // add each item only once
                        timeIntervalItems.Add(item);
                }

            }
            return groupedByDate;
        }

        private SerializableDictionary<string, SortableList<IHistoryItem>> InitializeGroups()
        {
            var groupedByDate = new SerializableDictionary<string, SortableList<IHistoryItem>>();
            groupedByDate.Add(TODAY, new SortableList<IHistoryItem>());
            groupedByDate.Add(YESTERDAY, new SortableList<IHistoryItem>());
            groupedByDate.Add(WEEK, new SortableList<IHistoryItem>());
            groupedByDate.Add(TWOWEEKS, new SortableList<IHistoryItem>());
            groupedByDate.Add(MONTH, new SortableList<IHistoryItem>());
            groupedByDate.Add(OVERONEMONTH, new SortableList<IHistoryItem>());
            groupedByDate.Add(HALFYEAR, new SortableList<IHistoryItem>());
            groupedByDate.Add(YEAR, new SortableList<IHistoryItem>());
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
