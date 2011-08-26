using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.History
{
    public class HistoryByFavorite : SerializableSortedDictionary<string, List<HistoryItem>>
    {
        SerializableDictionary<string, List<History.HistoryItem>> groupedByDate;
        public SerializableDictionary<string, List<HistoryItem>> GroupedByDate
        {
            get
            {
                if (groupedByDate != null) return groupedByDate;
                groupedByDate = new SerializableDictionary<string, List<History.HistoryItem>>();

                //add these, in order for rendering later
                groupedByDate.Add("Today", new List<History.HistoryItem>());
                groupedByDate.Add("Yesterday", new List<History.HistoryItem>());
                groupedByDate.Add("Less than 1 Week", new List<History.HistoryItem>());
                groupedByDate.Add("Less than 2 Weeks", new List<History.HistoryItem>());
                groupedByDate.Add("Less than 1 Month", new List<History.HistoryItem>());
                groupedByDate.Add("Over 1 Month", new List<History.HistoryItem>());
                groupedByDate.Add("Over 6 Months", new List<History.HistoryItem>());
                groupedByDate.Add("Over 1 Year", new List<History.HistoryItem>());
                
                
                foreach (string name in this.Keys)  //name is the favorite name
                {
                    string timeKey = "Really Old";

                    foreach (HistoryItem item in this[name])  //each history item per favorite
                    {
                        List<HistoryItem> lst = null; //this will contain each name in each bin of grouped by time
                        if (item.Date.Date <= DateTime.Now.AddMonths(-1).Date)
                        {
                            timeKey = "Over 1 Month";
                        }
                        if (item.Date.Date <= DateTime.Now.AddMonths(-6).Date)
                        {
                            timeKey = "Over 6 Months";
                        }

                        if (item.Date.Date <= DateTime.Now.AddDays(-365).Date)
                        {
                            timeKey = "Over 1 Year";
                        }
                        
                        if (item.Date.Date >= DateTime.Now.AddMonths(-1).Date)
                        {
                            timeKey = "Less than 1 Month";
                        }
                        if (item.Date.Date >= DateTime.Now.AddDays(-14).Date)
                        {
                            timeKey = "Less than 2 Weeks";
                        }
                        if (item.Date.Date >= DateTime.Now.AddDays(-7).Date)
                        {
                            timeKey = "Less than 1 Week";
                        }
                        if (item.Date.Date == DateTime.Now.AddDays(-1).Date)
                        {
                            timeKey = "Yesterday";
                        }
                        if (item.Date.Date >= DateTime.Now.Date)
                        {
                            timeKey = "Today";
                        }
                        if (groupedByDate.ContainsKey(timeKey)) lst = groupedByDate[timeKey];
                        if (lst == null)
                        {
                            lst = new List<History.HistoryItem>();
                            groupedByDate.Add(timeKey, lst);
                        }
                        if (!lst.Contains(item)) lst.Add(item);                        
                    }

                }

                return groupedByDate;
            }
        }
    }
}
