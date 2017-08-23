using System.IO;
using System.Reflection;
using Terminals.Common.Forms;

namespace Terminals.Plugins.Putty
{
    internal class Executables
    {
        private const string RESOURCES = "Resources"; 
        internal const string PUTTY_BINARY = "putty.exe";
        internal const string PAGEANT_BINARY = "pageant.exe";

        internal static void LaunchPutty()
        {
            var puttyBinaryPath = GetPuttyBinaryPath();
            ExternalLinks.OpenPath(puttyBinaryPath);
        }

        internal static void LaunchPageant()
        {
            var puttyBinaryPath = GetBinaryPath(PAGEANT_BINARY);
            // todo launch only, if pageant isnt running otherwise it shows a message in case the key file doesnt exist.
            ExternalLinks.OpenPath(puttyBinaryPath);
        }

        internal static string GetPuttyBinaryPath()
        {
            return GetBinaryPath(PUTTY_BINARY);
        }

        private static string GetBinaryPath(string assembly)
        {
            string baseLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(baseLocation, RESOURCES, assembly);
        }
    }
}