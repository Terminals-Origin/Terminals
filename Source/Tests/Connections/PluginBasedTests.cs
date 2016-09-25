using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Connections
{
    /// <summary>
    /// Some tests need Deployed plugins in tests directory.
    /// Because of dynamic loading of the plugins, it is necessary to compare by Type name, because types doenst equals.
    /// This is necessary, because for testing we cant inject the plugins into Connection manager.
    /// See Serializer and its connectionmanager field usage, see also setter of Favorite Protocol property.
    /// </summary>
    [DeploymentItem(VNC_PLUGIN, VNC_TARGET)]
    [DeploymentItem(ICA_PLUGIN, ICA_TARGET)]
    [TestClass]
    public abstract class PluginBasedTests
    {
        internal const string VNC_PLUGIN = "Terminals.Plugins.Vnc.dll";

        internal const string VNC_TARGET = @"Plugins\Vnc";

        internal const string ICA_PLUGIN = "Terminals.Plugins.Ica.dll";

        internal const string ICA_TARGET = @"Plugins\Ica";
    }
}