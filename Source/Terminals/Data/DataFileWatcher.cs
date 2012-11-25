using System;
using System.ComponentModel;
using System.IO;

namespace Terminals.Data
{
    /// <summary>
    /// Detects data or configuration file changes done 
    /// by another application or Terminals instance and reports them.
    /// Raises events in GUI thread, so no Invoke is required.
    /// </summary>
    internal class DataFileWatcher
    {
        internal event EventHandler FileChanged;
        private FileSystemWatcher fileWatcher;

        private string fullfileName;
        internal string FullFileName
        {
            get
            {
                return this.fullfileName;
            }
            set
            {
                this.fullfileName = value;
                SetFileToWatch(value);
            }
        }

        internal DataFileWatcher(string fullFileName)
        {
            InitializeFileWatcher();
            this.FullFileName = fullFileName;
        }

        /// <summary>
        /// Because file watcher is created before the main form,
        /// the synchronization object has to be assigned later.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal void AssignSynchronizer(ISynchronizeInvoke synchronizer)
        {
            this.fileWatcher.SynchronizingObject = synchronizer;
        }

        private void InitializeFileWatcher()
        {
            if (fileWatcher != null)
                return;

            fileWatcher = new FileSystemWatcher();
            SetFileToWatch(this.FullFileName);
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                                       NotifyFilters.CreationTime | NotifyFilters.Size;
            fileWatcher.Changed += new FileSystemEventHandler(ConfigFileChanged);
        }

        private void SetFileToWatch(string fullFileName)
        {
            this.fileWatcher.Path = Path.GetDirectoryName(fullFileName);
            this.fileWatcher.Filter = Path.GetFileName(fullFileName);
        }

        private void ConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            Logging.Log.DebugFormat("{0} file change by another application (or Terminals instance) detected!",
                                    this.FullFileName);
            if (FileChanged != null && fileWatcher.SynchronizingObject != null)
                FileChanged(this.FullFileName, new EventArgs());
        }

        internal void StopObservation()
        {
            fileWatcher.EnableRaisingEvents = false;
        }

        internal void StartObservation()
        {
            fileWatcher.EnableRaisingEvents = true;
        }
    }
}
