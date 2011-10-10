using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Terminals.Integration.Import;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// This is the Terminals native exporter, which exports favorites into its own xml file
    /// </summary>
    internal class ExportTerminals : IExport
    {
        public void Export(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword)
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

        string IIntegration.Name
        {
            get { return ImportTerminals.PROVIDER_NAME; }
        }

        string IIntegration.KnownExtension
        {
            get { return ImportTerminals.TERMINALS_FILEEXTENSION; }
        }
    }
}
