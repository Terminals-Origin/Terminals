using System.Collections.Generic;

namespace Terminals.History
{
    public class HistoryByFavorite : SerializableSortedDictionary<string, List<HistoryItem>>
    {
        internal const string TODAY = "Today";
        internal const string YESTERDAY = "Yesterday";
        internal const string WEEK = "Less than 1 Week";
        internal const string TWOWEEKS = "Less than 2 Weeks";
        internal const string MONTH = "Less than 1 Month";
        internal const string OVERONEMONTH = "Over 1 Month";
        internal const string HALFYEAR = "Over 6 Months";
        internal const string YEAR = "Over 1 Year";

        internal SerializableDictionary<string, SortableList<HistoryItem>> GroupByDate()
        {
            SerializableDictionary<string, SortableList<HistoryItem>> groupedByDate = InitializeGroups();

            foreach (string name in this.Keys)  //name is the favorite name
            {
                foreach (HistoryItem item in this[name])  //each history item per favorite
                {
                    SortableList<HistoryItem> timeIntervalItems = GetTimeIntervalItems(item.DateGroup, groupedByDate);
                    if (!timeIntervalItems.Contains(item))
                        timeIntervalItems.Add(item);
                }

            }
            return groupedByDate;
        }

        private SerializableDictionary<string, SortableList<HistoryItem>> InitializeGroups()
        {
            var groupedByDate = new SerializableDictionary<string, SortableList<HistoryItem>>();
            groupedByDate.Add(TODAY, new SortableList<HistoryItem>());
            groupedByDate.Add(YESTERDAY, new SortableList<HistoryItem>());
            groupedByDate.Add(WEEK, new SortableList<HistoryItem>());
            groupedByDate.Add(TWOWEEKS, new SortableList<HistoryItem>());
            groupedByDate.Add(MONTH, new SortableList<HistoryItem>());
            groupedByDate.Add(OVERONEMONTH, new SortableList<HistoryItem>());
            groupedByDate.Add(HALFYEAR, new SortableList<HistoryItem>());
            groupedByDate.Add(YEAR, new SortableList<HistoryItem>());
            return groupedByDate;
        }

        /// <summary>
        /// this will contain each name in each bin of grouped by time.
        /// Returns not null list of items.
        /// </summary>
        private SortableList<HistoryItem> GetTimeIntervalItems(string timeKey,
            SerializableDictionary<string, SortableList<HistoryItem>> groupedByDate)
        {
            if (!groupedByDate.ContainsKey(timeKey))
            {
                var timeIntervalItems = new SortableList<HistoryItem>();
                groupedByDate.Add(timeKey, timeIntervalItems);
            }

            return groupedByDate[timeKey];
        }
    }
}
