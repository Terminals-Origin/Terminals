using System;
using Terminals.Data;

namespace Terminals
{
    internal class RenameCopyService : RenameService
    {
        /// <summary>
        /// Gets or sets the action to perform. Used as service property injection.
        /// </summary>
        internal Action<IFavorite, string> RenameAction { get; set; }

        public RenameCopyService(IFavorites favorites)
            : base(favorites)
        {
        }

        public override void Rename(IFavorite favorite, string newName)
        {
            this.RenameAction(favorite, newName);
        }
    }
}