using System;
using System.Collections.Generic;

namespace Terminals.History
{
    public class HistoryByFavorite : SerializableSortedDictionary<string, List<HistoryItem>>
    {
        private const string TODAY = "Today";
        private const string YESTERDAY = "Yesterday";
        private const string WEEK = "Less than 1 Week";
        private const string TWOWEEKS = "Less than 2 Weeks";
        private const string MONTH = "Less than 1 Month";
        private const string OVERONEMONTH = "Over 1 Month";
        private const string HALFYEAR = "Over 6 Months";
        private const string YEAR = "Over 1 Year";

        internal SerializableDictionary<string, List<HistoryItem>> GroupByDate()
        {
            SerializableDictionary<string, List<HistoryItem>> groupedByDate = InitializeGroups();

            foreach (string name in this.Keys)  //name is the favorite name
            {
                foreach (HistoryItem item in this[name])  //each history item per favorite
                {
                    string timeKey = GetTimeKey(item.Date.Date);

                    List<HistoryItem> timeIntervalItems = GetTimeIntervalItems(timeKey, groupedByDate);
                    if (!timeIntervalItems.Contains(item))
                        timeIntervalItems.Add(item);
                }

            }
            return groupedByDate;
        }

        private string GetTimeKey(DateTime itemDate)
        {
            if (itemDate >= DateTime.Now.Date)
                return TODAY;

            if (itemDate >= DateTime.Now.AddDays(-1).Date)
                return YESTERDAY;
            
            if (itemDate >= DateTime.Now.AddDays(-7).Date)
                return WEEK;
            
            if (itemDate >= DateTime.Now.AddDays(-14).Date)
                return TWOWEEKS;

            if (itemDate >= DateTime.Now.AddMonths(-1).Date)
                return MONTH;

            if (itemDate >= DateTime.Now.AddMonths(-6).Date)
                return OVERONEMONTH;

            if (itemDate >= DateTime.Now.AddYears(-1).Date)
                return YEAR;

            return YEAR;
        }

        private SerializableDictionary<string, List<HistoryItem>> InitializeGroups()
        {
            var groupedByDate = new SerializableDictionary<string, List<HistoryItem>>();
            groupedByDate.Add(TODAY, new List<HistoryItem>());
            groupedByDate.Add(YESTERDAY, new List<HistoryItem>());
            groupedByDate.Add(WEEK, new List<HistoryItem>());
            groupedByDate.Add(TWOWEEKS, new List<HistoryItem>());
            groupedByDate.Add(MONTH, new List<HistoryItem>());
            groupedByDate.Add(OVERONEMONTH, new List<HistoryItem>());
            groupedByDate.Add(HALFYEAR, new List<HistoryItem>());
            groupedByDate.Add(YEAR, new List<HistoryItem>());
            return groupedByDate;
        }

        /// <summary>
        /// this will contain each name in each bin of grouped by time.
        /// Returns not null list of items.
        /// </summary>
        private List<HistoryItem> GetTimeIntervalItems(string timeKey, 
            SerializableDictionary<string, List<HistoryItem>> groupedByDate)
        {
            List<HistoryItem> timeIntervalItems = null; 
            if (groupedByDate.ContainsKey(timeKey))
                timeIntervalItems = groupedByDate[timeKey];
            if (timeIntervalItems == null)
            {
                timeIntervalItems = new List<HistoryItem>();
                groupedByDate.Add(timeKey, timeIntervalItems);
            }
            return timeIntervalItems;
        }
    }
}
