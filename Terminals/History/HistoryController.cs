﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace Terminals.History
{
    public class HistoryController
    {
        /// <summary>
        /// lets make it a static string, just in case later we want to override its location
        /// </summary>
        private static string _historyLocation = "History.xml";
        private object _threadLock = new object();
        private bool _loadingHistory = false;
        private HistoryByFavorite _currentHistory = null;
        
        public delegate void HistoryLoaded(HistoryByFavorite History);
        public event HistoryLoaded OnHistoryLoaded;
        
        public HistoryController()
        {
            CurrentHistory = new HistoryByFavorite();
        }

        /// <summary>
        /// Capture the OnHistoryLoaded Event
        /// </summary>
        public void LazyLoadHistory()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadHistory), null);
        }

        /// <summary>
        /// Load or re-load history from HistoryLocation
        /// </summary>
        private void LoadHistory(object threadState)
        {
            if (_loadingHistory)
                return;
            
            lock (_threadLock)
            {
                try
                {
                    Logging.Log.Info("Loading History:" + ((_historyLocation == null) ? "" : _historyLocation));
                    _loadingHistory = true;
                    //So, how to store history..that is the question. Should it be per computer or per terminals install (portable)? Maybe both?
                    //I think for now lets stick with the portable option.  history should follow the user around. KISS (KeepItSimpleStupid)

                    if (string.IsNullOrEmpty(_historyLocation))
                    {
                        _historyLocation = _historyLocation.Trim();

                        if (!File.Exists(_historyLocation))
                            SaveHistory();//the file doesnt exist.  lets save it out for the first time

                        _currentHistory = (Unified.Serialize.DeserializeXMLFromDisk(_historyLocation, typeof(HistoryByFavorite)) as HistoryByFavorite);
                    }
                    _loadingHistory = false;
                    Logging.Log.Info("Done Loading History");
                }
                catch (Exception exc)
                {
                    Logging.Log.Error("Error Loading History", exc);
                }
            }

            if (_currentHistory != null && OnHistoryLoaded != null) 
                OnHistoryLoaded(_currentHistory);
        }        
        private void SaveHistory()
        {
            try
            {
                Logging.Log.Info("Saving History");
                Unified.Serialize.SerializeXMLToDisk(CurrentHistory, _historyLocation);
                Logging.Log.Info("Done Saving History");
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Saving History", exc);
            }
        }
        public void RecordHistoryItem(string name, bool save)
        {
            List<HistoryItem> lst = null;
            if (lst == null)
                lst = new List<HistoryItem>();

            if (_currentHistory.ContainsKey(name))
                lst = _currentHistory[name];
            else
                _currentHistory.Add(name, lst);
            /*if (lst == null)
            {
                lst = new List<HistoryItem>();
                _currentHistory.Add(Name, lst);
            }*/

            lst.Add(new HistoryItem(name));
            
            if (save)
                this.SaveHistory();
            
            this.LazyLoadHistory();
        }
        public HistoryByFavorite CurrentHistory {
            get
            {
                if (_currentHistory == null) 
                    LoadHistory(null);
                return _currentHistory;
            }
            set
            {
                _currentHistory = value;
            }
        }
    }
}
