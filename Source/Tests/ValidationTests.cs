using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void DbFavoriteValidationTest()
        {
            var favorite = new DbFavorite();
            favorite.Protocol = "12345678901";
            favorite.ServerName = longText;
            favorite.Name = longText;
            favorite.Notes = longText;

            favorite.ExecuteBeforeConnect = new DbBeforeConnectExecute();
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