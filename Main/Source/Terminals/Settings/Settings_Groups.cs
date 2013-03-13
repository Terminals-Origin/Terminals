using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        internal static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        /// <summary>
        /// "Since version 2. only for updates. Use new persistence instead."
        /// </summary>
        internal static void ClearGroups()
        {
            GroupConfigurationElementCollection configGroups = GetGroups();
            configGroups.Clear();
        }
    }
}
