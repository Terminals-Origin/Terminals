using System;
using System.Text;
using System.Xml;
using Terminals.Integration.Import;
using Terminals.Connections;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// This is the Terminals native exporter, which exports favorites into its own xml file
    /// </summary>
    internal class ExportTerminals : IExport
    {
        public void Export(ExportOptions options)
        {
            try
            {
                using (var w = new XmlTextWriter(options.FileName, Encoding.UTF8))
                {
                    w.Formatting = Formatting.Indented;
                    w.WriteStartDocument();
                    w.WriteStartElement("favorites");
                    foreach (FavoriteConfigurationElement favorite in options.Favorites)
                    {
                        WriteFavorite(w, options.IncludePasswords, favorite);
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

            // General tab
            w.WriteElementString("protocol", favorite.Protocol);
            w.WriteElementString("port", favorite.Port.ToString());
            w.WriteElementString("serverName", favorite.ServerName);
            w.WriteElementString("url", favorite.Url);
            w.WriteElementString("name", favorite.Name);
            w.WriteElementString("credential", favorite.Credential);
            w.WriteElementString("domainName", favorite.DomainName);

            if (includePassword)
            {
                w.WriteElementString("userName", favorite.UserName);
                w.WriteElementString("password", favorite.Password);
                //w.WriteElementString("encryptedPassword", element.EncryptedPassword);
            }

            w.WriteElementString("notes", favorite.Notes);

            // RDP tab
            // Display Settings
            if (favorite.Protocol == ConnectionManager.RDP)
            {
                w.WriteElementString("desktopSize", favorite.DesktopSize.ToString());
                w.WriteElementString("desktopSizeHeight", favorite.DesktopSizeHeight.ToString());
                w.WriteElementString("desktopSizeWidth", favorite.DesktopSizeWidth.ToString());
                w.WriteElementString("colors", favorite.Colors.ToString());
                if (favorite.DisableCursorShadow) w.WriteElementString("disableCursorShadow", favorite.DisableCursorShadow.ToString());
                if (favorite.DisableCursorBlinking) w.WriteElementString("disableCursorBlinking", favorite.DisableCursorBlinking.ToString());
                if (favorite.DisableFullWindowDrag) w.WriteElementString("disableFullWindowDrag", favorite.DisableFullWindowDrag.ToString());
                if (favorite.DisableMenuAnimations) w.WriteElementString("disableMenuAnimations", favorite.DisableMenuAnimations.ToString());
                if (favorite.DisableTheming) w.WriteElementString("disableTheming", favorite.DisableTheming.ToString());
                if (favorite.DisableWallPaper) w.WriteElementString("disableWallPaper", favorite.DisableWallPaper.ToString());
                if (favorite.EnableFontSmoothing) w.WriteElementString("enableFontSmoothing", favorite.EnableFontSmoothing.ToString());
                if (favorite.EnableDesktopComposition) w.WriteElementString("enableDesktopComposition", favorite.EnableDesktopComposition.ToString());
                if (favorite.ConnectToConsole) w.WriteElementString("connectToConsole", favorite.ConnectToConsole.ToString());

                // Local Resources
                w.WriteElementString("sounds", favorite.Sounds.ToString());
                if (!String.IsNullOrEmpty(favorite.redirectedDrives)) w.WriteElementString("redirectedDrives", favorite.redirectedDrives);
                if (favorite.RedirectPorts) w.WriteElementString("redirectPorts", favorite.RedirectPorts.ToString());
                if (favorite.RedirectPrinters) w.WriteElementString("redirectPrinters", favorite.RedirectPrinters.ToString());
                if (favorite.RedirectSmartCards) w.WriteElementString("redirectSmartCards", favorite.RedirectSmartCards.ToString());
                if (favorite.RedirectClipboard) w.WriteElementString("redirectClipboard", favorite.RedirectClipboard.ToString());
                if (favorite.RedirectDevices) w.WriteElementString("redirectDevices", favorite.RedirectDevices.ToString());
                if (!String.IsNullOrEmpty(favorite.DesktopShare)) w.WriteElementString("desktopShare", favorite.DesktopShare);

                // Extended Settings
                if (favorite.AllowBackgroundInput) w.WriteElementString("allowBackgroundInput", favorite.AllowBackgroundInput.ToString());
                if (favorite.GrabFocusOnConnect) w.WriteElementString("grabFocusOnConnect", favorite.GrabFocusOnConnect.ToString());
                if (favorite.AcceleratorPassthrough) w.WriteElementString("acceleratorPassthrough", favorite.AcceleratorPassthrough.ToString());
                if (favorite.DisableControlAltDelete) w.WriteElementString("disableControlAltDelete", favorite.DisableControlAltDelete.ToString());
                if (favorite.DisplayConnectionBar) w.WriteElementString("displayConnectionBar", favorite.DisplayConnectionBar.ToString());
                if (favorite.DoubleClickDetect) w.WriteElementString("doubleClickDetect", favorite.DoubleClickDetect.ToString());
                if (favorite.DisableWindowsKey) w.WriteElementString("disableWindowsKey", favorite.DisableWindowsKey.ToString());

                if (favorite.EnableEncryption) w.WriteElementString("enableEncryption", favorite.EnableEncryption.ToString());
                if (favorite.EnableCompression) w.WriteElementString("enableCompression", favorite.EnableCompression.ToString());
                if (favorite.EnableTLSAuthentication) w.WriteElementString("enableTLSAuthentication", favorite.EnableTLSAuthentication.ToString());
                if (favorite.EnableNLAAuthentication) w.WriteElementString("enableNLAAuthentication", favorite.EnableNLAAuthentication.ToString());
                w.WriteElementString("idleTimeout", favorite.IdleTimeout.ToString());
                w.WriteElementString("overallTimeout", favorite.OverallTimeout.ToString());
                w.WriteElementString("connectionTimeout", favorite.ConnectionTimeout.ToString());
                w.WriteElementString("shutdownTimeout", favorite.ShutdownTimeout.ToString());

                // Security Settings
                if (favorite.EnableSecuritySettings)
                {
                    w.WriteElementString("enableSecuritySettings", favorite.EnableSecuritySettings.ToString());
                    w.WriteElementString("securityStartProgram", favorite.SecurityStartProgram);
                    w.WriteElementString("securityWorkingFolder", favorite.SecurityWorkingFolder);
                    w.WriteElementString("securityFullScreen", favorite.SecurityFullScreen.ToString());
                }

                // TSGW
                if (favorite.TsgwUsageMethod != 0)
                {
                    w.WriteElementString("tsgwUsageMethod", favorite.TsgwUsageMethod.ToString());
                    w.WriteElementString("tsgwCredsSource", favorite.TsgwCredsSource.ToString());
                    w.WriteElementString("tsgwDomain", favorite.TsgwDomain);
                    w.WriteElementString("tsgwHostname", favorite.TsgwHostname);
                    w.WriteElementString("tsgwPassword", favorite.TsgwPassword);
                    w.WriteElementString("tsgwSeparateLogin", favorite.TsgwSeparateLogin.ToString());
                    w.WriteElementString("tsgwUsername", favorite.TsgwUsername);
                }
            }

            // VNC tab
            if (favorite.Protocol == ConnectionManager.VNC)
            {
                w.WriteElementString("vncAutoScale", favorite.VncAutoScale.ToString());
                w.WriteElementString("vncViewOnly", favorite.VncViewOnly.ToString());
                w.WriteElementString("vncDisplayNumber", favorite.VncDisplayNumber.ToString());
            }

            // Console tab
            if (favorite.Protocol == ConnectionManager.SSH || favorite.Protocol == ConnectionManager.TELNET)
            {
                // Console settings
                w.WriteElementString("consolerows", favorite.ConsoleRows.ToString());
                w.WriteElementString("consolecols", favorite.ConsoleCols.ToString());
                w.WriteElementString("consolefont", favorite.ConsoleFont);
                w.WriteElementString("consolebackcolor", favorite.ConsoleBackColor);
                w.WriteElementString("consoletextcolor", favorite.ConsoleTextColor);
                w.WriteElementString("consolecursorcolor", favorite.ConsoleCursorColor);
            }

            // SSH tab
            if (favorite.Protocol == ConnectionManager.SSH)
            {
                w.WriteElementString("ssh1", favorite.SSH1.ToString());
                w.WriteElementString("authMethod", favorite.AuthMethod.ToString());
                w.WriteElementString("keyTag", favorite.KeyTag);
            }

            // Telnet
            if (favorite.Protocol == ConnectionManager.TELNET)
            {
                w.WriteElementString("telnet", favorite.Telnet.ToString());
                w.WriteElementString("telnetRows", favorite.TelnetRows.ToString());
                w.WriteElementString("telnetCols", favorite.TelnetCols.ToString());
                w.WriteElementString("telnetFont", favorite.TelnetFont);
                w.WriteElementString("telnetBackColor", favorite.TelnetBackColor);
                w.WriteElementString("telnetTextColor", favorite.TelnetTextColor);
                w.WriteElementString("telnetCursorColor", favorite.TelnetCursorColor);
            }

            // VMRC tab
            if (favorite.Protocol == ConnectionManager.VMRC)
            {
                w.WriteElementString("vmrcadministratormode", favorite.VMRCAdministratorMode.ToString());
                w.WriteElementString("vmrcreducedcolorsmode", favorite.VMRCReducedColorsMode.ToString());
            }

            // Citrix tab
            if (favorite.Protocol == ConnectionManager.ICA_CITRIX)
            {
                w.WriteElementString("iCAApplicationName", favorite.ICAApplicationName);
                w.WriteElementString("iCAApplicationPath", favorite.ICAApplicationPath);
                w.WriteElementString("iCAApplicationWorkingFolder", favorite.ICAApplicationWorkingFolder);
                w.WriteElementString("icaServerINI", favorite.IcaServerINI);
                w.WriteElementString("icaClientINI", favorite.IcaClientINI);
                w.WriteElementString("icaEnableEncryption", favorite.IcaEnableEncryption.ToString());
                w.WriteElementString("icaEncryptionLevel", favorite.IcaEncryptionLevel);
            }

            // RAS tab


            // Tags tab
            w.WriteElementString("tags", favorite.Tags.ToString());

            // Execute tab
            if (favorite.ExecuteBeforeConnect)
            {
                w.WriteElementString("executeBeforeConnect", favorite.ExecuteBeforeConnect.ToString());
                w.WriteElementString("executeBeforeConnectCommand", favorite.ExecuteBeforeConnectCommand);
                w.WriteElementString("executeBeforeConnectArgs", favorite.ExecuteBeforeConnectArgs);
                w.WriteElementString("executeBeforeConnectInitialDirectory", favorite.ExecuteBeforeConnectInitialDirectory);
                w.WriteElementString("executeBeforeConnectWaitForExit", favorite.ExecuteBeforeConnectWaitForExit.ToString());
            }

            // Overall form
            w.WriteElementString("newWindow", favorite.NewWindow.ToString());
            w.WriteElementString("toolBarIcon", favorite.ToolBarIcon);
            w.WriteElementString("bitmapPeristence", favorite.Protocol);

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
