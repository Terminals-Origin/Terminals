using Terminals.Integration.Export;

namespace Terminals.Plugins.WinBox
{
    internal class TerminalsWinBoxExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol == WinBoxConnectionPlugin.WinBox)
            {
                context.WriteElementString("WinBoxSessionName", context.Favorite.WinBoxSessionName);
                context.WriteElementString("WinBoxVerbose", context.Favorite.WinBoxVerbose.ToString());
                context.WriteElementString("WinBoxEnablePagentAuthentication", context.Favorite.WinBoxEnablePagentAuthentication.ToString());
                context.WriteElementString("WinBoxEnablePagentForwarding", context.Favorite.WinBoxEnablePagentForwarding.ToString());
                context.WriteElementString("WinBoxX11Forwarding", context.Favorite.WinBoxX11Forwarding.ToString());
                context.WriteElementString("WinBoxEnableCompression", context.Favorite.WinBoxEnableCompression.ToString());
                context.WriteElementString("WinBoxVersion", context.Favorite.WinBoxVersion.ToString());
            }
        }
    }
}
