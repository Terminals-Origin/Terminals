using Terminals.Common.Connections;
using Terminals.Common.Converters;
using Terminals.Data;

namespace Terminals.Plugins.Web
{
    internal class WebOptionsConverter : IOptionsConverter
    {
        public void FromConfigFavorite(OptionsConversionContext context)
        {
            UrlConverter.UpdateFavoriteUrl(context.Favorite, context.ConfigFavorite.Url);

            if (context.Favorite.ProtocolProperties is WebOptions options)
            {
                this.FromConfigFavorite(context.ConfigFavorite, options);
            }
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            context.ConfigFavorite.Url = UrlConverter.ExtractAbsoluteUrl(context.Favorite);

            if (context.Favorite.ProtocolProperties is WebOptions options)
            {
                this.ToConfigFavorite(context.ConfigFavorite, options);
            }
        }

        protected void FromConfigFavorite(FavoriteConfigurationElement source, WebOptions options)
        {
            options.UsernameID = source.UsernameID;
            options.PasswordID = source.PasswordID;
            options.OptionalID = source.OptionalID;
            options.OptionalValue = source.OptionalValue;
            options.SubmitID = source.SubmitID;
            options.EnableHTMLAuth = source.EnableHTMLAuth;
            options.EnableFormsAuth = source.EnableFormsAuth;
        }

        protected void ToConfigFavorite(FavoriteConfigurationElement destination, WebOptions options)
        {
            destination.UsernameID = options.UsernameID;
            destination.PasswordID = options.PasswordID;
            destination.OptionalID = options.OptionalID;
            destination.OptionalValue = options.OptionalValue;
            destination.SubmitID = options.SubmitID;
            destination.EnableHTMLAuth = options.EnableHTMLAuth;
            destination.EnableFormsAuth = options.EnableFormsAuth;
        }
    }
}