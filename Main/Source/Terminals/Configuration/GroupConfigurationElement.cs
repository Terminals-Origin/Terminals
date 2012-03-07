using System.Configuration;

namespace Terminals
{
    /// <summary>
    /// Obsolete since v2.1 replaced by IGroup. Groups are no longer stored in config file
    /// </summary>
    public class GroupConfigurationElement : ConfigurationElement
    {
        public GroupConfigurationElement()
        {

        }

        public GroupConfigurationElement(string name)
        {
            Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("favoriteAliases")]
        [ConfigurationCollection(typeof(FavoriteAliasConfigurationElementCollection))]
        public FavoriteAliasConfigurationElementCollection FavoriteAliases
        {
            get
            {
                return (FavoriteAliasConfigurationElementCollection)this["favoriteAliases"];
            }
            set
            {
                this["favoriteAliases"] = value;
            }
        }
    }
}