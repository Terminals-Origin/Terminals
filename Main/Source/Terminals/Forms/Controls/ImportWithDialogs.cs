using System;
using System.Collections.Generic;
using System.Linq;
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
        private const String IMPORT_SUFFIX = "_(imported)";
        private readonly Form sourceForm;
        private readonly IPersistence persistence;

        private IFavorites PersistedFavorites
        {
            get { return this.persistence.Favorites; }
        }

        internal ImportWithDialogs(Form sourceForm)
            : this(sourceForm, Persistence.Instance)
        {
        }
        
        internal ImportWithDialogs(Form sourceForm, IPersistence persistence)
        {
            this.sourceForm = sourceForm;
            this.persistence = persistence;
        }
        internal Boolean Import(List<FavoriteConfigurationElement> favoritesToImport)
        {
            this.sourceForm.Cursor = Cursors.WaitCursor;
            bool imported = ImportPreservingNames(favoritesToImport, AskIfOverwriteOrRename);
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

        private Boolean ImportPreservingNames(List<FavoriteConfigurationElement> favoritesToImport,
            Func<int, DialogResult> askIfOverwriteOrRename)
        {
            List<FavoriteConfigurationElement> uniqueToImport = GetUniqueItemsToImport(favoritesToImport);
            List<FavoriteConfigurationElement> conflictingFavorites = GetConflictingFavorites(uniqueToImport);
            // using delegate allows us to call this method from unit tests, without use of UI
            DialogResult renameAnswer = askIfOverwriteOrRename(conflictingFavorites.Count);
            if (renameAnswer == DialogResult.Yes)
                RenameConflictingFavorites(conflictingFavorites);

            if (renameAnswer != DialogResult.Cancel)
            {
                PerformImport(uniqueToImport);
                return true;
            }

            return false;
        }

        private List<FavoriteConfigurationElement> GetConflictingFavorites(List<FavoriteConfigurationElement> favorites)
        {
            // an item may appear more then once in persistence and also in the import file
            return favorites.Where(favorite => this.NotUniqueInPersistence(favorite.Name))
                .ToList();
        }

        private static List<FavoriteConfigurationElement> GetUniqueItemsToImport(List<FavoriteConfigurationElement> favorites)
        {
            // respect all other comparisons to keep unique names case insensitive
            return favorites.GroupBy(favorite => favorite.Name, StringComparer.CurrentCultureIgnoreCase)
                .SelectMany(group => group.Take(1))
                .ToList();
        }

        private void PerformImport(List<FavoriteConfigurationElement> configFavoritesToImport)
        {
            this.persistence.StartDelayedUpdate();

            foreach (FavoriteConfigurationElement configFavorite in configFavoritesToImport)
            {
                IFavorite favorite = this.PrepareFavoriteToImport(configFavorite);
                AddFavoriteIntoGroups(configFavorite, favorite);
            }

            this.persistence.SaveAndFinishDelayedUpdate();
        }

        internal static void AddFavoriteIntoGroups(FavoriteConfigurationElement configFavorite, IFavorite favorite)
        {
            foreach (string groupName in configFavorite.TagList)
            {
                IGroup group = FavoritesFactory.GetOrAddNewGroup(groupName);
                group.AddFavorite(favorite);
            }
        }

        private IFavorite PrepareFavoriteToImport(FavoriteConfigurationElement configFavorite)
        {
            var favorite = ModelConverterV1ToV2.ConvertToFavorite(configFavorite, this.persistence);
            var oldFavorite = this.PersistedFavorites[favorite.Name];
            if (oldFavorite != null) // force to override old favorite
            {
                oldFavorite.UpdateFrom(favorite);
                this.PersistedFavorites.Update(oldFavorite);
            }
            else
            {
                this.PersistedFavorites.Add(favorite);
            }
            
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
            favoriteToRename.Name += IMPORT_SUFFIX;
            if (NotUniqueInPersistence(favoriteToRename.Name))
                this.AddImportSuffixToFavorite(favoriteToRename);
        }

        private bool NotUniqueInPersistence(string favoriteName)
        {
            return this.PersistedFavorites[favoriteName] != null;
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
                            "\r\n- Yes to rename the newly imported items with \"" + IMPORT_SUFFIX + "\" suffix" +
                            "\r\n- No to overwrite existing items" +
                            "\r\n- Cancel to interupt the import";

                overwriteResult = MessageBox.Show(message, "Terminals - conflict found in import",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            }

            return overwriteResult;
        }
    }
}
