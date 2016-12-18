using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Configuration;
using Terminals.Connections;

namespace Tests.Connections
{
    [TestClass]
    public class PluginLoaderTests : PluginBasedTests
    {
        private const string NO_PLUGIN_ERRORMESSAGE = "If no plugin is enabled, factories cant provide default protocol.";

        private static readonly Tuple<string, string>[] expectedPlugins = new Tuple<string, string>[]
            {
                new Tuple<string, string>("VNC", "Terminals.Plugins.Vnc"),
                new Tuple<string, string>("ICA", "Terminals.Plugins.Ica"),
                new Tuple<string, string>("VMRC", "Terminals.Plugins.Vmrc"),
                new Tuple<string, string>("Telnet and Ssh", "Terminals.Plugins.Terminal"),
                new Tuple<string, string>("HTTP and HTTPS", "Terminals.Plugins.Web"),
                new Tuple<string, string>("RDP", "Terminals.Plugins.Rdp")
            };

        [ExpectedException(typeof(ApplicationException), NO_PLUGIN_ERRORMESSAGE)]
        [TestMethod]
        public void NoEnabledPlugins_Load_ThrowsApplicationException()
        {
            var disabledPlugins = this.CreateAllAvailablePlugins();
            var loader = CreateLoader(disabledPlugins);
            loader.Load();
        }

        [TestMethod]
        public void UnknownDisabledPlugin_Load_IgnoresWrongPath()
        {
            var disabled = new string[] { @"C:\PathTo\IncompatiblePluging.dll" };
            this.AssertAllPluginsAreLoaded(disabled);
        }

        [TestMethod]
        public void FindAvailablePlugins_LoadsAllPlugins()
        {
            this.AssertAllPluginsAreLoaded(new string[0]);
        }

        private void AssertAllPluginsAreLoaded(string[] disabledPlugins)
        {
            var loader = CreateLoader(disabledPlugins);
            IEnumerable<PluginDefinition> loaded = loader.FindAvailablePlugins();
            bool allValid = expectedPlugins.All(e => this.AssertLoadedPlugins(e, loaded));
            Assert.IsTrue(allValid, "All plugins have to be listed by plugin properties in options dialog.");
        }

        internal static PluginsLoader CreateLoader(string[] disabledPlugins)
        {
            var mockSettings = new Mock<IPluginSettings>();
            mockSettings.SetupGet(s => s.DisabledPlugins).Returns(disabledPlugins);
            return new PluginsLoader(mockSettings.Object);
        }

        private bool AssertLoadedPlugins(Tuple<string, string> expected, IEnumerable<PluginDefinition> loadedDefinition)
        {
            string message = string.Format("Loaded Plugins'{0}'", expected.Item1);
            Console.WriteLine(message);
            PluginDefinition definition = loadedDefinition.FirstOrDefault(d => d.Description == expected.Item1);
            return definition != null && definition.FullPath.Contains(expected.Item2);
        }
    }
}
