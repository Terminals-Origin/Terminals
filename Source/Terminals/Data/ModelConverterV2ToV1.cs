using System.Linq;

namespace Terminals.Data
{
    /// <summary>
    /// Converts favorites to data model used in version 1.X (FavoriteConfigurationElement)
    /// from the model used in version 2.0 (Favorite).
    /// Temporary used also to support imports and export using old data model, 
    /// before they will be updated.
    /// </summary>
    internal class ModelConverterV2ToV1
    {
        private readonly IPersistence persistence;

        private ModelConverterV2ToV1(IPersistence persistence)
        {
            this.persistence = persistence;
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
            sourceFavorite.ProtocolProperties.ToConfigFavorite(sourceFavorite, result);
            ConvertGroups(result, sourceFavorite);
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
            result.Url = WebOptions.ExtractAbsoluteUrl(sourceFavorite);

            result.ToolBarIcon = sourceFavorite.ToolBarIconFile;
            result.NewWindow = sourceFavorite.NewWindow;
            result.DesktopShare = sourceFavorite.DesktopShare;
            result.Notes = sourceFavorite.Notes;
        }

        private void ConvertSecurity(FavoriteConfigurationElement result, IFavorite sourceFavorite)
        {
            ISecurityOptions security = sourceFavorite.Security;
            result.DomainName = security.Domain;
            result.UserName = security.UserName;
            result.EncryptedPassword = security.EncryptedPassword;
            
            ICredentialSet credential = this.persistence.Credentials[security.Credential];
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
