using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal class UpdateFavoriteWithRenameCommand
    {
        private readonly IPersistence persistence;

        private readonly IFavorites favorites;

        private readonly Func<string, bool> askUserIfWantsToOverwrite = AskUserIfWantsToOverwrite;

        public UpdateFavoriteWithRenameCommand(IPersistence persistence, Func<string, bool> askUserIfWantsToOverwrite = null)
        {
            this.persistence = persistence;
            this.favorites = persistence.Favorites;

            if (askUserIfWantsToOverwrite != null)
                this.askUserIfWantsToOverwrite = askUserIfWantsToOverwrite;
        }

        /// <summary>
        /// Asks user, if wants to overwrite already present favorite the newName by conflicting (editedFavorite).
        /// </summary>
        /// <param name="oldName">The olready present favorite name to check.</param>
        /// <param name="newName">The newly assigned name of edited favorite.</param>
        /// <param name="editedFavorite">The currently edited favorite to update.</param>
        internal void UpdateFavoritePreservingDuplicitNames(string oldName, string newName, IFavorite editedFavorite)
        {
            editedFavorite.Name = oldName; // to prevent find it self as oldFavorite
            var oldFavorite = this.favorites[newName];
            // prevent conflict with another favorite than edited
            if (oldFavorite != null && !editedFavorite.StoreIdEquals(oldFavorite))
            {
                this.OverwriteByConflictingName(newName, oldFavorite, editedFavorite);
            }
            else
            {
                editedFavorite.Name = newName;
                // dont have to update buttons here, because they arent changed
                this.favorites.Update(editedFavorite);
            }
        }

        private void OverwriteByConflictingName(string newName, IFavorite oldFavorite, IFavorite editedFavorite)
        {
            if (!askUserIfWantsToOverwrite(newName))
                return;

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

        private static bool AskUserIfWantsToOverwrite(string newName)
        {
            string message = String.Format("A connection named \"{0}\" already exists\r\nDo you want to overwrite it?", newName);
            return MessageBox.Show(message, Program.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}