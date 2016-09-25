using System.Linq;
using Terminals.Common.Connections;
using Terminals.Common.Converters;
using Terminals.Configuration;
using Terminals.Data.Credentials;

namespace Terminals.Data
{
    /// <summary>
    /// Converts favorites to data model used in version 1.X (FavoriteConfigurationElement)
    /// from the model used in version 2.0 (Favorite).
    /// Temporary used also to support imports and export using old data model, 
    /// before they will be updated.
    /// </summary>
    internal class ModelConverterV2ToV1 : ModelConvertersTemplate
    {
        private ModelConverterV2ToV1(IPersistence persistence) : base(persistence)
        {
        }

        internal static FavoriteConfigurationElement ConvertToFavorite(IFavorite sourceFavorite, IPersistence persistence)
        {
            var converter = new ModelConverterV2ToV1(persistence);
            return converter.ConvertToFavorite(sourceFavorite);
        }

        private FavoriteConfigurationElement ConvertToFavorite(IFavorite sourceFavorite)
        {
            var result = new FavoriteConfigurationElement();
            ConvertGeneralProperties(result, sourceFavorite);
            this.ConvertSecurity(result, sourceFavorite);
            ConvertBeforeConnetExecute(result, sourceFavorite);
            ConvertDisplay(result, sourceFavorite);
            ConvertGroups(result, sourceFavorite);

            IOptionsConverter converter = this.CreateOptionsConverter(result.Protocol);
            var context = new OptionsConversionContext(this.CredentialFactory, sourceFavorite, result);
            converter.ToConfigFavorite(context);
            return result;
        }

        private static void ConvertGroups(FavoriteConfigurationElement result, IFavorite sourceFavorite)
        {
            var groupNames = sourceFavorite.Groups.Select(group => group.Name).ToArray();
            result.Tags = string.Join(",", groupNames);
        }

        private static void ConvertGeneralProperties(FavoriteConfigurationElement result, IFavorite sourceFavorite)
        {
            result.Name = sourceFavorite.Name;
            result.Protocol = sourceFavorite.Protocol;
            result.Port = sourceFavorite.Port;
            result.ServerName = sourceFavorite.ServerName;
            result.Url = UrlConverter.ExtractAbsoluteUrl(sourceFavorite);

            // stoping the backward compatibility: result.ToolBarIcon = sourceFavorite.ToolBarIconFile;
            result.NewWindow = sourceFavorite.NewWindow;
            result.DesktopShare = sourceFavorite.DesktopShare;
            result.Notes = sourceFavorite.Notes;
        }

        private void ConvertSecurity(FavoriteConfigurationElement result, IFavorite sourceFavorite)
        {
            var security = sourceFavorite.Security;
            var guarded = new GuardedSecurity(this.Persistence.Security, security);
            result.DomainName = guarded.Domain;
            result.UserName = guarded.UserName;
            // because persistence and application masterpassword may differ, we have to go through encryption
            var resultSecurity = new FavoriteConfigurationSecurity(this.Persistence, result);
            resultSecurity.Password = guarded.Password;
            
            ICredentialSet credential = this.Persistence.Credentials[security.Credential];
            if(credential != null)
                result.Credential = credential.Name;
        }

        private static void ConvertBeforeConnetExecute(FavoriteConfigurationElement result, IFavorite sourceFavorite)
        {
            IBeforeConnectExecuteOptions executeOptions = sourceFavorite.ExecuteBeforeConnect;
            result.ExecuteBeforeConnect = executeOptions.Execute;
            result.ExecuteBeforeConnectCommand = executeOptions.Command;
            result.ExecuteBeforeConnectArgs = executeOptions.CommandArguments;
            result.ExecuteBeforeConnectInitialDirectory = executeOptions.InitialDirectory;
            result.ExecuteBeforeConnectWaitForExit = executeOptions.WaitForExit;
        }

        private static void ConvertDisplay(FavoriteConfigurationElement result, IFavorite sourceFavorite)
        {
            IDisplayOptions display = sourceFavorite.Display;
            result.Colors = display.Colors;
            result.DesktopSize = display.DesktopSize;
            result.DesktopSizeWidth = display.Width;
            result.DesktopSizeHeight = display.Height;
        }
    }
}
