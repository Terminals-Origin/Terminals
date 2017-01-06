using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data.DB;
using Terminals.Data.Validation;
using Tests.Connections;

namespace Tests.Validation
{
    [TestClass]
    public class DbValidationTests : ValidationTests
    {
        private readonly DbValidations validator = new DbValidations(TestConnectionManager.Instance);

        [TestMethod]
        public void LongDbGroupName_ValidateNameOnly_ReturnsNameError()
        {
            var group = new DbGroup();
            group.Name = LongText;
            AssertNameOnlyValidation(this.validator, group);
        }

        [TestMethod]
        public void DbCredential_Validate_ReturnsError()
        {
            AssertCredentialsValidation(this.validator, new DbCredentialSet(), 2);
        }

        [TestMethod]
        public void EmptyDbGroupName_ValidateGroup_ReturnsError()
        {
            AssertGroupValidation(this.validator, new DbGroup());
        }

        [TestMethod]
        public void InvalidDbFavorite_Validatie_ReturnsErrorsForAllProperties()
        {
            ValidationStates results = ValidateDbFavorite();
            Assert.AreEqual(9, results.Count(), "Some properties arent validated properly for DbFavorite");
        }

        [TestMethod]
        public void DbFavorite_Validate_ReturnsAllServerNameErrors()
        {
            ValidationStates results = ValidateDbFavorite();
            var serverNameErrors = results.Count(result => result.PropertyName == "ServerName");
            Assert.AreEqual(2, serverNameErrors, "DbFavorite ServerName wasnt validated properly");
        }

        [TestMethod]
        public void DbFavorite_Validate_ReturnsAllProtocolErrors()
        {
            ValidationStates results = ValidateDbFavorite();
            var protocolErrors = results.Count(result => result.PropertyName == "Protocol");
            Assert.AreEqual(2, protocolErrors, "DbFavorite protocol wasnt validated properly");
        }

        private ValidationStates ValidateDbFavorite()
        {
            // created dbfavorite is not compleate, only necessary to make validable using IFavorite
            var favorite = new DbFavorite();
            favorite.ExecuteBeforeConnect = new DbBeforeConnectExecute();
            favorite.Security = new DbSecurityOptions();
            favorite.Details.LoadFieldsFromReferences();

            favorite.Protocol = LongText.Substring(0, 11);
            favorite.ServerName = LongText;
            favorite.Name = LongText;
            favorite.Notes = LongText;

            favorite.ExecuteBeforeConnect.Command = LongText;
            favorite.ExecuteBeforeConnect.CommandArguments = LongText;
            favorite.ExecuteBeforeConnect.InitialDirectory = LongText;

            return validator.Validate(favorite);
        }

    }
}