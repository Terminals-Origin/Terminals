using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{


    public class SpecialCommandConfigurationElementCollection : ConfigurationElementCollection
    {
        public SpecialCommandConfigurationElementCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SpecialCommandConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new SpecialCommandConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((SpecialCommandConfigurationElement)element).Name;
        }

        public new string AddElementName
        {
            get
            {
                return base.AddElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string ClearElementName
        {
            get
            {
                return base.ClearElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string RemoveElementName
        {
            get
            {
                return base.RemoveElementName;
            }
        }

        public new int Count
        {
            get
            {
                return base.Count;
            }
        }

        public SpecialCommandConfigurationElement this[int index]
        {
            get
            {
                return (SpecialCommandConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public SpecialCommandConfigurationElement this[string Name]
        {
            get
            {
                return (SpecialCommandConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(SpecialCommandConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public SpecialCommandConfigurationElement ItemByName(string name)
        {
            return (SpecialCommandConfigurationElement)BaseGet(name);
        }

        public void Add(SpecialCommandConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SpecialCommandConfigurationElement item)
        {
            if (BaseIndexOf(item) >= 0)
                BaseRemove(item.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

}
