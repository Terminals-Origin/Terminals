using System;
using System.Windows.Forms;
using Terminals.Forms.Controls;

namespace Tests.FilePersisted
{
    /// <summary>
    /// MOQ to communicate with the import
    /// </summary>
    internal class TestImportUi : IImportUi
    {
        private readonly Func<int, DialogResult> strategy;

        internal int Imported { get; private set; }

        internal int ConflictingFavoritesCount { get; private set; }

        public TestImportUi(Func<int, DialogResult> strategy)
        {
            this.strategy = strategy;
        }

        public void ReportStart()
        {
            // nothing to do here
        }

        public void ReportEnd()
        {
            // nothing to do here
        }

        public DialogResult AskIfOverwriteOrRename(int conflictingFavoritesCount)
        {
            this.ConflictingFavoritesCount = conflictingFavoritesCount;
            return this.strategy(conflictingFavoritesCount);
        }

        public void ShowResultMessage(int importedCount)
        {
            this.Imported = importedCount;
        }
    }
}