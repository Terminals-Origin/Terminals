using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;

namespace Tests.UserInterface
{
    /// <summary>
    /// We dont care about the settings file location by default, because it points to the assembly directory
    /// </summary>
    [TestClass]
    public class ToolStripSettingsTest
    {
        private const int IRELEVANT_KEY = 1;
        private const int IRELEVANT_LEFT = 40;

        [TestMethod]
        public void NotExistingFile_Load_ReturnsNull()
        {
            ToolStripSettings loaded = ToolStripSettings.Load();
            Assert.IsNull(loaded, "We do nothing, when settings are loaded as null.");
        }

        [ExpectedException(typeof(UnauthorizedAccessException))]
        [TestMethod]
        public void ReadOnlyFile_Save_ThrowsAnException()
        {
            try
            {
                AssertReadOnlyFileSave();
            }
            finally // test specific teardown
            {
                SetSettingsFileAttributes(FileLocations.ToolStripsFullFileName, FileAttributes.Archive);
            }
        }

        private static void AssertReadOnlyFileSave()
        {
            SetUpReadOnlySettingsFile();
            // content is not relevant here
            var newSettings = new ToolStripSettings();
            newSettings.Save();
            Assert.Fail("Save of read only file has to fail");
        }

        private static void SetUpReadOnlySettingsFile()
        {
            string settingsFile = FileLocations.ToolStripsFullFileName;
            File.WriteAllText(settingsFile, string.Empty);
            SetSettingsFileAttributes(settingsFile, FileAttributes.ReadOnly);
        }

        private static void SetSettingsFileAttributes(string settingsFile, FileAttributes attributes)
        {
            var fileInfo = new FileInfo(settingsFile);
            fileInfo.Attributes = attributes;
        }

        /// <summary>
        /// Simple roundtrip test of settings reload
        /// </summary>
        [TestMethod]
        public void PanelLocation_LoadFromSaved_ReadsSavedLocation()
        {
            SaveSampleSettings();
            var loaded = ToolStripSettings.Load();
            int loadedLeft = loaded[IRELEVANT_KEY].Left;
            Assert.AreEqual(IRELEVANT_LEFT, loadedLeft, "We need to be able read previously saved settings");
        }

        private static void SaveSampleSettings()
        {
            var newSettings = new ToolStripSettings();
            newSettings.Add(IRELEVANT_KEY, new ToolStripSetting() {Left = IRELEVANT_LEFT});
            newSettings.Save();
        }
    }
}
