using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.History
{
    public class HistoryController
    {
        /// <summary>
        /// lets make it a static string, just in case later we want to override its location
        /// </summary>
        public static string HistoryLocation = "History.xml";
        public HistoryController()
        {
            CurrentHistory = new HistoryByFavorite();
        }

        public delegate void HistoryLoaded(HistoryByFavorite History);
        public event HistoryLoaded OnHistoryLoaded;

        /// <summary>
        /// Capture the OnHistoryLoaded Event
        /// </summary>
        public void LazyLoadHistory()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(LoadHistory), null);
        }
        private object threadLock = new object();
        private bool loadingHistory = false;
        /// <summary>
        /// Load or re-load history from HistoryLocation
        /// </summary>
        private void LoadHistory(object ThreadState)
        {
            if (loadingHistory) return;
            lock (threadLock)
            {
                loadingHistory = true;
                //so, how to store history..that is the question.
                //should it be per computer or per terminals install (portable)?
                //maybe both?
                //i think for now lets stick with the portable option.  history should follow the user around
                //KeepItSimpleStupid

                if (HistoryLocation != null && HistoryLocation.Trim() != "")
                {
                    HistoryLocation = HistoryLocation.Trim();
                    if (!System.IO.File.Exists(HistoryLocation))
                    {
                        //the file doesnt exist.  lets save it out for the first time
                        SaveHistory();
                    }
                    currentHistory = (Unified.Serialize.DeserializeXMLFromDisk(HistoryLocation, typeof(HistoryByFavorite)) as HistoryByFavorite);
                }
                loadingHistory = false;
            }
            if (currentHistory != null) if (OnHistoryLoaded != null) OnHistoryLoaded(currentHistory);
        }
        
        private void SaveHistory()
        {
            Unified.Serialize.SerializeXMLToDisk(CurrentHistory, HistoryLocation);            
        }
        public void RecordHistoryItem(string Name, bool Save)
        {
            List<HistoryItem> lst = null;
            if (currentHistory.ContainsKey(Name))
            {
                lst = currentHistory[Name];
            }
            if (lst == null)
            {
                lst = new List<HistoryItem>();
                currentHistory.Add(Name, lst);
            }
            lst.Add(new HistoryItem(Name));
            if (Save) this.SaveHistory();
        }

        HistoryByFavorite currentHistory = null;
        public HistoryByFavorite CurrentHistory {
            get
            {
                if (currentHistory == null) LoadHistory(null);
                return currentHistory;
            }
            set
            {
                currentHistory = value;
            }
        }
    }
}
