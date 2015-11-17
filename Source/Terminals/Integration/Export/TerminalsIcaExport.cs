using System.Xml;
using Terminals.Connections;
using Terminals.Connections.ICA;

namespace Terminals.Integration.Export
{
    internal class TerminalsIcaExport : ITerminalsOptionsExport
    {
        public void ExportOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            if (favorite.Protocol == ICAConnectionPlugin.ICA_CITRIX)
            {
                w.WriteElementString("iCAApplicationName", favorite.ICAApplicationName);
                w.WriteElementString("iCAApplicationPath", favorite.ICAApplicationPath);
                w.WriteElementString("iCAApplicationWorkingFolder", favorite.ICAApplicationWorkingFolder);
                w.WriteElementString("icaServerINI", favorite.IcaServerINI);
                w.WriteElementString("icaClientINI", favorite.IcaClientINI);
                w.WriteElementString("icaEnableEncryption", favorite.IcaEnableEncryption.ToString());
                w.WriteElementString("icaEncryptionLevel", favorite.IcaEncryptionLevel);
            }
        }
    }
}