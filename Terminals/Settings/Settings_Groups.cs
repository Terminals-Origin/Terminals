using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        public static GroupConfigurationElementCollection GetGroups()
        {
            _terminalsConfigurationSection = GetSection();
            if (_terminalsConfigurationSection == null)
                return null;
            return _terminalsConfigurationSection.Groups;
        }

        public static void DeleteGroup(string name)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).Groups.Remove(name);
            SaveImmediatelyIfRequested(configuration);
        }

        public static void AddGroup(GroupConfigurationElement group)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).Groups.Add(group);
            SaveImmediatelyIfRequested(configuration);
        }
    }
}
