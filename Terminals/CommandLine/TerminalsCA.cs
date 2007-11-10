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


    }
}
