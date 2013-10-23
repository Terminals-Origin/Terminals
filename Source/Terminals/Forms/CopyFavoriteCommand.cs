using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms;

namespace Terminals
{
    internal class CopyFavoriteCommand
    {
        private readonly Func<InputBoxResult> copyPrompt;

        private IFavorite source;

        private readonly IFavorites favorites;

        private readonly FavoriteRenameCommand renameCommand;

        internal CopyFavoriteCommand(IPersistence persistence, Func<InputBoxResult> copyPrompt = null)
        {
            this.favorites = persistence.Favorites;
            var renameService = new RenameCopyService(persistence.Favorites);
            renameService.RenameAction = this.AddIfValid; // property injection
            this.renameCommand = new FavoriteRenameCommand(persistence, renameService);

            if (copyPrompt != null)
                this.copyPrompt = copyPrompt;
            else
                this.copyPrompt = () => InputBox.Show("Enter new name:", "Duplicate selected favorite as ...");
        }

        /// <summary>
        /// Creates deep copy of provided favorite in persistence including its toolbar button and groups memebership.
        /// The copy is already added to the persistence.
        /// Returns newly created favorite copy if operation was successfull; otherwise null.
        /// </summary>
        internal IFavorite Copy(IFavorite favorite)
        {
            if (favorite == null)
                return null;

            return this.CopyUsingPrompt(favorite);
        }

        private IFavorite CopyUsingPrompt(IFavorite favorite)
        {
            InputBoxResult result = this.copyPrompt();
            if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                return this.CopySelectedFavorite(favorite, result.Text);

            return null;
        }

        private IFavorite CopySelectedFavorite(IFavorite favorite, string newName)
        {
            this.source = favorite;
            IFavorite copy = favorite.Copy();
            // expecting the copy it self is valid, let validate only the name
            copy.Name = newName;
            
            bool valid = this.renameCommand.ValidateNewName(favorite, newName);
            if (!valid)
                return null;

            this.renameCommand.ApplyRename(copy, newName);
            return copy;
        }

        private void AddIfValid(IFavorite favorite, string newName)
        {
            this.favorites.Add(favorite);
            this.favorites.UpdateFavorite(favorite, this.source.Groups);

            if (Settings.HasToolbarButton(this.source.Id))
                Settings.AddFavoriteButton(favorite.Id);
        }
    }
}