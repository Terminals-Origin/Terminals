using System;
using System.Xml.Serialization;

namespace Terminals.History
{
    public class HistoryItem
    {
        public HistoryItem(string name) : this()
        {
            this.Name = name;
        }

        public HistoryItem()
        {
            Date = DateTime.Now;
            ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [XmlIgnore]
        internal string DateGroup
        {
            get { return GetDateGroup(this.Date.Date); }
        }

        private static string GetDateGroup(DateTime itemDate)
        {
            if (itemDate >= DateTime.Now.Date)
                return HistoryByFavorite.TODAY;

            if (itemDate >= DateTime.Now.AddDays(-1).Date)
                return HistoryByFavorite.YESTERDAY;

            if (itemDate >= DateTime.Now.AddDays(-7).Date)
                return HistoryByFavorite.WEEK;

            if (itemDate >= DateTime.Now.AddDays(-14).Date)
                return HistoryByFavorite.TWOWEEKS;

            if (itemDate >= DateTime.Now.AddMonths(-1).Date)
                return HistoryByFavorite.MONTH;

            if (itemDate >= DateTime.Now.AddMonths(-6).Date)
                return HistoryByFavorite.OVERONEMONTH;

            if (itemDate >= DateTime.Now.AddYears(-1).Date)
                return HistoryByFavorite.YEAR;

            return HistoryByFavorite.YEAR;
        }
    }
}
