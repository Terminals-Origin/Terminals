using System;

namespace Terminals.Data
{
    /// <summary>
    /// Represents (time interval) part of timeline for history items. Used to group history items by time.
    /// </summary>
    internal class HistoryInterval
    {
        private readonly Func<DateTime> getFrom;
        private readonly Func<DateTime> getTo;
        internal string Name { get; private set; }

        internal DateTime From
        {
            get { return getFrom(); }
        }

        internal DateTime To
        {
            get { return getTo(); }
        }

        internal HistoryInterval(Func<DateTime> getFrom, Func<DateTime> getTo, string name)
        {
            this.getFrom = getFrom;
            this.getTo = getTo;
            this.Name = name;
        }

        public override string ToString()
        {
            return String.Format("HistoryTimeInterval:{0}{{{1} - {2}}}", this.Name, this.From, this.To);
        }

        internal bool IsInRange(DateTime dateToCheck)
        {
            return this.From < dateToCheck && dateToCheck <= this.To;
        }

        internal static DateTime GetNow()
        {
            return DateTime.Now;
        }

        internal static DateTime GetToday()
        {
            return DateTime.Today;
        }

        internal static DateTime GetYesterday()
        {
            return DateTime.Today.AddDays(-1);
        }

        internal static DateTime GetWeek()
        {
            return DateTime.Today.AddDays(-7);
        }

        internal static DateTime GetTwoWeeks()
        {
            return DateTime.Today.AddDays(-14);
        }

        internal static DateTime GetMonth()
        {
            return DateTime.Today.AddMonths(-1);
        }

        internal static DateTime GetOneHalfYear()
        {
            return DateTime.Today.AddMonths(-6);
        }

        internal static DateTime GetYear()
        {
            return DateTime.Today.AddYears(-1);
        }

        internal static DateTime GetEveryTime()
        {
            return new DateTime();
        }
    }
}
