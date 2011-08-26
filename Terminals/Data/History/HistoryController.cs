using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                System.Diagnostics.Stopwatch sw = new Stopwatch();

                try
                {
                    sw.Start();
                    Logging.Log.Info("Loading History:" + ((_historyLocation == null) ? "" : _historyLocation));
                    _loadingHistory = true;
                    //So, how to store history..that is the question. Should it be per computer or per terminals install (portable)? Maybe both?
                    //I think for now lets stick with the portable option.  history should follow the user around. KISS (KeepItSimpleStupid)

                    if (!string.IsNullOrEmpty(_historyLocation))
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
                    try
                    {
                        Logging.Log.Info("Lets try to move the history file and kill it for now");
                        string backupFile = _historyLocation + System.DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".bak";
                        System.IO.File.Copy(_historyLocation, backupFile);
                        Logging.Log.Info("History file copied to:" + backupFile);
                        Logging.Log.Info("Try to delete original history file:" + _historyLocation);
                        if (System.IO.File.Exists(_historyLocation)) System.IO.File.Delete(_historyLocation);
                        Logging.Log.Info("Delete finished, moving on.");
                        _currentHistory = new HistoryByFavorite();
                    }
                    catch (Exception ex1)
                    {
                        Logging.Log.Info("This failed, again: Lets try to move the history file and kill it for now", ex1);
                    }

                }
                finally
                {
                    sw.Stop();
                    Logging.Log.Info(string.Format("Load History Duration:{0}ms", sw.ElapsedMilliseconds));
                }
            }

            if (_currentHistory != null && OnHistoryLoaded != null) 
                OnHistoryLoaded(_currentHistory);
        }        
        private void SaveHistory()
        {
            System.Diagnostics.Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                Logging.Log.Info("Saving History");
                Unified.Serialize.SerializeXMLToDisk(CurrentHistory, _historyLocation);
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
        public void RecordHistoryItem(string name, bool save)
        {
            if (_currentHistory == null)
                return;

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
