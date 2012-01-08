﻿namespace Terminals.Data
{
    /// <summary>
    /// Converts favorites from data model used in version 1.X (FavoriteConfigurationElement)
    /// to the model used in verison 2.0 (Favorite).
    /// Temoporary used also to suport imports and export using old data model, 
    /// before they will be updated.
    /// </summary>
    internal static class ModelConverterV1ToV2
    {
        /// <summary>
        /// Doesnt convert Tags to groups, it has to be handled manualy, 
        /// when adding Favorite into Persistance
        /// </summary>
        internal static IFavorite ConvertToFavorite(FavoriteConfigurationElement sourceFavorite)
        {
            var result = Persistance.Instance.Factory.CreateFavorite();
            ConvertGeneralProperties(result, sourceFavorite);
            ConvertSecurity(result, sourceFavorite);
            ConvertBeforeConnetExecute(result, sourceFavorite);
            ConvertDisplay(result, sourceFavorite);
            result.ProtocolProperties.FromCofigFavorite(sourceFavorite);

            return result;
        }

        private static void ConvertGeneralProperties(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            result.Name = sourceFavorite.Name;
            result.Protocol = sourceFavorite.Protocol;
            result.Port = sourceFavorite.Port;
            result.ServerName = sourceFavorite.ServerName;
            result.ToolBarIcon = sourceFavorite.ToolBarIcon;
            result.NewWindow = sourceFavorite.NewWindow;
            result.DesktopShare = sourceFavorite.DesktopShare;
            result.Notes = sourceFavorite.Notes;
        }

        private static void ConvertSecurity(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            SecurityOptions security = result.Security;
            security.DomainName = sourceFavorite.DomainName;
            security.UserName = sourceFavorite.UserName;
            security.EncryptedPassword = sourceFavorite.EncryptedPassword;
            ICredentialSet credential = Persistance.Instance.Credentials[sourceFavorite.Credential];
            if (credential != null)
                security.Credential = credential.Id;
        }

        private static void ConvertBeforeConnetExecute(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            BeforeConnectExecuteOptions executeOptions = result.ExecuteBeforeConnect;
            executeOptions.Execute = sourceFavorite.ExecuteBeforeConnect;
            executeOptions.Command = sourceFavorite.ExecuteBeforeConnectCommand;
            executeOptions.CommandArguments = sourceFavorite.ExecuteBeforeConnectArgs;
            executeOptions.InitialDirectory = sourceFavorite.ExecuteBeforeConnectInitialDirectory;
            executeOptions.WaitForExit = sourceFavorite.ExecuteBeforeConnectWaitForExit;
        }

        private static void ConvertDisplay(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            DisplayOptions display = result.Display;
            display.Colors = sourceFavorite.Colors;
            display.DesktopSize = sourceFavorite.DesktopSize;
            display.Width = sourceFavorite.DesktopSizeWidth;
            display.Height = sourceFavorite.DesktopSizeHeight;
        }
    }
}
