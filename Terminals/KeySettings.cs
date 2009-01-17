using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace Terminals
{
    public class SSH2UserAuthKeyConverter : TypeConverter
    {
        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context,
           Type sourceType)
        {

            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        // Overrides the CanConvertTo method of TypeConverter.
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {

            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context,
           CultureInfo culture, object value)
        {
            if (value is string)
            {
                Byte[] mk = new Byte[2000];
                MemoryStream ks = new MemoryStream(mk);
                StreamWriter sw = new StreamWriter(ks);
                sw.WriteLine(value);
                ks = new MemoryStream(mk);
                Routrek.SSHCV2.SSH2UserAuthKey k = SSH2UserAuthKey.FromSECSHStyleStream(ks, "");
                return k as SSH2UserAuthKey;
            }
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Byte[] mk = new Byte[2000];
                MemoryStream ks = new MemoryStream(mk);
                ((SSH2UserAuthKey)value).WritePrivatePartInSECSHStyleFile(ks, "", "");
                ks = new MemoryStream(mk);
                StreamReader sr = new StreamReader(ks);
                return sr.ReadToEnd();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(SSH2UserAuthKeyConverter))]
    public class SSH2UserAuthKey : Routrek.SSHCV2.SSH2UserAuthKey
    {
        public SSH2UserAuthKey(Routrek.PKI.KeyPair kp) : base(kp)
        {
        }
        public string PublicPartInOpenSSHStyle()
        {
            Byte[] mk = new Byte[2048];
            MemoryStream ks = new MemoryStream(mk);
            base.WritePublicPartInOpenSSHStyle(ks);
            ks = new MemoryStream(mk);
            StreamReader sr = new StreamReader(ks);
            return sr.ReadLine();
        }
    }

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