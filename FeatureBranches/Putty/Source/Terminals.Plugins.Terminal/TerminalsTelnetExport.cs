using Terminals.Connections.Terminal;

namespace Terminals.Integration.Export
{
    internal class TerminalsTelnetExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol == TelnetConnectionPlugin.TELNET)
            {
                TerminalsConsoleExport.ExportConsoleOptions(context, context.Favorite);

                context.WriteElementString("telnet", context.Favorite.Telnet.ToString());
                context.WriteElementString("telnetRows", context.Favorite.TelnetRows.ToString());
                context.WriteElementString("telnetCols", context.Favorite.TelnetCols.ToString());
                context.WriteElementString("telnetFont", context.Favorite.TelnetFont);
                context.WriteElementString("telnetBackColor", context.Favorite.TelnetBackColor);
                context.WriteElementString("telnetTextColor", context.Favorite.TelnetTextColor);
                context.WriteElementString("telnetCursorColor", context.Favorite.TelnetCursorColor);
            }
        }
    }
}