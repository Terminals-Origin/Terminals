using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Terminals
{
    public class MRUItemConfigurationElementCollection : ConfigurationElementCollection
    {
        public MRUItemConfigurationElementCollection() { }

        internal MRUItemConfigurationElementCollection(IEnumerable<string> values)
        {
            foreach (string newItem in values)
            {
                this.AddByName(newItem);
            }
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
            return new MRUItemConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new MRUItemConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MRUItemConfigurationElement)element).Name;
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

        public MRUItemConfigurationElement this[int index]
        {
            get
            {
                return (MRUItemConfigurationElement)BaseGet(index);
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

        new public MRUItemConfigurationElement this[string Name]
        {
            get
            {
                return (MRUItemConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(MRUItemConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public MRUItemConfigurationElement ItemByName(string name)
        {
            return (MRUItemConfigurationElement)BaseGet(name);
        }

        public void Add(MRUItemConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(MRUItemConfigurationElement item)
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

        internal List<string> ToList()
        {
            return this.Cast<MRUItemConfigurationElement>()
                .Select(configurationElement => configurationElement.Name)
                .ToList();
        }

        internal string[] ToSortedArray()
        {
            List<string> domainNames = ToList();
            domainNames.Sort();
            return domainNames.ToArray();
        }

        internal void AddByName(string name)
        {
            MRUItemConfigurationElement configurationElement = this.ItemByName(name);
            if (configurationElement == null)
            {
                this.Add(new MRUItemConfigurationElement(name));
            }
        }

        internal void DeleteByName(string name)
        {
            MRUItemConfigurationElement configurationElement = this.ItemByName(name);
            if (configurationElement != null)
            {
                this.Remove(name);
            }
        }

        internal void EditByName(string oldName, string newName)
        {
            MRUItemConfigurationElement configurationElement = this.ItemByName(oldName);
            if (configurationElement != null)
            {
                this[oldName].Name = newName;
            }
        }
    }
}
