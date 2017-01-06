using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Terminal
{
    internal class ConsoleOptionsConverter : OptionsConverterTemplate<ConsoleOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, ConsoleOptions options)
        {
            FromConfigFavorite(options, source);
        }

        internal static void FromConfigFavorite(ConsoleOptions options, FavoriteConfigurationElement source)
        {
            options.BackColor = source.ConsoleBackColor;
            options.TextColor = source.ConsoleTextColor;
            options.CursorColor = source.ConsoleCursorColor;
            options.Columns = source.ConsoleCols;
            options.Rows = source.ConsoleRows;
            options.Font = source.ConsoleFont;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, ConsoleOptions options)
        {
            ToConfigFavorite(options, destination);
        }

        internal static void ToConfigFavorite(ConsoleOptions options, FavoriteConfigurationElement destination)
        {
            destination.ConsoleBackColor = options.BackColor;
            destination.ConsoleTextColor = options.TextColor;
            destination.ConsoleCursorColor = options.CursorColor;
            destination.ConsoleCols = options.Columns;
            destination.ConsoleRows = options.Rows;
            destination.ConsoleFont = options.Font;
        }
    }
}