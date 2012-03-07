using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Terminals.ExportImport
{
    /// <summary>
    /// Main class which manages terminals import and export into the xml file
    /// </summary>
    internal class ExportImport
    {
        internal static void ExportXML(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword)
        {
            try
            {
                using (XmlTextWriter w = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    w.WriteStartDocument();
                    w.WriteStartElement("favorites");
                    foreach (FavoriteConfigurationElement favorite in favorites)
                    {
                        WriteFavorite(w, includePassword, favorite);
                    }
                    w.WriteEndElement();
                    w.WriteEndDocument();
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Export XML Failed", ex);
            }
        }

        private static void WriteFavorite(XmlTextWriter w, bool includePassword, FavoriteConfigurationElement favorite)
        {
            w.WriteStartElement("favorite");
            if (includePassword)
            {
                w.WriteElementString("userName", favorite.UserName);
                //w.WriteElementString("encryptedPassword", element.EncryptedPassword);
                w.WriteElementString("password", favorite.Password);
            }

            w.WriteElementString("acceleratorPassthrough", favorite.AcceleratorPassthrough.ToString());
            w.WriteElementString("allowBackgroundInput", favorite.AllowBackgroundInput.ToString());
            w.WriteElementString("authMethod", favorite.AuthMethod.ToString());

            w.WriteElementString("bitmapPeristence", favorite.Protocol);

            w.WriteElementString("connectionTimeout", favorite.ConnectionTimeout.ToString());
            w.WriteElementString("consolefont", favorite.ConsoleFont);
            w.WriteElementString("consolerows", favorite.ConsoleRows.ToString());
            w.WriteElementString("consolecols", favorite.ConsoleCols.ToString());
            w.WriteElementString("consolebackcolor", favorite.ConsoleBackColor);
            w.WriteElementString("consoletextcolor", favorite.ConsoleTextColor);
            w.WriteElementString("consolecursorcolor", favorite.ConsoleCursorColor);
            w.WriteElementString("connectToConsole", favorite.ConnectToConsole.ToString());
            w.WriteElementString("colors", favorite.Colors.ToString());
            w.WriteElementString("credential", favorite.Credential);

            w.WriteElementString("disableWindowsKey", favorite.DisableWindowsKey.ToString());
            w.WriteElementString("doubleClickDetect", favorite.DoubleClickDetect.ToString());
            w.WriteElementString("displayConnectionBar", favorite.DisplayConnectionBar.ToString());
            w.WriteElementString("disableControlAltDelete", favorite.DisableControlAltDelete.ToString());
            w.WriteElementString("domainName", favorite.DomainName);
            w.WriteElementString("desktopSizeHeight", favorite.DesktopSizeHeight.ToString());
            w.WriteElementString("desktopSizeWidth", favorite.DesktopSizeWidth.ToString());
            w.WriteElementString("desktopSize", favorite.DesktopSize.ToString());
            w.WriteElementString("desktopShare", favorite.DesktopShare);
            w.WriteElementString("disableTheming", favorite.DisableTheming.ToString());
            w.WriteElementString("disableMenuAnimations", favorite.DisableMenuAnimations.ToString());
            w.WriteElementString("disableFullWindowDrag", favorite.DisableFullWindowDrag.ToString());
            w.WriteElementString("disableCursorBlinking", favorite.DisableCursorBlinking.ToString());
            w.WriteElementString("disableCursorShadow", favorite.DisableCursorShadow.ToString());
            w.WriteElementString("disableWallPaper", favorite.DisableWallPaper.ToString());

            w.WriteElementString("executeBeforeConnect", favorite.ExecuteBeforeConnect.ToString());
            w.WriteElementString("executeBeforeConnectCommand", favorite.ExecuteBeforeConnectCommand);
            w.WriteElementString("executeBeforeConnectArgs", favorite.ExecuteBeforeConnectArgs);
            w.WriteElementString("executeBeforeConnectInitialDirectory", favorite.ExecuteBeforeConnectInitialDirectory);
            w.WriteElementString("executeBeforeConnectWaitForExit", favorite.ExecuteBeforeConnectWaitForExit.ToString());
            w.WriteElementString("enableDesktopComposition", favorite.EnableDesktopComposition.ToString());
            w.WriteElementString("enableFontSmoothing", favorite.EnableFontSmoothing.ToString());
            w.WriteElementString("enableSecuritySettings", favorite.EnableSecuritySettings.ToString());
            w.WriteElementString("enableEncryption", favorite.EnableEncryption.ToString());
            w.WriteElementString("enableCompression", favorite.EnableCompression.ToString());
            w.WriteElementString("enableTLSAuthentication", favorite.EnableTLSAuthentication.ToString());
            w.WriteElementString("enableNLAAuthentication", favorite.EnableNLAAuthentication.ToString());

            w.WriteElementString("grabFocusOnConnect", favorite.GrabFocusOnConnect.ToString());

            w.WriteElementString("idleTimeout", favorite.IdleTimeout.ToString());
            w.WriteElementString("icaServerINI", favorite.IcaServerINI);
            w.WriteElementString("icaClientINI", favorite.IcaClientINI);
            w.WriteElementString("icaEncryptionLevel", favorite.IcaEncryptionLevel);
            w.WriteElementString("iCAApplicationName", favorite.ICAApplicationName);
            w.WriteElementString("iCAApplicationWorkingFolder", favorite.ICAApplicationWorkingFolder);
            w.WriteElementString("iCAApplicationPath", favorite.ICAApplicationPath);
            w.WriteElementString("icaEnableEncryption", favorite.IcaEnableEncryption.ToString());

            w.WriteElementString("keyTag", favorite.KeyTag);

            w.WriteElementString("newWindow", favorite.NewWindow.ToString());
            w.WriteElementString("notes", favorite.Notes);
            w.WriteElementString("name", favorite.Name);

            w.WriteElementString("overallTimeout", favorite.OverallTimeout.ToString());

            w.WriteElementString("protocol", favorite.Protocol);
            w.WriteElementString("port", favorite.Port.ToString());

            w.WriteElementString("redirectedDrives", favorite.redirectedDrives);
            w.WriteElementString("redirectPorts", favorite.RedirectPorts.ToString());
            w.WriteElementString("redirectPrinters", favorite.RedirectPrinters.ToString());
            w.WriteElementString("redirectSmartCards", favorite.RedirectSmartCards.ToString());
            w.WriteElementString("redirectClipboard", favorite.RedirectClipboard.ToString());
            w.WriteElementString("redirectDevices", favorite.RedirectDevices.ToString());

            w.WriteElementString("sounds", favorite.Sounds.ToString());
            w.WriteElementString("serverName", favorite.ServerName);
            w.WriteElementString("shutdownTimeout", favorite.ShutdownTimeout.ToString());
            w.WriteElementString("securityFullScreen", favorite.SecurityFullScreen.ToString());
            w.WriteElementString("ssh1", favorite.SSH1.ToString());
            w.WriteElementString("securityStartProgram", favorite.SecurityStartProgram);
            w.WriteElementString("securityWorkingFolder", favorite.SecurityWorkingFolder);

            w.WriteElementString("telnet", favorite.Telnet.ToString());
            w.WriteElementString("tags", favorite.Tags.ToString());
            w.WriteElementString("toolBarIcon", favorite.ToolBarIcon);
            w.WriteElementString("telnetBackColor", favorite.TelnetBackColor);
            w.WriteElementString("telnetCols", favorite.TelnetCols.ToString());
            w.WriteElementString("telnetCursorColor", favorite.TelnetCursorColor);
            w.WriteElementString("telnetFont", favorite.TelnetFont);
            w.WriteElementString("telnetRows", favorite.TelnetRows.ToString());
            w.WriteElementString("telnetTextColor", favorite.TelnetTextColor);

            w.WriteElementString("tsgwCredsSource", favorite.TsgwCredsSource.ToString());
            w.WriteElementString("tsgwDomain", favorite.TsgwDomain);
            w.WriteElementString("tsgwHostname", favorite.TsgwHostname);
            w.WriteElementString("tsgwPassword", favorite.TsgwPassword);
            w.WriteElementString("tsgwSeparateLogin", favorite.TsgwSeparateLogin.ToString());
            w.WriteElementString("tsgwUsageMethod", favorite.TsgwUsageMethod.ToString());
            w.WriteElementString("tsgwUsername", favorite.TsgwUsername);

            w.WriteElementString("url", favorite.Url);

            w.WriteElementString("vncAutoScale", favorite.VncAutoScale.ToString());
            w.WriteElementString("vncViewOnly", favorite.VncViewOnly.ToString());
            w.WriteElementString("vncDisplayNumber", favorite.VncDisplayNumber.ToString());
            w.WriteElementString("vmrcadministratormode", favorite.VMRCAdministratorMode.ToString());
            w.WriteElementString("vmrcreducedcolorsmode", favorite.VMRCReducedColorsMode.ToString());

            w.WriteEndElement();
        }

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

        private static FavoriteConfigurationElement ReadProperty(XmlTextReader reader, List<FavoriteConfigurationElement> favorites, FavoriteConfigurationElement favorite)
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
