using System;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.CommandLine
{
    /// <summary>
    /// Container for parsed command line arguments for whole application
    /// </summary>
    public class CommandLineArgs
    {
        [Argument(ArgumentType.AtMostOnce, HelpText = "URL to quick connect to.")]
        public string url;

        [Argument(ArgumentType.AtMostOnce, LongName = "favs", HelpText = "Favorite list to quick connect to.")]
        public string favs;

        [Argument(ArgumentType.AtMostOnce,
            HelpText = "Local path the the config file to use.  Defaults to the standard Terminals.config")]
        public string config;

        [Argument(ArgumentType.AtMostOnce, LongName = "AutomaticallyUpdate", ShortName = "au", HelpText = "Enable Automatic Updates")]
        public bool AutomaticallyUpdate;

        [Argument(ArgumentType.AtMostOnce, LongName="console", ShortName="c", HelpText = "Connect to the console")]
        public bool console;

        [Argument(ArgumentType.AtMostOnce, ShortName="v", HelpText = "Quick connect machine to match mstsc.exe's parameter")]
        public string machine;

        [Argument(ArgumentType.AtMostOnce, LongName = "fullscreen", ShortName = "f",
            HelpText = "Run terminals in Full Screen mode at startup")]
        public bool fullscreen;

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

        internal bool HasFavorites
        {
            get { return !String.IsNullOrEmpty(this.favs); }
        }

        internal bool HasUrlDefined
        {
            get { return !String.IsNullOrEmpty(this.url); }
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
                return ConnectionManager.HTTPPort;
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

                return ConnectionManager.RDPPort;
            }
        }

        internal bool SingleInstance
        {
            get
            {
                return Settings.SingleInstance || this.reuse;
            }
        }
    }
}
