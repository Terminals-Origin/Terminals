using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms;

namespace Terminals
{
    internal class CopyFavoriteUI
    {
        private readonly Func<InputBoxResult> copyPrompt;

        private readonly IPersistence persistence;

        internal CopyFavoriteUI(IPersistence persistence, Func<InputBoxResult> copyPrompt = null)
        {
            this.persistence = persistence;

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

        private IFavorite source;

        private IFavorite CopySelectedFavorite(IFavorite favorite, string newName)
        {
            this.source = favorite;
            IFavorite copy = favorite.Copy();
            // expecting the copy it self is valid, let validate only the name
            copy.Name = newName;
            var renameUI = new FavoriteRenameUI(this.persistence, this.AddIfValid);
            bool valid = renameUI.ValidateFavoriteName(copy, newName);
            
            if (valid)
              return copy;

            return null;
        }

        private void AddIfValid(IFavorite favorite, string newName)
        {
            var favorites = this.persistence.Favorites;
            favorites.Add(favorite);
            favorites.UpdateFavorite(favorite, this.source.Groups);

            if (Settings.HasToolbarButton(this.source.Id))
                Settings.AddFavoriteButton(favorite.Id);
        }
    }
}