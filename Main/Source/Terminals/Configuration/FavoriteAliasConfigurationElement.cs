using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{

    public class FavoriteAliasConfigurationElement : ConfigurationElement
    {
        public FavoriteAliasConfigurationElement()
        {

        }

        public FavoriteAliasConfigurationElement(string name)
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
    }

}