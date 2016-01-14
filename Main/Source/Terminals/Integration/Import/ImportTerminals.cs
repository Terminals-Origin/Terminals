using System;
using System.Collections.Generic;
using System.Xml;
using Terminals.Data;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Terminals native xml format import
    /// </summary>
    internal class ImportTerminals : IImport
    {
        internal const string TERMINALS_FILEEXTENSION = ".xml";
        internal const string PROVIDER_NAME = "Terminals favorites";

        private readonly IPersistence persistence;

        public string Name
        {
            get { return PROVIDER_NAME; }
        }

        public string KnownExtension
        {
            get { return TERMINALS_FILEEXTENSION; }
        }

        public ImportTerminals(IPersistence persistence)
        {
            this.persistence = persistence;
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

        private List<FavoriteConfigurationElement> TryImport(string filename)
        {
            using (var reader = new XmlTextReader(filename))
            {
                var propertyReader = new PropertyReader(reader);
                var context = new ImportTerminalsContext(propertyReader, this.persistence);
                while (propertyReader.Read())
                {
                    this.ReadProperty(context);
                }

                return context.Favorites;
            }
        }

        private void ReadProperty(ImportTerminalsContext context)
        {
            switch (context.Reader.NodeType)
            {
                case XmlNodeType.Element:
                    switch (context.Reader.NodeName)
                    {
                        case "favorite":
                            context.SetNewCurrent();
                            break;
                        case "userName":
                            context.Current.UserName = context.Reader.ReadString();
                            break;
                        case "password":
                            context.ReadPassword();
                            break;

                        case "acceleratorPassthrough":
                            context.Current.AcceleratorPassthrough = context.Reader.ReadBool();
                            break;
                        case "allowBackgroundInput":
                            context.Current.AllowBackgroundInput = context.Reader.ReadBool();
                            break;
                        case "authMethod":
                            context.Current.AuthMethod = context.Reader.ReadAuthMethod();
                            break;
                        case "bitmapPeristence":
                            context.Current.BitmapPeristence = context.Reader.ReadBool();
                            break;
                        case "connectionTimeout":
                            context.Current.ConnectionTimeout = context.Reader.ReadInt();
                            break;
                        case "consolefont":
                            context.Current.ConsoleFont = context.Reader.ReadString();
                            break;
                        case "consolerows":
                            context.Current.ConsoleRows = context.Reader.ReadInt();
                            break;
                        case "consolecols":
                            context.Current.ConsoleCols = context.Reader.ReadInt();
                            break;
                        case "consolebackcolor":
                            context.Current.ConsoleBackColor = context.Reader.ReadString();
                            break;
                        case "consoletextcolor":
                            context.Current.ConsoleTextColor = context.Reader.ReadString();
                            break;
                        case "consolecursorcolor":
                            context.Current.ConsoleCursorColor = context.Reader.ReadString();
                            break;
                        case "connectToConsole":
                            context.Current.ConnectToConsole = context.Reader.ReadBool();
                            break;
                        case "colors":
                            context.Current.Colors = context.Reader.ReadColors();
                            break;
                        case "credential":
                            context.Current.Credential = context.Reader.ReadString();
                            break;
                        case "disableWindowsKey":
                            context.Current.DisableWindowsKey = context.Reader.ReadBool();
                            break;
                        case "doubleClickDetect":
                            context.Current.DoubleClickDetect = context.Reader.ReadBool();
                            break;
                        case "displayConnectionBar":
                            context.Current.DisplayConnectionBar = context.Reader.ReadBool();
                            break;
                        case "disableControlAltDelete":
                            context.Current.DisableControlAltDelete = context.Reader.ReadBool();
                            break;
                        case "domainName":
                            context.Current.DomainName = context.Reader.ReadString();
                            break;
                        case "desktopSizeHeight":
                            context.Current.DesktopSizeHeight = context.Reader.ReadInt();
                            break;
                        case "desktopSizeWidth":
                            context.Current.DesktopSizeWidth = context.Reader.ReadInt();
                            break;
                        case "desktopSize":
                            context.Current.DesktopSize = context.Reader.ReadDesktopSize();
                            break;
                        case "desktopShare":
                            context.Current.DesktopShare = context.Reader.ReadString();
                            break;
                        case "disableTheming":
                            context.Current.DisableTheming = context.Reader.ReadBool();
                            break;
                        case "disableMenuAnimations":
                            context.Current.DisableMenuAnimations = context.Reader.ReadBool();
                            break;
                        case "disableFullWindowDrag":
                            context.Current.DisableFullWindowDrag = context.Reader.ReadBool();
                            break;
                        case "disableCursorBlinking":
                            context.Current.DisableCursorBlinking = context.Reader.ReadBool();
                            break;
                        case "disableCursorShadow":
                            context.Current.DisableCursorShadow = context.Reader.ReadBool();
                            break;
                        case "disableWallPaper":
                            context.Current.DisableWallPaper = context.Reader.ReadBool();
                            break;
                        case "executeBeforeConnect":
                            context.Current.ExecuteBeforeConnect = context.Reader.ReadBool();
                            break;
                        case "executeBeforeConnectCommand":
                            context.Current.ExecuteBeforeConnectCommand = context.Reader.ReadString();
                            break;
                        case "executeBeforeConnectArgs":
                            context.Current.ExecuteBeforeConnectArgs = context.Reader.ReadString();
                            break;
                        case "executeBeforeConnectInitialDirectory":
                            context.Current.ExecuteBeforeConnectInitialDirectory = context.Reader.ReadString();
                            break;
                        case "executeBeforeConnectWaitForExit":
                            context.Current.ExecuteBeforeConnectWaitForExit = context.Reader.ReadBool();
                            break;
                        case "enableDesktopComposition":
                            context.Current.EnableDesktopComposition = context.Reader.ReadBool();
                            break;
                        case "enableFontSmoothing":
                            context.Current.EnableFontSmoothing = context.Reader.ReadBool();
                            break;
                        case "enableSecuritySettings":
                            context.Current.EnableSecuritySettings = context.Reader.ReadBool();
                            break;
                        case "enableEncryption":
                            context.Current.EnableEncryption = context.Reader.ReadBool();
                            break;
                        case "enableCompression":
                            context.Current.EnableCompression = context.Reader.ReadBool();
                            break;
                        case "enableTLSAuthentication":
                            context.Current.EnableTLSAuthentication = context.Reader.ReadBool();
                            break;
                        case "enableNLAAuthentication":
                            context.Current.EnableNLAAuthentication = context.Reader.ReadBool();
                            break;
                        case "grabFocusOnConnect":
                            context.Current.GrabFocusOnConnect = context.Reader.ReadBool();
                            break;
                        case "idleTimeout":
                            context.Current.IdleTimeout = context.Reader.ReadInt();
                            break;
                        case "icaServerINI":
                            context.Current.IcaServerINI = context.Reader.ReadString();
                            break;
                        case "icaClientINI":
                            context.Current.IcaClientINI = context.Reader.ReadString();
                            break;
                        case "icaEncryptionLevel":
                            context.Current.IcaEncryptionLevel = context.Reader.ReadString();
                            break;
                        case "iCAApplicationName":
                            context.Current.ICAApplicationName = context.Reader.ReadString();
                            break;
                        case "iCAApplicationWorkingFolder":
                            context.Current.ICAApplicationWorkingFolder = context.Reader.ReadString();
                            break;
                        case "iCAApplicationPath":
                            context.Current.ICAApplicationPath = context.Reader.ReadString();
                            break;
                        case "icaEnableEncryption":
                            context.Current.IcaEnableEncryption = context.Reader.ReadBool();
                            break;
                        case "keyTag":
                            context.Current.KeyTag = context.Reader.ReadString();
                            break;
                        case "SSHKeyFile":
                            context.Current.SSHKeyFile = context.Reader.ReadString();
                            break;
                        case "newWindow":
                            context.Current.NewWindow = context.Reader.ReadBool();
                            break;
                        case "notes":
                            context.Current.Notes = context.Reader.ReadString();
                            break;
                        case "name":
                            context.Current.Name = context.Reader.ReadString();
                            break;
                        case "overallTimeout":
                            context.Current.OverallTimeout = context.Reader.ReadInt();
                            break;
                        case "protocol":
                            context.Current.Protocol = context.Reader.ReadString();
                            break;
                        case "port":
                            context.Current.Port = context.Reader.ReadInt();
                            break;
                        case "redirectedDrives":
                            context.Current.redirectedDrives = context.Reader.ReadString();
                            break;
                        case "redirectPorts":
                            context.Current.RedirectPorts = context.Reader.ReadBool();
                            break;
                        case "redirectPrinters":
                            context.Current.RedirectPrinters = context.Reader.ReadBool();
                            break;
                        case "redirectSmartCards":
                            context.Current.RedirectSmartCards = context.Reader.ReadBool();
                            break;
                        case "redirectClipboard":
                            context.Current.RedirectClipboard = context.Reader.ReadBool();
                            break;
                        case "redirectDevices":
                            context.Current.RedirectDevices = context.Reader.ReadBool();
                            break;
                        case "sounds":
                            context.Current.Sounds = context.Reader.ReadRemoteSounds();
                            break;
                        case "serverName":
                            context.Current.ServerName = context.Reader.ReadString();
                            break;
                        case "shutdownTimeout":
                            context.Current.ShutdownTimeout = context.Reader.ReadInt();
                            break;
                        case "ssh1":
                            context.Current.SSH1 = context.Reader.ReadBool();
                            break;
                        case "securityFullScreen":
                            context.Current.SecurityFullScreen = context.Reader.ReadBool();
                            break;
                        case "securityStartProgram":
                            context.Current.SecurityStartProgram = context.Reader.ReadString();
                            break;
                        case "securityWorkingFolder":
                            context.Current.SecurityWorkingFolder = context.Reader.ReadString();
                            break;
                        case "tags":
                            context.Current.Tags = context.Reader.ReadString();
                            break;
                        case "telnet":
                            context.Current.Telnet = context.Reader.ReadBool();
                            break;
                        case "telnetBackColor":
                            context.Current.TelnetBackColor = context.Reader.ReadString();
                            break;
                        case "telnetCols":
                            context.Current.TelnetCols = context.Reader.ReadInt();
                            break;
                        case "telnetCursorColor":
                            context.Current.TelnetCursorColor = context.Reader.ReadString();
                            break;
                        case "telnetFont":
                            context.Current.TelnetFont = context.Reader.ReadString();
                            break;
                        case "telnetRows":
                            context.Current.TelnetRows = context.Reader.ReadInt();
                            break;
                        case "telnetTextColor":
                            context.Current.TelnetTextColor = context.Reader.ReadString();
                            break;
                        case "toolBarIcon":
                            context.Current.ToolBarIcon = context.Reader.ReadString();
                            break;
                        case "tsgwCredsSource":
                            context.Current.TsgwCredsSource = context.Reader.ReadInt();
                            break;
                        case "tsgwDomain":
                            context.Current.TsgwDomain = context.Reader.ReadString();
                            break;
                        case "tsgwHostname":
                            context.Current.TsgwHostname = context.Reader.ReadString();
                            break;
                        case "tsgwPassword":
                            context.ReadTsgwPassword();
                            break;
                        case "tsgwSeparateLogin":
                            context.Current.TsgwSeparateLogin = context.Reader.ReadBool();
                            break;
                        case "tsgwUsageMethod":
                            context.Current.TsgwUsageMethod = context.Reader.ReadInt();
                            break;
                        case "tsgwUsername":
                            context.Current.TsgwUsername = context.Reader.ReadString();
                            break;
                        case "url":
                            context.Current.Url = context.Reader.ReadString();
                            break;
                        case "vncAutoScale":
                            context.Current.VncAutoScale = context.Reader.ReadBool();
                            break;
                        case "vncViewOnly":
                            context.Current.VncViewOnly = context.Reader.ReadBool();
                            break;
                        case "vncDisplayNumber":
                            context.Current.VncDisplayNumber = context.Reader.ReadInt();
                            break;
                        case "vmrcadministratormode":
                            context.Current.VMRCAdministratorMode = context.Reader.ReadBool();
                            break;
                        case "vmrcreducedcolorsmode":
                            context.Current.VMRCReducedColorsMode = context.Reader.ReadBool();
                            break;
                    }
                    break;
            }
        }
    }
}
