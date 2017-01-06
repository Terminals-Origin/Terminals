using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace SSHClient
{


	[ConfigurationCollection(typeof(SSHKeyCollection),
    	CollectionType=ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class SSHKeyCollection : ConfigurationElementCollection
	{
        public SSHKeyElement this[int index]
        {
            get { return (SSHKeyElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public SSHKeyElement this[string tag]
        {
            get { return (SSHKeyElement)BaseGet(tag); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SSHKeyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
        	return (element as SSHKeyElement).tag;
        }

        public void Add(SSHKeyElement key)
        {
            BaseAdd(key);
        }

	}

	public class SSHKeyElement : ConfigurationElement
	{
        public SSHKeyElement() { }

        public SSHKeyElement(string tag, SSH2UserAuthKey key)
        {
             this.tag = tag;
             this.key = key;
        }

        [ConfigurationProperty("tag", IsRequired = true)]
        public string tag
        {
            get { return (string)this["tag"]; }
            set { this["tag"] = value; }
        }

	    [ConfigurationProperty("key", IsRequired = true)]
        [TypeConverter(typeof(SSH2UserAuthKeyConverter))]
        public SSH2UserAuthKey key
        {
            get { return (SSH2UserAuthKey)this["key"]; }
            set { this["key"] = value; }
        }
	}
}