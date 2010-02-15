using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Terminals.ExportImport
{
    class ExportImport
    {
        public static void ExportXML(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword)
        {
            try
            {
                XmlTextWriter w = new XmlTextWriter(fileName, Encoding.UTF8);
                w.WriteStartElement("favorites");
                foreach (FavoriteConfigurationElement element in favorites)
                {
                    w.WriteStartElement("favorite");
                    if (includePassword)
                    {
                        w.WriteElementString("userName", element.UserName);
                        //w.WriteElementString("encryptedPassword", element.EncryptedPassword);
                        w.WriteElementString("password", element.Password);
                    }

                    w.WriteElementString("acceleratorPassthrough", element.AcceleratorPassthrough.ToString());
                    w.WriteElementString("allowBackgroundInput", element.AllowBackgroundInput.ToString());
                    w.WriteElementString("authMethod", element.AuthMethod.ToString());

                    w.WriteElementString("bitmapPeristence", element.Protocol);

                    w.WriteElementString("connectionTimeout", element.ConnectionTimeout.ToString());
                    w.WriteElementString("consolefont", element.ConsoleFont);
                    w.WriteElementString("consolerows", element.ConsoleRows.ToString());
                    w.WriteElementString("consolecols", element.ConsoleCols.ToString());
                    w.WriteElementString("consolebackcolor", element.ConsoleBackColor);
                    w.WriteElementString("consoletextcolor", element.ConsoleTextColor);
                    w.WriteElementString("consolecursorcolor", element.ConsoleCursorColor);
                    w.WriteElementString("connectToConsole", element.ConnectToConsole.ToString());
                    w.WriteElementString("colors", element.Colors.ToString());
                    w.WriteElementString("credential", element.Credential);

                    w.WriteElementString("disableWindowsKey", element.DisableWindowsKey.ToString());
                    w.WriteElementString("doubleClickDetect", element.DoubleClickDetect.ToString());
                    w.WriteElementString("displayConnectionBar", element.DisplayConnectionBar.ToString());
                    w.WriteElementString("disableControlAltDelete", element.DisableControlAltDelete.ToString());
                    w.WriteElementString("domainName", element.DomainName);
                    w.WriteElementString("desktopSizeHeight", element.DesktopSizeHeight.ToString());
                    w.WriteElementString("desktopSizeWidth", element.DesktopSizeWidth.ToString());
                    w.WriteElementString("desktopSize", element.DesktopSize.ToString());
                    w.WriteElementString("desktopShare", element.DesktopShare);
                    w.WriteElementString("disableTheming", element.DisableTheming.ToString());
                    w.WriteElementString("disableMenuAnimations", element.DisableMenuAnimations.ToString());
                    w.WriteElementString("disableFullWindowDrag", element.DisableFullWindowDrag.ToString());
                    w.WriteElementString("disableCursorBlinking", element.DisableCursorBlinking.ToString());
                    w.WriteElementString("disableCursorShadow", element.DisableCursorShadow.ToString());
                    w.WriteElementString("disableWallPaper", element.DisableWallPaper.ToString());

                    w.WriteElementString("executeBeforeConnect", element.ExecuteBeforeConnect.ToString());
                    w.WriteElementString("executeBeforeConnectCommand", element.ExecuteBeforeConnectCommand);
                    w.WriteElementString("executeBeforeConnectArgs", element.ExecuteBeforeConnectArgs);
                    w.WriteElementString("executeBeforeConnectInitialDirectory", element.ExecuteBeforeConnectInitialDirectory);
                    w.WriteElementString("executeBeforeConnectWaitForExit", element.ExecuteBeforeConnectWaitForExit.ToString());
                    w.WriteElementString("enableDesktopComposition", element.EnableDesktopComposition.ToString());
                    w.WriteElementString("enableFontSmoothing", element.EnableFontSmoothing.ToString());
                    w.WriteElementString("enableSecuritySettings", element.EnableSecuritySettings.ToString());
                    w.WriteElementString("enableEncryption", element.EnableEncryption.ToString());
                    w.WriteElementString("enableCompression", element.EnableCompression.ToString());
                    w.WriteElementString("enableTLSAuthentication", element.EnableTLSAuthentication.ToString());

                    w.WriteElementString("grabFocusOnConnect", element.GrabFocusOnConnect.ToString());

                    w.WriteElementString("idleTimeout", element.IdleTimeout.ToString());
                    w.WriteElementString("icaServerINI", element.IcaServerINI);
                    w.WriteElementString("icaClientINI", element.IcaClientINI);
                    w.WriteElementString("icaEncryptionLevel", element.IcaEncryptionLevel);
                    w.WriteElementString("iCAApplicationName", element.ICAApplicationName);
                    w.WriteElementString("iCAApplicationWorkingFolder", element.ICAApplicationWorkingFolder);
                    w.WriteElementString("iCAApplicationPath", element.ICAApplicationPath);
                    w.WriteElementString("icaEnableEncryption", element.IcaEnableEncryption.ToString());

                    w.WriteElementString("keyTag", element.KeyTag);

                    w.WriteElementString("newWindow", element.NewWindow.ToString());
                    w.WriteElementString("notes", element.Notes);
                    w.WriteElementString("name", element.Name);

                    w.WriteElementString("overallTimeout", element.OverallTimeout.ToString());

                    w.WriteElementString("protocol", element.Protocol);
                    w.WriteElementString("port", element.Port.ToString());

                    w.WriteElementString("redirectDrives", element.RedirectDrives.ToString());
                    w.WriteElementString("redirectPorts", element.RedirectPorts.ToString());
                    w.WriteElementString("redirectPrinters", element.RedirectPrinters.ToString());
                    w.WriteElementString("redirectSmartCards", element.RedirectSmartCards.ToString());
                    w.WriteElementString("redirectClipboard", element.RedirectClipboard.ToString());
                    w.WriteElementString("redirectDevices", element.RedirectDevices.ToString());

                    w.WriteElementString("sounds", element.Sounds.ToString());
                    w.WriteElementString("serverName", element.ServerName);
                    w.WriteElementString("shutdownTimeout", element.ShutdownTimeout.ToString());
                    w.WriteElementString("securityFullScreen", element.SecurityFullScreen.ToString());
                    w.WriteElementString("ssh1", element.SSH1.ToString());
                    w.WriteElementString("securityStartProgram", element.SecurityStartProgram);
                    w.WriteElementString("securityWorkingFolder", element.SecurityWorkingFolder);

                    w.WriteElementString("telnet", element.Telnet.ToString());
                    w.WriteElementString("tags", element.Tags.ToString());
                    w.WriteElementString("toolBarIcon", element.ToolBarIcon);
                    w.WriteElementString("telnetBackColor", element.TelnetBackColor);
                    w.WriteElementString("telnetCols", element.TelnetCols.ToString());
                    w.WriteElementString("telnetCursorColor", element.TelnetCursorColor);
                    w.WriteElementString("telnetFont", element.TelnetFont);
                    w.WriteElementString("telnetRows", element.TelnetRows.ToString());
                    w.WriteElementString("telnetTextColor", element.TelnetTextColor);
                    
                    w.WriteElementString("tsgwCredsSource", element.TsgwCredsSource.ToString());
                    w.WriteElementString("tsgwDomain", element.TsgwDomain);
                    w.WriteElementString("tsgwHostname", element.TsgwHostname);
                    w.WriteElementString("tsgwPassword", element.TsgwPassword);
                    w.WriteElementString("tsgwSeparateLogin", element.TsgwSeparateLogin.ToString());
                    w.WriteElementString("tsgwUsageMethod", element.TsgwUsageMethod.ToString());
                    w.WriteElementString("tsgwUsername", element.TsgwUsername);

                    w.WriteElementString("url", element.Url);

                    w.WriteElementString("vncAutoScale", element.VncAutoScale.ToString());
                    w.WriteElementString("vncViewOnly", element.VncViewOnly.ToString());
                    w.WriteElementString("vncDisplayNumber", element.VncDisplayNumber.ToString());
                    w.WriteElementString("vmrcadministratormode", element.VMRCAdministratorMode.ToString());
                    w.WriteElementString("vmrcreducedcolorsmode", element.VMRCReducedColorsMode.ToString());

                    w.WriteEndElement();
                }
                w.WriteEndElement();
                w.Flush();
                w.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public static void ImportXML(string file, bool showOnToolbar)
        {
            FavoriteConfigurationElement fav = null;
            try
            {
                XmlTextReader reader = new XmlTextReader(file);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "favorite":
                                    if (fav != null)
                                        Settings.AddFavorite(fav, showOnToolbar);
                                    fav = new FavoriteConfigurationElement();
                                    break;
                                case "userName":
                                    fav.UserName = reader.ReadString();
                                    break;
                                //case "encryptedPassword":
                                //    fav.EncryptedPassword = reader.ReadString();
                                //    break;
                                case "password":
                                    fav.Password = reader.ReadString();
                                    break;

                                case "acceleratorPassthrough":
                                    fav.AcceleratorPassthrough = ReadBool(reader.ReadString());
                                    break;
                                case "allowBackgroundInput":
                                    fav.AllowBackgroundInput = ReadBool(reader.ReadString());
                                    break;
                                case "authMethod":
                                    fav.AuthMethod = ReadAuthMethod(reader.ReadString());
                                    break;
                                case "bitmapPeristence":
                                    fav.BitmapPeristence = ReadBool(reader.ReadString());
                                    break;
                                case "connectionTimeout":
                                    fav.ConnectionTimeout = ReadInt(reader.ReadString());
                                    break;
                                case "consolefont":
                                    fav.ConsoleFont = reader.ReadString();
                                    break;
                                case "consolerows":
                                    fav.ConsoleRows = ReadInt(reader.ReadString());
                                    break;
                                case "consolecols":
                                    fav.ConsoleCols = ReadInt(reader.ReadString());
                                    break;
                                case "consolebackcolor":
                                    fav.ConsoleBackColor = reader.ReadString();
                                    break;
                                case "consoletextcolor":
                                    fav.ConsoleTextColor = reader.ReadString();
                                    break;
                                case "consolecursorcolor":
                                    fav.ConsoleCursorColor = reader.ReadString();
                                    break;
                                case "connectToConsole":
                                    fav.ConnectToConsole = ReadBool(reader.ReadString());
                                    break;
                                case "colors":
                                    fav.Colors = ReadColors(reader.ReadString());
                                    break;
                                case "credential":
                                    fav.Credential = reader.ReadString();
                                    break;
                                case "disableWindowsKey":
                                    fav.DisableWindowsKey = ReadBool(reader.ReadString());
                                    break;
                                case "doubleClickDetect":
                                    fav.DoubleClickDetect = ReadBool(reader.ReadString());
                                    break;
                                case "displayConnectionBar":
                                    fav.DisplayConnectionBar = ReadBool(reader.ReadString());
                                    break;
                                case "disableControlAltDelete":
                                    fav.DisableControlAltDelete = ReadBool(reader.ReadString());
                                    break;
                                case "domainName":
                                    fav.DomainName = reader.ReadString();
                                    break;
                                case "desktopSizeHeight":
                                    fav.DesktopSizeHeight = ReadInt(reader.ReadString());
                                    break;
                                case "desktopSizeWidth":
                                    fav.DesktopSizeWidth = ReadInt(reader.ReadString());
                                    break;
                                case "desktopSize":
                                    fav.DesktopSize = ReadDesktopSize(reader.ReadString());
                                    break;
                                case "desktopShare":
                                    fav.DesktopShare = reader.ReadString();
                                    break;
                                case "disableTheming":
                                    fav.DisableTheming = ReadBool(reader.ReadString());
                                    break;
                                case "disableMenuAnimations":
                                    fav.DisableMenuAnimations = ReadBool(reader.ReadString());
                                    break;
                                case "disableFullWindowDrag":
                                    fav.DisableFullWindowDrag = ReadBool(reader.ReadString());
                                    break;
                                case "disableCursorBlinking":
                                    fav.DisableCursorBlinking = ReadBool(reader.ReadString());
                                    break;
                                case "disableCursorShadow":
                                    fav.DisableCursorShadow = ReadBool(reader.ReadString());
                                    break;
                                case "disableWallPaper":
                                    fav.DisableWallPaper = ReadBool(reader.ReadString());
                                    break;
                                case "executeBeforeConnect":
                                    fav.ExecuteBeforeConnect = ReadBool(reader.ReadString());
                                    break;
                                case "executeBeforeConnectCommand":
                                    fav.ExecuteBeforeConnectCommand = reader.ReadString();
                                    break;
                                case "executeBeforeConnectArgs":
                                    fav.ExecuteBeforeConnectArgs = reader.ReadString();
                                    break;
                                case "executeBeforeConnectInitialDirectory":
                                    fav.ExecuteBeforeConnectInitialDirectory = reader.ReadString();
                                    break;
                                case "executeBeforeConnectWaitForExit":
                                    fav.ExecuteBeforeConnectWaitForExit = ReadBool(reader.ReadString());
                                    break;
                                case "enableDesktopComposition":
                                    fav.EnableDesktopComposition = ReadBool(reader.ReadString());
                                    break;
                                case "enableFontSmoothing":
                                    fav.EnableFontSmoothing = ReadBool(reader.ReadString());
                                    break;
                                case "enableSecuritySettings":
                                    fav.EnableSecuritySettings = ReadBool(reader.ReadString());
                                    break;
                                case "enableEncryption":
                                    fav.EnableEncryption = ReadBool(reader.ReadString());
                                    break;
                                case "enableCompression":
                                    fav.EnableCompression = ReadBool(reader.ReadString());
                                    break;
                                case "enableTLSAuthentication":
                                    fav.EnableTLSAuthentication = ReadBool(reader.ReadString());
                                    break;
                                case "grabFocusOnConnect":
                                    fav.GrabFocusOnConnect = ReadBool(reader.ReadString());
                                    break;
                                case "idleTimeout":
                                    fav.IdleTimeout = ReadInt(reader.ReadString());
                                    break;
                                case "icaServerINI":
                                    fav.IcaServerINI = reader.ReadString();
                                    break;
                                case "icaClientINI":
                                    fav.IcaClientINI = reader.ReadString();
                                    break;
                                case "icaEncryptionLevel":
                                    fav.IcaEncryptionLevel = reader.ReadString();
                                    break;
                                case "iCAApplicationName":
                                    fav.ICAApplicationName = reader.ReadString();
                                    break;
                                case "iCAApplicationWorkingFolder":
                                    fav.ICAApplicationWorkingFolder = reader.ReadString();
                                    break;
                                case "iCAApplicationPath":
                                    fav.ICAApplicationPath = reader.ReadString();
                                    break;
                                case "icaEnableEncryption":
                                    fav.IcaEnableEncryption = ReadBool(reader.ReadString());
                                    break;
                                case "keyTag":
                                    fav.KeyTag = reader.ReadString();
                                    break;
                                case "newWindow":
                                    fav.NewWindow = ReadBool(reader.ReadString());
                                    break;
                                case "notes":
                                    fav.Notes = reader.ReadString();
                                    break;
                                case "name":
                                    fav.Name = reader.ReadString();
                                    break;
                                case "overallTimeout":
                                    fav.OverallTimeout = ReadInt(reader.ReadString());
                                    break;
                                case "protocol":
                                    fav.Protocol = reader.ReadString();
                                    break;
                                case "port":
                                    fav.Port = ReadInt(reader.ReadString());
                                    break;
                                case "redirectDrives":
                                    fav.RedirectDrives = ReadBool(reader.ReadString());
                                    break;
                                case "redirectPorts":
                                    fav.RedirectPorts = ReadBool(reader.ReadString());
                                    break;
                                case "redirectPrinters":
                                    fav.RedirectPrinters = ReadBool(reader.ReadString());
                                    break;
                                case "redirectSmartCards":
                                    fav.RedirectSmartCards = ReadBool(reader.ReadString());
                                    break;
                                case "redirectClipboard":
                                    fav.RedirectClipboard = ReadBool(reader.ReadString());
                                    break;
                                case "redirectDevices":
                                    fav.RedirectDevices = ReadBool(reader.ReadString());
                                    break;
                                case "sounds":
                                    fav.Sounds = ReadRemoteSounds(reader.ReadString());
                                    break;
                                case "serverName":
                                    fav.ServerName = reader.ReadString();
                                    break;
                                case "shutdownTimeout":
                                    fav.ShutdownTimeout = ReadInt(reader.ReadString());
                                    break;
                                case "ssh1":
                                    fav.SSH1 = ReadBool(reader.ReadString());
                                    break;
                                case "securityFullScreen":
                                    fav.SecurityFullScreen = ReadBool(reader.ReadString());
                                    break;
                                case "securityStartProgram":
                                    fav.SecurityStartProgram = reader.ReadString();
                                    break;
                                case "securityWorkingFolder":
                                    fav.SecurityWorkingFolder = reader.ReadString();
                                    break;
                                case "tags":
                                    fav.Tags = reader.ReadString();
                                    break;
                                case "telnet":
                                    fav.Telnet = ReadBool(reader.ReadString());
                                    break;
                                case "telnetBackColor":
                                    fav.TelnetBackColor = reader.ReadString();
                                    break;
                                case "telnetCols":
                                    fav.TelnetCols = ReadInt(reader.ReadString());
                                    break;
                                case "telnetCursorColor":
                                    fav.TelnetCursorColor = reader.ReadString();
                                    break;
                                case "telnetFont":
                                    fav.TelnetFont = reader.ReadString();
                                    break;
                                case "telnetRows":
                                    fav.TelnetRows = ReadInt(reader.ReadString());
                                    break;
                                case "toolBarIcon":
                                    fav.ToolBarIcon = reader.ReadString();
                                    break;
                                case "tsgwCredsSource":
                                    fav.TsgwCredsSource = ReadInt(reader.ReadString());
                                    break;
                                case "tsgwDomain":
                                    fav.TsgwDomain = reader.ReadString();
                                    break;
                                case "tsgwHostname":
                                    fav.TsgwHostname = reader.ReadString();
                                    break;
                                case "tsgwPassword":
                                    fav.TsgwPassword = reader.ReadString();
                                    break;
                                case "tsgwSeparateLogin":
                                    fav.TsgwSeparateLogin = ReadBool(reader.ReadString());
                                    break;
                                case "tsgwUsageMethod":
                                    fav.TsgwUsageMethod = ReadInt(reader.ReadString());
                                    break;
                                case "tsgwUsername":
                                    fav.TsgwUsername = reader.ReadString();
                                    break;
                                case "url":
                                    fav.Url = reader.ReadString();
                                    break;
                                case "vncAutoScale":
                                    fav.VncAutoScale = ReadBool(reader.ReadString());
                                    break;
                                case "vncViewOnly":
                                    fav.VncViewOnly = ReadBool(reader.ReadString());
                                    break;
                                case "vncDisplayNumber":
                                    fav.VncDisplayNumber = ReadInt(reader.ReadString());
                                    break;
                                case "vmrcadministratormode":
                                    fav.VMRCAdministratorMode = ReadBool(reader.ReadString());
                                    break;
                                case "vmrcreducedcolorsmode":
                                    fav.VMRCReducedColorsMode = ReadBool(reader.ReadString());
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static bool ReadBool(string str)
        {
            bool tmp = false;
            bool.TryParse(str, out tmp);
            return tmp;
        }
        private static int ReadInt(string str)
        {
            int tmp = 0;
            int.TryParse(str, out tmp);
            return tmp;
        }
        private static DesktopSize ReadDesktopSize(string str)
        {
            DesktopSize tmp = DesktopSize.AutoScale;
            if (string.IsNullOrEmpty(str))
                tmp = (DesktopSize)Enum.Parse(typeof(DesktopSize), str);
            return tmp;
        }
        private static Colors ReadColors(string str)
        {
            Colors tmp = Colors.Bit16;
            if (string.IsNullOrEmpty(str))
                tmp = (Colors)Enum.Parse(typeof(Colors), str);
            return tmp;
        }
        private static RemoteSounds ReadRemoteSounds(string str)
        {
            RemoteSounds tmp = RemoteSounds.DontPlay;
            if (string.IsNullOrEmpty(str))
                tmp = (RemoteSounds)Enum.Parse(typeof(RemoteSounds), str);
            return tmp;
        }
        private static SSHClient.AuthMethod ReadAuthMethod(string str)
        {
            SSHClient.AuthMethod tmp = SSHClient.AuthMethod.Password;
            if (string.IsNullOrEmpty(str))
                tmp = (SSHClient.AuthMethod)Enum.Parse(typeof(SSHClient.AuthMethod), str);
            return tmp;
        }
    }
}
