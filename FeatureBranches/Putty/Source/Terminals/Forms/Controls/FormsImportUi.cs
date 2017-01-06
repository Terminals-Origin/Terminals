using System;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    internal class FormsImportUi : IImportUi
    {
        private readonly Form parentForm;
        public FormsImportUi(Form parentForm)
        {
            this.parentForm = parentForm;
        }

        public void ReportStart()
        {
            this.parentForm.Cursor = Cursors.WaitCursor;
        }

        public void ReportEnd()
        {
            this.parentForm.Cursor = Cursors.Default;
        }

        public DialogResult AskIfOverwriteOrRename(Int32 conflictingFavoritesCount)
        {
            DialogResult overwriteResult = DialogResult.No;

            if (conflictingFavoritesCount > 0)
            {
                String messagePrefix = String.Format("There are {0} connections to import, which already exist.",
                                                     conflictingFavoritesCount);
                String message = messagePrefix +
                                 "\r\nDo you want to rename them?\r\n\r\nSelect" +
                                 "\r\n- Yes to rename the newly imported items with \"" + ImportWithDialogs.IMPORT_SUFFIX + "\" suffix" +
                                 "\r\n- No to overwrite existing items" +
                                 "\r\n- Cancel to interupt the import";

                overwriteResult = MessageBox.Show(message, "Terminals - conflict found in import",
                                                  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            }

            return overwriteResult;
        }

        public void ShowResultMessage(Int32 importedItemsCount)
        {
            String message = "1 item was added to your favorites.";
            if (importedItemsCount > 1)
                message = String.Format("{0} items were added to your favorites.", importedItemsCount);
            MessageBox.Show(message, "Terminals import result", MessageBoxButtons.OK);
        }
    }
}