using System;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// User interface contract used to communicate the import
    /// </summary>
    internal interface IImportUi
    {
        void ReportStart();

        void ReportEnd();

        DialogResult AskIfOverwriteOrRename(Int32 conflictingFavoritesCount);

        void ShowResultMessage(int importedCount);
    }
}