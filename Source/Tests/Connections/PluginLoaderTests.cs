using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;

namespace Tests.Connections
{
    [TestClass]
    public class PluginLoaderTests : PluginBasedTests
    {
        private static readonly Tuple<string, string>[] expectedPlugins = new Tuple<string, string>[]
            {
                new Tuple<string, string>("VNC", "Terminals.Plugins.Vnc"),
                new Tuple<string, string>("ICA", "Terminals.Plugins.Ica"),
                new Tuple<string, string>("VMRC", "Terminals.Plugins.Vmrc"),
                new Tuple<string, string>("Telnet and Ssh", "Terminals.Plugins.Terminal"),
                new Tuple<string, string>("HTTP and HTTPS", "Terminals.Plugins.Web"),
                // todo add RDP plugin to test the loader
            };

        [TestMethod]
        public void FindAvailablePlugins_LoadsAllPlugins()
        {
            var loader = new PluginsLoader();
            IEnumerable<PluginDefinition> loaded = loader.FindAvailablePlugins();
            bool allValid = expectedPlugins.All(e => this.AssertLoadedPlugins(e, loaded));
            Assert.IsTrue(allValid, "All plugins have to be listed by plugin properties in options dialog.");
        }

        private bool AssertLoadedPlugins(Tuple<string, string> expected, IEnumerable<PluginDefinition> loadedDefinition)
        {
            string message = string.Format("Expecting '{0}'", expected.Item1);
            Console.WriteLine(message);
            PluginDefinition definition = loadedDefinition.FirstOrDefault(d => d.Description == expected.Item1);
            return definition != null && definition.FullPath.Contains(expected.Item2);
        }
    }
}
