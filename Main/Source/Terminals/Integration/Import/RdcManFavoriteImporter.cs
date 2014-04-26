using System;
using System.Drawing;

namespace Terminals.Integration.Import
{
    internal class RdcManFavoriteImporter
    {
        private readonly RdcManServer server;

        private readonly FavoriteConfigurationElement favorite;

        public RdcManFavoriteImporter(RdcManServer server)
        {
            this.server = server;
            this.favorite = new FavoriteConfigurationElement();
        }

        internal FavoriteConfigurationElement ImportServer()
        {
            ImportGeneralProperties();
            ImportConnectionSettings();
            ImportLocalResources();
            ImportCredentials();
            ImportGatewaySettings();
            ImportDesktopSettings();
            return this.favorite;
        }

        private void ImportGeneralProperties()
        {
            this.favorite.Name = this.server.DisplayName;
            this.favorite.ServerName = this.server.Name;
            this.favorite.Notes = this.server.Comment;
        }

        private void ImportConnectionSettings()
        {
            ConnectionSettings connectionSettings = this.server.ConnectionSettings;
            this.favorite.ConnectToConsole = connectionSettings.ConnectToConsole;
            this.favorite.Port =  connectionSettings.Port;
            this.favorite.SecurityWorkingFolder = connectionSettings.StartDir;
            this.favorite.SecurityStartProgram = connectionSettings.StartProgram;
        }

        private void ImportLocalResources()
        {
            LocalResources localResources = this.server.LocalResources;
            this.favorite.RedirectSmartCards = localResources.RedirectSmartCards;
            this.favorite.RedirectPrinters = localResources.RedirectPrinters;
            this.favorite.RedirectPorts = localResources.RedirectPorts;
            this.favorite.RedirectClipboard = localResources.RedirectClipboard;
            // todo is the mapping correct of keyboardHook?
            this.favorite.GrabFocusOnConnect = localResources.KeyboardHook == 0;
        }

        private void ImportCredentials()
        {
            LogonCredentials credentials = this.server.LogonCredentials;
            this.favorite.DomainName = credentials.Domain;
            this.favorite.UserName = credentials.UserName;
            this.favorite.Password = credentials.Password;
        }

        private void ImportGatewaySettings()
        {
            GatewaySettings tsgwSettings = this.server.GatewaySettings;
            this.favorite.TsgwSeparateLogin = tsgwSettings.IsEnabled;
            this.favorite.TsgwCredsSource = tsgwSettings.LogonMethod;
            this.favorite.TsgwHostname = tsgwSettings.HostName;
            this.favorite.TsgwDomain = tsgwSettings.Domain;
            this.favorite.TsgwUsername = tsgwSettings.UserName;
        }

        private void ImportDesktopSettings()
        {
            RdcManRemoteDesktop desktopSettings = this.server.RemoteDesktop;
            this.favorite.Colors = this.ConvertColorDepthToColors(desktopSettings.ColorDept);
            this.favorite.DesktopSize = this.ConvertDeskopSize(desktopSettings);
            Size screenSize = this.ConvertSize(desktopSettings.Size);
            this.favorite.DesktopSizeHeight = screenSize.Height;
            this.favorite.DesktopSizeWidth = screenSize.Width;
        }

        private Colors ConvertColorDepthToColors(int colorDept)
        {
            switch (colorDept)
            {
                case 32: return Colors.Bits32;
                case 24: return Colors.Bits24;
                case 8: return Colors.Bits8;
                default: return Colors.Bit16;
            }
        }

        private DesktopSize ConvertDeskopSize(RdcManRemoteDesktop desktopSettings)
        {
            if (desktopSettings.FullScreen)
                return DesktopSize.FullScreen;

            if (desktopSettings.SameSizeAsClientArea)
                return DesktopSize.FitToWindow;

            return ResolveConcreateSize(desktopSettings.Size);
        }

        private DesktopSize ResolveConcreateSize(string size)
        {
            switch (size) // compatible values only
            {
                case "800 x 600": return DesktopSize.x800;
                case "1024 x 768": return DesktopSize.x1024;
                case "1280 x 1024": return DesktopSize.x1280;
            }

            // Other applicable, but handled as custom:
            // 1280 x 800, 1440 x 900, 1600 x 1200, 1680 x 1050, 1920 x 1200
            return DesktopSize.Custom;
        }

        private Size ConvertSize(string size)
        {
            var sizeParts = size.Split(new string[]{ " x " }, StringSplitOptions.None);
            int width = Convert.ToInt32(sizeParts[0]);
            int height = Convert.ToInt32(sizeParts[1]);
            return new Size(width, height);
        }
    }
}