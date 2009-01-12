using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.History
{
    public class History
    {
        public History()
        {
            HistoryByTime = new HistoryByTime();
        }
        private void LoadHistory()
        {

        }
        private void SaveHistory()
        {
        }
        public HistoryByTime HistoryByTime { get; set; }
        public static void RecordItem(string Name)
        {


            HistoryItem item = new HistoryItem();
            item.Name = Name;
        }
    }
}
