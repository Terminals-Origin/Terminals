using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.History
{
    public class HistoryItem
    {
        public HistoryItem(string Name) : this()
        {
            this.Name = Name;
        }
        public HistoryItem()
        {
            Date = DateTime.Now;
        }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
