using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;
using Terminals.Data;

namespace Tests.FilePersisted
{
    [TestClass]
    public class FavoritesTest : FilePersistedTestLab
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Notes property can store special characters, so it is Base64 encoded.
        /// </summary>
        [TestMethod]
        public void SaveLoadNotesTest()
        {
            IFavorite favorite = this.AddFavorite();
            const string SPECIAL_CHARACTERS = "čočka\r\nčočka"; // some example special characters
            favorite.Notes = SPECIAL_CHARACTERS;
            this.Persistence.Favorites.Update(favorite);
            var secondary = new FilePersistence();
            secondary.Initialize();
            IFavorite checkfavorite = secondary.Favorites.FirstOrDefault();
            Assert.AreEqual(SPECIAL_CHARACTERS, checkfavorite.Notes, "favorite notes were not saved properly");
        }

        /// <summary>
        /// Checks, if we are still able to manipulate passwords after protocol update.
        /// This is a special case for RdpOptions, which need persistence to handle Gateway credentials
        /// </summary>
        [TestMethod]
        public void UpdateProtocolTest()
        {
            IFavorite favorite = this.AddFavorite();
            // now it has RdpOptions
            favorite.Protocol = ConnectionManager.VNC;
            this.Persistence.Favorites.Update(favorite);
            AssertRdpSecurity(favorite);
        }

        /// <summary>
        /// Checks, if Gateway has still assigned persistence security, to be able work with passwords.
        /// </summary>
        internal static void AssertRdpSecurity(IFavorite favorite)
        {
            favorite.Protocol = ConnectionManager.RDP;
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            // next line shouldn't fail
            rdpOptions.TsGateway.Security.Password = "aaa";
        }
    }
}
