using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{


    public class GroupConfigurationElementCollection : ConfigurationElementCollection
    {
        public GroupConfigurationElementCollection()
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
            return new GroupConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new GroupConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((GroupConfigurationElement)element).Name;
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

        public GroupConfigurationElement this[int index]
        {
            get
            {
                return (GroupConfigurationElement)BaseGet(index);
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

        new public GroupConfigurationElement this[string Name]
        {
            get
            {
                return (GroupConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(GroupConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public GroupConfigurationElement ItemByName(string name)
        {
            return (GroupConfigurationElement)BaseGet(name);
        }

        public void Add(GroupConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(GroupConfigurationElement item)
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
