using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data.DB;
using Terminals.Data.Validation;
using Tests.FilePersisted;

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
            var results = Validations.ValidateFavorite(favorite);

            Assert.AreEqual(results.Count, 9, "Some properties arent validated properly for DbFavorite");
            var serverNameErrors = results.Count(result => result.PropertyName == "ServerName");
            Assert.AreEqual(serverNameErrors, 2, "DbFavorite ServerName wasnt validated properly");
            var protocolErrors = results.Count(result => result.PropertyName == "Protocol");
            Assert.AreEqual(protocolErrors, 2, "DbFavorite ServerName wasnt validated properly");
        }
    }
}