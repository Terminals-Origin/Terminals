using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Rdp
{
    internal class RdpOptionsConverter : IOptionsConverter
    {
        public void FromConfigFavorite(OptionsConversionContext context)
        {
            var rdpOptions = context.Favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
            {
                FavoriteConfigurationElement source = context.ConfigFavorite;
                rdpOptions.ConnectToConsole = source.ConnectToConsole;
                rdpOptions.FullScreen = source.SecurityFullScreen;
                rdpOptions.GrabFocusOnConnect = source.GrabFocusOnConnect;

                this.ToSecurity(source, rdpOptions.Security);
                this.ToRedirect(source, rdpOptions.Redirect);
                this.ToTimeOuts(source, rdpOptions.TimeOuts);
                this.ToUserInterface(source, rdpOptions.UserInterface);
                this.ToTsgwOptions(source, rdpOptions.TsGateway, context.CredentialsFactory);
            }
        }

        private void ToUserInterface(FavoriteConfigurationElement favorite, RdpUserInterfaceOptions userInterface)
        {
            userInterface.AcceleratorPassthrough = favorite.AcceleratorPassthrough;
            userInterface.AllowBackgroundInput = favorite.AllowBackgroundInput;
            userInterface.BitmapPeristence = favorite.BitmapPeristence;
            userInterface.DisableControlAltDelete = favorite.DisableControlAltDelete;
            userInterface.DisableCursorBlinking = favorite.DisableCursorBlinking;
            userInterface.DisableCursorShadow = favorite.DisableCursorShadow;
            userInterface.DisableFullWindowDrag = favorite.DisableFullWindowDrag;
            userInterface.DisableMenuAnimations = favorite.DisableMenuAnimations;
            userInterface.DisableTheming = favorite.DisableTheming;
            userInterface.DisableWallPaper = favorite.DisableWallPaper;
            userInterface.DisableWindowsKey = favorite.DisableWindowsKey;
            userInterface.DisplayConnectionBar = favorite.DisplayConnectionBar;
            userInterface.DoubleClickDetect = favorite.DoubleClickDetect;
            userInterface.EnableCompression = favorite.EnableCompression;
            userInterface.EnableDesktopComposition = favorite.EnableDesktopComposition;
            userInterface.EnableFontSmoothing = favorite.EnableFontSmoothing;
            userInterface.LoadBalanceInfo = favorite.LoadBalanceInfo;
        }

        private void ToTimeOuts(FavoriteConfigurationElement favorite, RdpTimeOutOptions timeOuts)
        {
            timeOuts.IdleTimeout = favorite.IdleTimeout;
            timeOuts.ConnectionTimeout = favorite.ConnectionTimeout;
            timeOuts.OverallTimeout = favorite.OverallTimeout;
            timeOuts.ShutdownTimeout = favorite.ShutdownTimeout;
        }

        private void ToRedirect(FavoriteConfigurationElement favorite, RdpRedirectOptions redirect)
        {
            redirect.Clipboard = favorite.RedirectClipboard;
            redirect.Devices = favorite.RedirectDevices;
            redirect.drives = favorite.redirectedDrives;
            redirect.Ports = favorite.RedirectPorts;
            redirect.Printers = favorite.RedirectPrinters;
            redirect.SmartCards = favorite.RedirectSmartCards;
            redirect.Sounds = favorite.Sounds;
        }

        private void ToSecurity(FavoriteConfigurationElement favorite, RdpSecurityOptions security)
        {
            security.EnableEncryption = favorite.EnableEncryption;
            security.EnableNLAAuthentication = favorite.EnableNLAAuthentication;
            security.Enabled = favorite.EnableSecuritySettings;
            security.EnableTLSAuthentication = favorite.EnableTLSAuthentication;
            security.StartProgram = favorite.SecurityStartProgram;
            security.WorkingFolder = favorite.SecurityWorkingFolder;
        }

        private void ToTsgwOptions(FavoriteConfigurationElement source, TsGwOptions destination, IGuardedCredentialFactory credentialsFactory)
        {
            destination.CredentialSource = source.TsgwCredsSource;
            destination.HostName = source.TsgwHostname;
            destination.SeparateLogin = source.TsgwSeparateLogin;
            destination.UsageMethod = source.TsgwUsageMethod;

            IGuardedSecurity securityOptoins = credentialsFactory.CreateSecurityOptoins(destination.Security);
            securityOptoins.Domain = source.TsgwDomain;
            destination.Security.EncryptedPassword = source.TsgwEncryptedPassword;
            securityOptoins.UserName = source.TsgwUsername;
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var rdpOptions = context.Favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
            {
                FavoriteConfigurationElement destination = context.ConfigFavorite;
                destination.ConnectToConsole = rdpOptions.ConnectToConsole;
                destination.SecurityFullScreen = rdpOptions.FullScreen;
                destination.GrabFocusOnConnect = rdpOptions.GrabFocusOnConnect;

                this.FromSecurity(destination, rdpOptions.Security);
                this.FromRedirect(destination, rdpOptions.Redirect);
                this.FromTimeOuts(destination, rdpOptions.TimeOuts);
                this.FromUserInterface(destination, rdpOptions.UserInterface);
                this.FromTsgwOptions(destination, rdpOptions.TsGateway, context.CredentialsFactory);
            }
        }

        private void FromTsgwOptions(FavoriteConfigurationElement favorite, TsGwOptions tsGateway, IGuardedCredentialFactory credentialsFactory)
        {
            favorite.TsgwCredsSource = tsGateway.CredentialSource;
            favorite.TsgwHostname = tsGateway.HostName;
            favorite.TsgwSeparateLogin = tsGateway.SeparateLogin;
            favorite.TsgwUsageMethod = tsGateway.UsageMethod;

            IGuardedCredential guarded = credentialsFactory.CreateCredential(tsGateway.Security);
            favorite.TsgwDomain = guarded.Domain;
            favorite.TsgwEncryptedPassword = tsGateway.Security.EncryptedPassword;
            favorite.TsgwUsername = guarded.UserName;
        }

        private void FromUserInterface(FavoriteConfigurationElement favorite, RdpUserInterfaceOptions userInterface)
        {
            favorite.AcceleratorPassthrough = userInterface.AcceleratorPassthrough;
            favorite.AllowBackgroundInput = userInterface.AllowBackgroundInput;
            favorite.BitmapPeristence = userInterface.BitmapPeristence;
            favorite.DisableControlAltDelete = userInterface.DisableControlAltDelete;
            favorite.DisableCursorBlinking = userInterface.DisableCursorBlinking;
            favorite.DisableCursorShadow = userInterface.DisableCursorShadow;
            favorite.DisableFullWindowDrag = userInterface.DisableFullWindowDrag;
            favorite.DisableMenuAnimations = userInterface.DisableMenuAnimations;
            favorite.DisableTheming = userInterface.DisableTheming;
            favorite.DisableWallPaper = userInterface.DisableWallPaper;
            favorite.DisableWindowsKey = userInterface.DisableWindowsKey;
            favorite.DisplayConnectionBar = userInterface.DisplayConnectionBar;
            favorite.DoubleClickDetect = userInterface.DoubleClickDetect;
            favorite.EnableCompression = userInterface.EnableCompression;
            favorite.EnableDesktopComposition = userInterface.EnableDesktopComposition;
            favorite.EnableFontSmoothing = userInterface.EnableFontSmoothing;
            favorite.LoadBalanceInfo = userInterface.LoadBalanceInfo;
        }

        private void FromTimeOuts(FavoriteConfigurationElement favorite, RdpTimeOutOptions timeOuts)
        {
            favorite.IdleTimeout = timeOuts.IdleTimeout;
            favorite.ConnectionTimeout = timeOuts.ConnectionTimeout;
            favorite.OverallTimeout = timeOuts.OverallTimeout;
            favorite.ShutdownTimeout = timeOuts.ShutdownTimeout;
        }

        private void FromRedirect(FavoriteConfigurationElement favorite, RdpRedirectOptions redirect)
        {
            favorite.RedirectClipboard = redirect.Clipboard;
            favorite.RedirectDevices = redirect.Devices;
            favorite.redirectedDrives = redirect.drives;
            favorite.RedirectPorts = redirect.Ports;
            favorite.RedirectPrinters = redirect.Printers;
            favorite.RedirectSmartCards = redirect.SmartCards;
            favorite.Sounds = redirect.Sounds;
        }

        private void FromSecurity(FavoriteConfigurationElement favorite, RdpSecurityOptions security)
        {
            favorite.EnableEncryption = security.EnableEncryption;
            favorite.EnableNLAAuthentication = security.EnableNLAAuthentication;
            favorite.EnableSecuritySettings = security.Enabled;
            favorite.EnableTLSAuthentication = security.EnableTLSAuthentication;
            favorite.SecurityStartProgram = security.StartProgram;
            favorite.SecurityWorkingFolder = security.WorkingFolder;
        }
    }
}