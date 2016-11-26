using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Tests.FilePersisted;

namespace Tests.Commands
{
    [TestClass]
    public class FavoriteRenameCommandTests : FilePersistedTestLab
    {
        private const string ORIGINAL_NAME = "Original";
        private const string COPY_NAME = "Copy";

        private const string RENAMED_NAME = "Renamed";

        private IFavorite copy;

        private FavoriteRenameCommand command;

        [TestInitialize]
        public void InitializeFavorites()
        {
            this.AddFavorite(ORIGINAL_NAME);
            this.copy = this.AddFavorite(COPY_NAME);
            // command is set to rename by default
            var service = new TestRenameService(this.Persistence.Favorites, newName => true);
            this.command = new FavoriteRenameCommand(this.Persistence, service);
        }
        
        [TestMethod]
        public void NoDuplicitProperlyRenames()
        {
            this.command.ApplyRename(this.copy, RENAMED_NAME);
            Assert.AreEqual(RENAMED_NAME, this.copy.Name, "Favorite wasnt properly renamed");
        }

        [TestMethod]
        public void NoDuplicitDoesntAskUser()
        {
            bool asked = false;
            var service = new TestRenameService(this.Persistence.Favorites, newName =>
                {
                   asked = true;
                   return true;
                });
            var customCommand = new FavoriteRenameCommand(this.Persistence, service);
            customCommand.ApplyRename(this.copy, RENAMED_NAME);
            Assert.IsFalse(asked, "If there is no duplicit, user shouldnt be prompter.");
        }

        [TestMethod]
        public void OverwriteProperlyRemovesDuplicit()
        {
            this.command.ApplyRename(this.copy, ORIGINAL_NAME);
            int favoritesCount = this.Persistence.Favorites.Count();
            Assert.AreEqual(1, favoritesCount, "overwritten favorite should be removed from persistence.");
        }

        [TestMethod]
        public void RejectedRenameDoesntChangeAnything()
        {
            var service = new TestRenameService(this.Persistence.Favorites, newName => false);
            var customCommand = new FavoriteRenameCommand(this.Persistence, service);
            bool performAction = customCommand.ValidateNewName(this.copy, ORIGINAL_NAME);
            Assert.IsFalse(performAction, "Favorite cant be changed, if user refused the rename.");
        }
    }
}
