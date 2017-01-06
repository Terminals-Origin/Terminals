using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal partial class Settings
    {
        internal GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        /// <summary>
        /// "Since version 2. only for updates. Use new persistence instead."
        /// </summary>
        internal void ClearGroups()
        {
            GroupConfigurationElementCollection configGroups = GetGroups();
            configGroups.Clear();
        }
    }
}
