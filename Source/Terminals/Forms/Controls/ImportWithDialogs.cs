using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

        private static IFavorites PersistedFavorites
        {
            get { return Persistence.Instance.Favorites; }
        }

        private static IGroups PersistedGroups
        {
            get { return Persistence.Instance.Groups; }
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
            if (renameAnswer == DialogResult.Yes)
                RenameConflictingFavorites(conflictingFavorites);
                  
            if (renameAnswer != DialogResult.Cancel)
            {
                PerformImport(favoritesToImport);
                return true;
            }

            return false;
        }

        private static void PerformImport(List<FavoriteConfigurationElement> configFavoritesToImport)
        {
            var favorites = new List<IFavorite>();
            Persistence.Instance.StartDelayedUpdate();
            foreach (FavoriteConfigurationElement configFavorite in configFavoritesToImport)
            {
                IFavorite favorite = PrepareFavoriteToImport(configFavorite);
                favorites.Add(favorite);
                AddFavoriteIntoGroups(configFavorite, favorite);
            }

            PersistedFavorites.Add(favorites);
            Persistence.Instance.SaveAndFinishDelayedUpdate();
        }

        internal static void AddFavoriteIntoGroups(FavoriteConfigurationElement configFavorite, IFavorite favorite)
        {
            foreach (string groupName in configFavorite.TagList)
            {
                IGroup group = FavoritesFactory.GetOrCreateGroup(groupName);
                group.AddFavorite(favorite);
            }
        }

        private static IFavorite PrepareFavoriteToImport(FavoriteConfigurationElement configFavorite)
        {
            var favorite = ModelConverterV1ToV2.ConvertToFavorite(configFavorite);
            var oldFavorite = PersistedFavorites[favorite.Name];
            if (oldFavorite != null) // force to override old favorite
                favorite.Id = oldFavorite.Id;
            return favorite;
        }

        private void RenameConflictingFavorites(List<FavoriteConfigurationElement> conflictingFavorites)
        {
            foreach (FavoriteConfigurationElement favoriteToRename in conflictingFavorites)
            {
                this.AddImportSuffixToFavorite(favoriteToRename);
            }
        }

        private void AddImportSuffixToFavorite(FavoriteConfigurationElement favoriteToRename)
        {
            favoriteToRename.Name += importSuffix;
            if (PersistedFavorites[favoriteToRename.Name] != null)
                this.AddImportSuffixToFavorite(favoriteToRename);
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

            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                if (PersistedFavorites[favorite.Name] != null)
                {
                    conflictingFavorites.Add(favorite);
                }
            }
            return conflictingFavorites;
        }
    }
}
