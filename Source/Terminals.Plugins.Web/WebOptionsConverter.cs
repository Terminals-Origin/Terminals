using Terminals.Common.Connections;
using Terminals.Common.Converters;
using Terminals.Data;

namespace Terminals.Plugins.Web
{
    internal class WebOptionsConverter : OptionsConverterTemplate<WebOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, WebOptions options)
        {
            options.UsernameID = source.UsernameID;
            options.PasswordID = source.PasswordID;
            options.OptionalID = source.OptionalID;
            options.OptionalValue = source.OptionalValue;
            options.SubmitID = source.SubmitID;
            options.EnableHTMLAuth = source.EnableHTMLAuth;
            options.EnableFormsAuth = source.EnableFormsAuth;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, WebOptions options)
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