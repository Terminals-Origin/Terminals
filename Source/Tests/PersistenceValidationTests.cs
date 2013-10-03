using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.Validation;
using Tests.FilePersisted;

namespace Tests
{
    /// <summary>
    /// Validate data using persistence to check uniquenes of the names
    /// </summary>
    [TestClass]
    public class PersistenceValidationTests : FilePersistedTestLab
    {
        private const string ERROR_MESSAGE = GroupValidator.NOT_UNIQUE;
        private const string ORIGINAL_NAME = "GroupA";

        private const string UNIQUE_NAME = "Unique";

        private GroupValidator validator;

        private IGroup originalGroup;

        [TestInitialize]
        public void InitializeValidator()
        {
            this.validator  = new GroupValidator(this.Persistence);
            this.originalGroup = this.AddNewGroup(ORIGINAL_NAME);
        }

        [TestMethod]
        public void ValidateNotRenamedGroup()
        {
            string resultMessage = this.validator.ValidateCurrent(this.originalGroup, ORIGINAL_NAME);
            Assert.AreEqual(string.Empty, resultMessage, "Group name has to be valid, if not changed.");
        }

        [TestMethod]
        public void ValidateRenamedGroup()
        {
            string resultMessage = this.validator.ValidateCurrent(this.originalGroup, UNIQUE_NAME);
            const string MESSAGE = "Group name has to be valid, if persistence doesnt contain group with validated name.";
            Assert.AreEqual(string.Empty, resultMessage, MESSAGE);
        }

        [TestMethod]
        public void ValidateRenamedDuplicitGroup()
        {
            this.AddNewGroup(UNIQUE_NAME);
            string resultMessage = this.validator.ValidateCurrent(this.originalGroup, UNIQUE_NAME);
            const string MESSAGE = "Group name cant be valid, there already is another with new name.";
            Assert.AreEqual(ERROR_MESSAGE, resultMessage, MESSAGE);
        }

        [TestMethod]
        public void ValidateNewUniqueGroupName()
        {
            string resultMessage = this.validator.ValidateNew(UNIQUE_NAME);
            const string MESSAGE = "New Group name has to be valid, if persistence doesnt contain group with validated name.";
            Assert.AreEqual(string.Empty, resultMessage, MESSAGE);
        }

        [TestMethod]
        public void ValidateNotUniqueNewGroupName()
        {
            string resultMessage = this.validator.ValidateNew(ORIGINAL_NAME);
            const string MESSAGE = "New Group name is already present in persistence.";
            Assert.AreEqual(ERROR_MESSAGE, resultMessage, MESSAGE);
        }
    }
}
