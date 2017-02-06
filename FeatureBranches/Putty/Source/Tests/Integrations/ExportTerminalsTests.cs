using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Common.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Data;
using Terminals.Integration.Export;
using Terminals.Integration.Import;
using Tests.Connections;
using Tests.UserInterface;
using Terminals.Plugins.Putty;

namespace Tests.Integrations
{
    /// <summary>
    /// Needs persistence, because of historical reasons the FavoriteConfigurationElement
    /// resolves security using persistence
    /// </summary>
    [TestClass]
    public class ExportTerminalsTests
    {
        private const string TEST_FILE = "Exported.xml";

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ValidFavorite_Export_WritesProtocolOptions()
        {
            var testData = new[]
            {
                new Tuple<string, string>(KnownConnectionConstants.RDP, "sounds"),
                new Tuple<string, string>(KnownConnectionConstants.RDP, "enableSecuritySettings"),
                new Tuple<string, string>(KnownConnectionConstants.RDP, "tsgwUsageMethod"),
                new Tuple<string, string>(KnownConnectionConstants.RDP, "executeBeforeConnect"), // applies to all protocols
                new Tuple<string, string>(VncConnectionPlugin.VNC, "vncAutoScale"),
                new Tuple<string, string>(VmrcConnectionPlugin.VMRC, "vmrcadministratormode"),
                new Tuple<string, string>(TelnetConnectionPlugin.TELNET, "telnetSessionName"),
                new Tuple<string, string>(TelnetConnectionPlugin.TELNET, "telnetVerbose"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshSessionName"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshVerbose"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshEnablePagentAuthentication"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshEnablePagentForwarding"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshX11Forwarding"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshEnableCompression"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "sshVersion"),
                new Tuple<string, string>(ICAConnectionPlugin.ICA_CITRIX, "iCAApplicationName")
            };

            bool allValid = testData.All(this.AssertExportedFavoriteContent);
            Assert.IsTrue(allValid, "all protocols have to export only their options");
        }

        private bool AssertExportedFavoriteContent(Tuple<string, string> testCase)
        {
            var favoriteElement = new FavoriteConfigurationElement()
            {
                Protocol = testCase.Item1,
                EnableSecuritySettings = true,
                TsgwUsageMethod = 1, // anything else than default value
                ExecuteBeforeConnect = true
            };

            ExportFavorite(favoriteElement);
            return this.AssertAttribute(testCase);
        }

        private void ExportFavorite(FavoriteConfigurationElement favoriteElement)
        {
            ExportOptions options = new ExportOptions
            {
                ProviderFilter = ImportTerminals.TERMINALS_FILEEXTENSION,
                Favorites = new List<FavoriteConfigurationElement> {favoriteElement},
                FileName = TEST_FILE,
                IncludePasswords = true
            };

            IPersistence persistence = TestMocksFactory.CreatePersistence().Object;
            ITerminalsOptionsExport[] exporters = TestConnectionManager.Instance.GetTerminalsOptionsExporters();
            var exporter = new ExportTerminals(persistence, exporters);
            exporter.Export(options);
        }

        private bool AssertAttribute(Tuple<string, string> testCase)
        {
            string fileContent = System.IO.File.ReadAllText(TEST_FILE);
            bool contains = fileContent.Contains(testCase.Item2);
            this.ReportTestCase(testCase, contains);
            return contains;
        }

        private void ReportTestCase(Tuple<string, string> testCase, bool contains)
        {
            const string FORMAT = "Exported attribute '{0}' in '{1}' is present: {2}";
            string message = string.Format(FORMAT, testCase.Item2, testCase.Item1, contains);
            this.TestContext.WriteLine(message);
        }
    }
}
