using System;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        [Obsolete("Use new persistence instead.")]
        public static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        [Obsolete("Use new persistence instead.")]
        public static void DeleteGroup(string name)
        {
            GetSection().Groups.Remove(name);
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Use new persistence instead.")]
        public static void AddGroup(GroupConfigurationElement group)
        {
            GetSection().Groups.Add(group);
            SaveImmediatelyIfRequested();
        }
    }
}
