using Terminals.Connections.ICA;

namespace Terminals.Integration.Export
{
    internal class TerminalsIcaExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol == ICAConnectionPlugin.ICA_CITRIX)
            {
                context.WriteElementString("iCAApplicationName", context.Favorite.ICAApplicationName);
                context.WriteElementString("iCAApplicationPath", context.Favorite.ICAApplicationPath);
                context.WriteElementString("iCAApplicationWorkingFolder", context.Favorite.ICAApplicationWorkingFolder);
                context.WriteElementString("icaServerINI", context.Favorite.IcaServerINI);
                context.WriteElementString("icaClientINI", context.Favorite.IcaClientINI);
                context.WriteElementString("icaEnableEncryption", context.Favorite.IcaEnableEncryption.ToString());
                context.WriteElementString("icaEncryptionLevel", context.Favorite.IcaEncryptionLevel);
            }
        }
    }
}