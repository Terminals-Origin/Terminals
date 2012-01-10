using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        /// Gets the file name of stored history values
        /// </summary>
        internal const string FILE_NAME = "History.xml";
        private static string FullFileName
        {
            get { return FileLocations.GetFullPath(FILE_NAME); }
        }

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

        internal ConnectionHistory()
        {
            fileWatcher = new DataFileWatcher(FullFileName);
            fileWatcher.FileChanged += new EventHandler(this.OnFileChanged);
            fileWatcher.StartObservation();
        }

        private void OnFileChanged(object sender, EventArgs e)
        {
            SortableList<IFavorite> oldTodays = GetOldTodaysHistory();
            LoadHistory(null);
            List<IFavorite> newTodays = MergeWithNewTodays(oldTodays);
            foreach (IFavorite favorite in newTodays)
            {
                FireOnHistoryRecorded(favorite);
            }
        }

        private List<IFavorite> MergeWithNewTodays(SortableList<IFavorite> oldTodays)
        {
            List<IFavorite> newTodays = this.GetDateItems(HistoryByFavorite.TODAY);
            if (oldTodays != null)
                newTodays = DataDispatcher.GetMissingFavorites(newTodays, oldTodays);
            return newTodays;
        }

        private SortableList<IFavorite> GetOldTodaysHistory()
        {
            SortableList<IFavorite> oldTodays = null;
            if (this._currentHistory != null)
                oldTodays = this.GetDateItems(HistoryByFavorite.TODAY);
            return oldTodays;
        }

        /// <summary>
        /// Because filewatcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            fileWatcher.AssignSynchronizer(synchronizer);
        }

        internal SortableList<IFavorite> GetDateItems(string historyDateKey)
        {
            var historyGroupItems = GetGroupedByDate()[historyDateKey];
            var groupFavorites = SelectFavoritesFromHistoryItems(historyGroupItems);
            return Favorites.OrderByDefaultSorting(groupFavorites);
        }

        private static SortableList<IFavorite> SelectFavoritesFromHistoryItems(
            SortableList<IHistoryItem> groupedByDate)
        {
            var selection = new SortableList<IFavorite>();
            foreach (IHistoryItem favoriteTouch in groupedByDate)
            {
                IFavorite favorite = favoriteTouch.Favorite;
                if (favorite != null && !selection.Contains(favorite)) // add each favorite only once
                    selection.Add(favorite);
            }

            return selection;
        }

        private SerializableDictionary<string, SortableList<IHistoryItem>> GetGroupedByDate()
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
            string fileName = FullFileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                Logging.Log.InfoFormat("Loading History from: {0}", fileName);
                if (!File.Exists(fileName))
                    this.SaveHistory();//the file doesnt exist. Lets save it out for the first time
                else
                    LoadFile();
            }
        }

        private void LoadFile()
        {
            fileLock.WaitOne();
            this._currentHistory = Serialize.DeserializeXMLFromDisk(FullFileName, typeof(HistoryByFavorite)) as HistoryByFavorite;
            fileLock.ReleaseMutex();
        }

        private void TryToRecoverHistoryFile()
        {
            try
            {
                string fileName = FullFileName;
                string backupFile = GetBackupFileName();
                fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                File.Copy(fileName, backupFile);
                if (File.Exists(fileName))
                    File.Delete(fileName);

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
            return String.Format("{0}{1}.bak", fileDate, FILE_NAME);
        }

        private void SaveHistory()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                Serialize.SerializeXMLToDisk(CurrentHistory, FullFileName);

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

        public void RecordHistoryItem(IFavorite favorite)
        {
            if (_currentHistory == null || favorite == null)
                return;

            List<HistoryItem> favoriteHistoryList = GetFavoriteHistoryList(favorite.Id);
            favoriteHistoryList.Add(new HistoryItem());
            this.SaveHistory();
            this.FireOnHistoryRecorded(favorite);
        }

        private void FireOnHistoryRecorded(IFavorite favorite)
        {
            var args = new HistoryRecordedEventArgs(favorite);
            if (this.OnHistoryRecorded != null)
            {
                this.OnHistoryRecorded(this, args);
            }
        }

        private List<HistoryItem> GetFavoriteHistoryList(Guid favoriteId)
        {
            if (!this._currentHistory.ContainsKey(favoriteId))
                this._currentHistory.Add(favoriteId, new List<HistoryItem>());

            return this._currentHistory[favoriteId];
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
