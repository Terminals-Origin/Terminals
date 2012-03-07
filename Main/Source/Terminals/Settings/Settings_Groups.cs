using System;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        [Obsolete("Use new persistance instead.")]
        public static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        [Obsolete("Use new persistance instead.")]
        public static void DeleteGroup(string name)
        {
            GetSection().Groups.Remove(name);
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Use new persistance instead.")]
        public static void AddGroup(GroupConfigurationElement group)
        {
            GetSection().Groups.Add(group);
            SaveImmediatelyIfRequested();
        }
    }
}
