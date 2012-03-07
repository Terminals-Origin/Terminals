using System;
using System.Collections.Generic;

namespace Terminals.Configuration
{
    internal class ConfigurationChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Use Create methods instead
        /// </summary>
        private ConfigurationChangedEventArgs() {}

        internal List<FavoriteConfigurationElement> OldFavorites { get; private set; }
        internal List<FavoriteConfigurationElement> NewFavorites { get; private set; }

        internal List<String> OldTags { get; private set; }
        internal List<String> NewTags { get; private set; }

        internal static ConfigurationChangedEventArgs CreateFromSettings(
          TerminalsConfigurationSection oldSettings,
          TerminalsConfigurationSection newSettings)
        {
          var args = new ConfigurationChangedEventArgs();
          
          args.OldFavorites = oldSettings.Favorites.ToList();
          args.NewFavorites = newSettings.Favorites.ToList();
          args.OldTags = oldSettings.Tags.ToList();
          args.NewTags = newSettings.Tags.ToList();

          return args;
        }

        internal static ConfigurationChangedEventArgs CreateFromButtons()
        {
            var args = new ConfigurationChangedEventArgs();

            args.OldFavorites = new List<FavoriteConfigurationElement>();
            args.NewFavorites = new List<FavoriteConfigurationElement>();
            args.OldTags = new List<string>();
            args.NewTags = new List<string>();

            return args;
        }
    }
}
