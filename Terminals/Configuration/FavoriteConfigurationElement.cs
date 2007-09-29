using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{

    public class FavoriteConfigurationElement : ConfigurationElement
    {
        public FavoriteConfigurationElement()
        {

        }

        public FavoriteConfigurationElement(string name)
        {
            Name = name;
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
        [ConfigurationProperty("vmrcreducedcolorsmode", IsRequired = true, DefaultValue = false)]
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
        [ConfigurationProperty("telnet", IsRequired = true, DefaultValue = true)]
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
        [ConfigurationProperty("telnetrows", IsRequired = true, DefaultValue = 33)]
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
        [ConfigurationProperty("telnetcols", IsRequired = true, DefaultValue = 110)]
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
        [ConfigurationProperty("vmrcadministratormode", IsRequired = true, DefaultValue = false)]
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
        [ConfigurationProperty("telnetfont", IsRequired = true, DefaultValue = "[Font: Name=Microsoft Sans Serif, Size=10]")]
        public string TelnetFont
        {
            get
            {
                return (string)this["telnetfont"];
            }
            set
            {
                this["telnetfont"] = value;
            }
        }
        [ConfigurationProperty("telnetbackcolor", IsRequired = true, DefaultValue = "Black")]
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

        [ConfigurationProperty("telnettextcolor", IsRequired = true, DefaultValue = "White")]
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
        [ConfigurationProperty("telnetcursorcolor", IsRequired = true, DefaultValue = "Green")]
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

        [ConfigurationProperty("domainName", IsRequired = true)]
        public string DomainName
        {
            get
            {
                return (string)this["domainName"];
            }
            set
            {
                this["domainName"] = value;
            }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                return (string)this["userName"];
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("encryptedPassword", IsRequired = true)]
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
                return Functions.DecryptPassword(EncryptedPassword);
            }
            set
            {
                EncryptedPassword = Functions.EncryptPassword(value);
            }
        }

        [ConfigurationProperty("connectToConsole", IsRequired = true)]
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

        [ConfigurationProperty("desktopSize", IsRequired = true, DefaultValue = DesktopSize.FitToWindow)]
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

        [ConfigurationProperty("colors", IsRequired = true, DefaultValue = Colors.Bits32)]
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

        [ConfigurationProperty("showDesktopBackground")]
        public bool ShowDesktopBackground
        {
            get
            {
                return (bool)this["showDesktopBackground"];
            }
            set
            {
                this["showDesktopBackground"] = value;
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
                this["tags"] = value;
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
                if (!ShowDesktopBackground)
                    result = result | 1;
                //TODO: Add more performance flags
                return result;
            }
        }
    }

}
