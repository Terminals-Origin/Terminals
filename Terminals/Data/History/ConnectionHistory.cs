using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.IO;
using Terminals.Configuration;
using Terminals.Data;
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
        private const string _historyLocation = "History.xml";
        private object _threadLock = new object();
        private bool _loadingHistory = false;
        private DataFileWatcher fileWatcher;

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
        /// Prevent concurent updates on History file by another program
        /// </summary>
        private Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.History");

        #region Thread safe Singleton

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

        private ConnectionHistory()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullHistoryFullName = Path.Combine(directory, _historyLocation);
            fileWatcher = new DataFileWatcher(fullHistoryFullName);
            fileWatcher.FileChanged += new EventHandler(this.OnFileChanged);
            fileWatcher.StartObservation();
        }

        private void OnFileChanged(object sender, EventArgs e)
        {
            SortableList<FavoriteConfigurationElement> oldTodays = GetOldTodaysHistory();
            LoadHistory(null);
            List<FavoriteConfigurationElement> newTodays = MergeWithNewTodays(oldTodays);
            foreach (FavoriteConfigurationElement favorite in newTodays)
            {
                FireOnHistoryRecorded(favorite.Name);
            }
        }

        private List<FavoriteConfigurationElement> MergeWithNewTodays(SortableList<FavoriteConfigurationElement> oldTodays)
        {
            List<FavoriteConfigurationElement> newTodays = this.GetDateItems(HistoryByFavorite.TODAY);
            if (oldTodays != null)
                newTodays = DataDispatcher.GetMissingFavorites(newTodays, oldTodays);
            return newTodays;
        }

        private SortableList<FavoriteConfigurationElement> GetOldTodaysHistory()
        {
            SortableList<FavoriteConfigurationElement> oldTodays = null;
            if (this._currentHistory != null)
                oldTodays = this.GetDateItems(HistoryByFavorite.TODAY);
            return oldTodays;
        }

        private static class Nested
        {
            internal static readonly ConnectionHistory instance = new ConnectionHistory();
        }

        #endregion

        /// <summary>
        /// Because filewatcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            fileWatcher.AssignSynchronizer(synchronizer);
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
            if (!string.IsNullOrEmpty(_historyLocation))
            {
                Logging.Log.InfoFormat("Loading History from: {0}", _historyLocation);
                if (!File.Exists(_historyLocation))
                    this.SaveHistory();//the file doesnt exist. Lets save it out for the first time
                else
                    LoadFile();
            }
        }

        private void LoadFile()
        {
            fileLock.WaitOne();
            this._currentHistory = Serialize.DeserializeXMLFromDisk(_historyLocation, typeof(HistoryByFavorite)) as HistoryByFavorite;
            fileLock.ReleaseMutex();
        }

        private void TryToRecoverHistoryFile()
        {
            try
            {
                string backupFile = GetBackupFileName();
                fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                File.Copy(_historyLocation, backupFile);
                if (File.Exists(_historyLocation))
                    File.Delete(_historyLocation);

                Logging.Log.InfoFormat("History file recovered, backup in {0}", backupFile);
                this._currentHistory = new HistoryByFavorite();
            }
            catch (Exception ex1)
            {
                Logging.Log.Error("Try to recover History file failed.", ex1);
            }
            finally
            {
                fileLock.ReleaseMutex();
                this.fileWatcher.StartObservation();
            }
        }

        private static string GetBackupFileName()
        {
            string fileDate = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            return String.Format("{0}{1}.bak", fileDate, _historyLocation);
        }

        private void SaveHistory()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                Serialize.SerializeXMLToDisk(CurrentHistory, _historyLocation);

                sw.Stop();
                Logging.Log.Info(string.Format("History saved. Duration:{0} ms", sw.ElapsedMilliseconds));
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Saving History", exc);
            }
            finally
            {
                fileLock.ReleaseMutex();
                this.fileWatcher.StartObservation();
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
            var args = new HistoryRecordedEventArgs { ConnectionName = favoriteName };
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
