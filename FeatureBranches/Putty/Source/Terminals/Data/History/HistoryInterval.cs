using System;

namespace Terminals.Data
{
    /// <summary>
    /// Represents (time interval) part of timeline for history items. Used to group history items by time.
    /// All members operate with in UTC.
    /// </summary>
    internal class HistoryInterval
    {
        private readonly Func<DateTime> getFrom;
        private readonly Func<DateTime> getTo;
        internal string Name { get; private set; }

        /// <summary>
        /// Gets starting time in universal time
        /// </summary>
        internal DateTime From
        {
            get { return getFrom(); }
        }

        /// <summary>
        /// Gets end time in universal time
        /// </summary>
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

        /// <summary>
        /// Checks whether the given UTC date is inside this interval range.
        /// </summary>
        /// <param name="dateToCheck">UTC DateTime of the interval to find</param>
        internal bool IsInRange(DateTime dateToCheck)
        {
            return this.From < dateToCheck && dateToCheck <= this.To;
        }

        internal static DateTime GetNow()
        {
            return DateTime.UtcNow;
        }

        internal static DateTime GetToday()
        {
            return DateTime.Today
                .ToUniversalTime();
        }

        internal static DateTime GetYesterday()
        {
            return DateTime.Today.AddDays(-1)
                .ToUniversalTime();
        }

        internal static DateTime GetWeek()
        {
            return DateTime.Today.AddDays(-7)
                .ToUniversalTime();
        }

        internal static DateTime GetTwoWeeks()
        {
            return DateTime.Today.AddDays(-14)
                .ToUniversalTime();
        }

        internal static DateTime GetMonth()
        {
            return DateTime.Today.AddMonths(-1)
                .ToUniversalTime();
        }

        internal static DateTime GetOneHalfYear()
        {
            return DateTime.Today.AddMonths(-6)
                .ToUniversalTime();
        }

        internal static DateTime GetYear()
        {
            return DateTime.Today.AddYears(-1)
                .ToUniversalTime();
        }

        internal static DateTime GetEveryTime()
        {
            // prevent system specific (MS SQL) date range,
            // limit older item to this century (longer before first release)
            return new DateTime(2000,1,1);
        }
    }
}
