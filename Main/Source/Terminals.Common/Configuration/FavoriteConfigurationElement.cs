using System;
using System.Collections.Generic;
using System.Configuration;
using Terminals.Common.Configuration;
using Terminals.Converters;

namespace Terminals
{
    /// <summary>
    /// If adding a value here, update constructor, Clone() and native Export and Import.
    /// </summary>
    [Serializable]
    public class FavoriteConfigurationElement : ConfigurationElement, ICloneable
    {
        private const int DEFAULT_PORT = 3389;
        private const string DEFAULT_PROTOCOL = "RDP";
        private const string DEFAULT_BACKCOLOR = "Black";
        private const string DEFAULT_CURSORCOLOR = "Green";
        private const string DEFAULT_TEXTCOLOR = "White";

        public FavoriteConfigurationElement(String name): this()
        {
            Name = name;
        }

        public FavoriteConfigurationElement()
        {
        }

        public override String ToString()
        {
            string domain = String.Empty;
            if(!String.IsNullOrEmpty(this.DomainName))
                domain = this.DomainName + "\\";

            return String.Format(@"Favorite:{0}({1})={2}{3}:{4}",
                this.Name, this.Protocol, domain, this.ServerName, this.Port);
        }

        public Int32 PerformanceFlags
        {
            get
            {
                Int32 result = 0;

                if (DisableCursorShadow) result += (Int32)PerfomanceOptions.TS_PERF_DISABLE_CURSOR_SHADOW;
                if (DisableCursorBlinking) result += (Int32)PerfomanceOptions.TS_PERF_DISABLE_CURSORSETTINGS;
                if (DisableFullWindowDrag) result += (Int32)PerfomanceOptions.TS_PERF_DISABLE_FULLWINDOWDRAG;
                if (DisableMenuAnimations) result += (Int32)PerfomanceOptions.TS_PERF_DISABLE_MENUANIMATIONS;
                if (DisableTheming) result += (Int32)PerfomanceOptions.TS_PERF_DISABLE_THEMING;
                if (DisableWallPaper) result += (Int32)PerfomanceOptions.TS_PERF_DISABLE_WALLPAPER;
                if (EnableDesktopComposition) result += (Int32)PerfomanceOptions.TS_PERF_ENABLE_DESKTOP_COMPOSITION;
                if (EnableFontSmoothing) result += (Int32)PerfomanceOptions.TS_PERF_ENABLE_FONT_SMOOTHING;

                return result;
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            FavoriteConfigurationElement fav = new FavoriteConfigurationElement
                                                   {
                                                       AcceleratorPassthrough = this.AcceleratorPassthrough,
                                                       AllowBackgroundInput = this.AllowBackgroundInput,
                                                       AuthMethod = this.AuthMethod,
                                                       BitmapPeristence = this.BitmapPeristence,
                                                       Colors = this.Colors,
                                                       ConnectionTimeout = this.ConnectionTimeout,
                                                       ConnectToConsole = this.ConnectToConsole,
                                                       ConsoleBackColor = this.ConsoleBackColor,
                                                       ConsoleCols = this.ConsoleCols,
                                                       ConsoleCursorColor = this.ConsoleCursorColor,
                                                       ConsoleFont = this.ConsoleFont,
                                                       ConsoleRows = this.ConsoleRows,
                                                       ConsoleTextColor = this.ConsoleTextColor,
                                                       Credential = this.Credential,
                                                       DesktopShare = this.DesktopShare,
                                                       DesktopSize = this.DesktopSize,
                                                       DesktopSizeHeight = this.DesktopSizeHeight,
                                                       DesktopSizeWidth = this.DesktopSizeWidth,
                                                       DisableControlAltDelete = this.DisableControlAltDelete,
                                                       DisableCursorBlinking = this.DisableCursorBlinking,
                                                       DisableCursorShadow = this.DisableCursorShadow,
                                                       DisableFullWindowDrag = this.DisableFullWindowDrag,
                                                       DisableMenuAnimations = this.DisableMenuAnimations,
                                                       DisableTheming = this.DisableTheming,
                                                       DisableWallPaper = this.DisableWallPaper,
                                                       DisableWindowsKey = this.DisableWindowsKey,
                                                       DisplayConnectionBar = this.DisplayConnectionBar,
                                                       DomainName = this.DomainName,
                                                       DoubleClickDetect = this.DoubleClickDetect,
                                                       EnableCompression = this.EnableCompression,
                                                       EnableDesktopComposition = this.EnableDesktopComposition,
                                                       EnableEncryption = this.EnableCompression,
                                                       EnableFontSmoothing = this.EnableFontSmoothing,
                                                       EnableSecuritySettings = this.EnableSecuritySettings,
                                                       EnableTLSAuthentication = this.EnableTLSAuthentication,
                                                       EnableNLAAuthentication = this.EnableNLAAuthentication,
                                                       EncryptedPassword = this.EncryptedPassword,
                                                       ExecuteBeforeConnect = this.ExecuteBeforeConnect,
                                                       ExecuteBeforeConnectArgs = this.ExecuteBeforeConnectArgs,
                                                       ExecuteBeforeConnectCommand = this.ExecuteBeforeConnectCommand,
                                                       ExecuteBeforeConnectInitialDirectory = this.ExecuteBeforeConnectInitialDirectory,
                                                       ExecuteBeforeConnectWaitForExit = this.ExecuteBeforeConnectWaitForExit,
                                                       GrabFocusOnConnect = this.GrabFocusOnConnect,
                                                       ICAApplicationName = this.ICAApplicationName,
                                                       ICAApplicationWorkingFolder = this.ICAApplicationWorkingFolder,
                                                       ICAApplicationPath = this.ICAApplicationPath,
                                                       IcaClientINI = this.IcaClientINI,
                                                       IcaEnableEncryption = this.IcaEnableEncryption,
                                                       IcaEncryptionLevel = this.IcaEncryptionLevel,
                                                       IcaServerINI = this.IcaServerINI,
                                                       IdleTimeout = this.IdleTimeout,
                                                       KeyTag = this.KeyTag,
                                                       Name = this.Name,
                                                       NewWindow = this.NewWindow,
                                                       Notes = this.Notes,
                                                       OverallTimeout = this.OverallTimeout,
                                                       SshSessionName = this.SshSessionName,
                                                       SshVerbose = this.SshVerbose,
                                                       SshEnablePagentAuthentication = this.SshEnablePagentAuthentication,
                                                       SshEnablePagentForwarding = this.SshEnablePagentForwarding,
                                                       SshX11Forwarding = this.SshX11Forwarding,
                                                       SshEnableCompression = this.SshEnableCompression,
                                                       SshVersion = this.SshVersion,
                                                       Port = this.Port,
                                                       Protocol = this.Protocol,
                                                       RedirectClipboard = this.RedirectClipboard,
                                                       RedirectDevices = this.RedirectDevices,
                                                       RedirectedDrives = this.RedirectedDrives,
                                                       RedirectPorts = this.RedirectPorts,
                                                       RedirectPrinters = this.RedirectPrinters,
                                                       RedirectSmartCards = this.RedirectSmartCards,
                                                       SecurityFullScreen = this.SecurityFullScreen,
                                                       SecurityStartProgram = this.SecurityStartProgram,
                                                       SecurityWorkingFolder = this.SecurityWorkingFolder,
                                                       ServerName = this.ServerName,
                                                       ShutdownTimeout = this.ShutdownTimeout,
                                                       Sounds = this.Sounds,
                                                       SSH1 = this.SSH1,
                                                       Tags = this.Tags,
                                                       Telnet = this.Telnet,
                                                       TelnetBackColor = this.TelnetBackColor,
                                                       TelnetCols = this.TelnetCols,
                                                       TelnetCursorColor = this.TelnetCursorColor,
                                                       TelnetFont = this.TelnetFont,
                                                       TelnetRows = this.TelnetRows,
                                                       TelnetTextColor = this.TelnetTextColor,
                                                       ToolBarIcon = this.ToolBarIcon,
                                                       TsgwCredsSource = this.TsgwCredsSource,
                                                       TsgwDomain = this.TsgwDomain,
                                                       TsgwEncryptedPassword = this.TsgwEncryptedPassword,
                                                       TsgwHostname = this.TsgwHostname,
                                                       TsgwSeparateLogin = this.TsgwSeparateLogin,
                                                       TsgwUsageMethod = this.TsgwUsageMethod,
                                                       TsgwUsername = this.TsgwUsername,
                                                       Url = this.Url,
                                                       UserName = this.UserName,
                                                       VMRCAdministratorMode = this.VMRCAdministratorMode,
                                                       VMRCReducedColorsMode = this.VMRCReducedColorsMode,
                                                       VncAutoScale = this.VncAutoScale,
                                                       VncDisplayNumber = this.VncDisplayNumber,
                                                       VncViewOnly = this.VncViewOnly,
                                                       SSHKeyFile = this.SSHKeyFile,
                                                       TelnetSessionName = this.TelnetSessionName,
                                                       TelnetVerbose = this.TelnetVerbose
                                                   };
            return fav;

        }

        #endregion
       
        #region Serializable properties

        [ConfigurationProperty("telnet", IsRequired = false, DefaultValue = true)]
        public Boolean Telnet
        {
            get
            {
                return (Boolean)this["telnet"];
            }
            set
            {
                this["telnet"] = value;
            }
        }

        [ConfigurationProperty("telnetrows", IsRequired = false, DefaultValue = 33)]
        public Int32 TelnetRows
        {
            get
            {
                return (Int32)this["telnetrows"];
            }
            set
            {
                this["telnetrows"] = value;
            }
        }

        [ConfigurationProperty("telnetcols", IsRequired = false, DefaultValue = 110)]
        public Int32 TelnetCols
        {
            get
            {
                return (Int32)this["telnetcols"];
            }
            set
            {
                this["telnetcols"] = value;
            }
        }

        [ConfigurationProperty("shutdownTimeout", IsRequired = false, DefaultValue = 10)]
        public Int32 ShutdownTimeout
        {
            get
            {
                Int32 val = (Int32)this["shutdownTimeout"];
                if (val > 600)
                    val = 600;

                if (val < 10)
                    val = 10;

                return val;
            }
            set
            {
                if (value > 600) 
                    value = 600;

                if (value < 10) 
                    value = 10;

                this["shutdownTimeout"] = value;
            }
        }
        [ConfigurationProperty("overallTimeout", IsRequired = false, DefaultValue = 600)]
        public Int32 OverallTimeout
        {
            get
            {
                Int32 val = (Int32)this["overallTimeout"];
                if (val > 600) 
                    val = 600;

                if (val < 10) 
                    val = 10;

                return val;
            }
            set
            {
                if (value > 600) 
                    value = 600;

                if (value < 0) 
                    value = 0;

                this["overallTimeout"] = value;
            }
        }

        [ConfigurationProperty("connectionTimeout", IsRequired = false, DefaultValue = 600)]
        public Int32 ConnectionTimeout
        {
            get
            {
                Int32 val = (Int32)this["connectionTimeout"];
                if (val > 600) 
                    val = 600;

                if (val < 10) 
                    val = 10;

                return val;
            }
            set
            {
                if (value > 600) 
                    value = 600;

                if (value < 0) 
                    value = 0;

                this["connectionTimeout"] = value;
            }
        }

        [ConfigurationProperty("idleTimeout", IsRequired = false, DefaultValue = 240)]
        public Int32 IdleTimeout
        {
            get
            {
                Int32 val = (Int32)this["idleTimeout"];
                if (val > 600) 
                    val = 600;

                if (val < 10) 
                    val = 10;

                return val;
            }
            set
            {
                if (value > 240) 
                    value = 240;

                if (value < 0) 
                    value = 0;

                this["idleTimeout"] = value;
            }
        }

        [ConfigurationProperty("securityWorkingFolder", IsRequired = false, DefaultValue = "")]
        public String SecurityWorkingFolder
        {
            get
            {
                return (String)this["securityWorkingFolder"];
            }
            set
            {
                this["securityWorkingFolder"] = value;
            }
        }

        /// <summary>
        /// added in v3.4
        /// </summary>
        [ConfigurationProperty("SSHKeyFile", IsRequired = false, DefaultValue = "")]
        public String SSHKeyFile
        {
            get
            {
                return (String)this["SSHKeyFile"];
            }
            set
            {
                this["SSHKeyFile"] = value;
            }
        }

        [ConfigurationProperty("securityStartProgram", IsRequired = false, DefaultValue = "")]
        public String SecurityStartProgram
        {
            get
            {
                return (String)this["securityStartProgram"];
            }
            set
            {
                this["securityStartProgram"] = value;
            }
        }

        [ConfigurationProperty("credential", IsRequired = false, DefaultValue = "")]
        public String Credential
        {
            get
            {
                return (String)this["credential"];
            }
            set
            {
                this["credential"] = value;
            }
        }

        [ConfigurationProperty("securityFullScreen", IsRequired = false, DefaultValue = false)]
        public Boolean SecurityFullScreen
        {
            get
            {
                return (Boolean)this["securityFullScreen"];
            }
            set
            {
                this["securityFullScreen"] = value;
            }
        }

        [ConfigurationProperty("enableSecuritySettings", IsRequired = false, DefaultValue = false)]
        public Boolean EnableSecuritySettings
        {
            get
            {
                return (Boolean)this["enableSecuritySettings"];
            }
            set
            {
                this["enableSecuritySettings"] = value;
            }
        }

        [ConfigurationProperty("grabFocusOnConnect", IsRequired = false, DefaultValue = false)]
        public Boolean GrabFocusOnConnect
        {
            get
            {
                return (Boolean)this["grabFocusOnConnect"];
            }
            set
            {
                this["grabFocusOnConnect"] = value;
            }
        }

        [ConfigurationProperty("enableEncryption", IsRequired = false, DefaultValue = false)]
        public Boolean EnableEncryption
        {
            get
            {
                return (Boolean)this["enableEncryption"];
            }
            set
            {
                this["enableEncryption"] = value;
            }
        }

        [ConfigurationProperty("disableWindowsKey", IsRequired = false, DefaultValue = false)]
        public Boolean DisableWindowsKey
        {
            get
            {
                return (Boolean)this["disableWindowsKey"];
            }
            set
            {
                this["disableWindowsKey"] = value;
            }
        }

        [ConfigurationProperty("doubleClickDetect", IsRequired = false, DefaultValue = false)]
        public Boolean DoubleClickDetect
        {
            get
            {
                return (Boolean)this["doubleClickDetect"];
            }
            set
            {
                this["doubleClickDetect"] = value;
            }
        }

        [ConfigurationProperty("displayConnectionBar", IsRequired = false, DefaultValue = false)]
        public Boolean DisplayConnectionBar
        {
            get
            {
                return (Boolean)this["displayConnectionBar"];
            }
            set
            {
                this["displayConnectionBar"] = value;
            }
        }

        [ConfigurationProperty("disableControlAltDelete", IsRequired = false, DefaultValue = false)]
        public Boolean DisableControlAltDelete
        {
            get
            {
                return (Boolean)this["disableControlAltDelete"];
            }
            set
            {
                this["disableControlAltDelete"] = value;
            }
        }

        [ConfigurationProperty("acceleratorPassthrough", IsRequired = false, DefaultValue = false)]
        public Boolean AcceleratorPassthrough
        {
            get
            {
                return (Boolean)this["acceleratorPassthrough"];
            }
            set
            {
                this["acceleratorPassthrough"] = value;
            }
        }

        [ConfigurationProperty("enableCompression", IsRequired = false, DefaultValue = false)]
        public Boolean EnableCompression
        {
            get
            {
                return (Boolean)this["enableCompression"];
            }
            set
            {
                this["enableCompression"] = value;
            }
        }

        [ConfigurationProperty("bitmapPeristence", IsRequired = false, DefaultValue = false)]
        public Boolean BitmapPeristence
        {
            get
            {
                return (Boolean)this["bitmapPeristence"];
            }
            set
            {
                this["bitmapPeristence"] = value;
            }
        }

        [ConfigurationProperty("enableTLSAuthentication", IsRequired = false, DefaultValue = false)]
        public Boolean EnableTLSAuthentication
        {
            get
            {
                return (Boolean)this["enableTLSAuthentication"];
            }
            set
            {
                this["enableTLSAuthentication"] = value;
            }
        }

        [ConfigurationProperty("enableNLAAuthentication", IsRequired = false, DefaultValue = false)]
        public Boolean EnableNLAAuthentication
        {
            get
            {
                return (Boolean)this["enableNLAAuthentication"];
            }
            set
            {
                this["enableNLAAuthentication"] = value;
            }
        }

        [ConfigurationProperty("allowBackgroundInput", IsRequired = false, DefaultValue = false)]
        public Boolean AllowBackgroundInput
        {
            get
            {
                return (Boolean)this["allowBackgroundInput"];
            }
            set
            {
                this["allowBackgroundInput"] = value;
            }
        }

        [ConfigurationProperty("ICAApplicationName", IsRequired = false, DefaultValue = "")]
        public String ICAApplicationName
        {
            get
            {
                return (String)this["ICAApplicationName"];
            }
            set
            {
                this["ICAApplicationName"] = value;
            }
        }

        [ConfigurationProperty("ICAApplicationWorkingFolder", IsRequired = false, DefaultValue = "")]
        public String ICAApplicationWorkingFolder
        {
            get
            {
                return (String)this["ICAApplicationWorkingFolder"];
            }
            set
            {
                this["ICAApplicationWorkingFolder"] = value;
            }
        }

        [ConfigurationProperty("ICAApplicationPath", IsRequired = false, DefaultValue = "")]
        public String ICAApplicationPath
        {
            get
            {
                return (String)this["ICAApplicationPath"];
            }
            set
            {
                this["ICAApplicationPath"] = value;
            }
        }


        [ConfigurationProperty("sshSessionName", IsRequired = false, DefaultValue = "")]
        public String SshSessionName {
            get {
                return (String)this["sshSessionName"];
            }
            set {
                this["sshSessionName"] = value;
            }
        }
        [ConfigurationProperty("sshVerbose", IsRequired = false, DefaultValue = false)]
        public bool SshVerbose {
            get {
                return (Boolean)this["sshVerbose"];
            }
            set {
                this["sshVerbose"] = value;
            }
        }

        [ConfigurationProperty("sshEnablePagentAuthentication", IsRequired = false, DefaultValue = false)]
        public bool SshEnablePagentAuthentication {
            get {
                return (Boolean)this["sshEnablePagentAuthentication"];
            }
            set {
                this["sshEnablePagentAuthentication"] = value;
            }
        }

        [ConfigurationProperty("sshEnablePagentForwarding", IsRequired = false, DefaultValue = false)]
        public bool SshEnablePagentForwarding {
            get {
                return (Boolean)this["sshEnablePagentForwarding"];
            }
            set {
                this["sshEnablePagentForwarding"] = value;
            }
        }

        [ConfigurationProperty("sshX11Forwarding", IsRequired = false, DefaultValue = false)]
        public Boolean SshX11Forwarding {
            get {
                return (Boolean)this["sshX11Forwarding"];
            }
            set {
                this["sshX11Forwarding"] = value;
            }
        }
        [ConfigurationProperty("sshEnableCompression", IsRequired = false, DefaultValue = false)]
        public Boolean SshEnableCompression {
            get {
                return (Boolean)this["sshEnableCompression"];
            }
            set {
                this["sshEnableCompression"] = value;
            }
        }

        [ConfigurationProperty("sshVersion", IsRequired = false, DefaultValue = (byte)0)]
        public byte SshVersion {
            get {
                return (byte)this["sshVersion"];
            }
            set {
                this["sshVersion"] = value;
            }
        }

        [ConfigurationProperty("telnetSessionName", IsRequired = false, DefaultValue = "")]
        public String TelnetSessionName {
            get {
                return (String)this["telnetSessionName"];
            }
            set {
                this["telnetSessionName"] = value;
            }
        }
        [ConfigurationProperty("telnetVerbose", IsRequired = false, DefaultValue = false)]
        public bool TelnetVerbose {
            get {
                return (Boolean)this["telnetVerbose"];
            }
            set {
                this["telnetVerbose"] = value;
            }
        }


        [ConfigurationProperty("vmrcreducedcolorsmode", IsRequired = false, DefaultValue = false)]
        public Boolean VMRCReducedColorsMode
        {
            get
            {
                return (Boolean)this["vmrcreducedcolorsmode"];
            }
            set
            {
                this["vmrcreducedcolorsmode"] = value;
            }
        }

        [ConfigurationProperty("vmrcadministratormode", IsRequired = false, DefaultValue = false)]
        public Boolean VMRCAdministratorMode
        {
            get
            {
                return (Boolean)this["vmrcadministratormode"];
            }
            set
            {
                this["vmrcadministratormode"] = value;
            }
        }
        [ConfigurationProperty("ssh1", IsRequired = false, DefaultValue = false)]
        public Boolean SSH1
        {
            get
            {
                return (Boolean)this["ssh1"];
            }
            set
            {
                this["ssh1"] = value;
            }
        }
        [ConfigurationProperty("consolerows", IsRequired = false, DefaultValue = 33)]
        public Int32 ConsoleRows
        {
            get
            {
                return (Int32)this["consolerows"];
            }
            set
            {
                this["consolerows"] = value;
            }
        }
        [ConfigurationProperty("consolecols", IsRequired = false, DefaultValue = 110)]
        public Int32 ConsoleCols
        {
            get
            {
                return (Int32)this["consolecols"];
            }
            set
            {
                this["consolecols"] = value;
            }
        }

        [ConfigurationProperty("consolefont", IsRequired = false, DefaultValue = "")]
        public String ConsoleFont
        {
            get
            {
                String font = (String)this["consolefont"];
                if (String.IsNullOrEmpty(font))
                    font = FontParser.DEFAULT_FONT;
                
                return font;
            }
            set
            {
                this["consolefont"] = value;
            }
        }

        [ConfigurationProperty("consolebackcolor", IsRequired = false, DefaultValue = DEFAULT_BACKCOLOR)]
        public String ConsoleBackColor
        {
            get
            {
                return (String)this["consolebackcolor"];
            }
            set
            {
                this["consolebackcolor"] = value;
            }
        }

        [ConfigurationProperty("consoletextcolor", IsRequired = false, DefaultValue = DEFAULT_TEXTCOLOR)]
        public String ConsoleTextColor
        {
            get
            {
                return (String)this["consoletextcolor"];
            }
            set
            {
                this["consoletextcolor"] = value;
            }
        }

        [ConfigurationProperty("consolecursorcolor", IsRequired = false, DefaultValue = DEFAULT_CURSORCOLOR)]
        public String ConsoleCursorColor
        {
            get
            {
                return (String)this["consolecursorcolor"];
            }
            set
            {
                this["consolecursorcolor"] = value;
            }
        }

        [ConfigurationProperty("protocol", IsRequired = true, DefaultValue = DEFAULT_PROTOCOL)]
        public String Protocol
        {
            get
            {
                return (String)this["protocol"];
            }
            set
            {
                this["protocol"] = value;
            }
        }

        [ConfigurationProperty("toolBarIcon", IsRequired = false, DefaultValue = "")]
        public String ToolBarIcon
        {
            get
            {
                return (String)this["toolBarIcon"];
            }
            set
            {
                this["toolBarIcon"] = value;
            }
        }
        [ConfigurationProperty("telnetfont", IsRequired = false)]
        public String TelnetFont
        {
            get
            {
                String font = (String)this["telnetfont"];
                if (String.IsNullOrEmpty(font))
                    font = FontParser.DEFAULT_FONT;

                return font;
            }
            set
            {
                this["telnetfont"] = value;
            }
        }

        [ConfigurationProperty("telnetbackcolor", IsRequired = false, DefaultValue = DEFAULT_BACKCOLOR)]
        public String TelnetBackColor
        {
            get
            {
                return (String)this["telnetbackcolor"];
            }
            set
            {
                this["telnetbackcolor"] = value;
            }
        }

        [ConfigurationProperty("telnettextcolor", IsRequired = false, DefaultValue = DEFAULT_TEXTCOLOR)]
        public String TelnetTextColor
        {
            get
            {
                return (String)this["telnettextcolor"];
            }
            set
            {
                this["telnettextcolor"] = value;
            }
        }

        [ConfigurationProperty("telnetcursorcolor", IsRequired = false, DefaultValue = DEFAULT_CURSORCOLOR)]
        public String TelnetCursorColor
        {
            get
            {
                return (String)this["telnetcursorcolor"];
            }
            set
            {
                this["telnetcursorcolor"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("serverName", IsRequired = true)]
        public String ServerName
        {
            get
            {
                return (String)this["serverName"];
            }
            set
            {
                this["serverName"] = value;
            }
        }

        [ConfigurationProperty("authMethod", DefaultValue = AuthMethod.Password)]
        public AuthMethod AuthMethod
        {
            get
            {
                return (AuthMethod)this["authMethod"];
            }
            set
            {
                this["authMethod"] = value;
            }
        }

        [ConfigurationProperty("keyTag", DefaultValue = "")]
        public String KeyTag
        {
            get
            {
                return (String)this["keyTag"];
            }
            set
            {
                this["keyTag"] = value;
            }
        }

        [ConfigurationProperty("encryptedPassword", IsRequired = false)]
        public String EncryptedPassword
        {
            get
            {
                return (String)this["encryptedPassword"];
            }
            set
            {
                this["encryptedPassword"] = value;
            }
        }

        [ConfigurationProperty("vncAutoScale", IsRequired = false, DefaultValue = false)]
        public Boolean VncAutoScale
        {
            get
            {
                return (Boolean)this["vncAutoScale"];
            }
            set
            {
                this["vncAutoScale"] = value;
            }
        }

        [ConfigurationProperty("vncViewOnly", IsRequired = false, DefaultValue = false)]
        public Boolean VncViewOnly
        {
            get
            {
                return (Boolean)this["vncViewOnly"];
            }
            set
            {
                this["vncViewOnly"] = value;
            }
        }

        [ConfigurationProperty("vncDisplayNumber", IsRequired = false, DefaultValue = 0)]
        public Int32 VncDisplayNumber
        {
            get
            {
                return (Int32)this["vncDisplayNumber"];
            }
            set
            {
                this["vncDisplayNumber"] = value;
            }
        }


        [ConfigurationProperty("connectToConsole", IsRequired = false)]
        public Boolean ConnectToConsole
        {
            get
            {
                return (Boolean)this["connectToConsole"];
            }
            set
            {
                this["connectToConsole"] = value;
            }
        }

        [ConfigurationProperty("desktopSizeHeight", IsRequired = false)]
        public Int32 DesktopSizeHeight
        {
            get
            {
                return (Int32)this["desktopSizeHeight"];
            }
            set
            {
                this["desktopSizeHeight"] = value;
            }
        }

        [ConfigurationProperty("desktopSizeWidth", IsRequired = false)]
        public Int32 DesktopSizeWidth
        {
            get
            {
                return (Int32)this["desktopSizeWidth"];
            }
            set
            {
                this["desktopSizeWidth"] = value;
            }
        }

        [ConfigurationProperty("desktopSize", IsRequired = false, DefaultValue = DesktopSize.FitToWindow)]
        public DesktopSize DesktopSize
        {
            get
            {
                return (DesktopSize)this["desktopSize"];
            }
            set
            {
                this["desktopSize"] = value;
            }
        }

        [ConfigurationProperty("colors", IsRequired = false, DefaultValue = Colors.Bits32)]
        public Colors Colors
        {
            get
            {
                return (Colors)this["colors"];
            }
            set
            {
                this["colors"] = value;
            }
        }

        [ConfigurationProperty("sounds", DefaultValue = RemoteSounds.DontPlay)]
        public RemoteSounds Sounds
        {
            get
            {
                return (RemoteSounds)this["sounds"];
            }
            set
            {
                this["sounds"] = value;
            }
        }

        [ConfigurationProperty("redirectDrives")]
        public String redirectedDrives
        {
            get
            {
                return (String)this["redirectDrives"];
            }
            set
            {
                this["redirectDrives"] = value;
            }
        }

        public List<String> RedirectedDrives
        {
            get
            {
                List<String> outputList = new List<String>();
                if (!String.IsNullOrEmpty(redirectedDrives))
                {
                    /* Following added for backwards compatibility
                    if (redirectedDrives.Equals("true"))
                    {
                        DriveInfo[] drives = DriveInfo.GetDrives();
                        foreach (DriveInfo drive in drives)
                        {
                            try
                            {
                                outputList.Add(drive.Name.TrimEnd("\\".ToCharArray()));
                            }
                            catch (Exception)
                            { }
                        }
                    }
                    */

        String[] driveArray = redirectedDrives.Split(";".ToCharArray());
                    foreach (String drive in driveArray)
                    {
                        outputList.Add(drive);
                    }
                }

                return outputList;
            }
            set
            {
                String drives = String.Empty;
                for (Int32 i = 0; i < value.Count; i++)
                {
                    drives += value[i];
                    if (i < value.Count - 1)
                        drives += ";";
                }

                redirectedDrives = drives;
            }
        }

        [ConfigurationProperty("redirectPorts")]
        public Boolean RedirectPorts
        {
            get
            {
                return (Boolean)this["redirectPorts"];
            }
            set
            {
                this["redirectPorts"] = value;
            }
        }

        [ConfigurationProperty("newWindow")]
        public Boolean NewWindow
        {
            get
            {
                return (Boolean)this["newWindow"];
            }
            set
            {
                this["newWindow"] = value;
            }
        }

        [ConfigurationProperty("redirectPrinters")]
        public Boolean RedirectPrinters
        {
            get
            {
                return (Boolean)this["redirectPrinters"];
            }
            set
            {
                this["redirectPrinters"] = value;
            }
        }

        [ConfigurationProperty("redirectSmartCards")]
        public Boolean RedirectSmartCards
        {
            get
            {
                return (Boolean)this["redirectSmartCards"];
            }
            set
            {
                this["redirectSmartCards"] = value;
            }
        }

        [ConfigurationProperty("redirectClipboard", DefaultValue = true)]
        public Boolean RedirectClipboard
        {
            get
            {
                return (Boolean)this["redirectClipboard"];
            }
            set
            {
                this["redirectClipboard"] = value;
            }
        }

        [ConfigurationProperty("redirectDevices")]
        public Boolean RedirectDevices
        {
            get
            {
                return (Boolean)this["redirectDevices"];
            }
            set
            {
                this["redirectDevices"] = value;
            }
        }

        /// <summary>
        /// TSC_PROXY_MODE_NONE_DIRECT 0 (0x0)
        /// Do not use an RD Gateway server. In the Remote Desktop Connection (RDC) client UI, the Bypass RD Gateway server for local addresses check box is cleared.
        /// 
        /// TSC_PROXY_MODE_DIRECT 1 (0x1)
        /// Always use an RD Gateway server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is cleared.
        /// 
        /// TSC_PROXY_MODE_DETECT 2 (0x2)
        /// Use an RD Gateway server if a direct connection cannot be made to the RD Session Host server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is selected.
        /// 
        /// TSC_PROXY_MODE_DEFAULT 3 (0x3)
        /// Use the default RD Gateway server settings.
        /// 
        /// TSC_PROXY_MODE_NONE_DETECT 4 (0x4)
        /// Do not use an RD Gateway server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is selected.
        /// </summary>
        [ConfigurationProperty("tsgwUsageMethod", DefaultValue = 0)]
        public Int32 TsgwUsageMethod
        {
            get
            {
                return (Int32)this["tsgwUsageMethod"];
            }
            set
            {
                this["tsgwUsageMethod"] = value;
            }
        }

        [ConfigurationProperty("tsgwHostname", DefaultValue = "")]
        public String TsgwHostname
        {
            get
            {
                return (String)this["tsgwHostname"];
            }
            set
            {
                this["tsgwHostname"] = value;
            }
        }

        [ConfigurationProperty("tsgwCredsSource", DefaultValue = 0)]
        public Int32 TsgwCredsSource
        {
            get
            {
                return (Int32)this["tsgwCredsSource"];
            }
            set
            {
                this["tsgwCredsSource"] = value;
            }
        }

        [ConfigurationProperty("tsgwSeparateLogin", DefaultValue = false)]
        public Boolean TsgwSeparateLogin
        {
            get
            {
                return (Boolean)this["tsgwSeparateLogin"];
            }
            set
            {
                this["tsgwSeparateLogin"] = value;
            }
        }

        [ConfigurationProperty("tsgwUsername", DefaultValue = "")]
        public String TsgwUsername
        {
            get
            {
                return (String)this["tsgwUsername"];
            }
            set
            {
                this["tsgwUsername"] = value;
            }
        }

        [ConfigurationProperty("tsgwDomain", DefaultValue = "")]
        public String TsgwDomain
        {
            get
            {
                return (String)this["tsgwDomain"];
            }
            set
            {
                this["tsgwDomain"] = value;
            }
        }

        [ConfigurationProperty("tsgwPassword", DefaultValue = "")]
        public String TsgwEncryptedPassword
        {
            get
            {
                return (String)this["tsgwPassword"];
            }
            set
            {
                this["tsgwPassword"] = value;
            }
        }

        [ConfigurationProperty("url", DefaultValue = "http://terminals.codeplex.com")]
        public String Url
        {
            get
            {
                return (String)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }

        [ConfigurationProperty("notes")]
        public String Notes
        {
            get
            {
                return TextConverter.DecodeFrom64((String)this["notes"]);
            }
            set
            {
                this["notes"] = TextConverter.EncodeTo64(value);
            }
        }

        [ConfigurationProperty("icaServerINI")]
        public String IcaServerINI
        {
            get
            {
                return (String)this["icaServerINI"];
            }
            set
            {
                this["icaServerINI"] = value;
            }
        }
        [ConfigurationProperty("icaClientINI")]
        public String IcaClientINI
        {
            get
            {
                return (String)this["icaClientINI"];
            }
            set
            {
                this["icaClientINI"] = value;
            }
        }
        [ConfigurationProperty("icaEnableEncryption")]
        public Boolean IcaEnableEncryption
        {
            get
            {
                return (Boolean)this["icaEnableEncryption"];
            }
            set
            {
                this["icaEnableEncryption"] = value;
            }
        }
        [ConfigurationProperty("icaEncryptionLevel")]
        public String IcaEncryptionLevel
        {
            get
            {
                return (String)this["icaEncryptionLevel"];
            }
            set
            {
                this["icaEncryptionLevel"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = DEFAULT_PORT)]
        public Int32 Port
        {
            get
            {
                return (Int32)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("desktopShare")]
        public String DesktopShare
        {
            get
            {
                return (String)this["desktopShare"];
            }
            set
            {
                this["desktopShare"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnect")]
        public Boolean ExecuteBeforeConnect
        {
            get
            {
                return (Boolean)this["executeBeforeConnect"];
            }
            set
            {
                this["executeBeforeConnect"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectCommand")]
        public String ExecuteBeforeConnectCommand
        {
            get
            {
                return (String)this["executeBeforeConnectCommand"];
            }
            set
            {
                this["executeBeforeConnectCommand"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectArgs")]
        public String ExecuteBeforeConnectArgs
        {
            get
            {
                return (String)this["executeBeforeConnectArgs"];
            }
            set
            {
                this["executeBeforeConnectArgs"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectInitialDirectory")]
        public String ExecuteBeforeConnectInitialDirectory
        {
            get
            {
                return (String)this["executeBeforeConnectInitialDirectory"];
            }
            set
            {
                this["executeBeforeConnectInitialDirectory"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectWaitForExit")]
        public Boolean ExecuteBeforeConnectWaitForExit
        {
            get
            {
                return (Boolean)this["executeBeforeConnectWaitForExit"];
            }
            set
            {
                this["executeBeforeConnectWaitForExit"] = value;
            }
        }

        [ConfigurationProperty("disableTheming")]
        public Boolean DisableTheming
        {
            get
            {
                return (Boolean)this["disableTheming"];
            }
            set
            {
                this["disableTheming"] = value;
            }
        }

        [ConfigurationProperty("disableMenuAnimations")]
        public Boolean DisableMenuAnimations
        {
            get
            {
                return (Boolean)this["disableMenuAnimations"];
            }
            set
            {
                this["disableMenuAnimations"] = value;
            }
        }

        [ConfigurationProperty("disableFullWindowDrag")]
        public Boolean DisableFullWindowDrag
        {
            get
            {
                return (Boolean)this["disableFullWindowDrag"];
            }
            set
            {
                this["disableFullWindowDrag"] = value;
            }
        }

        [ConfigurationProperty("disableCursorBlinking")]
        public Boolean DisableCursorBlinking
        {
            get
            {
                return (Boolean)this["disableCursorBlinking"];
            }
            set
            {
                this["disableCursorBlinking"] = value;
            }
        }

        [ConfigurationProperty("enableDesktopComposition")]
        public Boolean EnableDesktopComposition
        {
            get
            {
                return (Boolean)this["enableDesktopComposition"];
            }
            set
            {
                this["enableDesktopComposition"] = value;
            }
        }

        [ConfigurationProperty("enableFontSmoothing")]
        public Boolean EnableFontSmoothing
        {
            get
            {
                return (Boolean)this["enableFontSmoothing"];
            }
            set
            {
                this["enableFontSmoothing"] = value;
            }
        }

        [ConfigurationProperty("disableCursorShadow")]
        public Boolean DisableCursorShadow
        {
            get
            {
                return (Boolean)this["disableCursorShadow"];
            }
            set
            {
                this["disableCursorShadow"] = value;
            }
        }

        [ConfigurationProperty("disableWallPaper")]
        public Boolean DisableWallPaper
        {
            get
            {
                return (Boolean)this["disableWallPaper"];
            }
            set
            {
                this["disableWallPaper"] = value;
            }
        }

        [ConfigurationProperty("LoadBalanceInfo")]
        public string LoadBalanceInfo
        {
            get
            {
                return (string)this["LoadBalanceInfo"];
            }
            set
            {
                this["LoadBalanceInfo"] = value;
            }
        }

        /// <summary>
        /// Gets or sets comma delimeted names of groups
        /// </summary>
        [ConfigurationProperty("tags")]
        public String Tags
        {
            get
            {
                return (String)this["tags"];
            }
            set
            {
                this["tags"] = value;
            }
        }

        private const string DOMAIN_ELEMENT = "domainName";

        /// <summary>
        /// Gets the stored domain name without checking for credentials resolution.
        /// Use for direct upgrades.
        /// </summary>
        [ConfigurationProperty(DOMAIN_ELEMENT, IsRequired = false)]
        public String DomainName
        {
            get
            {
                return (String)this[DOMAIN_ELEMENT];
            }
            set
            {
                this[DOMAIN_ELEMENT] = value;
            }
        }

        private const string USERNAME_ELEMENT = "userName";

        /// <summary>
        /// Gets the stored user name without checking for credentials resolution.
        /// Use for direct upgrades.
        /// </summary>
        [ConfigurationProperty(USERNAME_ELEMENT, IsRequired = false)]
        public String UserName
        {
            get
            {
                return (String)this[USERNAME_ELEMENT];
            }
            set
            {
                this[USERNAME_ELEMENT] = value;
            }
        }
        #endregion
    }
}
