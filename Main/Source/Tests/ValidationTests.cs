using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.DB;
using Terminals.Data.Validation;

namespace Tests
{
    [TestClass]
    public class ValidationTests
    {
        private static string longText;

        [ClassInitialize]
        public static void InitializeLongText(TestContext context)
        {
            StringBuilder longTextBuilder = new StringBuilder();
            for (int index = 0; index < 500; index++)
            {
                longTextBuilder.Append(index.ToString());
            }

            longText = longTextBuilder.ToString();
        }

        [TestMethod]
        public void FavoriteValidationTest()
        {
            var favorite = new Favorite();
            favorite.Protocol = longText.Substring(0, 11);
            favorite.ServerName = longText;
            var results = Validations.Validate(favorite);
            Assert.AreEqual(results.Count, 3, "Some properties arent validated properly for Favorite");
        }

        [TestMethod]
        public void DbFavoriteValidationTest()
        {
            // created dbfavorite is not compleate, only necessary to make validable using IFavorite
            var favorite = new DbFavorite();
            favorite.ExecuteBeforeConnect = new DbBeforeConnectExecute();
            favorite.Security = new DbSecurityOptions();
            favorite.Details.LoadFieldsFromReferences();
            
            favorite.Protocol = longText.Substring(0, 11);
            favorite.ServerName = longText;
            favorite.Name = longText;
            favorite.Notes = longText;
            
            favorite.ExecuteBeforeConnect.Command = longText;
            favorite.ExecuteBeforeConnect.CommandArguments = longText;
            favorite.ExecuteBeforeConnect.InitialDirectory = longText;
            var results = Validations.Validate(favorite);

            Assert.AreEqual(results.Count, 9, "Some properties arent validated properly for DbFavorite");
            var serverNameErrors = results.Count(result => result.PropertyName == "ServerName");
            Assert.AreEqual(serverNameErrors, 2, "DbFavorite ServerName wasnt validated properly");
            var protocolErrors = results.Count(result => result.PropertyName == "Protocol");
            Assert.AreEqual(protocolErrors, 2, "DbFavorite protocol wasnt validated properly");
        }

        [TestMethod]
        public void DbGroupValidationTest()
        {
            var group = new DbGroup();
            group.Name = longText;
            var results = Validations.Validate(group);
            Assert.AreEqual(results.Count, 1, "Group name validation failed");
        }

        [TestMethod]
        public void DbCredentialsetValidationTest()
        {
            var credentailSet = new DbCredentialSet();
            credentailSet.Name = longText;
            var results = Validations.Validate(credentailSet);
            Assert.AreEqual(results.Count, 1, "CredentailSet name validation failed");
        }
    }
}