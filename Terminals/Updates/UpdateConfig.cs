using System;
using Terminals.Configuration;

namespace Terminals.Updates
{
    /// <summary>
    /// Class containing methods to update config files after an update. 
    /// </summary>
    public static class UpdateConfig
    {
        public static void CheckConfigVersionUpdate()
        {
            // If the Terminals version is not in the config or the version number in the config
            // is lower then the current assembly version, check for config updates
            if (Settings.ConfigVersion == null || Settings.ConfigVersion < Program.Info.Version)
            {
                UpdateToVersion20();

                // After all updates change the config version to the current assembly version
                Settings.ConfigVersion = Program.Info.Version;
            }
        }

        public static void UpdateToVersion20()
        {
            if (Program.Info.Version == new Version(2, 0, 0, 0))
            {
                // Change the Terminals URL to the correct URL used for Terminals News as of version 2.0 RC1
                FavoriteConfigurationElement newsFavorite = Settings.GetOneFavorite(FavoritesFactory.TerminalsReleasesFavoriteName);
                if (newsFavorite != null)
                {
                    newsFavorite.Url = Program.Resources.GetString("TerminalsURL");
                    Settings.SaveDefaultFavorite(newsFavorite);
                }
            }
        }
    }
}
