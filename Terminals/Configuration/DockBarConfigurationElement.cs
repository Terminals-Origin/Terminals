using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace Terminals
{

    public class DockBarConfigurationElement : ConfigurationElement
    {
        public DockBarConfigurationElement()
        {

        }

        public DockBarConfigurationElement(string name)
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


        [ConfigurationProperty("visible", IsRequired = true, DefaultValue=true)]
        public bool Visible
        {
            get
            {
                return (bool)this["visible"];
            }
            set
            {
                this["visible"] = value;
            }
        }

        [ConfigurationProperty("dockStyle", IsRequired = false, DefaultValue = "Top")]
        public string DockStyle
        {
            get
            {
                return (string)this["dockStyle"];
            }
            set
            {
                this["dockStyle"] = value;
            }
        }


        [ConfigurationProperty("toolbarTitle", IsRequired = false)]
        public string ToolbarTitle
        {
            get
            {
                return (string)this["toolbarTitle"];
            }
            set
            {
                this["toolbarTitle"] = value;
            }
        }

        [ConfigurationProperty("x", IsRequired = false)]
        public int X
        {
            get
            {
                return (int)this["x"];
            }
            set
            {
                this["x"] = value;
            }
        }
        [ConfigurationProperty("y", IsRequired = false)]
        public int Y
        {
            get
            {
                return (int)this["y"];
            }
            set
            {
                this["y"] = value;
            }
        }

    }
}