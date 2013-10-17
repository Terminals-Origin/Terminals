using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Tests.FilePersisted;

namespace Tests.Commands
{
    [TestClass]
    public class UpdateFavoriteWithRenameCommandTests : FilePersistedTestLab
    {
        private const string ORIGINAL_NAME = "Original";
        private const string COPY_NAME = "Copy";

        private const string RENAMED_NAME = "Renamed";

        private IFavorite original;

        private IFavorite copy;

        private UpdateFavoriteWithRenameCommand command;

        [TestInitialize]
        public void InitializeFavorites()
        {
            this.original = this.AddFavorite(ORIGINAL_NAME);
            this.copy = this.AddFavorite(COPY_NAME);
            // command is set to rename by default
            this.command = new UpdateFavoriteWithRenameCommand(this.Persistence, newName => true);
        }

        [TestMethod]
        public void NoDuplicitProperlyRenames()
        {
            this.command.UpdateFavoritePreservingDuplicitNames(COPY_NAME, RENAMED_NAME, this.copy);
            Assert.AreEqual(RENAMED_NAME, this.copy.Name, "Favorite wasnt properly renamed");
        }

        [TestMethod]
        public void NoDuplicitDoesntAskUser()
        {
            bool asked = false;
            var customCommand = new UpdateFavoriteWithRenameCommand(this.Persistence, newName =>
                {
                   asked = true;
                   return true;
                });
            customCommand.UpdateFavoritePreservingDuplicitNames(COPY_NAME, RENAMED_NAME, this.copy);
            Assert.IsFalse(asked, "If there is no duplicit, user shouldnt be prompter.");
        }

        [TestMethod]
        public void OverwriteProperlyRemovesDuplicit()
        {
            this.command.UpdateFavoritePreservingDuplicitNames(COPY_NAME, ORIGINAL_NAME, this.copy);
            int favoritesCount = this.Persistence.Favorites.Count();
            Assert.AreEqual(1, favoritesCount, "overwritten favorite should be removed from persistence.");
        }

        [TestMethod]
        public void RejectedRenameDoesntChangeAnything()
        {
            var customCommand = new UpdateFavoriteWithRenameCommand(this.Persistence, newName => false);
            customCommand.UpdateFavoritePreservingDuplicitNames(COPY_NAME, ORIGINAL_NAME, this.copy);
            Assert.AreEqual(COPY_NAME, this.copy.Name, "Favorite cant be changed, if user refused the rename.");
        }
    }
}
