using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Terminal
{
    internal class ConsoleOptionsConverter : IOptionsConverter
    {
        public void FromCofigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as ConsoleOptions;
            if (options != null)
            {
                FavoriteConfigurationElement source = context.ConfigFavorite;
                options.BackColor = source.ConsoleBackColor;
                options.TextColor = source.ConsoleTextColor;
                options.CursorColor = source.ConsoleCursorColor;
                options.Columns = source.ConsoleCols;
                options.Rows = source.ConsoleRows;
                options.Font = source.ConsoleFont;
            }
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as ConsoleOptions;
            if (options != null)
            {
                FavoriteConfigurationElement destination = context.ConfigFavorite;
                destination.ConsoleBackColor = options.BackColor;
                destination.ConsoleTextColor = options.TextColor;
                destination.ConsoleCursorColor = options.CursorColor;
                destination.ConsoleCols = options.Columns;
                destination.ConsoleRows = options.Rows;
                destination.ConsoleFont = options.Font;
            }
        }
    }
}