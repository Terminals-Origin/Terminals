using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Updates
{
    /// <summary>
    /// User loses values previously stored in SshOptions and ConsoleOptions
    /// </summary>
    internal class V401ContentUpgrade
    {
        private const string SERIALIZED_SSH_OPTIONS = @"
<SshOptions>
  <SessionName />
  <Verbose>false</Verbose>
  <EnablePagentAuthentication>false</EnablePagentAuthentication>
  <EnablePagentForwarding>false</EnablePagentForwarding>
  <X11Forwarding>false</X11Forwarding>
  <EnableCompression>false</EnableCompression>
  <SshVersion>SshNegotiate</SshVersion>
</SshOptions>";

        private const string SERIALIZED_TELENT_OPTIONS = @"
<TelnetOptions>
  <SessionName>devboxTelne</SessionName>
  <Verbose>false</Verbose>
</TelnetOptions>";

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
                favorite.ReplaceByNewElement("SshOptions", SERIALIZED_SSH_OPTIONS);
                favorite.ReplaceByNewElement("ConsoleOptions", SERIALIZED_TELENT_OPTIONS); //telnet
            }
        }
    }
}
