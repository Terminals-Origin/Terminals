using System;
using System.Collections.Generic;
using System.Xml;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Terminals native xml format import
    /// </summary>
    internal class ImportTerminals : IImport
    {
        internal const string TERMINALS_FILEEXTENSION = ".xml";
        internal const string PROVIDER_NAME = "Terminals favorites";
        
        #region IImport members

        public string Name
        {
            get { return PROVIDER_NAME; }
        }

        public string KnownExtension
        {
            get { return TERMINALS_FILEEXTENSION; }
        }

        /// <summary>
        /// Loads a new collection of favorites from source file.
        /// The newly created favorites aren't imported into configuration.
        /// </summary>
        List<FavoriteConfigurationElement> IImport.ImportFavorites(string filename)
        {
            try
            {
                return TryImport(filename);
            }
            catch (Exception ex)
            {
                Logging.Error("Import XML Failed", ex);
                return  new List<FavoriteConfigurationElement>();
            }
        }

        private static List<FavoriteConfigurationElement> TryImport(string filename)
        {
            var favorites = new List<FavoriteConfigurationElement>();
            // bacause reading more than one property into the same favorite, keep the favorite out of the read property method
            FavoriteConfigurationElement favorite = null;

            using (var reader = new XmlTextReader(filename))
            {
                var propertyReaded = new PropertyReader(reader);
                while (propertyReaded.Read())
                {
                    favorite = ReadProperty(propertyReaded, favorites, favorite);
                }
            }

            return favorites;
        }

        #endregion

        private static FavoriteConfigurationElement ReadProperty(PropertyReader reader,
            List<FavoriteConfigurationElement> favorites, FavoriteConfigurationElement favorite)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    switch (reader.NodeName)
                    {
                        case "favorite":
                            favorite = new FavoriteConfigurationElement();
                            favorites.Add(favorite);
                            break;
                        case "userName":
                            favorite.UserName = reader.ReadString();
                            break;
                        case "password":
                            favorite.Password = reader.ReadString();
                            break;

                        case "acceleratorPassthrough":
                            favorite.AcceleratorPassthrough = reader.ReadBool();
                            break;
                        case "allowBackgroundInput":
                            favorite.AllowBackgroundInput = reader.ReadBool();
                            break;
                        case "authMethod":
                            favorite.AuthMethod = reader.ReadAuthMethod();
                            break;
                        case "bitmapPeristence":
                            favorite.BitmapPeristence = reader.ReadBool();
                            break;
                        case "connectionTimeout":
                            favorite.ConnectionTimeout = reader.ReadInt();
                            break;
                        case "consolefont":
                            favorite.ConsoleFont = reader.ReadString();
                            break;
                        case "consolerows":
                            favorite.ConsoleRows = reader.ReadInt();
                            break;
                        case "consolecols":
                            favorite.ConsoleCols = reader.ReadInt();
                            break;
                        case "consolebackcolor":
                            favorite.ConsoleBackColor = reader.ReadString();
                            break;
                        case "consoletextcolor":
                            favorite.ConsoleTextColor = reader.ReadString();
                            break;
                        case "consolecursorcolor":
                            favorite.ConsoleCursorColor = reader.ReadString();
                            break;
                        case "connectToConsole":
                            favorite.ConnectToConsole = reader.ReadBool();
                            break;
                        case "colors":
                            favorite.Colors = reader.ReadColors();
                            break;
                        case "credential":
                            favorite.Credential = reader.ReadString();
                            break;
                        case "disableWindowsKey":
                            favorite.DisableWindowsKey = reader.ReadBool();
                            break;
                        case "doubleClickDetect":
                            favorite.DoubleClickDetect = reader.ReadBool();
                            break;
                        case "displayConnectionBar":
                            favorite.DisplayConnectionBar = reader.ReadBool();
                            break;
                        case "disableControlAltDelete":
                            favorite.DisableControlAltDelete = reader.ReadBool();
                            break;
                        case "domainName":
                            favorite.DomainName = reader.ReadString();
                            break;
                        case "desktopSizeHeight":
                            favorite.DesktopSizeHeight = reader.ReadInt();
                            break;
                        case "desktopSizeWidth":
                            favorite.DesktopSizeWidth = reader.ReadInt();
                            break;
                        case "desktopSize":
                            favorite.DesktopSize = reader.ReadDesktopSize();
                            break;
                        case "desktopShare":
                            favorite.DesktopShare = reader.ReadString();
                            break;
                        case "disableTheming":
                            favorite.DisableTheming = reader.ReadBool();
                            break;
                        case "disableMenuAnimations":
                            favorite.DisableMenuAnimations = reader.ReadBool();
                            break;
                        case "disableFullWindowDrag":
                            favorite.DisableFullWindowDrag = reader.ReadBool();
                            break;
                        case "disableCursorBlinking":
                            favorite.DisableCursorBlinking = reader.ReadBool();
                            break;
                        case "disableCursorShadow":
                            favorite.DisableCursorShadow = reader.ReadBool();
                            break;
                        case "disableWallPaper":
                            favorite.DisableWallPaper = reader.ReadBool();
                            break;
                        case "executeBeforeConnect":
                            favorite.ExecuteBeforeConnect = reader.ReadBool();
                            break;
                        case "executeBeforeConnectCommand":
                            favorite.ExecuteBeforeConnectCommand = reader.ReadString();
                            break;
                        case "executeBeforeConnectArgs":
                            favorite.ExecuteBeforeConnectArgs = reader.ReadString();
                            break;
                        case "executeBeforeConnectInitialDirectory":
                            favorite.ExecuteBeforeConnectInitialDirectory = reader.ReadString();
                            break;
                        case "executeBeforeConnectWaitForExit":
                            favorite.ExecuteBeforeConnectWaitForExit = reader.ReadBool();
                            break;
                        case "enableDesktopComposition":
                            favorite.EnableDesktopComposition = reader.ReadBool();
                            break;
                        case "enableFontSmoothing":
                            favorite.EnableFontSmoothing = reader.ReadBool();
                            break;
                        case "enableSecuritySettings":
                            favorite.EnableSecuritySettings = reader.ReadBool();
                            break;
                        case "enableEncryption":
                            favorite.EnableEncryption = reader.ReadBool();
                            break;
                        case "enableCompression":
                            favorite.EnableCompression = reader.ReadBool();
                            break;
                        case "enableTLSAuthentication":
                            favorite.EnableTLSAuthentication = reader.ReadBool();
                            break;
                        case "enableNLAAuthentication":
                            favorite.EnableNLAAuthentication = reader.ReadBool();
                            break;
                        case "grabFocusOnConnect":
                            favorite.GrabFocusOnConnect = reader.ReadBool();
                            break;
                        case "idleTimeout":
                            favorite.IdleTimeout = reader.ReadInt();
                            break;
                        case "icaServerINI":
                            favorite.IcaServerINI = reader.ReadString();
                            break;
                        case "icaClientINI":
                            favorite.IcaClientINI = reader.ReadString();
                            break;
                        case "icaEncryptionLevel":
                            favorite.IcaEncryptionLevel = reader.ReadString();
                            break;
                        case "iCAApplicationName":
                            favorite.ICAApplicationName = reader.ReadString();
                            break;
                        case "iCAApplicationWorkingFolder":
                            favorite.ICAApplicationWorkingFolder = reader.ReadString();
                            break;
                        case "iCAApplicationPath":
                            favorite.ICAApplicationPath = reader.ReadString();
                            break;
                        case "icaEnableEncryption":
                            favorite.IcaEnableEncryption = reader.ReadBool();
                            break;
                        case "keyTag":
                            favorite.KeyTag = reader.ReadString();
                            break;
                        case "newWindow":
                            favorite.NewWindow = reader.ReadBool();
                            break;
                        case "notes":
                            favorite.Notes = reader.ReadString();
                            break;
                        case "name":
                            favorite.Name = reader.ReadString();
                            break;
                        case "overallTimeout":
                            favorite.OverallTimeout = reader.ReadInt();
                            break;
                        case "protocol":
                            favorite.Protocol = reader.ReadString();
                            break;
                        case "port":
                            favorite.Port = reader.ReadInt();
                            break;
                        case "redirectedDrives":
                            favorite.redirectedDrives = reader.ReadString();
                            break;
                        case "redirectPorts":
                            favorite.RedirectPorts = reader.ReadBool();
                            break;
                        case "redirectPrinters":
                            favorite.RedirectPrinters = reader.ReadBool();
                            break;
                        case "redirectSmartCards":
                            favorite.RedirectSmartCards = reader.ReadBool();
                            break;
                        case "redirectClipboard":
                            favorite.RedirectClipboard = reader.ReadBool();
                            break;
                        case "redirectDevices":
                            favorite.RedirectDevices = reader.ReadBool();
                            break;
                        case "sounds":
                            favorite.Sounds = reader.ReadRemoteSounds();
                            break;
                        case "serverName":
                            favorite.ServerName = reader.ReadString();
                            break;
                        case "shutdownTimeout":
                            favorite.ShutdownTimeout = reader.ReadInt();
                            break;
                        case "ssh1":
                            favorite.SSH1 = reader.ReadBool();
                            break;
                        case "securityFullScreen":
                            favorite.SecurityFullScreen = reader.ReadBool();
                            break;
                        case "securityStartProgram":
                            favorite.SecurityStartProgram = reader.ReadString();
                            break;
                        case "securityWorkingFolder":
                            favorite.SecurityWorkingFolder = reader.ReadString();
                            break;
                        case "tags":
                            favorite.Tags = reader.ReadString();
                            break;
                        case "telnet":
                            favorite.Telnet = reader.ReadBool();
                            break;
                        case "telnetBackColor":
                            favorite.TelnetBackColor = reader.ReadString();
                            break;
                        case "telnetCols":
                            favorite.TelnetCols = reader.ReadInt();
                            break;
                        case "telnetCursorColor":
                            favorite.TelnetCursorColor = reader.ReadString();
                            break;
                        case "telnetFont":
                            favorite.TelnetFont = reader.ReadString();
                            break;
                        case "telnetRows":
                            favorite.TelnetRows = reader.ReadInt();
                            break;
                        case "telnetTextColor":
                            favorite.TelnetTextColor = reader.ReadString();
                            break;
                        case "toolBarIcon":
                            favorite.ToolBarIcon = reader.ReadString();
                            break;
                        case "tsgwCredsSource":
                            favorite.TsgwCredsSource = reader.ReadInt();
                            break;
                        case "tsgwDomain":
                            favorite.TsgwDomain = reader.ReadString();
                            break;
                        case "tsgwHostname":
                            favorite.TsgwHostname = reader.ReadString();
                            break;
                        case "tsgwPassword":
                            favorite.TsgwPassword = reader.ReadString();
                            break;
                        case "tsgwSeparateLogin":
                            favorite.TsgwSeparateLogin = reader.ReadBool();
                            break;
                        case "tsgwUsageMethod":
                            favorite.TsgwUsageMethod = reader.ReadInt();
                            break;
                        case "tsgwUsername":
                            favorite.TsgwUsername = reader.ReadString();
                            break;
                        case "url":
                            favorite.Url = reader.ReadString();
                            break;
                        case "vncAutoScale":
                            favorite.VncAutoScale = reader.ReadBool();
                            break;
                        case "vncViewOnly":
                            favorite.VncViewOnly = reader.ReadBool();
                            break;
                        case "vncDisplayNumber":
                            favorite.VncDisplayNumber = reader.ReadInt();
                            break;
                        case "vmrcadministratormode":
                            favorite.VMRCAdministratorMode = reader.ReadBool();
                            break;
                        case "vmrcreducedcolorsmode":
                            favorite.VMRCReducedColorsMode = reader.ReadBool();
                            break;
                    }
                    break;
            }

            return favorite;
        }
    }
}
