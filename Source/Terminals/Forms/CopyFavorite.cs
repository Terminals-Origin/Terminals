using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms;

namespace Terminals
{
    internal class CopyFavorite
    {
        private readonly Func<InputBoxResult> copyPrompt;

        private readonly IFavorites favorites;

        internal CopyFavorite(IFavorites favorites, Func<InputBoxResult> copyPrompt = null)
        {
            this.favorites = favorites;
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
            if (favorite != null)
            {
                InputBoxResult result = this.copyPrompt();
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    return this.CopySelectedFavorite(favorite, result.Text);
                }
            }

            return null;
        }

        private IFavorite CopySelectedFavorite(IFavorite favorite, string newName)
        {
            IFavorite copy = favorite.Copy();
            copy.Name = newName;
            // todo validate the favorite name
            this.favorites.Add(copy);
            this.favorites.UpdateFavorite(copy, favorite.Groups);

            if (Settings.HasToolbarButton(favorite.Id))
                Settings.AddFavoriteButton(copy.Id);
            return copy;
        }
    }
}