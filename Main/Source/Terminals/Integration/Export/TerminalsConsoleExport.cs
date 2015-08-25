using System.Xml;

namespace Terminals.Integration.Export
{
    internal class TerminalsConsoleExport
    {
        public static void ExportConsoleOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            w.WriteElementString("consolerows", favorite.ConsoleRows.ToString());
            w.WriteElementString("consolecols", favorite.ConsoleCols.ToString());
            w.WriteElementString("consolefont", favorite.ConsoleFont);
            w.WriteElementString("consolebackcolor", favorite.ConsoleBackColor);
            w.WriteElementString("consoletextcolor", favorite.ConsoleTextColor);
            w.WriteElementString("consolecursorcolor", favorite.ConsoleCursorColor);
        }
    }
}