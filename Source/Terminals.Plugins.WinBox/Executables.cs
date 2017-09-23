using System.IO;
using System.Reflection;

namespace Terminals.Plugins.WinBox
{
    internal class Executables
    {
        private const string RESOURCES = "Resources";
        internal const string WINBOX_BINARY = "winbox.exe";

        internal static string GetWinBoxBinaryPath()
        {
            return GetBinaryPath(WINBOX_BINARY);
        }

        private static string GetBinaryPath(string assembly)
        {
            string baseLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(baseLocation, RESOURCES, assembly);
        }
    }
}
