using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals
{
    internal class RenameService : IRenameService
    {
        private readonly IFavorites favorites;

        public RenameService(IFavorites favorites)
        {
            this.favorites = favorites;
        }

        public virtual void Rename(IFavorite favorite, string newName)
        {
            favorite.Name = newName;
            // dont have to update buttons here, because they arent changed
            this.favorites.Update(favorite);
        }

        public virtual bool AskUserIfWantsToOverwrite(string newName)
        {
            string message = String.Format("A connection named \"{0}\" already exists\r\nDo you want to overwrite it?", newName);
            return MessageBox.Show(message, Program.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public virtual void ReportInvalidName(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Favorite name is not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}