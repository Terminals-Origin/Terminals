using System.Collections.Generic;
using Terminals.Data;
using Terminals.Data.Validation;

namespace Terminals
{
    internal class UpdateFavoriteWithRenameCommand
    {
        private readonly IPersistence persistence;
        private readonly IFavorites favorites;

        private readonly IRenameService service;
        private readonly FavoriteNameValidator validator;

        public UpdateFavoriteWithRenameCommand(IPersistence persistence, IRenameService service)
        {
            this.persistence = persistence;
            this.service = service;
            this.favorites = persistence.Favorites;
            this.validator = new FavoriteNameValidator(this.persistence);
        }

        internal bool ValidateNewName(IFavorite favorite, string newName)
        {
            // dont validate, against persistence, validate only the newName
            string errorMessage = this.validator.ValidateNameValue(newName);
            bool valid = string.IsNullOrEmpty(errorMessage);
            if (!valid)
                this.service.ReportInvalidName(errorMessage);

            bool unique = !this.validator.NotUniqueInPersistence(favorite, newName);
            if (!unique)
              unique = this.service.AskUserIfWantsToOverwrite(newName);

            return valid && unique;
        }

        /// <summary>
        /// Asks user, if wants to overwrite already present favorite the newName by conflicting (editedFavorite)
        /// and then take an action asigned to be performed as rename.
        /// </summary>
        /// <param name="editedFavorite">The currently edited favorite to update.</param>
        /// <param name="newName">The newly assigned name of edited favorite.</param>
        internal void ApplyRename(IFavorite editedFavorite, string newName)
        {
            IFavorite oldFavorite = this.favorites[newName];
            // prevent conflict with another favorite than edited
            bool notUnique = oldFavorite != null && !editedFavorite.StoreIdEquals(oldFavorite);
            if (notUnique)
                this.OverwriteByConflictingName(newName, oldFavorite, editedFavorite);
            else
                this.service.Rename(editedFavorite, newName);
        }

        private void OverwriteByConflictingName(string newName, IFavorite oldFavorite, IFavorite editedFavorite)
        {
            this.persistence.StartDelayedUpdate();
            // remember the edited favorite groups, because delete may also delete its groups,
            // if it is the last favorite in the group
            List<IGroup> groups = editedFavorite.Groups;
            editedFavorite.Name = newName;
            oldFavorite.UpdateFrom(editedFavorite);
            this.favorites.Update(oldFavorite);
            this.favorites.UpdateFavorite(oldFavorite, groups);
            this.favorites.Delete(editedFavorite);
            this.persistence.SaveAndFinishDelayedUpdate();
        }
    }
}