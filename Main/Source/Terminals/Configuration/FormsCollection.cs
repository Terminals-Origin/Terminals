using System;
using System.Configuration;

namespace Terminals
{
    /// <summary>
    /// Collection of Windows Form states
    /// </summary>
    [ConfigurationCollection(typeof(FormStateConfigElement), CollectionType=ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class FormsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FormStateConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FormStateConfigElement)element).Name;
        }

        public FormStateConfigElement this[int index]
        {
            get
            {
                return (FormStateConfigElement)this.BaseGet(index);
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

        new public FormStateConfigElement this[string name]
        {
            get
            {
                return (FormStateConfigElement)this.BaseGet(name);
            }
        }

        public void Add(FormStateConfigElement formState)
        {
            this.BaseAdd(formState, false);
        }
    }
}