using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Terminals.Configuration;
using Unified;

namespace Terminals.History
{
    internal delegate void HistoryRecorded(ConnectionHistory sender, HistoryRecordedEventArgs args);

    internal sealed class ConnectionHistory
    {
        /// <summary>
        ///So, how to store history..that is the question. Should it be per computer or per terminals install (portable)? Maybe both?
        ///I think for now lets stick with the portable option.  history should follow the user around. KISS (KeepItSimpleStupid)
        /// </summary>
        private static string _historyLocation = "History.xml";
        private object _threadLock = new object();
        private bool _loadingHistory = false;

        private HistoryByFavorite _currentHistory = null;
        /// <summary>
        /// Gets or sets the private field encapsulating the lazy loading
        /// </summary>
        private HistoryByFavorite CurrentHistory
        {
            get
            {
                if (_currentHistory == null)
                    LoadHistory(null);

                if (_currentHistory == null)
                    _currentHistory = new HistoryByFavorite();

                return _currentHistory;
            }
        }
        
        internal event EventHandler OnHistoryLoaded;
        internal event HistoryRecorded OnHistoryRecorded;

        /// <summary>
        /// Gets the singleton instance of history provider
        /// </summary>
        public static ConnectionHistory Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private ConnectionHistory() { }

        private static class Nested
        {
            internal static readonly ConnectionHistory instance = new ConnectionHistory();
        }

        internal SortableList<FavoriteConfigurationElement> GetDateItems(string historyDateKey)
        {
            var historyGroupItems = GetGroupedByDate()[historyDateKey];
            var groupFavorites = SelectFavoritesFromHistoryItems(historyGroupItems);
            return FavoriteConfigurationElementCollection.OrderByDefaultSorting(groupFavorites);
        }

        private static SortableList<FavoriteConfigurationElement> SelectFavoritesFromHistoryItems(
            SortableList<HistoryItem> groupedByDate)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            var selection = new SortableList<FavoriteConfigurationElement>();
            foreach (HistoryItem favoriteTouch in groupedByDate)
            {
                FavoriteConfigurationElement favorite = favorites[favoriteTouch.Name];
                if (favorite != null && !selection.Contains(favorite))
                    selection.Add(favorite);
            }

            return selection;
        }

        private SerializableDictionary<string, SortableList<HistoryItem>> GetGroupedByDate()
        {
            return this.CurrentHistory.GroupByDate();
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
                    this._loadingHistory = true;
                    TryLoadHistory();
                }
                catch (Exception exc)
                {
                    Logging.Log.Error("Error Loading History", exc);
                    TryToRecoverHistoryFile();
                }
                finally
                {
                    this._loadingHistory = false;
                    Logging.Log.Info(string.Format("Load History Duration:{0}ms", sw.ElapsedMilliseconds));
                }
            }

            if (_currentHistory != null && OnHistoryLoaded != null)
                OnHistoryLoaded(this, new EventArgs());
        }

        private void TryLoadHistory()
        {
            Logging.Log.Info("Loading History from: " + (_historyLocation == null ? String.Empty : _historyLocation));

            if (!string.IsNullOrEmpty(_historyLocation))
            {
                _historyLocation = _historyLocation.Trim();

                if (!File.Exists(_historyLocation))
                    this.SaveHistory();//the file doesnt exist.  lets save it out for the first time
                else
                    this._currentHistory = Serialize.DeserializeXMLFromDisk(_historyLocation, typeof(HistoryByFavorite)) as HistoryByFavorite;
            }

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
        
        public void RecordHistoryItem(string favoriteName)
        {
            if (_currentHistory == null)
                return;

            List<HistoryItem> favoriteHistoryList = GetFavoriteHistoryList(favoriteName);
            favoriteHistoryList.Add(new HistoryItem(favoriteName));
            this.SaveHistory();
            this.FireOnHistoryRecorded(favoriteName);
        }

        private void FireOnHistoryRecorded(string favoriteName)
        {
            var args = new HistoryRecordedEventArgs {ConnectionName = favoriteName};
            if (this.OnHistoryRecorded != null)
            {
                this.OnHistoryRecorded(this, args);
            }
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
        public void LoadHistoryAsync()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadHistory), null);
        }
    }
}
