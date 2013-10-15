using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.Validation;
using Tests.FilePersisted;

namespace Tests.Validation
{
    /// <summary>
    /// Validate data using persistence to check uniquenes of the Favorite names
    /// </summary>
    [TestClass]
    public class FavoriteNameValidatorTests : FilePersistedTestLab
    {
        private NameValidatorTestLab<IFavorite> testLab;

        [TestInitialize]
        public void InitializeValidator()
        {
            var originalGroup = this.AddFavorite(NameValidatorTestLab<IFavorite>.ORIGINAL_NAME);
            var validator = new FavoriteNameValidator(this.Persistence);
            this.testLab = new NameValidatorTestLab<IFavorite>(validator, originalGroup, FavoriteNameValidator.NOT_UNIQUE);
        }

        [TestMethod]
        public void ValidateNotRenamed()
        {
            this.testLab.ValidateNotRenamed();
        }

        [TestMethod]
        public void ValidateRenamed()
        {
            this.testLab.ValidateRenamed();
        }

        [TestMethod]
        public void ValidateRenamedDuplicit()
        {
            this.AddFavorite(NameValidatorTestLab<IFavorite>.UNIQUE_NAME);
            this.testLab.ValidateRenamedDuplicit();
        }

        [TestMethod]
        public void ValidateNewUniqueName()
        {
            this.testLab.ValidateNewUniqueName();
        }

        [TestMethod]
        public void ValidateNotUniqueNewName()
        {
            this.testLab.ValidateNotUniqueNewName();
        }
    }
}