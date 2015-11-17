using System.Xml;
using Terminals.Connections;
using Terminals.Connections.Terminal;

namespace Terminals.Integration.Export
{
    internal class TerminalsTelnetExport : ITerminalsOptionsExport
    {
        public void ExportOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            if (favorite.Protocol == TelnetConnectionPlugin.TELNET)
            {
                TerminalsConsoleExport.ExportConsoleOptions(w, favorite);

                w.WriteElementString("telnet", favorite.Telnet.ToString());
                w.WriteElementString("telnetRows", favorite.TelnetRows.ToString());
                w.WriteElementString("telnetCols", favorite.TelnetCols.ToString());
                w.WriteElementString("telnetFont", favorite.TelnetFont);
                w.WriteElementString("telnetBackColor", favorite.TelnetBackColor);
                w.WriteElementString("telnetTextColor", favorite.TelnetTextColor);
                w.WriteElementString("telnetCursorColor", favorite.TelnetCursorColor);
            }
        }
    }
}