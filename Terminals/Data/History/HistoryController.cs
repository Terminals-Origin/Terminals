using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Unified;

namespace Terminals.History
{
    internal delegate void HistoryLoaded(HistoryByFavorite History);
    
    internal class HistoryController
    {
        /// <summary>
        ///So, how to store history..that is the question. Should it be per computer or per terminals install (portable)? Maybe both?
        ///I think for now lets stick with the portable option.  history should follow the user around. KISS (KeepItSimpleStupid)
        /// </summary>
        private static string _historyLocation = "History.xml";
        private object _threadLock = new object();
        private bool _loadingHistory = false;

        private HistoryByFavorite _currentHistory = null;
        public HistoryByFavorite CurrentHistory
        {
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
        
        public event HistoryLoaded OnHistoryLoaded;

        public HistoryController()
        {
            CurrentHistory = new HistoryByFavorite();
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
                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    TryLoadHistory();
                }
                catch (Exception exc)
                {
                    Logging.Log.Error("Error Loading History", exc);
                    TryToRecoverHistoryFile();
                }
                finally
                {
                    Logging.Log.Info(string.Format("Load History Duration:{0}ms", sw.ElapsedMilliseconds));
                }
            }

            if (_currentHistory != null && OnHistoryLoaded != null)
                OnHistoryLoaded(_currentHistory);
        }

        private void TryLoadHistory()
        {
            Logging.Log.Info("Loading History from: " + (_historyLocation == null ? String.Empty : _historyLocation));
            this._loadingHistory = true;

            if (!string.IsNullOrEmpty(_historyLocation))
            {
                _historyLocation = _historyLocation.Trim();

                if (!File.Exists(_historyLocation))
                    this.SaveHistory();//the file doesnt exist.  lets save it out for the first time

                this._currentHistory = Serialize.DeserializeXMLFromDisk(_historyLocation, typeof(HistoryByFavorite)) as HistoryByFavorite;
            }
            this._loadingHistory = false;

            Logging.Log.Info("Done Loading History");
        }

        private void TryToRecoverHistoryFile()
        {
            try
            {
                Logging.Log.Info("Lets try to move the history file and kill it for now");
                string backupFile = _historyLocation + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".bak";
                File.Copy(_historyLocation, backupFile);
                Logging.Log.Info("History file copied to:" + backupFile);
                Logging.Log.Info("Try to delete original history file:" + _historyLocation);
                if (File.Exists(_historyLocation))
                    File.Delete(_historyLocation);
                Logging.Log.Info("Delete finished, moving on.");
                this._currentHistory = new HistoryByFavorite();
            }
            catch (Exception ex1)
            {
                Logging.Log.Info("This failed, again: Lets try to move the history file and kill it for now", ex1);
            }
        }

        private void SaveHistory()
        {
            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                Logging.Log.Info("Saving History");
                Serialize.SerializeXMLToDisk(CurrentHistory, _historyLocation);
                Logging.Log.Info("Done Saving History");
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Saving History", exc);
            }
            finally
            {
                sw.Stop();
                Logging.Log.Info(string.Format("Save History Duration:{0}ms", sw.ElapsedMilliseconds));
            }
        }
        
        public void RecordHistoryItem(string favoriteName, bool save)
        {
            if (_currentHistory == null)
                return;

            List<HistoryItem> favoriteHistoryList = GetFavoriteHistoryList(favoriteName);
            favoriteHistoryList.Add(new HistoryItem(favoriteName));

            if (save)
                this.SaveHistory();

            this.LazyLoadHistory();
        }

        private List<HistoryItem> GetFavoriteHistoryList(string favoriteName)
        {
            if (!this._currentHistory.ContainsKey(favoriteName))
                this._currentHistory.Add(favoriteName, new List<HistoryItem>());

            return this._currentHistory[favoriteName];
        }

        /// <summary>
        /// Capture the OnHistoryLoaded Event
        /// </summary>
        public void LazyLoadHistory()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadHistory), null);
        }
    }
}
