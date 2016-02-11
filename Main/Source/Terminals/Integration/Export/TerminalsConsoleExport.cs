namespace Terminals.Integration.Export
{
    internal class TerminalsConsoleExport
    {
        public static void ExportConsoleOptions(IExportOptionsContext context, FavoriteConfigurationElement favorite)
        {
            context.WriteElementString("consolerows", favorite.ConsoleRows.ToString());
            context.WriteElementString("consolecols", favorite.ConsoleCols.ToString());
            context.WriteElementString("consolefont", favorite.ConsoleFont);
            context.WriteElementString("consolebackcolor", favorite.ConsoleBackColor);
            context.WriteElementString("consoletextcolor", favorite.ConsoleTextColor);
            context.WriteElementString("consolecursorcolor", favorite.ConsoleCursorColor);
        }
    }
}