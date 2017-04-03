using Terminals.Integration.Export;

namespace Terminals.Plugins.Putty
{
    internal class TerminalsTelnetExport : ITerminalsOptionsExport
    {

        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol == TelnetConnectionPlugin.TELNET)
            {
                context.WriteElementString("telnetSessionName", context.Favorite.TelnetSessionName);
                context.WriteElementString("telnetVerbose", context.Favorite.TelnetVerbose.ToString());
            }
        }
    }
}
