namespace Terminals.Common.Connections
{
    public abstract class OptionsConverterTemplate<TOptions> : IOptionsConverter
        where TOptions : class
    {
        public void FromConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as TOptions;
            if (options != null)
            {
                this.FromConfigFavorite(context.ConfigFavorite, options);
            }
        }

        protected abstract void FromConfigFavorite(FavoriteConfigurationElement source, TOptions options);

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as TOptions;
            if (options != null)
            {
                this.ToConfigFavorite(context.ConfigFavorite, options);
            }
        }

        protected abstract void ToConfigFavorite(FavoriteConfigurationElement destination, TOptions options);
    }
}