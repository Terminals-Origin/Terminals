using Terminals.Connections;

namespace Terminals.Data
{
    /// <summary>
    /// Converts favorites to data model used in version 1.X (FavoriteConfigurationElement)
    /// from the model used in verison 2.0 (Favorite).
    /// Temoporary used also to suport imports and export using old data model, 
    /// before they will be updated.
    /// </summary>
    internal static class FavoriteModelConverterV2ToV1
    {
        internal static FavoriteConfigurationElement ConvertToFavorite(Favorite sourceFavorite)
        {
            var result = new FavoriteConfigurationElement();
            ConvertGeneralProperties(result, sourceFavorite);
            ConvertSecurity(result, sourceFavorite);
            ConvertBeforeConnetExecute(result, sourceFavorite);
            ConvertDisplay(result, sourceFavorite);
            ConvertProtocolOptions(result, sourceFavorite);

            // todo convert groups/Tags
            return result;
        }

        private static void ConvertGeneralProperties(FavoriteConfigurationElement result, Favorite sourceFavorite)
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
        
        private static void ConvertSecurity(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            SecurityOptions security = sourceFavorite.Security;
            result.DomainName = security.DomainName;
            result.UserName = security.UserName;
            result.EncryptedPassword = security.EncryptedPassword;
            result.Credential = security.Credential;
        }

        private static void ConvertBeforeConnetExecute(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            BeforeConnectExecuteOptions executeOptions = sourceFavorite.BeforeConnectExecute;
            result.ExecuteBeforeConnect = executeOptions.Execute;
            result.ExecuteBeforeConnectCommand = executeOptions.Command;
            result.ExecuteBeforeConnectArgs = executeOptions.ExecuteBeforeConnectArgs;
            result.ExecuteBeforeConnectInitialDirectory = executeOptions.InitialDirectory;
            result.ExecuteBeforeConnectWaitForExit = executeOptions.WaitForExit;
        }

        private static void ConvertDisplay(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            DisplayOptions display = sourceFavorite.Display;
            result.Colors = display.Colors;
            result.DesktopSize = display.DesktopSize;
            result.DesktopSizeWidth = display.Width;
            result.DesktopSizeHeight = display.Height;
        }

        private static void ConvertProtocolOptions(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            switch (result.Protocol)
            {
                case ConnectionManager.VNC:
                    ConvertVncOptions(result, sourceFavorite);
                    break;
                case ConnectionManager.VMRC:
                    ConvertVMRCOptions(result, sourceFavorite);
                    break;
                case ConnectionManager.TELNET:
                    ConvertConsoleOptions(result, sourceFavorite);
                    break;
                case ConnectionManager.RDP:
                    ConvertRdpOptions(result, sourceFavorite);
                    break;
                case ConnectionManager.ICA_CITRIX:
                    ConvertICAOptions(result, sourceFavorite);
                    break;
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                default: // no options to convert
                    break;
            }
        }

        private static void ConvertVncOptions(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            var options = sourceFavorite.ProtocolProperties as VncOptions;
            result.VncAutoScale = options.AutoScale;
            result.VncDisplayNumber = options.DisplayNumber;
            result.VncViewOnly = options.ViewOnly;
        }

        private static void ConvertVMRCOptions(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            var options = sourceFavorite.ProtocolProperties as VMRCOptions;
            result.VMRCAdministratorMode = options.AdministratorMode;
            result.VMRCReducedColorsMode = options.ReducedColorsMode;
        }

        private static void ConvertConsoleOptions(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            var options = sourceFavorite.ProtocolProperties as ConsoleOptions;
            result.ConsoleBackColor = options.BackColor;
            result.ConsoleTextColor = options.TextColor;
            result.ConsoleCursorColor = options.CursorColor;
            result.ConsoleCols = options.Columns;
            result.ConsoleRows = options.Rows;
            result.ConsoleFont = options.Font;
        }

        private static void ConvertICAOptions(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            var options = sourceFavorite.ProtocolProperties as ICAOptions;
            result.ICAApplicationName = options.ApplicationName;
            result.ICAApplicationPath = options.ApplicationPath;
            result.ICAApplicationWorkingFolder = options.ApplicationWorkingFolder;
            result.IcaClientINI = options.ClientINI;
            result.IcaServerINI = options.ServerINI;
            result.IcaEnableEncryption = options.EnableEncryption;
            result.IcaEncryptionLevel = options.EncryptionLevel;
        }

        private static void ConvertRdpOptions(FavoriteConfigurationElement result, Favorite sourceFavorite)
        {
            var options = sourceFavorite.ProtocolProperties as RdpOptions;
            result.ConnectToConsole = options.ConnectToConsole;
            result.SecurityFullScreen = options.FullScreen;
            result.GrabFocusOnConnect = options.GrabFocusOnConnect;
            ConvertRdpSecurity(options.Security, result);
            ConvertRdpRedirects(options.Redirect, result);
            ConvertRdpTimeOuts(options.TimeOuts, result);
            ConvertRdpTsGateway(options.TsGateway, result);
            ConvertRdpUserInterface(options.UserInterface, result);
        }

        private static void ConvertRdpSecurity(RdpSecurityOptions sourceSecurity,
            FavoriteConfigurationElement result)
        {
            result.EnableEncryption = sourceSecurity.EnableEncryption;
            result.EnableNLAAuthentication = sourceSecurity.EnableNLAAuthentication;
            result.EnableSecuritySettings = sourceSecurity.EnableSecuritySettings;
            result.EnableTLSAuthentication = sourceSecurity.EnableTLSAuthentication;
            result.SecurityStartProgram = sourceSecurity.StartProgram;
            result.SecurityWorkingFolder = sourceSecurity.WorkingFolder;
        }

        private static void ConvertRdpRedirects(RdpRedirectOptions sourceRedirect,
            FavoriteConfigurationElement result)
        {
            result.RedirectClipboard = sourceRedirect.Clipboard;
            result.RedirectDevices = sourceRedirect.Devices;
            result.redirectedDrives = sourceRedirect.drives;
            result.RedirectPorts = sourceRedirect.Ports;
            result.RedirectPrinters = sourceRedirect.Printers;
            result.RedirectSmartCards = sourceRedirect.SmartCards;
            result.Sounds = sourceRedirect.Sounds;
        }

        private static void ConvertRdpTimeOuts(RdpTimeOutOptions sourceTimeOuts, FavoriteConfigurationElement result)
        {
            result.ConnectionTimeout = sourceTimeOuts.ConnectionTimeout;
            result.IdleTimeout = sourceTimeOuts.IdleTimeout;
            result.OverallTimeout = sourceTimeOuts.OverallTimeout;
            result.ShutdownTimeout = sourceTimeOuts.ShutdownTimeout;
        }

        private static void ConvertRdpTsGateway(TsGwOptions sourceTsGw, FavoriteConfigurationElement result)
        {
            result.TsgwCredsSource = sourceTsGw.CredentialSource;
            result.TsgwHostname = sourceTsGw.HostName;
            result.TsgwSeparateLogin = sourceTsGw.SeparateLogin;
            result.TsgwUsageMethod = sourceTsGw.UsageMethod;

            result.TsgwDomain = sourceTsGw.Security.DomainName;
            result.TsgwEncryptedPassword = sourceTsGw.Security.EncryptedPassword;
            result.TsgwUsername = sourceTsGw.Security.UserName;
        }

        private static void ConvertRdpUserInterface(RdpUserInterfaceOptions sourceUserInterface,
            FavoriteConfigurationElement result)
        {
            result.AcceleratorPassthrough = sourceUserInterface.AcceleratorPassthrough;
            result.AllowBackgroundInput = sourceUserInterface.AllowBackgroundInput;
            result.BitmapPeristence = sourceUserInterface.BitmapPeristence;
            result.DisableControlAltDelete = sourceUserInterface.DisableControlAltDelete;
            result.DisableCursorBlinking = sourceUserInterface.DisableCursorBlinking;
            result.DisableCursorShadow = sourceUserInterface.DisableCursorShadow;
            result.DisableFullWindowDrag = sourceUserInterface.DisableFullWindowDrag;
            result.DisableMenuAnimations = sourceUserInterface.DisableMenuAnimations;
            result.DisableTheming = sourceUserInterface.DisableTheming;
            result.DisableWallPaper = sourceUserInterface.DisableWallPaper;
            result.DisableWindowsKey = sourceUserInterface.DisableWindowsKey;
            result.DisplayConnectionBar = sourceUserInterface.DisplayConnectionBar;
            result.DoubleClickDetect = sourceUserInterface.DoubleClickDetect;
            result.EnableCompression = sourceUserInterface.EnableCompression;
            result.EnableDesktopComposition = sourceUserInterface.EnableDesktopComposition;
            result.EnableFontSmoothing = sourceUserInterface.EnableFontSmoothing;
        }
    }
}
