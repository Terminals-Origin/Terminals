using System;
using System.Runtime.Serialization;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.CommandLine
{
    /// <summary>
    /// Container for parsed command line arguments for whole application
    /// </summary>
    [DataContract]
    public class CommandLineArgs
    {
        [DataMember]
        [Argument(ArgumentType.AtMostOnce, HelpText = "URL to quick connect to.")]
        public string url;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "favs", HelpText = "Favorite list to quick connect to.")]
        public string favs;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "config",
            HelpText = "Path to the config file to use. Defaults to the standard Terminals.config")]
        public string configFile;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "cred",
            HelpText = "Path to the credentials file to use. Defaults to the standard Credentials.xml")]
        public string credentialsFile;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "favsfile", ShortName = "ff",
            HelpText = "Path to the favorites and groups file to use. Defaults to the standard favorites.xml")]
        public string favoritesFile;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "AutomaticallyUpdate", ShortName = "au", HelpText = "Enable Automatic Updates")]
        public bool AutomaticallyUpdate;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName="console", ShortName="c", HelpText = "Connect to the console")]
        public bool console;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, ShortName="v", HelpText = "Quick connect machine to match mstsc.exe's parameter")]
        public string machine;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "fullscreen", ShortName = "f",
            HelpText = "Run terminals in Full Screen mode at startup")]
        public bool fullscreen;

        [DataMember]
        [Argument(ArgumentType.AtMostOnce, LongName = "reuse", ShortName = "r", 
            HelpText = "Enforces start application in single instance mode. If defined, overrides the application option")]
        public bool reuse;


        internal string[] Favorites
        {
            get
            {
                if (HasFavorites)
                {
                   if(favs.Contains(","))
                    return favs.Split(',');

                    return new string[] {favs};
                }

                return new string[0];
            }
        }

        private bool HasFavorites
        {
            get { return !String.IsNullOrEmpty(this.favs); }
        }

        internal bool HasUrlDefined
        {
            get { return !String.IsNullOrEmpty(this.url) && !String.IsNullOrEmpty(this.UrlServer); }
        }

        internal string UrlServer
        {
            get
            {
                String server;
                Int32 port;
                ProtocolHandler.Parse(this.url, out server, out port);
                return server;
            }
        }

        internal int UrlPort
        {
            get
            {
                if (HasUrlDefined)
                {
                    String server;
                    Int32 port;
                    ProtocolHandler.Parse(this.url, out server, out port);
                    return port;
                }
                return KnownConnectionConstants.HTTPPort;
            }
        }

        internal bool HasMachineDefined
        {
            get { return !String.IsNullOrEmpty(this.machine); }
        }

        internal string MachineName
        {
            get
            {
                if (HasMachineDefined)
                {
                    Int32 index = this.machine.IndexOf(":");
                    if (index > 0)
                        return this.machine.Substring(0, index);
                }

                return this.machine;
            }
        }

        internal int Port
        {
            get
            {
                if (HasMachineDefined)
                {
                    Int32 index = this.machine.IndexOf(":");
                    if (index > 0)
                    {
                        Int32 port;
                        String portText = this.machine.Substring(index + 1);
                        if (Int32.TryParse(portText, out port))
                            return port;
                    }
                }

                return KnownConnectionConstants.RDPPort;
            }
        }

        internal bool SingleInstance
        {
            get
            {
                return Settings.Instance.SingleInstance || this.reuse;
            }
        }
    }
}
