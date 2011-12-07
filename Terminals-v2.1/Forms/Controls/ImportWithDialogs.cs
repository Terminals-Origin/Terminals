using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Calls import, asking user what to do, if there are duplicit items to import.
    /// Handles showing result message box after import and wait cursor for import source form.
    /// </summary>
    internal class ImportWithDialogs
    {
        private const String importSuffix = "_(imported)";
        private Form sourceForm;

        private static Favorites PersistedFavorites
        {
            get { return Persistance.Instance.Favorites; }
        }

        internal ImportWithDialogs(Form sourceForm)
        {
            this.sourceForm = sourceForm;
        }

        internal Boolean Import(List<FavoriteConfigurationElement> favoritesToImport)
        {
            this.sourceForm.Cursor = Cursors.WaitCursor;
            bool imported = ImportPreservingNames(favoritesToImport);
            this.sourceForm.Cursor = Cursors.Default;
            if (imported)
                ShowImportResultMessage(favoritesToImport.Count);

            return imported;
        }

        private static void ShowImportResultMessage(Int32 importedItemsCount)
        {
            String message = "1 item was added to your favorites.";
            if (importedItemsCount > 1)
                message = String.Format("{0} items were added to your favorites.", importedItemsCount);
            MessageBox.Show(message, "Terminals import result", MessageBoxButtons.OK);
        }

        private Boolean ImportPreservingNames(List<FavoriteConfigurationElement> favoritesToImport)
        {
            List<FavoriteConfigurationElement> conflictingFavorites = GetConflictingFavorites(favoritesToImport);
            DialogResult renameAnswer = AskIfOverwriteOrRename(conflictingFavorites.Count);
            return this.PerformImport(favoritesToImport, conflictingFavorites, renameAnswer);
        }

        private Boolean PerformImport(List<FavoriteConfigurationElement> favoritesToImport,
            List<FavoriteConfigurationElement> conflictingFavorites, DialogResult renameAnswer)
        {
            if (renameAnswer == DialogResult.Yes)
                RenameConflictingFavorites(conflictingFavorites);

            if (renameAnswer != DialogResult.Cancel)
            {
                PersistedFavorites.AddFavorites(favoritesToImport);
                return true;
            }

            return false;
        }

        private void RenameConflictingFavorites(List<FavoriteConfigurationElement> conflictingFavorites)
        {
            FavoriteConfigurationElementCollection savedFavorites = PersistedFavorites.GetFavorites();
            foreach (FavoriteConfigurationElement favoriteToRename in conflictingFavorites)
            {
                this.AddImportSuffixToFavorite(favoriteToRename, savedFavorites);
            }
        }

        private void AddImportSuffixToFavorite(FavoriteConfigurationElement favoriteToRename,
                                    FavoriteConfigurationElementCollection savedFavorites)
        {
            favoriteToRename.Name += importSuffix;
            if (savedFavorites[favoriteToRename.Name] != null)
                this.AddImportSuffixToFavorite(favoriteToRename, savedFavorites);
        }

        private static DialogResult AskIfOverwriteOrRename(Int32 conflictingFavoritesCount)
        {
            DialogResult overwriteResult = DialogResult.No;

            if (conflictingFavoritesCount > 0)
            {
                String messagePrefix = String.Format("There are {0} connections to import, which already exist.",
                    conflictingFavoritesCount);
                String message = messagePrefix +
                            "\r\nDo you want to rename them?\r\n\r\nSelect" +
                            "\r\n- Yes to rename the newly imported items with \"" + importSuffix + "\" suffix" +
                            "\r\n- No to overwrite existing items" +
                            "\r\n- Cancel to interupt the import";

                overwriteResult = MessageBox.Show(message, "Terminals - conflict found in import",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            }

            return overwriteResult;
        }

        private static List<FavoriteConfigurationElement> GetConflictingFavorites(List<FavoriteConfigurationElement> favorites)
        {
            var conflictingFavorites = new List<FavoriteConfigurationElement>();
            FavoriteConfigurationElementCollection savedFavorites = PersistedFavorites.GetFavorites();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                if (savedFavorites[favorite.Name] != null)
                {
                    conflictingFavorites.Add(favorite);
                }
            }
            return conflictingFavorites;
        }
    }
}
