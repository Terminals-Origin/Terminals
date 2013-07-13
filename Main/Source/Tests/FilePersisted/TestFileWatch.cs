using System;
using System.ComponentModel;
using System.Threading;
using Terminals.Data;

namespace Tests.FilePersisted
{
    /// <summary>
    /// Simulates file obserwation using ManualResetEvent.
    /// Used to serialize otherwise parallel work in unit tests, when working with OS file system.
    /// </summary>
    internal class TestFileWatch : IDataFileWatcher
    {
        public event EventHandler FileChanged;

        internal ManualResetEvent ObservationWatch { get; private set; }

        internal ManualResetEvent ReleaseWatch { get; private set; }

        internal TestFileWatch()
        {
            this.ObservationWatch = new ManualResetEvent(false);
            this.ReleaseWatch = new ManualResetEvent(false);
        }

        public void AssignSynchronizer(ISynchronizeInvoke synchronizer)
        {
            // nothing to do here
        }

        public void StopObservation()
        {
            // nothing to do here, the test finishes immediately
        }

        public void StartObservation()
        {
            // nothing to do here
            // dont fire the event directly from here, the file persistence starts the observation
        }

        internal void FireFileChanged()
        {
            // synchronize with primary persistence to wait till the changes are done
            this.ObservationWatch.WaitOne();
            if (this.FileChanged != null)
                this.FileChanged(this, EventArgs.Empty);
            this.ReleaseWatch.Set();
        }
    }
}