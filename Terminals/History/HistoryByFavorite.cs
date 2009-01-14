using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.History
{
    public class HistoryByFavorite : SerializableSortedDictionary<string, List<HistoryItem>>
    {
        SerializableSortedDictionary<string, List<string>> groupedByDate;
        public SerializableSortedDictionary<string, List<string>> GroupedByDate
        {
            get
            {
                if (groupedByDate != null) return groupedByDate;
                groupedByDate = new SerializableSortedDictionary<string, List<string>>();

                foreach (string name in this.Keys)  //name is the favorite name
                {
                    List<string> lst = null; //this will contain each name in each bin of grouped by time
                    string timeKey = "Really Old";

                    foreach (HistoryItem item in this[name])  //each history item per favorite
                    {

                        if (item.Date <= DateTime.Now.AddDays(-365))
                        {
                            timeKey = "Over 1 Year";
                        }
                        if (item.Date <= DateTime.Now.AddMonths(-6))
                        {
                            timeKey = "Over 6 Months";
                        }
                        if (item.Date <= DateTime.Now.AddMonths(-1))
                        {
                            timeKey = "Over 1 Month";
                        }
                        if (item.Date >= DateTime.Now.AddMonths(-1))
                        {
                            timeKey = "Less than 1 Month";
                        }
                        if (item.Date >= DateTime.Now.AddDays(-14))
                        {
                            timeKey = "Less than 2 Weeks";
                        }
                        if (item.Date >= DateTime.Now.AddDays(-7))
                        {
                            timeKey = "Less than 1 Week";
                        }
                        if (item.Date >= DateTime.Now.AddDays(-1))
                        {
                            timeKey = "Today and Yesterday";
                        }
                        if (item.Date.Date >= DateTime.Now.Date)
                        {
                            timeKey = "Today";
                        }
                        if (groupedByDate.ContainsKey(timeKey)) lst = groupedByDate[timeKey];
                        if (lst == null)
                        {
                            lst = new List<string>();
                            groupedByDate.Add(timeKey, lst);
                        }

                        if (!lst.Contains(name)) lst.Add(name);
                    }

                }

                return groupedByDate;
            }
        }
    }
}
