using System.Xml;
using Terminals.Connections;

namespace Terminals.Integration.Export
{
    internal class TerminalsSshExport : ITerminalsOptionsExport
    {
        public void ExportOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            if (favorite.Protocol == ConnectionManager.SSH)
            {
                TerminalsConsoleExport.ExportConsoleOptions(w, favorite);

                w.WriteElementString("ssh1", favorite.SSH1.ToString());
                w.WriteElementString("authMethod", favorite.AuthMethod.ToString());
                w.WriteElementString("keyTag", favorite.KeyTag);
                w.WriteElementString("SSHKeyFile", favorite.SSHKeyFile);
            }
        }
    }
}