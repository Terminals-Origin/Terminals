using System.Linq;
using Microsoft.Win32;

namespace Terminals.Plugins.Putty
{
    internal class PuttyRegistry
    {
        private const string HIVE = @"Software\SimonTatham\PuTTY\Sessions";

        public string[] GetSessions()
        {
            using (var sessionsHive = Registry.CurrentUser.OpenSubKey(HIVE))
            {
                if (null != sessionsHive)
                {
                    return sessionsHive.GetSubKeyNames().OrderBy(s => s)
                        .ToArray();
                }
            }

            return new string[0];
        }


    }
}
