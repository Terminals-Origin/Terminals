using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        public static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        public static void DeleteGroup(string name)
        {
            GetSection().Groups.Remove(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddGroup(GroupConfigurationElement group)
        {
            GetSection().Groups.Add(group);
            SaveImmediatelyIfRequested();
        }
    }
}
