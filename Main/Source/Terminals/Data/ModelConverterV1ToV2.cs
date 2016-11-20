using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data.Credentials;

namespace Terminals.Data
{
    /// <summary>
    /// Converts favorites from data model used in version 1.X (FavoriteConfigurationElement)
    /// to the model used in version 2.0 (Favorite).
    /// Temporary used also to support imports and export using old data model, 
    /// before they will be updated.
    /// </summary>
    internal class ModelConverterV1ToV2 : ModelConvertersTemplate
    {
        private readonly IFavorites favorites;

        private ModelConverterV1ToV2(IPersistence persistence, ConnectionManager connectionManager)
            : base(persistence, connectionManager)
        {
            this.favorites = persistence.Favorites;
        }

        /// <summary>
        /// Doesn't convert Tags to groups, it has to be handled manually, 
        /// when adding Favorite into Persistence
        /// </summary>
        internal static IFavorite ConvertToFavorite(FavoriteConfigurationElement sourceFavorite,
            IPersistence persistence, ConnectionManager connectionManager)
        {
            var converter = new ModelConverterV1ToV2(persistence, connectionManager);
            return converter.Convert(sourceFavorite);
        }

        private IFavorite Convert(FavoriteConfigurationElement sourceFavorite)
        {
            IFavorite result = this.Persistence.Factory.CreateFavorite();
            ConvertGeneralProperties(result, sourceFavorite);
            ConvertSecurity(result, sourceFavorite);
            ConvertBeforeConnetExecute(result, sourceFavorite);
            ConvertDisplay(result, sourceFavorite);
            
            IOptionsConverter converter = this.CreateOptionsConverter(result.Protocol);
            var context = new OptionsConversionContext(this.CredentialFactory, result, sourceFavorite);
            converter.FromConfigFavorite(context);
            return result;
        }

        private void ConvertGeneralProperties(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            this.ConnectionManager.ChangeProtocol(result, sourceFavorite.Protocol);
            result.Name = sourceFavorite.Name;
            result.Port = sourceFavorite.Port;
            result.ServerName = sourceFavorite.ServerName;
            result.NewWindow = sourceFavorite.NewWindow;
            result.DesktopShare = sourceFavorite.DesktopShare;
            result.Notes = sourceFavorite.Notes;
            this.favorites.UpdateFavoriteIcon(result, sourceFavorite.ToolBarIcon);
        }

        private void ConvertSecurity(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var security = result.Security;
            var guarded = new GuardedSecurity(this.Persistence.Security, security);
            guarded.Domain = sourceFavorite.DomainName;
            guarded.UserName = sourceFavorite.UserName;
            // because persistence and application masterpassword may differ,
            // we have to go through encryption without credential resolution
            guarded.Password = this.Persistence.Security.DecryptPassword(sourceFavorite.EncryptedPassword);
            
            ICredentialSet credential = this.Persistence.Credentials[sourceFavorite.Credential];
            if (credential != null)
                security.Credential = credential.Id;
        }

        private static void ConvertBeforeConnetExecute(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            IBeforeConnectExecuteOptions executeOptions = result.ExecuteBeforeConnect;
            executeOptions.Execute = sourceFavorite.ExecuteBeforeConnect;
            executeOptions.Command = sourceFavorite.ExecuteBeforeConnectCommand;
            executeOptions.CommandArguments = sourceFavorite.ExecuteBeforeConnectArgs;
            executeOptions.InitialDirectory = sourceFavorite.ExecuteBeforeConnectInitialDirectory;
            executeOptions.WaitForExit = sourceFavorite.ExecuteBeforeConnectWaitForExit;
        }

        private static void ConvertDisplay(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            IDisplayOptions display = result.Display;
            display.Colors = sourceFavorite.Colors;
            display.DesktopSize = sourceFavorite.DesktopSize;
            display.Width = sourceFavorite.DesktopSizeWidth;
            display.Height = sourceFavorite.DesktopSizeHeight;
        }
    }
}
