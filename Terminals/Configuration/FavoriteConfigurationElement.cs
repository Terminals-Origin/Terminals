using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;

namespace Terminals
{
    ///If adding a value here, update Clone() and ExportImport.ExportImport.cs.
    [Serializable]
    public class FavoriteConfigurationElement : ConfigurationElement, ICloneable
    {
        public FavoriteConfigurationElement()
        {

        }
        [ConfigurationProperty("telnet", IsRequired = false, DefaultValue = true)]
        public bool Telnet
        {
            get
            {
                return (bool)this["telnet"];
            }
            set
            {
                this["telnet"] = value;
            }
        }
        [ConfigurationProperty("telnetrows", IsRequired = false, DefaultValue = 33)]
        public int TelnetRows
        {
            get
            {
                return (int)this["telnetrows"];
            }
            set
            {
                this["telnetrows"] = value;
            }
        }
        [ConfigurationProperty("telnetcols", IsRequired = false, DefaultValue = 110)]
        public int TelnetCols
        {
            get
            {
                return (int)this["telnetcols"];
            }
            set
            {
                this["telnetcols"] = value;
            }
        }

        public FavoriteConfigurationElement(string name)
        {
            Name = name;
        }
        [ConfigurationProperty("shutdownTimeout", IsRequired = false, DefaultValue = 10)]
        public int ShutdownTimeout
        {
            get
            {
                int val = (int)this["shutdownTimeout"];
                if (val > 600) val = 600;
                if (val < 10) val = 10;
                return val;
            }
            set
            {
                if (value > 600) value = 600;
                if (value < 10) value = 10;
                this["shutdownTimeout"] = value;
            }
        }
        [ConfigurationProperty("overallTimeout", IsRequired = false, DefaultValue = 600)]
        public int OverallTimeout
        {
            get
            {
                int val = (int)this["overallTimeout"];
                if (val > 600) val = 600;
                if (val < 10) val = 10;
                return val;
            }
            set
            {
                if (value > 600) value = 600;
                if (value < 0) value = 0;
                this["overallTimeout"] = value;
            }
        }
        [ConfigurationProperty("connectionTimeout", IsRequired = false, DefaultValue = 600)]
        public int ConnectionTimeout
        {
            get
            {
                int val = (int)this["connectionTimeout"];
                if (val > 600) val = 600;
                if (val < 10) val = 10;
                return val;
            }
            set
            {
                if (value > 600) value = 600;
                if (value < 0) value = 0;
                this["connectionTimeout"] = value;
            }
        }

        [ConfigurationProperty("idleTimeout", IsRequired = false, DefaultValue = 240)]
        public int IdleTimeout
        {
            get
            {
                int val = (int)this["idleTimeout"];
                if (val > 600) val = 600;
                if (val < 10) val = 10;
                return val;
            }
            set
            {
                if (value > 240) value = 240;
                if (value < 0) value = 0;
                this["idleTimeout"] = value;
            }
        }
        [ConfigurationProperty("securityWorkingFolder", IsRequired = false, DefaultValue = "")]
        public string SecurityWorkingFolder
        {
            get
            {
                return (string)this["securityWorkingFolder"];
            }
            set
            {
                this["securityWorkingFolder"] = value;
            }
        }
        [ConfigurationProperty("securityStartProgram", IsRequired = false, DefaultValue = "")]
        public string SecurityStartProgram
        {
            get
            {
                return (string)this["securityStartProgram"];
            }
            set
            {
                this["securityStartProgram"] = value;
            }
        }
        [ConfigurationProperty("credential", IsRequired = false, DefaultValue = "")]
        public string Credential
        {
            get
            {
                return (string)this["credential"];
            }
            set
            {
                this["credential"] = value;
            }
        }

        [ConfigurationProperty("securityFullScreen", IsRequired = false, DefaultValue = false)]
        public bool SecurityFullScreen
        {
            get
            {
                return (bool)this["securityFullScreen"];
            }
            set
            {
                this["securityFullScreen"] = value;
            }
        }
        [ConfigurationProperty("enableSecuritySettings", IsRequired = false, DefaultValue = false)]
        public bool EnableSecuritySettings
        {
            get
            {
                return (bool)this["enableSecuritySettings"];
            }
            set
            {
                this["enableSecuritySettings"] = value;
            }
        }
        [ConfigurationProperty("grabFocusOnConnect", IsRequired = false, DefaultValue = false)]
        public bool GrabFocusOnConnect
        {
            get
            {
                return (bool)this["grabFocusOnConnect"];
            }
            set
            {
                this["grabFocusOnConnect"] = value;
            }
        }

        [ConfigurationProperty("enableEncryption", IsRequired = false, DefaultValue = false)]
        public bool EnableEncryption
        {
            get
            {
                return (bool)this["enableEncryption"];
            }
            set
            {
                this["enableEncryption"] = value;
            }
        }

        [ConfigurationProperty("disableWindowsKey", IsRequired = false, DefaultValue = false)]
        public bool DisableWindowsKey
        {
            get
            {
                return (bool)this["disableWindowsKey"];
            }
            set
            {
                this["disableWindowsKey"] = value;
            }
        }

        [ConfigurationProperty("doubleClickDetect", IsRequired = false, DefaultValue = false)]
        public bool DoubleClickDetect
        {
            get
            {
                return (bool)this["doubleClickDetect"];
            }
            set
            {
                this["doubleClickDetect"] = value;
            }
        }

        [ConfigurationProperty("displayConnectionBar", IsRequired = false, DefaultValue = false)]
        public bool DisplayConnectionBar
        {
            get
            {
                return (bool)this["displayConnectionBar"];
            }
            set
            {
                this["displayConnectionBar"] = value;
            }
        }

        [ConfigurationProperty("disableControlAltDelete", IsRequired = false, DefaultValue = false)]
        public bool DisableControlAltDelete
        {
            get
            {
                return (bool)this["disableControlAltDelete"];
            }
            set
            {
                this["disableControlAltDelete"] = value;
            }
        }
        [ConfigurationProperty("acceleratorPassthrough", IsRequired = false, DefaultValue = false)]
        public bool AcceleratorPassthrough
        {
            get
            {
                return (bool)this["acceleratorPassthrough"];
            }
            set
            {
                this["acceleratorPassthrough"] = value;
            }
        }
        [ConfigurationProperty("enableCompression", IsRequired = false, DefaultValue = false)]
        public bool EnableCompression
        {
            get
            {
                return (bool)this["enableCompression"];
            }
            set
            {
                this["enableCompression"] = value;
            }
        }
        [ConfigurationProperty("bitmapPeristence", IsRequired = false, DefaultValue = false)]
        public bool BitmapPeristence
        {
            get
            {
                return (bool)this["bitmapPeristence"];
            }
            set
            {
                this["bitmapPeristence"] = value;
            }
        }
        [ConfigurationProperty("enableTLSAuthentication", IsRequired = false, DefaultValue = false)]
        public bool EnableTLSAuthentication
        {
            get
            {
                return (bool)this["enableTLSAuthentication"];
            }
            set
            {
                this["enableTLSAuthentication"] = value;
            }
        }
        [ConfigurationProperty("allowBackgroundInput", IsRequired = false, DefaultValue = false)]
        public bool AllowBackgroundInput
        {
            get
            {
                return (bool)this["allowBackgroundInput"];
            }
            set
            {
                this["allowBackgroundInput"] = value;
            }
        }

        [ConfigurationProperty("ICAApplicationName", IsRequired = false, DefaultValue = "")]
        public string ICAApplicationName
        {
            get
            {
                return (string)this["ICAApplicationName"];
            }
            set
            {
                this["ICAApplicationName"] = value;
            }
        }
        [ConfigurationProperty("ICAApplicationWorkingFolder", IsRequired = false, DefaultValue = "")]
        public string ICAApplicationWorkingFolder
        {
            get
            {
                return (string)this["ICAApplicationWorkingFolder"];
            }
            set
            {
                this["ICAApplicationWorkingFolder"] = value;
            }
        }
        [ConfigurationProperty("ICAApplicationPath", IsRequired = false, DefaultValue = "")]
        public string ICAApplicationPath
        {
            get
            {
                return (string)this["ICAApplicationPath"];
            }
            set
            {
                this["ICAApplicationPath"] = value;
            }
        }
        [ConfigurationProperty("vmrcreducedcolorsmode", IsRequired = false, DefaultValue = false)]
        public bool VMRCReducedColorsMode
        {
            get
            {
                return (bool)this["vmrcreducedcolorsmode"];
            }
            set
            {
                this["vmrcreducedcolorsmode"] = value;
            }
        }
        [ConfigurationProperty("vmrcadministratormode", IsRequired = false, DefaultValue = false)]
        public bool VMRCAdministratorMode
        {
            get
            {
                return (bool)this["vmrcadministratormode"];
            }
            set
            {
                this["vmrcadministratormode"] = value;
            }
        }
        [ConfigurationProperty("ssh1", IsRequired = false, DefaultValue = false)]
        public bool SSH1
        {
            get
            {
                return (bool)this["ssh1"];
            }
            set
            {
                this["ssh1"] = value;
            }
        }
        [ConfigurationProperty("consolerows", IsRequired = false, DefaultValue = 33)]
        public int ConsoleRows
        {
            get
            {
                return (int)this["consolerows"];
            }
            set
            {
                this["consolerows"] = value;
            }
        }
        [ConfigurationProperty("consolecols", IsRequired = false, DefaultValue = 110)]
        public int ConsoleCols
        {
            get
            {
                return (int)this["consolecols"];
            }
            set
            {
                this["consolecols"] = value;
            }
        }
        [ConfigurationProperty("consolefont", IsRequired = false)]
        public string ConsoleFont
        {
            get
            {
                string font = (string)this["consolefont"]; ;
                if (String.IsNullOrEmpty(font)) font = "[Font: Name=Courier New, Size=10, Units=3, GdiCharSet=0, GdiVerticalFont=False]";
                return font;
            }
            set
            {
                this["consolefont"] = value;
            }
        }
        [ConfigurationProperty("consolebackcolor", IsRequired = false, DefaultValue = "Black")]
        public string ConsoleBackColor
        {
            get
            {
                return (string)this["consolebackcolor"];
            }
            set
            {
                this["consolebackcolor"] = value;
            }
        }

        [ConfigurationProperty("consoletextcolor", IsRequired = false, DefaultValue = "White")]
        public string ConsoleTextColor
        {
            get
            {
                return (string)this["consoletextcolor"];
            }
            set
            {
                this["consoletextcolor"] = value;
            }
        }
        [ConfigurationProperty("consolecursorcolor", IsRequired = false, DefaultValue = "Green")]
        public string ConsoleCursorColor
        {
            get
            {
                return (string)this["consolecursorcolor"];
            }
            set
            {
                this["consolecursorcolor"] = value;
            }
        }
        [ConfigurationProperty("protocol", IsRequired = true, DefaultValue = "RDP")]
        public string Protocol
        {
            get
            {
                return (string)this["protocol"];
            }
            set
            {
                this["protocol"] = value;
            }
        }
        [ConfigurationProperty("toolBarIcon", IsRequired = false, DefaultValue = "")]
        public string ToolBarIcon
        {
            get
            {
                return (string)this["toolBarIcon"];
            }
            set
            {
                this["toolBarIcon"] = value;
            }
        }
        [ConfigurationProperty("telnetfont", IsRequired = false)]
        public string TelnetFont
        {
            get
            {
                string font = (string)this["telnetfont"]; ;
                if (String.IsNullOrEmpty(font)) font = "[Font: Name=Courier New, Size=10, Units=3, GdiCharSet=0, GdiVerticalFont=False]";
                return font;
            }
            set
            {
                this["telnetfont"] = value;
            }
        }
        [ConfigurationProperty("telnetbackcolor", IsRequired = false, DefaultValue = "Black")]
        public string TelnetBackColor
        {
            get
            {
                return (string)this["telnetbackcolor"];
            }
            set
            {
                this["telnetbackcolor"] = value;
            }
        }

        [ConfigurationProperty("telnettextcolor", IsRequired = false, DefaultValue = "White")]
        public string TelnetTextColor
        {
            get
            {
                return (string)this["telnettextcolor"];
            }
            set
            {
                this["telnettextcolor"] = value;
            }
        }
        [ConfigurationProperty("telnetcursorcolor", IsRequired = false, DefaultValue = "Green")]
        public string TelnetCursorColor
        {
            get
            {
                return (string)this["telnetcursorcolor"];
            }
            set
            {
                this["telnetcursorcolor"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("serverName", IsRequired = true)]
        public string ServerName
        {
            get
            {
                return (string)this["serverName"];
            }
            set
            {
                this["serverName"] = value;
            }
        }

        [ConfigurationProperty("domainName", IsRequired = false)]
        public string DomainName
        {
            get
            {
                Credentials.CredentialSet cred = Credentials.CredentialSet.CredentialByName(Credential);
                if (cred != null)
                    return cred.Domain;
                else
                    return (string)this["domainName"];
            }
            set
            {
                this["domainName"] = value;
            }
        }
        [ConfigurationProperty("authMethod", DefaultValue = SSHClient.AuthMethod.Password)]
        public SSHClient.AuthMethod AuthMethod
        {
            get
            {
                return (SSHClient.AuthMethod)this["authMethod"];
            }
            set
            {
                this["authMethod"] = value;
            }
        }

        [ConfigurationProperty("keyTag", DefaultValue = "")]
        public string KeyTag
        {
            get
            {
                return (string)this["keyTag"];
            }
            set
            {
                this["keyTag"] = value;
            }
        }

        [ConfigurationProperty("userName", IsRequired = false)]
        public string UserName
        {
            get
            {
                Credentials.CredentialSet cred = Credentials.CredentialSet.CredentialByName(Credential);
                if (cred != null)
                    return cred.Username;
                else
                    return (string)this["userName"];
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("encryptedPassword", IsRequired = false)]
        public string EncryptedPassword
        {
            get
            {
                return (string)this["encryptedPassword"];
            }
            set
            {
                this["encryptedPassword"] = value;
            }
        }

        public string Password
        {
            get
            {
                Credentials.CredentialSet cred = Credentials.CredentialSet.CredentialByName(Credential);
                if (cred != null)
                    return cred.Password;
                else
                    return Functions.DecryptPassword(EncryptedPassword);
            }
            set
            {
                EncryptedPassword = Functions.EncryptPassword(value);
            }
        }

        [ConfigurationProperty("vncAutoScale", IsRequired = false, DefaultValue = false)]
        public bool VncAutoScale
        {
            get
            {
                return (bool)this["vncAutoScale"];
            }
            set
            {
                this["vncAutoScale"] = value;
            }
        }

        [ConfigurationProperty("vncViewOnly", IsRequired = false, DefaultValue = false)]
        public bool VncViewOnly
        {
            get
            {
                return (bool)this["vncViewOnly"];
            }
            set
            {
                this["vncViewOnly"] = value;
            }
        }

        [ConfigurationProperty("vncDisplayNumber", IsRequired = false, DefaultValue = 0)]
        public int VncDisplayNumber
        {
            get
            {
                return (int)this["vncDisplayNumber"];
            }
            set
            {
                this["vncDisplayNumber"] = value;
            }
        }


        [ConfigurationProperty("connectToConsole", IsRequired = false)]
        public bool ConnectToConsole
        {
            get
            {
                return (bool)this["connectToConsole"];
            }
            set
            {
                this["connectToConsole"] = value;
            }
        }

        [ConfigurationProperty("desktopSizeHeight", IsRequired = false)]
        public int DesktopSizeHeight
        {
            get
            {
                return (int)this["desktopSizeHeight"];
            }
            set
            {
                this["desktopSizeHeight"] = value;
            }
        }

        [ConfigurationProperty("desktopSizeWidth", IsRequired = false)]
        public int DesktopSizeWidth
        {
            get
            {
                return (int)this["desktopSizeWidth"];
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
        public bool RedirectDrives
        {
            get
            {
                return (bool)this["redirectDrives"];
            }
            set
            {
                this["redirectDrives"] = value;
            }
        }

        [ConfigurationProperty("redirectPorts")]
        public bool RedirectPorts
        {
            get
            {
                return (bool)this["redirectPorts"];
            }
            set
            {
                this["redirectPorts"] = value;
            }
        }
        [ConfigurationProperty("newWindow")]
        public bool NewWindow
        {
            get
            {
                return (bool)this["newWindow"];
            }
            set
            {
                this["newWindow"] = value;
            }
        }

        [ConfigurationProperty("redirectPrinters")]
        public bool RedirectPrinters
        {
            get
            {
                return (bool)this["redirectPrinters"];
            }
            set
            {
                this["redirectPrinters"] = value;
            }
        }

        [ConfigurationProperty("redirectSmartCards")]
        public bool RedirectSmartCards
        {
            get
            {
                return (bool)this["redirectSmartCards"];
            }
            set
            {
                this["redirectSmartCards"] = value;
            }
        }

        [ConfigurationProperty("redirectClipboard", DefaultValue = true)]
        public bool RedirectClipboard
        {
            get
            {
                return (bool)this["redirectClipboard"];
            }
            set
            {
                this["redirectClipboard"] = value;
            }
        }

        [ConfigurationProperty("redirectDevices")]
        public bool RedirectDevices
        {
            get
            {
                return (bool)this["redirectDevices"];
            }
            set
            {
                this["redirectDevices"] = value;
            }
        }
        [ConfigurationProperty("url", DefaultValue = "http://www.codeplex.com/terminals")]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;

        }

        [ConfigurationProperty("notes")]
        public string Notes
        {
            get
            {

                return DecodeFrom64((string)this["notes"]);
            }
            set
            {
                this["notes"] = EncodeTo64(value);
            }
        }

        [ConfigurationProperty("icaServerINI")]
        public string IcaServerINI
        {
            get
            {
                return (string)this["icaServerINI"];
            }
            set
            {
                this["icaServerINI"] = value;
            }
        }
        [ConfigurationProperty("icaClientINI")]
        public string IcaClientINI
        {
            get
            {
                return (string)this["icaClientINI"];
            }
            set
            {
                this["icaClientINI"] = value;
            }
        }
        [ConfigurationProperty("icaEnableEncryption")]
        public bool IcaEnableEncryption
        {
            get
            {
                return (bool)this["icaEnableEncryption"];
            }
            set
            {
                this["icaEnableEncryption"] = value;
            }
        }
        [ConfigurationProperty("icaEncryptionLevel")]
        public string IcaEncryptionLevel
        {
            get
            {
                return (string)this["icaEncryptionLevel"];
            }
            set
            {
                this["icaEncryptionLevel"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = 3389)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("desktopShare")]
        public string DesktopShare
        {
            get
            {
                return (string)this["desktopShare"];
            }
            set
            {
                this["desktopShare"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnect")]
        public bool ExecuteBeforeConnect
        {
            get
            {
                return (bool)this["executeBeforeConnect"];
            }
            set
            {
                this["executeBeforeConnect"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectCommand")]
        public string ExecuteBeforeConnectCommand
        {
            get
            {
                return (string)this["executeBeforeConnectCommand"];
            }
            set
            {
                this["executeBeforeConnectCommand"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectArgs")]
        public string ExecuteBeforeConnectArgs
        {
            get
            {
                return (string)this["executeBeforeConnectArgs"];
            }
            set
            {
                this["executeBeforeConnectArgs"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectInitialDirectory")]
        public string ExecuteBeforeConnectInitialDirectory
        {
            get
            {
                return (string)this["executeBeforeConnectInitialDirectory"];
            }
            set
            {
                this["executeBeforeConnectInitialDirectory"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectWaitForExit")]
        public bool ExecuteBeforeConnectWaitForExit
        {
            get
            {
                return (bool)this["executeBeforeConnectWaitForExit"];
            }
            set
            {
                this["executeBeforeConnectWaitForExit"] = value;
            }
        }

        [ConfigurationProperty("disableTheming")]
        public bool DisableTheming
        {
            get
            {
                return (bool)this["disableTheming"];
            }
            set
            {
                this["disableTheming"] = value;
            }
        }

        [ConfigurationProperty("disableMenuAnimations")]
        public bool DisableMenuAnimations
        {
            get
            {
                return (bool)this["disableMenuAnimations"];
            }
            set
            {
                this["disableMenuAnimations"] = value;
            }
        }

        [ConfigurationProperty("disableFullWindowDrag")]
        public bool DisableFullWindowDrag
        {
            get
            {
                return (bool)this["disableFullWindowDrag"];
            }
            set
            {
                this["disableFullWindowDrag"] = value;
            }
        }

        [ConfigurationProperty("disableCursorBlinking")]
        public bool DisableCursorBlinking
        {
            get
            {
                return (bool)this["disableCursorBlinking"];
            }
            set
            {
                this["disableCursorBlinking"] = value;
            }
        }

        [ConfigurationProperty("enableDesktopComposition")]
        public bool EnableDesktopComposition
        {
            get
            {
                return (bool)this["enableDesktopComposition"];
            }
            set
            {
                this["enableDesktopComposition"] = value;
            }
        }

        [ConfigurationProperty("enableFontSmoothing")]
        public bool EnableFontSmoothing
        {
            get
            {
                return (bool)this["enableFontSmoothing"];
            }
            set
            {
                this["enableFontSmoothing"] = value;
            }
        }

        [ConfigurationProperty("disableCursorShadow")]
        public bool DisableCursorShadow
        {
            get
            {
                return (bool)this["disableCursorShadow"];
            }
            set
            {
                this["disableCursorShadow"] = value;
            }
        }

        [ConfigurationProperty("disableWallPaper")]
        public bool DisableWallPaper
        {
            get
            {
                return (bool)this["disableWallPaper"];
            }
            set
            {
                this["disableWallPaper"] = value;
            }
        }

        [ConfigurationProperty("tags")]
        public string Tags
        {
            get
            {
                return (string)this["tags"];
            }
            set
            {
                if (Settings.AutoCaseTags)
                {
                    this["tags"] = Settings.ToTitleCase(value);
                }
                else
                {
                    this["tags"] = value;
                }
            }
        }

        public List<string> TagList
        {
            get
            {
                List<string> tagList = new List<string>();
                string[] splittedTags = Tags.Split(',');
                if (!((splittedTags.Length == 1) && (String.IsNullOrEmpty(splittedTags[0]))))
                {
                    foreach (string tag in splittedTags)
                    {
                        tagList.Add(tag);
                    }
                }
                return tagList;
            }
        }

        public int PerformanceFlags
        {
            get
            {
                int result = 0;

                if (DisableCursorShadow) result += (int)Terminals.PerfomanceOptions.TS_PERF_DISABLE_CURSOR_SHADOW;
                if (DisableCursorBlinking) result += (int)Terminals.PerfomanceOptions.TS_PERF_DISABLE_CURSORSETTINGS;
                if (DisableFullWindowDrag) result += (int)Terminals.PerfomanceOptions.TS_PERF_DISABLE_FULLWINDOWDRAG;
                if (DisableMenuAnimations) result += (int)Terminals.PerfomanceOptions.TS_PERF_DISABLE_MENUANIMATIONS;
                if (DisableTheming) result += (int)Terminals.PerfomanceOptions.TS_PERF_DISABLE_THEMING;
                if (DisableWallPaper) result += (int)Terminals.PerfomanceOptions.TS_PERF_DISABLE_WALLPAPER;
                if (EnableDesktopComposition) result += (int)Terminals.PerfomanceOptions.TS_PERF_ENABLE_DESKTOP_COMPOSITION;
                if (EnableFontSmoothing) result += (int)Terminals.PerfomanceOptions.TS_PERF_ENABLE_FONT_SMOOTHING;


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
                                                       EncryptedPassword = this.EncryptedPassword,
                                                       ExecuteBeforeConnect = this.ExecuteBeforeConnect,
                                                       ExecuteBeforeConnectArgs = this.ExecuteBeforeConnectArgs,
                                                       ExecuteBeforeConnectCommand = this.ExecuteBeforeConnectCommand,
                                                       ExecuteBeforeConnectInitialDirectory =
                                                           this.ExecuteBeforeConnectInitialDirectory,
                                                       ExecuteBeforeConnectWaitForExit =
                                                           this.ExecuteBeforeConnectWaitForExit,
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
                                                       Password = this.Password,
                                                       Port = this.Port,
                                                       Protocol = this.Protocol,
                                                       RedirectClipboard = this.RedirectClipboard,
                                                       RedirectDevices = this.RedirectDevices,
                                                       RedirectDrives = this.RedirectDrives,
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
                                                       Url = this.Url,
                                                       UserName = this.UserName,
                                                       VMRCAdministratorMode = this.VMRCAdministratorMode,
                                                       VMRCReducedColorsMode = this.VMRCReducedColorsMode,
                                                       VncAutoScale = this.VncAutoScale,
                                                       VncDisplayNumber = this.VncDisplayNumber,
                                                       VncViewOnly = this.VncViewOnly
                                                   };
            return fav;

        }
        #endregion        
    }
}

