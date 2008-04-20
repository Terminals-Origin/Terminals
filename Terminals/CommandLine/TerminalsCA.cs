using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.CommandLine
{
    
    public class TerminalsCA
    {
        [Argument(ArgumentType.AtMostOnce, HelpText = "URL to quick connect to.")]
        public string url;


        [Argument(ArgumentType.AtMostOnce, HelpText = "Favorite list to quick connect to.")]
        public string favs;

        [Argument(ArgumentType.AtMostOnce, HelpText = "Local path the the config file to use.  Defaults to the standard Terminals.config")]
        public string config;

        [Argument(ArgumentType.AtMostOnce, HelpText = "Enable Automatic Updates")]
        public string AutomaticallyUpdate;

        [Argument(ArgumentType.AtMostOnce, LongName="console", ShortName="c", HelpText = "Connect to the console")]
        public bool console;

        [Argument(ArgumentType.AtMostOnce, ShortName="v", HelpText = "Quick connect machine to match mstsc.exe's parameter")]
        public string machine;

        [Argument(ArgumentType.AtMostOnce, LongName = "fullscreen", ShortName = "f", HelpText = "Run terminals in Full Screen mode at startup")]
        public bool fullscreen;


    }
}
