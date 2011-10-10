using System;
using System.Collections.Generic;
using System.Xml;

namespace Terminals.Integration.Import
{
    internal class ImportTerminals : IImport
    {
        internal const string TERMINALS_FILEEXTENSION = ".xml";
        internal const string PROVIDER_NAME = "Terminals favorites";
        
        #region IImport members

        List<FavoriteConfigurationElement> IImport.ImportFavorites(string Filename)
        {
            return ImportXML(Filename, true);
        }

        public string Name
        {
            get { return PROVIDER_NAME; }
        }

        public string KnownExtension
        {
            get { return TERMINALS_FILEEXTENSION; }
        }
        
        #endregion

        /// <summary>
        /// Loads a new collection of favorites from source file.
        /// The newly created favorites aren't imported into configuration.
        /// </summary>
        internal static List<FavoriteConfigurationElement> ImportXML(string file, bool showOnToolbar)
        {
            List<FavoriteConfigurationElement> favorites = ImportFavorites(file);
            return favorites;
        }

        private static List<FavoriteConfigurationElement> ImportFavorites(string file)
        {
            List<FavoriteConfigurationElement> favorites = new List<FavoriteConfigurationElement>();
            FavoriteConfigurationElement favorite = null;
            try
            {
                using (XmlTextReader reader = new XmlTextReader(file))
                {
                    while (reader.Read())
                    {
                        favorite = ReadProperty(reader, favorites, favorite);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Import XML Failed", ex);
            }

            return favorites;
        }

        private static FavoriteConfigurationElement ReadProperty(XmlTextReader reader,
            List<FavoriteConfigurationElement> favorites, FavoriteConfigurationElement favorite)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    switch (reader.Name)
                    {
                        case "favorite":
                            favorite = new FavoriteConfigurationElement();
                            favorites.Add(favorite);
                            break;
                        case "userName":
                            favorite.UserName = reader.ReadString();
                            break;
                        //case "encryptedPassword":
                        //    fav.EncryptedPassword = reader.ReadString();
                        //    break;
                        case "password":
                            favorite.Password = reader.ReadString();
                            break;

                        case "acceleratorPassthrough":
                            favorite.AcceleratorPassthrough = ReadBool(reader.ReadString());
                            break;
                        case "allowBackgroundInput":
                            favorite.AllowBackgroundInput = ReadBool(reader.ReadString());
                            break;
                        case "authMethod":
                            favorite.AuthMethod = ReadAuthMethod(reader.ReadString());
                            break;
                        case "bitmapPeristence":
                            favorite.BitmapPeristence = ReadBool(reader.ReadString());
                            break;
                        case "connectionTimeout":
                            favorite.ConnectionTimeout = ReadInt(reader.ReadString());
                            break;
                        case "consolefont":
                            favorite.ConsoleFont = reader.ReadString();
                            break;
                        case "consolerows":
                            favorite.ConsoleRows = ReadInt(reader.ReadString());
                            break;
                        case "consolecols":
                            favorite.ConsoleCols = ReadInt(reader.ReadString());
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
                            favorite.ConnectToConsole = ReadBool(reader.ReadString());
                            break;
                        case "colors":
                            favorite.Colors = ReadColors(reader.ReadString());
                            break;
                        case "credential":
                            favorite.Credential = reader.ReadString();
                            break;
                        case "disableWindowsKey":
                            favorite.DisableWindowsKey = ReadBool(reader.ReadString());
                            break;
                        case "doubleClickDetect":
                            favorite.DoubleClickDetect = ReadBool(reader.ReadString());
                            break;
                        case "displayConnectionBar":
                            favorite.DisplayConnectionBar = ReadBool(reader.ReadString());
                            break;
                        case "disableControlAltDelete":
                            favorite.DisableControlAltDelete = ReadBool(reader.ReadString());
                            break;
                        case "domainName":
                            favorite.DomainName = reader.ReadString();
                            break;
                        case "desktopSizeHeight":
                            favorite.DesktopSizeHeight = ReadInt(reader.ReadString());
                            break;
                        case "desktopSizeWidth":
                            favorite.DesktopSizeWidth = ReadInt(reader.ReadString());
                            break;
                        case "desktopSize":
                            favorite.DesktopSize = ReadDesktopSize(reader.ReadString());
                            break;
                        case "desktopShare":
                            favorite.DesktopShare = reader.ReadString();
                            break;
                        case "disableTheming":
                            favorite.DisableTheming = ReadBool(reader.ReadString());
                            break;
                        case "disableMenuAnimations":
                            favorite.DisableMenuAnimations = ReadBool(reader.ReadString());
                            break;
                        case "disableFullWindowDrag":
                            favorite.DisableFullWindowDrag = ReadBool(reader.ReadString());
                            break;
                        case "disableCursorBlinking":
                            favorite.DisableCursorBlinking = ReadBool(reader.ReadString());
                            break;
                        case "disableCursorShadow":
                            favorite.DisableCursorShadow = ReadBool(reader.ReadString());
                            break;
                        case "disableWallPaper":
                            favorite.DisableWallPaper = ReadBool(reader.ReadString());
                            break;
                        case "executeBeforeConnect":
                            favorite.ExecuteBeforeConnect = ReadBool(reader.ReadString());
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
                            favorite.ExecuteBeforeConnectWaitForExit = ReadBool(reader.ReadString());
                            break;
                        case "enableDesktopComposition":
                            favorite.EnableDesktopComposition = ReadBool(reader.ReadString());
                            break;
                        case "enableFontSmoothing":
                            favorite.EnableFontSmoothing = ReadBool(reader.ReadString());
                            break;
                        case "enableSecuritySettings":
                            favorite.EnableSecuritySettings = ReadBool(reader.ReadString());
                            break;
                        case "enableEncryption":
                            favorite.EnableEncryption = ReadBool(reader.ReadString());
                            break;
                        case "enableCompression":
                            favorite.EnableCompression = ReadBool(reader.ReadString());
                            break;
                        case "enableTLSAuthentication":
                            favorite.EnableTLSAuthentication = ReadBool(reader.ReadString());
                            break;
                        case "enableNLAAuthentication":
                            favorite.EnableNLAAuthentication = ReadBool(reader.ReadString());
                            break;
                        case "grabFocusOnConnect":
                            favorite.GrabFocusOnConnect = ReadBool(reader.ReadString());
                            break;
                        case "idleTimeout":
                            favorite.IdleTimeout = ReadInt(reader.ReadString());
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
                            favorite.IcaEnableEncryption = ReadBool(reader.ReadString());
                            break;
                        case "keyTag":
                            favorite.KeyTag = reader.ReadString();
                            break;
                        case "newWindow":
                            favorite.NewWindow = ReadBool(reader.ReadString());
                            break;
                        case "notes":
                            favorite.Notes = reader.ReadString();
                            break;
                        case "name":
                            favorite.Name = reader.ReadString();
                            break;
                        case "overallTimeout":
                            favorite.OverallTimeout = ReadInt(reader.ReadString());
                            break;
                        case "protocol":
                            favorite.Protocol = reader.ReadString();
                            break;
                        case "port":
                            favorite.Port = ReadInt(reader.ReadString());
                            break;
                        case "redirectedDrives":
                            favorite.redirectedDrives = reader.ReadString();
                            break;
                        case "redirectPorts":
                            favorite.RedirectPorts = ReadBool(reader.ReadString());
                            break;
                        case "redirectPrinters":
                            favorite.RedirectPrinters = ReadBool(reader.ReadString());
                            break;
                        case "redirectSmartCards":
                            favorite.RedirectSmartCards = ReadBool(reader.ReadString());
                            break;
                        case "redirectClipboard":
                            favorite.RedirectClipboard = ReadBool(reader.ReadString());
                            break;
                        case "redirectDevices":
                            favorite.RedirectDevices = ReadBool(reader.ReadString());
                            break;
                        case "sounds":
                            favorite.Sounds = ReadRemoteSounds(reader.ReadString());
                            break;
                        case "serverName":
                            favorite.ServerName = reader.ReadString();
                            break;
                        case "shutdownTimeout":
                            favorite.ShutdownTimeout = ReadInt(reader.ReadString());
                            break;
                        case "ssh1":
                            favorite.SSH1 = ReadBool(reader.ReadString());
                            break;
                        case "securityFullScreen":
                            favorite.SecurityFullScreen = ReadBool(reader.ReadString());
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
                            favorite.Telnet = ReadBool(reader.ReadString());
                            break;
                        case "telnetBackColor":
                            favorite.TelnetBackColor = reader.ReadString();
                            break;
                        case "telnetCols":
                            favorite.TelnetCols = ReadInt(reader.ReadString());
                            break;
                        case "telnetCursorColor":
                            favorite.TelnetCursorColor = reader.ReadString();
                            break;
                        case "telnetFont":
                            favorite.TelnetFont = reader.ReadString();
                            break;
                        case "telnetRows":
                            favorite.TelnetRows = ReadInt(reader.ReadString());
                            break;
                        case "telnetTextColor":
                            favorite.TelnetTextColor = reader.ReadString();
                            break;
                        case "toolBarIcon":
                            favorite.ToolBarIcon = reader.ReadString();
                            break;
                        case "tsgwCredsSource":
                            favorite.TsgwCredsSource = ReadInt(reader.ReadString());
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
                            favorite.TsgwSeparateLogin = ReadBool(reader.ReadString());
                            break;
                        case "tsgwUsageMethod":
                            favorite.TsgwUsageMethod = ReadInt(reader.ReadString());
                            break;
                        case "tsgwUsername":
                            favorite.TsgwUsername = reader.ReadString();
                            break;
                        case "url":
                            favorite.Url = reader.ReadString();
                            break;
                        case "vncAutoScale":
                            favorite.VncAutoScale = ReadBool(reader.ReadString());
                            break;
                        case "vncViewOnly":
                            favorite.VncViewOnly = ReadBool(reader.ReadString());
                            break;
                        case "vncDisplayNumber":
                            favorite.VncDisplayNumber = ReadInt(reader.ReadString());
                            break;
                        case "vmrcadministratormode":
                            favorite.VMRCAdministratorMode = ReadBool(reader.ReadString());
                            break;
                        case "vmrcreducedcolorsmode":
                            favorite.VMRCReducedColorsMode = ReadBool(reader.ReadString());
                            break;
                    }
                    break;
            }

            return favorite;
        }

        private static bool ReadBool(String str)
        {
            bool tmp = false;
            bool.TryParse(str, out tmp);
            return tmp;
        }

        private static int ReadInt(String str)
        {
            int tmp = 0;
            int.TryParse(str, out tmp);
            return tmp;
        }

        private static DesktopSize ReadDesktopSize(String str)
        {
            DesktopSize tmp = DesktopSize.AutoScale;
            if (!String.IsNullOrEmpty(str))
                tmp = (DesktopSize)Enum.Parse(typeof(DesktopSize), str);
            return tmp;
        }

        private static Colors ReadColors(String str)
        {
            Colors tmp = Colors.Bit16;
            if (!String.IsNullOrEmpty(str))
                tmp = (Colors)Enum.Parse(typeof(Colors), str);
            return tmp;
        }

        private static RemoteSounds ReadRemoteSounds(String str)
        {
            RemoteSounds tmp = RemoteSounds.DontPlay;
            if (!String.IsNullOrEmpty(str))
                tmp = (RemoteSounds)Enum.Parse(typeof(RemoteSounds), str);
            return tmp;
        }

        private static SSHClient.AuthMethod ReadAuthMethod(String str)
        {
            SSHClient.AuthMethod tmp = SSHClient.AuthMethod.Password;
            if (!String.IsNullOrEmpty(str))
                tmp = (SSHClient.AuthMethod)Enum.Parse(typeof(SSHClient.AuthMethod), str);
            return tmp;
        }
    }
}
