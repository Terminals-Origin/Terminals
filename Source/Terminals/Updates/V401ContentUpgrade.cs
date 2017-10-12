using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Updates
{
    /// <summary>
    /// User loses values previously stored in SshOptions and ConsoleOptions
    /// </summary>
    internal class V401ContentUpgrade
    {
        private static string PUTTYOPTIONS = "PuttyOptions";

        internal void Run(string favoritesFileFullFPath)
        {
            var doc = XDocument.Load(favoritesFileFullFPath);
            var favorites = doc.Root.FindByLocalName("Favorite");
            ReplaceObsoleteOptions(favorites);
            doc.Save(favoritesFileFullFPath);
        }

        private static void ReplaceObsoleteOptions(IEnumerable<XElement> favorites)
        {
            foreach (XElement favorite in favorites)
            {
                favorite.ReplaceByNewElement("SshOptions", PUTTYOPTIONS);
                favorite.ReplaceByNewElement("ConsoleOptions", PUTTYOPTIONS); //telnet
            }
        }
    }
}
