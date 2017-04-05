using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminals.Plugins.Putty
{
    // HKEY_CURRENT_USER\Software\SimonTatham\PuTTY\Sessions
    internal class PuttyRegistry
    {
        public string Hive { get; set; }

        public PuttyRegistry()
        {
            this.Hive = @"Software\SimonTatham\PuTTY\Sessions";
        }

        internal PuttyRegistry(string hive)
        {
            this.Hive = hive;
        }

        public string[] GetSessions()
        {
            var sessionList = default(string[]);
            using (var sessionsHive = Registry.CurrentUser.OpenSubKey(this.Hive))
            {
                if (null != sessionsHive)
                {
                    sessionList = sessionsHive.GetSubKeyNames();
                }
            }

            return sessionList;
        }
    }
}
