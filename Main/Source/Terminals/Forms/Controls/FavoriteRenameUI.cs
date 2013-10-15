using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Data.Validation;

namespace Terminals
{
    /// <summary>
    /// Performs UI action to rename the favorite or shows an messgae box, if favorite is not valid
    /// </summary>
    internal class FavoriteRenameUI
    {
        private readonly IPersistence persistence;

        private readonly Action<IFavorite, string> renameAction;

        private readonly FavoriteNameValidator validator;

        public FavoriteRenameUI(IPersistence persistence, Action<IFavorite, string> renameAction)
        {
            this.persistence = persistence;
            this.renameAction = renameAction;
            this.validator = new FavoriteNameValidator(this.persistence);
        }

        internal bool ValidateFavoriteName(IFavorite favorite, string newName)
        {
            string errorMessage = this.validator.ValidateCurrent(favorite, newName);
            bool valid = string.IsNullOrEmpty(errorMessage);
            if (valid)
                this.renameAction(favorite, newName);
            else
                MessageBox.Show(errorMessage, "Favorite name is not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return valid;
        }
    }
}