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
    internal delegate void HistoryRecorded(HistoryRecordedEventArgs args);

    internal sealed class ConnectionHistory : IConnectionHistory
    {
        private readonly Favorites favorites;
        private ManualResetEvent loadingGate = new ManualResetEvent(false);
        private DataFileWatcher fileWatcher;
        private HistoryByFavorite currentHistory = null;
        public event HistoryRecorded OnHistoryRecorded;

        /// <summary>
        /// Prevent concurrent updates on History file by another program
        /// </summary>
        private Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.History");

        internal ConnectionHistory(Favorites favorites)
        {
            this.favorites = favorites;
            fileWatcher = new DataFileWatcher(FileLocations.HistoryFullFileName);
            fileWatcher.FileChanged += new EventHandler(this.OnFileChanged);
            fileWatcher.StartObservation();
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadHistory));
        }

        private void OnFileChanged(object sender, EventArgs e)
        {
            // don't need locking here, because only today is updated adding new items
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
            List<IFavorite> newTodays = this.GetDateItems(HistoryIntervals.TODAY);
            if (oldTodays != null)
                newTodays = DataDispatcher.GetMissingFavorites(newTodays, oldTodays);
            return newTodays;
        }

        private SortableList<IFavorite> GetOldTodaysHistory()
        {
            SortableList<IFavorite> oldTodays = null;
            if (this.currentHistory != null)
                oldTodays = this.GetDateItems(HistoryIntervals.TODAY);
            return oldTodays;
        }

        /// <summary>
        /// Because file watcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            fileWatcher.AssignSynchronizer(synchronizer);
        }

        public SortableList<IFavorite> GetDateItems(string historyDateKey)
        {
            this.loadingGate.WaitOne();
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
            return this.currentHistory.GroupByDate();
        }

        /// <summary>
        /// Load or re-load history from HistoryLocation
        /// </summary>
        private void LoadHistory(object threadState)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                this.TryLoadFile();
                Debug.WriteLine("History Loaded. Duration:{0}ms", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error Loading History", exc);
            }
            finally
            {
                this.loadingGate.Set();
            }
        }

        private void TryLoadFile()
        {
            string fileName = FileLocations.HistoryFullFileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                Logging.Log.InfoFormat("Loading History from: {0}", fileName);
                if (File.Exists(fileName))
                {
                    LoadFile();
                    return;
                }
            }

            this.currentHistory = new HistoryByFavorite { Favorites = this.favorites };
        }

        private void LoadFile()
        {
            try
            {
                fileLock.WaitOne();
                var loadedHistory = Serialize.DeserializeXMLFromDisk(FileLocations.HistoryFullFileName, typeof(HistoryByFavorite)) as HistoryByFavorite;
                loadedHistory.Favorites = this.favorites;
                this.currentHistory = loadedHistory;
            }
            catch // fails also in case of history upgrade, we don't upgrade history file
            {
                this.currentHistory = new HistoryByFavorite{ Favorites = this.favorites};
            }
            finally 
            {
                fileLock.ReleaseMutex();
            }
        }

        private void SaveHistory()
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                Serialize.SerializeXMLToDisk(this.currentHistory, FileLocations.HistoryFullFileName);
                Logging.Log.Info(string.Format("History saved. Duration:{0} ms", stopwatch.ElapsedMilliseconds));
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
            this.loadingGate.WaitOne();
            if (this.currentHistory == null || favorite == null)
                return;

            List<HistoryItem> favoriteHistoryList = GetFavoriteHistoryList(favorite.Id);
            favoriteHistoryList.Add(new HistoryItem());
            this.SaveHistory();
            this.FireOnHistoryRecorded(favorite);
        }

        private void FireOnHistoryRecorded(IFavorite favorite)
        {
            if (this.OnHistoryRecorded != null)
            {
                var args = new HistoryRecordedEventArgs(favorite);
                this.OnHistoryRecorded(args);
            }
        }

        private List<HistoryItem> GetFavoriteHistoryList(Guid favoriteId)
        {
            if (!this.currentHistory.ContainsKey(favoriteId))
                this.currentHistory.Add(favoriteId, new List<HistoryItem>());

            return this.currentHistory[favoriteId];
        }
    }
}
