using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.Validation;
using Tests.FilePersisted;

namespace Tests.Validation
{
    /// <summary>
    /// Validate data using persistence to check uniquenes of the Group names
    /// </summary>
    [TestClass]
    public class GroupNameValidatorTests : FilePersistedTestLab
    {
        private NameValidatorTestLab<IGroup> testLab;

        [TestInitialize]
        public void InitializeValidator()
        {
            var originalGroup = this.AddNewGroup(NameValidatorTestLab<IGroup>.ORIGINAL_NAME);
            var validator = new GroupNameValidator(this.Persistence);
            this.testLab = new NameValidatorTestLab<IGroup>(validator, originalGroup, GroupNameValidator.NOT_UNIQUE);
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
            this.AddNewGroup(NameValidatorTestLab<IGroup>.UNIQUE_NAME);
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
