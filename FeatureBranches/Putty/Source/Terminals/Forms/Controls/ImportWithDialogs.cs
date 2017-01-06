using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Converters;
using Terminals.Data;
using Terminals.Data.Interfaces;
using Terminals.Data.Validation;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Calls import, asking user what to do, if there are duplicit items to import.
    /// Handles showing result message box after import and wait cursor for import source form.
    /// </summary>
    internal class ImportWithDialogs
    {
        internal const String IMPORT_SUFFIX = "_(imported)";
        private readonly IImportUi importUi;
        private readonly IPersistence persistence;

        private readonly ConnectionManager connectionManager;

        private readonly IDataValidator validator;

        private IFavorites PersistedFavorites
        {
            get { return this.persistence.Favorites; }
        }

        internal ImportWithDialogs(Form sourceForm, IPersistence persistence, ConnectionManager connectionManager)
            : this(new FormsImportUi(sourceForm), persistence, connectionManager)
        {
        }

        internal ImportWithDialogs(IImportUi importUi, IPersistence persistence, ConnectionManager connectionManager)
        {
            this.importUi = importUi;
            this.persistence = persistence;
            this.validator = persistence.Factory.CreateValidator();
            this.connectionManager = connectionManager;
        }

        internal Boolean Import(List<FavoriteConfigurationElement> favoritesToImport)
        {
            this.importUi.ReportStart();
            int importedCount = ImportPreservingNames(favoritesToImport);
            this.importUi.ReportEnd();
            bool imported = importedCount > 0;
            if (imported)
                this.importUi.ShowResultMessage(importedCount);

            return imported;
        }

        private int ImportPreservingNames(List<FavoriteConfigurationElement> favoritesToImport)
        {
            List<FavoriteConfigurationElement> uniqueToImport = GetUniqueItemsToImport(favoritesToImport);
            List<FavoriteConfigurationElement> conflictingFavorites = GetConflictingFavorites(uniqueToImport);
            // using delegate allows us to call this method from unit tests, without use of UI
            DialogResult renameAnswer = this.importUi.AskIfOverwriteOrRename(conflictingFavorites.Count);
            if (renameAnswer == DialogResult.Yes)
                RenameConflictingFavorites(conflictingFavorites);

            if (renameAnswer != DialogResult.Cancel)
                return this.ImportAllToPeristence(uniqueToImport);

            return 0;
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

        private int ImportAllToPeristence(List<FavoriteConfigurationElement> configFavoritesToImport)
        {
            this.persistence.StartDelayedUpdate();
            int importedCount = 0;

            foreach (FavoriteConfigurationElement configFavorite in configFavoritesToImport)
            {
               var context = new ImportContext(configFavorite);
               this.TryProcessFavorite(context);
               if (context.Imported)
                    importedCount++;
            }

            this.persistence.SaveAndFinishDelayedUpdate();
            return importedCount;
        }

        private void TryProcessFavorite(ImportContext context)
        {
            context.ToPerisist = ModelConverterV1ToV2.ConvertToFavorite(context.ToImport, this.persistence, this.connectionManager);
            ValidationStates results = this.validator.Validate(context.ToPerisist);
            if (results.Empty)
                this.ProcessFavorite(context);
            else
                LogFavoriteImportError(results, context.ToPerisist);
        }

        private void ProcessFavorite(ImportContext context)
        {
            this.ImportToPersistence(context.ToPerisist);
            IEnumerable<string> validGroupNames = this.SelectValidGroupNames(context.ToImport);
            AddFavoriteIntoGroups(this.persistence, context.ToPerisist, validGroupNames);
            context.Imported = true;
        }
        
        private static void LogFavoriteImportError(ValidationStates results, IFavorite favorite)
        {
            string allErrors = results.ToOneMessage();
            string message = string.Format("Couldnt import invalid favorite: '{0}' because:\r\n{1}", favorite.Name, allErrors);
            Logging.Warn(message);
        }

        private IEnumerable<string> SelectValidGroupNames(FavoriteConfigurationElement toImport)
        {
            var validator = new GroupNameValidator(this.persistence);
            var tagsConverter = new TagsConverter();
            return tagsConverter.ResolveTagsList(toImport)
                .Where(groupName => string.IsNullOrEmpty(validator.ValidateNameValue(groupName)));
        }

        internal static void AddFavoriteIntoGroups(IPersistence persistence, IFavorite toPerisist, IEnumerable<string> validGroupNames)
        {
            foreach (string groupName in validGroupNames)
            {
                IGroup group = FavoritesFactory.GetOrAddNewGroup(persistence, groupName);
                group.AddFavorite(toPerisist);
            }
        }
        
        private void ImportToPersistence(IFavorite favorite)
        {
            IFavorite oldFavorite = this.PersistedFavorites[favorite.Name];
            if (oldFavorite != null) // force to override old favorite
            {
                oldFavorite.UpdateFrom(favorite);
                this.PersistedFavorites.Update(oldFavorite);
            }
            else
            {
                this.PersistedFavorites.Add(favorite);
            }
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
    }
}
