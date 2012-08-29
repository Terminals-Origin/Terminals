using System;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        public static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        [Obsolete("Since version 2. only for updates. Use new persistence instead.")]
        internal static void ClearGroups()
        {
            GroupConfigurationElementCollection configGroups = GetGroups();
            configGroups.Clear();
        }
    }
}
