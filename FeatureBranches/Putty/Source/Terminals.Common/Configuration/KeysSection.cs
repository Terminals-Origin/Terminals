using System;
using System.Configuration;

namespace Terminals.Common.Configuration
{
	/// <summary>
	/// SSHKeysSection is a special configuration section for SSH keys.
	/// </summary>
	public class KeysSection : ConfigurationSection
	{
        [ConfigurationProperty("name", 
            DefaultValue = "SSH",
            IsRequired = true, 
            IsKey = false)]
		public string Name
        {
            get{return (string)this["name"];}
            set{this["name"] = value;}
        }

		// Declare a collection element represented
        // in the configuration file by the sub-section
        // <keys> <add .../> </keys> 
        // Note: the "IsDefaultCollection = false" 
        // instructs the .NET Framework to build a nested 
        // section like <keys> ...</keys>.
        [ConfigurationProperty("keys",IsDefaultCollection = false)]
        public KeysCollection Keys
        {
            get
            {
                return (KeysCollection)base["keys"];
            }
            set
            {
            	this["keys"] = value;
            }
        }

        public void AddKey(string name, string key)
        {
        	this.Keys.Add(new KeyConfigElement(name, key));
        }
 
        public string Key(string name)
        {
        	return this.Keys[name].Key;
        }
	}	
	[ConfigurationCollection(typeof(KeyConfigElement),
    	CollectionType=ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class KeysCollection : ConfigurationElementCollection
    {
        public KeysCollection()
        {
        }
        
		protected override ConfigurationElement CreateNewElement()
		{
			return new KeyConfigElement();
		}

        protected override Object 
            GetElementKey(ConfigurationElement element)
        {
            return (element as KeyConfigElement).Name;
        }

        public KeyConfigElement this[int index]
        {
            get
            {
                return (KeyConfigElement)this.BaseGet(index);
            }
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        new public KeyConfigElement this[string Name]
        {
            get
            {
                return (KeyConfigElement)this.BaseGet(Name);
            }
        }

        public void Add(KeyConfigElement key)
        {
            this.BaseAdd(key, false);
            // Add custom code here.
        }
    }

    public class KeyConfigElement : ConfigurationElement
    {
        // Constructor allowing name, and key to be specified.
        public KeyConfigElement(String newName, String newKey)
        {
            this.Name = newName;
            this.Key = newKey;
        }

        // Default constructor, will use default values as defined
        // below.
        public KeyConfigElement()
        {
        }

        // Constructor allowing name to be specified, will take the
        // default values for url and port.
        public KeyConfigElement(string elementName)
        {
            this.Name = elementName;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get{return (string)this["name"];}
            set{this["name"] = value;}
        }

        [ConfigurationProperty("key",IsRequired = true)]
        public string Key
        {
            get{return (string)this["key"];}
            set{this["key"] = value;}
        }

        protected override void DeserializeElement(
           System.Xml.XmlReader reader, 
            bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, 
                serializeCollectionKey);
            // You can your custom processing code here.
        }

        protected override bool SerializeElement(
            System.Xml.XmlWriter writer, 
            bool serializeCollectionKey)
        {
            bool ret = base.SerializeElement(writer, 
                serializeCollectionKey);
            // You can enter your custom processing code here.
            return ret;
        }
    }
}