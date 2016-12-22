using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.Interfaces;
using Terminals.Data.Validation;
using Tests.Connections;

namespace Tests.Validation
{
    [TestClass]
    internal class FileValidationTests : ValidationTests
    {
        private readonly IDataValidator validator = new FileValidations(TestConnectionManager.Instance);

        [TestMethod]
        public void Favorite_Validate_ReturnsErrors()
        {
            var favorite = new Favorite();
            favorite.Protocol = LongText.Substring(0, 11);
            favorite.ServerName = LongText;
            var results = this.validator.Validate(favorite);
            Assert.AreEqual(3, results.Count(), "Some properties arent validated properly for Favorite");
        }

        [TestMethod]
        public void EmptyGroupName_ValidateGroup_ReturnsError()
        {
            AssertGroupValidation(this.validator, new Group());
        }

        [TestMethod]
        public void Credential_Validate_ReturnsError()
        {
            AssertCredentialsValidation(this.validator, new CredentialSet(), 1);
        }


        [TestMethod]
        public void EmptyGroupName_ValidateNameOnly_ReturnsNameError()
        {
            var group = new Group();
            group.Name = string.Empty;
            AssertNameOnlyValidation(this.validator, group);
        }
    }
}