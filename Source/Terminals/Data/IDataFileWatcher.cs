using System;
using System.ComponentModel;

namespace Terminals.Data
{
    /// <summary>
    /// Service representation, which observes file changes, reports file changed event,
    /// allowing it to raise in GUI thread.
    /// </summary>
    internal interface IDataFileWatcher
    {
        event EventHandler FileChanged;

        /// <summary>
        /// Because file watcher is created before the main form,
        /// the synchronization object has to be assigned later.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        void AssignSynchronizer(ISynchronizeInvoke synchronizer);

        void StopObservation();

        void StartObservation();
    }
}