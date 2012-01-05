using Terminals.Connections;

namespace Terminals.Data
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
            ConvertProtocolOptions(result, sourceFavorite);

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
            security.Credential = sourceFavorite.Credential;
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

        private static void ConvertProtocolOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
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
                    ConvertTelnetOptions(result, sourceFavorite);
                    break;
                case ConnectionManager.SSH:
                    ConvertSshOptions(result, sourceFavorite);
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

        private static void ConvertVncOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var options = result.ProtocolProperties as VncOptions;
            options.AutoScale = sourceFavorite.VncAutoScale;
            options.DisplayNumber = sourceFavorite.VncDisplayNumber;
            options.ViewOnly = sourceFavorite.VncViewOnly;
        }

        private static void ConvertVMRCOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var options = result.ProtocolProperties as VMRCOptions;
            options.AdministratorMode = sourceFavorite.VMRCAdministratorMode;
            options.ReducedColorsMode = sourceFavorite.VMRCReducedColorsMode;
        }

        private static void ConvertTelnetOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var options = result.ProtocolProperties as ConsoleOptions;
            ConvertConsoleOptions(options, sourceFavorite);
        }

        private static void ConvertSshOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var options = result.ProtocolProperties as SshOptions;
            options.SSH1 = sourceFavorite.SSH1;
            options.AuthMethod = sourceFavorite.AuthMethod;
            options.CertificateKey = sourceFavorite.KeyTag;
            ConvertConsoleOptions(options.Console, sourceFavorite);
        }
        private static void ConvertConsoleOptions(ConsoleOptions options, FavoriteConfigurationElement sourceFavorite)
        {
            options.BackColor = sourceFavorite.ConsoleBackColor;
            options.TextColor = sourceFavorite.ConsoleTextColor;
            options.CursorColor = sourceFavorite.ConsoleCursorColor;
            options.Columns = sourceFavorite.ConsoleCols;
            options.Rows = sourceFavorite.ConsoleRows;
            options.Font = sourceFavorite.ConsoleFont;
        }

        private static void ConvertICAOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var options = result.ProtocolProperties as ICAOptions;
            options.ApplicationName = sourceFavorite.ICAApplicationName;
            options.ApplicationPath = sourceFavorite.ICAApplicationPath;
            options.ApplicationWorkingFolder = sourceFavorite.ICAApplicationWorkingFolder;
            options.ClientINI = sourceFavorite.IcaClientINI;
            options.ServerINI = sourceFavorite.IcaServerINI;
            options.EnableEncryption = sourceFavorite.IcaEnableEncryption;
            options.EncryptionLevel = sourceFavorite.IcaEncryptionLevel;
        }

        private static void ConvertRdpOptions(IFavorite result, FavoriteConfigurationElement sourceFavorite)
        {
            var options = result.ProtocolProperties as RdpOptions;
            options.ConnectToConsole = sourceFavorite.ConnectToConsole;
            options.FullScreen = sourceFavorite.SecurityFullScreen;
            options.GrabFocusOnConnect = sourceFavorite.GrabFocusOnConnect;
            ConvertRdpSecurity(options.Security, sourceFavorite);
            ConvertRdpRedirects(options.Redirect, sourceFavorite);
            ConvertRdpTimeOuts(options.TimeOuts, sourceFavorite);
            ConvertRdpTsGateway(options.TsGateway, sourceFavorite);
            ConvertRdpUserInterface(options.UserInterface, sourceFavorite);
        }

        private static void ConvertRdpSecurity(RdpSecurityOptions resultSecurity,
            FavoriteConfigurationElement sourceFavorite)
        {
            resultSecurity.EnableEncryption = sourceFavorite.EnableEncryption;
            resultSecurity.EnableNLAAuthentication = sourceFavorite.EnableNLAAuthentication;
            resultSecurity.Enabled = sourceFavorite.EnableSecuritySettings;
            resultSecurity.EnableTLSAuthentication = sourceFavorite.EnableTLSAuthentication;
            resultSecurity.StartProgram = sourceFavorite.SecurityStartProgram;
            resultSecurity.WorkingFolder = sourceFavorite.SecurityWorkingFolder;
        }

        private static void ConvertRdpRedirects(RdpRedirectOptions resultRedirect,
            FavoriteConfigurationElement sourceFavorite)
        {
            resultRedirect.Clipboard = sourceFavorite.RedirectClipboard;
            resultRedirect.Devices = sourceFavorite.RedirectDevices;
            resultRedirect.drives = sourceFavorite.redirectedDrives;
            resultRedirect.Ports = sourceFavorite.RedirectPorts;
            resultRedirect.Printers = sourceFavorite.RedirectPrinters;
            resultRedirect.SmartCards = sourceFavorite.RedirectSmartCards;
            resultRedirect.Sounds = sourceFavorite.Sounds;
        }

        private static void ConvertRdpTimeOuts(RdpTimeOutOptions resultTimeOuts, FavoriteConfigurationElement sourceFavorite)
        {
            resultTimeOuts.ConnectionTimeout = sourceFavorite.ConnectionTimeout;
            resultTimeOuts.IdleTimeout = sourceFavorite.IdleTimeout;
            resultTimeOuts.OverallTimeout = sourceFavorite.OverallTimeout;
            resultTimeOuts.ShutdownTimeout = sourceFavorite.ShutdownTimeout;
        }

        private static void ConvertRdpTsGateway(TsGwOptions resultTsGw, FavoriteConfigurationElement sourceFavorite)
        {
            resultTsGw.CredentialSource = sourceFavorite.TsgwCredsSource;
            resultTsGw.HostName = sourceFavorite.TsgwHostname;
            resultTsGw.SeparateLogin = sourceFavorite.TsgwSeparateLogin;
            resultTsGw.UsageMethod = sourceFavorite.TsgwUsageMethod;

            resultTsGw.Security.DomainName = sourceFavorite.TsgwDomain;
            resultTsGw.Security.EncryptedPassword = sourceFavorite.TsgwEncryptedPassword;
            resultTsGw.Security.UserName = sourceFavorite.TsgwUsername;
        }

        private static void ConvertRdpUserInterface(RdpUserInterfaceOptions resultUserInterface,
            FavoriteConfigurationElement sourceFavorite)
        {
            resultUserInterface.AcceleratorPassthrough = sourceFavorite.AcceleratorPassthrough;
            resultUserInterface.AllowBackgroundInput = sourceFavorite.AllowBackgroundInput;
            resultUserInterface.BitmapPeristence = sourceFavorite.BitmapPeristence;
            resultUserInterface.DisableControlAltDelete = sourceFavorite.DisableControlAltDelete;
            resultUserInterface.DisableCursorBlinking = sourceFavorite.DisableCursorBlinking;
            resultUserInterface.DisableCursorShadow = sourceFavorite.DisableCursorShadow;
            resultUserInterface.DisableFullWindowDrag = sourceFavorite.DisableFullWindowDrag;
            resultUserInterface.DisableMenuAnimations = sourceFavorite.DisableMenuAnimations;
            resultUserInterface.DisableTheming = sourceFavorite.DisableTheming;
            resultUserInterface.DisableWallPaper = sourceFavorite.DisableWallPaper;
            resultUserInterface.DisableWindowsKey = sourceFavorite.DisableWindowsKey;
            resultUserInterface.DisplayConnectionBar = sourceFavorite.DisplayConnectionBar;
            resultUserInterface.DoubleClickDetect = sourceFavorite.DoubleClickDetect;
            resultUserInterface.EnableCompression = sourceFavorite.EnableCompression;
            resultUserInterface.EnableDesktopComposition = sourceFavorite.EnableDesktopComposition;
            resultUserInterface.EnableFontSmoothing = sourceFavorite.EnableFontSmoothing;
        }
    }
}
