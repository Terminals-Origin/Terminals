using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Connections.Terminal;
using Terminals.Data;
using Tests.FilePersisted;

namespace Tests
{
    /// <summary>
    /// Till the Settings is static class we have not chance to isolate the other unit test.
    /// So we have to reset all settings possibly conflicting with other unit tests.
    /// Update passwords, are tested as part of Update config file.
    /// Tags (Groups) and Favorites are obsolete and will be removed, so we dont test them here.
    /// </summary>
    [TestClass]
    public class ConfigurationSettingsTests
    {
        private static readonly Guid favoriteA = new Guid("eb45aefd-138f-49ca-9c9e-ce5d5877ca62");
        private static readonly Guid favoriteB = new Guid("8dda2ccb-cfb9-4487-bab5-0da5319e1485");

        private readonly Settings settings = Settings.Instance;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            FilePersistedTestLab.SetDefaultFileLocations();
        }

        [TestInitialize]
        public void Setup()
        {
            // Reset previous test run changes in config fie
            settings.SaveDefaultConfigFile();
            settings.ForceReload();
        }

        [TestMethod]
        public void Ssh_SaveDefaultFavorite_IsSaved()
        {
            var favorite = new FavoriteConfigurationElement();
            favorite.Protocol = SshConnectionPlugin.SSH; // avoid all default values
            settings.SaveDefaultFavorite(favorite);
            settings.ForceReload();
            var loaded = settings.GetDefaultFavorite();
            const string MESSAGE = "Newly saved default favorite has return last saved value.";
            Assert.AreEqual(SshConnectionPlugin.SSH, loaded.Protocol, MESSAGE);
        }

        [TestMethod]
        public void NewMRU_AddUserMRu_IsSaved()
        {
            const string EXPECTED_USER_MRU = "ExpectedUserMRu";
            settings.AddUserMRUItem(EXPECTED_USER_MRU);
            settings.ForceReload();
            var firstUserMru = settings.MRUUserNames[0];
            Assert.AreEqual(EXPECTED_USER_MRU, firstUserMru, "Adding user name to MRU, has to return last saved value.");
        }

        [TestMethod]
        public void FileWatch_FileChanged_FiresConfigurationChanged()
        {
            var originalWatch = settings.FileWatch;
            var mockFileWatch = new Mock<IDataFileWatcher>(MockBehavior.Strict);
            settings.FileWatch = mockFileWatch.Object;
            bool eventReceived = false;
            settings.ConfigurationChanged += args => eventReceived = true;
            mockFileWatch.Raise(watch => watch.FileChanged += null, EventArgs.Empty);
            Assert.IsTrue(eventReceived, "When receiving file changed event, Settings have to fire configuration changed");
            settings.FileWatch = originalWatch;
        }

        [TestMethod]
        public void DumyControl_AssignSynchronizer_AssignesToFileWatch()
        {
            var originalWatch = settings.FileWatch;
            var expectedSynchronizer = new Control();
            var mockFileWatch = new Mock<IDataFileWatcher>(MockBehavior.Strict);
            mockFileWatch.Setup(watch => watch.AssignSynchronizer(expectedSynchronizer))
                         .Verifiable("The assigned synchronized wasnt delivered to the file watching service");
            settings.FileWatch = mockFileWatch.Object;
            settings.AssignSynchronizationObject(expectedSynchronizer);
            mockFileWatch.VerifyAll();
            settings.FileWatch = originalWatch;
        }

        [TestMethod]
        public void DefaultSettings_Forms_DoesntReturnNull()
        {
            Assert.IsNotNull(settings.Forms, "The forms configuration always has to be accessible");
        }

        [TestMethod]
        public void EmptyCaptureRoot_SetGet_ReturnsDefaultRoot()
        {
            settings.CaptureRoot = string.Empty;
            const string MESSAGE = "Undefined capture root has to return default location";
            Assert.AreEqual(FileLocations.DefaultCaptureRootDirectory, settings.CaptureRoot, MESSAGE);
        }

        [TestMethod]
        public void ProtocolDefaultSortProperty_SetGet_IsSaved()
        {
            settings.DefaultSortProperty = SortProperties.Protocol;
            const string MESSAGE = "Get DefaultSortProperty has to return last saved value";
            Assert.AreEqual(SortProperties.Protocol, settings.DefaultSortProperty, MESSAGE);
            settings.DefaultSortProperty = SortProperties.ConnectionName;
        }

        [TestMethod]
        public void Vesion2_SetGet_IsSaved()
        {
            var expectedVersion = new Version(2, 0);
            settings.ConfigVersion = expectedVersion;
            Assert.AreEqual(expectedVersion, settings.ConfigVersion, "Get config file version has to return last saved value");
        }

        [TestMethod]
        public void TwoConnections_CreateSavedConnectionsList_AreSaved()
        {
            const string CONNECTION_B = "CONNECTION_B";
            var connecitonNames = new string[] {"FavoriteA", CONNECTION_B};
            settings.CreateSavedConnectionsList(connecitonNames);
            var saved = settings.SavedConnections;
            Assert.AreEqual(CONNECTION_B, saved[1], "Saved connections should correspond with last saved connections list.");
        }

        [TestMethod]
        public void FavoriteB_EditFavoriteButton_IsSaved()
        {
            settings.UpdateFavoritesToolbarButtons(new List<Guid>());
            settings.EditFavoriteButton(favoriteA, favoriteB, true);
            var savedButtons = settings.FavoritesToolbarButtons;
            Assert.AreEqual(favoriteB, savedButtons[0], "Loaded buttons should contain last saved buttons.");
        }

        [TestMethod]
        public void TwoFavorites_UpdateButtons_FiresSettingsChanged()
        {
            var favorites = new List<Guid>() { favoriteA, favoriteB };
            bool changeReported = false;
            settings.ConfigurationChanged += args => changeReported = true;
            settings.UpdateFavoritesToolbarButtons(favorites);
            Assert.IsTrue(changeReported, "Changing buttons has to fire event to report the imediate change");
        }

        [TestMethod]
        public void SavedValue_ForceReload_IsLoaded()
        {
            this.AssertAskToReconnectReloaded(false, "Direct assignment of property saves the property to file");
        }

        [TestMethod]
        public void DelayedSave_ForceReload_LosesValue()
        {
            settings.StartDelayedUpdate();
            const string MESSAGE = "When delayed save was called, directly changing settings doesnt force save.";
            this.AssertAskToReconnectReloaded(true, MESSAGE);
            settings.SaveAndFinishDelayedUpdate();
        }

        private void AssertAskToReconnectReloaded(bool expectedValue, string message)
        {
            settings.AskToReconnect = false; // because default is true
            settings.ForceReload();
            Assert.AreEqual(expectedValue, settings.AskToReconnect, message);
            settings.AskToReconnect = true;
        }
    }
}
