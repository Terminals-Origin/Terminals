using System;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data
{
    /// <summary>
    /// Collection of time intervals used to represent connection history time line
    /// cut into smaler logical parts.
    /// </summary>
    internal class HistoryIntervals
    {
        internal const string TODAY = "Today";
        internal const string YESTERDAY = "Yesterday";
        internal const string WEEK = "Less than 1 Week";
        internal const string TWOWEEKS = "Less than 2 Weeks";
        internal const string MONTH = "Less than 1 Month";
        internal const string OVERONEMONTH = "Over 1 Month";
        internal const string HALFYEAR = "Over 6 Months";
        internal const string YEAR = "Over 1 Year";

        private static readonly List<HistoryInterval> intervals = new List<HistoryInterval>
        {
            new HistoryInterval(HistoryInterval.GetToday, HistoryInterval.GetNow, TODAY),
            new HistoryInterval(HistoryInterval.GetYesterday, HistoryInterval.GetToday, YESTERDAY),
            new HistoryInterval(HistoryInterval.GetWeek, HistoryInterval.GetYesterday, WEEK),
            new HistoryInterval(HistoryInterval.GetTwoWeeks, HistoryInterval.GetWeek, TWOWEEKS),
            new HistoryInterval(HistoryInterval.GetMonth, HistoryInterval.GetTwoWeeks, MONTH),
            new HistoryInterval(HistoryInterval.GetOneHalfYear, HistoryInterval.GetMonth, OVERONEMONTH),
            new HistoryInterval(HistoryInterval.GetYear, HistoryInterval.GetOneHalfYear, HALFYEAR),
            new HistoryInterval(HistoryInterval.GetEveryTime, HistoryInterval.GetYear, YEAR)
        };

        internal static HistoryInterval GetIntervalByName(string intervalName)
        {
            return intervals.FirstOrDefault(candidate => candidate.Name == intervalName);
        }

        internal static string GetDateGroupName(DateTime itemDate)
        {
            HistoryInterval interval = DateGroup(itemDate);
            return interval.Name;
        }

        private static HistoryInterval DateGroup(DateTime itemDate)
        {
            return intervals.FirstOrDefault(interval => interval.IsInRange(itemDate));
        }
    }
}
